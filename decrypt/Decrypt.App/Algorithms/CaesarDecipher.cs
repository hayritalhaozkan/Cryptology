using System.Globalization;
using System.Text;

namespace Decrypt.App.Algorithms;

// CAESAR SIFRE COZME
// sifreleme ileri kaydiriyordu, cozme geri kaydirir
// formul: cozulmus = (harf - k) mod 29
public class CaesarCoz
{
    // turk alfabesi - 29 harf
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Coz(string sifreliMetin, int kaydirma)
    {
        string temizMetin = MetniTemizle(sifreliMetin);

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
                // geri kaydir (sifreleme + yapmisti, cozme - yapar)
                int yeniYer = (yer - kaydirma) % 29;
                if (yeniYer < 0) yeniYer = yeniYer + 29;
                sonuc = sonuc + alfabe[yeniYer];
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