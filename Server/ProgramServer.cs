using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        internal static int ReceiveBufferSize = 4194304; // 4MB Puffer (Default ist 64KB...)

        static void Main(string[] args)
        {
            try
            {
                int Port = 9898;
                string ServerIp = "127.0.0.1";

                IPAddress ipAddress = IPAddress.Parse(ServerIp);
                TcpListener listener = new TcpListener(ipAddress, Port);
                Console.WriteLine("Listening...");
                listener.Start();

                var mem = new MemoryStream();

                TcpClient client = listener.AcceptTcpClient();
                client.ReceiveBufferSize = ReceiveBufferSize;
                int bytesRead = 0;
                int totalyBytesRead = 0;
                NetworkStream nwStream = client.GetStream();
                do
                {
                    Thread.Sleep(100); // delay!!!
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
                    totalyBytesRead += bytesRead;
                    mem.Write(buffer, 0, bytesRead);
                } while (bytesRead == client.ReceiveBufferSize);

                Console.WriteLine($"Received Bytes: {totalyBytesRead}");

                string dataReceived = "";
                mem.Position = 0;
                using (StreamReader reader = new StreamReader(mem, Encoding.UTF8))
                    dataReceived = reader.ReadToEnd();
                File.WriteAllText("RowaScalaVisuData.txt", dataReceived);
                Console.WriteLine($"Received string: {dataReceived.Length}");

                client.Close();
                listener.Stop();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
