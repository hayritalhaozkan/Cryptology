namespace Encrypt.App.Algorithms;

// ============================================================================
// SIFRELEME ARAYUZU (INTERFACE)
// ============================================================================
// Bu bir interface'tir. Interface nedir?
// Bir "sozlesme" gibidir. Bu interface'i uygulayan her sinif
// asagidaki 4 seyi MUTLAKA tanimlamak zorundadir.
//
// Neden interface kullaniyoruz?
// Cunku 7 farkli sifreleme algoritmamiz var (Caesar, Affine, Vigenere vb.)
// Hepsi farkli calisiyor ama hepsinin ortak ozellikleri var:
// - Bir adi var
// - Bir anahtar ipucu var
// - Anahtar alanlari var
// - Bir Encrypt metodu var
//
// MainWindow.axaml.cs dosyasinda bu interface sayesinde
// hangi algoritma secilirse secilsin ayni sekilde kullanabiliyoruz.
// ornegin: _selectedCipher.Encrypt(metin, anahtarlar)
// bu satir Caesar icin de calisir, Affine icin de calisir.
// ============================================================================
public interface ICipher
{
    // algoritmanin kullaniciya gorunecek adi
    // ornegin: "Kaydirmali (Caesar)", "Dogrusal (Affine)" gibi
    // ComboBox'ta bu isim gosterilir
    string Name { get; }

    // kullaniciya anahtari nasil girmesi gerektigini anlatan ipucu
    // ornegin: "Kaydirma sayisi girin (orn: 3)"
    string KeyHint { get; }

    // anahtar girisi icin etiketler
    // bazi algoritmalar tek anahtar alir (ornegin Caesar sadece "k" alir)
    // bazi algoritmalar birden fazla anahtar alir (ornegin Affine "a" ve "b" alir)
    // bu dizi her anahtar alani icin bir etiket icerir
    string[] KeyLabels { get; }

    // sifreleme metodudur
    // duzMetin: kullanicinin girdigi acik metin
    // anahtarlar: kullanicinin girdigi anahtar degerleri (string dizisi olarak)
    // geri donus: sifrelenmis metin
    string Encrypt(string duzMetin, string[] anahtarlar);
}