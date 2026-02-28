namespace Encrypt.App.Algorithms;

/// <summary>
/// Tüm şifreleme algoritmalarının ortak arayüzü.
/// </summary>
public interface ICipher
{
    /// <summary>Algoritma görünen adı (ComboBox'ta görünecek).</summary>
    string Name { get; }

    /// <summary>Anahtar girişi için kullanıcıya gösterilecek ipucu.</summary>
    string KeyHint { get; }

    /// <summary>Anahtar alanlarının etiketleri (birden fazla alan olabilir).</summary>
    string[] KeyLabels { get; }

    /// <summary>Metni şifreler. keys dizisi UI'daki anahtar alanlarından gelir.</summary>
    string Encrypt(string plainText, string[] keys);
}