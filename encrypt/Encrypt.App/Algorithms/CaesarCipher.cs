using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

// kaydirmali sifre (caesar)
// E(x) = (x + k) mod 29
public sealed class CaesarCipher : ICipher
{
    public string Name => "Kaydirmali (Caesar)";
    public string KeyHint => "Kaydirma sayisi girin (orn: 3)";
    public string[] KeyLabels => new[] { "Kaydirma (k)" };

    public string Encrypt(string duzMetin, string[] anahtarlar)
    {
        // anahtari al
        int kaydirma = int.Parse(anahtarlar[0]);

        // metni normalize et
        string normalMetin = TextNormalizer.Normalize(duzMetin);

        // her harfi kaydir
        var sonuc = new StringBuilder();
        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i];
            int index = TurkishAlphabet.IndexOf(harf);
            if (index >= 0)
            {
                // harfi kaydir
                char yeniHarf = TurkishAlphabet.CharAt(index + kaydirma);
                sonuc.Append(yeniHarf);
            }
            else
            {
                sonuc.Append(harf);
            }
        }

        return sonuc.ToString();
    }
}