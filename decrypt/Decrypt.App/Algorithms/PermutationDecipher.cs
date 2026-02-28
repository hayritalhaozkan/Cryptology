using System;
using System.Linq;
using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

/// <summary>
/// Permütasyon Çözme (Block Transposition Decipher).
/// Ters permütasyon uygular.
/// </summary>
public sealed class PermutationDecipher : IDecipher
{
    public string Name => "Permütasyon (Transposition)";
    public string KeyHint => "Permütasyon sırası girin.\nÖrn: 3,1,4,2 (blok=4)";
    public string[] KeyLabels => new[] { "Permütasyon (virgülle)" };

    public string Decrypt(string cipherText, string[] keys)
    {
        if (keys.Length < 1 || string.IsNullOrWhiteSpace(keys[0]))
            throw new ArgumentException("Permütasyon anahtarı gerekli. Örn: 3,1,4,2");

        var perm = ParsePerm(keys[0]);
        int blockSize = perm.Length;

        var normalized = TextNormalizer.Normalize(cipherText);

        while (normalized.Length % blockSize != 0)
            normalized += TurkishAlphabet.Letters[0];

        var sb = new StringBuilder(normalized.Length);

        for (int b = 0; b < normalized.Length; b += blockSize)
        {
            var block = normalized.Substring(b, blockSize);
            var outBlock = new char[blockSize];

            for (int i = 0; i < blockSize; i++)
            {
                // Encrypt: outBlock[perm[i]-1] = block[i]
                // Decrypt (ters): outBlock[i] = block[perm[i]-1]
                outBlock[i] = block[perm[i] - 1];
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
