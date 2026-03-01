using System;
using System.Collections.Generic;

namespace Encrypt.App.Helpers;

public static class TurkishAlphabet
{
    // turk alfabesindeki harf sayisi
    public const int N = 29;

    // turk alfabesi harfleri - buyuk harf
    public static readonly string Harfler = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    // eski kodla uyumluluk icin
    public static readonly string Letters = Harfler;

    // her harfin indexini tutan sozluk
    private static readonly Dictionary<char, int> harfIndex;

    static TurkishAlphabet()
    {
        harfIndex = new Dictionary<char, int>(N);
        for (int i = 0; i < Harfler.Length; i++)
            harfIndex[Harfler[i]] = i;
    }

    // harfin indexini bul, yoksa -1 dondur
    public static int IndexOf(char c)
    {
        if (harfIndex.ContainsKey(c))
            return harfIndex[c];
        return -1;
    }

    // indexten harfi bul (mod N ile)
    public static char CharAt(int index)
    {
        int sonuc = ((index % N) + N) % N;
        return Harfler[sonuc];
    }

    // harf alfabede var mi
    public static bool Contains(char c)
    {
        return harfIndex.ContainsKey(c);
    }

    // moduler ters bulma - affine icin lazim
    public static int ModInverse(int a, int m)
    {
        a = ((a % m) + m) % m;
        for (int x = 1; x < m; x++)
        {
            if ((a * x) % m == 1)
                return x;
        }
        return -1; // bulunamadi
    }

    // en buyuk ortak bolen
    public static int Gcd(int a, int b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (b != 0)
        {
            int gecici = b;
            b = a % b;
            a = gecici;
        }
        return a;
    }
}
