using System;
namespace TestTask.Net.Client.Config
{
	public class ClientConfig
	{
		public static readonly ClientConfig DEFAULT_CONFIG = new ClientConfig();

		public const int DEFAULT_BUFFER_SIZE = 1024;
		public const string DEFAULT_STOP_CONN_STR = "<END>";

		public int BufferSize { get; private set; }
		public string StopConnStr { get; private set; }

		public ClientConfig(int bufferSize = DEFAULT_BUFFER_SIZE,
		                    string stopConnStr = DEFAULT_STOP_CONN_STR)
		{
			BufferSize = bufferSize;
			StopConnStr = stopConnStr;
		}
	}
}
