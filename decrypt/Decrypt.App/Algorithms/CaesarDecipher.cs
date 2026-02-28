using System;
using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

/// <summary>
/// Kaydırmalı Şifre Çözme (Caesar Decipher).
/// D(y) = (y - k) mod 29
/// </summary>
public sealed class CaesarDecipher : IDecipher
{
    public string Name => "Kaydırmalı (Caesar)";
    public string KeyHint => "Kaydırma sayısı girin (ör: 3)";
    public string[] KeyLabels => new[] { "Kaydırma (k)" };

    public string Decrypt(string cipherText, string[] keys)
    {
        if (keys.Length < 1 || !int.TryParse(keys[0], out int shift))
            throw new ArgumentException("Anahtar bir tamsayı olmalı. Örn: 3");

        var normalized = TextNormalizer.Normalize(cipherText);
        var sb = new StringBuilder(normalized.Length);

        foreach (char c in normalized)
        {
            int idx = TurkishAlphabet.IndexOf(c);
            if (idx >= 0)
                sb.Append(TurkishAlphabet.CharAt(idx - shift));
            else
                sb.Append(c);
        }

        return sb.ToString();
    }
}