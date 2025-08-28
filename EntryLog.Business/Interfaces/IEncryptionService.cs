namespace EntryLog.Business.Interfaces;

public interface IEncryptionService
{
    string Encrypt(string text);
    string Decrypt(string cipherText);
}