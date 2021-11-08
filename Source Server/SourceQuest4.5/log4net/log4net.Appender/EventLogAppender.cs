using log4net.Core;
using log4net.Layout;
using log4net.Util;
using System;
using System.Diagnostics;
using System.Threading;

namespace log4net.Appender
{
	/// <summary>
	/// Writes events to the system event log.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <c>EventID</c> of the event log entry can be
	/// set using the <c>EventLogEventID</c> property (<see cref="P:log4net.Core.LoggingEvent.Properties" />)
	/// on the <see cref="T:log4net.Core.LoggingEvent" />.
	/// </para>
	/// <para>
	/// There is a limit of 32K characters for an event log message
	/// </para>
	/// <para>
	/// When configuring the EventLogAppender a mapping can be
	/// specified to map a logging level to an event log entry type. For example:
	/// </para>
	/// <code lang="XML">
	/// &lt;mapping&gt;
	/// 	&lt;level value="ERROR" /&gt;
	/// 	&lt;eventLogEntryType value="Error" /&gt;
	/// &lt;/mapping&gt;
	/// &lt;mapping&gt;
	/// 	&lt;level value="DEBUG" /&gt;
	/// 	&lt;eventLogEntryType value="Information" /&gt;
	/// &lt;/mapping&gt;
	/// </code>
	/// <para>
	/// The Level is the standard log4net logging level and eventLogEntryType can be any value
	/// from the <see cref="T:System.Diagnostics.EventLogEntryType" /> enum, i.e.:
	/// <list type="bullet">
	/// <item><term>Error</term><description>an error event</description></item>
	/// <item><term>Warning</term><description>a warning event</description></item>
	/// <item><term>Information</term><description>an informational event</description></item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <author>Aspi Havewala</author>
	/// <author>Douglas de la Torre</author>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	/// <author>Thomas Voss</author>
	public class EventLogAppender : AppenderSkeleton
	{
		/// <summary>
		/// A class to act as a mapping between the level that a logging call is made at and
		/// the color it should be displayed as.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Defines the mapping between a level and its event log entry type.
		/// </para>
		/// </remarks>
		public class Level2EventLogEntryType : LevelMappingEntry
		{
			private EventLogEntryType m_entryType;

			/// <summary>
			/// The <see cref="P:log4net.Appender.EventLogAppender.Level2EventLogEntryType.EventLogEntryType" /> for this entry
			/// </summary>
			/// <remarks>
			/// <para>
			/// Required property.
			/// The <see cref="P:log4net.Appender.EventLogAppender.Level2EventLogEntryType.EventLogEntryType" /> for this entry
			/// </para>
			/// </remarks>
			public EventLogEntryType EventLogEntryType
			{
				get
				{
					return m_entryType;
				}
				set
				{
					m_entryType = value;
				}
			}
		}

		/// <summary>
		/// The log name is the section in the event logs where the messages
		/// are stored.
		/// </summary>
		private string m_logName;

		/// <summary>
		/// Name of the application to use when logging.  This appears in the
		/// application column of the event log named by <see cref="F:log4net.Appender.EventLogAppender.m_logName" />.
		/// </summary>
		private string m_applicationName;

		/// <summary>
		/// The name of the machine which holds the event log. This is
		/// currently only allowed to be '.' i.e. the current machine.
		/// </summary>
		private string m_machineName;

		/// <summary>
		/// Mapping from level object to EventLogEntryType
		/// </summary>
		private LevelMapping m_levelMapping = new LevelMapping();

		/// <summary>
		/// The security context to use for privileged calls
		/// </summary>
		private SecurityContext m_securityContext;

		/// <summary>
		/// The name of the log where messages will be stored.
		/// </summary>
		/// <value>
		/// The string name of the log where messages will be stored.
		/// </value>
		/// <remarks>
		/// <para>This is the name of the log as it appears in the Event Viewer
		/// tree. The default value is to log into the <c>Application</c>
		/// log, this is where most applications write their events. However
		/// if you need a separate log for your application (or applications)
		/// then you should set the <see cref="P:log4net.Appender.EventLogAppender.LogName" /> appropriately.</para>
		/// <para>This should not be used to distinguish your event log messages
		/// from those of other applications, the <see cref="P:log4net.Appender.EventLogAppender.ApplicationName" />
		/// property should be used to distinguish events. This property should be 
		/// used to group together events into a single log.
		/// </para>
		/// </remarks>
		public string LogName
		{
			get
			{
				return m_logName;
			}
			set
			{
				m_logName = value;
			}
		}

