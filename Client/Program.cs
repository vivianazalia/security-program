using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

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
