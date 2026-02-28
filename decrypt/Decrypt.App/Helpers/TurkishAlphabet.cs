using System;
using System.Collections.Generic;

namespace Decrypt.App.Helpers;

/// <summary>
/// Türk alfabesi (29 harf) ile index işlemleri.
/// Alfabe: A B C Ç D E F G Ğ H I İ J K L M N O Ö P R S Ş T U Ü V Y Z
/// N = 29
/// </summary>
public static class TurkishAlphabet
{
    public const int N = 29;

    public static readonly string Letters = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    private static readonly Dictionary<char, int> _charToIndex;

    static TurkishAlphabet()
    {
        _charToIndex = new Dictionary<char, int>(N);
        for (int i = 0; i < Letters.Length; i++)
            _charToIndex[Letters[i]] = i;
    }

    public static int IndexOf(char c) =>
        _charToIndex.TryGetValue(c, out var idx) ? idx : -1;

    public static char CharAt(int index) =>
        Letters[((index % N) + N) % N];

    public static bool Contains(char c) =>
        _charToIndex.ContainsKey(c);

    public static int ModInverse(int a, int m)
    {
        a = ((a % m) + m) % m;
        for (int x = 1; x < m; x++)
        {
            if ((a * x) % m == 1)
                return x;
        }
        throw new ArgumentException($"'{a}' sayısının mod {m} tersi yok. gcd({a},{m}) ≠ 1.");
    }

    public static int Gcd(int a, int b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }
        return a;
    }
}
