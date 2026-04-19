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
                cozulmusMetin = HillCoz.Coz(sifreliMetin, m1, m2, m3, m4, m5, m6, m7, m8, m9);
            }
            else if (seciliAlgoritma == 8) // Four-Square
            {
                cozulmusMetin = FourSquareCoz.Coz(sifreliMetin, anahtarlar[0], anahtarlar[1]);
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