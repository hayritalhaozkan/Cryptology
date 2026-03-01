using System.Globalization;
using System.Text;

namespace Encrypt.App.Helpers;

// ============================================================================
// METIN NORMALIZASYON SINIFI
// ============================================================================
// Bu sinifin gorevi: kullanicinin girdigi metni sifrelemeye hazir hale getirmek.
//
// Kullanici "Merhaba Dünya!" yazabilir ama sifreleme algoritmasi sadece
// buyuk harflerle ve sadece turk alfabesindeki harflerle calisir.
//
// Bu sinif su islemleri yapar:
// 1. Tum harfleri BUYUK HARFE cevirir (turk kulturu kurallarina gore)
//    - 'i' harfi 'İ' olur (ingilizce'de 'I' olurdu ama turkcede 'İ')
//    - 'ı' harfi 'I' olur
// 2. Bosluklari, noktalama isaretlerini ve rakamlari kaldirir
// 3. Sadece turk alfabesinde olan harfleri birakir
//
// Ornek: "Merhaba Dünya! 123" -> "MERHABADÜNYA"
// ============================================================================
public static class TextNormalizer
{
    // turk kulturu nesnesi - buyuk/kucuk harf cevirimi icin kullanilir
    // neden CultureInfo lazim?
    // cunku C#'ta "i".ToUpper() normalde "I" yapar (ingilizce kurali)
    // ama turkce'de "i" nin buyugu "İ" dir
    // CultureInfo("tr-TR") kullaninca dogru cevirim yapilir
    private static readonly CultureInfo trKultur = new("tr-TR");

    // ana normalize metodu
    // girdi olarak herhangi bir metin alir
    // cikti olarak sadece buyuk turk alfabesi harflerinden olusan bir string dondurur
    public static string Normalize(string girdi)
    {
        // bos veya null kontrolu
        if (string.IsNullOrEmpty(girdi))
            return "";

        // once tum metni buyuk harfe cevir (turk kulturu kurallarina gore)
        string buyukHarf = girdi.ToUpper(trKultur);

        // simdi sadece turk alfabesindeki harfleri al, gerisini at
        var sonuc = new StringBuilder();
        for (int i = 0; i < buyukHarf.Length; i++)
        {
            char c = buyukHarf[i];
            // bu harf turk alfabesinde mi?
            if (TurkishAlphabet.Contains(c))
                sonuc.Append(c); // evet, ekle
            // hayirsa (bosluk, noktalama, rakam vs.) atla
        }

        return sonuc.ToString();
    }
}
