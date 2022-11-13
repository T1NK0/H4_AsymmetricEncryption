// This code is adapted from a sample found at the URL 
// "http://blogs.msdn.com/b/jmanning/archive/2004/12/19/325699.aspx"

using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace TcpEchoServer
{
    public class TcpEchoServer
    {
        public static void Main()
        {
            Console.WriteLine("Starting echo server...");

            int port = 1234;
            TcpListener listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();

            TcpClient client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            var rsa = new Encryption.Console.RSAWithXML();
            var aesClass = new Encryption.Console.AESEncryption();

            //RETURN ENCRYPTED RSA KEYS
            Aes aes = Aes.Create();
            aes.Key = aesClass.CreateKeyWithUserInput(32);
            aes.IV = aesClass.CreateKeyWithUserInput(16);
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;

            byte[] symmetricKey;

            Console.WriteLine("Welcome! I only speak encryption, please send me your key!");

            var workflow = 1;
            var clientPublicKey = "";
            var fromClient = "";
            var textDecrypted = "";
            while (true)
            {
                fromClient = reader.ReadLine();
                switch (workflow)
                {
                    case 1:
                        if (fromClient.Contains("<RSAKeyValue>"))
                        {
                            clientPublicKey = fromClient;

                            Console.WriteLine(clientPublicKey);
                            writer.WriteLine("Public RSA key received!");

                            //RETURN ENCRYPTED RSA KEYS
                            byte[] encryptedAesKey = rsa.Encrypt(clientPublicKey, aes.Key);
                            writer.WriteLine("case2" + Convert.ToBase64String(encryptedAesKey));

                            workflow = 3;
                        }
                        break;
                    case 3:
                        if (fromClient.Contains("case3"))
                        {
                            fromClient = fromClient.Substring(5);

                            byte[] encryptedAesIv = rsa.Encrypt(clientPublicKey, aes.IV);
                            writer.WriteLine("case4" + Convert.ToBase64String(encryptedAesIv));

                            workflow = 5;
                        }
                        break;
                    case 5:
                        if (fromClient.Contains("case5"))
                        {
                            fromClient = fromClient.Substring(5);

                            textDecrypted = aesClass.DecryptStringFromBytes(Convert.FromBase64String(fromClient), aes.Key, aes.IV);

                            Console.WriteLine("Client says: " + textDecrypted);

                            writer.WriteLine("case6"+ Convert.ToBase64String(aesClass.EncryptStringToBytes("Hello Client, welcome in!", aes.Key, aes.IV)));

                            workflow = 7;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
