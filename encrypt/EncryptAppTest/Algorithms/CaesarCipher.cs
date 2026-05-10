using System.Globalization;
using System.Text;

namespace Encrypt.App.Algorithms;

// CAESAR SIFRESI - KAYDIRMALI SIFRE
// her harfi k kadar ileri kaydirir
// formul: sifreli = (harf + k) mod 29
public class CaesarSifrele
{
    // turk alfabesi - 29 harf
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Sifrele(string metin, int kaydirma)
    {
        // oncelik metni buyuk harfe cevir ve sadece alfabe harflerini al
        string temizMetin = MetniTemizle(metin);

        string sonuc = "";

        // her harfi tek tek isle
        for (int i = 0; i < temizMetin.Length; i++)
        {
            char harf = temizMetin[i];

            // bu harfin alfabedeki yerini bul
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
                // harfi kaydirma kadar ileri kaydir
                int yeniYer = (yer + kaydirma) % 29;
                // negatif olursa duzelt
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

        // turkce buyuk harfe cevir
        CultureInfo turkKultur = new CultureInfo("tr-TR");
        string buyukHarf = girdi.ToUpper(turkKultur);

        string temiz = "";
        for (int i = 0; i < buyukHarf.Length; i++)
        {
            char c = buyukHarf[i];
            // bu harf alfabede var mi kontrol et
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