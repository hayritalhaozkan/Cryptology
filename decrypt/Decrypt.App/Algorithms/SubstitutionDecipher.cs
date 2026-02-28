using System;
using System.Collections.Generic;
using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

/// <summary>
/// Yer Değiştirme Çözme (Substitution Decipher).
/// Anahtar alfabedeki harfin pozisyonu → düz alfabe harfi.
/// </summary>
public sealed class SubstitutionDecipher : IDecipher
{
    public string Name => "Yer Değiştirme (Substitution)";
    public string KeyHint => "29 harflik karışık alfabe girin.\nÖrn: ÜYZABCÇDEFGĞHIİJKLMNOÖPRSŞTUV";
    public string[] KeyLabels => new[] { "Anahtar Alfabesi (29 harf)" };

    public string Decrypt(string cipherText, string[] keys)
    {
        if (keys.Length < 1 || string.IsNullOrWhiteSpace(keys[0]))
            throw new ArgumentException("29 harflik anahtar alfabesi gerekli.");

        var keyAlphabet = TextNormalizer.Normalize(keys[0]);

        if (keyAlphabet.Length != TurkishAlphabet.N)
            throw new ArgumentException(
                $"Anahtar alfabe tam {TurkishAlphabet.N} harf olmalı. Girdiğiniz: {keyAlphabet.Length} harf.");

        var unique = new HashSet<char>(keyAlphabet);
        if (unique.Count != TurkishAlphabet.N)
            throw new ArgumentException("Anahtar alfabede tekrar eden veya eksik harfler var.");

        // Ters mapping: anahtar harf → düz alfabe pozisyonu
        var reverseMap = new Dictionary<char, int>(TurkishAlphabet.N);
        for (int i = 0; i < keyAlphabet.Length; i++)
            reverseMap[keyAlphabet[i]] = i;

        var normalized = TextNormalizer.Normalize(cipherText);
        var sb = new StringBuilder(normalized.Length);

        foreach (char c in normalized)
        {
            if (reverseMap.TryGetValue(c, out int plainIdx))
                sb.Append(TurkishAlphabet.Letters[plainIdx]);
            else
                sb.Append(c);
        }

        return sb.ToString();
    }
}
