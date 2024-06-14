using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Crypto
{
    private static readonly string key = "20FXY6xAFnXg+2AY4+H3JgfDeFpiPjqWK2jA3v1OGpk=";

    public static string Encrypt(string text)
    {
        byte[] keyBytes = Convert.FromBase64String(key);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = keyBytes;
            aesAlg.GenerateIV();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                // Écrire l'IV au début du flux chiffré
                msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    // Convertir le texte en bytes et l'écrire dans le CryptoStream
                    byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(text);
                    csEncrypt.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                    csEncrypt.FlushFinalBlock();  // Finaliser le chiffrement

                    // Convertir le MemoryStream résultant en un tableau de bytes et le convertir en base64
                    byte[] encryptedBytes = msEncrypt.ToArray();
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }
    }


    public static string Decrypt(string cipherText)
    {
        byte[] fullCipher = Convert.FromBase64String(cipherText);
        byte[] keyBytes = Convert.FromBase64String(key);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = keyBytes;

            byte[] iv = new byte[aesAlg.BlockSize / 8];
            byte[] cipher = new byte[fullCipher.Length - iv.Length];

            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipher))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
}
