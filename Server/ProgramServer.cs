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
                Globals.Init(server: true);
                Globals.ParseCommandLine(args);

                TcpListener listener = new TcpListener(IPAddress.Any, Globals.Port);
                Console.WriteLine("Listening...");
                listener.Start();

                TcpClient client = listener.AcceptTcpClient();
                var stopwatch = Stopwatch.StartNew();

                var text = Globals.Receive(client);
                File.WriteAllText("ServerData.Txt", text);

                if (Globals.TransferSize != -1)
                    text = text.Substring(0, Globals.TransferSize);

                Globals.Send(client, text);

                listener.Stop();
                Globals.Done();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
