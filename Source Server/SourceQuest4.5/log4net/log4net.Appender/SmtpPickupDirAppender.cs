using log4net.Core;
using log4net.Util;
using System;
using System.IO;

namespace log4net.Appender
{
	/// <summary>
	/// Send an email when a specific logging event occurs, typically on errors 
	/// or fatal errors. Rather than sending via smtp it writes a file into the
	/// directory specified by <see cref="P:log4net.Appender.SmtpPickupDirAppender.PickupDir" />. This allows services such
	/// as the IIS SMTP agent to manage sending the messages.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The configuration for this appender is identical to that of the <c>SMTPAppender</c>,
	/// except that instead of specifying the <c>SMTPAppender.SMTPHost</c> you specify
	/// <see cref="P:log4net.Appender.SmtpPickupDirAppender.PickupDir" />.
	/// </para>
	/// <para>
	/// The number of logging events delivered in this e-mail depend on
	/// the value of <see cref="P:log4net.Appender.BufferingAppenderSkeleton.BufferSize" /> option. The
	/// <see cref="T:log4net.Appender.SmtpPickupDirAppender" /> keeps only the last
	/// <see cref="P:log4net.Appender.BufferingAppenderSkeleton.BufferSize" /> logging events in its 
	/// cyclic buffer. This keeps memory requirements at a reasonable level while 
	/// still delivering useful application context.
	/// </para>
	/// </remarks>
	/// <author>Niall Daley</author>
	/// <author>Nicko Cadell</author>
	public class SmtpPickupDirAppender : BufferingAppenderSkeleton
	{
		private string m_to;

		private string m_from;

		private string m_subject;

		private string m_pickupDir;

		/// <summary>
		/// The security context to use for privileged calls
		/// </summary>
		private SecurityContext m_securityContext;

		/// <summary>
		/// Gets or sets a semicolon-delimited list of recipient e-mail addresses.
		/// </summary>
		/// <value>
		/// A semicolon-delimited list of e-mail addresses.
		/// </value>
		/// <remarks>
		/// <para>
		/// A semicolon-delimited list of e-mail addresses.
		/// </para>
		/// </remarks>
		public string To
		{
			get
			{
				return m_to;
			}
			set
			{
				m_to = value;
			}
		}

		/// <summary>
		/// Gets or sets the e-mail address of the sender.
		/// </summary>
		/// <value>
		/// The e-mail address of the sender.
		/// </value>
		/// <remarks>
		/// <para>
		/// The e-mail address of the sender.
		/// </para>
		/// </remarks>
		public string From
		{
			get
			{
				return m_from;
			}
			set
			{
				m_from = value;
			}
		}

		/// <summary>
		/// Gets or sets the subject line of the e-mail message.
		/// </summary>
		/// <value>
		/// The subject line of the e-mail message.
		/// </value>
		/// <remarks>
		/// <para>
		/// The subject line of the e-mail message.
		/// </para>
		/// </remarks>
		public string Subject
		{
			get
			{
				return m_subject;
			}
			set
			{
				m_subject = value;
			}
		}

		/// <summary>
		/// Gets or sets the path to write the messages to.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Gets or sets the path to write the messages to. This should be the same
		/// as that used by the agent sending the messages.
		/// </para>
		/// </remarks>
		public string PickupDir
		{
			get
			{
				return m_pickupDir;
			}
			set
			{
				m_pickupDir = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="P:log4net.Appender.SmtpPickupDirAppender.SecurityContext" /> used to write to the pickup directory.
		/// </summary>
		/// <value>
		/// The <see cref="P:log4net.Appender.SmtpPickupDirAppender.SecurityContext" /> used to write to the pickup directory.
		/// </value>
		/// <remarks>
		/// <para>
		/// Unless a <see cref="P:log4net.Appender.SmtpPickupDirAppender.SecurityContext" /> specified here for this appender
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
		/// Sends the contents of the cyclic buffer as an e-mail message.
		/// </summary>
		/// <param name="events">The logging events to send.</param>
		/// <remarks>
		/// <para>
		/// Sends the contents of the cyclic buffer as an e-mail message.
		/// </para>
		/// </remarks>
		protected override void SendBuffer(LoggingEvent[] events)
		{
			try
			{
				string text = null;
				StreamWriter streamWriter = null;
				using (SecurityContext.Impersonate(this))
				{
					text = Path.Combine(m_pickupDir, SystemInfo.NewGuid().ToString("N"));
					streamWriter = File.CreateText(text);
				}
				if (streamWriter == null)
				{
					ErrorHandler.Error("Failed to create output file for writing [" + text + "]", null, ErrorCode.FileOpenFailure);
				}
				else
				{
					using (streamWriter)
					{
						streamWriter.WriteLine("To: " + m_to);
						streamWriter.WriteLine("From: " + m_from);
						streamWriter.WriteLine("Subject: " + m_subject);
						streamWriter.WriteLine("");
						string header = Layout.Header;
						if (header != null)
						{
							streamWriter.Write(header);
						}
						for (int i = 0; i < events.Length; i++)
						{
							RenderLoggingEvent(streamWriter, events[i]);
						}
						header = Layout.Footer;
						if (header != null)
						{
							streamWriter.Write(header);
						}
						streamWriter.WriteLine("");
						streamWriter.WriteLine(".");
					}
				}
			}
			catch (Exception e)
			{
				ErrorHandler.Error("Error occurred while sending e-mail notification.", e);
			}
		}

		/// <summary>
		/// Activate the options on this appender. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Appender.SmtpPickupDirAppender.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Appender.SmtpPickupDirAppender.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Appender.SmtpPickupDirAppender.ActivateOptions" /> must be called again.
		/// </para>
		/// </remarks>
		public override void ActivateOptions()
		{
			base.ActivateOptions();
			if (m_securityContext == null)
			{
				m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
			}
			using (SecurityContext.Impersonate(this))
			{
				m_pickupDir = ConvertToFullPath(m_pickupDir.Trim());
			}
		}

		/// <summary>
		/// Convert a path into a fully qualified path.
		/// </summary>
		/// <param name="path">The path to convert.</param>
		/// <returns>The fully qualified path.</returns>
		/// <remarks>
		/// <para>
		/// Converts the path specified to a fully
		/// qualified path. If the path is relative it is
		/// taken as relative from the application base 
		/// directory.
		/// </para>
		/// </remarks>
		protected static string ConvertToFullPath(string path)
		{
			return SystemInfo.ConvertToFullPath(path);
		}
	}
}
