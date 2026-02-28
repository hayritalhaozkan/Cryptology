using System;
using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

/// <summary>
/// Doğrusal Şifre (Affine Cipher) — Türk alfabesi (mod 29).
/// E(x) = (a * x + b) mod 29
/// Anahtar: a ve b tamsayıları. a ile 29 arası arası gcd(a,29) = 1 olmalı.
/// </summary>
public sealed class AffineCipher : ICipher
{
    public string Name => "Doğrusal (Affine)";
    public string KeyHint => "a ve b girin. gcd(a,29)=1 olmalı. Örn: a=2, b=5";
    public string[] KeyLabels => new[] { "a", "b" };

    public string Encrypt(string plainText, string[] keys)
    {
        if (keys.Length < 2
            || !int.TryParse(keys[0], out int a)
            || !int.TryParse(keys[1], out int b))
            throw new ArgumentException("İki tamsayı gerekli: a ve b. Örn: a=2, b=5");

        if (TurkishAlphabet.Gcd(a, TurkishAlphabet.N) != 1)
            throw new ArgumentException(
                $"a={a} geçersiz. gcd({a}, {TurkishAlphabet.N}) = 1 olmalı.\n" +
                $"Geçerli a değerleri: 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28");

        var normalized = TextNormalizer.Normalize(plainText);
        var sb = new StringBuilder(normalized.Length);

        foreach (char c in normalized)
        {
            int x = TurkishAlphabet.IndexOf(c);
            if (x >= 0)
            {
                int enc = ((a * x + b) % TurkishAlphabet.N + TurkishAlphabet.N) % TurkishAlphabet.N;
                sb.Append(TurkishAlphabet.CharAt(enc));
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}
