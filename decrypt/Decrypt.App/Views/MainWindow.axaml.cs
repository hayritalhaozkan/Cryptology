using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Decrypt.App.Algorithms;

namespace Decrypt.App.Views;

public partial class MainWindow : Window
{
    private readonly IDecipher _decipher;

    public MainWindow()
    {
        InitializeComponent();

        _decipher = new CaesarDecipher();
        AlgoTitle.Text = $"Çözme Algoritması: {_decipher.Name}";

        DecryptBtn.Click += DoDecrypt;
        ClearBtn.Click += ClearAll;
        CopyBtn.Click += CopyResult;
    }

    private void DoDecrypt(object? sender, RoutedEventArgs e)
    {
        try
        {
            var cipher = CipherTextBox.Text ?? "";
            var key = KeyBox.Text ?? "";

            PlainTextBox.Text = _decipher.Decrypt(cipher, key);
            StatusText.Text = "Çözme tamamlandı.";
        }
        catch (Exception ex)
        {
            PlainTextBox.Text = "";
            StatusText.Text = ex.Message;
        }
    }

    private void ClearAll(object? sender, RoutedEventArgs e)
    {
        CipherTextBox.Text = "";
        KeyBox.Text = "";
        PlainTextBox.Text = "";
        StatusText.Text = "";
    }

    private async void CopyResult(object? sender, RoutedEventArgs e)
    {
        var result = PlainTextBox.Text ?? "";
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