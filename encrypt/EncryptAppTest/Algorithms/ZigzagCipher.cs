using System.Globalization;
using System.Text;

namespace Encrypt.App.Algorithms;

// ZIGZAG SIFRESI - RAIL FENCE
// metin zigzag seklinde raylara yazilir, sonra ray ray okunur
// anahtar: ray sayisi
// ornegin 3 rayla "MERHABADUNYA":
//   ray 0: M . . . A . . . U . . .
//   ray 1: . E . H . B . D . N . A
//   ray 2: . . R . . . A . . . Y .
// ray ray okunur: MAU + EHBDNA + RAY = "MAUEHBDNARAY"
public class ZigzagSifrele
{
    // turk alfabesi - 29 harf
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Sifrele(string metin, int raySayisi)
    {
        string temizMetin = MetniTemizle(metin);

        // bos metin
        if (temizMetin.Length == 0)
            return "";

        // ray 1 ise veya ray sayisi metin uzunlugundan buyukse metin ayni kalir
        if (raySayisi == 1 || raySayisi >= temizMetin.Length)
            return temizMetin;

        // her ray icin bos string olustur
        string[] raylar = new string[raySayisi];
        for (int i = 0; i < raySayisi; i++)
            raylar[i] = "";

        int simdikiRay = 0;    // su an hangi raydayiz
        bool asagiMi = true;   // asagi mi iniyoruz yukari mi cikiyoruz

        // her harfi zigzag sirasinda ilgili raya yaz
        for (int i = 0; i < temizMetin.Length; i++)
        {
            // harfi simdiki raya ekle
            raylar[simdikiRay] = raylar[simdikiRay] + temizMetin[i];

            // yon degistir
            if (simdikiRay == 0)
                asagiMi = true;   // en ustteyiz, artik asagi
            else if (simdikiRay == raySayisi - 1)
                asagiMi = false;  // en alttayiz, artik yukari

            // bir sonraki raya gec
            if (asagiMi)
                simdikiRay++;
            else
                simdikiRay--;
        }

        // tum raylari birlestir
        string sonuc = "";
        for (int i = 0; i < raySayisi; i++)
            sonuc = sonuc + raylar[i];

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
