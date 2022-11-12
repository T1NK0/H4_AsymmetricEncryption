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


            var rsa = new Encryption.Console.RSAWithCsp();

            var aes = new Encryption.Console.AESEncryption();
            //var aesKey = aes.CreateKeyWithUserInput(32);
            var aesKey = "Ole";
            //var aesIv = aes.CreateKeyWithUserInput(16);
            var aesIv = "Ulla";

            Console.WriteLine("Welcome! I only speak encryption, please send me your key!");

            var workflow = 1;
            var clientPublicKey = "";
            string fromClient = "";
            while (true)
            {
                //fromClient = reader.ReadLine();
                //writer.WriteLine("Echoing string: " + fromClient);
                //Console.WriteLine("Echoing string: " + fromClient);
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
                            SymmetricAlgorithm mySymetricAlgorithm = Aes.Create();
                            string encryptedAesKey = rsa.Encrypt(clientPublicKey, aesIv + ";" + aesKey);

                            writer.WriteLine("server 1" + encryptedAesKey);

                            workflow = 3;
                        }
                        break;
                    case 3:
                        if (fromClient.Contains("Client requesting aesKey"))
                        {
                            Console.WriteLine(fromClient);

                            Console.WriteLine("Making AES KEY, and sending to client.");
                            
                            workflow = 5;
                        }

                        break;
                    default:
                        break;
                }
            }
        }
    }
}
