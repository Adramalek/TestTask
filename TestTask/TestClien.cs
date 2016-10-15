using System;
using System.Net;
using TestTask.Net.Client;

namespace TestTask
{
	public class TestClien
	{
		static public void Main(string[] args)
		{
			Console.Write("Enter a port number: ");
			int port = int.Parse(Console.ReadLine());
			IPAddress address = Dns.GetHostEntry("localhost").AddressList[0];
			Client client = new EncryptionClient();
			client.Connect(address,port);
		}
	}
}
