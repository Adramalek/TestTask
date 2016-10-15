using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using TestTask.Net.Server.Config;

namespace TestTask.Net.Server
{
	public abstract class Server
	{
		private Socket serverSocket;
		private IPEndPoint ipEndPoint;
		private ServerConfig config;

		public event StartEvent onStart = delegate { };
		public event StopEvent onCloseConnection = delegate { };
		public event StopEvent onStop = delegate { };
		public event ActionEvent beforeDataManipulates = delegate { };
		public event ActionEvent afterDataManipulates = delegate { };

		protected Server(IPAddress addres,
					  int port,
					  ServerConfig config = null)
		{
			ipEndPoint = new IPEndPoint(addres,port);
			serverSocket = new Socket(addres.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			if (config == null) this.config = ServerConfig.DEFAULT_CONFIG;
			else this.config = config;
		}

		public abstract void Start();

		public void Start<T>(StoreAction<T> store,
		                     ActionWithData<T> action,
		                     StopCondition<T> closeConnectionCondition) where T : class
		{
			onStart();
			try
			{
				serverSocket.Bind(ipEndPoint);
				serverSocket.Listen(config.MaxConnections);
				ThreadParams<T> param = new ThreadParams<T>(store,action,closeConnectionCondition);
				Thread connectionListener = new Thread(Listen<T>);
				connectionListener.Start(param);
				string cmd;
				while (true)
				{
					cmd = Console.ReadLine();
					if (cmd.Equals(config.ServerStopCommand)) break;
				}
				connectionListener.Abort("Stopping server.");
				connectionListener.Join();
				serverSocket.Shutdown(SocketShutdown.Both);
				serverSocket.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private void Listen<T>(object obj) where T : class
		{
			if (obj == null){
				throw new ArgumentNullException();
			}
			ThreadParams<T> param = obj as ThreadParams<T>;
			if (param == null){
				throw new InvalidCastException("Cannot cast "+obj+" to ThreadParams<"+typeof(T)+">");
			}
			try
			{
				while (true)
				{
					Console.WriteLine("Waiting for a connection via port {0}", ipEndPoint);
					Socket handler = serverSocket.Accept();
					Console.WriteLine("Server set connection with {0}", handler.LocalEndPoint);
					byte[] bytes = new byte[config.BufferSize];
					int bytesRecieved;
					T data = null;
					while (handler.Available > 0)
					{
						bytesRecieved = handler.Receive(bytes);
						data = param.store(bytes, bytesRecieved, data);
					}
					beforeDataManipulates();
					data = param.action(data);
					afterDataManipulates();
					if (param.closeConnectionCondition(data))
					{
						Console.WriteLine("Server closed the connection with client: {0}", handler.LocalEndPoint);
						onCloseConnection();
						handler.Shutdown(SocketShutdown.Both);
						handler.Close();
					}
				}
			}
			catch (ThreadAbortException e)
			{
				Console.WriteLine(e.ExceptionState);
				onStop();
			}
		}

		private class ThreadParams<T> where T : class
		{
			public StoreAction<T> store;
			public ActionWithData<T> action;
			public StopCondition<T> closeConnectionCondition;

			public ThreadParams(StoreAction<T> store,
			                    ActionWithData<T> action,
			                    StopCondition<T> closeConnectionCondition)
			{
				this.store = store;
				this.action = action;
				this.closeConnectionCondition = closeConnectionCondition;
			}
		}
	}
}
