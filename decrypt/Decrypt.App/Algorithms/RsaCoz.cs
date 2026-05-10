using System;
using System.Numerics;
using System.Text;

namespace Decrypt.App.Algorithms;

public class RsaCoz
{
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Coz(string sifreliMetin, long p, long q, long e)
    {
        if (string.IsNullOrWhiteSpace(sifreliMetin)) return "";
        
        BigInteger n = p * q;
        BigInteger phi = (p - 1) * (q - 1);
        BigInteger d = ModInverse(e, phi);

        string[] parcalar = sifreliMetin.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        StringBuilder sb = new StringBuilder();
        
        foreach (string parca in parcalar)
        {
            if (BigInteger.TryParse(parca, out BigInteger c_val))
            {
                BigInteger m = BigInteger.ModPow(c_val, d, n);
                int index = (int)m;
                if (index >= 0 && index < alfabe.Length)
                {
                    sb.Append(alfabe[index]);
                }
                else
                {
                    sb.Append("?");
                }
            }
        }
        return sb.ToString();
    }

    private static BigInteger ModInverse(BigInteger a, BigInteger m)
    {
        BigInteger m0 = m;
        BigInteger y = 0, x = 1;

        if (m == 1)
            return 0;

        while (a > 1)
        {
            BigInteger q = a / m;
            BigInteger t = m;

            m = a % m;
            a = t;
            t = y;

            y = x - q * y;
            x = t;
        }

        if (x < 0)
            x += m0;

        return x;
    }
}
