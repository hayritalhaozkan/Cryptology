using System.Globalization;
using System.Text;

namespace Encrypt.App.Helpers;

public static class TextNormalizer
{
    // turk kulturu
    private static readonly CultureInfo trKultur = new("tr-TR");

    // metni normalize et: buyuk harfe cevir, sadece turk alfabesindeki harfleri birak
    public static string Normalize(string girdi)
    {
        if (string.IsNullOrEmpty(girdi))
            return "";

        // buyuk harfe cevir
        string buyukHarf = girdi.ToUpper(trKultur);

        // sadece turk alfabesindeki harfleri al
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
