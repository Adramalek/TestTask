using System.Linq;
using System.Net;
using System.Net.Sockets;
using TestTask.Net.Client.Config;

namespace TestTask.Net.Client
{
	public abstract class Client
	{
		private ClientConfig config;

		public event StopEvent onCloseConnection = delegate { };

		protected Client(ClientConfig config = null)
		{
			if (config == null) this.config = ClientConfig.DEFAULT_CONFIG;
			else this.config = config;
		}

		public abstract void Connect(IPAddress address, int port);

		public void Connect<T>(IPAddress address,
		                       int port,
		                       AccessToData<T> accessToData,
		                       GetBytes<T> bytesGetter,
		                       StoreAction<T> store,
		                       ActionWithData<T> action,
		                       StopCondition<T> closeConnectionCondition) where T : class
		{
			IPEndPoint ipEndPoint = new IPEndPoint(address,port);
			Socket sender = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			sender.Connect(ipEndPoint);
			byte[] buffer = new byte[config.BufferSize];
			while (true)
			{
				T data = accessToData();
				if (closeConnectionCondition(data))
				{
					onCloseConnection();
					sender.Shutdown(SocketShutdown.Both);
					sender.Close();
					return;
				}
				byte[] dataBytes = bytesGetter(data);
				int bytesRest = dataBytes.Length;
				while (bytesRest > 0)
				{
					int bytesSent = sender.Send(dataBytes);
					dataBytes = dataBytes.Skip(bytesSent).ToArray();
					bytesRest -= bytesSent;
				}
				while (sender.Available > 0)
				{
					int byteRecived = sender.Receive(buffer);
					data = store(buffer, byteRecived, data);
				}
				data = action(data);
			}
		}
	}
}
