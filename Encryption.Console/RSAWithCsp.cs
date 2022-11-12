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
            cspParams.Flags = CspProviderFlags.NoFlags;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(4096, cspParams))
            {
                //Pair of public and private key as XML string.
                //Do not share this to other party
                publicPrivateKeyXML = rsa.ToXmlString(true);
                newKey.Add("private", publicPrivateKeyXML);

                //Private key in xml file, this string should be share to other parties
                publicOnlyKeyXML = rsa.ToXmlString(false);
                newKey.Add("public", publicOnlyKeyXML);
            }
            return newKey;
        }
        
        public string Encrypt(string publicKeyXML, string dataToDycript)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKeyXML);
            var temp = rsa.Encrypt(Encoding.UTF8.GetBytes(dataToDycript), false);
            return Convert.ToBase64String(temp);
        }

        public string Decrypt(string publicPrivateKeyXML, string encryptedData)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicPrivateKeyXML);

            var temp = rsa.Decrypt(Encoding.UTF8.GetBytes(encryptedData), false);

            return Convert.ToBase64String(temp);
        }
    }
}
