namespace Decrypt.App.Algorithms;

// ============================================================================
// SIFRE COZME ARAYUZU (INTERFACE)
// ============================================================================
// Encrypt tarafinda ICipher vardi, bu da onun decrypt karsiligi.
// Tum sifre cozme siniflari bu interface'i uygulamak zorunda.
//
// Encrypt'teki ICipher ile ayni mantikta calisir:
// - Her algoritmanin bir adi var
// - Anahtar ipucu ve alanlari var
// - Bir Decrypt metodu var
// ============================================================================
public interface IDecipher
{
    // algoritmanin adi (ComboBox'ta gorunur)
    string Name { get; }

    // anahtar girisi icin ipucu
    string KeyHint { get; }

    // anahtar alanlari
    string[] KeyLabels { get; }

    // sifre cozme metodu
    // sifreliMetin: cozulecek sifreli metin
    // anahtarlar: sifreleme sirasinda kullanilan anahtar degerleri
    // donus: cozulmus (orijinal) metin
    string Decrypt(string sifreliMetin, string[] anahtarlar);
}