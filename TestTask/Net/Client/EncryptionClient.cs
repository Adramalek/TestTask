using System;
using System.Text;
using System.Net;
using TestTask.Net.Config;
using TestTask.Net.Client.Config;

namespace TestTask.Net.Client
{
	public class EncryptionClient : Client
	{
		private EncryptionConfig config;
		private string lastMessage;

		public EncryptionClient(ClientConfig clientConfig = null,
		                        EncryptionConfig config = null) : base (clientConfig)
		{
			if (this.config == null) this.config = EncryptionConfig.DEFAULT_CONFIG;
			else this.config = config;
		}

		public override void Connect(IPAddress address, int port)
		{
			Connect(address,
			        port,
			        GetMessage,
			        (data) => Encoding.UTF8.GetBytes(data.ToString()),
			        (data, bytesRecieve, update) => update.Append(Encoding.UTF8.GetString(data, 0, bytesRecieve)),
			        Action,
			        (data) => data.ToString().Equals(config.StopCondStr));
		}

		private StringBuilder GetMessage()
		{
			Console.Write("Choose what to do: load last or write new message [l/w]: ");
			string answer; bool correct = false;
			do
			{
				answer = Console.ReadLine();
				bool isL = answer.ToLower().Equals("l");
				bool isW = answer.ToLower().Equals("w");
				correct = isL || isW;
				if (!correct)
				{
					Console.Write("Invalid input. Try again: ");
					continue;
				}
				if (isL){ answer = lastMessage; }
				if (isW){ answer = Console.ReadLine(); }
			} while (!correct);
			Console.Write("Choose what to do: encrypt or decrypt [e/d]: ");
			correct = false;
			do
			{
				answer = Console.ReadLine();
				bool isE = answer.ToLower().Equals("e");
				bool isD = answer.ToLower().Equals("d");
				correct = isE || isD;
				if (!correct)
				{
					Console.Write("Invalid input. Try again: ");
					continue;	
				}
				if (isE) { answer = config.EncFlag + answer; }
				if (isD) { answer = config.DecFlag + answer; }
			} while (!correct);
			StringBuilder builder = new StringBuilder().Append(answer);
			return builder;
		}

		private StringBuilder Action(StringBuilder data)
		{
			char flag = data[0];
			bool enc = false;
			if (flag == '+') enc = true;
			data = data.Remove(0,1);
			string message = "Showing " + (enc ? "en" : "de") + "crypted message:\n"+data;
			Console.WriteLine(message);
			lastMessage = data.ToString();
			return data;
		}
	}
}
