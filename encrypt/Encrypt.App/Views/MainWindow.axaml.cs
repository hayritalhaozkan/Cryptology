using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Encrypt.App.Algorithms;
using MailKit.Net.Smtp;
using MimeKit;

namespace Encrypt.App.Views;

public partial class MainWindow : Window
{
    // Tüm algoritmalar — sırayla ComboBox'a eklenir
    private readonly List<ICipher> _ciphers = new()
    {
        new CaesarCipher(),
        new AffineCipher(),
        new SubstitutionCipher(),
        new VigenereCipher(),
        new PermutationCipher(),
        new RouteCipher(),
        new ZigzagCipher(),
    };

    private ICipher _selectedCipher;

    // Dinamik anahtar TextBox'ları — yöntem değişince yeniden oluşturulur
    private readonly List<TextBox> _keyTextBoxes = new();

    public MainWindow()
    {
        InitializeComponent();

        // ComboBox'ı doldur
        foreach (var c in _ciphers)
            MethodCombo.Items.Add(c.Name);

        MethodCombo.SelectedIndex = 0;
        _selectedCipher = _ciphers[0];

        MethodCombo.SelectionChanged += OnMethodChanged;
        EncryptBtn.Click += DoEncrypt;
        CopyBtn.Click += CopyResult;
        ClearBtn.Click += ClearAll;
        SendMailBtn.Click += SendMail;

        BuildKeyFields(_selectedCipher);
    }

    /// <summary>Yöntem değiştiğinde çağrılır. Anahtar alanlarını yeniden oluşturur.</summary>
    private void OnMethodChanged(object? sender, SelectionChangedEventArgs e)
    {
        int idx = MethodCombo.SelectedIndex;
        if (idx < 0 || idx >= _ciphers.Count) return;

        _selectedCipher = _ciphers[idx];
        BuildKeyFields(_selectedCipher);
    }

    /// <summary>Seçilen algoritmaya göre dinamik anahtar alanlarını oluşturur.</summary>
    private void BuildKeyFields(ICipher cipher)
    {
        KeyFieldsPanel.Children.Clear();
        _keyTextBoxes.Clear();

        HintText.Text = cipher.KeyHint;

        foreach (var label in cipher.KeyLabels)
        {
            var lbl = new TextBlock
            {
                Text = label + ":",
                FontSize = 12,
                Margin = new Avalonia.Thickness(0, 2, 0, 0)
            };

            var tb = new TextBox
            {
                Watermark = label,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            KeyFieldsPanel.Children.Add(lbl);
            KeyFieldsPanel.Children.Add(tb);
            _keyTextBoxes.Add(tb);
        }
    }

    /// <summary>Anahtar alanlarından değerleri toplar.</summary>
    private string[] CollectKeys()
    {
        var keys = new string[_keyTextBoxes.Count];
        for (int i = 0; i < _keyTextBoxes.Count; i++)
            keys[i] = _keyTextBoxes[i].Text ?? "";
        return keys;
    }

    private void DoEncrypt(object? sender, RoutedEventArgs e)
    {
        try
        {
            var plain = PlainTextBox.Text ?? "";
            var keys = CollectKeys();

            CipherTextBox.Text = _selectedCipher.Encrypt(plain, keys);
            StatusText.Text = $"✅ {_selectedCipher.Name} ile şifreleme tamamlandı.";
            StatusText.Foreground = Avalonia.Media.Brushes.ForestGreen;
        }
        catch (Exception ex)
        {
            CipherTextBox.Text = "";
            StatusText.Text = $"❌ Hata: {ex.Message}";
            StatusText.Foreground = Avalonia.Media.Brushes.Red;
        }
    }

    private async void CopyResult(object? sender, RoutedEventArgs e)
    {
        var result = CipherTextBox.Text ?? "";
        if (string.IsNullOrWhiteSpace(result))
        {
            StatusText.Text = "⚠ Kopyalanacak sonuç yok.";
            StatusText.Foreground = Avalonia.Media.Brushes.Orange;
            return;
        }

        if (Clipboard is not null)
        {
            await Clipboard.SetTextAsync(result);
            StatusText.Text = "📋 Sonuç panoya kopyalandı.";
            StatusText.Foreground = Avalonia.Media.Brushes.ForestGreen;
        }
        else
        {
            StatusText.Text = "❌ Clipboard erişilemedi.";
            StatusText.Foreground = Avalonia.Media.Brushes.Red;
        }
    }

    private void ClearAll(object? sender, RoutedEventArgs e)
    {
        PlainTextBox.Text = "";
        CipherTextBox.Text = "";
        StatusText.Text = "";
        MailStatusText.Text = "";
        foreach (var tb in _keyTextBoxes)
            tb.Text = "";
    }

    /// <summary>Şifreli metni SMTP ile e-posta olarak gönderir.</summary>
    private async void SendMail(object? sender, RoutedEventArgs e)
    {
        var cipherText = CipherTextBox.Text ?? "";
        if (string.IsNullOrWhiteSpace(cipherText))
        {
            MailStatusText.Text = "⚠ Önce bir metin şifrelemelisiniz.";
            MailStatusText.Foreground = Avalonia.Media.Brushes.Orange;
            return;
        }

        var senderEmail = SenderEmailBox.Text ?? "";
        var senderPassword = SenderPasswordBox.Text ?? "";
        var recipientEmail = RecipientEmailBox.Text ?? "";
        var smtpHost = SmtpHostBox.Text ?? "smtp.gmail.com";
        var smtpPortText = SmtpPortBox.Text ?? "587";

        if (string.IsNullOrWhiteSpace(senderEmail) ||
            string.IsNullOrWhiteSpace(senderPassword) ||
            string.IsNullOrWhiteSpace(recipientEmail))
        {
            MailStatusText.Text = "⚠ Gönderen e-posta, şifre ve alıcı e-posta gerekli.";
            MailStatusText.Foreground = Avalonia.Media.Brushes.Orange;
            return;
        }

        if (!int.TryParse(smtpPortText, out int smtpPort))
        {
            MailStatusText.Text = "⚠ SMTP portu geçersiz.";
            MailStatusText.Foreground = Avalonia.Media.Brushes.Orange;
            return;
        }

        MailStatusText.Text = "📤 Gönderiliyor...";
        MailStatusText.Foreground = Avalonia.Media.Brushes.Gray;
        SendMailBtn.IsEnabled = false;

        try
        {
            await Task.Run(async () =>
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Encrypt App", senderEmail));
                message.To.Add(new MailboxAddress("Alıcı", recipientEmail));
                message.Subject = "CRYPT";
                message.Body = new TextPart("plain") { Text = cipherText };

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpHost, smtpPort,
                    MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            });

            MailStatusText.Text = "✅ E-posta başarıyla gönderildi!";
            MailStatusText.Foreground = Avalonia.Media.Brushes.ForestGreen;
        }
        catch (Exception ex)
        {
            MailStatusText.Text = $"❌ Mail hatası: {ex.Message}";
            MailStatusText.Foreground = Avalonia.Media.Brushes.Red;
        }
        finally
        {
            SendMailBtn.IsEnabled = true;
        }
    }
}