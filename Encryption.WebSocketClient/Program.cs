
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

			while (true)
			{
				Console.Write("Enter text to send: ");
				string lineToSend = Console.ReadLine();
				Console.WriteLine("Sending to server: " + lineToSend);
                rsaWithCsp.AssignNewKey();
                //rsaWithCsp.Encrypt();

                writer.WriteLine(lineToSend);
				string lineReceived = reader.ReadLine();
				Console.WriteLine("Received from server: " + lineReceived);
			}
		}

        //static void Main()
        //{
        //    try
        //    {
        //        //Create a UnicodeEncoder to convert between byte array and string.
        //        UnicodeEncoding ByteConverter = new UnicodeEncoding();

        //        //Create byte arrays to hold original, encrypted, and decrypted data.
        //        byte[] dataToEncrypt = ByteConverter.GetBytes("Data to Encrypt");
        //        byte[] encryptedData;

        //        //Create a new instance of RSACryptoServiceProvider to generate
        //        //public and private key data.
        //        using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
        //        {

        //            //Pass the data to ENCRYPT, the public key information 
        //            //(using RSACryptoServiceProvider.ExportParameters(false),
        //            //and a boolean flag specifying no OAEP padding.
        //            encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);

        //            //Display the decrypted plaintext to the console. 
        //            Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));
        //        }
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        //Catch this exception in case the encryption did
        //        //not succeed.
        //        Console.WriteLine("Encryption failed.");
        //    }
        //}

        //public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        //{
        //    try
        //    {
        //        byte[] encryptedData;
        //        //Create a new instance of RSACryptoServiceProvider.
        //        using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
        //        {

        //            //Import the RSA Key information. This only needs
        //            //toinclude the public key information.
        //            RSA.ImportParameters(RSAKeyInfo);

        //            //Encrypt the passed byte array and specify OAEP padding.  
        //            //OAEP padding is only available on Microsoft Windows XP or
        //            //later.  
        //            encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
        //        }
        //        return encryptedData;
        //    }
        //    //Catch and display a CryptographicException  
        //    //to the console.
        //    catch (CryptographicException e)
        //    {
        //        Console.WriteLine(e.Message);

        //        return null;
        //    }
        //}
    }
}
