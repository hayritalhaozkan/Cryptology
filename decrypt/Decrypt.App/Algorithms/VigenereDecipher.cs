using System;
using System.Collections.Generic;
using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

/// <summary>
/// Sayı Anahtarlı Çözme (Vigenère Decipher).
/// D(yᵢ) = (yᵢ - kᵢ mod len) mod 29
/// </summary>
public sealed class VigenereDecipher : IDecipher
{
    public string Name => "Sayı Anahtarlı (Vigenère)";
    public string KeyHint => "Virgülle ayrılmış sayılar girin.\nÖrn: 3,7,1,15,22";
    public string[] KeyLabels => new[] { "Sayısal Anahtar" };

    public string Decrypt(string cipherText, string[] keys)
    {
        if (keys.Length < 1 || string.IsNullOrWhiteSpace(keys[0]))
            throw new ArgumentException("Sayısal anahtar gerekli. Örn: 3,7,1,15,22");

        var keyNums = ParseKey(keys[0]);
        if (keyNums.Count == 0)
            throw new ArgumentException("En az bir anahtar sayısı gerekli.");

        var normalized = TextNormalizer.Normalize(cipherText);
        var sb = new StringBuilder(normalized.Length);

        int ki = 0;
        foreach (char c in normalized)
        {
            int y = TurkishAlphabet.IndexOf(c);
            if (y >= 0)
            {
                int shift = keyNums[ki % keyNums.Count];
                sb.Append(TurkishAlphabet.CharAt(y - shift));
                ki++;
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    private static List<int> ParseKey(string raw)
    {
        var result = new List<int>();
        var parts = raw.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        foreach (var p in parts)
        {
            if (!int.TryParse(p, out int val))
                throw new ArgumentException($"'{p}' geçerli bir sayı değil.");
            result.Add(val);
        }
        return result;
    }
}
