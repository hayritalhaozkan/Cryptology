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
    private readonly string[] algoritmaIsimleri = new string[]
    {
        "Kaydirmali",
        "Dogrusal",
        "Yer Degistirme",
        "Vigenere",
        "Permutasyon",
        "Rota",
        "Zigzag",
        "Hill",
        "4 Kare (Matris)",
    };

    private readonly string[] anahtarIpuclari = new string[]
    {
        "Kaydirma sayisi girin (orn: 3)",
        "a ve b degerlerini girin. Orn: a=2, b=5",
        "29 harflik karisik alfabe girin.\nOrn: ÜYZABCÇDEFGĞHIIJKLMNOÖPRSŞTÜV",
        "Metin anahtar girin.\nOrn: kayali",
        "Permutasyon sirasi girin.\nOrn: 3,1,4,2 (blok=4)",
        "Satir ve sutun sayisi girin.\nOrn: 4 ve 5",
        "Ray sayisi girin. Orn: 3",
        "3x3 matris degerlerini girin (a, b, c, d, e, f, g, h, i).\nDeterminant 29 ile aralarinda asal olmali.",
        "Matris 2 ve 3 için 30 harflik karışık matris alfabesini girin.\n(A-Z + X harflerinden oluşan 30 karakter)",
    };

    private readonly string[][] anahtarAlanlari = new string[][]
    {
        new[] { "Kaydirma (k)" },           // caesar
        new[] { "a", "b" },                 // affine
        new[] { "Anahtar Alfabesi (29 harf)" }, // substitution
        new[] { "Anahtar Kelime" },        // vigenere
        new[] { "Permutasyon (virgul ile)" }, // permutation
        new[] { "Satir", "Sutun" },         // route
        new[] { "Ray Sayisi" },             // zigzag
        new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i" }, // hill
        new[] { "Matris 2 Alfabesi (30 harf)", "Matris 3 Alfabesi (30 harf)" }, // four-square
    };

    private int seciliAlgoritma = 0;  // hangi algoritma secili
    private readonly List<TextBox> anahtarKutulari = new();  // anahtar girisi kutulari

    public MainWindow()
    {
        InitializeComponent();

        // combobox'a algoritma isimlerini ekle
        for (int i = 0; i < algoritmaIsimleri.Length; i++)
            MethodCombo.Items.Add(algoritmaIsimleri[i]);

        MethodCombo.SelectedIndex = 0;
        seciliAlgoritma = 0;

        MethodCombo.SelectionChanged += AlgoritmaSecildi;
        EncryptBtn.Click += SifreleButonu;
        CopyBtn.Click += KopyalaButonu;
        ClearBtn.Click += TemizleButonu;
        SendMailBtn.Click += MailGonder;

        AnahtarAlanlariniOlustur(0);
    }

    // algoritma degistiginde anahtar alanlarini yeniden olustur
    private void AlgoritmaSecildi(object? sender, SelectionChangedEventArgs e)
    {
        int idx = MethodCombo.SelectedIndex;
        if (idx < 0 || idx >= algoritmaIsimleri.Length) return;

        seciliAlgoritma = idx;
        AnahtarAlanlariniOlustur(idx);
    }

    // secilen algoritmaya gore anahtar alanlari olustur
    private void AnahtarAlanlariniOlustur(int algoritmaIndex)
    {
        KeyFieldsPanel.Children.Clear();
        anahtarKutulari.Clear();

        HintText.Text = anahtarIpuclari[algoritmaIndex];

        string[] alanlar = anahtarAlanlari[algoritmaIndex];
        for (int i = 0; i < alanlar.Length; i++)
        {
            var etiket = new TextBlock
            {
                Text = alanlar[i] + ":",
                FontSize = 12,
                Margin = new Avalonia.Thickness(0, 2, 0, 0)
            };

            var kutu = new TextBox
            {
                Watermark = alanlar[i],
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            KeyFieldsPanel.Children.Add(etiket);
            KeyFieldsPanel.Children.Add(kutu);
            anahtarKutulari.Add(kutu);
        }
    }

    // anahtar kutularindaki degerleri topla
    private string[] AnahtarlariTopla()
    {
        string[] anahtarlar = new string[anahtarKutulari.Count];
        for (int i = 0; i < anahtarKutulari.Count; i++)
            anahtarlar[i] = anahtarKutulari[i].Text ?? "";
        return anahtarlar;
    }

    // sifrele butonuna basildiginda
    private void SifreleButonu(object? sender, RoutedEventArgs e)
    {
        try
        {
            string metin = PlainTextBox.Text ?? "";
            string[] anahtarlar = AnahtarlariTopla();
            string sifreliMetin = "";

            // hangi algoritma seciliyse onu calistir
            if (seciliAlgoritma == 0) // Caesar
            {
                int kaydirma = int.Parse(anahtarlar[0]);
                sifreliMetin = CaesarSifrele.Sifrele(metin, kaydirma);
            }
            else if (seciliAlgoritma == 1) // Affine
            {
                int a = int.Parse(anahtarlar[0]);
                int b = int.Parse(anahtarlar[1]);
                sifreliMetin = AffineSifrele.Sifrele(metin, a, b);
            }
            else if (seciliAlgoritma == 2) // Substitution
            {
                sifreliMetin = SubstitutionSifrele.Sifrele(metin, anahtarlar[0]);
            }
            else if (seciliAlgoritma == 3) // Vigenere
            {
                sifreliMetin = VigenereSifrele.Sifrele(metin, anahtarlar[0]);
            }
            else if (seciliAlgoritma == 4) // Permutasyon
            {
                sifreliMetin = PermutasyonSifrele.Sifrele(metin, anahtarlar[0]);
            }
            else if (seciliAlgoritma == 5) // Rota
            {
                int satir = int.Parse(anahtarlar[0]);
                int sutun = int.Parse(anahtarlar[1]);
                sifreliMetin = RotaSifrele.Sifrele(metin, satir, sutun);
            }
            else if (seciliAlgoritma == 6) // Zigzag
            {
                int raySayisi = int.Parse(anahtarlar[0]);
                sifreliMetin = ZigzagSifrele.Sifrele(metin, raySayisi);
            }
            else if (seciliAlgoritma == 7) // Hill
            {
                int m1 = int.Parse(anahtarlar[0]);
                int m2 = int.Parse(anahtarlar[1]);
                int m3 = int.Parse(anahtarlar[2]);
                int m4 = int.Parse(anahtarlar[3]);
                int m5 = int.Parse(anahtarlar[4]);
                int m6 = int.Parse(anahtarlar[5]);
                int m7 = int.Parse(anahtarlar[6]);
                int m8 = int.Parse(anahtarlar[7]);
                int m9 = int.Parse(anahtarlar[8]);
                sifreliMetin = HillSifrele.Sifrele(metin, m1, m2, m3, m4, m5, m6, m7, m8, m9);
            }
            else if (seciliAlgoritma == 8) // Four-Square
            {
                sifreliMetin = FourSquareSifrele.Sifrele(metin, anahtarlar[0], anahtarlar[1]);
            }

            CipherTextBox.Text = sifreliMetin;
            StatusText.Text = $"✅ {algoritmaIsimleri[seciliAlgoritma]} ile şifreleme tamamlandı.";
            StatusText.Foreground = Avalonia.Media.Brushes.ForestGreen;
        }
        catch (Exception ex)
        {
            CipherTextBox.Text = "";
            StatusText.Text = $"❌ Hata: {ex.Message}";
            StatusText.Foreground = Avalonia.Media.Brushes.Red;
        }
    }

    private async void KopyalaButonu(object? sender, RoutedEventArgs e)
    {
        var sonuc = CipherTextBox.Text ?? "";
        if (string.IsNullOrWhiteSpace(sonuc))
        {
            StatusText.Text = "⚠ Kopyalanacak sonuç yok.";
            StatusText.Foreground = Avalonia.Media.Brushes.Orange;
            return;
        }

        if (Clipboard is not null)
        {
            await Clipboard.SetTextAsync(sonuc);
            StatusText.Text = "📋 Sonuç panoya kopyalandı.";
            StatusText.Foreground = Avalonia.Media.Brushes.ForestGreen;
        }
        else
        {
            StatusText.Text = "❌ Clipboard erişilemedi.";
            StatusText.Foreground = Avalonia.Media.Brushes.Red;
        }
    }

    private void TemizleButonu(object? sender, RoutedEventArgs e)
    {
        PlainTextBox.Text = "";
        CipherTextBox.Text = "";
        StatusText.Text = "";
        MailStatusText.Text = "";
        for (int i = 0; i < anahtarKutulari.Count; i++)
            anahtarKutulari[i].Text = "";
    }

    private async void MailGonder(object? sender, RoutedEventArgs e)
    {
        var sifreliMetin = CipherTextBox.Text ?? "";
        if (string.IsNullOrWhiteSpace(sifreliMetin))
        {
            MailStatusText.Text = "⚠ Önce bir metin şifrelemelisiniz.";
            MailStatusText.Foreground = Avalonia.Media.Brushes.Orange;
            return;
        }

        var gonderenEposta = SenderEmailBox.Text ?? "";
        var gonderenSifre = SenderPasswordBox.Text ?? "";
        var aliciEposta = RecipientEmailBox.Text ?? "";
        var smtpSunucu = SmtpHostBox.Text ?? "smtp.gmail.com";
        var smtpPortMetin = SmtpPortBox.Text ?? "587";

        if (string.IsNullOrWhiteSpace(gonderenEposta) ||
            string.IsNullOrWhiteSpace(gonderenSifre) ||
            string.IsNullOrWhiteSpace(aliciEposta))
        {
            MailStatusText.Text = "⚠ Gönderen e-posta, şifre ve alıcı e-posta gerekli.";
            MailStatusText.Foreground = Avalonia.Media.Brushes.Orange;
            return;
        }

        if (!int.TryParse(smtpPortMetin, out int smtpPort))
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
                var mesaj = new MimeMessage();
                mesaj.From.Add(new MailboxAddress("Encrypt App", gonderenEposta));
                mesaj.To.Add(new MailboxAddress("Alıcı", aliciEposta));
                mesaj.Subject = "CRYPT";
                mesaj.Body = new TextPart("plain") { Text = sifreliMetin };

                using var istemci = new SmtpClient();
                await istemci.ConnectAsync(smtpSunucu, smtpPort,
                    MailKit.Security.SecureSocketOptions.StartTls);
                await istemci.AuthenticateAsync(gonderenEposta, gonderenSifre);
                await istemci.SendAsync(mesaj);
                await istemci.DisconnectAsync(true);
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