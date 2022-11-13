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

            var rsa = new Encryption.Console.RSAWithXML();
            var aes = new Encryption.Console.AESEncryption();

            var newKeys = rsa.AssignNewKey();

            var workflow = 0;
            var aesKey = "";
            var aesIv = "";
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
                        
                        workflow = 2;
                        break;
                    case 2:
                        fromServer = reader.ReadLine();
                        if (fromServer.Contains("case2"))
                        {
                            fromServer = fromServer.Substring(5);

                            Console.WriteLine("Requesting AES Key");

                            byte[] convertedKeyToByteArray = Convert.FromBase64String(fromServer);
                            aesKey = rsa.Decrypt(newKeys["private"], (convertedKeyToByteArray));

                            Console.WriteLine(aesKey);

                            writer.WriteLine("case3");

                            workflow = 4;
                        }
                        break;
                    case 4:
                        fromServer = reader.ReadLine();
                        if (fromServer.Contains("case4"))
                        {
                            fromServer = fromServer.Substring(5);

                            Console.WriteLine("Requesting AES IV");


                            byte[] convertedIvToByteArray = Convert.FromBase64String(fromServer);
                            aesIv = rsa.Decrypt(newKeys["private"], (convertedIvToByteArray));

                            Console.WriteLine(aesIv);

                            var textToServer = aes.EncryptStringToBytes("Hello Server, it is i!", Convert.FromBase64String(aesKey), Convert.FromBase64String(aesIv));

                            Console.WriteLine(Convert.ToBase64String(textToServer));
                            writer.WriteLine("case5" + Convert.ToBase64String(textToServer));

                            workflow = 6;
                        }
                        break;
                    case 6:
                        fromServer = reader.ReadLine();
                        if (fromServer.Contains("case6"))
                        {
                            fromServer = fromServer.Substring(5);


                            workflow = 8;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
