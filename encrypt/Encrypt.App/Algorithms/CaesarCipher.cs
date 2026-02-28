using System;
using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

/// <summary>
/// Kaydırmalı Şifre (Caesar Cipher) — Türk alfabesi (mod 29).
/// E(x) = (x + k) mod 29
/// Anahtar: tek bir tamsayı (kaydırma miktarı).
/// </summary>
public sealed class CaesarCipher : ICipher
{
    public string Name => "Kaydırmalı (Caesar)";
    public string KeyHint => "Kaydırma sayısı girin (ör: 3)";
    public string[] KeyLabels => new[] { "Kaydırma (k)" };

    public string Encrypt(string plainText, string[] keys)
    {
        if (keys.Length < 1 || !int.TryParse(keys[0], out int shift))
            throw new ArgumentException("Anahtar bir tamsayı olmalı. Örn: 3");

        var normalized = TextNormalizer.Normalize(plainText);
        var sb = new StringBuilder(normalized.Length);

        foreach (char c in normalized)
        {
            int idx = TurkishAlphabet.IndexOf(c);
            if (idx >= 0)
                sb.Append(TurkishAlphabet.CharAt(idx + shift));
            else
                sb.Append(c);
        }

        return sb.ToString();
    }
}