using System.Collections.Generic;
using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

// yer degistirme sifre cozme (substitution)
// anahtar alfabedeki harfin pozisyonu duz alfabe harfine donusur
public sealed class SubstitutionDecipher : IDecipher
{
    public string Name => "Yer Degistirme (Substitution)";
    public string KeyHint => "29 harflik karisik alfabe girin.\nOrn: ÜYZABCÇDEFGĞHIIJKLMNOÖPRSŞTÜV";
    public string[] KeyLabels => new[] { "Anahtar Alfabesi (29 harf)" };

    public string Decrypt(string sifreliMetin, string[] anahtarlar)
    {
        // anahtar alfabeyi normalize et
        string anahtarAlfabe = TextNormalizer.Normalize(anahtarlar[0]);

        // ters eslesme tablosu olustur
        var tersEsleme = new Dictionary<char, int>();
        for (int i = 0; i < anahtarAlfabe.Length; i++)
            tersEsleme[anahtarAlfabe[i]] = i;

        string normalMetin = TextNormalizer.Normalize(sifreliMetin);

        var sonuc = new StringBuilder();
        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i];
            if (tersEsleme.ContainsKey(harf))
            {
                int duzIndex = tersEsleme[harf];
                sonuc.Append(TurkishAlphabet.Letters[duzIndex]);
            }
            else
            {
                sonuc.Append(harf);
            }
        }

        return sonuc.ToString();
    }
}
