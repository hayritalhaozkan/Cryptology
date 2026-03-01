using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

// permutasyon sifre cozme
// ters permutasyon uygulayarak bloklari eski haline getirir
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

        // her blok icin ters permutasyon uygula
        for (int b = 0; b < normalMetin.Length; b += blokBoyutu)
        {
            string blok = normalMetin.Substring(b, blokBoyutu);
            char[] yeniBlok = new char[blokBoyutu];

            for (int i = 0; i < blokBoyutu; i++)
            {
                // sifrelemede: yeniBlok[perm[i]-1] = blok[i]
                // cozumde: yeniBlok[i] = blok[perm[i]-1]
                yeniBlok[i] = blok[perm[i] - 1];
            }

            sonuc.Append(yeniBlok);
        }

        return sonuc.ToString();
    }
}
