using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Shared;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string ServerIp = "127.0.0.1";
                if (args.Length > 0)
                    ServerIp = args[0];

                Console.Title = $"Client to {ServerIp}:{Globals.Port}";
                string textToSend = DateTime.Now.ToString();
                textToSend = File.ReadAllText("ClientData.txt");
                TcpClient client = new TcpClient();
                client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);
                client.ReceiveBufferSize = Globals.ReceiveBufferSize;

                client.Connect(IPAddress.Parse(ServerIp), Globals.Port);

                NetworkStream nwStream = client.GetStream();
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                //---send the text---
                Console.WriteLine("Sending : " + textToSend.Length);
                client.SendBufferSize = bytesToSend.Length;
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                nwStream.Flush();
                Console.WriteLine("Ready waiting for enter");
                Console.ReadLine();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
