using System;

namespace Decrypt.App.Algorithms;

public sealed class CaesarDecipher : IDecipher
{
    public string Name => "Caesar";

    public string Decrypt(string cipherText, string key)
    {
        if (!int.TryParse(key, out var shift))
            throw new ArgumentException("Anahtar sayı olmalı. Örn: 3");

        return Shift(cipherText ?? "", -shift);
    }

    private static string Shift(string input, int shift)
    {
        shift %= 26;

        char ShiftChar(char c, char baseChar)
        {
            int offset = c - baseChar;
            int shifted = (offset + shift + 26) % 26;
            return (char)(baseChar + shifted);
        }

        var chars = input.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            char c = chars[i];
            if (c >= 'a' && c <= 'z') chars[i] = ShiftChar(c, 'a');
            else if (c >= 'A' && c <= 'Z') chars[i] = ShiftChar(c, 'A');
        }
        return new string(chars);
    }
}