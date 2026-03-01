using System.Collections.Generic;
using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

// ============================================================================
// YER DEGISTIRME SIFRE COZME (SUBSTITUTION DECIPHER)
// ============================================================================
// Substitution sifresinin tersini yapar.
//
// SIFRELEME NASIL CALISIYORDU?
//   Duz alfabe:    A B C Ç D ...
//   Anahtar alf.:  Ü Y Z A B ...
//   A -> Ü, B -> Y, C -> Z, ...
//   (duz alfabe indexi -> o indexteki anahtar harfi)
//
// COZME NASIL CALISIR?
//   Tam tersini yapar:
//   Ü -> A, Y -> B, Z -> C, ...
//   (anahtar harfinin pozisyonu -> o pozisyondaki duz alfabe harfi)
//
//   Yani sifreli metindeki bir harf anahtar alfabede bulunur,
//   o harfin anahtar alfabedeki pozisyonu alinir,
//   ve duz alfabenin ayni pozisyonundaki harf yazilir.
//
// ORNEK:
//   Sifreli metin: "ÜYZ"    Anahtar: "ÜYZABCÇDEFGĞHIIJKLMNOÖPRSŞTÜV"
//   Ü -> anahtar alfabede 0. sirada -> duz alfabede 0. sira = A
//   Y -> anahtar alfabede 1. sirada -> duz alfabede 1. sira = B
//   Z -> anahtar alfabede 2. sirada -> duz alfabede 2. sira = C
//   Sonuc: "ABC"
// ============================================================================
public sealed class SubstitutionDecipher : IDecipher
{
    public string Name => "Yer Degistirme (Substitution)";
    public string KeyHint => "29 harflik karisik alfabe girin.\nOrn: ÜYZABCÇDEFGĞHIIJKLMNOÖPRSŞTÜV";
    public string[] KeyLabels => new[] { "Anahtar Alfabesi (29 harf)" };

    public string Decrypt(string sifreliMetin, string[] anahtarlar)
    {
        // anahtar alfabeyi normalize et
        string anahtarAlfabe = TextNormalizer.Normalize(anahtarlar[0]);

        // TERS ESLESME TABLOSU olustur
        // sifreleme: duz index -> anahtar harf
        // cozme: anahtar harf -> duz index
        // yani her anahtar harfinin hangi indexte oldugunu tutar
        var tersEsleme = new Dictionary<char, int>();
        for (int i = 0; i < anahtarAlfabe.Length; i++)
            tersEsleme[anahtarAlfabe[i]] = i;
        // ornegin anahtar "ÜYZAB..." ise:
        // tersEsleme = { 'Ü': 0, 'Y': 1, 'Z': 2, 'A': 3, 'B': 4, ... }

        string normalMetin = TextNormalizer.Normalize(sifreliMetin);

        var sonuc = new StringBuilder();

        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i];

            // sifreli harfi anahtar alfabede bul
            if (tersEsleme.ContainsKey(harf))
            {
                // bu harfin anahtar alfabedeki pozisyonunu al
                int duzIndex = tersEsleme[harf];

                // duz alfabenin ayni pozisyonundaki harfi yaz
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
