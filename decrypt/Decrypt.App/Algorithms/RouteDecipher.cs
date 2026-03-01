using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

// ============================================================================
// ROTA SIFRE COZME (ROUTE DECIPHER)
// ============================================================================
// Rota sifresinin tersini yapar.
//
// SIFRELEME NASIL CALISIYORDU?
//   1. Metin izgaraya SATIR SATIR yazildi
//   2. Izgara SUTUN SUTUN okundu
//
// COZME NASIL CALISIR? (TAM TERS)
//   1. Sifreli metin izgaraya SUTUN SUTUN yazilir
//   2. Izgara SATIR SATIR okunur -> orijinal metin elde edilir
//
// ORNEK:
//   Sifreli metin: "MAUEBNRAYHDA"    Anahtar: 3 satir, 4 sutun
//
//   Adim 1: Izgaraya SUTUN SUTUN yaz
//     1. sutun: M, A, U
//     2. sutun: E, B, N
//     3. sutun: R, A, Y
//     4. sutun: H, D, A
//
//     Izgara:
//       M  E  R  H
//       A  B  A  D
//       U  N  Y  A
//
//   Adim 2: Izgarayi SATIR SATIR oku
//     1. satir: M, E, R, H
//     2. satir: A, B, A, D
//     3. satir: U, N, Y, A
//     Sonuc: "MERHABADUNYA"
// ============================================================================
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

        // padding ve kirpma
        while (normalMetin.Length < izgaraBoyutu)
            normalMetin += 'A';

        if (normalMetin.Length > izgaraBoyutu)
            normalMetin = normalMetin.Substring(0, izgaraBoyutu);

        // SIFRELEME sutun sutun OKUMUSTU
        // COZME icin sutun sutun YAZIYORUZ (ters islem)
        char[,] izgara = new char[satirSayisi, sutunSayisi];
        int sayac = 0;
        for (int st = 0; st < sutunSayisi; st++)      // once sutunlar (column-major)
            for (int s = 0; s < satirSayisi; s++)      // sonra satirlar
                izgara[s, st] = normalMetin[sayac++];

        // izgarayi SATIR SATIR oku -> orijinal metin
        var sonuc = new StringBuilder();
        for (int s = 0; s < satirSayisi; s++)          // once satirlar (row-major)
            for (int st = 0; st < sutunSayisi; st++)    // sonra sutunlar
                sonuc.Append(izgara[s, st]);

        return sonuc.ToString();
    }
}
