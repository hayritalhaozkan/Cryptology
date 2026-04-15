using System.Globalization;
using System.Text;

namespace Decrypt.App.Algorithms;

// VIGENERE SIFRE COZME
// her harften anahtar kelimedeki karsilik gelen harfin alfabedeki sirasi kadar kaydirma cikarir
public class VigenereCoz
{
    // turk alfabesi - 29 harf (kucuk harfler)
    static string alfabe = "abcçdefgğhıijklmnoöprsştuüvyz";

    public static string Coz(string sifreliMetin, string anahtarMetin)
    {
        string temizMetin = MetniTemizle(sifreliMetin);
        string temizAnahtar = MetniTemizle(anahtarMetin);

        if (temizAnahtar.Length == 0) return temizMetin; // Anahtar yoksa aynen dondur

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
                // anahtarin ilgili harfini bul
                char anahtarHarfi = temizAnahtar[anahtarSirasi % temizAnahtar.Length];
                int anahtarYer = -1;
                for (int j = 0; j < alfabe.Length; j++)
                {
                    if (alfabe[j] == anahtarHarfi)
                    {
                        anahtarYer = j;
                        break;
                    }
                }

                // cozme: geri kaydir
                int yeniYer = (yer - anahtarYer) % 29;
                if (yeniYer < 0) yeniYer = yeniYer + 29;
                sonuc = sonuc + alfabe[yeniYer];

                anahtarSirasi++;
            }
        }

        return sonuc;
    }

    // metni kucuk harfe cevir ve sadece turk alfabesindeki harfleri birak
    static string MetniTemizle(string girdi)
    {
        if (girdi == null || girdi.Length == 0)
            return "";

        CultureInfo turkKultur = new CultureInfo("tr-TR");
        string kucukHarf = girdi.ToLower(turkKultur);

        string temiz = "";
        for (int i = 0; i < kucukHarf.Length; i++)
        {
            char c = kucukHarf[i];
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
