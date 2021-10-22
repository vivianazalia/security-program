using System;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener tcpListener = new TcpListener(System.Net.IPAddress.Any, 8888);
            tcpListener.Start();

            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("Client Accepted");
                NetworkStream stream = client.GetStream();

                StreamReader read = new StreamReader(stream);
                StreamWriter write = new StreamWriter(stream);

                try
                {
                    byte[] buffer = new byte[1024];
                    stream.Read(buffer, 0, buffer.Length);
                    int recv = 0;
                    foreach(byte b in buffer)
                    {
                        if (b != 0)
                        {
                            recv++;
                        }
                    }

                    string request = Encoding.UTF8.GetString(buffer, 0, recv);
                    Console.WriteLine("Request received : " + request);
                    write.Flush();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Something went wrong");
                    write.WriteLine(e.ToString());
                }
            }
        }
    }
}
