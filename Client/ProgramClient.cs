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

                Console.Title = $"Cl{ServerIp}:{Globals.Port}";
                string sText = File.ReadAllText("ClientData.txt");
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(ServerIp), Globals.Port);

                Globals.Send(client, sText);
                Console.WriteLine("Send read no wait for receive");
                string rText= Globals.Receive(client);
                File.WriteAllText("ClientDataReceived.Txt", rText);

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
