using log4net.Core;
using log4net.Util;
using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace log4net.Appender
{
	/// <summary>
	/// Sends logging events as connectionless UDP datagrams to a remote host or a 
	/// multicast group using an <see cref="T:System.Net.Sockets.UdpClient" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// UDP guarantees neither that messages arrive, nor that they arrive in the correct order.
	/// </para>
	/// <para>
	/// To view the logging results, a custom application can be developed that listens for logging 
	/// events.
	/// </para>
	/// <para>
	/// When decoding events send via this appender remember to use the same encoding
	/// to decode the events as was used to send the events. See the <see cref="P:log4net.Appender.UdpAppender.Encoding" />
	/// property to specify the encoding to use.
	/// </para>
	/// </remarks>
	/// <example>
	/// This example shows how to log receive logging events that are sent 
	/// on IP address 244.0.0.1 and port 8080 to the console. The event is 
	/// encoded in the packet as a unicode string and it is decoded as such. 
	/// <code lang="C#">
	/// IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
	/// UdpClient udpClient;
	/// byte[] buffer;
	/// string loggingEvent;
	///
	/// try 
	/// {
	///     udpClient = new UdpClient(8080);
	///
	///     while(true) 
	///     {
	///         buffer = udpClient.Receive(ref remoteEndPoint);
	///         loggingEvent = System.Text.Encoding.Unicode.GetString(buffer);
	///         Console.WriteLine(loggingEvent);
	///     }
	/// } 
	/// catch(Exception e) 
	/// {
	///     Console.WriteLine(e.ToString());
	/// }
	/// </code>
	/// <code lang="Visual Basic">
	/// Dim remoteEndPoint as IPEndPoint
	/// Dim udpClient as UdpClient
	/// Dim buffer as Byte()
	/// Dim loggingEvent as String
	///
	/// Try 
	///     remoteEndPoint = new IPEndPoint(IPAddress.Any, 0)
	///     udpClient = new UdpClient(8080)
	///
	///     While True
	///         buffer = udpClient.Receive(ByRef remoteEndPoint)
	///         loggingEvent = System.Text.Encoding.Unicode.GetString(buffer)
	///         Console.WriteLine(loggingEvent)
	///     Wend
	/// Catch e As Exception
	///     Console.WriteLine(e.ToString())
	/// End Try
	/// </code>
	/// <para>
	/// An example configuration section to log information using this appender to the 
	/// IP 224.0.0.1 on port 8080:
	/// </para>
	/// <code lang="XML" escaped="true">
	/// <appender name="UdpAppender" type="log4net.Appender.UdpAppender">
	///     <remoteAddress value="224.0.0.1" />
	///     <remotePort value="8080" />
	///     <layout type="log4net.Layout.PatternLayout" value="%-5level %logger [%ndc] - %message%newline" />
	/// </appender>
	/// </code>
	/// </example>
	/// <author>Gert Driesen</author>
	/// <author>Nicko Cadell</author>
	public class UdpAppender : AppenderSkeleton
	{
		/// <summary>
		/// The IP address of the remote host or multicast group to which 
		/// the logging event will be sent.
		/// </summary>
		private IPAddress m_remoteAddress;

		/// <summary>
		/// The TCP port number of the remote host or multicast group to 
		/// which the logging event will be sent.
		/// </summary>
		private int m_remotePort;

		/// <summary>
		/// The cached remote endpoint to which the logging events will be sent.
		/// </summary>
		private IPEndPoint m_remoteEndPoint;

		/// <summary>
		/// The TCP port number from which the <see cref="T:System.Net.Sockets.UdpClient" /> will communicate.
		/// </summary>
		private int m_localPort;

		/// <summary>
		/// The <see cref="T:System.Net.Sockets.UdpClient" /> instance that will be used for sending the 
		/// logging events.
		/// </summary>
		private UdpClient m_client;

		/// <summary>
		/// The encoding to use for the packet.
		/// </summary>
		private Encoding m_encoding = Encoding.Default;

		/// <summary>
		/// Gets or sets the IP address of the remote host or multicast group to which
		/// the underlying <see cref="T:System.Net.Sockets.UdpClient" /> should sent the logging event.
		/// </summary>
		/// <value>
		/// The IP address of the remote host or multicast group to which the logging event 
		/// will be sent.
		/// </value>
		/// <remarks>
		/// <para>
		/// Multicast addresses are identified by IP class <b>D</b> addresses (in the range 224.0.0.0 to
		/// 239.255.255.255).  Multicast packets can pass across different networks through routers, so
		/// it is possible to use multicasts in an Internet scenario as long as your network provider 
		/// supports multicasting.
		/// </para>
		/// <para>
		/// Hosts that want to receive particular multicast messages must register their interest by joining
		/// the multicast group.  Multicast messages are not sent to networks where no host has joined
		/// the multicast group.  Class <b>D</b> IP addresses are used for multicast groups, to differentiate
		/// them from normal host addresses, allowing nodes to easily detect if a message is of interest.
		/// </para>
		/// <para>
		/// Static multicast addresses that are needed globally are assigned by IANA.  A few examples are listed in the table below:
		/// </para>
		/// <para>
		/// <list type="table">
		///     <listheader>
		///         <term>IP Address</term>
		///         <description>Description</description>
		///     </listheader>
		///     <item>
		///         <term>224.0.0.1</term>
		///         <description>
		///             <para>
		///             Sends a message to all system on the subnet.
		///             </para>
		///         </description>
		///     </item>
		///     <item>
		///         <term>224.0.0.2</term>
		///         <description>
		///             <para>
		///             Sends a message to all routers on the subnet.
		///             </para>
		///         </description>
		///     </item>
		///     <item>
		///         <term>224.0.0.12</term>
		///         <description>
		///             <para>
		///             The DHCP server answers messages on the IP address 224.0.0.12, but only on a subnet.
		///             </para>
		///         </description>
		///     </item>
		/// </list>
		/// </para>
		/// <para>
		/// A complete list of actually reserved multicast addresses and their owners in the ranges
		/// defined by RFC 3171 can be found at the <A href="http://www.iana.org/assignments/multicast-addresses">IANA web site</A>. 
		/// </para>
		/// <para>
		/// The address range 239.0.0.0 to 239.255.255.255 is reserved for administrative scope-relative 
		/// addresses.  These addresses can be reused with other local groups.  Routers are typically 
		/// configured with filters to prevent multicast traffic in this range from flowing outside
		/// of the local network.
		/// </para>
		/// </remarks>
		public IPAddress RemoteAddress
		{
			get
			{
				return m_remoteAddress;
			}
			set
			{
				m_remoteAddress = value;
			}
		}

		/// <summary>
		/// Gets or sets the TCP port number of the remote host or multicast group to which 
		/// the underlying <see cref="T:System.Net.Sockets.UdpClient" /> should sent the logging event.
		/// </summary>
		/// <value>
		/// An integer value in the range <see cref="F:System.Net.IPEndPoint.MinPort" /> to <see cref="F:System.Net.IPEndPoint.MaxPort" /> 
		/// indicating the TCP port number of the remote host or multicast group to which the logging event 
		/// will be sent.
		/// </value>
		/// <remarks>
		/// The underlying <see cref="T:System.Net.Sockets.UdpClient" /> will send messages to this TCP port number
		/// on the remote host or multicast group.
		/// </remarks>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified is less than <see cref="F:System.Net.IPEndPoint.MinPort" /> or greater than <see cref="F:System.Net.IPEndPoint.MaxPort" />.</exception>
		public int RemotePort
		{
			get
			{
				return m_remotePort;
			}
			set
			{
				if (value < 0 || value > 65535)
				{
					throw SystemInfo.CreateArgumentOutOfRangeException("value", value, "The value specified is less than " + 0.ToString(NumberFormatInfo.InvariantInfo) + " or greater than " + 65535.ToString(NumberFormatInfo.InvariantInfo) + ".");
				}
				m_remotePort = value;
			}
		}

		/// <summary>
		/// Gets or sets the TCP port number from which the underlying <see cref="T:System.Net.Sockets.UdpClient" /> will communicate.
		/// </summary>
		/// <value>
		/// An integer value in the range <see cref="F:System.Net.IPEndPoint.MinPort" /> to <see cref="F:System.Net.IPEndPoint.MaxPort" /> 
		/// indicating the TCP port number from which the underlying <see cref="T:System.Net.Sockets.UdpClient" /> will communicate.
		/// </value>
		/// <remarks>
		/// <para>
		/// The underlying <see cref="T:System.Net.Sockets.UdpClient" /> will bind to this port for sending messages.
		/// </para>
		/// <para>
		/// Setting the value to 0 (the default) will cause the udp client not to bind to
		/// a local port.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified is less than <see cref="F:System.Net.IPEndPoint.MinPort" /> or greater than <see cref="F:System.Net.IPEndPoint.MaxPort" />.</exception>
		public int LocalPort
		{
			get
			{
				return m_localPort;
			}
			set
			{
				if (value != 0 && (value < 0 || value > 65535))
				{
					throw SystemInfo.CreateArgumentOutOfRangeException("value", value, "The value specified is less than " + 0.ToString(NumberFormatInfo.InvariantInfo) + " or greater than " + 65535.ToString(NumberFormatInfo.InvariantInfo) + ".");
				}
				m_localPort = value;
			}
		}

		/// <summary>
		/// Gets or sets <see cref="P:log4net.Appender.UdpAppender.Encoding" /> used to write the packets.
		/// </summary>
		/// <value>
		/// The <see cref="P:log4net.Appender.UdpAppender.Encoding" /> used to write the packets.
		/// </value>
		/// <remarks>
		/// <para>
		/// The <see cref="P:log4net.Appender.UdpAppender.Encoding" /> used to write the packets.
		/// </para>
		/// </remarks>
		public Encoding Encoding
		{
			get
			{
				return m_encoding;
			}
			set
			{
				m_encoding = value;
			}
		}

		/// <summary>
		/// Gets or sets the underlying <see cref="T:System.Net.Sockets.UdpClient" />.
		/// </summary>
		/// <value>
		/// The underlying <see cref="T:System.Net.Sockets.UdpClient" />.
		/// </value>
		/// <remarks>
		/// <see cref="T:log4net.Appender.UdpAppender" /> creates a <see cref="T:System.Net.Sockets.UdpClient" /> to send logging events 
		/// over a network.  Classes deriving from <see cref="T:log4net.Appender.UdpAppender" /> can use this
		/// property to get or set this <see cref="T:System.Net.Sockets.UdpClient" />.  Use the underlying <see cref="T:System.Net.Sockets.UdpClient" />
		/// returned from <see cref="P:log4net.Appender.UdpAppender.Client" /> if you require access beyond that which 
		/// <see cref="T:log4net.Appender.UdpAppender" /> provides.
		/// </remarks>
		protected UdpClient Client
		{
			get
			{
				return m_client;
			}
			set
			{
				m_client = value;
			}
		}

		/// <summary>
		/// Gets or sets the cached remote endpoint to which the logging events should be sent.
		/// </summary>
		/// <value>
		/// The cached remote endpoint to which the logging events will be sent.
		/// </value>
		/// <remarks>
		/// The <see cref="M:log4net.Appender.UdpAppender.ActivateOptions" /> method will initialize the remote endpoint 
		/// with the values of the <see cref="P:log4net.Appender.UdpAppender.RemoteAddress" /> and <see cref="P:log4net.Appender.UdpAppender.RemotePort" />
		/// properties.
		/// </remarks>
		protected IPEndPoint RemoteEndPoint
		{
			get
			{
				return m_remoteEndPoint;
			}
			set
			{
				m_remoteEndPoint = value;
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
		/// Initialize the appender based on the options set.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Appender.UdpAppender.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Appender.UdpAppender.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Appender.UdpAppender.ActivateOptions" /> must be called again.
		/// </para>
		/// <para>
		/// The appender will be ignored if no <see cref="P:log4net.Appender.UdpAppender.RemoteAddress" /> was specified or 
		/// an invalid remote or local TCP port number was specified.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">The required property <see cref="P:log4net.Appender.UdpAppender.RemoteAddress" /> was not specified.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The TCP port number assigned to <see cref="P:log4net.Appender.UdpAppender.LocalPort" /> or <see cref="P:log4net.Appender.UdpAppender.RemotePort" /> is less than <see cref="F:System.Net.IPEndPoint.MinPort" /> or greater than <see cref="F:System.Net.IPEndPoint.MaxPort" />.</exception>
		public override void ActivateOptions()
		{
			base.ActivateOptions();
			if (RemoteAddress == null)
			{
				throw new ArgumentNullException("The required property 'Address' was not specified.");
			}
			if (RemotePort < 0 || RemotePort > 65535)
			{
				throw SystemInfo.CreateArgumentOutOfRangeException("this.RemotePort", RemotePort, "The RemotePort is less than " + 0.ToString(NumberFormatInfo.InvariantInfo) + " or greater than " + 65535.ToString(NumberFormatInfo.InvariantInfo) + ".");
			}
			if (LocalPort != 0 && (LocalPort < 0 || LocalPort > 65535))
			{
				throw SystemInfo.CreateArgumentOutOfRangeException("this.LocalPort", LocalPort, "The LocalPort is less than " + 0.ToString(NumberFormatInfo.InvariantInfo) + " or greater than " + 65535.ToString(NumberFormatInfo.InvariantInfo) + ".");
			}
			RemoteEndPoint = new IPEndPoint(RemoteAddress, RemotePort);
			InitializeClientConnection();
		}

		/// <summary>
		/// This method is called by the <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)" /> method.
		/// </summary>
		/// <param name="loggingEvent">The event to log.</param>
		/// <remarks>
		/// <para>
		/// Sends the event using an UDP datagram.
		/// </para>
		/// <para>
		/// Exceptions are passed to the <see cref="P:log4net.Appender.AppenderSkeleton.ErrorHandler" />.
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent loggingEvent)
		{
			try
			{
				byte[] bytes = m_encoding.GetBytes(RenderLoggingEvent(loggingEvent).ToCharArray());
				Client.Send(bytes, bytes.Length, RemoteEndPoint);
			}
			catch (Exception e)
			{
				ErrorHandler.Error("Unable to send logging event to remote host " + RemoteAddress.ToString() + " on port " + RemotePort + ".", e, ErrorCode.WriteFailure);
			}
		}

		/// <summary>
		/// Closes the UDP connection and releases all resources associated with 
		/// this <see cref="T:log4net.Appender.UdpAppender" /> instance.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Disables the underlying <see cref="T:System.Net.Sockets.UdpClient" /> and releases all managed 
		/// and unmanaged resources associated with the <see cref="T:log4net.Appender.UdpAppender" />.
		/// </para>
		/// </remarks>
		protected override void OnClose()
		{
			base.OnClose();
			if (Client != null)
			{
				Client.Close();
				Client = null;
			}
		}

		/// <summary>
		/// Initializes the underlying  <see cref="T:System.Net.Sockets.UdpClient" /> connection.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The underlying <see cref="T:System.Net.Sockets.UdpClient" /> is initialized and binds to the 
		/// port number from which you intend to communicate.
		/// </para>
		/// <para>
		/// Exceptions are passed to the <see cref="P:log4net.Appender.AppenderSkeleton.ErrorHandler" />.
		/// </para>
		/// </remarks>
		protected virtual void InitializeClientConnection()
		{
			try
			{
				if (LocalPort == 0)
				{
					Client = new UdpClient();
				}
				else
				{
					Client = new UdpClient(LocalPort);
				}
			}
			catch (Exception e)
			{
				ErrorHandler.Error("Could not initialize the UdpClient connection on port " + LocalPort.ToString(NumberFormatInfo.InvariantInfo) + ".", e, ErrorCode.GenericFailure);
				Client = null;
			}
		}
	}
}
