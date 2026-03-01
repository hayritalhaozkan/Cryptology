using System.Collections.Generic;
using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

// ============================================================================
// SAYI ANAHTARLI SIFRE (VIGENERE CIPHER - SAYISAL VERSIYON)
// ============================================================================
// Caesar sifresinde tek bir kaydirma sayisi vardi ve her harfe ayni kaydirma uygulaniyordu.
// Vigenere sifresinde ise bir SAYI DIZISI kullanilir ve her harfe
// dizideki farkli bir sayi ile kaydirma uygulanir.
//
// NASIL CALISIR?
// Anahtar bir sayi dizisidir, ornegin: 3, 7, 1
// Metnin 1. harfine 3 kaydirma uygulanir
// Metnin 2. harfine 7 kaydirma uygulanir
// Metnin 3. harfine 1 kaydirma uygulanir
// Metnin 4. harfine tekrar 3 kaydirma uygulanir (anahtar basa doner)
// Metnin 5. harfine tekrar 7 kaydirma uygulanir
// ... ve boyle devam eder (dongusel / cyclic)
//
// MATEMATIKSEL FORMUL:
//   E(xi) = (xi + k[i mod anahtar_uzunlugu]) mod 29
//
// ORNEK:
//   Metin: "MERHABA"    Anahtar: 3,7,1
//   M(16) + 3 = 19 -> Ö
//   E(5)  + 7 = 12 -> J
//   R(20) + 1 = 21 -> S
//   H(9)  + 3 = 12 -> J  (anahtar basa dondu)
//   A(0)  + 7 = 7  -> G
//   B(1)  + 1 = 2  -> C
//   A(0)  + 3 = 3  -> Ç
//   Sonuc: "ÖJSJGCÇ"
//
// NEDEN CAESAR'DAN DAHA GUVENLI?
//   Caesar'da ayni harf her zaman ayni harfe donusur (orn: A hep D olur)
//   Vigenere'de ise ayni harf farkli harflere donusebilir cunku
//   her pozisyonda farkli kaydirma uygulanir.
// ============================================================================
public sealed class VigenereCipher : ICipher
{
    public string Name => "Sayi Anahtarli (Vigenere)";
    public string KeyHint => "Virgul ile ayrilmis sayilar girin.\nOrn: 3,7,1,15,22";
    public string[] KeyLabels => new[] { "Sayisal Anahtar" };

    public string Encrypt(string duzMetin, string[] anahtarlar)
    {
        // kullanicinin girdigi anahtari virgullerden parcala
        // ornegin "3,7,1" -> ["3", "7", "1"]
        string[] parcalar = anahtarlar[0].Split(',');

        // string parcalari sayiya cevir ve listeye ekle
        var anahtarSayilari = new List<int>();
        for (int i = 0; i < parcalar.Length; i++)
        {
            string parca = parcalar[i].Trim(); // basindaki/sonundaki bosluklari temizle
            if (parca.Length > 0)
                anahtarSayilari.Add(int.Parse(parca));
        }
        // simdi anahtarSayilari = [3, 7, 1]

        // metni normalize et
        string normalMetin = TextNormalizer.Normalize(duzMetin);

        var sonuc = new StringBuilder();
        int anahtarIndex = 0; // anahtarin hangi elemanindayiz

        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i];
            int x = TurkishAlphabet.IndexOf(harf);

            if (x >= 0)
            {
                // anahtardaki simdiki kaydirma degerini al
                // % (mod) islemiyle anahtar basa doner
                // ornegin anahtar [3,7,1] ve 4. harfteyiz: 3 % 3 = 0 -> tekrar 3 kullanilir
                int kaydirma = anahtarSayilari[anahtarIndex % anahtarSayilari.Count];

                // harfi kaydir
                sonuc.Append(TurkishAlphabet.CharAt(x + kaydirma));

                // bir sonraki anahtar elemanina gec
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
