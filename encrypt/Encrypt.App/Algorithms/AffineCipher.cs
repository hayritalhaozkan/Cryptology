using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

// dogrusal sifre (affine)
// E(x) = (a * x + b) mod 29
public sealed class AffineCipher : ICipher
{
    public string Name => "Dogrusal (Affine)";
    public string KeyHint => "a ve b degerlerini girin. Orn: a=2, b=5";
    public string[] KeyLabels => new[] { "a", "b" };

    public string Encrypt(string duzMetin, string[] anahtarlar)
    {
        int a = int.Parse(anahtarlar[0]);
        int b = int.Parse(anahtarlar[1]);

        string normalMetin = TextNormalizer.Normalize(duzMetin);

        var sonuc = new StringBuilder();
        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i];
            int x = TurkishAlphabet.IndexOf(harf);
            if (x >= 0)
            {
                // affine formulu: (a * x + b) mod 29
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
