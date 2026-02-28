using System;
using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

/// <summary>
/// Rota Çözme (Route Decipher).
/// Şifreleme: satır satır yaz → sütun sütun oku.
/// Çözme: sütun sütun yaz → satır satır oku.
/// </summary>
public sealed class RouteDecipher : IDecipher
{
    public string Name => "Rota (Route)";
    public string KeyHint => "Satır ve sütun sayısı.\nÖrn: 4,5";
    public string[] KeyLabels => new[] { "Satır", "Sütun" };

    public string Decrypt(string cipherText, string[] keys)
    {
        if (keys.Length < 2
            || !int.TryParse(keys[0], out int rows)
            || !int.TryParse(keys[1], out int cols))
            throw new ArgumentException("Satır ve sütun sayısı gerekli. Örn: 4 ve 5");

        if (rows <= 0 || cols <= 0)
            throw new ArgumentException("Satır ve sütun pozitif olmalı.");

        int gridSize = rows * cols;

        var normalized = TextNormalizer.Normalize(cipherText);

        while (normalized.Length < gridSize)
            normalized += TurkishAlphabet.Letters[0];

        if (normalized.Length > gridSize)
            normalized = normalized.Substring(0, gridSize);

        // Şifreleme sütun-major olarak okumuştu → çözmede sütun-major olarak yaz
        var grid = new char[rows, cols];
        int idx = 0;
        for (int c = 0; c < cols; c++)
            for (int r = 0; r < rows; r++)
                grid[r, c] = normalized[idx++];

        // Satır satır oku
        var sb = new StringBuilder(gridSize);
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                sb.Append(grid[r, c]);

        return sb.ToString();
    }
}
