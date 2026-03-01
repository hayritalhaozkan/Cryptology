using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

// rota sifresi (route cipher)
// metin satirxsutun izgaraya yazilir, sonra sutun sutun okunur
public sealed class RouteCipher : ICipher
{
    public string Name => "Rota (Route)";
    public string KeyHint => "Satir ve sutun sayisi girin.\nOrn: 4 ve 5";
    public string[] KeyLabels => new[] { "Satir", "Sutun" };

    public string Encrypt(string duzMetin, string[] anahtarlar)
    {
        int satirSayisi = int.Parse(anahtarlar[0]);
        int sutunSayisi = int.Parse(anahtarlar[1]);

        int izgaraBoyutu = satirSayisi * sutunSayisi;

        string normalMetin = TextNormalizer.Normalize(duzMetin);

        // eksikse 'A' ile doldur
        while (normalMetin.Length < izgaraBoyutu)
            normalMetin += 'A';

        // fazlaysa kirp
        if (normalMetin.Length > izgaraBoyutu)
            normalMetin = normalMetin.Substring(0, izgaraBoyutu);

        // izgaraya satir satir yaz
        char[,] izgara = new char[satirSayisi, sutunSayisi];
        int sayac = 0;
        for (int s = 0; s < satirSayisi; s++)
            for (int st = 0; st < sutunSayisi; st++)
                izgara[s, st] = normalMetin[sayac++];

        // sutun sutun oku
        var sonuc = new StringBuilder();
        for (int st = 0; st < sutunSayisi; st++)
            for (int s = 0; s < satirSayisi; s++)
                sonuc.Append(izgara[s, st]);

        return sonuc.ToString();
    }
}
