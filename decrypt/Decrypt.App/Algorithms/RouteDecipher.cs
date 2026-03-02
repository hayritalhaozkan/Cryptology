using System.Globalization;
using System.Text;

namespace Decrypt.App.Algorithms;

// ROTA SIFRE COZME
// sifreleme: tabloya SATIR SATIR yazildi, SUTUN SUTUN okundu
// cozme: tabloya SUTUN SUTUN yazilir, SATIR SATIR okunur (tam ters)
public class RotaCoz
{
    // turk alfabesi - 29 harf
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Coz(string sifreliMetin, int satirSayisi, int sutunSayisi)
    {
        string temizMetin = MetniTemizle(sifreliMetin);

        int tabloBoyu = satirSayisi * sutunSayisi;

        // metin kisaysa A ile doldur
        while (temizMetin.Length < tabloBoyu)
            temizMetin = temizMetin + "A";

        // metin uzunsa kes
        if (temizMetin.Length > tabloBoyu)
            temizMetin = temizMetin.Substring(0, tabloBoyu);

        // sifreleme SUTUN SUTUN okumustu
        // cozme icin SUTUN SUTUN yaziyoruz (tam ters)
        char[,] tablo = new char[satirSayisi, sutunSayisi];
        int sayac = 0;
        for (int st = 0; st < sutunSayisi; st++)
        {
            for (int s = 0; s < satirSayisi; s++)
            {
                tablo[s, st] = temizMetin[sayac];
                sayac++;
            }
        }

        // tabloyu SATIR SATIR oku -> orijinal metin
        string sonuc = "";
        for (int s = 0; s < satirSayisi; s++)
        {
            for (int st = 0; st < sutunSayisi; st++)
            {
                sonuc = sonuc + tablo[s, st];
            }
        }

        return sonuc;
    }

    static string MetniTemizle(string girdi)
    {
        if (girdi == null || girdi.Length == 0)
            return "";

        CultureInfo turkKultur = new CultureInfo("tr-TR");
        string buyukHarf = girdi.ToUpper(turkKultur);

        string temiz = "";
        for (int i = 0; i < buyukHarf.Length; i++)
        {
            char c = buyukHarf[i];
            bool var = false;
            for (int j = 0; j < alfabe.Length; j++)
            {
                if (alfabe[j] == c)
                {
                    var = true;
                    break;
                }
            }
            if (var)
                temiz = temiz + c;
        }
        return temiz;
    }
}
