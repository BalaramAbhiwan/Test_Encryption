using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.UIElements;
using UnityEngine;
public static class EncryptionHelper
{
    private static readonly string ENCRYPTION_KEY = "4d727001e0c2df9f0586ef82dd23857556950f7fc8323f0a8458f1f411369c11";

    public static string EncryptData(string data)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = StringToByteArray(ENCRYPTION_KEY);
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            aesAlg.GenerateIV();
            byte[] iv = aesAlg.IV;

            using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv))
            using (var msEncrypt = new MemoryStream())
            {
                //msEncrypt.Write(iv, 0, iv.Length);
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8)) // Ensure UTF-8 encoding
                {
                    swEncrypt.Write(data);
                }

                byte[] encrypted = msEncrypt.ToArray();
            
                return BitConverter.ToString(iv).Replace("-", "").ToLower() + ":" + BitConverter.ToString(encrypted).Replace("-", "").ToLower();
            }
        }
    }

    public static string DecryptData(string encryptedData)
    {

        string[] parts = encryptedData.Split(':');
        //Debug.Log(parts[0]);
        //Debug.Log(parts[1]);
        if (parts.Length != 2)
        {
            throw new ArgumentException("Invalid encrypted data format");
        }

        byte[] iv = StringToByteArray(parts[0]);
        byte[] cipherText = StringToByteArray(parts[1]);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = StringToByteArray(ENCRYPTION_KEY);
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, iv))
            using (var msDecrypt = new MemoryStream(cipherText))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8)) // Ensure UTF-8 encoding
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }

    private static byte[] StringToByteArray(string hex)
    {
        int numberChars = hex.Length;
        byte[] bytes = new byte[numberChars / 2];
        for (int i = 0; i < numberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }

    public static string CleanHexString(string hex)
    {
        var sb = new StringBuilder();
        foreach (char c in hex)
        {
            if (Uri.IsHexDigit(c))
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
}