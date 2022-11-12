// This code is adapted from a sample found at the URL 
// "http://blogs.msdn.com/b/jmanning/archive/2004/12/19/325699.aspx"

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace TcpEchoClient
{
    class TcpEchoClient
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting echo client...");

            int port = 1234;
            TcpClient client = new TcpClient("localhost", port);
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            var rsa = new Encryption.Console.RSAWithCsp();
            var aes = new Encryption.Console.AESEncryption();

            var newKeys = rsa.AssignNewKey();

            var workflow = 0;
            var aesKey = "";
            var ivKey = "";
            string fromServer = "";

            //var encrypted = rsa.Encrypt(newKeys["private"], "This is a test of rsa encryption.");
            //var decrypted = rsa.Decrypt(newKeys["private"], encrypted);

            //Console.WriteLine("Encrypted: ");
            //Console.WriteLine(encrypted + "\n");

            //Console.WriteLine("Decrypted: ");
            //Console.WriteLine(decrypted);
            //Console.WriteLine("");


            while (true)
            {
                switch (workflow)
                {
                    case 0:
                        var clientPublicKey = newKeys["public"];
                        writer.WriteLine(clientPublicKey);

                        fromServer = reader.ReadLine();

                        Console.WriteLine(fromServer);
                        workflow = 2;
                        break;
                    case 2:
                        fromServer = reader.ReadLine();
                        if (fromServer.Contains("server 1"))
                        {
                            fromServer = fromServer.Substring(7);

                            Console.WriteLine("Requesting AESKey");
                            writer.WriteLine("Client requesting aesKey");

                            aesKey = rsa.Decrypt(newKeys["private"], fromServer);

                            writer.WriteLine("Client got key");

                            workflow = 4;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
