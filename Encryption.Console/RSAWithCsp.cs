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
        RSACryptoServiceProvider rsa = null;
        string publicPrivateKeyXML;
        string publicOnlyKeyXML;
        public void AssignNewKey()
        {
            const int PROVIDER_RSA_FULL = 1;
            const string CONTAINER_NAME = "KeyContainer";
            CspParameters cspParams;
            cspParams = new CspParameters(PROVIDER_RSA_FULL);
            cspParams.KeyContainerName = CONTAINER_NAME;
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";
            rsa = new RSACryptoServiceProvider(cspParams);

            //Pair of public and private key as XML string.
            //Do not share this to other party
            publicPrivateKeyXML = rsa.ToXmlString(true);

            //Private key in xml file, this string should be share to other parties
            publicOnlyKeyXML = rsa.ToXmlString(false);

        }

        public byte[] Encrypt(string publicKeyXML, string dataToDycript)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKeyXML);

            return rsa.Encrypt(ASCIIEncoding.ASCII.GetBytes(dataToDycript), true);
        }

        public string Decrypt(string publicPrivateKeyXML, byte[] encryptedData)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicPrivateKeyXML);

            return ASCIIEncoding.ASCII.GetString(rsa.Decrypt(encryptedData, true));
        }

        //const string ContainerName = "MyContainer";

        //public void AssignNewKey()
        //{
        //    CspParameters cspParams = new CspParameters(1);
        //    cspParams.KeyContainerName = ContainerName;
        //    cspParams.Flags = CspProviderFlags.UseMachineKeyStore;

        //    var rsa = new RSACryptoServiceProvider(cspParams) { PersistKeyInCsp = true };
        //}

        //public void DeleteKeyInCsp()
        //{
        //    var cspParams = new CspParameters { KeyContainerName = ContainerName };
        //    var rsa = new RSACryptoServiceProvider(cspParams) { PersistKeyInCsp = false };

        //    rsa.Clear();
        //}

        //public byte[] EncryptData(byte[] dataToEncrypt)
        //{
        //    byte[] cipherbytes;

        //    var cspParams = new CspParameters { KeyContainerName = ContainerName };

        //    using (var rsa = new RSACryptoServiceProvider(2048, cspParams))
        //    {
        //        cipherbytes = rsa.Encrypt(dataToEncrypt, false);
        //    }

        //    return cipherbytes;
        //}

        //public byte[] DecryptData(byte[] dataToDecrypt)
        //{
        //    byte[] plain;

        //    var cspParams = new CspParameters { KeyContainerName = ContainerName };

        //    using (var rsa = new RSACryptoServiceProvider(2048, cspParams))
        //    {
        //        plain = rsa.Decrypt(dataToDecrypt, false);
        //    }

        //    return plain;
        //}

    }
}
