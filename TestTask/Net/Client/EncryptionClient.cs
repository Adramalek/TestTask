using System;
using System.Text;
using System.Net;
using TestTask.Net.Config;
using TestTask.Net.Client.Config;
using TestTask.Util.Validation;
using TestTask.Util;

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
			string answer = "", message = ""; bool correct = false;
			Validator<string> validator = new Validator<string>();
			AcceptEvent first = (variant) => message = variant == 1 ? lastMessage : Console.ReadLine();
			AcceptEvent second = (variant) => message = variant == 1 ? config.EncFlag + message : config.DecFlag + message;
			validator.onFail += () => Console.Write("Invalid input. Try again: ");
			validator.onAccept += first;
			do
			{
				Console.Write("Choose what to do: load last or write new message [l/w]: ");
				answer = Console.ReadLine();
				correct = validator.Validate(answer.ToLower(),"l","w");
			} while (!correct);
			correct = false;
			validator.onAccept -= first;
			validator.onAccept += second;
			do
			{
				Console.Write("Choose what to do: encrypt or decrypt [e/d]: ");
				answer = Console.ReadLine();
				correct = validator.Validate(answer,"e","d");
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
