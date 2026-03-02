using System.Globalization;
using System.Text;

namespace Decrypt.App.Algorithms;

// ZIGZAG SIFRE COZME - RAIL FENCE DECIPHER
// en karmasik cozme algoritmasi
// 3 adimda cozulur:
// 1. her raya kac harf dustugunu hesapla
// 2. sifreli metni raylara dagit
// 3. zigzag sirasinda raylarden harfleri oku
public class ZigzagCoz
{
    // turk alfabesi - 29 harf
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Coz(string sifreliMetin, int raySayisi)
    {
        string temizMetin = MetniTemizle(sifreliMetin);
        int uzunluk = temizMetin.Length;

        if (uzunluk == 0)
            return "";

        if (raySayisi == 1 || raySayisi >= uzunluk)
            return temizMetin;

        // ADIM 1: her raya kac harf dustugunu hesapla
        // zigzag desenini simule et ama harf yazmadan sadece say
        int[] rayUzunluklari = new int[raySayisi];
        int ray = 0;
        bool asagi = true;

        for (int i = 0; i < uzunluk; i++)
        {
            rayUzunluklari[ray]++; // bu raya bir harf daha dustu

            // yon degistir
            if (ray == 0) asagi = true;
            else if (ray == raySayisi - 1) asagi = false;

            if (asagi) ray++;
            else ray--;
        }

        // ADIM 2: sifreli metni raylara dagit
        // sifreli metinde once ray 0'in harfleri, sonra ray 1, sonra ray 2 ...
        string[] raylar = new string[raySayisi];
        int pozisyon = 0;

        for (int r = 0; r < raySayisi; r++)
        {
            raylar[r] = temizMetin.Substring(pozisyon, rayUzunluklari[r]);
            pozisyon = pozisyon + rayUzunluklari[r];
        }

        // ADIM 3: zigzag sirasinda raylarden harfleri oku
        int[] rayIndexleri = new int[raySayisi]; // her rayin kacinci harfindeyiz
        string sonuc = "";

        ray = 0;
        asagi = true;

        for (int i = 0; i < uzunluk; i++)
        {
            // mevcut raydan siradaki harfi al
            sonuc = sonuc + raylar[ray][rayIndexleri[ray]];
            rayIndexleri[ray]++;

            // yon degistir
            if (ray == 0) asagi = true;
            else if (ray == raySayisi - 1) asagi = false;

            if (asagi) ray++;
            else ray--;
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
