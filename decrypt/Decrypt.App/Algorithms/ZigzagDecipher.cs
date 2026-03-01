using System.Text;
using Decrypt.App.Helpers;

namespace Decrypt.App.Algorithms;

// ============================================================================
// ZIGZAG SIFRE COZME (RAIL FENCE DECIPHER)
// ============================================================================
// Zigzag sifresinin tersini yapar. Bu en karmasik cozme algoritmasi cunku
// dogrudan ters islem yapmak yetmez, once raylara kac harf dustugunu hesaplamak gerekir.
//
// SIFRELEME NASIL CALISIYORDU?
//   Metin zigzag seklinde raylara yazildi, sonra ray ray okundu.
//   Ornegin "MERHABADUNYA" 3 rayla:
//     Ray 0: M, A, U     -> "MAU"
//     Ray 1: E, H, B, D, N, A -> "EHBDNA"
//     Ray 2: R, A, Y     -> "RAY"
//     Sifreli metin: "MAUEHBDNARAY"
//
// COZME NASIL CALISIR? (3 ADIM)
//
// ADIM 1: Her raya kac harf dustugunu hesapla
//   Zigzag desenini simule et ama harf yazmadan sadece say
//   Ray 0: 3 harf, Ray 1: 6 harf, Ray 2: 3 harf
//
// ADIM 2: Sifreli metni raylara dagit
//   Sifreli metin "MAUEHBDNARAY" (12 harf)
//   Ray 0: ilk 3 harf  -> "MAU"
//   Ray 1: sonraki 6 harf -> "EHBDNA"
//   Ray 2: son 3 harf   -> "RAY"
//
// ADIM 3: Zigzag sirasinda rayladan harfleri oku
//   Zigzag sirasi: ray0, ray1, ray2, ray1, ray0, ray1, ray2, ray1, ray0...
//   ray0[0]=M, ray1[0]=E, ray2[0]=R, ray1[1]=H, ray0[1]=A, ray1[2]=B,
//   ray2[1]=A, ray1[3]=D, ray0[2]=U, ray1[4]=N, ray2[2]=Y, ray1[5]=A
//   Sonuc: "MERHABADUNYA"
// ============================================================================
public sealed class ZigzagDecipher : IDecipher
{
    public string Name => "Zigzag (Rail Fence)";
    public string KeyHint => "Ray sayisi girin. Orn: 3";
    public string[] KeyLabels => new[] { "Ray Sayisi" };

    public string Decrypt(string sifreliMetin, string[] anahtarlar)
    {
        int raySayisi = int.Parse(anahtarlar[0]);

        string normalMetin = TextNormalizer.Normalize(sifreliMetin);
        int uzunluk = normalMetin.Length;

        if (uzunluk == 0)
            return "";

        if (raySayisi == 1 || raySayisi >= uzunluk)
            return normalMetin;

        // ---------------------------------------------------------------
        // ADIM 1: Her raydeki harf sayisini hesapla
        // ---------------------------------------------------------------
        // Zigzag desenini simule ediyoruz ama harf yazmiyoruz
        // sadece her raya kac harf dustugunu sayiyoruz
        int[] rayUzunluklari = new int[raySayisi];
        int ray = 0;
        bool asagi = true;

        for (int i = 0; i < uzunluk; i++)
        {
            rayUzunluklari[ray]++; // bu raya bir harf daha dustu

            // yon degistirme
            if (ray == 0) asagi = true;
            else if (ray == raySayisi - 1) asagi = false;

            if (asagi) ray++;
            else ray--;
        }
        // simdi biliyoruz: ray0'a 3 harf, ray1'e 6 harf, ray2'ye 3 harf dustu

        // ---------------------------------------------------------------
        // ADIM 2: Sifreli metni raylara dagit
        // ---------------------------------------------------------------
        // sifreli metinde once ray 0'in harfleri, sonra ray 1'in, sonra ray 2'nin var
        // her ray icin o raya dusen kadar harf aliyoruz
        string[] satirlar = new string[raySayisi];
        int pozisyon = 0;

        for (int r = 0; r < raySayisi; r++)
        {
            satirlar[r] = normalMetin.Substring(pozisyon, rayUzunluklari[r]);
            pozisyon += rayUzunluklari[r];
        }
        // simdi: satirlar[0]="MAU", satirlar[1]="EHBDNA", satirlar[2]="RAY"

        // ---------------------------------------------------------------
        // ADIM 3: Zigzag sirasinda rayladan harfleri oku
        // ---------------------------------------------------------------
        // zigzag desenini tekrar simule ediyoruz
        // ama bu sefer her raya simdi o raydan siradaki harfi aliyoruz
        int[] rayIndexleri = new int[raySayisi]; // her rayin kacinci harfindeyiz
        var sonuc = new StringBuilder();

        ray = 0;
        asagi = true;

        for (int i = 0; i < uzunluk; i++)
        {
            // mevcut raydan siradaki harfi al
            sonuc.Append(satirlar[ray][rayIndexleri[ray]]);
            rayIndexleri[ray]++; // bu rayda bir sonraki harfe gec

            // yon degistirme (zigzag hareketi)
            if (ray == 0) asagi = true;
            else if (ray == raySayisi - 1) asagi = false;

            if (asagi) ray++;
            else ray--;
        }

        return sonuc.ToString();
    }
}
