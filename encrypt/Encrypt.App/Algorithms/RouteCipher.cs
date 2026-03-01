using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

// ============================================================================
// ROTA SIFRESI (ROUTE CIPHER)
// ============================================================================
// Bu sifrede metin bir IZGARA (tablo/matris) uzerine yazilir
// ve farkli bir sirada okunur.
//
// NASIL CALISIR?
// 1. Metin satir x sutun boyutunda bir izgaraya SATIR SATIR yazilir (soldan saga)
// 2. Izgara SUTUN SUTUN okunur (yukaridan asagiya)
//
// ANAHTAR: Satir sayisi ve sutun sayisi. Ornegin: 3 satir, 4 sutun = 12 hucre
//
// ORNEK:
//   Metin: "MERHABADUNY" (11 harf, 1 eksik -> 'A' eklenir -> "MERHABADUNYA")
//   Anahtar: 3 satir, 4 sutun
//
//   Izgaraya SATIR SATIR yaz:
//     M  E  R  H
//     A  B  A  D
//     U  N  Y  A
//
//   Izgarayi SUTUN SUTUN oku (yukaridan asagiya, soldan saga):
//     1. sutun: M, A, U
//     2. sutun: E, B, N
//     3. sutun: R, A, Y
//     4. sutun: H, D, A
//     Sonuc: "MAUEBNRAYHDA"
//
// NOT: Metin izgara boyutundan kisaysa 'A' ile doldurulur.
//      Metin izgara boyutundan uzunsa fazlasi kesilir.
// ============================================================================
public sealed class RouteCipher : ICipher
{
    public string Name => "Rota (Route)";
    public string KeyHint => "Satir ve sutun sayisi girin.\nOrn: 4 ve 5";

    // iki anahtar alani: satir ve sutun
    public string[] KeyLabels => new[] { "Satir", "Sutun" };

    public string Encrypt(string duzMetin, string[] anahtarlar)
    {
        // satir ve sutun sayilarini al
        int satirSayisi = int.Parse(anahtarlar[0]);
        int sutunSayisi = int.Parse(anahtarlar[1]);

        // toplam hucre sayisi
        int izgaraBoyutu = satirSayisi * sutunSayisi;

        // metni normalize et
        string normalMetin = TextNormalizer.Normalize(duzMetin);

        // metin izgaradan kisaysa 'A' ile doldur (padding)
        while (normalMetin.Length < izgaraBoyutu)
            normalMetin += 'A';

        // metin izgaradan uzunsa kirp
        if (normalMetin.Length > izgaraBoyutu)
            normalMetin = normalMetin.Substring(0, izgaraBoyutu);

        // 2 boyutlu izgarayi olustur ve metin SATIR SATIR yaz
        char[,] izgara = new char[satirSayisi, sutunSayisi];
        int sayac = 0;
        for (int s = 0; s < satirSayisi; s++)      // her satir icin
            for (int st = 0; st < sutunSayisi; st++) // her sutun icin
                izgara[s, st] = normalMetin[sayac++]; // harfi yerlestir

        // izgarayi SUTUN SUTUN oku (column-major order)
        // once 1. sutunu yukardan asagi, sonra 2. sutunu, ...
        var sonuc = new StringBuilder();
        for (int st = 0; st < sutunSayisi; st++)     // her sutun icin
            for (int s = 0; s < satirSayisi; s++)     // yukardan asagi
                sonuc.Append(izgara[s, st]);

        return sonuc.ToString();
    }
}
