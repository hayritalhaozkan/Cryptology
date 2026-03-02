using System.Globalization;
using System.Text;

namespace Encrypt.App.Algorithms;

// VIGENERE SIFRESI - SAYI ANAHTARLI SIFRE
// her harfe farkli bir kaydirma uygular
// anahtar: virgul ile ayrilmis sayilar, ornegin 3,7,1
// 1. harfe 3, 2. harfe 7, 3. harfe 1 kaydirma uygulanir
// anahtar bitince basa doner
public class VigenereSifrele
{
    // turk alfabesi - 29 harf
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Sifrele(string metin, string anahtarMetin)
    {
        string temizMetin = MetniTemizle(metin);

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
        int anahtarSirasi = 0; // anahtarin hangi elemanindayiz

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
                // simdiki anahtar degerini al
                int kaydirma = anahtarSayilari[anahtarSirasi % anahtarUzunluk];

                // harfi kaydir
                int yeniYer = (yer + kaydirma) % 29;
                if (yeniYer < 0) yeniYer = yeniYer + 29;
                sonuc = sonuc + alfabe[yeniYer];

                anahtarSirasi++;
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
