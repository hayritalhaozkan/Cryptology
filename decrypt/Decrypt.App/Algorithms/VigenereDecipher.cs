using System.Globalization;
using System.Text;

namespace Decrypt.App.Algorithms;

// VIGENERE SIFRE COZME
// sifreleme her harfe farkli kaydirma EKLIYORDU
// cozme ayni kaydirmayi CIKARIR
// formul: cozulmus = (harf - anahtar) mod 29
public class VigenereCoz
{
    // turk alfabesi - 29 harf
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Coz(string sifreliMetin, string anahtarMetin)
    {
        string temizMetin = MetniTemizle(sifreliMetin);

        // anahtari virgullerden bol ve sayilara cevir
        string[] parcalar = anahtarMetin.Split(',');
        int[] anahtarSayilari = new int[parcalar.Length];
        int anahtarUzunluk = 0;
        for (int i = 0; i < parcalar.Length; i++)
        {
            string parca = parcalar[i].Trim();
            if (parca.Length > 0)
            {
                anahtarSayilari[anahtarUzunluk] = int.Parse(parca);
                anahtarUzunluk++;
            }
        }

        string sonuc = "";
        int anahtarSirasi = 0;

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
                // cozme: geri kaydir (sifreleme + yapmisti, cozme - yapar)
                int kaydirma = anahtarSayilari[anahtarSirasi % anahtarUzunluk];
                int yeniYer = (yer - kaydirma) % 29;
                if (yeniYer < 0) yeniYer = yeniYer + 29;
                sonuc = sonuc + alfabe[yeniYer];

                anahtarSirasi++;
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
