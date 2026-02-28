using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

/// <summary>
/// Yer Değiştirme Şifresi (Monoalphabetic Substitution Cipher).
/// Anahtar: 29 harflik permütlenmiş (karışık) Türk alfabesi.
/// Örn: "ÜYZABCÇDEFGĞHIİJKLMNOÖPRSŞTUV"
/// Düz alfabe:   A B C Ç D E F G Ğ H I İ J K L M N O Ö P R S Ş T U Ü V Y Z
/// Anahtar alf.: Ü Y Z A B C Ç D E F G Ğ H I İ J K L M N O Ö P R S Ş T U V
/// </summary>
public sealed class SubstitutionCipher : ICipher
{
    public string Name => "Yer Değiştirme (Substitution)";
    public string KeyHint => "29 harflik karışık alfabe girin.\nÖrn: ÜYZABCÇDEFGĞHIİJKLMNOÖPRSŞTUV";
    public string[] KeyLabels => new[] { "Anahtar Alfabesi (29 harf)" };

    public string Encrypt(string plainText, string[] keys)
    {
        if (keys.Length < 1 || string.IsNullOrWhiteSpace(keys[0]))
            throw new ArgumentException("29 harflik anahtar alfabesi gerekli.");

        var keyAlphabet = TextNormalizer.Normalize(keys[0]);

        if (keyAlphabet.Length != TurkishAlphabet.N)
            throw new ArgumentException(
                $"Anahtar alfabe tam {TurkishAlphabet.N} harf olmalı. Girdiğiniz: {keyAlphabet.Length} harf.");

        // Tekrar kontrolü
        var unique = new HashSet<char>(keyAlphabet);
        if (unique.Count != TurkishAlphabet.N)
            throw new ArgumentException("Anahtar alfabede tekrar eden harfler var veya eksik harf var.");

        // Tüm harfler Türk alfabesinde mi?
        foreach (char c in keyAlphabet)
        {
            if (!TurkishAlphabet.Contains(c))
                throw new ArgumentException($"'{c}' Türk alfabesinde yok.");
        }

        // Mapping: düz alfabe index → anahtar alfabe harfi
        var normalized = TextNormalizer.Normalize(plainText);
        var sb = new StringBuilder(normalized.Length);

        foreach (char c in normalized)
        {
            int idx = TurkishAlphabet.IndexOf(c);
            if (idx >= 0)
                sb.Append(keyAlphabet[idx]);
            else
                sb.Append(c);
        }

        return sb.ToString();
    }
}
