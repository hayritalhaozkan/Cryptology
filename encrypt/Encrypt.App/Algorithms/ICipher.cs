namespace Encrypt.App.Algorithms;

public interface ICipher
{
    string Name { get; }
    string Encrypt(string plainText, string key);
}