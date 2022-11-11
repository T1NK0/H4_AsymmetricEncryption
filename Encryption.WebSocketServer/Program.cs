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
            StreamWriter writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };
            StreamReader reader = new StreamReader(stream, Encoding.ASCII);


            var rsa = new Encryption.Console.RSAWithCsp();
            var newKeys = rsa.AssignNewKey();

            var aes = new Encryption.Console.AESEncryption();
            var aesKey = aes.CreateKeyWithUserInput(32);
            var aesIv = aes.CreateKeyWithUserInput(16);

            Console.WriteLine("Welcome! I only speak encryption, please send me your key!");

            //string decrypt = rsa.Decrypt(newKeys["private"], encrypted);

            //Console.WriteLine("Hello i only speak encryption, please send me your public RSA key.");
            ////var clientKey = reader.ReadLine();

            //Console.WriteLine("Send me your key and iv for AES");

            var workflow = 0;
            var clientPublicKey = "";
            string fromClient = "";

            while (true)
            {
                string inputLine = "";
                while (inputLine != null)
                {
                    switch (workflow)
                    {
                        case 0:
                            Console.WriteLine("Key received:");

                            clientPublicKey = reader.ReadLine();
                            Console.WriteLine(clientPublicKey);

                            SymmetricAlgorithm mySymetricAlgorithm = Aes.Create();
                            workflow = 1;
                            break;
                        case 1:
                            byte[] encryptedAesKey = rsa.Encrypt(clientPublicKey, aesIv + ";" + aesKey);
                            writer.WriteLine(encryptedAesKey);


                            Console.WriteLine(ASCIIEncoding.ASCII.GetString(encryptedAesKey));

                            Console.WriteLine(workflow);

                            workflow = 2;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
