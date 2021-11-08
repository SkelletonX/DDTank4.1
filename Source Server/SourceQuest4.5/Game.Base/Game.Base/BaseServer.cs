using log4net;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Game.Base
{
	public class BaseServer
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static readonly int SEND_BUFF_SIZE = 16384;

		protected readonly HybridDictionary _clients = new HybridDictionary();

		protected Socket _linstener;

		protected SocketAsyncEventArgs ac_event;

		public int ClientCount => _clients.Count;

		public BaseServer()
		{
			ac_event = new SocketAsyncEventArgs();
			ac_event.Completed += AcceptAsyncCompleted;
		}

		private void AcceptAsync()
		{
			try
			{
				if (_linstener != null)
				{
					SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
					socketAsyncEventArgs.Completed += AcceptAsyncCompleted;
					_linstener.AcceptAsync(socketAsyncEventArgs);
				}
			}
			catch (Exception exception)
			{
				log.Error("AcceptAsync is error!", exception);
			}
		}

		private void AcceptAsyncCompleted(object sender, SocketAsyncEventArgs e)
		{
			Socket socket = null;
			try
			{
				socket = e.AcceptSocket;
				socket.SendBufferSize = SEND_BUFF_SIZE;
				BaseClient newClient = GetNewClient();
				try
				{
					if (log.IsInfoEnabled)
					{
						string str = socket.Connected ? socket.RemoteEndPoint.ToString() : "socket disconnected";
						log.Info("Incoming connection from " + str);
					}
					lock (_clients.SyncRoot)
					{
						_clients.Add(newClient, newClient);
						newClient.Disconnected += client_Disconnected;
					}
					newClient.Connect(socket);
					newClient.ReceiveAsync();
				}
				catch (Exception arg)
				{
					log.ErrorFormat("create client failed:{0}", arg);
					newClient.Disconnect();
				}
			}
			catch
			{
				if (socket != null)
				{
					try
					{
						socket.Close();
					}
					catch
					{
					}
				}
			}
			finally
			{
				e.Dispose();
				AcceptAsync();
			}
		}

		private void client_Disconnected(BaseClient client)
		{
			client.Disconnected -= client_Disconnected;
			RemoveClient(client);
		}

		protected virtual BaseClient GetNewClient()
		{
			return new BaseClient(new byte[8192], new byte[8192]);
		}

		public virtual bool InitSocket(IPAddress ip, int port)
		{
			try
			{
				_linstener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				_linstener.Bind(new IPEndPoint(ip, port));
			}
			catch (Exception exception)
			{
				log.Error("InitSocket", exception);
				return false;
			}
			return true;
		}

		public virtual bool Start()
		{
			if (_linstener == null)
			{
				return false;
			}
			try
			{
				_linstener.Listen(100);
				AcceptAsync();
				if (log.IsDebugEnabled)
				{
					log.Debug("Server is now listening to incoming connections!");
				}
			}
			catch (Exception exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Start", exception);
				}
				if (_linstener != null)
				{
					_linstener.Close();
				}
				return false;
			}
			return true;
		}

		public virtual void Stop()
		{
			Console.WriteLine("Stopping server! - Entering method");
			try
			{
				if (_linstener != null)
				{
					Socket linstener = _linstener;
					_linstener = null;
					linstener.Close();
					Console.WriteLine("Server is no longer listening for incoming connections!");
				}
			}
			catch (Exception exception)
			{
				log.Error("Stop", exception);
			}
			if (_clients != null)
			{
				lock (_clients.SyncRoot)
				{
					try
					{
						BaseClient[] array = new BaseClient[_clients.Keys.Count];
						_clients.Keys.CopyTo(array, 0);
						BaseClient[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							array2[i].Disconnect();
						}
						Console.WriteLine("Stopping server! - Cleaning up client list!");
					}
					catch (Exception exception2)
					{
						log.Error("Stop", exception2);
					}
				}
			}
			Console.WriteLine("Stopping server! - End of method!");
		}

		public virtual void RemoveClient(BaseClient client)
		{
			lock (_clients.SyncRoot)
			{
				_clients.Remove(client);
			}
		}

		public BaseClient[] GetAllClients()
		{
			lock (_clients.SyncRoot)
			{
				BaseClient[] array = new BaseClient[_clients.Count];
				_clients.Keys.CopyTo(array, 0);
				return array;
			}
		}

		public void Dispose()
		{
			ac_event.Dispose();
		}
	}
}
