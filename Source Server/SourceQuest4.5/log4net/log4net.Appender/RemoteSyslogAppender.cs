using log4net.Core;
using log4net.Layout;
using log4net.Util;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace log4net.Appender
{
	/// <summary>
	/// Logs events to a remote syslog daemon.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The BSD syslog protocol is used to remotely log to
	/// a syslog daemon. The syslogd listens for for messages
	/// on UDP port 514.
	/// </para>
	/// <para>
	/// The syslog UDP protocol is not authenticated. Most syslog daemons
	/// do not accept remote log messages because of the security implications.
	/// You may be able to use the LocalSyslogAppender to talk to a local
	/// syslog service.
	/// </para>
	/// <para>
	/// There is an RFC 3164 that claims to document the BSD Syslog Protocol.
	/// This RFC can be seen here: http://www.faqs.org/rfcs/rfc3164.html.
	/// This appender generates what the RFC calls an "Original Device Message",
	/// i.e. does not include the TIMESTAMP or HOSTNAME fields. By observation
	/// this format of message will be accepted by all current syslog daemon
	/// implementations. The daemon will attach the current time and the source
	/// hostname or IP address to any messages received.
	/// </para>
	/// <para>
	/// Syslog messages must have a facility and and a severity. The severity
	/// is derived from the Level of the logging event.
	/// The facility must be chosen from the set of defined syslog 
	/// <see cref="T:log4net.Appender.RemoteSyslogAppender.SyslogFacility" /> values. The facilities list is predefined
	/// and cannot be extended.
	/// </para>
	/// <para>
	/// An identifier is specified with each log message. This can be specified
	/// by setting the <see cref="P:log4net.Appender.RemoteSyslogAppender.Identity" /> property. The identity (also know 
	/// as the tag) must not contain white space. The default value for the
	/// identity is the application name (from <see cref="P:log4net.Core.LoggingEvent.Domain" />).
	/// </para>
	/// </remarks>
	/// <author>Rob Lyon</author>
	/// <author>Nicko Cadell</author>
	public class RemoteSyslogAppender : UdpAppender
	{
		/// <summary>
		/// syslog severities
		/// </summary>
		/// <remarks>
		/// <para>
		/// The syslog severities.
		/// </para>
		/// </remarks>
		public enum SyslogSeverity
		{
			/// <summary>
			/// system is unusable
			/// </summary>
			Emergency,
			/// <summary>
			/// action must be taken immediately
			/// </summary>
			Alert,
			/// <summary>
			/// critical conditions
			/// </summary>
			Critical,
			/// <summary>
			/// error conditions
			/// </summary>
			Error,
			/// <summary>
			/// warning conditions
			/// </summary>
			Warning,
			/// <summary>
			/// normal but significant condition
			/// </summary>
			Notice,
			/// <summary>
			/// informational
			/// </summary>
			Informational,
			/// <summary>
			/// debug-level messages
			/// </summary>
			Debug
		}

		/// <summary>
		/// syslog facilities
		/// </summary>
		/// <remarks>
		/// <para>
		/// The syslog facilities
		/// </para>
		/// </remarks>
		public enum SyslogFacility
		{
			/// <summary>
			/// kernel messages
			/// </summary>
			Kernel,
			/// <summary>
			/// random user-level messages
			/// </summary>
			User,
			/// <summary>
			/// mail system
			/// </summary>
			Mail,
			/// <summary>
			/// system daemons
			/// </summary>
			Daemons,
			/// <summary>
			/// security/authorization messages
			/// </summary>
			Authorization,
			/// <summary>
			/// messages generated internally by syslogd
			/// </summary>
			Syslog,
			/// <summary>
			/// line printer subsystem
			/// </summary>
			Printer,
			/// <summary>
			/// network news subsystem
			/// </summary>
			News,
			/// <summary>
			/// UUCP subsystem
			/// </summary>
			Uucp,
			/// <summary>
			/// clock (cron/at) daemon
			/// </summary>
			Clock,
			/// <summary>
			/// security/authorization  messages (private)
			/// </summary>
			Authorization2,
			/// <summary>
			/// ftp daemon
			/// </summary>
			Ftp,
			/// <summary>
			/// NTP subsystem
			/// </summary>
			Ntp,
			/// <summary>
			/// log audit
			/// </summary>
			Audit,
			/// <summary>
			/// log alert
			/// </summary>
			Alert,
			/// <summary>
			/// clock daemon
			/// </summary>
			Clock2,
			/// <summary>
			/// reserved for local use
			/// </summary>
			Local0,
			/// <summary>
			/// reserved for local use
			/// </summary>
			Local1,
			/// <summary>
			/// reserved for local use
			/// </summary>
			Local2,
			/// <summary>
			/// reserved for local use
			/// </summary>
			Local3,
			/// <summary>
			/// reserved for local use
			/// </summary>
			Local4,
			/// <summary>
			/// reserved for local use
			/// </summary>
			Local5,
			/// <summary>
			/// reserved for local use
			/// </summary>
			Local6,
			/// <summary>
			/// reserved for local use
			/// </summary>
			Local7
		}

		/// <summary>
		/// A class to act as a mapping between the level that a logging call is made at and
		/// the syslog severity that is should be logged at.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A class to act as a mapping between the level that a logging call is made at and
		/// the syslog severity that is should be logged at.
		/// </para>
		/// </remarks>
		public class LevelSeverity : LevelMappingEntry
		{
			private SyslogSeverity m_severity;

			/// <summary>
			/// The mapped syslog severity for the specified level
			/// </summary>
			/// <remarks>
			/// <para>
			/// Required property.
			/// The mapped syslog severity for the specified level
			/// </para>
			/// </remarks>
			public SyslogSeverity Severity
			{
				get
				{
					return m_severity;
				}
				set
				{
					m_severity = value;
				}
			}
		}

		/// <summary>
		/// Syslog port 514
		/// </summary>
		private const int DefaultSyslogPort = 514;

		/// <summary>
		/// The facility. The default facility is <see cref="F:log4net.Appender.RemoteSyslogAppender.SyslogFacility.User" />.
		/// </summary>
		private SyslogFacility m_facility = SyslogFacility.User;

		/// <summary>
		/// The message identity
		/// </summary>
		private PatternLayout m_identity;

		/// <summary>
		/// Mapping from level object to syslog severity
		/// </summary>
		private LevelMapping m_levelMapping = new LevelMapping();

		/// <summary>
		/// Message identity
		/// </summary>
		/// <remarks>
		/// <para>
		/// An identifier is specified with each log message. This can be specified
		/// by setting the <see cref="P:log4net.Appender.RemoteSyslogAppender.Identity" /> property. The identity (also know 
		/// as the tag) must not contain white space. The default value for the
		/// identity is the application name (from <see cref="P:log4net.Core.LoggingEvent.Domain" />).
		/// </para>
		/// </remarks>
		public PatternLayout Identity
		{
			get
			{
				return m_identity;
			}
			set
			{
				m_identity = value;
			}
		}

		/// <summary>
		/// Syslog facility
		/// </summary>
		/// <remarks>
		/// Set to one of the <see cref="T:log4net.Appender.RemoteSyslogAppender.SyslogFacility" /> values. The list of
		/// facilities is predefined and cannot be extended. The default value
		/// is <see cref="F:log4net.Appender.RemoteSyslogAppender.SyslogFacility.User" />.
		/// </remarks>
		public SyslogFacility Facility
		{
			get
			{
				return m_facility;
			}
			set
			{
				m_facility = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Appender.RemoteSyslogAppender" /> class.
		/// </summary>
		/// <remarks>
		/// This instance of the <see cref="T:log4net.Appender.RemoteSyslogAppender" /> class is set up to write 
		/// to a remote syslog daemon.
		/// </remarks>
		public RemoteSyslogAppender()
		{
			base.RemotePort = 514;
			base.RemoteAddress = IPAddress.Parse("127.0.0.1");
			base.Encoding = Encoding.ASCII;
		}

		/// <summary>
		/// Add a mapping of level to severity
		/// </summary>
		/// <param name="mapping">The mapping to add</param>
		/// <remarks>
		/// <para>
		/// Add a <see cref="T:log4net.Appender.RemoteSyslogAppender.LevelSeverity" /> mapping to this appender.
		/// </para>
		/// </remarks>
		public void AddMapping(LevelSeverity mapping)
		{
			m_levelMapping.Add(mapping);
		}

		/// <summary>
		/// This method is called by the <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)" /> method.
		/// </summary>
		/// <param name="loggingEvent">The event to log.</param>
		/// <remarks>
		/// <para>
		/// Writes the event to a remote syslog daemon.
		/// </para>
		/// <para>
		/// The format of the output will depend on the appender's layout.
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent loggingEvent)
		{
			try
			{
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				int value = GeneratePriority(m_facility, GetSeverity(loggingEvent.Level));
				stringWriter.Write('<');
				stringWriter.Write(value);
				stringWriter.Write('>');
				if (m_identity != null)
				{
					m_identity.Format(stringWriter, loggingEvent);
				}
				else
				{
					stringWriter.Write(loggingEvent.Domain);
				}
				stringWriter.Write(": ");
				RenderLoggingEvent(stringWriter, loggingEvent);
				string text = stringWriter.ToString();
				byte[] bytes = base.Encoding.GetBytes(text.ToCharArray());
				base.Client.Send(bytes, bytes.Length, base.RemoteEndPoint);
			}
			catch (Exception e)
			{
				ErrorHandler.Error("Unable to send logging event to remote syslog " + base.RemoteAddress.ToString() + " on port " + base.RemotePort + ".", e, ErrorCode.WriteFailure);
			}
		}

		/// <summary>
		/// Initialize the options for this appender
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initialize the level to syslog severity mappings set on this appender.
		/// </para>
		/// </remarks>
		public override void ActivateOptions()
		{
			base.ActivateOptions();
			m_levelMapping.ActivateOptions();
		}

		/// <summary>
		/// Translates a log4net level to a syslog severity.
		/// </summary>
		/// <param name="level">A log4net level.</param>
		/// <returns>A syslog severity.</returns>
		/// <remarks>
		/// <para>
		/// Translates a log4net level to a syslog severity.
		/// </para>
		/// </remarks>
		protected virtual SyslogSeverity GetSeverity(Level level)
		{
			LevelSeverity levelSeverity = m_levelMapping.Lookup(level) as LevelSeverity;
			if (levelSeverity != null)
			{
				return levelSeverity.Severity;
			}
			if (level >= Level.Alert)
			{
				return SyslogSeverity.Alert;
			}
			if (level >= Level.Critical)
			{
				return SyslogSeverity.Critical;
			}
			if (level >= Level.Error)
			{
				return SyslogSeverity.Error;
			}
			if (level >= Level.Warn)
			{
				return SyslogSeverity.Warning;
			}
			if (level >= Level.Notice)
			{
				return SyslogSeverity.Notice;
			}
			if (level >= Level.Info)
			{
				return SyslogSeverity.Informational;
			}
			return SyslogSeverity.Debug;
		}

		/// <summary>
		/// Generate a syslog priority.
		/// </summary>
		/// <param name="facility">The syslog facility.</param>
		/// <param name="severity">The syslog severity.</param>
		/// <returns>A syslog priority.</returns>
		/// <remarks>
		/// <para>
		/// Generate a syslog priority.
		/// </para>
		/// </remarks>
		public static int GeneratePriority(SyslogFacility facility, SyslogSeverity severity)
		{
			if (facility < SyslogFacility.Kernel || facility > SyslogFacility.Local7)
			{
				throw new ArgumentException("SyslogFacility out of range", "facility");
			}
			if (severity < SyslogSeverity.Emergency || severity > SyslogSeverity.Debug)
			{
				throw new ArgumentException("SyslogSeverity out of range", "severity");
			}
			return (int)((int)facility * 8 + severity);
		}
	}
}
