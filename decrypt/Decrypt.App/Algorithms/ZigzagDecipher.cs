using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

// zigzag sifre cozme (rail fence decipher)
// sifreli metin raylara dagilir, sonra zigzag sirasinda okunur
public sealed class ZigzagDecipher : IDecipher
{
    public string Name => "Zigzag (Rail Fence)";
    public string KeyHint => "Ray sayisi girin. Orn: 3";
    public string[] KeyLabels => new[] { "Ray Sayisi" };

    public string Decrypt(string sifreliMetin, string[] anahtarlar)
    {
        int raySayisi = int.Parse(anahtarlar[0]);

        string normalMetin = TextNormalizer.Normalize(sifreliMetin);
        int uzunluk = normalMetin.Length;

        if (uzunluk == 0)
            return "";

        if (raySayisi == 1 || raySayisi >= uzunluk)
            return normalMetin;

        // her raydeki harf sayisini hesapla
        int[] rayUzunluklari = new int[raySayisi];
        int ray = 0;
        bool asagi = true;
        for (int i = 0; i < uzunluk; i++)
        {
            rayUzunluklari[ray]++;
            if (ray == 0) asagi = true;
            else if (ray == raySayisi - 1) asagi = false;
            if (asagi) ray++;
            else ray--;
        }

        // sifreli metni raylara dagit
        string[] satirlar = new string[raySayisi];
        int pozisyon = 0;
        for (int r = 0; r < raySayisi; r++)
        {
            satirlar[r] = normalMetin.Substring(pozisyon, rayUzunluklari[r]);
            pozisyon += rayUzunluklari[r];
        }

        // zigzag sirasinda okuyarak orijinal metni olustur
        int[] rayIndexleri = new int[raySayisi];
        var sonuc = new StringBuilder();

        ray = 0;
        asagi = true;
        for (int i = 0; i < uzunluk; i++)
        {
            sonuc.Append(satirlar[ray][rayIndexleri[ray]]);
            rayIndexleri[ray]++;

            if (ray == 0) asagi = true;
            else if (ray == raySayisi - 1) asagi = false;
            if (asagi) ray++;
            else ray--;
        }

        return sonuc.ToString();
    }
}
