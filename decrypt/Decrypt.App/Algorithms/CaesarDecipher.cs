using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

// ============================================================================
// KAYDIRMALI SIFRE COZME (CAESAR DECIPHER)
// ============================================================================
// Caesar sifresinin tersini yapar.
// Sifreleme ileri kaydiriyordu, cozme geri kaydirir.
//
// SIFRELEME FORMULU: E(x) = (x + k) mod 29
// COZME FORMULU:     D(y) = (y - k) mod 29
//
// ORNEK:
//   Sifreli metin: "ÖĞTJÇDÇ"    Anahtar: k = 3
//   Ö(18) - 3 = 15 -> M
//   Ğ(8)  - 3 = 5  -> E
//   T(23) - 3 = 20 -> R
//   ...
//   Sonuc: "MERHABA"
//
// Tek fark: sifreleme + k yapar, cozme - k yapar.
// ============================================================================
public sealed class CaesarDecipher : IDecipher
{
    public string Name => "Kaydirmali (Caesar)";
    public string KeyHint => "Kaydirma sayisi girin (orn: 3)";
    public string[] KeyLabels => new[] { "Kaydirma (k)" };

    public string Decrypt(string sifreliMetin, string[] anahtarlar)
    {
        // kaydirma degerini al
        int kaydirma = int.Parse(anahtarlar[0]);

        // sifreli metni normalize et
        string normalMetin = TextNormalizer.Normalize(sifreliMetin);

        var sonuc = new StringBuilder();

        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i];
            int index = TurkishAlphabet.IndexOf(harf);

            if (index >= 0)
            {
                // sifreleme + yapmisti, cozme - yapar
                // CharAt mod 29 ile dairesel calisir
                // yani 0'in altina inerse sondan devam eder
                char yeniHarf = TurkishAlphabet.CharAt(index - kaydirma);
                sonuc.Append(yeniHarf);
            }
            else
            {
                sonuc.Append(harf);
            }
        }

        return sonuc.ToString();
    }
}