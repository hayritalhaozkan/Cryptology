using System;
using System.Collections.Generic;

namespace Decrypt.App.Helpers;

// ============================================================================
// TURK ALFABESI YARDIMCI SINIFI (DECRYPT TARAFI)
// ============================================================================
// Encrypt tarafindaki TurkishAlphabet sinifinin aynisi.
// Decrypt projesinde ayri bir proje oldugu icin burada da tanimlanmasi gerekiyor.
//
// Bu sinif turk alfabesiyle ilgili tum islemleri yapar:
// - Harf -> index cevirimi (ornegin 'A' -> 0, 'Ç' -> 3)
// - Index -> harf cevirimi (ornegin 0 -> 'A', 3 -> 'Ç')
// - Moduler ters hesaplama (Affine cozme icin)
// - EBOB (GCD) hesaplama
// ============================================================================
public static class TurkishAlphabet
{
    // turk alfabesindeki toplam harf sayisi
    public const int N = 29;

    // turk alfabesinin tum harfleri sirali sekilde
    public static readonly string Harfler = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    // eski kodlarla uyumluluk icin
    public static readonly string Letters = Harfler;

    // harf -> index eslemesi icin sozluk
    private static readonly Dictionary<char, int> harfIndex;

    // sinif ilk kullanildiginda calisir, sozlugu doldurur
    static TurkishAlphabet()
    {
        harfIndex = new Dictionary<char, int>(N);
        for (int i = 0; i < Harfler.Length; i++)
            harfIndex[Harfler[i]] = i;
    }

    // harfin alfabedeki indexini dondurur, yoksa -1
    public static int IndexOf(char c)
    {
        if (harfIndex.ContainsKey(c))
            return harfIndex[c];
        return -1;
    }

    // indexten harfi bulur (mod 29 dairesel)
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

    // moduler ters bulma - affine sifre cozme icin lazim
    // a * x = 1 (mod m) saglayan x'i bulur
    public static int ModInverse(int a, int m)
    {
        a = ((a % m) + m) % m;
        for (int x = 1; x < m; x++)
        {
            if ((a * x) % m == 1)
                return x;
        }
        return -1;
    }

    // en buyuk ortak bolen (EBOB) - Euclidean algoritmasi
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
