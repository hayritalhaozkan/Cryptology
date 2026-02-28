using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

/// <summary>
/// Permütasyon Şifresi (Block/Columnar Transposition Cipher).
/// Metin blok boyutuna (permütasyon uzunluğu) bölünür. Her blok permütasyon sırasına göre yeniden dizilir.
/// 
/// Anahtar: virgülle ayrılmış permütasyon dizisi. Örn: "3,1,4,2" → blok boyutu 4.
/// Blok = [H,E,L,L]  perm = [3,1,4,2] → çıktı pozisyon sırasına göre: [E,L,H,L]
/// 
/// Permütasyon tanımı: perm[i] = "plain'ın i. pozisyonundaki harf, cipher'ın perm[i]. pozisyonuna gider."
/// </summary>
public sealed class PermutationCipher : ICipher
{
    public string Name => "Permütasyon (Transposition)";
    public string KeyHint => "Permütasyon sırası girin.\nÖrn: 3,1,4,2 (blok=4)";
    public string[] KeyLabels => new[] { "Permütasyon (virgülle)" };

    public string Encrypt(string plainText, string[] keys)
    {
        if (keys.Length < 1 || string.IsNullOrWhiteSpace(keys[0]))
            throw new ArgumentException("Permütasyon anahtarı gerekli. Örn: 3,1,4,2");

        var perm = ParsePerm(keys[0]);
        int blockSize = perm.Length;

        var normalized = TextNormalizer.Normalize(plainText);

        // Eksik kalan blok son harfi 'A' ile doldurulur (padding)
        while (normalized.Length % blockSize != 0)
            normalized += TurkishAlphabet.Letters[0]; // 'A' padding

        var sb = new StringBuilder(normalized.Length);

        for (int b = 0; b < normalized.Length; b += blockSize)
        {
            var block = normalized.Substring(b, blockSize);
            var outBlock = new char[blockSize];

            for (int i = 0; i < blockSize; i++)
            {
                // perm[i] = plain'ın i. harfinin cipher'da gideceği pozisyon (1-indexed)
                outBlock[perm[i] - 1] = block[i];
            }

            sb.Append(outBlock);
        }

        return sb.ToString();
    }

    private static int[] ParsePerm(string raw)
    {
        var parts = raw.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            throw new ArgumentException("Permütasyon boş olamaz.");

        var perm = new int[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            if (!int.TryParse(parts[i], out perm[i]))
                throw new ArgumentException($"'{parts[i]}' geçerli bir sayı değil.");
        }

        // Validate: 1..n permütasyon olmalı
        var sorted = perm.OrderBy(x => x).ToArray();
        for (int i = 0; i < sorted.Length; i++)
        {
            if (sorted[i] != i + 1)
                throw new ArgumentException(
                    $"Permütasyon 1'den {perm.Length}'ye kadar her sayıyı tam bir kez içermeli.");
        }

        return perm;
    }
}
