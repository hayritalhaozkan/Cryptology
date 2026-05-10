using System.Globalization;
using System.Text;

namespace Encrypt.App.Algorithms;

// AFFINE SIFRESI - DOGRUSAL SIFRE
// her harfe carpma ve toplama uygular
// formul: sifreli = (a * harf + b) mod 29
// a ile 29 aralarinda asal olmali (EBOB = 1)
public class AffineSifrele
{
    // turk alfabesi - 29 harf
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Sifrele(string metin, int a, int b)
    {
        string temizMetin = MetniTemizle(metin);

        string sonuc = "";

        for (int i = 0; i < temizMetin.Length; i++)
        {
            char harf = temizMetin[i];

            // harfin alfabedeki yerini bul
            int yer = -1;
            for (int j = 0; j < alfabe.Length; j++)
            {
                if (alfabe[j] == harf)
                {
                    yer = j;
                    break;
                }
            }

            if (yer >= 0)
            {
                // affine formulu: (a * yer + b) mod 29
                int yeniYer = (a * yer + b) % 29;
                if (yeniYer < 0) yeniYer = yeniYer + 29;
                sonuc = sonuc + alfabe[yeniYer];
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
