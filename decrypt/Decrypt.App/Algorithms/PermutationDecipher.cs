using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

// ============================================================================
// PERMUTASYON SIFRE COZME (TRANSPOSITION DECIPHER)
// ============================================================================
// Permutasyon sifresinin tersini yapar.
//
// SIFRELEME NASIL CALISIYORDU?
//   Blok: M E R H    Permutasyon: 3,1,4,2
//   M(1. poz) -> 3. poza gider
//   E(2. poz) -> 1. poza gider
//   R(3. poz) -> 4. poza gider
//   H(4. poz) -> 2. poza gider
//   Sifreli: E H M R
//
// COZME NASIL CALISIR?
//   Tam tersini yapar:
//   Sifreli blok: E H M R    Permutasyon: 3,1,4,2
//   perm[0]=3 -> cozulen[0] = sifreli[3-1] = sifreli[2] = M
//   perm[1]=1 -> cozulen[1] = sifreli[1-1] = sifreli[0] = E
//   perm[2]=4 -> cozulen[2] = sifreli[4-1] = sifreli[3] = R
//   perm[3]=2 -> cozulen[3] = sifreli[2-1] = sifreli[1] = H
//   Cozulen: M E R H
//
// NEDEN TERS CALISIYOR?
//   Sifreleme: yeniBlok[perm[i]-1] = blok[i]
//     (i. harfi perm[i]. pozisyona KOY)
//   Cozme: yeniBlok[i] = blok[perm[i]-1]
//     (perm[i]. pozisyondan AL ve i. pozisyona koy)
// ============================================================================
public sealed class PermutationDecipher : IDecipher
{
    public string Name => "Permutasyon (Transposition)";
    public string KeyHint => "Permutasyon sirasi girin.\nOrn: 3,1,4,2 (blok=4)";
    public string[] KeyLabels => new[] { "Permutasyon (virgul ile)" };

    public string Decrypt(string sifreliMetin, string[] anahtarlar)
    {
        // permutasyon anahtarini parcala
        string[] parcalar = anahtarlar[0].Split(',');
        int[] perm = new int[parcalar.Length];
        for (int i = 0; i < parcalar.Length; i++)
            perm[i] = int.Parse(parcalar[i].Trim());

        int blokBoyutu = perm.Length;

        string normalMetin = TextNormalizer.Normalize(sifreliMetin);

        // eksik kalanlari 'A' ile doldur
        while (normalMetin.Length % blokBoyutu != 0)
            normalMetin += 'A';

        var sonuc = new StringBuilder();

        // her blok icin TERS permutasyon uygula
        for (int b = 0; b < normalMetin.Length; b += blokBoyutu)
        {
            string blok = normalMetin.Substring(b, blokBoyutu);
            char[] yeniBlok = new char[blokBoyutu];

            for (int i = 0; i < blokBoyutu; i++)
            {
                // SIFRELEME: yeniBlok[perm[i]-1] = blok[i]  (harfi perm pozisyonuna KOY)
                // COZME:     yeniBlok[i] = blok[perm[i]-1]   (perm pozisyonundan harfi AL)
                yeniBlok[i] = blok[perm[i] - 1];
            }

            sonuc.Append(yeniBlok);
        }

        return sonuc.ToString();
    }
}
