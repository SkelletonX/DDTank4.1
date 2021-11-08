using log4net.Core;
using log4net.Util;
using System;
using System.Runtime.InteropServices;

namespace log4net.Appender
{
	/// <summary>
	/// Logs events to a local syslog service.
	/// </summary>
	/// <remarks>
	/// <note>
	/// This appender uses the POSIX libc library functions <c>openlog</c>, <c>syslog</c>, and <c>closelog</c>.
	/// If these functions are not available on the local system then this appender will not work!
	/// </note>
	/// <para>
	/// The functions <c>openlog</c>, <c>syslog</c>, and <c>closelog</c> are specified in SUSv2 and 
	/// POSIX 1003.1-2001 standards. These are used to log messages to the local syslog service.
	/// </para>
	/// <para>
	/// This appender talks to a local syslog service. If you need to log to a remote syslog
	/// daemon and you cannot configure your local syslog service to do this you may be
	/// able to use the <see cref="T:log4net.Appender.RemoteSyslogAppender" /> to log via UDP.
	/// </para>
	/// <para>
	/// Syslog messages must have a facility and and a severity. The severity
	/// is derived from the Level of the logging event.
	/// The facility must be chosen from the set of defined syslog 
	/// <see cref="T:log4net.Appender.LocalSyslogAppender.SyslogFacility" /> values. The facilities list is predefined
	/// and cannot be extended.
	/// </para>
	/// <para>
	/// An identifier is specified with each log message. This can be specified
	/// by setting the <see cref="P:log4net.Appender.LocalSyslogAppender.Identity" /> property. The identity (also know 
	/// as the tag) must not contain white space. The default value for the
	/// identity is the application name (from <see cref="P:log4net.Util.SystemInfo.ApplicationFriendlyName" />).
	/// </para>
	/// </remarks>
	/// <author>Rob Lyon</author>
	/// <author>Nicko Cadell</author>
	public class LocalSyslogAppender : AppenderSkeleton
	{
		/// <summary>
		/// syslog severities
		/// </summary>
		/// <remarks>
		/// <para>
		/// The log4net Level maps to a syslog severity using the
		/// <see cref="M:log4net.Appender.LocalSyslogAppender.AddMapping(log4net.Appender.LocalSyslogAppender.LevelSeverity)" /> method and the <see cref="T:log4net.Appender.LocalSyslogAppender.LevelSeverity" />
		/// class. The severity is set on <see cref="P:log4net.Appender.LocalSyslogAppender.LevelSeverity.Severity" />.
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
		/// The syslog facility defines which subsystem the logging comes from.
		/// This is set on the <see cref="P:log4net.Appender.LocalSyslogAppender.Facility" /> property.
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
		/// The facility. The default facility is <see cref="F:log4net.Appender.LocalSyslogAppender.SyslogFacility.User" />.
		/// </summary>
		private SyslogFacility m_facility = SyslogFacility.User;

		/// <summary>
		/// The message identity
		/// </summary>
		private string m_identity;

		/// <summary>
		/// Marshaled handle to the identity string. We have to hold on to the
		/// string as the <c>openlog</c> and <c>syslog</c> APIs just hold the
		/// pointer to the ident and dereference it for each log message.
		/// </summary>
		private IntPtr m_handleToIdentity = IntPtr.Zero;

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
		/// by setting the <see cref="P:log4net.Appender.LocalSyslogAppender.Identity" /> property. The identity (also know 
		/// as the tag) must not contain white space. The default value for the
		/// identity is the application name (from <see cref="P:log4net.Util.SystemInfo.ApplicationFriendlyName" />).
		/// </para>
		/// </remarks>
		public string Identity
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
		/// Set to one of the <see cref="T:log4net.Appender.LocalSyslogAppender.SyslogFacility" /> values. The list of
		/// facilities is predefined and cannot be extended. The default value
		/// is <see cref="F:log4net.Appender.LocalSyslogAppender.SyslogFacility.User" />.
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
		/// This appender requires a <see cref="P:log4net.Appender.AppenderSkeleton.Layout" /> to be set.
		/// </summary>
		/// <value><c>true</c></value>
		/// <remarks>
		/// <para>
		/// This appender requires a <see cref="P:log4net.Appender.AppenderSkeleton.Layout" /> to be set.
		/// </para>
		/// </remarks>
		protected override bool RequiresLayout => true;

		/// <summary>
		/// Add a mapping of level to severity
		/// </summary>
		/// <param name="mapping">The mapping to add</param>
		/// <remarks>
		/// <para>
		/// Adds a <see cref="T:log4net.Appender.LocalSyslogAppender.LevelSeverity" /> to this appender.
		/// </para>
		/// </remarks>
		public void AddMapping(LevelSeverity mapping)
		{
			m_levelMapping.Add(mapping);
		}

		/// <summary>
		/// Initialize the appender based on the options set.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Appender.LocalSyslogAppender.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Appender.LocalSyslogAppender.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Appender.LocalSyslogAppender.ActivateOptions" /> must be called again.
		/// </para>
		/// </remarks>
		public override void ActivateOptions()
		{
			base.ActivateOptions();
			m_levelMapping.ActivateOptions();
			string text = m_identity;
			if (text == null)
			{
				text = SystemInfo.ApplicationFriendlyName;
			}
			m_handleToIdentity = Marshal.StringToHGlobalAnsi(text);
			openlog(m_handleToIdentity, 1, m_facility);
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
			int priority = GeneratePriority(m_facility, GetSeverity(loggingEvent.Level));
			string message = RenderLoggingEvent(loggingEvent);
			syslog(priority, "%s", message);
		}

		/// <summary>
		/// Close the syslog when the appender is closed
		/// </summary>
		/// <remarks>
		/// <para>
		/// Close the syslog when the appender is closed
		/// </para>
		/// </remarks>
		protected override void OnClose()
		{
			base.OnClose();
			try
			{
				closelog();
			}
			catch (DllNotFoundException)
			{
			}
			if (m_handleToIdentity != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(m_handleToIdentity);
			}
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
		private static int GeneratePriority(SyslogFacility facility, SyslogSeverity severity)
		{
			return (int)((int)facility * 8 + severity);
		}

		/// <summary>
		/// Open connection to system logger.
		/// </summary>
		[DllImport("libc")]
		private static extern void openlog(IntPtr ident, int option, SyslogFacility facility);

		/// <summary>
		/// Generate a log message.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The libc syslog method takes a format string and a variable argument list similar
		/// to the classic printf function. As this type of vararg list is not supported
		/// by C# we need to specify the arguments explicitly. Here we have specified the
		/// format string with a single message argument. The caller must set the format 
		/// string to <c>"%s"</c>.
		/// </para>
		/// </remarks>
		[DllImport("libc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern void syslog(int priority, string format, string message);

		/// <summary>
		/// Close descriptor used to write to system logger.
		/// </summary>
		[DllImport("libc")]
		private static extern void closelog();
	}
}
