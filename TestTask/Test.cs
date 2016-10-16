using System;
using System.Net;
using TestTask.Net.Server;
using TestTask.Net.Client;
using TestTask.Util.Validation;

namespace TestTask
{
	public class Test
	{
		static public void Main(string[] args)
		{
			bool correctAnswer;
			string answer;
			Validator<string> validator = new Validator<string>();
			validator.onFail += () => Console.WriteLine("Invalid input. Try again");
			validator.onAccept += (variant) => {
				if (variant == 1) RunServer();
				else RunClient();
			};
			do
			{
				Console.Write("Choose what to run: server or client [s/c]? ");
				answer = Console.ReadLine();
				correctAnswer = validator.Validate(answer, "s", "c");
			} while (!correctAnswer);
		}

		private static void RunServer()
		{
			Console.Write("Enter a port number: ");
			int port = int.Parse(Console.ReadLine());
			IPAddress address = Dns.GetHostEntry("localhost").AddressList[0];
			Server server = new EncryptionServer(address, port);
			server.Start();
		}

		private static void RunClient()
		{
			Console.Write("Enter a port number: ");
			int port = int.Parse(Console.ReadLine());
			IPAddress address = Dns.GetHostEntry("localhost").AddressList[0];
			Client client = new EncryptionClient();
			client.Connect(address, port);
		}
	}
}
