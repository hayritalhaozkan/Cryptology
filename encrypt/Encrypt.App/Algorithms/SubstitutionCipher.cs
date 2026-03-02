using System.Globalization;
using System.Text;

namespace Encrypt.App.Algorithms;

// YER DEGISTIRME SIFRESI - SUBSTITUTION
// anahtar olarak karisik bir alfabe kullanilir (29 harf)
// her harf, anahtar alfabedeki ayni siradaki harfle degistirilir
// ornegin: A -> anahtar[0], B -> anahtar[1], ...
public class SubstitutionSifrele
{
    // turk alfabesi - 29 harf
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Sifrele(string metin, string anahtarAlfabe)
    {
        string temizMetin = MetniTemizle(metin);
        string temizAnahtar = MetniTemizle(anahtarAlfabe);

        string sonuc = "";

        for (int i = 0; i < temizMetin.Length; i++)
        {
            char harf = temizMetin[i];

            // harfin normal alfabedeki yerini bul
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
                // anahtar alfabeden ayni siradaki harfi al
                sonuc = sonuc + temizAnahtar[yer];
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
