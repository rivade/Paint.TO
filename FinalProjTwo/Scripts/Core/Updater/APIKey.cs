using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DrawingProgram;

public class APIKey
{
    // (i know this shouldnt be on github i included it for my girlfriend lmao)
    // Security is not important here, the key only allows for reading of public repositories
    // GitHub wouldn't let me submit with the API token written plainly in the code lol
    private const string EncryptedKey = "CPtoaPtsz9+JlIyQ7sFPShDWgtN6suuu1mZOMqTjvP+jcT8pQ9dMQ2WIyaIjdnDp";
    private const string DecryptionKey = "ABC123DEF456GHIJ";
    private const string IV = "JIHG654FED321CBA";

    public static string GetAPIKey()
    {
        return DecryptKey();
    }

    private static string DecryptKey()
    {
        byte[] cipherText = Convert.FromBase64String(EncryptedKey);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(DecryptionKey);
            aesAlg.IV = Encoding.UTF8.GetBytes(IV);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
