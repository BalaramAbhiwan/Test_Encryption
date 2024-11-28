using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class EncryptionExample : MonoBehaviour
{
    private const string ENCRYPTION_KEY = "4d727001e0c2df9f0586ef82dd23857556950f7fc8323f0a8458f1f411369c11";

    public string Tests = "";
    public static string EncryptData(object data)
    {
        try
        {
            string jsonData = data is string ? (string)data : JsonConvert.SerializeObject(data);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(jsonData);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = StringToByteArray(ENCRYPTION_KEY);
                aesAlg.GenerateIV(); // Generate a new IV for each encryption
                aesAlg.Padding = PaddingMode.PKCS7; // Ensure padding mode

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // Prepend the IV to the encrypted data
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(plainTextBytes, 0, plainTextBytes.Length);
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Encryption error: " + e.Message);
            return null;
        }
    }

    public static object DecryptData(string encryptedData)
    {
        try
        {
            byte[] cipherTextCombined = Convert.FromBase64String(encryptedData);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = StringToByteArray(ENCRYPTION_KEY);
                aesAlg.Padding = PaddingMode.PKCS7; // Ensure padding mode

                // Extract the IV from the beginning of the encrypted data
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                Array.Copy(cipherTextCombined, 0, iv, 0, iv.Length);

                // Extract the actual encrypted data
                byte[] cipherText = new byte[cipherTextCombined.Length - iv.Length];
                Array.Copy(cipherTextCombined, iv.Length, cipherText, 0, cipherText.Length);

                aesAlg.IV = iv;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            string jsonData = srDecrypt.ReadToEnd();
                            try
                            {
                                return JsonConvert.DeserializeObject(jsonData);
                            }
                            catch (JsonException)
                            {
                                return jsonData; // Return plain string if not JSON
                            }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Decryption error: " + e.Message);
            return null;
        }
    }

    private static byte[] StringToByteArray(string hex)
    {
        int NumberChars = hex.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }

    void Start()
    {
        var exampleData = new
        {
            walletAddress = "addr_test1qrpmklafl83phr57sknpl56nqmafc723n5lnawrkn5mrkewm349cl6c5ka7h0kqkapstqsjjqvy9mkhrkng66mpq33gqfau0c9"
        };

        string encryptedData = EncryptData(exampleData);
        Debug.Log("Encrypted: " + encryptedData);

        var decryptedData = DecryptData(encryptedData);
        Debug.Log("Decrypted: " + JsonConvert.SerializeObject(decryptedData));
    }
}
