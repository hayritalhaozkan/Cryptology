using System;
using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

/// <summary>
/// Zigzag Çözme (Rail Fence Decipher).
/// Şifreli metin satır satır okunmuştu → ters işlem: her ray'e düşen harf sayısını hesapla,
/// şifreli metinden o kadar harfi al, zigzag düzeninde geri yerleştir.
/// </summary>
public sealed class ZigzagDecipher : IDecipher
{
    public string Name => "Zigzag (Rail Fence)";
    public string KeyHint => "Ray sayısı girin. Örn: 3";
    public string[] KeyLabels => new[] { "Ray Sayısı" };

    public string Decrypt(string cipherText, string[] keys)
    {
        if (keys.Length < 1 || !int.TryParse(keys[0], out int rails))
            throw new ArgumentException("Ray sayısı tamsayı olmalı. Örn: 3");

        if (rails <= 0)
            throw new ArgumentException("Ray sayısı pozitif olmalı.");

        var normalized = TextNormalizer.Normalize(cipherText);
        int len = normalized.Length;

        if (len == 0) return string.Empty;
        if (rails == 1 || rails >= len) return normalized;

        // Her ray'deki harf sayısını hesapla
        var rowLengths = new int[rails];
        int rail = 0;
        bool down = true;
        for (int i = 0; i < len; i++)
        {
            rowLengths[rail]++;
            if (rail == 0) down = true;
            else if (rail == rails - 1) down = false;
            rail += down ? 1 : -1;
        }

        // Şifreli metni raylara dağıt
        var rows = new string[rails];
        int pos = 0;
        for (int r = 0; r < rails; r++)
        {
            rows[r] = normalized.Substring(pos, rowLengths[r]);
            pos += rowLengths[r];
        }

        // Zigzag sırasında okuyarak orijinal metni oluştur
        var indices = new int[rails]; // her ray'in mevcut indeksi
        var sb = new StringBuilder(len);

        rail = 0;
        down = true;
        for (int i = 0; i < len; i++)
        {
            sb.Append(rows[rail][indices[rail]]);
            indices[rail]++;

            if (rail == 0) down = true;
            else if (rail == rails - 1) down = false;
            rail += down ? 1 : -1;
        }

        return sb.ToString();
    }
}
