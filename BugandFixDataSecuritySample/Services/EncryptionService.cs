using BugandFixDataSecuritySample.Abstraction;
using System.Security.Cryptography;
using System.Text;

namespace BugandFixDataSecuritySample.Services;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;
    private readonly IConfiguration _configuration;
    public EncryptionService(IConfiguration configuration)
    {
        _configuration = configuration;
        var key = _configuration["encryptionKey"];

        _key = Encoding.UTF8.GetBytes(key.Substring(0, 32)); // 256-bit key
        _iv = Encoding.UTF8.GetBytes(key.Substring(32, 16)); // 128-bit IV          

    }

    public string Encrypt(string plainText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(plainText);
                    }
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public string Decrypt(string cipherText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
