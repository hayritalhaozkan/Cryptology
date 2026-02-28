namespace Decrypt.App.Algorithms;

/// <summary>
/// Tüm çözme algoritmalarının ortak arayüzü.
/// </summary>
public interface IDecipher
{
    string Name { get; }
    string KeyHint { get; }
    string[] KeyLabels { get; }
    string Decrypt(string cipherText, string[] keys);
}