		/// <summary>
		/// Property used to set the Application name.  This appears in the
		/// event logs when logging.
		/// </summary>
		/// <value>
		/// The string used to distinguish events from different sources.
		/// </value>
		/// <remarks>
		/// Sets the event log source property.
		/// </remarks>
		public string ApplicationName
		{
			get
			{
				return m_applicationName;
			}
			set
			{
				m_applicationName = value;
			}
		}

		/// <summary>
		/// This property is used to return the name of the computer to use
		/// when accessing the event logs.  Currently, this is the current
		/// computer, denoted by a dot "."
		/// </summary>
		/// <value>
		/// The string name of the machine holding the event log that 
		/// will be logged into.
		/// </value>
		/// <remarks>
		/// This property cannot be changed. It is currently set to '.'
		/// i.e. the local machine. This may be changed in future.
		/// </remarks>
		public string MachineName
		{
			get
			{
				return m_machineName;
			}
			set
			{
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="P:log4net.Appender.EventLogAppender.SecurityContext" /> used to write to the EventLog.
		/// </summary>
		/// <value>
		/// The <see cref="P:log4net.Appender.EventLogAppender.SecurityContext" /> used to write to the EventLog.
		/// </value>
		/// <remarks>
		/// <para>
		/// The system security context used to write to the EventLog.
		/// </para>
		/// <para>
		/// Unless a <see cref="P:log4net.Appender.EventLogAppender.SecurityContext" /> specified here for this appender
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
		/// Initializes a new instance of the <see cref="T:log4net.Appender.EventLogAppender" /> class.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Default constructor.
		/// </para>
		/// </remarks>
		public EventLogAppender()
		{
			m_applicationName = Thread.GetDomain().FriendlyName;
			m_logName = "Application";
			m_machineName = ".";
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Appender.EventLogAppender" /> class
		/// with the specified <see cref="T:log4net.Layout.ILayout" />.
		/// </summary>
		/// <param name="layout">The <see cref="T:log4net.Layout.ILayout" /> to use with this appender.</param>
		/// <remarks>
		/// <para>
		/// Obsolete constructor.
		/// </para>
		/// </remarks>
		[Obsolete("Instead use the default constructor and set the Layout property")]
		public EventLogAppender(ILayout layout)
			: this()
		{
			Layout = layout;
		}

		/// <summary>
		/// Add a mapping of level to <see cref="T:System.Diagnostics.EventLogEntryType" /> - done by the config file
		/// </summary>
		/// <param name="mapping">The mapping to add</param>
		/// <remarks>
		/// <para>
		/// Add a <see cref="T:log4net.Appender.EventLogAppender.Level2EventLogEntryType" /> mapping to this appender.
		/// Each mapping defines the event log entry type for a level.
		/// </para>
		/// </remarks>
		public void AddMapping(Level2EventLogEntryType mapping)
		{
			m_levelMapping.Add(mapping);
		}

		/// <summary>
		/// Initialize the appender based on the options set
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Appender.EventLogAppender.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Appender.EventLogAppender.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Appender.EventLogAppender.ActivateOptions" /> must be called again.
		/// </para>
		/// </remarks>
		public override void ActivateOptions()
		{
			base.ActivateOptions();
			if (m_securityContext == null)
			{
				m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
			}
			bool flag = false;
			string text = null;
			using (SecurityContext.Impersonate(this))
			{
				flag = EventLog.SourceExists(m_applicationName);
				if (flag)
				{
					text = EventLog.LogNameFromSourceName(m_applicationName, m_machineName);
				}
			}
			if (flag && text != m_logName)
			{
				LogLog.Debug("EventLogAppender: Changing event source [" + m_applicationName + "] from log [" + text + "] to log [" + m_logName + "]");
			}
			else if (!flag)
			{
				LogLog.Debug("EventLogAppender: Creating event source Source [" + m_applicationName + "] in log " + m_logName + "]");
			}
			string text2 = null;
			using (SecurityContext.Impersonate(this))
			{
				if (flag && text != m_logName)
				{
					EventLog.DeleteEventSource(m_applicationName, m_machineName);
					CreateEventSource(m_applicationName, m_logName, m_machineName);
					text2 = EventLog.LogNameFromSourceName(m_applicationName, m_machineName);
				}
				else if (!flag)
				{
					CreateEventSource(m_applicationName, m_logName, m_machineName);
					text2 = EventLog.LogNameFromSourceName(m_applicationName, m_machineName);
				}
			}
			m_levelMapping.ActivateOptions();
			LogLog.Debug("EventLogAppender: Source [" + m_applicationName + "] is registered to log [" + text2 + "]");
		}

		/// <summary>
		/// Create an event log source
		/// </summary>
		/// <remarks>
		/// Uses different API calls under NET_2_0
		/// </remarks>
		private static void CreateEventSource(string source, string logName, string machineName)
		{
			EventSourceCreationData eventSourceCreationData = new EventSourceCreationData(source, logName);
			eventSourceCreationData.MachineName = machineName;
			EventLog.CreateEventSource(eventSourceCreationData);
		}

		/// <summary>
		/// This method is called by the <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)" />
		/// method. 
		/// </summary>
		/// <param name="loggingEvent">the event to log</param>
		/// <remarks>
		/// <para>Writes the event to the system event log using the 
		/// <see cref="P:log4net.Appender.EventLogAppender.ApplicationName" />.</para>
		///
		/// <para>If the event has an <c>EventID</c> property (see <see cref="P:log4net.Core.LoggingEvent.Properties" />)
		/// set then this integer will be used as the event log event id.</para>
		///
		/// <para>
		/// There is a limit of 32K characters for an event log message
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent loggingEvent)
		{
			int eventID = 0;
			object obj = loggingEvent.LookupProperty("EventID");
			if (obj != null)
			{
				if (obj is int)
				{
					eventID = (int)obj;
				}
				else
				{
					string text = obj as string;
					if (text != null && text.Length > 0)
					{
						if (SystemInfo.TryParse(text, out int val))
						{
							eventID = val;
						}
						else
						{
							ErrorHandler.Error("Unable to parse event ID property [" + text + "].");
						}
					}
				}
			}
			try
			{
				string text2 = RenderLoggingEvent(loggingEvent);
				if (text2.Length > 32000)
				{
					text2 = text2.Substring(0, 32000);
				}
				EventLogEntryType entryType = GetEntryType(loggingEvent.Level);
				using (SecurityContext.Impersonate(this))
				{
					EventLog.WriteEntry(m_applicationName, text2, entryType, eventID);
				}
			}
			catch (Exception e)
			{
				ErrorHandler.Error("Unable to write to event log [" + m_logName + "] using source [" + m_applicationName + "]", e);
			}
		}

		/// <summary>
		/// Get the equivalent <see cref="T:System.Diagnostics.EventLogEntryType" /> for a <see cref="T:log4net.Core.Level" /> <paramref name="p" />
		/// </summary>
		/// <param name="level">the Level to convert to an EventLogEntryType</param>
		/// <returns>The equivalent <see cref="T:System.Diagnostics.EventLogEntryType" /> for a <see cref="T:log4net.Core.Level" /> <paramref name="p" /></returns>
		/// <remarks>
		/// Because there are fewer applicable <see cref="T:System.Diagnostics.EventLogEntryType" />
		/// values to use in logging levels than there are in the 
		/// <see cref="T:log4net.Core.Level" /> this is a one way mapping. There is
		/// a loss of information during the conversion.
		/// </remarks>
		protected virtual EventLogEntryType GetEntryType(Level level)
		{
			Level2EventLogEntryType level2EventLogEntryType = m_levelMapping.Lookup(level) as Level2EventLogEntryType;
			if (level2EventLogEntryType != null)
			{
				return level2EventLogEntryType.EventLogEntryType;
			}
			if (level >= Level.Error)
			{
				return EventLogEntryType.Error;
			}
			if (level == Level.Warn)
			{
				return EventLogEntryType.Warning;
			}
			return EventLogEntryType.Information;
		}
	}
}
