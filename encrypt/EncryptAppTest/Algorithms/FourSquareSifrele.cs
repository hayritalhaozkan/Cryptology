using System.Globalization;
using System.Text;

namespace Encrypt.App.Algorithms;

public class FourSquareSifrele
{
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZX";

    public static string Sifrele(string metin, string anahtar1, string anahtar2)
    {
        string temizMetin = MetniTemizle(metin);
        if (temizMetin.Length % 2 != 0)
        {
            temizMetin += "X";
        }

        string matris1 = alfabe;
        string matris4 = alfabe;
        string matris2 = MatrisOlustur(anahtar1);
        string matris3 = MatrisOlustur(anahtar2);

        string sonuc = "";

        for (int i = 0; i < temizMetin.Length; i += 2)
        {
            char m1 = temizMetin[i];
            char m2 = temizMetin[i + 1];

            int idx1 = matris1.IndexOf(m1);
            int idx2 = matris4.IndexOf(m2);

            int r1 = idx1 / 5;
            int c1 = idx1 % 5;

            int r2 = idx2 / 5;
            int c2 = idx2 % 5;

            char g1 = matris2[r1 * 5 + c2];
            char g2 = matris3[r2 * 5 + c1];

            sonuc += g1.ToString() + g2.ToString();
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
