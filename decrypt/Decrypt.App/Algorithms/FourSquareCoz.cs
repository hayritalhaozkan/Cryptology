using System.Globalization;
using System.Text;

namespace Decrypt.App.Algorithms;

public class FourSquareCoz
{
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZX";

    public static string Coz(string metin, string anahtar1, string anahtar2)
    {
        string temizMetin = MetniTemizle(metin);
        if (temizMetin.Length % 2 != 0)
        {
            throw new System.Exception("Şifreli metin uzunluğu çift olmalıdır.");
        }

        string matris1 = alfabe;
        string matris4 = alfabe;
        string matris2 = MatrisOlustur(anahtar1);
        string matris3 = MatrisOlustur(anahtar2);

        string sonuc = "";

        for (int i = 0; i < temizMetin.Length; i += 2)
        {
            char c1 = temizMetin[i];
            char c2 = temizMetin[i + 1];

            int idx1 = matris2.IndexOf(c1);
            int idx2 = matris3.IndexOf(c2);

            if (idx1 == -1 || idx2 == -1)
            {
                sonuc += c1.ToString() + c2.ToString();
                continue;
            }

            int r1 = idx1 / 5;
            int col2 = idx1 % 5;

            int r2 = idx2 / 5;
            int col1 = idx2 % 5;

            char m1 = matris1[r1 * 5 + col1];
            char m2 = matris4[r2 * 5 + col2];

            sonuc += m1.ToString() + m2.ToString();
        }

        return sonuc;
    }

    static string MatrisOlustur(string anahtar)
    {
        string temiz = MetniTemizle(anahtar);
        string matris = "";

        for (int i = 0; i < temiz.Length; i++)
        {
            char c = temiz[i];
            if (!matris.Contains(c))
            {
                matris += c;
            }
        }

        if (matris.Length != 30)
        {
            throw new System.Exception("Matris anahtarı tam olarak 30 farklı harf içermelidir. Eksik veya fazla harf var: " + matris.Length);
        }

        return matris;
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
            if (alfabe.Contains(c))
            {
                temiz += c;
            }
        }
        return temiz;
    }
}
