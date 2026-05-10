using System;
using System.Globalization;
using System.Text;

namespace Encrypt.App.Algorithms;

public class HillSifrele
{
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Sifrele(string metin, int m1, int m2, int m3, int m4, int m5, int m6, int m7, int m8, int m9)
    {
        // Metni temizle ve büyük harfe çevir
        string temizMetin = MetniTemizle(metin);
        
        // Hill şifresi için metin uzunluğu 3'ün katı olmalı (3x3 matris için)
        // Eğer değilse sonuna 'A' (indeks 0) ekleyelim
        while (temizMetin.Length % 3 != 0)
            temizMetin += "A";

        StringBuilder sonuc = new StringBuilder();

        for (int k = 0; k < temizMetin.Length; k += 3)
        {
            int p1 = alfabe.IndexOf(temizMetin[k]);
            int p2 = alfabe.IndexOf(temizMetin[k + 1]);
            int p3 = alfabe.IndexOf(temizMetin[k + 2]);

            // C = (P * A) mod 29
            // C1 = (p1*m1 + p2*m4 + p3*m7) % 29
            // C2 = (p1*m2 + p2*m5 + p3*m8) % 29
            // C3 = (p1*m3 + p2*m6 + p3*m9) % 29
            
            int c1 = (p1 * m1 + p2 * m4 + p3 * m7) % 29;
            int c2 = (p1 * m2 + p2 * m5 + p3 * m8) % 29;
            int c3 = (p1 * m3 + p2 * m6 + p3 * m9) % 29;

            if (c1 < 0) c1 += 29;
            if (c2 < 0) c2 += 29;
            if (c3 < 0) c3 += 29;

            sonuc.Append(alfabe[c1]);
            sonuc.Append(alfabe[c2]);
            sonuc.Append(alfabe[c3]);
        }

        return sonuc.ToString();
    }

    static string MetniTemizle(string girdi)
    {
        if (string.IsNullOrEmpty(girdi)) return "";
        CultureInfo turkKultur = new CultureInfo("tr-TR");
        string buyukHarf = girdi.ToUpper(turkKultur);
        StringBuilder temiz = new StringBuilder();
        foreach (char ch in buyukHarf)
        {
            if (alfabe.Contains(ch))
                temiz.Append(ch);
        }
        return temiz.ToString();
    }
}
