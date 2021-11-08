using log4net.Core;
using log4net.Util;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace log4net.Appender
{
	/// <summary>
	/// Appender that allows clients to connect via Telnet to receive log messages
	/// </summary>
	/// <remarks>	
	/// <para>
	/// The TelnetAppender accepts socket connections and streams logging messages
	/// back to the client.  
	/// The output is provided in a telnet-friendly way so that a log can be monitored 
	/// over a TCP/IP socket.
	/// This allows simple remote monitoring of application logging.
	/// </para>
	/// <para>
	/// The default <see cref="P:log4net.Appender.TelnetAppender.Port" /> is 23 (the telnet port).
	/// </para>
	/// </remarks>
	/// <author>Keith Long</author>
	/// <author>Nicko Cadell</author>
	public class TelnetAppender : AppenderSkeleton
	{
		/// <summary>
		/// Helper class to manage connected clients
		/// </summary>
		/// <remarks>
		/// <para>
		/// The SocketHandler class is used to accept connections from
		/// clients.  It is threaded so that clients can connect/disconnect
		/// asynchronously.
		/// </para>
		/// </remarks>
		protected class SocketHandler : IDisposable
		{
			/// <summary>
			/// Class that represents a client connected to this handler
			/// </summary>
			/// <remarks>
			/// <para>
			/// Class that represents a client connected to this handler
			/// </para>
			/// </remarks>
			protected class SocketClient : IDisposable
			{
				private Socket m_socket;

				private StreamWriter m_writer;

				/// <summary>
				/// Create this <see cref="T:log4net.Appender.TelnetAppender.SocketHandler.SocketClient" /> for the specified <see cref="T:System.Net.Sockets.Socket" />
				/// </summary>
				/// <param name="socket">the client's socket</param>
				/// <remarks>
				/// <para>
				/// Opens a stream writer on the socket.
				/// </para>
				/// </remarks>
				public SocketClient(Socket socket)
				{
					m_socket = socket;
					try
					{
						m_writer = new StreamWriter(new NetworkStream(socket));
					}
					catch
					{
						Dispose();
						throw;
					}
				}

				/// <summary>
				/// Write a string to the client
				/// </summary>
				/// <param name="message">string to send</param>
				/// <remarks>
				/// <para>
				/// Write a string to the client
				/// </para>
				/// </remarks>
				public void Send(string message)
				{
					m_writer.Write(message);
					m_writer.Flush();
				}

				/// <summary>
				/// Cleanup the clients connection
				/// </summary>
				/// <remarks>
				/// <para>
				/// Close the socket connection.
				/// </para>
				/// </remarks>
				public void Dispose()
				{
					try
					{
						if (m_writer != null)
						{
							m_writer.Close();
							m_writer = null;
						}
					}
					catch
					{
					}
					if (m_socket != null)
					{
						try
						{
							m_socket.Shutdown(SocketShutdown.Both);
						}
						catch
						{
						}
						try
						{
							m_socket.Close();
						}
						catch
						{
						}
						m_socket = null;
					}
				}
			}

			private const int MAX_CONNECTIONS = 20;

			private Socket m_serverSocket;

			private ArrayList m_clients = new ArrayList();

			/// <summary>
			/// Test if this handler has active connections
			/// </summary>
			/// <value>
			/// <c>true</c> if this handler has active connections
			/// </value>
			/// <remarks>
			/// <para>
			/// This property will be <c>true</c> while this handler has
			/// active connections, that is at least one connection that 
			/// the handler will attempt to send a message to.
			/// </para>
			/// </remarks>
			public bool HasConnections
			{
				get
				{
					ArrayList clients = m_clients;
					return clients != null && clients.Count > 0;
				}
			}

			/// <summary>
			/// Opens a new server port on <paramref ref="port" />
			/// </summary>
			/// <param name="port">the local port to listen on for connections</param>
			/// <remarks>
			/// <para>
			/// Creates a socket handler on the specified local server port.
			/// </para>
			/// </remarks>
			public SocketHandler(int port)
			{
				m_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				m_serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
				m_serverSocket.Listen(5);
				m_serverSocket.BeginAccept(OnConnect, null);
			}

			/// <summary>
			/// Sends a string message to each of the connected clients
			/// </summary>
			/// <param name="message">the text to send</param>
			/// <remarks>
			/// <para>
			/// Sends a string message to each of the connected clients
			/// </para>
			/// </remarks>
			public void Send(string message)
			{
				ArrayList clients = m_clients;
				foreach (SocketClient item in clients)
				{
					try
					{
						item.Send(message);
					}
					catch (Exception)
					{
						item.Dispose();
						RemoveClient(item);
					}
				}
			}

			/// <summary>
			/// Add a client to the internal clients list
			/// </summary>
			/// <param name="client">client to add</param>
			private void AddClient(SocketClient client)
			{
				lock (this)
				{
					ArrayList arrayList = (ArrayList)m_clients.Clone();
					arrayList.Add(client);
					m_clients = arrayList;
				}
			}

			/// <summary>
			/// Remove a client from the internal clients list
			/// </summary>
			/// <param name="client">client to remove</param>
			private void RemoveClient(SocketClient client)
			{
				lock (this)
				{
					ArrayList arrayList = (ArrayList)m_clients.Clone();
					arrayList.Remove(client);
					m_clients = arrayList;
				}
			}

			/// <summary>
			/// Callback used to accept a connection on the server socket
			/// </summary>
			/// <param name="asyncResult">The result of the asynchronous operation</param>
			/// <remarks>
			/// <para>
			/// On connection adds to the list of connections 
			/// if there are two many open connections you will be disconnected
			/// </para>
			/// </remarks>
			private void OnConnect(IAsyncResult asyncResult)
			{
				try
				{
					Socket socket = m_serverSocket.EndAccept(asyncResult);
					LogLog.Debug("TelnetAppender: Accepting connection from [" + socket.RemoteEndPoint.ToString() + "]");
					SocketClient socketClient = new SocketClient(socket);
					int count = m_clients.Count;
					if (count < 20)
					{
						try
						{
							socketClient.Send("TelnetAppender v1.0 (" + (count + 1) + " active connections)\r\n\r\n");
							AddClient(socketClient);
						}
						catch
						{
							socketClient.Dispose();
						}
					}
					else
					{
						socketClient.Send("Sorry - Too many connections.\r\n");
						socketClient.Dispose();
					}
				}
				catch
				{
				}
				finally
				{
					if (m_serverSocket != null)
					{
						m_serverSocket.BeginAccept(OnConnect, null);
					}
				}
			}

			/// <summary>
			/// Close all network connections
			/// </summary>
			/// <remarks>
			/// <para>
			/// Make sure we close all network connections
			/// </para>
			/// </remarks>
			public void Dispose()
			{
				ArrayList clients = m_clients;
				foreach (SocketClient item in clients)
				{
					item.Dispose();
				}
				m_clients.Clear();
				Socket serverSocket = m_serverSocket;
				m_serverSocket = null;
				try
				{
					serverSocket.Shutdown(SocketShutdown.Both);
				}
				catch
				{
				}
				try
				{
					serverSocket.Close();
				}
				catch
				{
				}
			}
		}

		private SocketHandler m_handler;

		private int m_listeningPort = 23;

		/// <summary>
		/// Gets or sets the TCP port number on which this <see cref="T:log4net.Appender.TelnetAppender" /> will listen for connections.
		/// </summary>
		/// <value>
		/// An integer value in the range <see cref="F:System.Net.IPEndPoint.MinPort" /> to <see cref="F:System.Net.IPEndPoint.MaxPort" /> 
		/// indicating the TCP port number on which this <see cref="T:log4net.Appender.TelnetAppender" /> will listen for connections.
		/// </value>
		/// <remarks>
		/// <para>
		/// The default value is 23 (the telnet port).
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified is less than <see cref="F:System.Net.IPEndPoint.MinPort" /> 
		/// or greater than <see cref="F:System.Net.IPEndPoint.MaxPort" />.</exception>
		public int Port
		{
			get
			{
				return m_listeningPort;
			}
			set
			{
				if (value < 0 || value > 65535)
				{
					throw SystemInfo.CreateArgumentOutOfRangeException("value", value, "The value specified for Port is less than " + 0.ToString(NumberFormatInfo.InvariantInfo) + " or greater than " + 65535.ToString(NumberFormatInfo.InvariantInfo) + ".");
				}
				m_listeningPort = value;
			}
		}

		/// <summary>
		/// This appender requires a <see cref="N:log4net.Layout" /> to be set.
		/// </summary>
		/// <value><c>true</c></value>
		/// <remarks>
		/// <para>
		/// This appender requires a <see cref="N:log4net.Layout" /> to be set.
		/// </para>
		/// </remarks>
		protected override bool RequiresLayout => true;

		/// <summary>
		/// Overrides the parent method to close the socket handler
		/// </summary>
		/// <remarks>
		/// <para>
		/// Closes all the outstanding connections.
		/// </para>
		/// </remarks>
		protected override void OnClose()
		{
			base.OnClose();
			if (m_handler != null)
			{
				m_handler.Dispose();
				m_handler = null;
			}
		}

		/// <summary>
		/// Initialize the appender based on the options set.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Appender.TelnetAppender.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Appender.TelnetAppender.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Appender.TelnetAppender.ActivateOptions" /> must be called again.
		/// </para>
		/// <para>
		/// Create the socket handler and wait for connections
		/// </para>
		/// </remarks>
		public override void ActivateOptions()
		{
			base.ActivateOptions();
			try
			{
				LogLog.Debug("TelnetAppender: Creating SocketHandler to listen on port [" + m_listeningPort + "]");
				m_handler = new SocketHandler(m_listeningPort);
			}
			catch (Exception exception)
			{
				LogLog.Error("TelnetAppender: Failed to create SocketHandler", exception);
				throw;
			}
		}

		/// <summary>
		/// Writes the logging event to each connected client.
		/// </summary>
		/// <param name="loggingEvent">The event to log.</param>
		/// <remarks>
		/// <para>
		/// Writes the logging event to each connected client.
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent loggingEvent)
		{
			if (m_handler != null && m_handler.HasConnections)
			{
				m_handler.Send(RenderLoggingEvent(loggingEvent));
			}
		}
	}
}
