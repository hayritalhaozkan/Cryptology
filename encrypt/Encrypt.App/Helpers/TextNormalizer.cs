using System.Globalization;
using System.Text;

namespace Encrypt.App.Helpers;

/// <summary>
/// Metni normalize eder:
/// 1. Türkçe kültürle büyük harfe çevirir (i→İ, ı→I vb.)
/// 2. Boşluk ve noktalama kaldırır
/// 3. Yalnızca Türk alfabesindeki harfleri bırakır
/// </summary>
public static class TextNormalizer
{
    private static readonly CultureInfo _trCulture = new("tr-TR");

    public static string Normalize(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // TR kültürüyle büyük harfe çevir
        var upper = input.ToUpper(_trCulture);

        var sb = new StringBuilder(upper.Length);
        foreach (char c in upper)
        {
            if (TurkishAlphabet.Contains(c))
                sb.Append(c);
            // Rakam desteği istersen:
            // else if (c >= '0' && c <= '9') sb.Append(c);
        }

        return sb.ToString();
    }
}
