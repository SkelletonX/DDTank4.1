using Game.Base.Packets;
using log4net;
using System;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace Game.Base
{
	public class BaseClient
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected Socket m_sock;

		protected byte[] m_readBuffer;

		protected int m_readBufEnd;

		private SocketAsyncEventArgs rc_event;

		private int m_isConnected;

		public bool IsClientPacketSended = true;

		public int numPacketProcces;

		protected byte[] m_sendBuffer;

		private bool m_encryted;

		private bool m_strict;

		private bool m_asyncPostSend;

		public StreamProcessor m_processor;

		public byte[] RECEIVE_KEY;

		public byte[] SEND_KEY;

		public Socket Socket
		{
			get
			{
				return m_sock;
			}
			set
			{
				m_sock = value;
			}
		}

		public byte[] PacketBuf => m_readBuffer;

		public bool IsConnected => m_isConnected == 1;

		public int PacketBufSize
		{
			get
			{
				return m_readBufEnd;
			}
			set
			{
				m_readBufEnd = value;
			}
		}

		public string TcpEndpoint
		{
			get
			{
				Socket sock = m_sock;
				if (sock != null && sock.Connected && sock.RemoteEndPoint != null)
				{
					return sock.RemoteEndPoint.ToString();
				}
				return "not connected";
			}
		}

		public byte[] SendBuffer => m_sendBuffer;

		public bool Encryted
		{
			get
			{
				return m_encryted;
			}
			set
			{
				m_encryted = value;
			}
		}

		public bool Strict
		{
			get
			{
				return m_strict;
			}
			set
			{
				m_strict = value;
			}
		}

		public bool AsyncPostSend
		{
			get
			{
				return m_asyncPostSend;
			}
			set
			{
				m_asyncPostSend = value;
			}
		}

		public event ClientEventHandle Connected;

		public event ClientEventHandle Disconnected;

		public virtual void OnRecv(int num_bytes)
		{
			m_processor.ReceiveBytes(num_bytes);
		}

		public virtual void OnRecvPacket(GSPacketIn pkg)
		{
		}

		protected virtual void OnConnect()
		{
			if (Interlocked.Exchange(ref m_isConnected, 1) == 0 && this.Connected != null)
			{
				this.Connected(this);
			}
		}

		protected virtual void OnDisconnect()
		{
			if (this.Disconnected != null)
			{
				this.Disconnected(this);
			}
		}

		public BaseClient(byte[] readBuffer, byte[] sendBuffer)
		{
			m_readBuffer = readBuffer;
			m_sendBuffer = sendBuffer;
			m_readBufEnd = 0;
			rc_event = new SocketAsyncEventArgs();
			rc_event.Completed += RecvEventCallback;
			m_processor = new StreamProcessor(this);
			m_encryted = false;
			m_strict = true;
		}

		public void SetFsm(int adder, int muliter)
		{
			m_processor.SetFsm(adder, muliter);
		}

		public void ReceiveAsync()
		{
			ReceiveAsyncImp(rc_event);
		}

		private void ReceiveAsyncImp(SocketAsyncEventArgs e)
		{
			if (m_sock != null && m_sock.Connected)
			{
				int num = m_readBuffer.Length;
				if (m_readBufEnd >= num)
				{
					if (log.IsErrorEnabled)
					{
						log.Error(TcpEndpoint + " disconnected because of buffer overflow!");
						log.Error("m_pBufEnd=" + m_readBufEnd + "; buf size=" + num);
						log.Error(m_readBuffer);
					}
					Disconnect();
				}
				else
				{
					e.SetBuffer(m_readBuffer, m_readBufEnd, num - m_readBufEnd);
					if (!m_sock.ReceiveAsync(e))
					{
						RecvEventCallback(m_sock, e);
					}
				}
			}
			else
			{
				Disconnect();
			}
		}

		private void RecvEventCallback(object sender, SocketAsyncEventArgs e)
		{
			try
			{
				int bytesTransferred = e.BytesTransferred;
				if (bytesTransferred > 0)
				{
					OnRecv(bytesTransferred);
					ReceiveAsyncImp(e);
				}
				else
				{
					log.InfoFormat("Disconnecting client ({0}), received bytes={1}", TcpEndpoint, bytesTransferred);
					Disconnect();
				}
			}
			catch (Exception arg)
			{
				log.ErrorFormat("{0} RecvCallback:{1}", TcpEndpoint, arg);
				Disconnect();
			}
		}

		public virtual void SendTCP(GSPacketIn pkg)
		{
			m_processor.SendTCP(pkg);
		}

		public bool SendAsync(SocketAsyncEventArgs e)
		{
			int tickCount = Environment.TickCount;
			bool result = true;
			if (m_sock.Connected)
			{
				result = m_sock.SendAsync(e);
			}
			int num = Environment.TickCount - tickCount;
			if (num > 1500)
			{
				log.WarnFormat("AsyncTcpSendCallback.BeginSend took {0}ms! (TCP to client: {1})", num, TcpEndpoint);
			}
			return result;
		}

		protected void CloseConnections()
		{
			if (m_sock != null)
			{
				try
				{
					m_sock.Shutdown(SocketShutdown.Both);
				}
				catch
				{
				}
				try
				{
					m_sock.Close();
				}
				catch
				{
				}
			}
		}

		public virtual bool Connect(Socket connectedSocket)
		{
			m_sock = connectedSocket;
			if (m_sock.Connected)
			{
				if (Interlocked.Exchange(ref m_isConnected, 1) == 0)
				{
					OnConnect();
				}
				return true;
			}
			return false;
		}

		public virtual void Disconnect()
		{
			try
			{
				if (Interlocked.Exchange(ref m_isConnected, 0) == 1)
				{
					CloseConnections();
					OnDisconnect();
					rc_event.Dispose();
					m_processor.Dispose();
				}
			}
			catch (Exception exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Exception", exception);
				}
			}
		}

		public virtual void DisplayMessage(string msg)
		{
		}

		public virtual void resetKey()
		{
			RECEIVE_KEY = StreamProcessor.cloneArrary(StreamProcessor.KEY);
			SEND_KEY = StreamProcessor.cloneArrary(StreamProcessor.KEY);
		}

		public virtual void setKey(byte[] data)
		{
			for (int i = 0; i < 8; i++)
			{
				RECEIVE_KEY[i] = data[i];
				SEND_KEY[i] = data[i];
			}
		}
	}
}
