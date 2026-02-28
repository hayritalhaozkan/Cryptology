using System;
using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

/// <summary>
/// Rota Şifresi (Route Cipher).
/// Metin satır×sütun'luk bir ızgaraya yazılır (satır satır), sonra sütun sütun okunur.
/// 
/// Anahtar: "satır,sütun" biçiminde. Örn: "4,5" → 4 satır 5 sütun = 20 hücre.
/// Metin 20 karakterden kısa ise 'A' ile doldurulur.
/// 
/// Yazma: soldan sağa, yukarıdan aşağı.
/// Okuma: yukarıdan aşağıya, soldan sağa (column-major order).
/// </summary>
public sealed class RouteCipher : ICipher
{
    public string Name => "Rota (Route)";
    public string KeyHint => "Satır ve sütun sayısı.\nÖrn: 4,5";
    public string[] KeyLabels => new[] { "Satır", "Sütun" };

    public string Encrypt(string plainText, string[] keys)
    {
        if (keys.Length < 2
            || !int.TryParse(keys[0], out int rows)
            || !int.TryParse(keys[1], out int cols))
            throw new ArgumentException("Satır ve sütun sayısı (tamsayı) gerekli. Örn: 4 ve 5");

        if (rows <= 0 || cols <= 0)
            throw new ArgumentException("Satır ve sütun pozitif olmalı.");

        int gridSize = rows * cols;

        var normalized = TextNormalizer.Normalize(plainText);

        // Padding
        while (normalized.Length < gridSize)
            normalized += TurkishAlphabet.Letters[0]; // 'A'

        // Metni grid boyutuna kırp (fazlası varsa — uyarı yerine kırp)
        if (normalized.Length > gridSize)
            normalized = normalized.Substring(0, gridSize);

        // Grid'e satır satır yaz
        var grid = new char[rows, cols];
        int idx = 0;
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                grid[r, c] = normalized[idx++];

        // Sütun sütun oku (column-major)
        var sb = new StringBuilder(gridSize);
        for (int c = 0; c < cols; c++)
            for (int r = 0; r < rows; r++)
                sb.Append(grid[r, c]);

        return sb.ToString();
    }
}
