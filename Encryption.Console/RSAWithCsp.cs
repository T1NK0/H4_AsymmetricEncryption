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
        public Dictionary<string, string> AssignNewKey()
        {
            Dictionary<string, string> newKey = new Dictionary<string, string>();

            const int PROVIDER_RSA_FULL = 1;
            string CONTAINER_NAME = Guid.NewGuid().ToString();
            CspParameters cspParams;
            cspParams = new CspParameters(PROVIDER_RSA_FULL);
            cspParams.KeyContainerName = CONTAINER_NAME;
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";
            rsa = new RSACryptoServiceProvider(cspParams);

            //Pair of public and private key as XML string.
            //Do not share this to other party
            publicPrivateKeyXML = rsa.ToXmlString(true);
            newKey.Add("private", publicPrivateKeyXML);

            //Private key in xml file, this string should be share to other parties
            publicOnlyKeyXML = rsa.ToXmlString(false);
            newKey.Add("public", publicOnlyKeyXML);

            return newKey;
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
    }
}
