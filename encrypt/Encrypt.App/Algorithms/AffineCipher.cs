using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

// ============================================================================
// DOGRUSAL SIFRE (AFFINE CIPHER)
// ============================================================================
// Caesar sifresinin gelismis halidir. Caesar'da sadece toplama vardi,
// burada hem carpma hem toplama var.
//
// NASIL CALISIR?
// Her harfin alfabedeki sirasini alir, bir formule sokar, yeni sirasini bulur.
//
// MATEMATIKSEL FORMUL:
//   E(x) = (a * x + b) mod 29
//   x = harfin alfabedeki sirasi (0-28)
//   a = carpma anahtari (29 ile aralarinda asal olmali, yani EBOB(a,29) = 1)
//   b = toplama anahtari (herhangi bir sayi olabilir)
//   mod 29 = 29'a bolumunden kalan
//
// NEDEN a VE 29 ARALARINDA ASAL OLMALI?
//   Eger aralarinda asal olmazlarsa, farkli harfler ayni harfe donusebilir
//   ve sifre cozulemez hale gelir.
//   Ornegin a=29 olursa: (29 * x + b) mod 29 = b -> tum harfler ayni harf olur!
//
// ORNEK:
//   Metin: "AB"    Anahtar: a=2, b=5
//   A(0) -> (2*0 + 5) mod 29 = 5  -> E
//   B(1) -> (2*1 + 5) mod 29 = 7  -> G
//   Sonuc: "EG"
//
// NOT: Caesar sifresi aslinda a=1 olan ozel bir Affine sifresidir.
//   Caesar: E(x) = (x + k) mod 29  =  (1*x + k) mod 29
// ============================================================================
public sealed class AffineCipher : ICipher
{
    public string Name => "Dogrusal (Affine)";
    public string KeyHint => "a ve b degerlerini girin. Orn: a=2, b=5";

    // iki anahtar alani var: a (carpan) ve b (toplanan)
    public string[] KeyLabels => new[] { "a", "b" };

    public string Encrypt(string duzMetin, string[] anahtarlar)
    {
        // kullanicinin girdigi a ve b degerlerini sayiya cevir
        int a = int.Parse(anahtarlar[0]); // carpma anahtari
        int b = int.Parse(anahtarlar[1]); // toplama anahtari

        // metni normalize et
        string normalMetin = TextNormalizer.Normalize(duzMetin);

        var sonuc = new StringBuilder();

        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i];

            // harfin alfabedeki sirasini bul
            int x = TurkishAlphabet.IndexOf(harf);

            if (x >= 0)
            {
                // affine formulunu uygula: (a * x + b) mod 29
                // ((a * x + b) % N + N) % N seklinde yaziyoruz cunku
                // C#'ta negatif sayilarin mod'u negatif olabilir
                // +N ekleyip tekrar mod alinca her zaman pozitif sonuc cikar
                int sifreli = ((a * x + b) % TurkishAlphabet.N + TurkishAlphabet.N) % TurkishAlphabet.N;
                sonuc.Append(TurkishAlphabet.CharAt(sifreli));
            }
            else
            {
                sonuc.Append(harf);
            }
        }

        return sonuc.ToString();
    }
}
