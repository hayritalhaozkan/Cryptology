using System.Globalization;
using System.Text;

namespace Decrypt.App.Algorithms;

// SUBSTITUTION SIFRE COZME
// sifreleme: normal alfabe -> anahtar alfabe
// cozme: anahtar alfabe -> normal alfabe (ters islem)
// sifreli metindeki harfi anahtar alfabede bul, onun sirasindaki normal alfabe harfini yaz
public class SubstitutionCoz
{
    // turk alfabesi - 29 harf
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Coz(string sifreliMetin, string anahtarAlfabe)
    {
        string temizMetin = MetniTemizle(sifreliMetin);
        string temizAnahtar = MetniTemizle(anahtarAlfabe);

        string sonuc = "";

        for (int i = 0; i < temizMetin.Length; i++)
        {
            char harf = temizMetin[i];

            // bu harfi anahtar alfabede bul
            int yer = -1;
            for (int j = 0; j < temizAnahtar.Length; j++)
            {
                if (temizAnahtar[j] == harf)
                {
                    yer = j;
                    break;
                }
            }

            if (yer >= 0)
            {
                // anahtar alfabedeki sira = normal alfabedeki sira
                sonuc = sonuc + alfabe[yer];
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
