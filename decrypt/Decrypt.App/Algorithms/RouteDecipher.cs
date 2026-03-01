using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

// rota sifre cozme (route decipher)
// sifreleme sutun sutun okunmustu, cozme icin sutun sutun yazilir satir satir okunur
public sealed class RouteDecipher : IDecipher
{
    public string Name => "Rota (Route)";
    public string KeyHint => "Satir ve sutun sayisi girin.\nOrn: 4 ve 5";
    public string[] KeyLabels => new[] { "Satir", "Sutun" };

    public string Decrypt(string sifreliMetin, string[] anahtarlar)
    {
        int satirSayisi = int.Parse(anahtarlar[0]);
        int sutunSayisi = int.Parse(anahtarlar[1]);

        int izgaraBoyutu = satirSayisi * sutunSayisi;

        string normalMetin = TextNormalizer.Normalize(sifreliMetin);

        // eksikse 'A' ile doldur
        while (normalMetin.Length < izgaraBoyutu)
            normalMetin += 'A';

        // fazlaysa kirp
        if (normalMetin.Length > izgaraBoyutu)
            normalMetin = normalMetin.Substring(0, izgaraBoyutu);

        // sifreleme sutun sutun okumustu, cozme icin sutun sutun yaz
        char[,] izgara = new char[satirSayisi, sutunSayisi];
        int sayac = 0;
        for (int st = 0; st < sutunSayisi; st++)
            for (int s = 0; s < satirSayisi; s++)
                izgara[s, st] = normalMetin[sayac++];

        // satir satir oku
        var sonuc = new StringBuilder();
        for (int s = 0; s < satirSayisi; s++)
            for (int st = 0; st < sutunSayisi; st++)
                sonuc.Append(izgara[s, st]);

        return sonuc.ToString();
    }
}
