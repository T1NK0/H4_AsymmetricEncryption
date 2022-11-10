using System.Security.Cryptography;

namespace Encryption.Console
{
    public class Logic
    {
        private RSACryptoServiceProvider myRSA = new RSACryptoServiceProvider(2048);
        private SymmetricAlgorithm myAes = Aes.Create();


        //RSACryptoServiceProvider myRsa = new RSACryptoServiceProvider(2048);
        //SymmetricAlgorithm myAes = Aes.Create();

        //myAes.GenerateKey();

        //byte[] RSACipherText;
        //byte[] plainText;

        //myAes.GenerateKey();
        //RSACipherText = myRsa.Encrypt(myAes.Key, true);
        //plainText = myRsa.Decrypt(RSACipherText, true);


        //HashAlgorithm myHash = SHA256.Create();
        //string someText = "This is an important message";
        //byte[] signature;
        //signature = myRsa.SignData(System.Text.Encoding.ASCII.GetBytes(someText), myHash);
        //bool verified;
        //verified = myRsa.VerifyData(System.Text.Encoding.ASCII.GetBytes(someText), myHash, signature);
    }
}