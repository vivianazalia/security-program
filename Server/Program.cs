using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Encryption encryption = new Encryption();
            TcpListener tcpListener = new TcpListener(System.Net.IPAddress.Any, 8888);
            tcpListener.Start();

            Console.WriteLine("Waiting for a connection...");
            TcpClient client = tcpListener.AcceptTcpClient();
            Console.WriteLine("Client Accepted");

            NetworkStream stream = client.GetStream();

            encryption.SendPublicKeyToConnectedClient(stream);

            StreamReader read = new StreamReader(stream);
            StreamWriter write = new StreamWriter(stream);

            try
            {
                byte[] buffer = new byte[1024];
                stream.Read(buffer, 0, buffer.Length);
                int recv = 0;
                foreach (byte b in buffer)
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
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong");
                write.WriteLine(e.ToString());
            }
        }

        class Encryption
        {
            private static RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);
            private RSAParameters publicKey;
            private RSAParameters privateKey;

            public Encryption()
            {
                publicKey = csp.ExportParameters(false);
                privateKey = csp.ExportParameters(true);
            }

            public string GetPublicKey()
            {
                var sw = new StringWriter();
                var xs = new XmlSerializer(typeof(RSAParameters));
                xs.Serialize(sw, publicKey);
                return sw.ToString();
            }

            public void SendPublicKeyToConnectedClient(NetworkStream stream)
            {
                StreamWriter streamWriter = new StreamWriter(stream);
                streamWriter.Write(GetPublicKey());
            }

            public string Encrypt(string plainText)
            {
                csp = new RSACryptoServiceProvider();
                csp.ImportParameters(publicKey);
                var data = Encoding.Unicode.GetBytes(plainText);
                var chyper = csp.Encrypt(data, false);
                return Convert.ToBase64String(chyper);
            }

            public string Decrypt(string chyperText)
            {
                var dataBytes = Convert.FromBase64String(chyperText);
                csp.ImportParameters(privateKey);
                var plainText = csp.Decrypt(dataBytes, false);
                return Encoding.Unicode.GetString(plainText);
            }
        }
    }
}
