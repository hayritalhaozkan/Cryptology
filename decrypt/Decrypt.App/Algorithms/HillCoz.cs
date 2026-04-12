using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Decrypt.App.Algorithms;

public class HillCoz
{
    static string alfabe = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

    public static string Coz(string metin, int m1, int m2, int m3, int m4, int m5, int m6, int m7, int m8, int m9)
    {
        int[,] keyMatrix = { { m1, m2, m3 }, { m4, m5, m6 }, { m7, m8, m9 } };

        // 1. Determinant hesapla
        int det = GetDeterminant3x3(keyMatrix);
        int detMod = Mod29(det);

        // 2. Determinantın Mod 29'daki tersini bul
        int detTersi = ModuloInverse(detMod, 29);
        if (detTersi == -1)
            throw new Exception("Matrisin determinantının Mod 29'da tersi bulunamadı. Matris terslenemez!");

        // 3. Ters matrisi hesapla (Adjugate * detTersi) mod 29
        int[,] invMatrix = GetInverseMatrix3x3(keyMatrix, detTersi);

        // 4. Çözme işlemi
        StringBuilder sonuc = new StringBuilder();
        for (int k = 0; k < metin.Length; k += 3)
        {
            if (k + 2 >= metin.Length) break;

            int c1 = alfabe.IndexOf(metin[k]);
            int c2 = alfabe.IndexOf(metin[k + 1]);
            int c3 = alfabe.IndexOf(metin[k + 2]);

            if (c1 == -1 || c2 == -1 || c3 == -1) continue;

            // P = C * A^-1 mod 29
            int p1 = (c1 * invMatrix[0, 0] + c2 * invMatrix[1, 0] + c3 * invMatrix[2, 0]) % 29;
            int p2 = (c1 * invMatrix[0, 1] + c2 * invMatrix[1, 1] + c3 * invMatrix[2, 1]) % 29;
            int p3 = (c1 * invMatrix[0, 2] + c2 * invMatrix[1, 2] + c3 * invMatrix[2, 2]) % 29;

            if (p1 < 0) p1 += 29;
            if (p2 < 0) p2 += 29;
            if (p3 < 0) p3 += 29;

            sonuc.Append(alfabe[p1]);
            sonuc.Append(alfabe[p2]);
            sonuc.Append(alfabe[p3]);
        }

        return sonuc.ToString();
    }

    private static int Mod29(int value)
    {
        return (value % 29 + 29) % 29;
    }

    private static int GetDeterminant3x3(int[,] mat)
    {
        return mat[0, 0] * (mat[1, 1] * mat[2, 2] - mat[1, 2] * mat[2, 1]) -
               mat[0, 1] * (mat[1, 0] * mat[2, 2] - mat[1, 2] * mat[2, 0]) +
               mat[0, 2] * (mat[1, 0] * mat[2, 1] - mat[1, 1] * mat[2, 0]);
    }

    private static int ModuloInverse(int n, int mod)
    {
        n %= mod;
        for (int x = 1; x < mod; x++)
        {
            if ((n * x) % mod == 1)
                return x;
        }
        return -1;
    }

    private static int[,] GetInverseMatrix3x3(int[,] matrix, int detInverse)
    {
        int[,] adjugate = new int[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                adjugate[i, j] = Mod29(GetMinor3x3(matrix, j, i) * (int)Math.Pow(-1, i + j) * detInverse);
            }
        }
        return adjugate;
    }

    private static int GetMinor3x3(int[,] matrix, int row, int col)
    {
        List<int> elements = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i != row && j != col)
                    elements.Add(matrix[i, j]);
            }
        }
        return elements[0] * elements[3] - elements[1] * elements[2];
    }
}
