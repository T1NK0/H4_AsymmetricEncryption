using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Encryption.Console
{
    public class RSAWithXML
    {
        string publicPrivateKeyXML;
        string publicOnlyKeyXML;
        public Dictionary<string, string> AssignNewKey()
        {
            Dictionary<string, string> newKey = new Dictionary<string, string>();

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(4096))
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
        
        public byte[] Encrypt(string publicKeyXML, byte[] dataToEncrypt)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKeyXML);

            return rsa.Encrypt(dataToEncrypt, true);
        }

        public string Decrypt(string publicPrivateKeyXML, byte[] encryptedData)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicPrivateKeyXML);

            return Convert.ToBase64String(rsa.Decrypt(encryptedData, true));
        }
    }
}
