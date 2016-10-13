using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Shared;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Globals.Init(server: false);
                Globals.ParseCommandLine(args);

                string sText = File.ReadAllText("ClientData.txt");
                if (Globals.TransferSize != -1)
                    sText = sText.Substring(0, Globals.TransferSize);
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(Globals.IpAddrss), Globals.Port);
                Globals.Send(client, sText);

                Console.WriteLine("Send read no wait for receive, Waiting 5 Seconds");

                string rText = Globals.Receive(client);
                File.WriteAllText("ClientDataReceived.Txt", rText);

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
