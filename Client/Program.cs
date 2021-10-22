using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 8888);
                string msg = "Hello from client";

                int byteCount = Encoding.ASCII.GetByteCount(msg + 1);
                byte[] sendData = new byte[byteCount];
                sendData = Encoding.ASCII.GetBytes(msg);

                NetworkStream stream = client.GetStream();
                stream.Write(sendData, 0, sendData.Length);

                StreamReader read = new StreamReader(stream);
                string response = read.ReadLine();
                Console.WriteLine(response);

                stream.Close();
                client.Close();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to connect...");
            }
        }
    }
}
