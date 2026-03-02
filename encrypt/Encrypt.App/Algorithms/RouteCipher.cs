using System.Globalization;
using System.Text;

namespace Encrypt.App.Algorithms;

// ROTA SIFRESI - ROUTE CIPHER
// metin bir tabloya satir satir yazilir, sonra sutun sutun okunur
// anahtar: satir sayisi ve sutun sayisi
// ornegin 3 satir 4 sutun:
//   M E R H
//   A B A D
//   U N Y A
// sutun sutun okunur: M A U  E B N  R A Y  H D A -> "MAUEBNRAYHDA"
public class RotaSifrele
{
    // turk alfabesi - 29 harf
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Sifrele(string metin, int satirSayisi, int sutunSayisi)
    {
        string temizMetin = MetniTemizle(metin);

        int tabloBoyu = satirSayisi * sutunSayisi;

        // metin kisaysa A ile doldur
        while (temizMetin.Length < tabloBoyu)
            temizMetin = temizMetin + "A";

        // metin uzunsa kes
        if (temizMetin.Length > tabloBoyu)
            temizMetin = temizMetin.Substring(0, tabloBoyu);

        // tabloyu olustur ve metni satir satir yaz
        char[,] tablo = new char[satirSayisi, sutunSayisi];
        int sayac = 0;
        for (int s = 0; s < satirSayisi; s++)
        {
            for (int st = 0; st < sutunSayisi; st++)
            {
                tablo[s, st] = temizMetin[sayac];
                sayac++;
            }
        }

        // tabloyu sutun sutun oku
        string sonuc = "";
        for (int st = 0; st < sutunSayisi; st++)
        {
            for (int s = 0; s < satirSayisi; s++)
            {
                sonuc = sonuc + tablo[s, st];
            }
        }

        return sonuc;
    }

    // metni buyuk harfe cevir ve sadece turk alfabesindeki harfleri birak
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
