using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Decrypt.App.Algorithms;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;

namespace Decrypt.App.Views;

public partial class MainWindow : Window
{
    private readonly List<IDecipher> _deciphers = new()
    {
        new CaesarDecipher(),
        new AffineDecipher(),
        new SubstitutionDecipher(),
        new VigenereDecipher(),
        new PermutationDecipher(),
        new RouteDecipher(),
        new ZigzagDecipher(),
    };

    private IDecipher _selectedDecipher;
    private readonly List<TextBox> _keyTextBoxes = new();

    public MainWindow()
    {
        InitializeComponent();

        foreach (var d in _deciphers)
            MethodCombo.Items.Add(d.Name);

        MethodCombo.SelectedIndex = 0;
        _selectedDecipher = _deciphers[0];

        MethodCombo.SelectionChanged += OnMethodChanged;
        DecryptBtn.Click += DoDecrypt;
        CopyBtn.Click += CopyResult;
        ClearBtn.Click += ClearAll;
        FetchMailBtn.Click += FetchMail;

        BuildKeyFields(_selectedDecipher);
    }

    private void OnMethodChanged(object? sender, SelectionChangedEventArgs e)
    {
        int idx = MethodCombo.SelectedIndex;
        if (idx < 0 || idx >= _deciphers.Count) return;

        _selectedDecipher = _deciphers[idx];
        BuildKeyFields(_selectedDecipher);
    }

    private void BuildKeyFields(IDecipher decipher)
    {
        KeyFieldsPanel.Children.Clear();
        _keyTextBoxes.Clear();

        HintText.Text = decipher.KeyHint;

        foreach (var label in decipher.KeyLabels)
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

    private string[] CollectKeys()
    {
        var keys = new string[_keyTextBoxes.Count];
        for (int i = 0; i < _keyTextBoxes.Count; i++)
            keys[i] = _keyTextBoxes[i].Text ?? "";
        return keys;
    }

    private void DoDecrypt(object? sender, RoutedEventArgs e)
    {
        try
        {
            var cipher = CipherTextBox.Text ?? "";
            var keys = CollectKeys();

            PlainTextBox.Text = _selectedDecipher.Decrypt(cipher, keys);
            StatusText.Text = $"✅ {_selectedDecipher.Name} ile çözme tamamlandı.";
            StatusText.Foreground = Avalonia.Media.Brushes.ForestGreen;
        }
        catch (Exception ex)
        {
            PlainTextBox.Text = "";
            StatusText.Text = $"❌ Hata: {ex.Message}";
            StatusText.Foreground = Avalonia.Media.Brushes.Red;
        }
    }

    private async void CopyResult(object? sender, RoutedEventArgs e)
    {
        var result = PlainTextBox.Text ?? "";
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
        CipherTextBox.Text = "";
        PlainTextBox.Text = "";
        StatusText.Text = "";
        MailStatusText.Text = "";
        foreach (var tb in _keyTextBoxes)
            tb.Text = "";
    }

    /// <summary>
    /// IMAP ile son "CRYPT" konulu e-postanın içeriğini çeker
    /// ve şifreli metin alanına yapıştırır.
    /// </summary>
    private async void FetchMail(object? sender, RoutedEventArgs e)
    {
        var email = ImapEmailBox.Text ?? "";
        var password = ImapPasswordBox.Text ?? "";
        var host = ImapHostBox.Text ?? "imap.gmail.com";
        var portText = ImapPortBox.Text ?? "993";

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            MailStatusText.Text = "⚠ E-posta ve şifre gerekli.";
            MailStatusText.Foreground = Avalonia.Media.Brushes.Orange;
            return;
        }

        if (!int.TryParse(portText, out int port))
        {
            MailStatusText.Text = "⚠ IMAP portu geçersiz.";
            MailStatusText.Foreground = Avalonia.Media.Brushes.Orange;
            return;
        }

        MailStatusText.Text = "📥 Bağlanılıyor...";
        MailStatusText.Foreground = Avalonia.Media.Brushes.Gray;
        FetchMailBtn.IsEnabled = false;

        try
        {
            string? body = null;

            await Task.Run(async () =>
            {
                using var client = new ImapClient();
                await client.ConnectAsync(host, port,
                    MailKit.Security.SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(email, password);

                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                // "CRYPT" konulu son maili bul
                var query = SearchQuery.SubjectContains("CRYPT");
                var uids = await inbox.SearchAsync(query);

                if (uids.Count > 0)
                {
                    // En son maili al
                    var lastUid = uids[uids.Count - 1];
                    var message = await inbox.GetMessageAsync(lastUid);
                    body = message.TextBody;
                }

                await client.DisconnectAsync(true);
            });

            if (!string.IsNullOrWhiteSpace(body))
            {
                CipherTextBox.Text = body.Trim();
                MailStatusText.Text = "✅ Şifreli metin e-postadan alındı!";
                MailStatusText.Foreground = Avalonia.Media.Brushes.ForestGreen;
            }
            else
            {
                MailStatusText.Text = "⚠ 'CRYPT' konulu e-posta bulunamadı.";
                MailStatusText.Foreground = Avalonia.Media.Brushes.Orange;
            }
        }
        catch (Exception ex)
        {
            MailStatusText.Text = $"❌ Mail hatası: {ex.Message}";
            MailStatusText.Foreground = Avalonia.Media.Brushes.Red;
        }
        finally
        {
            FetchMailBtn.IsEnabled = true;
        }
    }
}