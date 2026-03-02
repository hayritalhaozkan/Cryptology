using System.Globalization;
using System.Text;

namespace Encrypt.App.Algorithms;

// PERMUTASYON SIFRESI - BLOK YER DEGISTIRME
// harfleri degistirmez, sadece yerlerini degistirir
// metin bloklara bolunur, her bloktaki harfler permutasyon sirasina gore yeniden dizilir
// anahtar: virgul ile ayrilmis sayilar, ornegin 3,1,4,2
// 1. harf 3. yere, 2. harf 1. yere, 3. harf 4. yere, 4. harf 2. yere gider
public class PermutasyonSifrele
{
    // turk alfabesi - 29 harf
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Sifrele(string metin, string permMetin)
    {
        string temizMetin = MetniTemizle(metin);

        // permutasyon anahtarini parcala
        // ornegin "3,1,4,2" -> [3, 1, 4, 2]
        string[] parcalar = permMetin.Split(',');
        int[] perm = new int[parcalar.Length];
        for (int i = 0; i < parcalar.Length; i++)
            perm[i] = int.Parse(parcalar[i].Trim());

        int blokBoyutu = perm.Length;

        // metin blok boyutuna tam bolunmuyorsa sonuna A ekle
        while (temizMetin.Length % blokBoyutu != 0)
            temizMetin = temizMetin + "A";

        string sonuc = "";

        // metni blok blok isle
        for (int b = 0; b < temizMetin.Length; b = b + blokBoyutu)
        {
            // bu bloktaki harfleri al
            char[] yeniBlok = new char[blokBoyutu];

            for (int i = 0; i < blokBoyutu; i++)
            {
                char harf = temizMetin[b + i];
                // bu harf perm[i]. pozisyona gidecek
                // -1 cunku perm 1den basliyor ama dizi 0dan
                yeniBlok[perm[i] - 1] = harf;
            }

            // yeni bloku sonuca ekle
            for (int i = 0; i < blokBoyutu; i++)
                sonuc = sonuc + yeniBlok[i];
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
