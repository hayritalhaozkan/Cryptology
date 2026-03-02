using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Decrypt.App.Algorithms;

namespace Decrypt.App.Views;

public partial class MainWindow : Window
{
    // algoritma isimleri listesi
    private readonly string[] algoritmaIsimleri = new string[]
    {
        "Kaydirmali",
        "Dogrusal",
        "Yer Degistirme",
        "Sayi Anahtarli",
        "Permutasyon",
        "Rota",
        "Zigzag",
    };

    // her algoritma icin anahtar ipucu
    private readonly string[] anahtarIpuclari = new string[]
    {
        "Kaydirma sayisi girin (orn: 3)",
        "a ve b degerlerini girin. Orn: a=2, b=5",
        "29 harflik karisik alfabe girin.\nOrn: ÜYZABCÇDEFGĞHIIJKLMNOÖPRSŞTÜV",
        "Virgul ile ayrilmis sayilar girin.\nOrn: 3,7,1,15,22",
        "Permutasyon sirasi girin.\nOrn: 3,1,4,2 (blok=4)",
        "Satir ve sutun sayisi girin.\nOrn: 4 ve 5",
        "Ray sayisi girin. Orn: 3",
    };

    // her algoritma icin anahtar alan isimleri
    private readonly string[][] anahtarAlanlari = new string[][]
    {
        new[] { "Kaydirma (k)" },           // caesar
        new[] { "a", "b" },                 // affine
        new[] { "Anahtar Alfabesi (29 harf)" }, // substitution
        new[] { "Sayisal Anahtar" },        // vigenere
        new[] { "Permutasyon (virgul ile)" }, // permutation
        new[] { "Satir", "Sutun" },         // route
        new[] { "Ray Sayisi" },             // zigzag
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
        DecryptBtn.Click += CozButonu;
        CopyBtn.Click += KopyalaButonu;
        ClearBtn.Click += TemizleButonu;


        AnahtarAlanlariniOlustur(0);
    }

    private void AlgoritmaSecildi(object? sender, SelectionChangedEventArgs e)
    {
        int idx = MethodCombo.SelectedIndex;
        if (idx < 0 || idx >= algoritmaIsimleri.Length) return;

        seciliAlgoritma = idx;
        AnahtarAlanlariniOlustur(idx);
    }

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

    private string[] AnahtarlariTopla()
    {
        string[] anahtarlar = new string[anahtarKutulari.Count];
        for (int i = 0; i < anahtarKutulari.Count; i++)
            anahtarlar[i] = anahtarKutulari[i].Text ?? "";
        return anahtarlar;
    }

    // coz butonuna basildiginda
    private void CozButonu(object? sender, RoutedEventArgs e)
    {
        try
        {
            string sifreliMetin = CipherTextBox.Text ?? "";
            string[] anahtarlar = AnahtarlariTopla();
            string cozulmusMetin = "";

            // hangi algoritma seciliyse onu calistir
            if (seciliAlgoritma == 0) // Caesar
            {
                int kaydirma = int.Parse(anahtarlar[0]);
                cozulmusMetin = CaesarCoz.Coz(sifreliMetin, kaydirma);
            }
            else if (seciliAlgoritma == 1) // Affine
            {
                int a = int.Parse(anahtarlar[0]);
                int b = int.Parse(anahtarlar[1]);
                cozulmusMetin = AffineCoz.Coz(sifreliMetin, a, b);
            }
            else if (seciliAlgoritma == 2) // Substitution
            {
                cozulmusMetin = SubstitutionCoz.Coz(sifreliMetin, anahtarlar[0]);
            }
            else if (seciliAlgoritma == 3) // Vigenere
            {
                cozulmusMetin = VigenereCoz.Coz(sifreliMetin, anahtarlar[0]);
            }
            else if (seciliAlgoritma == 4) // Permutasyon
            {
                cozulmusMetin = PermutasyonCoz.Coz(sifreliMetin, anahtarlar[0]);
            }
            else if (seciliAlgoritma == 5) // Rota
            {
                int satir = int.Parse(anahtarlar[0]);
                int sutun = int.Parse(anahtarlar[1]);
                cozulmusMetin = RotaCoz.Coz(sifreliMetin, satir, sutun);
            }
            else if (seciliAlgoritma == 6) // Zigzag
            {
                int raySayisi = int.Parse(anahtarlar[0]);
                cozulmusMetin = ZigzagCoz.Coz(sifreliMetin, raySayisi);
            }

            PlainTextBox.Text = cozulmusMetin;
            StatusText.Text = $"✅ {algoritmaIsimleri[seciliAlgoritma]} ile çözme tamamlandı.";
            StatusText.Foreground = Avalonia.Media.Brushes.ForestGreen;
        }
        catch (Exception ex)
        {
            PlainTextBox.Text = "";
            StatusText.Text = $"❌ Hata: {ex.Message}";
            StatusText.Foreground = Avalonia.Media.Brushes.Red;
        }
    }

    private async void KopyalaButonu(object? sender, RoutedEventArgs e)
    {
        var sonuc = PlainTextBox.Text ?? "";
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
        CipherTextBox.Text = "";
        PlainTextBox.Text = "";
        StatusText.Text = "";
        for (int i = 0; i < anahtarKutulari.Count; i++)
            anahtarKutulari[i].Text = "";
    }


}