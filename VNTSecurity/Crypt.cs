using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace VNTSecurity.VNTCrypt
{
    public class Crypt1
    {                

        private static string vlEncrypt(string pass_Text, string pass_Phrase, string salt_Value,
            string hash_Algorithm, int pass_Iterations, string init_Vector, int key_Size)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(init_Vector);
            byte[] saltValuesBytes = Encoding.ASCII.GetBytes(salt_Value);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(pass_Text);

            PasswordDeriveBytes password = new PasswordDeriveBytes(pass_Phrase, saltValuesBytes, hash_Algorithm, pass_Iterations);
            byte[] keyBytes = password.GetBytes(key_Size / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            // Start encrypting
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting
            cryptoStream.FlushFinalBlock();

            // Convert out encrypted data from a memory stream into a byte array
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both Streams
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base 64 - encoded string
            string cipherText = Convert.ToBase64String(cipherTextBytes);

            // Return encrypted string
            return cipherText;
        }

        private static string vlDecrypt(string cipher_Text, string pass_Phrase, string salt_Value,
            string hash_Algorithm, int pass_Iterations, string init_Vector, int key_Size)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(init_Vector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt_Value);
            byte[] cipherTextBytes = Convert.FromBase64String(cipher_Text);

            PasswordDeriveBytes password = new PasswordDeriveBytes(pass_Phrase, saltValueBytes, hash_Algorithm,
                pass_Iterations);
            byte[] keyBytes = password.GetBytes(key_Size / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            // Close both streams
            memoryStream.Close();
            cryptoStream.Close();

            string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

            // return decrypted string
            return plainText;
        }

        public string Encrypt_Active(string passw)
        {
            string pass_Phrase = "Pass:vnthanh@nvSolution";
            string salt_Value = "vnthanh@java_dotnet@master";
            string hash_Algorithm = "MD5";
            int pass_Iterations = 5;
            string init_Vector = "@NhAnVuOnGsTuDiO";
            int key_Size = 128;

            return (vlEncrypt(passw, pass_Phrase, salt_Value, hash_Algorithm, pass_Iterations, init_Vector, key_Size));
        }

        public string Decrypt_Active(string cipher)
        {
            string pass_Phrase = "Pass:vnthanh@nvSolution";
            string salt_Value = "vnthanh@java_dotnet@master";
            string hash_Algorithm = "MD5";
            int pass_Iterations = 5;
            string init_Vector = "@NhAnVuOnGsTuDiO";
            int key_Size = 128;

            return (vlDecrypt(cipher, pass_Phrase, salt_Value, hash_Algorithm, pass_Iterations, init_Vector, key_Size));
        }
    }
}
