using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Encrypt.App.Algorithms;

namespace Encrypt.App.Views;

public partial class MainWindow : Window
{
    private readonly ICipher _cipher;

    public MainWindow()
    {
        InitializeComponent();

        // Burada hangi algoritma kullanılacaksa onu veriyorsun (UI'da seçim yok)
        _cipher = new CaesarCipher();
        AlgoTitle.Text = $"Şifreleme Algoritması: {_cipher.Name}";

        EncryptBtn.Click += DoEncrypt;
        ClearBtn.Click += ClearAll;
        CopyBtn.Click += CopyResult;
    }

    private void DoEncrypt(object? sender, RoutedEventArgs e)
    {
        try
        {
            var plain = PlainTextBox.Text ?? "";
            var key = KeyBox.Text ?? "";

            CipherTextBox.Text = _cipher.Encrypt(plain, key);
            StatusText.Text = "Şifreleme tamamlandı.";
        }
        catch (Exception ex)
        {
            CipherTextBox.Text = "";
            StatusText.Text = ex.Message;
        }
    }

    private void ClearAll(object? sender, RoutedEventArgs e)
    {
        PlainTextBox.Text = "";
        KeyBox.Text = "";
        CipherTextBox.Text = "";
        StatusText.Text = "";
    }

    private async void CopyResult(object? sender, RoutedEventArgs e)
    {
        var result = CipherTextBox.Text ?? "";
        if (string.IsNullOrWhiteSpace(result))
        {
            StatusText.Text = "Kopyalanacak sonuç yok.";
            return;
        }

        if (Clipboard is not null)
        {
            await Clipboard.SetTextAsync(result);
            StatusText.Text = "Sonuç kopyalandı.";
        }
        else
        {
            StatusText.Text = "Clipboard erişilemedi.";
        }
    }
}