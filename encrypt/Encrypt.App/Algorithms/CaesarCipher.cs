using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

// ============================================================================
// KAYDIRMALI SIFRE (CAESAR CIPHER)
// ============================================================================
// En basit sifreleme yontemlerinden biridir. Julius Caesar tarafindan kullanilmistir.
//
// NASIL CALISIR?
// Alfabedeki her harfi belirli bir sayi kadar ileri kaydirir.
// Ornegin kaydirma = 3 ise:
//   A -> Ç  (A'dan 3 harf ileri)
//   B -> D  (B'den 3 harf ileri)
//   Z -> C  (Z'den 3 ileri gidince basa doner: Z -> A -> B -> C)
//
// MATEMATIKSEL FORMUL:
//   E(x) = (x + k) mod 29
//   x = harfin alfabedeki sirasi (0-28)
//   k = kaydirma miktari (anahtar)
//   mod 29 = 29'a bolumunden kalan (alfabe sonu gelince basa doner)
//
// ORNEK:
//   Metin: "MERHABA"    Anahtar: k = 3
//   M(16) -> (16+3) mod 29 = 19 -> Ö
//   E(5)  -> (5+3)  mod 29 = 8  -> Ğ
//   R(20) -> (20+3) mod 29 = 23 -> T
//   ...
//   Sonuc: "ÖĞTJÇDÇ"
// ============================================================================
public sealed class CaesarCipher : ICipher
{
    // combobox'ta gorunecek isim
    public string Name => "Kaydirmali (Caesar)";

    // kullaniciya gosterilecek ipucu
    public string KeyHint => "Kaydirma sayisi girin (orn: 3)";

    // tek bir anahtar alani var: kaydirma miktari
    public string[] KeyLabels => new[] { "Kaydirma (k)" };

    public string Encrypt(string duzMetin, string[] anahtarlar)
    {
        // kullanicinin girdigi kaydirma degerini sayiya cevir
        // ornegin kullanici "3" yazdiysa kaydirma = 3 olur
        int kaydirma = int.Parse(anahtarlar[0]);

        // metni normalize et (buyuk harf yap, bosluk/noktalama kaldir)
        string normalMetin = TextNormalizer.Normalize(duzMetin);

        // sifrelenmis metni olusturmak icin StringBuilder kullaniyoruz
        var sonuc = new StringBuilder();

        // metnin her harfini tek tek isle
        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i]; // simdiki harf

            // harfin alfabedeki sirasini bul (A=0, B=1, ... Z=28)
            int index = TurkishAlphabet.IndexOf(harf);

            if (index >= 0) // harf alfabede bulunduysa
            {
                // harfi kaydirma miktari kadar ileri kaydir
                // CharAt metodu mod 29 islemini otomatik yapar
                // yani alfabe sonuna gelince basa doner
                char yeniHarf = TurkishAlphabet.CharAt(index + kaydirma);
                sonuc.Append(yeniHarf);
            }
            else
            {
                // alfabede olmayan karakter (normalde buraya gelmez
                // cunku normalize zaten sadece alfabe harflerini birakir)
                sonuc.Append(harf);
            }
        }

        return sonuc.ToString();
    }
}