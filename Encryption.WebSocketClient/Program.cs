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
            Encryption.Console.RSAWithCsp rsaWithCsp = new Encryption.Console.RSAWithCsp();
            Console.WriteLine("Starting echo client...");

            int port = 1234;
            TcpClient client = new TcpClient("localhost", port);
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            var rsa = new Encryption.Console.RSAWithCsp();

            var newKeys = rsa.AssignNewKey();

            byte[] encrypted = rsa.Encrypt(newKeys["public"], "Test");
            string decrypt = rsa.Decrypt(newKeys["private"], encrypted);



            var workflow = 0;
            var aesKey = "";
            string fromServer = "";

            while (true)
            {
                while (fromServer != null)
                {
                    switch (workflow)
                    {
                        case 0:
                            Console.WriteLine("Press key to send public key to server");

                            var clientPublicKey = newKeys["public"];
                            writer.WriteLine(clientPublicKey);

                            workflow = 1;
                            break;
                        case 1:
                            Console.WriteLine("Get aesKey from server");

                            fromServer = reader.ReadLine();

                            aesKey = rsa.Decrypt(newKeys["private"], ASCIIEncoding.ASCII.GetBytes(fromServer));

                            Console.WriteLine(aesKey);

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
