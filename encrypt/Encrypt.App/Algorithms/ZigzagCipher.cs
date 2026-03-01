using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

// zigzag sifresi (rail fence)
// metin zigzag seklinde raylara yazilir, sonra satir satir okunur
public sealed class ZigzagCipher : ICipher
{
    public string Name => "Zigzag (Rail Fence)";
    public string KeyHint => "Ray sayisi girin. Orn: 3";
    public string[] KeyLabels => new[] { "Ray Sayisi" };

    public string Encrypt(string duzMetin, string[] anahtarlar)
    {
        int raySayisi = int.Parse(anahtarlar[0]);

        string normalMetin = TextNormalizer.Normalize(duzMetin);

        if (normalMetin.Length == 0)
            return "";

        if (raySayisi == 1 || raySayisi >= normalMetin.Length)
            return normalMetin;

        // her ray icin bir StringBuilder olustur
        var satirlar = new StringBuilder[raySayisi];
        for (int i = 0; i < raySayisi; i++)
            satirlar[i] = new StringBuilder();

        int mevcutRay = 0;
        bool asagiMi = true;

        // her harfi ilgili raya yaz
        for (int i = 0; i < normalMetin.Length; i++)
        {
            satirlar[mevcutRay].Append(normalMetin[i]);

            if (mevcutRay == 0)
                asagiMi = true;
            else if (mevcutRay == raySayisi - 1)
                asagiMi = false;

            if (asagiMi)
                mevcutRay++;
            else
                mevcutRay--;
        }

        // satirlari birlestir
        var sonuc = new StringBuilder();
        for (int i = 0; i < raySayisi; i++)
            sonuc.Append(satirlar[i]);

        return sonuc.ToString();
    }
}
