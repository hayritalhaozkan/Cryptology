namespace Decrypt.App.Algorithms;

// sifre cozme algoritmalarinin ortak arayuzu
public interface IDecipher
{
    // algoritmanin adi
    string Name { get; }

    // anahtar icin ipucu
    string KeyHint { get; }

    // anahtar alanlari
    string[] KeyLabels { get; }

    // sifre coz
    string Decrypt(string sifreliMetin, string[] anahtarlar);
}