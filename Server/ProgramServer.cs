using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
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
                if (args.Length > 0)
                    Globals.ReceiveBufferSize = (int)Convert.ToInt64(args[0]);
                TcpListener listener = new TcpListener(IPAddress.Any, Globals.Port);
                Console.Title = $"Server ({Assembly.GetExecutingAssembly().GetName().Version}): {Globals.Port}";
                Console.WriteLine("Listening...");
                listener.Start();

                TcpClient client = listener.AcceptTcpClient();
                var stopwatch = Stopwatch.StartNew();

                var text = Globals.Receive(client);
                File.WriteAllText("ServerData.Txt", text);

                Console.WriteLine("Received Send Back");
                Globals.Send(client, text);

                client.Close();
                listener.Stop();
                Console.WriteLine($"Ready waiting for enter {stopwatch.ElapsedMilliseconds}");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
