namespace Decrypt.App.Algorithms;

public interface IDecipher
{
    string Name { get; }
    string Decrypt(string cipherText, string key);
}