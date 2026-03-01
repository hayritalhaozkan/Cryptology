using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

// ============================================================================
// YER DEGISTIRME SIFRESI (MONOALPHABETIC SUBSTITUTION CIPHER)
// ============================================================================
// Bu sifrele yonteminde anahtar olarak KARISIK BIR ALFABE kullanilir.
// Duz (normal) alfabedeki her harfin karsiligi, anahtar alfabede ayni
// pozisyondaki harftir.
//
// NASIL CALISIR?
// Normal alfabe ile anahtar alfabe eslestirilir:
//   Duz alfabe:    A B C Ç D E F G Ğ H I İ J K L M N O Ö P R S Ş T U Ü V Y Z
//   Anahtar alf.:  Ü Y Z A B C Ç D E F G Ğ H I İ J K L M N O Ö P R S Ş T U V
//
// Metindeki her harf, duz alfabedeki sirasina bakilir
// ve anahtar alfabenin ayni sirasindaki harf yazilir.
//   A -> Ü (0. sira)
//   B -> Y (1. sira)
//   C -> Z (2. sira)
//   ...
//
// ORNEK:
//   Metin: "ABC"    Anahtar: "ÜYZABCÇDEFGĞHIIJKLMNOÖPRSŞTÜV"
//   A(0. sira) -> Ü
//   B(1. sira) -> Y
//   C(2. sira) -> Z
//   Sonuc: "ÜYZ"
//
// NEDEN GUVENLIDIR?
//   29! (29 faktoriyel) = yaklasik 8.8 * 10^30 farkli anahtar olasili var.
//   Bu da brute force ile kirmak icin cok buyuk bir sayi.
//   Ama harf frekansi analizi ile kirilebilir.
// ============================================================================
public sealed class SubstitutionCipher : ICipher
{
    public string Name => "Yer Degistirme (Substitution)";
    public string KeyHint => "29 harflik karisik alfabe girin.\nOrn: ÜYZABCÇDEFGĞHIIJKLMNOÖPRSŞTÜV";

    // tek anahtar alani: 29 harflik karisik alfabe
    public string[] KeyLabels => new[] { "Anahtar Alfabesi (29 harf)" };

    public string Encrypt(string duzMetin, string[] anahtarlar)
    {
        // kullanicinin girdigi anahtar alfabeyi normalize et
        // (buyuk harfe cevir, bosluklari kaldir)
        string anahtarAlfabe = TextNormalizer.Normalize(anahtarlar[0]);

        // duz metni normalize et
        string normalMetin = TextNormalizer.Normalize(duzMetin);

        var sonuc = new StringBuilder();

        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i];

            // harfin duz alfabedeki sirasini bul
            int index = TurkishAlphabet.IndexOf(harf);

            if (index >= 0)
            {
                // duz alfabedeki siraya karsilik gelen
                // anahtar alfabedeki harfi al
                // ornegin: A(0) ise anahtarAlfabe[0] = Ü
                sonuc.Append(anahtarAlfabe[index]);
            }
            else
            {
                sonuc.Append(harf);
            }
        }

        return sonuc.ToString();
    }
}
