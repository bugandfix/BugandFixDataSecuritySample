namespace BugandFixDataSecuritySample.Abstraction;

public interface IEncryptionService
{
    public string Decrypt(string cipherText);
    public string Encrypt(string plainText);
}