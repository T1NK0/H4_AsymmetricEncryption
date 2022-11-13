using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Encryption.Console
{
    public class AESEncryption
    {
        //Method to encrypt the string to bytes
        public byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments if null or less than or equal 0.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            //ByteArray we will return containing the result of the encryption.
            byte[] encrypted;


            //Create instance of SymmetricAlgorithm called MySymetricAlgorithm.
            SymmetricAlgorithm mySymetricAlgorithm = Aes.Create();

            //Key parameter of encryption.
            mySymetricAlgorithm.Key = Key;

            //IV (Can be seen as the salt of the encryption) parameter of the encryption.
            mySymetricAlgorithm.IV = IV;
            mySymetricAlgorithm.Mode = CipherMode.CBC;
            mySymetricAlgorithm.Padding = PaddingMode.PKCS7;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = mySymetricAlgorithm.CreateEncryptor(mySymetricAlgorithm.Key, mySymetricAlgorithm.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments if null or less then or equal to 0.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            //String we will return containing the result of the decryption.
            string plaintext = null;

            //Create instance of SymmetricAlgorithm called MySymetricAlgorithm.
            SymmetricAlgorithm mySymetricAlgorithm = Aes.Create();

            //Key parameter of encryption.
            mySymetricAlgorithm.Key = Key;

            //IV (Can be seen as the salt of the encryption) parameter of the encryption.
            mySymetricAlgorithm.IV = IV;
            mySymetricAlgorithm.Mode = CipherMode.CBC;
            mySymetricAlgorithm.Padding = PaddingMode.PKCS7;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = mySymetricAlgorithm.CreateDecryptor(mySymetricAlgorithm.Key, mySymetricAlgorithm.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }


        ////Method to encrypt the string to bytes
        //public byte[] EncryptStringToBytes(string plainText, byte[] Key)
        //{
        //    // Check arguments if null or less than or equal 0.
        //    if (plainText == null || plainText.Length <= 0)
        //        throw new ArgumentNullException("plainText");
        //    if (Key == null || Key.Length <= 0)
        //        throw new ArgumentNullException("Key");

        //    //ByteArray we will return containing the result of the encryption.
        //    byte[] encrypted;


        //    //Create instance of SymmetricAlgorithm called MySymetricAlgorithm.
        //    SymmetricAlgorithm sessionKey = Aes.Create();

        //    //Key parameter of encryption.
        //    sessionKey.Key = Key;

        //    //IV (Can be seen as the salt of the encryption) parameter of the encryption.
        //    sessionKey.IV = CreateKeyWithUserInput(16);
        //    sessionKey.KeySize = sessionKey.LegalKeySizes[0].MaxSize;
        //    sessionKey.Mode = CipherMode.CBC;
        //    sessionKey.Padding = PaddingMode.PKCS7;

        //    // Create an encryptor to perform the stream transform.
        //    ICryptoTransform encryptor = sessionKey.CreateEncryptor(sessionKey.Key, sessionKey.IV);

        //    // Create the streams used for encryption.
        //    using (MemoryStream msEncrypt = new MemoryStream())
        //    {
        //        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //        {
        //            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
        //            {
        //                //Write all data to the stream.
        //                swEncrypt.Write(plainText);
        //            }
        //            encrypted = msEncrypt.ToArray();
        //        }
        //    }
        //    // Return the encrypted bytes from the memory stream.
        //    return encrypted;
        //}

        //public string DecryptStringFromBytes(byte[] cipherText, byte[] Key)
        //{
        //    // Check arguments if null or less then or equal to 0.
        //    if (cipherText == null || cipherText.Length <= 0)
        //        throw new ArgumentNullException("cipherText");
        //    if (Key == null || Key.Length <= 0)
        //        throw new ArgumentNullException("Key");

        //    //String we will return containing the result of the decryption.
        //    string plaintext = null;

        //    //Create instance of SymmetricAlgorithm called MySymetricAlgorithm.
        //    SymmetricAlgorithm sessionKey = Aes.Create();

        //    //Key parameter of encryption.
        //    sessionKey.Key = Key;

        //    //IV (Can be seen as the salt of the encryption) parameter of the encryption.

        //    byte[] iv = new byte[sessionKey.BlockSize / 8];
        //    Array.Copy(cipherText, iv, iv.Length);
        //    sessionKey.IV = iv;

        //    sessionKey.Mode = CipherMode.CBC;
        //    sessionKey.Padding = PaddingMode.PKCS7;

        //    // Create a decryptor to perform the stream transform.
        //    ICryptoTransform decryptor = sessionKey.CreateDecryptor(sessionKey.Key, sessionKey.IV);

        //    // Create the streams used for decryption.
        //    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
        //    {
        //        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        //        {
        //            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
        //            {
        //                // Read the decrypted bytes from the decrypting stream and place them in a string.
        //                plaintext = srDecrypt.ReadToEnd();
        //            }
        //        }
        //    }
        //    return plaintext;
        //}

        public byte[] CreateKeyWithUserInput(int length)
        {
            //Generate a cryptographic random number
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            //Byte array called buff taking in the length of the parameter from the method.
            byte[] buff = new byte[length];
            //Fill the byte array.
            rng.GetBytes(buff);

            //return the byte array.
            return buff;
        }
    }
}
