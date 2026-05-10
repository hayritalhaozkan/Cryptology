using System;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace Encrypt.App.Algorithms;

public class RsaSifrele
{
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Sifrele(string metin, long p, long q, long e)
    {
        if (metin == null || metin.Length == 0) return "";
        BigInteger n = p * q;
        
        string temizMetin = MetniTemizle(metin);
        StringBuilder sb = new StringBuilder();
        
        foreach (char c in temizMetin)
        {
            int index = alfabe.IndexOf(c);
            if (index >= 0)
            {
                BigInteger m = new BigInteger(index);
                BigInteger c_val = BigInteger.ModPow(m, e, n);
                sb.Append(c_val.ToString()).Append(" ");
            }
        }
        return sb.ToString().TrimEnd();
    }

    static string MetniTemizle(string girdi)
    {
        CultureInfo turkKultur = new CultureInfo("tr-TR");
        string buyukHarf = girdi.ToUpper(turkKultur);

        string temiz = "";
        for (int i = 0; i < buyukHarf.Length; i++)
        {
            char c = buyukHarf[i];
            if (alfabe.Contains(c))
            {
                temiz = temiz + c;
            }
        }
        return temiz;
    }
}
