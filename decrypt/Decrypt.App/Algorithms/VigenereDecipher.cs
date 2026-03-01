using System.Collections.Generic;
using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

// sayi anahtarli sifre cozme (vigenere)
// her harf anahtardaki sayiya gore geri kaydrilir
public sealed class VigenereDecipher : IDecipher
{
    public string Name => "Sayi Anahtarli (Vigenere)";
    public string KeyHint => "Virgul ile ayrilmis sayilar girin.\nOrn: 3,7,1,15,22";
    public string[] KeyLabels => new[] { "Sayisal Anahtar" };

    public string Decrypt(string sifreliMetin, string[] anahtarlar)
    {
        // anahtari parcala
        string[] parcalar = anahtarlar[0].Split(',');
        var anahtarSayilari = new List<int>();
        for (int i = 0; i < parcalar.Length; i++)
        {
            string parca = parcalar[i].Trim();
            if (parca.Length > 0)
                anahtarSayilari.Add(int.Parse(parca));
        }

        string normalMetin = TextNormalizer.Normalize(sifreliMetin);

        var sonuc = new StringBuilder();
        int anahtarIndex = 0;

        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i];
            int y = TurkishAlphabet.IndexOf(harf);
            if (y >= 0)
            {
                int kaydirma = anahtarSayilari[anahtarIndex % anahtarSayilari.Count];
                sonuc.Append(TurkishAlphabet.CharAt(y - kaydirma));
                anahtarIndex++;
            }
            else
            {
                sonuc.Append(harf);
            }
        }

        return sonuc.ToString();
    }
}
