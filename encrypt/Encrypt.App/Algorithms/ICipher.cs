namespace Encrypt.App.Algorithms;

// sifreleme algoritmalarinin ortak arayuzu
public interface ICipher
{
    // algoritmanin adi
    string Name { get; }

    // anahtar icin ipucu
    string KeyHint { get; }

    // anahtar alanlari
    string[] KeyLabels { get; }

    // sifrele
    string Encrypt(string duzMetin, string[] anahtarlar);
}