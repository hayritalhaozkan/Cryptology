using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

// kaydirmali sifre cozme (caesar)
// D(y) = (y - k) mod 29
public sealed class CaesarDecipher : IDecipher
{
    public string Name => "Kaydirmali (Caesar)";
    public string KeyHint => "Kaydirma sayisi girin (orn: 3)";
    public string[] KeyLabels => new[] { "Kaydirma (k)" };

    public string Decrypt(string sifreliMetin, string[] anahtarlar)
    {
        // anahtari al
        int kaydirma = int.Parse(anahtarlar[0]);

        // metni normalize et
        string normalMetin = TextNormalizer.Normalize(sifreliMetin);

        // her harfi geri kaydir
        var sonuc = new StringBuilder();
        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i];
            int index = TurkishAlphabet.IndexOf(harf);
            if (index >= 0)
            {
                char yeniHarf = TurkishAlphabet.CharAt(index - kaydirma);
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