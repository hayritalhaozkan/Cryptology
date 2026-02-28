using System;
using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

/// <summary>
/// Doğrusal Şifre Çözme (Affine Decipher).
/// D(y) = a⁻¹ * (y - b) mod 29
/// </summary>
public sealed class AffineDecipher : IDecipher
{
    public string Name => "Doğrusal (Affine)";
    public string KeyHint => "a ve b girin. gcd(a,29)=1 olmalı. Örn: a=2, b=5";
    public string[] KeyLabels => new[] { "a", "b" };

    public string Decrypt(string cipherText, string[] keys)
    {
        if (keys.Length < 2
            || !int.TryParse(keys[0], out int a)
            || !int.TryParse(keys[1], out int b))
            throw new ArgumentException("İki tamsayı gerekli: a ve b.");

        if (TurkishAlphabet.Gcd(a, TurkishAlphabet.N) != 1)
            throw new ArgumentException(
                $"a={a} geçersiz. gcd({a}, {TurkishAlphabet.N}) = 1 olmalı.");

        int aInv = TurkishAlphabet.ModInverse(a, TurkishAlphabet.N);

        var normalized = TextNormalizer.Normalize(cipherText);
        var sb = new StringBuilder(normalized.Length);

        foreach (char c in normalized)
        {
            int y = TurkishAlphabet.IndexOf(c);
            if (y >= 0)
            {
                int dec = ((aInv * (y - b)) % TurkishAlphabet.N + TurkishAlphabet.N) % TurkishAlphabet.N;
                sb.Append(TurkishAlphabet.CharAt(dec));
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}
