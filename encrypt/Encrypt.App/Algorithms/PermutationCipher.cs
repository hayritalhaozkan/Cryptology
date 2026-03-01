using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

// permutasyon sifresi
// metin bloklara bolunur, her blok permutasyon sirasina gore yeniden dizilir
public sealed class PermutationCipher : ICipher
{
    public string Name => "Permutasyon (Transposition)";
    public string KeyHint => "Permutasyon sirasi girin.\nOrn: 3,1,4,2 (blok=4)";
    public string[] KeyLabels => new[] { "Permutasyon (virgul ile)" };

    public string Encrypt(string duzMetin, string[] anahtarlar)
    {
        // permutasyon anahtarini parcala
        string[] parcalar = anahtarlar[0].Split(',');
        int[] perm = new int[parcalar.Length];
        for (int i = 0; i < parcalar.Length; i++)
            perm[i] = int.Parse(parcalar[i].Trim());

        int blokBoyutu = perm.Length;

        string normalMetin = TextNormalizer.Normalize(duzMetin);

        // eksik kalanlari 'A' ile doldur
        while (normalMetin.Length % blokBoyutu != 0)
            normalMetin += 'A';

        var sonuc = new StringBuilder();

        // her blok icin permutasyon uygula
        for (int b = 0; b < normalMetin.Length; b += blokBoyutu)
        {
            string blok = normalMetin.Substring(b, blokBoyutu);
            char[] yeniBlok = new char[blokBoyutu];

            for (int i = 0; i < blokBoyutu; i++)
            {
                // i. harf perm[i]. pozisyona gider (1-indexed)
                yeniBlok[perm[i] - 1] = blok[i];
            }

            sonuc.Append(yeniBlok);
        }

        return sonuc.ToString();
    }
}
