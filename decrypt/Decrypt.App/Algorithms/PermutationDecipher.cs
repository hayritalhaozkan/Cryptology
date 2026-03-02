using System.Globalization;
using System.Text;

namespace Decrypt.App.Algorithms;

// PERMUTASYON SIFRE COZME
// sifreleme harfleri permutasyon sirasina gore YERLESTIRIYORDU
// cozme permutasyon sirasina gore ALIYOR (ters islem)
// sifreleme: yeniBlok[perm[i]-1] = blok[i]  (i. harfi perm pozisyonuna KOY)
// cozme:     yeniBlok[i] = blok[perm[i]-1]   (perm pozisyonundan harfi AL)
public class PermutasyonCoz
{
    // turk alfabesi - 29 harf
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Coz(string sifreliMetin, string permMetin)
    {
        string temizMetin = MetniTemizle(sifreliMetin);

        // permutasyon anahtarini parcala
        string[] parcalar = permMetin.Split(',');
        int[] perm = new int[parcalar.Length];
        for (int i = 0; i < parcalar.Length; i++)
            perm[i] = int.Parse(parcalar[i].Trim());

        int blokBoyutu = perm.Length;

        // metin blok boyutuna tam bolunmuyorsa sonuna A ekle
        while (temizMetin.Length % blokBoyutu != 0)
            temizMetin = temizMetin + "A";

        string sonuc = "";

        // her blok icin TERS permutasyon uygula
        for (int b = 0; b < temizMetin.Length; b = b + blokBoyutu)
        {
            char[] yeniBlok = new char[blokBoyutu];

            for (int i = 0; i < blokBoyutu; i++)
            {
                char harf = temizMetin[b + perm[i] - 1]; // perm pozisyonundaki harfi AL
                yeniBlok[i] = harf;                       // i. pozisyona KOY
            }

            for (int i = 0; i < blokBoyutu; i++)
                sonuc = sonuc + yeniBlok[i];
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
