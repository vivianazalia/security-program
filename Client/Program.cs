using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using ExpressEncription;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 8888);
                string msg = Console.ReadLine();

                int byteCount = Encoding.ASCII.GetByteCount(msg + 1);
                byte[] sendData = new byte[byteCount];
                sendData = Encoding.ASCII.GetBytes(msg);

                NetworkStream stream = client.GetStream();
                stream.Write(sendData, 0, sendData.Length);
                Console.WriteLine("Message was sent and stream is closed");

                StreamReader read = new StreamReader(stream);
                string response = read.ReadLine();
                Console.WriteLine(response);

                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to connect...");
            }
        }
    }

    class Security
    {
        string publicClientKeyPath = @"D:\Project\security-program\Client\Keypath\public.key";
        string privateClientKeyPath = @"D:\Project\security-program\Client\Keypath\private.key";

        public void GenerateKey()
        {
            ExpressEncription.RSAEncription.MakeKey(publicClientKeyPath, privateClientKeyPath);
        }

        public void Encrypt()
        {

        }

        public void Decrypt()
        {

        }
    }
}
