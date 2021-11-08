using log4net.Core;
using log4net.Util;
using System;
using System.Runtime.InteropServices;

namespace log4net.Appender
{
	/// <summary>
	/// Logs entries by sending network messages using the 
	/// <see cref="M:log4net.Appender.NetSendAppender.NetMessageBufferSend(System.String,System.String,System.String,System.String,System.Int32)" /> native function.
	/// </summary>
	/// <remarks>
	/// <para>
	/// You can send messages only to names that are active 
	/// on the network. If you send the message to a user name, 
	/// that user must be logged on and running the Messenger 
	/// service to receive the message.
	/// </para>
	/// <para>
	/// The receiver will get a top most window displaying the 
	/// messages one at a time, therefore this appender should 
	/// not be used to deliver a high volume of messages.
	/// </para>
	/// <para>
	/// The following table lists some possible uses for this appender :
	/// </para>
	/// <para>
	/// <list type="table">
	///     <listheader>
	///         <term>Action</term>
	///         <description>Property Value(s)</description>
	///     </listheader>
	///     <item>
	///         <term>Send a message to a user account on the local machine</term>
	///         <description>
	///             <para>
	///             <paramref name="Server" /> = &lt;name of the local machine&gt;
	///             </para>
	///             <para>
	///             <paramref name="Recipient" /> = &lt;user name&gt;
	///             </para>
	///         </description>
	///     </item>
	///     <item>
	///         <term>Send a message to a user account on a remote machine</term>
	///         <description>
	///             <para>
	///             <paramref name="Server" /> = &lt;name of the remote machine&gt;
	///             </para>
	///             <para>
	///             <paramref name="Recipient" /> = &lt;user name&gt;
	///             </para>
	///         </description>
	///     </item>
	///     <item>
	///         <term>Send a message to a domain user account</term>
	///         <description>
	///             <para>
	///             <paramref name="Server" /> = &lt;name of a domain controller | uninitialized&gt;
	///             </para>
	///             <para>
	///             <paramref name="Recipient" /> = &lt;user name&gt;
	///             </para>
	///         </description>
	///     </item>
	///     <item>
	///         <term>Send a message to all the names in a workgroup or domain</term>
	///         <description>
	///             <para>
	///             <paramref name="Recipient" /> = &lt;workgroup name | domain name&gt;*
	///             </para>
	///         </description>
	///     </item>
	///     <item>
	///         <term>Send a message from the local machine to a remote machine</term>
	///         <description>
	///             <para>
	///             <paramref name="Server" /> = &lt;name of the local machine | uninitialized&gt;
	///             </para>
	///             <para>
	///             <paramref name="Recipient" /> = &lt;name of the remote machine&gt;
	///             </para>
	///         </description>
	///     </item>
	/// </list>
	/// </para>
	/// <para>
	/// <b>Note :</b> security restrictions apply for sending 
	/// network messages, see <see cref="M:log4net.Appender.NetSendAppender.NetMessageBufferSend(System.String,System.String,System.String,System.String,System.Int32)" /> 
	/// for more information.
	/// </para>
	/// </remarks>
	/// <example>
	/// <para>
	/// An example configuration section to log information 
	/// using this appender from the local machine, named 
	/// LOCAL_PC, to machine OPERATOR_PC :
	/// </para>
	/// <code lang="XML" escaped="true">
	/// <appender name="NetSendAppender_Operator" type="log4net.Appender.NetSendAppender">
	///     <server value="LOCAL_PC" />
	///     <recipient value="OPERATOR_PC" />
	///     <layout type="log4net.Layout.PatternLayout" value="%-5p %c [%x] - %m%n" />
	/// </appender>
	/// </code>
	/// </example>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class NetSendAppender : AppenderSkeleton
	{
		/// <summary>
		/// The DNS or NetBIOS name of the server on which the function is to execute.
		/// </summary>
		private string m_server;

		/// <summary>
		/// The sender of the network message.
		/// </summary>
		private string m_sender;

		/// <summary>
		/// The message alias to which the message should be sent.
		/// </summary>
		private string m_recipient;

		/// <summary>
		/// The security context to use for privileged calls
		/// </summary>
		private SecurityContext m_securityContext;

		/// <summary>
		/// Gets or sets the sender of the message.
		/// </summary>
		/// <value>
		/// The sender of the message.
		/// </value>
		/// <remarks>
		/// If this property is not specified, the message is sent from the local computer.
		/// </remarks>
		public string Sender
		{
			get
			{
				return m_sender;
			}
			set
			{
				m_sender = value;
			}
		}

		/// <summary>
		/// Gets or sets the message alias to which the message should be sent.
		/// </summary>
		/// <value>
		/// The recipient of the message.
		/// </value>
		/// <remarks>
		/// This property should always be specified in order to send a message.
		/// </remarks>
		public string Recipient
		{
			get
			{
				return m_recipient;
			}
			set
			{
				m_recipient = value;
			}
		}

		/// <summary>
		/// Gets or sets the DNS or NetBIOS name of the remote server on which the function is to execute.
		/// </summary>
		/// <value>
		/// DNS or NetBIOS name of the remote server on which the function is to execute.
		/// </value>
		/// <remarks>
		/// <para>
		/// For Windows NT 4.0 and earlier, the string should begin with \\.
		/// </para>
		/// <para>
		/// If this property is not specified, the local computer is used. 
		/// </para>
		/// </remarks>
		public string Server
		{
			get
			{
				return m_server;
			}
			set
			{
				m_server = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="P:log4net.Appender.NetSendAppender.SecurityContext" /> used to call the NetSend method.
		/// </summary>
		/// <value>
		/// The <see cref="P:log4net.Appender.NetSendAppender.SecurityContext" /> used to call the NetSend method.
		/// </value>
		/// <remarks>
		/// <para>
		/// Unless a <see cref="P:log4net.Appender.NetSendAppender.SecurityContext" /> specified here for this appender
		/// the <see cref="P:log4net.Core.SecurityContextProvider.DefaultProvider" /> is queried for the
		/// security context to use. The default behavior is to use the security context
		/// of the current thread.
		/// </para>
		/// </remarks>
		public SecurityContext SecurityContext
		{
			get
			{
				return m_securityContext;
			}
			set
			{
				m_securityContext = value;
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
		/// activation scheme. The <see cref="M:log4net.Appender.NetSendAppender.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Appender.NetSendAppender.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Appender.NetSendAppender.ActivateOptions" /> must be called again.
		/// </para>
		/// <para>
		/// The appender will be ignored if no <see cref="P:log4net.Appender.NetSendAppender.Recipient" /> was specified.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">The required property <see cref="P:log4net.Appender.NetSendAppender.Recipient" /> was not specified.</exception>
		public override void ActivateOptions()
		{
			base.ActivateOptions();
			if (Recipient == null)
			{
				throw new ArgumentNullException("Recipient", "The required property 'Recipient' was not specified.");
			}
			if (m_securityContext == null)
			{
				m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
			}
		}

		/// <summary>
		/// This method is called by the <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)" /> method.
		/// </summary>
		/// <param name="loggingEvent">The event to log.</param>
		/// <remarks>
		/// <para>
		/// Sends the event using a network message.
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent loggingEvent)
		{
			NativeError nativeError = null;
			string text = RenderLoggingEvent(loggingEvent);
			using (m_securityContext.Impersonate(this))
			{
				int num = NetMessageBufferSend(Server, Recipient, Sender, text, text.Length * Marshal.SystemDefaultCharSize);
				if (num != 0)
				{
					nativeError = NativeError.GetError(num);
				}
			}
			if (nativeError != null)
			{
				ErrorHandler.Error(nativeError.ToString() + " (Params: Server=" + Server + ", Recipient=" + Recipient + ", Sender=" + Sender + ")");
			}
		}

		/// <summary>
		/// Sends a buffer of information to a registered message alias.
		/// </summary>
		/// <param name="serverName">The DNS or NetBIOS name of the server on which the function is to execute.</param>
		/// <param name="msgName">The message alias to which the message buffer should be sent</param>
		/// <param name="fromName">The originator of the message.</param>
		/// <param name="buffer">The message text.</param>
		/// <param name="bufferSize">The length, in bytes, of the message text.</param>
		/// <remarks>
		/// <para>
		/// The following restrictions apply for sending network messages:
		/// </para>
		/// <para>
		/// <list type="table">
		///     <listheader>
		///         <term>Platform</term>
		///         <description>Requirements</description>
		///     </listheader>
		///     <item>
		///         <term>Windows NT</term>
		///         <description>
		///             <para>
		///             No special group membership is required to send a network message.
		///             </para>
		///             <para>
		///             Admin, Accounts, Print, or Server Operator group membership is required to 
		///             successfully send a network message on a remote server.
		///             </para>
		///         </description>
		///     </item>
		///     <item>
		///         <term>Windows 2000 or later</term>
		///         <description>
		///             <para>
		///             If you send a message on a domain controller that is running Active Directory, 
		///             access is allowed or denied based on the access control list (ACL) for the securable 
		///             object. The default ACL permits only Domain Admins and Account Operators to send a network message. 
		///             </para>
		///             <para>
		///             On a member server or workstation, only Administrators and Server Operators can send a network message. 
		///             </para>
		///         </description>
		///     </item>
		/// </list>
		/// </para>
		/// <para>
		/// For more information see <a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/netmgmt/netmgmt/security_requirements_for_the_network_management_functions.asp">Security Requirements for the Network Management Functions</a>.
		/// </para>
		/// </remarks>
		/// <returns>
		/// <para>
		/// If the function succeeds, the return value is zero.
		/// </para>
		/// </returns>
		[DllImport("netapi32.dll", SetLastError = true)]
		protected static extern int NetMessageBufferSend([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string msgName, [MarshalAs(UnmanagedType.LPWStr)] string fromName, [MarshalAs(UnmanagedType.LPWStr)] string buffer, int bufferSize);
	}
}
