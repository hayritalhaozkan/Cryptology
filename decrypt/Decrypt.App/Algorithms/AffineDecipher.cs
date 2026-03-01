using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

// ============================================================================
// DOGRUSAL SIFRE COZME (AFFINE DECIPHER)
// ============================================================================
// Affine sifresinin tersini yapar.
//
// SIFRELEME FORMULU: E(x) = (a * x + b) mod 29
// COZME FORMULU:     D(y) = a^(-1) * (y - b) mod 29
//
// a^(-1) nedir?
//   a'nin moduler tersidir. Yani a * a^(-1) = 1 (mod 29) olan sayi.
//   Ornegin: a=2 ise, 2 * 15 = 30, 30 mod 29 = 1 -> a^(-1) = 15
//
// COZME NASIL CALISIR?
//   1. Once a'nin tersini (a^-1) bul
//   2. Sifreli harfin indexinden b'yi cikar
//   3. Sonucu a^-1 ile carp
//   4. mod 29 al
//
// ORNEK:
//   Sifreli metin: "EG"    Anahtar: a=2, b=5
//   a^(-1) = 15  (cunku 2*15 = 30, 30 mod 29 = 1)
//
//   E(5) -> 15 * (5 - 5) mod 29 = 15 * 0 mod 29 = 0 -> A
//   G(7) -> 15 * (7 - 5) mod 29 = 15 * 2 mod 29 = 30 mod 29 = 1 -> B
//   Sonuc: "AB"
// ============================================================================
public sealed class AffineDecipher : IDecipher
{
    public string Name => "Dogrusal (Affine)";
    public string KeyHint => "a ve b degerlerini girin. Orn: a=2, b=5";
    public string[] KeyLabels => new[] { "a", "b" };

    public string Decrypt(string sifreliMetin, string[] anahtarlar)
    {
        // a ve b degerlerini al
        int a = int.Parse(anahtarlar[0]);
        int b = int.Parse(anahtarlar[1]);

        // a'nin moduler tersini bul
        // bu deger sifre cozme formulunde kullanilacak
        int aTersi = TurkishAlphabet.ModInverse(a, TurkishAlphabet.N);

        string normalMetin = TextNormalizer.Normalize(sifreliMetin);

        var sonuc = new StringBuilder();

        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i];
            int y = TurkishAlphabet.IndexOf(harf);

            if (y >= 0)
            {
                // affine cozme formulu: x = a^(-1) * (y - b) mod 29
                // once y'den b'yi cikar, sonra a'nin tersiyle carp, sonra mod 29 al
                int cozulmus = ((aTersi * (y - b)) % TurkishAlphabet.N + TurkishAlphabet.N) % TurkishAlphabet.N;
                sonuc.Append(TurkishAlphabet.CharAt(cozulmus));
            }
            else
            {
                sonuc.Append(harf);
            }
        }

        return sonuc.ToString();
    }
}
