using Shared;
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
        static void Main(string[] args)
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, Globals.Port);
                Console.Title = $"Server: {Globals.Port}";
                Console.WriteLine("Listening...");
                listener.Start();

            

                TcpClient client = listener.AcceptTcpClient();

                var text = Globals.Receive(client);
                File.WriteAllText("ServerData.Txt", text);

                Console.WriteLine("Received Send Back");
                Globals.Send(client, text);

                client.Close();
                listener.Stop();
                Console.WriteLine("Ready waiting for enter");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
