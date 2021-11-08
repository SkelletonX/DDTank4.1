using log4net;
using Road.Base.Packets;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Game.Base.Packets
{
	public class StreamProcessor
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected readonly BaseClient m_client;

		private FSM send_fsm;

		private FSM receive_fsm;

		private SocketAsyncEventArgs send_event;

		protected object m_object = new object();

		protected byte[] m_tcpSendBuffer;

		protected Queue m_tcpQueue;

		protected bool m_sendingTcp;

		protected int m_firstPkgOffset;

		protected int m_sendBufferLength;

		public static byte[] KEY = new byte[8]
		{
			174,
			191,
			86,
			120,
			171,
			205,
			239,
			241
		};

		public StreamProcessor(BaseClient client)
		{
			m_client = client;
			m_client.resetKey();
			m_tcpSendBuffer = client.SendBuffer;
			m_tcpQueue = new Queue(256);
			send_event = new SocketAsyncEventArgs();
			send_event.UserToken = this;
			send_event.Completed += AsyncTcpSendCallback;
			send_event.SetBuffer(m_tcpSendBuffer, 0, 0);
			send_fsm = new FSM(2059198199, 1501, "send_fsm");
			receive_fsm = new FSM(2059198199, 1501, "receive_fsm");
		}

		public void SetFsm(int adder, int muliter)
		{
			send_fsm.Setup(adder, muliter);
			receive_fsm.Setup(adder, muliter);
		}

		public void SendTCP(GSPacketIn packet)
		{
			packet.WriteHeader();
			packet.Offset = 0;
			if (m_client.Socket.Connected)
			{
				try
				{
					Statistics.BytesOut += packet.Length;
					Statistics.PacketsOut++;
					lock (m_tcpQueue.SyncRoot)
					{
						m_tcpQueue.Enqueue(packet);
						if (m_sendingTcp)
						{
							return;
						}
						m_sendingTcp = true;
					}
					if (m_client.AsyncPostSend)
					{
						ThreadPool.QueueUserWorkItem(AsyncSendTcpImp, this);
					}
					else
					{
						AsyncTcpSendCallback(this, send_event);
					}
				}
				catch (Exception ex)
				{
					log.Error("SendTCP", ex);
					log.WarnFormat("It seems <{0}> went linkdead. Closing connection. (SendTCP, {1}: {2})", m_client, ex.GetType(), ex.Message);
					m_client.Disconnect();
				}
			}
		}

		private static void AsyncSendTcpImp(object state)
		{
			StreamProcessor streamProcessor = state as StreamProcessor;
			BaseClient client = streamProcessor.m_client;
			try
			{
				AsyncTcpSendCallback(streamProcessor, streamProcessor.send_event);
			}
			catch (Exception exception)
			{
				log.Error("AsyncSendTcpImp", exception);
				client.Disconnect();
			}
		}

		private static void AsyncTcpSendCallback(object sender, SocketAsyncEventArgs e)
		{
			StreamProcessor streamProcessor = (StreamProcessor)e.UserToken;
			BaseClient client = streamProcessor.m_client;
			try
			{
				Queue tcpQueue = streamProcessor.m_tcpQueue;
				if (tcpQueue != null && client.Socket.Connected)
				{
					int bytesTransferred = e.BytesTransferred;
					byte[] tcpSendBuffer = streamProcessor.m_tcpSendBuffer;
					int num = 0;
					if (bytesTransferred != e.Count && streamProcessor.m_sendBufferLength > bytesTransferred)
					{
						num = streamProcessor.m_sendBufferLength - bytesTransferred;
						Array.Copy(tcpSendBuffer, bytesTransferred, tcpSendBuffer, 0, num);
					}
					e.SetBuffer(0, 0);
					int num2 = streamProcessor.m_firstPkgOffset;
					lock (tcpQueue.SyncRoot)
					{
						int num3 = 0;
						if (tcpQueue.Count > 0)
						{
							do
							{
								PacketIn packetIn = (PacketIn)tcpQueue.Peek();
								int num4 = 0;
								num4 = ((!client.Encryted) ? packetIn.CopyTo(tcpSendBuffer, num, num2) : packetIn.CopyTo3(tcpSendBuffer, num, num2, client.SEND_KEY, ref client.numPacketProcces));
								num3 = ((num4 == 0) ? (num3 + 1) : 0);
								num2 += num4;
								num += num4;
								if (packetIn.Length <= num2)
								{
									tcpQueue.Dequeue();
									num2 = 0;
									if (client.Encryted)
									{
										streamProcessor.send_fsm.UpdateState();
										packetIn.isSended = true;
									}
								}
								if (tcpSendBuffer.Length == num)
								{
									break;
								}
								if (num3 > 5)
								{
									packetIn.isSended = true;
									break;
								}
							}
							while (tcpQueue.Count > 0);
						}
						streamProcessor.m_firstPkgOffset = num2;
						if (num <= 0)
						{
							streamProcessor.m_sendingTcp = false;
							return;
						}
					}
					streamProcessor.m_sendBufferLength = num;
					e.SetBuffer(0, num);
					if (!client.SendAsync(e))
					{
						AsyncTcpSendCallback(sender, e);
					}
				}
			}
			catch (Exception ex)
			{
				log.Error("AsyncTcpSendCallback", ex);
				log.WarnFormat("It seems <{0}> went linkdead. Closing connection. (SendTCP, {1}: {2})", client, ex.GetType(), ex.Message);
				client.Disconnect();
			}
		}

		public void ReceiveBytes(int numBytes)
		{
			lock (m_object)
			{
				byte[] packetBuf = m_client.PacketBuf;
				int num = m_client.PacketBufSize + numBytes;
				if (num < 20)
				{
					m_client.PacketBufSize = num;
				}
				else
				{
					m_client.PacketBufSize = 0;
					int i = 0;
					do
					{
						int num2 = 0;
						if (m_client.Encryted)
						{
							_ = receive_fsm.count;
							byte[] param = cloneArrary(m_client.RECEIVE_KEY);
							for (; i + 4 < num; i++)
							{
								byte[] array = decryptBytes(packetBuf, i, 4, param);
								if ((array[0] << 8) + array[1] == 29099)
								{
									num2 = (array[2] << 8) + array[3];
									break;
								}
							}
						}
						else
						{
							for (; i + 4 < num; i++)
							{
								if ((packetBuf[i] << 8) + packetBuf[i + 1] == 29099)
								{
									num2 = (packetBuf[i + 2] << 8) + packetBuf[i + 3];
									break;
								}
							}
						}
						if ((num2 != 0 && num2 < 20) || num2 > 8192)
						{
							m_client.PacketBufSize = 0;
							if (m_client.Strict)
							{
								m_client.Disconnect();
							}
							return;
						}
						int num3 = num - i;
						if (num3 < num2 || num2 == 0)
						{
							Array.Copy(packetBuf, i, packetBuf, 0, num3);
							m_client.PacketBufSize = num3;
							break;
						}
						GSPacketIn gSPacketIn = new GSPacketIn(new byte[8192], 8192);
						if (m_client.Encryted)
						{
							gSPacketIn.CopyFrom3(packetBuf, i, 0, num2, m_client.RECEIVE_KEY);
						}
						else
						{
							gSPacketIn.CopyFrom(packetBuf, i, 0, num2);
						}
						gSPacketIn.ReadHeader();
						try
						{
							m_client.OnRecvPacket(gSPacketIn);
						}
						catch (Exception exception)
						{
							if (log.IsErrorEnabled)
							{
								log.Error("HandlePacket(pak)", exception);
							}
						}
						i += num2;
					}
					while (num - 1 > i);
					if (i >= packetBuf.Length)
					{
						i = packetBuf.Length - 1;
					}
					if (num - 1 == i)
					{
						packetBuf[0] = packetBuf[i];
						m_client.PacketBufSize = 1;
					}
				}
			}
		}

		public static byte[] cloneArrary(byte[] arr, int length = 8)
		{
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = arr[i];
			}
			return array;
		}

		public static string PrintArray(byte[] arr, int length = 8)
		{
			_ = new byte[length];
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[");
			for (int i = 0; i < length; i++)
			{
				stringBuilder.AppendFormat("{0} ", arr[i]);
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		public static string PrintArray(byte[] arr, int first, int length)
		{
			_ = new byte[length];
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[");
			for (int i = first; i < first + length; i++)
			{
				stringBuilder.AppendFormat("{0} ", arr[i]);
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		public static byte[] decryptBytes(byte[] param1, int curOffset, int param2, byte[] param3)
		{
			byte[] array = new byte[param2];
			for (int i = 0; i < param2; i++)
			{
				array[i] = param1[i];
			}
			for (int j = 0; j < param2; j++)
			{
				if (j > 0)
				{
					param3[j % 8] = (byte)((param3[j % 8] + param1[curOffset + j - 1]) ^ j);
					array[j] = (byte)((param1[curOffset + j] - param1[curOffset + j - 1]) ^ param3[j % 8]);
				}
				else
				{
					array[0] = (byte)(param1[curOffset] ^ param3[0]);
				}
			}
			return array;
		}

		public void Dispose()
		{
			send_event.Dispose();
			m_tcpQueue.Clear();
		}
	}
}
