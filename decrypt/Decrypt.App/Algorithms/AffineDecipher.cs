using System.Globalization;
using System.Text;

namespace Decrypt.App.Algorithms;

// AFFINE SIFRE COZME
// sifreleme: y = (a * x + b) mod 29
// cozme:     x = a_tersi * (y - b) mod 29
// a_tersi: a * a_tersi = 1 (mod 29) olan sayi
public class AffineCoz
{
    // turk alfabesi - 29 harf
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Coz(string sifreliMetin, int a, int b)
    {
        string temizMetin = MetniTemizle(sifreliMetin);

        // a'nin moduler tersini bul
        // yani a * ters = 1 (mod 29) olan ters degerini bul
        int aTersi = ModulerTersBul(a, 29);

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
                // cozme formulu: x = aTersi * (y - b) mod 29
                int cozulmus = (aTersi * (yer - b)) % 29;
                if (cozulmus < 0) cozulmus = cozulmus + 29;
                sonuc = sonuc + alfabe[cozulmus];
            }
        }

        return sonuc;
    }

    // moduler ters bulma
    // a * x = 1 (mod m) saglayan x degerini bulur
    // 1den m-1e kadar tum sayilari dener
    static int ModulerTersBul(int a, int m)
    {
        a = ((a % m) + m) % m;
        for (int x = 1; x < m; x++)
        {
            if ((a * x) % m == 1)
                return x;
        }
        return -1; // ters bulunamadi
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
