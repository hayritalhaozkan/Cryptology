using System;
using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

/// <summary>
/// Zigzag Şifresi (Rail Fence Cipher).
/// Metin zigzag deseninde ray sayısı kadar satıra yazılır, sonra satır satır okunur.
/// 
/// Anahtar: ray sayısı (tamsayı). Örn: 3
/// 
/// Örnek (3 ray):
///   H . . . O . . . R
///   . E . L . W . L . D
///   . . L . . . O . .
/// Çıktı: HORELWLDLOO (satır satır)
/// </summary>
public sealed class ZigzagCipher : ICipher
{
    public string Name => "Zigzag (Rail Fence)";
    public string KeyHint => "Ray sayısı girin. Örn: 3";
    public string[] KeyLabels => new[] { "Ray Sayısı" };

    public string Encrypt(string plainText, string[] keys)
    {
        if (keys.Length < 1 || !int.TryParse(keys[0], out int rails))
            throw new ArgumentException("Ray sayısı tamsayı olmalı. Örn: 3");

        if (rails <= 0)
            throw new ArgumentException("Ray sayısı pozitif olmalı.");

        var normalized = TextNormalizer.Normalize(plainText);

        if (normalized.Length == 0)
            return string.Empty;

        if (rails == 1 || rails >= normalized.Length)
            return normalized;

        // Her ray için StringBuilder
        var rows = new StringBuilder[rails];
        for (int i = 0; i < rails; i++)
            rows[i] = new StringBuilder();

        int currentRail = 0;
        bool goingDown = true;

        foreach (char c in normalized)
        {
            rows[currentRail].Append(c);

            if (currentRail == 0)
                goingDown = true;
            else if (currentRail == rails - 1)
                goingDown = false;

            currentRail += goingDown ? 1 : -1;
        }

        var sb = new StringBuilder(normalized.Length);
        foreach (var row in rows)
            sb.Append(row);

        return sb.ToString();
    }
}
