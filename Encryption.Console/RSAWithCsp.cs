using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Encryption.Console
{
    public class RSAWithCsp
    {
        const string ContainerName = "MyContainer";

        public void AssignNewKey()
        {
            CspParameters cspParams = new CspParameters(1);
            cspParams.KeyContainerName = ContainerName;
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspParams) { PersistKeyInCsp = true };
        }

        public void DeleteKeyInCsp()
        {
            CspParameters cspParams = new CspParameters { KeyContainerName = ContainerName };
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspParams) { PersistKeyInCsp = false };

            rsa.Clear();
        }

        public byte[] EncryptData(byte[] dataToEncrypt)
        {
            byte[] cipherbytes;

            CspParameters cspParams = new CspParameters { KeyContainerName = ContainerName };

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048, cspParams))
            {
                cipherbytes = rsa.Encrypt(dataToEncrypt, false);
            }

            return cipherbytes;
        }

        public byte[] DecryptData(byte[] dataToDecrypt)
        {
            byte[] plain;

            CspParameters cspParams = new CspParameters { KeyContainerName = ContainerName };

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048, cspParams))
            {
                plain = rsa.Decrypt(dataToDecrypt, false);
            }

            return plain;
        }

        public string MyStringBuilder(byte[] input)
        {
            //Use a string builder to assemble the bytes to a string with text format instead of hexadecimal
            StringBuilder sb = new StringBuilder();
            foreach (byte b in input)
            {
                //ToString("x2") is to format hexadecimal to text
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

    }
}
