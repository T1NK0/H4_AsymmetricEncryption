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

			Encryption.Console.RSAWithCsp rsaWithCsp = new Encryption.Console.RSAWithCsp();
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

            Console.WriteLine("Hello i only speak encryption, please send me your public RSA key.");
            //var clientKey = reader.ReadLine();

            Console.WriteLine("Send me your key and iv for AES");

            while (true)
            {
                string inputLine = "";
                while (inputLine != null)
                {
                    inputLine = reader.ReadLine();
                }
                Console.WriteLine("Server saw disconnect from client.");
            }
        }
	}
}
