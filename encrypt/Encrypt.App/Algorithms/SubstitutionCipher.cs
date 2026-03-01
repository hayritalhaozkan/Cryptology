using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

// yer degistirme sifresi (substitution)
// her harf anahtar alfabedeki karsiligina donusur
public sealed class SubstitutionCipher : ICipher
{
    public string Name => "Yer Degistirme (Substitution)";
    public string KeyHint => "29 harflik karisik alfabe girin.\nOrn: ÜYZABCÇDEFGĞHIIJKLMNOÖPRSŞTÜV";
    public string[] KeyLabels => new[] { "Anahtar Alfabesi (29 harf)" };

    public string Encrypt(string duzMetin, string[] anahtarlar)
    {
        // anahtar alfabeyi normalize et
        string anahtarAlfabe = TextNormalizer.Normalize(anahtarlar[0]);

        string normalMetin = TextNormalizer.Normalize(duzMetin);

        var sonuc = new StringBuilder();
        for (int i = 0; i < normalMetin.Length; i++)
        {
            char harf = normalMetin[i];
            int index = TurkishAlphabet.IndexOf(harf);
            if (index >= 0)
            {
                // duz alfabedeki indexe karsilik gelen anahtar harfi al
                sonuc.Append(anahtarAlfabe[index]);
            }
            else
            {
                sonuc.Append(harf);
            }
        }

        return sonuc.ToString();
    }
}
