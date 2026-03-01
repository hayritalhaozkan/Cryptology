using System.Globalization;
using System.Text;

namespace Decrypt.App.Helpers;

// ============================================================================
// METIN NORMALIZASYON SINIFI (DECRYPT TARAFI)
// ============================================================================
// Encrypt tarafindaki TextNormalizer'in aynisi.
// Gelen metni buyuk harfe cevirir ve sadece turk alfabesi harflerini birakir.
// Decrypt tarafinda da gerekli cunku sifreli metinde bosluk/noktalama olabilir.
// ============================================================================
public static class TextNormalizer
{
    // turk kulturu - buyuk/kucuk harf cevirimi icin
    private static readonly CultureInfo trKultur = new("tr-TR");

    // metni normalize et
    public static string Normalize(string girdi)
    {
        if (string.IsNullOrEmpty(girdi))
            return "";

        // turk kulturu ile buyuk harfe cevir
        string buyukHarf = girdi.ToUpper(trKultur);

        // sadece turk alfabesindeki harfleri filtrele
        var sonuc = new StringBuilder();
        for (int i = 0; i < buyukHarf.Length; i++)
        {
            char c = buyukHarf[i];
            if (TurkishAlphabet.Contains(c))
                sonuc.Append(c);
        }

        return sonuc.ToString();
    }
}
