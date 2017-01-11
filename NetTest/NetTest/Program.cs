using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace ConsoleApplication3
{
    class Program
    {
        private static void CheckIp(string ip, string[] ports)
        {
            foreach (string port in ports)
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
                Socket server = new Socket(AddressFamily.InterNetwork,
                                  SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server.Connect(ipep);
                    server.Disconnect(false);
                    Console.WriteLine(ipep.Address.ToString() + ":" + ipep.Port.ToString() + ":OK");
                }
                catch (Exception e)
                {
                    Console.WriteLine(ipep.Address.ToString() + ":" + ipep.Port.ToString() + ":FAILED:" + e.Message);
                }
            }
        }

        private static void CheckConnect(string line)
        {

            string[] parts = line.Split(' ');
            string[] ports;
            string ip;

            if (parts.Length == 1)
            {
                Console.WriteLine(line);
            }
            if (parts.Length > 1)
            {
                ip = parts[0];
                ports = parts[1].Split(',');

                if (parts[0] != "//")
                {

                    Regex rex = new Regex(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$");
                    string ip_ = ip;

                    if (!(rex.Match(ip)).Success)
                    {
                        Console.WriteLine("Checking IP's for " + ip);
                        try
                        {
                            IPAddress[] ips = Dns.GetHostAddresses(ip);
                            foreach (IPAddress curAdd in ips)
                            {
                                CheckIp(curAdd.ToString(), ports);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Checking for IP's for " + ip + " failed with " + e.Message);
                        }

                    }
                    else
                    {
                        CheckIp(ip, ports);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            // System.Configuration.ConfigurationSettings.AppSettings[ ConfigurationManager  C = new ConfigurationSettings()

            string server_inputfile = "servers.txt";

            if (args.Length < 0)
            {
                server_inputfile = args[0];
            }
            string[] lines = System.IO.File.ReadAllLines(@server_inputfile);
            Console.WriteLine("Reading server list from " + server_inputfile);

            foreach (string line in lines)
            {
                CheckConnect(line);
            }
        }
    }
}
