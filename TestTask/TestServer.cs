using System;
using System.Net;
using TestTask.Net.Server;

namespace TestTask
{
	public class TestServer
	{
		static public void Main(string[] args)
		{
			Console.Write("Enter a port number: ");
			int port = int.Parse (Console.ReadLine());
			IPAddress address = Dns.GetHostEntry("localhost").AddressList[0];
			Server server = new EncryptionServer(address,port);
			server.Start();
		}
	}
}
