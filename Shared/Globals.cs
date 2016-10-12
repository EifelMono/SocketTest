using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Shared
{
    public static class Globals
    {
        public static int ReceiveBufferSize = 4194304; // 4MB Puffer (Default ist 64KB...)
        public static int Port = 9898;


        public static void Send(TcpClient client, string text)
        {
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);
            client.ReceiveBufferSize = Globals.ReceiveBufferSize;

            NetworkStream nwStream = client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(text);
            //---send the text---
            Console.WriteLine("Sending : " + text.Length);
            client.SendBufferSize = bytesToSend.Length;
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            nwStream.Flush();
        }

        public static string Receive(TcpClient client)
        {
            var result = "";
            var mem = new MemoryStream();
            client.ReceiveBufferSize = Globals.ReceiveBufferSize;
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

            mem.Position = 0;
            using (StreamReader reader = new StreamReader(mem, Encoding.UTF8))
                result = reader.ReadToEnd();
            Console.WriteLine($"Received string: {result.Length}");
            return result;
        }
    }
}
