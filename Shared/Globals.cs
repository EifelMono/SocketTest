using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Shared
{
    public static class Globals
    {
        public const string AssemblyVersion = "1.0.0.1";
        public const string AssemblyVersioAssemblyFileVersion = AssemblyVersion;

        public static int Port = 9898;
        public static string IpAddrss = "127.0.0.1";
        public static int ReceiveBufferSize = 4194304; // 4MB Puffer (Default ist 64KB...)
        public static int TransferSize = -1;

        public static Stopwatch Stopwatch = null;

        public static bool Server = false;
        public static void Init(bool server)
        {
            Server = server;

            Stopwatch = Stopwatch.StartNew();
            Console.Title = $"{(Server ? "Server" : "Client")} ({Assembly.GetExecutingAssembly().GetName().Version}): {IpAddrss}:{Globals.Port}";

        }

        public static void Done()
        {
            Console.WriteLine($"Ready waiting for enter {Stopwatch.ElapsedMilliseconds}");
            Console.ReadLine();
        }

        public static void ParseCommandLine(string[] args)
        {
            IpAddrss = "127.0.0.1";
            if (args.Length > 0)
            {
                if (args[0] != "-1")
                    IpAddrss = args[0];
            }

            if (args.Length > 1)
            {
                var value = (int)Convert.ToInt64(args[1]);
                if (value != -1)
                    ReceiveBufferSize = value;
            }
            if (args.Length > 2)
            {
                var value = (int)Convert.ToInt64(args[2]);
                if (value != -1)
                    TransferSize = value;
            }
            Console.WriteLine($"args[0]:IpAddress={IpAddrss}\r\nargs[1]:ReceiveBufferSize={ReceiveBufferSize}\r\nargs[2]:TransferSize={TransferSize}");
        }
        public static void Send(TcpClient client, string text)
        {
            Console.WriteLine("SendText Length: " + text.Length);
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);
            client.ReceiveBufferSize = Globals.ReceiveBufferSize;
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(text);
            Console.WriteLine("SendBytes Length: " + bytesToSend.Length);
            client.SendBufferSize = bytesToSend.Length;
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            nwStream.Flush();
        }

        public static string Receive(TcpClient client)
        {
            Console.WriteLine("Waiting for Datas");
            var result = "";
            var mem = new MemoryStream();
            client.ReceiveBufferSize = Globals.ReceiveBufferSize;
            int bytesRead = 0;
            int totalyBytesRead = 0;
            NetworkStream nwStream = client.GetStream();
            var ready = false;
            var first = true;
            while (!ready)
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];
                try
                {
                    bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
                }
                catch { }
                if (bytesRead != 0)
                {
                    first = false;
                    totalyBytesRead += bytesRead;
                    mem.Write(buffer, 0, bytesRead);
                    Thread.Sleep(100);
                }
                if (!first)
                    ready = (bytesRead != client.ReceiveBufferSize);
            }

            Console.WriteLine($"ReceivedBytes Lenght: {totalyBytesRead}");

            mem.Position = 0;
            using (StreamReader reader = new StreamReader(mem, Encoding.UTF8))
                result = reader.ReadToEnd();
            Console.WriteLine($"ReceivedText Length: {result.Length}");
            return result;
        }
    }
}
