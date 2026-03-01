using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

// dogrusal sifre cozme (affine)
// D(y) = a^-1 * (y - b) mod 29
public sealed class AffineDecipher : IDecipher
{
    public string Name => "Dogrusal (Affine)";
    public string KeyHint => "a ve b degerlerini girin. Orn: a=2, b=5";
    public string[] KeyLabels => new[] { "a", "b" };

    public string Decrypt(string sifreliMetin, string[] anahtarlar)
    {
        int a = int.Parse(anahtarlar[0]);
        int b = int.Parse(anahtarlar[1]);

        // a nin tersini bul
        int aTersi = TurkishAlphabet.ModInverse(a, TurkishAlphabet.N);

        string normalMetin = TextNormalizer.Normalize(sifreliMetin);

        var sonuc = new StringBuilder();
        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i];
            int y = TurkishAlphabet.IndexOf(harf);
            if (y >= 0)
            {
                // ters affine formulu
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
