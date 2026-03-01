using System.Collections.Generic;
using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

// sayi anahtarli sifre (vigenere)
// her harf anahtardaki sayiya gore kaydrilir
public sealed class VigenereCipher : ICipher
{
    public string Name => "Sayi Anahtarli (Vigenere)";
    public string KeyHint => "Virgul ile ayrilmis sayilar girin.\nOrn: 3,7,1,15,22";
    public string[] KeyLabels => new[] { "Sayisal Anahtar" };

    public string Encrypt(string duzMetin, string[] anahtarlar)
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

        string normalMetin = TextNormalizer.Normalize(duzMetin);

        var sonuc = new StringBuilder();
        int anahtarIndex = 0;

        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i];
            int x = TurkishAlphabet.IndexOf(harf);
            if (x >= 0)
            {
                int kaydirma = anahtarSayilari[anahtarIndex % anahtarSayilari.Count];
                sonuc.Append(TurkishAlphabet.CharAt(x + kaydirma));
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
