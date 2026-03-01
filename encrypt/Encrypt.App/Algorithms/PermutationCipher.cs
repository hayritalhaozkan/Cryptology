using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

// ============================================================================
// PERMUTASYON SIFRESI (BLOCK TRANSPOSITION CIPHER)
// ============================================================================
// Diger sifrelerden farkli olarak bu sifre harfleri DEGISTIRMEZ,
// sadece YERLERINI DEGISTIRIR (yer degistirme / transposition).
//
// NASIL CALISIR?
// 1. Metin belirli boyuttaki BLOKLARA bolunur
// 2. Her bloktaki harfler, permutasyon sirasina gore yeniden dizilir
//
// ANAHTAR: Virgullerle ayrilmis sayilar. Ornegin: 3,1,4,2
//   Bu demek ki: blok boyutu = 4
//   1. pozisyondaki harf -> 3. pozisyona gider
//   2. pozisyondaki harf -> 1. pozisyona gider
//   3. pozisyondaki harf -> 4. pozisyona gider
//   4. pozisyondaki harf -> 2. pozisyona gider
//
// ORNEK:
//   Metin: "MERHABA"    Anahtar: 3,1,4,2
//   Blok boyutu = 4
//   
//   1. blok: M E R H
//     M(1. poz) -> 3. poza gider
//     E(2. poz) -> 1. poza gider
//     R(3. poz) -> 4. poza gider
//     H(4. poz) -> 2. poza gider
//     Sonuc: E H M R
//   
//   2. blok: A B A (eksik! 'A' ile doldurulur -> A B A A)
//     A(1. poz) -> 3. poza gider
//     B(2. poz) -> 1. poza gider  
//     A(3. poz) -> 4. poza gider
//     A(4. poz) -> 2. poza gider
//     Sonuc: B A A A
//   
//   Toplam sonuc: "EHMRBAAA"
//
// NOT: Eger metnin uzunlugu blok boyutuna tam bolunmezse
//      son blok 'A' harfleriyle doldurulur (padding).
// ============================================================================
public sealed class PermutationCipher : ICipher
{
    public string Name => "Permutasyon (Transposition)";
    public string KeyHint => "Permutasyon sirasi girin.\nOrn: 3,1,4,2 (blok=4)";
    public string[] KeyLabels => new[] { "Permutasyon (virgul ile)" };

    public string Encrypt(string duzMetin, string[] anahtarlar)
    {
        // anahtar stringi parcala ve sayilara cevir
        // "3,1,4,2" -> [3, 1, 4, 2]
        string[] parcalar = anahtarlar[0].Split(',');
        int[] perm = new int[parcalar.Length];
        for (int i = 0; i < parcalar.Length; i++)
            perm[i] = int.Parse(parcalar[i].Trim());

        // blok boyutu = permutasyon uzunlugu
        int blokBoyutu = perm.Length;

        // metni normalize et
        string normalMetin = TextNormalizer.Normalize(duzMetin);

        // metin uzunlugu blok boyutuna tam bolunmuyorsa
        // sonuna 'A' harfi ekle (padding)
        // ornegin: metin = "MERHABA" (7 harf), blok = 4
        // 7 % 4 = 3 (tam bolunmuyor), 1 tane 'A' eklenir -> "MERHABAA" (8 harf)
        while (normalMetin.Length % blokBoyutu != 0)
            normalMetin += 'A';

        var sonuc = new StringBuilder();

        // metni blok blok isle
        for (int b = 0; b < normalMetin.Length; b += blokBoyutu)
        {
            // mevcut bloku al (ornegin ilk 4 harf)
            string blok = normalMetin.Substring(b, blokBoyutu);

            // yeni blok icin bos dizi olustur
            char[] yeniBlok = new char[blokBoyutu];

            // permutasyonu uygula
            for (int i = 0; i < blokBoyutu; i++)
            {
                // perm[i] = i. harfin gidecegi pozisyon (1-indexed)
                // ornegin perm[0] = 3 ise, 0. harf 3. pozisyona gider
                // -1 cunku perm 1'den basliyor ama dizi 0'dan basliyor
                yeniBlok[perm[i] - 1] = blok[i];
            }

            // yeni bloku sonuca ekle
            sonuc.Append(yeniBlok);
        }

        return sonuc.ToString();
    }
}
