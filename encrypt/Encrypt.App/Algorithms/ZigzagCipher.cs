using System.Text;
using Encrypt.App.Helpers;

namespace Encrypt.App.Algorithms;

// ============================================================================
// ZIGZAG SIFRESI (RAIL FENCE CIPHER)
// ============================================================================
// Bu sifrede metin bir ZIGZAG deseninde "raylara" yazilir,
// sonra her ray soldan saga okunarak sifreli metin olusturulur.
//
// NASIL CALISIR?
// 1. Belirli sayida ray (satir) olusturulur
// 2. Metin zigzag seklinde bu raylara dagilir
//    (asagi-yukari-asagi-yukari seklinde ilerler)
// 3. Her ray soldan saga okunarak sifreli metin elde edilir
//
// ANAHTAR: Ray sayisi (tamsayi). Ornegin: 3
//
// ORNEK:
//   Metin: "MERHABADUNYA"    Anahtar: 3 ray
//
//   Zigzag deseni (3 ray):
//     Ray 0: M . . . A . . . U . . .
//     Ray 1: . E . H . B . D . N . A
//     Ray 2: . . R . . . A . . . Y .
//
//   Her rayi soldan saga oku:
//     Ray 0: M, A, U
//     Ray 1: E, H, B, D, N, A
//     Ray 2: R, A, Y
//
//   Sonuc: "MAUEHBDNARAY"
//
// ZIGZAG NASIL ILERLER?
//   Baslangicta ray 0'dasin ve asagi iniyorsun
//   Ray 0 -> Ray 1 -> Ray 2 (en alta geldin, simdi yukari don)
//   Ray 2 -> Ray 1 -> Ray 0 (en uste geldin, simdi tekrar asagi)
//   Ray 0 -> Ray 1 -> Ray 2 -> ...
//   Bu sekilde zigzag deseni olusur.
// ============================================================================
public sealed class ZigzagCipher : ICipher
{
    public string Name => "Zigzag (Rail Fence)";
    public string KeyHint => "Ray sayisi girin. Orn: 3";
    public string[] KeyLabels => new[] { "Ray Sayisi" };

    public string Encrypt(string duzMetin, string[] anahtarlar)
    {
        // ray sayisini al
        int raySayisi = int.Parse(anahtarlar[0]);

        // metni normalize et
        string normalMetin = TextNormalizer.Normalize(duzMetin);

        // bos metin kontrolu
        if (normalMetin.Length == 0)
            return "";

        // ozel durumler:
        // ray sayisi 1 ise zigzag olmaz, metin ayni kalir
        // ray sayisi metin uzunlugundan buyuk/esitse de metin ayni kalir
        if (raySayisi == 1 || raySayisi >= normalMetin.Length)
            return normalMetin;

        // her ray icin bir StringBuilder olustur
        // ornegin 3 ray icin: satirlar[0], satirlar[1], satirlar[2]
        var satirlar = new StringBuilder[raySayisi];
        for (int i = 0; i < raySayisi; i++)
            satirlar[i] = new StringBuilder();

        int mevcutRay = 0;   // su an hangi raydayiz (0'dan baslar)
        bool asagiMi = true; // su an asagi mi iniyoruz yoksa yukari mi cikiyoruz

        // her harfi zigzag sirasinda ilgili raya yaz
        for (int i = 0; i < normalMetin.Length; i++)
        {
            // harfi mevcut raya ekle
            satirlar[mevcutRay].Append(normalMetin[i]);

            // yon degistirme kontrolleri
            if (mevcutRay == 0)
                asagiMi = true;  // en ustteyiz, artik asagi inmeye basla
            else if (mevcutRay == raySayisi - 1)
                asagiMi = false; // en alttayiz, artik yukari cikamaya basla

            // bir sonraki raya gec
            if (asagiMi)
                mevcutRay++; // asagi in
            else
                mevcutRay--; // yukari cik
        }

        // tum raylari sirsiyla birlestir
        // once ray 0'daki harfler, sonra ray 1, sonra ray 2...
        var sonuc = new StringBuilder();
        for (int i = 0; i < raySayisi; i++)
            sonuc.Append(satirlar[i]);

        return sonuc.ToString();
    }
}
