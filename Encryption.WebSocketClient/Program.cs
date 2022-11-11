
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
            while (true)
            {
                switch (workflow)
                {
                    case 0:
                        Console.WriteLine("Press key to send public key server");

                        var clientPublicKey = newKeys["public"];
                        writer.WriteLine(clientPublicKey);

                        workflow = 1;
                        break;
                        case 1:

                        break;
                    default:
                        break;
                }

                Console.Write("Enter text to send: ");
                string lineToSend = Console.ReadLine();
                Console.WriteLine("Sending to server: " + lineToSend);

                writer.WriteLine(lineToSend);
                string lineReceived = reader.ReadLine();
                Console.WriteLine("Received from server: " + lineReceived);
            }
        }
    }
}
