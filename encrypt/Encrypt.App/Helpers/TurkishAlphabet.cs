using System;
using System.Collections.Generic;

namespace Encrypt.App.Helpers;

// ============================================================================
// TURK ALFABESI YARDIMCI SINIFI
// ============================================================================
// Bu sinif turk alfabesiyle ilgili tum islemleri yapar.
// Turk alfabesi 29 harften olusur: A B C Ç D E F G Ğ H I İ J K L M N O Ö P R S Ş T U Ü V Y Z
// Ingiliz alfabesinden farkli olarak Q, W, X harfleri yoktur.
// Ç, Ğ, I, İ, Ö, Ş, Ü harfleri ise sadece turk alfabesinde vardir.
//
// Bu sinif butun sifreleme algoritmalarinin temelini olusturur cunku
// her algoritma harfleri sayilara cevirip matematiksel islem yapar,
// sonra tekrar harfe cevirir. Bu sinif o cevirme isini yapar.
// ============================================================================
public static class TurkishAlphabet
{
    // turk alfabesindeki toplam harf sayisi
    // butun mod islemlerinde bu sayi kullanilir (mod 29)
    public const int N = 29;

    // turk alfabesinin tum harfleri sirali sekilde
    // index 0 = A, index 1 = B, index 2 = C, ... index 28 = Z
    public static readonly string Harfler = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    // eski kodlarla uyumluluk icin ayni seyi Letters olarak da tutuyoruz
    public static readonly string Letters = Harfler;

    // her harfin hangi indexte oldugunu hizli bulmak icin sozluk (dictionary)
    // ornegin: 'A' -> 0, 'B' -> 1, 'Ç' -> 3, 'Z' -> 28
    // sozluk kullanmamizin sebebi: her seferinde dongu ile aramak yerine
    // direkt harfi verip indexi almak cok daha hizli
    private static readonly Dictionary<char, int> harfIndex;

    // static constructor - sinif ilk kullanildiginda bir kez calisir
    // butun harfleri sozluge ekler
    static TurkishAlphabet()
    {
        harfIndex = new Dictionary<char, int>(N);
        for (int i = 0; i < Harfler.Length; i++)
            harfIndex[Harfler[i]] = i;
        // bu dongu bittiginde sozluk su sekilde olur:
        // { 'A': 0, 'B': 1, 'C': 2, 'Ç': 3, 'D': 4, ... 'Z': 28 }
    }

    // verilen harfin turk alfabesindeki sirasini (indexini) bulur
    // ornegin: 'A' icin 0 dondurur, 'D' icin 4 dondurur
    // eger harf turk alfabesinde yoksa -1 dondurur
    public static int IndexOf(char c)
    {
        if (harfIndex.ContainsKey(c))
            return harfIndex[c];
        return -1;
    }

    // verilen index numarasindan harfi bulur
    // mod 29 islemi yapar, boylece 29 verirsen tekrar A olur (dairesel)
    // ornegin: index=0 -> 'A', index=3 -> 'Ç', index=29 -> 'A' (mod 29 = 0)
    // negatif sayilar icin de calisir: index=-1 -> 'Z' (son harf)
    public static char CharAt(int index)
    {
        // ((index % N) + N) % N formuluyle negatif sayilari da pozitife ceviriyoruz
        // ornek: index = -1 ise: (-1 % 29) = -1, (-1 + 29) = 28, 28 % 29 = 28 -> 'Z'
        int sonuc = ((index % N) + N) % N;
        return Harfler[sonuc];
    }

    // verilen harf turk alfabesinde var mi diye kontrol eder
    // ornegin: 'A' icin true, 'Q' icin false dondurur
    public static bool Contains(char c)
    {
        return harfIndex.ContainsKey(c);
    }

    // MODULER TERS BULMA (Affine sifre cozme icin gerekli)
    // 
    // moduler ters nedir?
    // a * x = 1 (mod m) denklemini saglayan x degerini bulur
    // ornegin: a=2, m=29 icin -> 2 * 15 = 30, 30 mod 29 = 1 -> x = 15
    // yani 2'nin mod 29 tersi 15'tir
    //
    // neden lazim?
    // affine sifreleme: y = (a*x + b) mod 29
    // affine sifre cozme: x = a_tersi * (y - b) mod 29
    // cozme icin a'nin tersini bilmek gerekir
    //
    // nasil calisir?
    // 1'den m-1'e kadar tum sayilari dener
    // hangisi (a * x) % m == 1 kosulunu saglarsa onu dondurur
    public static int ModInverse(int a, int m)
    {
        a = ((a % m) + m) % m;
        for (int x = 1; x < m; x++)
        {
            if ((a * x) % m == 1)
                return x;
        }
        return -1; // ters bulunamadi (a ve m aralarinda asal degil demek)
    }

    // EN BUYUK ORTAK BOLEN (EBOB / GCD) HESAPLAMA
    // 
    // Euclidean algoritmasi kullanilir
    // iki sayinin en buyuk ortak bolenini bulur
    // ornegin: Gcd(12, 8) = 4, Gcd(7, 29) = 1
    //
    // neden lazim?
    // affine sifrede a degeri ile 29 aralarinda asal olmali
    // yani Gcd(a, 29) = 1 olmali
    // yoksa sifreleme duzgun calismaz (bazi harfler ayni harfe donusur)
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
        // ornek: Gcd(12, 8):
        // 1. tur: a=12, b=8 -> gecici=8, b=12%8=4, a=8
        // 2. tur: a=8, b=4 -> gecici=4, b=8%4=0, a=4
        // 3. tur: b=0 -> dongu biter, sonuc = a = 4
    }
}
