using System.Collections.Generic;
using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

// ============================================================================
// SAYI ANAHTARLI SIFRE COZME (VIGENERE DECIPHER)
// ============================================================================
// Vigenere sifresinin tersini yapar.
// Sifreleme her harfe farkli bir kaydirma EKLIYORDU,
// cozme ayni kaydirmayi CIKARIR.
//
// SIFRELEME: E(xi) = (xi + ki) mod 29
// COZME:     D(yi) = (yi - ki) mod 29
//
// ORNEK:
//   Sifreli metin: "ÖJSJGCÇ"    Anahtar: 3,7,1
//   Ö(18) - 3 = 15 -> M    (1. anahtar elemani: 3)
//   J(12) - 7 = 5  -> E    (2. anahtar elemani: 7)
//   S(21) - 1 = 20 -> R    (3. anahtar elemani: 1)
//   J(12) - 3 = 9  -> H    (tekrar 1. eleman: 3)
//   G(7)  - 7 = 0  -> A    (tekrar 2. eleman: 7)
//   C(2)  - 1 = 1  -> B    (tekrar 3. eleman: 1)
//   Ç(3)  - 3 = 0  -> A    (tekrar 1. eleman: 3)
//   Sonuc: "MERHABA"
// ============================================================================
public sealed class VigenereDecipher : IDecipher
{
    public string Name => "Sayi Anahtarli (Vigenere)";
    public string KeyHint => "Virgul ile ayrilmis sayilar girin.\nOrn: 3,7,1,15,22";
    public string[] KeyLabels => new[] { "Sayisal Anahtar" };

    public string Decrypt(string sifreliMetin, string[] anahtarlar)
    {
        // anahtari virgullerden parcala ve sayilara cevir
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
                // sifreleme + yapmisti, cozme - yapar
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
