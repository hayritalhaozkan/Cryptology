using System.Globalization;
using System.Text;

namespace Decrypt.App.Helpers;

/// <summary>
/// Metni normalize eder (Decrypt tarafında da ihtiyaç olabilir).
/// </summary>
public static class TextNormalizer
{
    private static readonly CultureInfo _trCulture = new("tr-TR");

    public static string Normalize(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        var upper = input.ToUpper(_trCulture);

        var sb = new StringBuilder(upper.Length);
        foreach (char c in upper)
        {
            if (TurkishAlphabet.Contains(c))
                sb.Append(c);
        }

        return sb.ToString();
    }
}
