using System;
namespace TestTask.Net.Server.Config
{
	public class ServerConfig
	{
		public static readonly ServerConfig DEFAULT_CONFIG = new ServerConfig();

		public const int DEFAULT_MAX_CONN_NUM = 1;
		public const int DEFAULT_BUFFER_SIZE = 1024;
		public const string DEFAULT_SERVER_STOP_CMD = "Stop";

		public int MaxConnections { get; private set; }
		public int BufferSize { get; private set; }
		public string ServerStopCommand { get; private set; }

		public ServerConfig(int maxConnections = DEFAULT_MAX_CONN_NUM,
		                    int bufferSize = DEFAULT_BUFFER_SIZE,
		                    string serverStopCommand = DEFAULT_SERVER_STOP_CMD)
		{
			MaxConnections = maxConnections;
			BufferSize = bufferSize;
			ServerStopCommand = serverStopCommand;
		}
	}
}
