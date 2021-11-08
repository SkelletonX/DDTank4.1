using log4net.Repository;
using log4net.Util;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace log4net.Core
{
	/// <summary>
	/// The internal representation of logging events. 
	/// </summary>
	/// <remarks>
	/// <para>
	/// When an affirmative decision is made to log then a 
	/// <see cref="T:log4net.Core.LoggingEvent" /> instance is created. This instance 
	/// is passed around to the different log4net components.
	/// </para>
	/// <para>
	/// This class is of concern to those wishing to extend log4net.
	/// </para>
	/// <para>
	/// Some of the values in instances of <see cref="T:log4net.Core.LoggingEvent" />
	/// are considered volatile, that is the values are correct at the
	/// time the event is delivered to appenders, but will not be consistent
	/// at any time afterwards. If an event is to be stored and then processed
	/// at a later time these volatile values must be fixed by calling
	/// <see cref="M:log4net.Core.LoggingEvent.FixVolatileData" />. There is a performance penalty
	/// for incurred by calling <see cref="M:log4net.Core.LoggingEvent.FixVolatileData" /> but it
	/// is essential to maintaining data consistency.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	/// <author>Douglas de la Torre</author>
	/// <author>Daniel Cazzulino</author>
	[Serializable]
	public class LoggingEvent : ISerializable
	{
		/// <summary>
		/// The key into the Properties map for the host name value.
		/// </summary>
		public const string HostNameProperty = "log4net:HostName";

		/// <summary>
		/// The key into the Properties map for the thread identity value.
		/// </summary>
		public const string IdentityProperty = "log4net:Identity";

		/// <summary>
		/// The key into the Properties map for the user name value.
		/// </summary>
		public const string UserNameProperty = "log4net:UserName";

		/// <summary>
		/// The internal logging event data.
		/// </summary>
		private LoggingEventData m_data;

		/// <summary>
		/// The internal logging event data.
		/// </summary>
		private CompositeProperties m_compositeProperties;

		/// <summary>
		/// The internal logging event data.
		/// </summary>
		private PropertiesDictionary m_eventProperties;

		/// <summary>
		/// The fully qualified Type of the calling 
		/// logger class in the stack frame (i.e. the declaring type of the method).
		/// </summary>
		private readonly Type m_callerStackBoundaryDeclaringType;

		/// <summary>
		/// The application supplied message of logging event.
		/// </summary>
		private readonly object m_message;

		/// <summary>
		/// The exception that was thrown.
		/// </summary>
		/// <remarks>
		/// This is not serialized. The string representation
		/// is serialized instead.
		/// </remarks>
		private readonly Exception m_thrownException;

		/// <summary>
		/// The repository that generated the logging event
		/// </summary>
		/// <remarks>
		/// This is not serialized.
		/// </remarks>
		private ILoggerRepository m_repository = null;

		/// <summary>
		/// The fix state for this event
		/// </summary>
		/// <remarks>
		/// These flags indicate which fields have been fixed.
		/// Not serialized.
		/// </remarks>
		private FixFlags m_fixFlags = FixFlags.None;

		/// <summary>
		/// Indicated that the internal cache is updateable (ie not fixed)
		/// </summary>
		/// <remarks>
		/// This is a seperate flag to m_fixFlags as it allows incrementel fixing and simpler
		/// changes in the caching strategy.
		/// </remarks>
		private bool m_cacheUpdatable = true;

		/// <summary>
		/// Gets the time when the current process started.
		/// </summary>
		/// <value>
		/// This is the time when this process started.
		/// </value>
		/// <remarks>
		/// <para>
		/// The TimeStamp is stored in the local time zone for this computer.
		/// </para>
		/// <para>
		/// Tries to get the start time for the current process.
		/// Failing that it returns the time of the first call to
		/// this property.
		/// </para>
		/// <para>
		/// Note that AppDomains may be loaded and unloaded within the
		/// same process without the process terminating and therefore
		/// without the process start time being reset.
		/// </para>
		/// </remarks>
		public static DateTime StartTime => SystemInfo.ProcessStartTime;

		/// <summary>
		/// Gets the <see cref="P:log4net.Core.LoggingEvent.Level" /> of the logging event.
		/// </summary>
		/// <value>
		/// The <see cref="P:log4net.Core.LoggingEvent.Level" /> of the logging event.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets the <see cref="P:log4net.Core.LoggingEvent.Level" /> of the logging event.
		/// </para>
		/// </remarks>
		public Level Level => m_data.Level;

		/// <summary>
		/// Gets the time of the logging event.
		/// </summary>
		/// <value>
		/// The time of the logging event.
		/// </value>
		/// <remarks>
		/// <para>
		/// The TimeStamp is stored in the local time zone for this computer.
		/// </para>
		/// </remarks>
		public DateTime TimeStamp => m_data.TimeStamp;

		/// <summary>
		/// Gets the name of the logger that logged the event.
		/// </summary>
		/// <value>
		/// The name of the logger that logged the event.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets the name of the logger that logged the event.
		/// </para>
		/// </remarks>
		public string LoggerName => m_data.LoggerName;

		/// <summary>
		/// Gets the location information for this logging event.
		/// </summary>
		/// <value>
		/// The location information for this logging event.
		/// </value>
		/// <remarks>
		/// <para>
		/// The collected information is cached for future use.
		/// </para>
		/// <para>
		/// See the <see cref="T:log4net.Core.LocationInfo" /> class for more information on
		/// supported frameworks and the different behavior in Debug and
		/// Release builds.
		/// </para>
		/// </remarks>
		public LocationInfo LocationInformation
		{
			get
			{
				if (m_data.LocationInfo == null && m_cacheUpdatable)
				{
					m_data.LocationInfo = new LocationInfo(m_callerStackBoundaryDeclaringType);
				}
				return m_data.LocationInfo;
			}
		}

		/// <summary>
		/// Gets the message object used to initialize this event.
		/// </summary>
		/// <value>
		/// The message object used to initialize this event.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets the message object used to initialize this event.
		/// Note that this event may not have a valid message object.
		/// If the event is serialized the message object will not 
		/// be transferred. To get the text of the message the
		/// <see cref="P:log4net.Core.LoggingEvent.RenderedMessage" /> property must be used 
		/// not this property.
		/// </para>
		/// <para>
		/// If there is no defined message object for this event then
		/// null will be returned.
		/// </para>
		/// </remarks>
		public object MessageObject => m_message;

		/// <summary>
		/// Gets the exception object used to initialize this event.
		/// </summary>
		/// <value>
		/// The exception object used to initialize this event.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets the exception object used to initialize this event.
		/// Note that this event may not have a valid exception object.
		/// If the event is serialized the exception object will not 
		/// be transferred. To get the text of the exception the
		/// <see cref="M:log4net.Core.LoggingEvent.GetExceptionString" /> method must be used 
		/// not this property.
		/// </para>
		/// <para>
		/// If there is no defined exception object for this event then
		/// null will be returned.
		/// </para>
		/// </remarks>
		public Exception ExceptionObject => m_thrownException;

		/// <summary>
		/// The <see cref="T:log4net.Repository.ILoggerRepository" /> that this event was created in.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="T:log4net.Repository.ILoggerRepository" /> that this event was created in.
		/// </para>
		/// </remarks>
		public ILoggerRepository Repository => m_repository;

		/// <summary>
		/// Gets the message, rendered through the <see cref="P:log4net.Repository.ILoggerRepository.RendererMap" />.
		/// </summary>
		/// <value>
		/// The message rendered through the <see cref="P:log4net.Repository.ILoggerRepository.RendererMap" />.
		/// </value>
		/// <remarks>
		/// <para>
		/// The collected information is cached for future use.
		/// </para>
		/// </remarks>
		public string RenderedMessage
		{
			get
			{
				if (m_data.Message == null && m_cacheUpdatable)
				{
					if (m_message == null)
					{
						m_data.Message = "";
					}
					else if (m_message is string)
					{
						m_data.Message = (m_message as string);
					}
					else if (m_repository != null)
					{
						m_data.Message = m_repository.RendererMap.FindAndRender(m_message);
					}
					else
					{
						m_data.Message = m_message.ToString();
					}
				}
				return m_data.Message;
			}
		}

		/// <summary>
		/// Gets the name of the current thread.  
		/// </summary>
		/// <value>
		/// The name of the current thread, or the thread ID when 
		/// the name is not available.
		/// </value>
		/// <remarks>
		/// <para>
		/// The collected information is cached for future use.
		/// </para>
		/// </remarks>
		public string ThreadName
		{
			get
			{
				if (m_data.ThreadName == null && m_cacheUpdatable)
				{
					m_data.ThreadName = Thread.CurrentThread.Name;
					if (m_data.ThreadName == null || m_data.ThreadName.Length == 0)
					{
						try
						{
							m_data.ThreadName = SystemInfo.CurrentThreadId.ToString(NumberFormatInfo.InvariantInfo);
						}
						catch (SecurityException)
						{
							LogLog.Debug("LoggingEvent: Security exception while trying to get current thread ID. Error Ignored. Empty thread name.");
							m_data.ThreadName = Thread.CurrentThread.GetHashCode().ToString(CultureInfo.InvariantCulture);
						}
					}
				}
				return m_data.ThreadName;
			}
		}

		/// <summary>
		/// Gets the name of the current user.
		/// </summary>
		/// <value>
		/// The name of the current user, or <c>NOT AVAILABLE</c> when the 
		/// underlying runtime has no support for retrieving the name of the 
		/// current user.
		/// </value>
		/// <remarks>
		/// <para>
		/// Calls <c>WindowsIdentity.GetCurrent().Name</c> to get the name of
		/// the current windows user.
		/// </para>
		/// <para>
		/// To improve performance, we could cache the string representation of 
		/// the name, and reuse that as long as the identity stayed constant.  
		/// Once the identity changed, we would need to re-assign and re-render 
		/// the string.
		/// </para>
		/// <para>
		/// However, the <c>WindowsIdentity.GetCurrent()</c> call seems to 
		/// return different objects every time, so the current implementation 
		/// doesn't do this type of caching.
		/// </para>
		/// <para>
		/// Timing for these operations:
		/// </para>
		/// <list type="table">
		///   <listheader>
		///     <term>Method</term>
		///     <description>Results</description>
		///   </listheader>
		///   <item>
		///     <term><c>WindowsIdentity.GetCurrent()</c></term>
		///     <description>10000 loops, 00:00:00.2031250 seconds</description>
		///   </item>
		///   <item>
		///     <term><c>WindowsIdentity.GetCurrent().Name</c></term>
		///     <description>10000 loops, 00:00:08.0468750 seconds</description>
		///   </item>
		/// </list>
		/// <para>
		/// This means we could speed things up almost 40 times by caching the 
		/// value of the <c>WindowsIdentity.GetCurrent().Name</c> property, since 
		/// this takes (8.04-0.20) = 7.84375 seconds.
		/// </para>
		/// </remarks>
		public string UserName
		{
			get
			{
				if (m_data.UserName == null && m_cacheUpdatable)
				{
					try
					{
						WindowsIdentity current = WindowsIdentity.GetCurrent();
						if (current != null && current.Name != null)
						{
							m_data.UserName = current.Name;
						}
						else
						{
							m_data.UserName = "";
						}
					}
					catch (SecurityException)
					{
						LogLog.Debug("LoggingEvent: Security exception while trying to get current windows identity. Error Ignored. Empty user name.");
						m_data.UserName = "";
					}
				}
				return m_data.UserName;
			}
		}

		/// <summary>
		/// Gets the identity of the current thread principal.
		/// </summary>
		/// <value>
		/// The string name of the identity of the current thread principal.
		/// </value>
		/// <remarks>
		/// <para>
		/// Calls <c>System.Threading.Thread.CurrentPrincipal.Identity.Name</c> to get
		/// the name of the current thread principal.
		/// </para>
		/// </remarks>
		public string Identity
		{
			get
			{
				if (m_data.Identity == null && m_cacheUpdatable)
				{
					try
					{
						if (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity != null && Thread.CurrentPrincipal.Identity.Name != null)
						{
							m_data.Identity = Thread.CurrentPrincipal.Identity.Name;
						}
						else
						{
							m_data.Identity = "";
						}
					}
					catch (SecurityException)
					{
						LogLog.Debug("LoggingEvent: Security exception while trying to get current thread principal. Error Ignored. Empty identity name.");
						m_data.Identity = "";
					}
				}
				return m_data.Identity;
			}
		}

		/// <summary>
		/// Gets the AppDomain friendly name.
		/// </summary>
		/// <value>
		/// The AppDomain friendly name.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets the AppDomain friendly name.
		/// </para>
		/// </remarks>
		public string Domain
		{
			get
			{
				if (m_data.Domain == null && m_cacheUpdatable)
				{
					m_data.Domain = SystemInfo.ApplicationFriendlyName;
				}
				return m_data.Domain;
			}
		}

		/// <summary>
		/// Additional event specific properties.
		/// </summary>
		/// <value>
		/// Additional event specific properties.
		/// </value>
		/// <remarks>
		/// <para>
		/// A logger or an appender may attach additional
		/// properties to specific events. These properties
		/// have a string key and an object value.
		/// </para>
		/// <para>
		/// This property is for events that have been added directly to
		/// this event. The aggregate properties (which include these
		/// event properties) can be retrieved using <see cref="M:log4net.Core.LoggingEvent.LookupProperty(System.String)" />
		/// and <see cref="M:log4net.Core.LoggingEvent.GetProperties" />.
		/// </para>
		/// <para>
		/// Once the properties have been fixed <see cref="P:log4net.Core.LoggingEvent.Fix" /> this property
		/// returns the combined cached properties. This ensures that updates to
		/// this property are always reflected in the underlying storage. When
		/// returning the combined properties there may be more keys in the
		/// Dictionary than expected.
		/// </para>
		/// </remarks>
		public PropertiesDictionary Properties
		{
			get
			{
				if (m_data.Properties != null)
				{
					return m_data.Properties;
				}
				if (m_eventProperties == null)
				{
					m_eventProperties = new PropertiesDictionary();
				}
				return m_eventProperties;
			}
		}

		/// <summary>
		/// The fixed fields in this event
		/// </summary>
		/// <value>
		/// The set of fields that are fixed in this event
		/// </value>
		/// <remarks>
		/// <para>
		/// Fields will not be fixed if they have previously been fixed.
		/// It is not possible to 'unfix' a field.
		/// </para>
		/// </remarks>
		public FixFlags Fix
		{
			get
			{
				return m_fixFlags;
			}
			set
			{
				FixVolatileData(value);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Core.LoggingEvent" /> class
		/// from the supplied parameters.
		/// </summary>
		/// <param name="callerStackBoundaryDeclaringType">The declaring type of the method that is
		/// the stack boundary into the logging system for this call.</param>
		/// <param name="repository">The repository this event is logged in.</param>
		/// <param name="loggerName">The name of the logger of this event.</param>
		/// <param name="level">The level of this event.</param>
		/// <param name="message">The message of this event.</param>
		/// <param name="exception">The exception for this event.</param>
		/// <remarks>
		/// <para>
		/// Except <see cref="P:log4net.Core.LoggingEvent.TimeStamp" />, <see cref="P:log4net.Core.LoggingEvent.Level" /> and <see cref="P:log4net.Core.LoggingEvent.LoggerName" />, 
		/// all fields of <c>LoggingEvent</c> are filled when actually needed. Call
		/// <see cref="M:log4net.Core.LoggingEvent.FixVolatileData" /> to cache all data locally
		/// to prevent inconsistencies.
		/// </para>
		/// <para>This method is called by the log4net framework
		/// to create a logging event.
		/// </para>
		/// </remarks>
		public LoggingEvent(Type callerStackBoundaryDeclaringType, ILoggerRepository repository, string loggerName, Level level, object message, Exception exception)
		{
			m_callerStackBoundaryDeclaringType = callerStackBoundaryDeclaringType;
			m_message = message;
			m_repository = repository;
			m_thrownException = exception;
			m_data.LoggerName = loggerName;
			m_data.Level = level;
			m_data.TimeStamp = DateTime.Now;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Core.LoggingEvent" /> class 
		/// using specific data.
		/// </summary>
		/// <param name="callerStackBoundaryDeclaringType">The declaring type of the method that is
		/// the stack boundary into the logging system for this call.</param>
		/// <param name="repository">The repository this event is logged in.</param>
		/// <param name="data">Data used to initialize the logging event.</param>
		/// <param name="fixedData">The fields in the <paranref name="data" /> struct that have already been fixed.</param>
		/// <remarks>
		/// <para>
		/// This constructor is provided to allow a <see cref="T:log4net.Core.LoggingEvent" />
		/// to be created independently of the log4net framework. This can
		/// be useful if you require a custom serialization scheme.
		/// </para>
		/// <para>
		/// Use the <see cref="M:log4net.Core.LoggingEvent.GetLoggingEventData(log4net.Core.FixFlags)" /> method to obtain an 
		/// instance of the <see cref="T:log4net.Core.LoggingEventData" /> class.
		/// </para>
		/// <para>
		/// The <paramref name="fixedData" /> parameter should be used to specify which fields in the
		/// <paramref name="data" /> struct have been preset. Fields not specified in the <paramref name="fixedData" />
		/// will be captured from the environment if requested or fixed.
		/// </para>
		/// </remarks>
		public LoggingEvent(Type callerStackBoundaryDeclaringType, ILoggerRepository repository, LoggingEventData data, FixFlags fixedData)
		{
			m_callerStackBoundaryDeclaringType = callerStackBoundaryDeclaringType;
			m_repository = repository;
			m_data = data;
			m_fixFlags = fixedData;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Core.LoggingEvent" /> class 
		/// using specific data.
		/// </summary>
		/// <param name="callerStackBoundaryDeclaringType">The declaring type of the method that is
		/// the stack boundary into the logging system for this call.</param>
		/// <param name="repository">The repository this event is logged in.</param>
		/// <param name="data">Data used to initialize the logging event.</param>
		/// <remarks>
		/// <para>
		/// This constructor is provided to allow a <see cref="T:log4net.Core.LoggingEvent" />
		/// to be created independently of the log4net framework. This can
		/// be useful if you require a custom serialization scheme.
		/// </para>
		/// <para>
		/// Use the <see cref="M:log4net.Core.LoggingEvent.GetLoggingEventData(log4net.Core.FixFlags)" /> method to obtain an 
		/// instance of the <see cref="T:log4net.Core.LoggingEventData" /> class.
		/// </para>
		/// <para>
		/// This constructor sets this objects <see cref="P:log4net.Core.LoggingEvent.Fix" /> flags to <see cref="F:log4net.Core.FixFlags.All" />,
		/// this assumes that all the data relating to this event is passed in via the <paramref name="data" />
		/// parameter and no other data should be captured from the environment.
		/// </para>
		/// </remarks>
		public LoggingEvent(Type callerStackBoundaryDeclaringType, ILoggerRepository repository, LoggingEventData data)
			: this(callerStackBoundaryDeclaringType, repository, data, FixFlags.All)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Core.LoggingEvent" /> class 
		/// using specific data.
		/// </summary>
		/// <param name="data">Data used to initialize the logging event.</param>
		/// <remarks>
		/// <para>
		/// This constructor is provided to allow a <see cref="T:log4net.Core.LoggingEvent" />
		/// to be created independently of the log4net framework. This can
		/// be useful if you require a custom serialization scheme.
		/// </para>
		/// <para>
		/// Use the <see cref="M:log4net.Core.LoggingEvent.GetLoggingEventData(log4net.Core.FixFlags)" /> method to obtain an 
		/// instance of the <see cref="T:log4net.Core.LoggingEventData" /> class.
		/// </para>
		/// <para>
		/// This constructor sets this objects <see cref="P:log4net.Core.LoggingEvent.Fix" /> flags to <see cref="F:log4net.Core.FixFlags.All" />,
		/// this assumes that all the data relating to this event is passed in via the <paramref name="data" />
		/// parameter and no other data should be captured from the environment.
		/// </para>
		/// </remarks>
		public LoggingEvent(LoggingEventData data)
			: this(null, null, data)
		{
		}

		/// <summary>
		/// Serialization constructor
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Core.LoggingEvent" /> class 
		/// with serialized data.
		/// </para>
		/// </remarks>
		protected LoggingEvent(SerializationInfo info, StreamingContext context)
		{
			m_data.LoggerName = info.GetString("LoggerName");
			m_data.Level = (Level)info.GetValue("Level", typeof(Level));
			m_data.Message = info.GetString("Message");
			m_data.ThreadName = info.GetString("ThreadName");
			m_data.TimeStamp = info.GetDateTime("TimeStamp");
			m_data.LocationInfo = (LocationInfo)info.GetValue("LocationInfo", typeof(LocationInfo));
			m_data.UserName = info.GetString("UserName");
			m_data.ExceptionString = info.GetString("ExceptionString");
			m_data.Properties = (PropertiesDictionary)info.GetValue("Properties", typeof(PropertiesDictionary));
			m_data.Domain = info.GetString("Domain");
			m_data.Identity = info.GetString("Identity");
			m_fixFlags = FixFlags.All;
		}

		/// <summary>
		/// Ensure that the repository is set.
		/// </summary>
		/// <param name="repository">the value for the repository</param>
		internal void EnsureRepository(ILoggerRepository repository)
		{
			if (repository != null)
			{
				m_repository = repository;
			}
		}

		/// <summary>
		/// Write the rendered message to a TextWriter
		/// </summary>
		/// <param name="writer">the writer to write the message to</param>
		/// <remarks>
		/// <para>
		/// Unlike the <see cref="P:log4net.Core.LoggingEvent.RenderedMessage" /> property this method
		/// does store the message data in the internal cache. Therefore 
		/// if called only once this method should be faster than the
		/// <see cref="P:log4net.Core.LoggingEvent.RenderedMessage" /> property, however if the message is
		/// to be accessed multiple times then the property will be more efficient.
		/// </para>
		/// </remarks>
		public void WriteRenderedMessage(TextWriter writer)
		{
			if (m_data.Message != null)
			{
				writer.Write(m_data.Message);
			}
			else if (m_message != null)
			{
				if (m_message is string)
				{
					writer.Write(m_message as string);
				}
				else if (m_repository != null)
				{
					m_repository.RendererMap.FindAndRender(m_message, writer);
				}
				else
				{
					writer.Write(m_message.ToString());
				}
			}
		}

		/// <summary>
		/// Serializes this object into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> provided.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="context">The destination for this serialization.</param>
		/// <remarks>
		/// <para>
		/// The data in this event must be fixed before it can be serialized.
		/// </para>
		/// <para>
		/// The <see cref="M:log4net.Core.LoggingEvent.FixVolatileData" /> method must be called during the
		/// <see cref="M:log4net.Appender.IAppender.DoAppend(log4net.Core.LoggingEvent)" /> method call if this event 
		/// is to be used outside that method.
		/// </para>
		/// </remarks>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("LoggerName", m_data.LoggerName);
			info.AddValue("Level", m_data.Level);
			info.AddValue("Message", m_data.Message);
			info.AddValue("ThreadName", m_data.ThreadName);
			info.AddValue("TimeStamp", m_data.TimeStamp);
			info.AddValue("LocationInfo", m_data.LocationInfo);
			info.AddValue("UserName", m_data.UserName);
			info.AddValue("ExceptionString", m_data.ExceptionString);
			info.AddValue("Properties", m_data.Properties);
			info.AddValue("Domain", m_data.Domain);
			info.AddValue("Identity", m_data.Identity);
		}

		/// <summary>
		/// Gets the portable data for this <see cref="T:log4net.Core.LoggingEvent" />.
		/// </summary>
		/// <returns>The <see cref="T:log4net.Core.LoggingEventData" /> for this event.</returns>
		/// <remarks>
		/// <para>
		/// A new <see cref="T:log4net.Core.LoggingEvent" /> can be constructed using a
		/// <see cref="T:log4net.Core.LoggingEventData" /> instance.
		/// </para>
		/// <para>
		/// Does a <see cref="F:log4net.Core.FixFlags.Partial" /> fix of the data
		/// in the logging event before returning the event data.
		/// </para>
		/// </remarks>
		public LoggingEventData GetLoggingEventData()
		{
			return GetLoggingEventData(FixFlags.Partial);
		}

		/// <summary>
		/// Gets the portable data for this <see cref="T:log4net.Core.LoggingEvent" />.
		/// </summary>
		/// <param name="fixFlags">The set of data to ensure is fixed in the LoggingEventData</param>
		/// <returns>The <see cref="T:log4net.Core.LoggingEventData" /> for this event.</returns>
		/// <remarks>
		/// <para>
		/// A new <see cref="T:log4net.Core.LoggingEvent" /> can be constructed using a
		/// <see cref="T:log4net.Core.LoggingEventData" /> instance.
		/// </para>
		/// </remarks>
		public LoggingEventData GetLoggingEventData(FixFlags fixFlags)
		{
			Fix = fixFlags;
			return m_data;
		}

		/// <summary>
		/// Returns this event's exception's rendered using the 
		/// <see cref="P:log4net.Repository.ILoggerRepository.RendererMap" />.
		/// </summary>
		/// <returns>
		/// This event's exception's rendered using the <see cref="P:log4net.Repository.ILoggerRepository.RendererMap" />.
		/// </returns>
		/// <remarks>
		/// <para>
		/// <b>Obsolete. Use <see cref="M:log4net.Core.LoggingEvent.GetExceptionString" /> instead.</b>
		/// </para>
		/// </remarks>
		[Obsolete("Use GetExceptionString instead")]
		public string GetExceptionStrRep()
		{
			return GetExceptionString();
		}

		/// <summary>
		/// Returns this event's exception's rendered using the 
		/// <see cref="P:log4net.Repository.ILoggerRepository.RendererMap" />.
		/// </summary>
		/// <returns>
		/// This event's exception's rendered using the <see cref="P:log4net.Repository.ILoggerRepository.RendererMap" />.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Returns this event's exception's rendered using the 
		/// <see cref="P:log4net.Repository.ILoggerRepository.RendererMap" />.
		/// </para>
		/// </remarks>
		public string GetExceptionString()
		{
			if (m_data.ExceptionString == null && m_cacheUpdatable)
			{
				if (m_thrownException != null)
				{
					if (m_repository != null)
					{
						m_data.ExceptionString = m_repository.RendererMap.FindAndRender(m_thrownException);
					}
					else
					{
						m_data.ExceptionString = m_thrownException.ToString();
					}
				}
				else
				{
					m_data.ExceptionString = "";
				}
			}
			return m_data.ExceptionString;
		}

		/// <summary>
		/// Fix instance fields that hold volatile data.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Some of the values in instances of <see cref="T:log4net.Core.LoggingEvent" />
		/// are considered volatile, that is the values are correct at the
		/// time the event is delivered to appenders, but will not be consistent
		/// at any time afterwards. If an event is to be stored and then processed
		/// at a later time these volatile values must be fixed by calling
		/// <see cref="M:log4net.Core.LoggingEvent.FixVolatileData" />. There is a performance penalty
		/// incurred by calling <see cref="M:log4net.Core.LoggingEvent.FixVolatileData" /> but it
		/// is essential to maintaining data consistency.
		/// </para>
		/// <para>
		/// Calling <see cref="M:log4net.Core.LoggingEvent.FixVolatileData" /> is equivalent to
		/// calling <see cref="M:log4net.Core.LoggingEvent.FixVolatileData(System.Boolean)" /> passing the parameter
		/// <c>false</c>.
		/// </para>
		/// <para>
		/// See <see cref="M:log4net.Core.LoggingEvent.FixVolatileData(System.Boolean)" /> for more
		/// information.
		/// </para>
		/// </remarks>
		[Obsolete("Use Fix property")]
		public void FixVolatileData()
		{
			Fix = FixFlags.All;
		}

		/// <summary>
		/// Fixes instance fields that hold volatile data.
		/// </summary>
		/// <param name="fastButLoose">Set to <c>true</c> to not fix data that takes a long time to fix.</param>
		/// <remarks>
		/// <para>
		/// Some of the values in instances of <see cref="T:log4net.Core.LoggingEvent" />
		/// are considered volatile, that is the values are correct at the
		/// time the event is delivered to appenders, but will not be consistent
		/// at any time afterwards. If an event is to be stored and then processed
		/// at a later time these volatile values must be fixed by calling
		/// <see cref="M:log4net.Core.LoggingEvent.FixVolatileData" />. There is a performance penalty
		/// for incurred by calling <see cref="M:log4net.Core.LoggingEvent.FixVolatileData" /> but it
		/// is essential to maintaining data consistency.
		/// </para>
		/// <para>
		/// The <paramref name="fastButLoose" /> param controls the data that
		/// is fixed. Some of the data that can be fixed takes a long time to 
		/// generate, therefore if you do not require those settings to be fixed
		/// they can be ignored by setting the <paramref name="fastButLoose" /> param
		/// to <c>true</c>. This setting will ignore the <see cref="P:log4net.Core.LoggingEvent.LocationInformation" />
		/// and <see cref="P:log4net.Core.LoggingEvent.UserName" /> settings.
		/// </para>
		/// <para>
		/// Set <paramref name="fastButLoose" /> to <c>false</c> to ensure that all 
		/// settings are fixed.
		/// </para>
		/// </remarks>
		[Obsolete("Use Fix property")]
		public void FixVolatileData(bool fastButLoose)
		{
			if (fastButLoose)
			{
				Fix = FixFlags.Partial;
			}
			else
			{
				Fix = FixFlags.All;
			}
		}

		/// <summary>
		/// Fix the fields specified by the <see cref="T:log4net.Core.FixFlags" /> parameter
		/// </summary>
		/// <param name="flags">the fields to fix</param>
		/// <remarks>
		/// <para>
		/// Only fields specified in the <paramref name="flags" /> will be fixed.
		/// Fields will not be fixed if they have previously been fixed.
		/// It is not possible to 'unfix' a field.
		/// </para>
		/// </remarks>
		protected void FixVolatileData(FixFlags flags)
		{
			object obj = null;
			m_cacheUpdatable = true;
			FixFlags fixFlags = (flags ^ m_fixFlags) & flags;
			if (fixFlags > FixFlags.None)
			{
				if ((fixFlags & FixFlags.Message) != 0)
				{
					obj = RenderedMessage;
					m_fixFlags |= FixFlags.Message;
				}
				if ((fixFlags & FixFlags.ThreadName) != 0)
				{
					obj = ThreadName;
					m_fixFlags |= FixFlags.ThreadName;
				}
				if ((fixFlags & FixFlags.LocationInfo) != 0)
				{
					obj = LocationInformation;
					m_fixFlags |= FixFlags.LocationInfo;
				}
				if ((fixFlags & FixFlags.UserName) != 0)
				{
					obj = UserName;
					m_fixFlags |= FixFlags.UserName;
				}
				if ((fixFlags & FixFlags.Domain) != 0)
				{
					obj = Domain;
					m_fixFlags |= FixFlags.Domain;
				}
				if ((fixFlags & FixFlags.Identity) != 0)
				{
					obj = Identity;
					m_fixFlags |= FixFlags.Identity;
				}
				if ((fixFlags & FixFlags.Exception) != 0)
				{
					obj = GetExceptionString();
					m_fixFlags |= FixFlags.Exception;
				}
				if ((fixFlags & FixFlags.Properties) != 0)
				{
					CacheProperties();
					m_fixFlags |= FixFlags.Properties;
				}
			}
			if (obj != null)
			{
			}
			m_cacheUpdatable = false;
		}

		private void CreateCompositeProperties()
		{
			m_compositeProperties = new CompositeProperties();
			if (m_eventProperties != null)
			{
				m_compositeProperties.Add(m_eventProperties);
			}
			PropertiesDictionary properties = LogicalThreadContext.Properties.GetProperties(create: false);
			if (properties != null)
			{
				m_compositeProperties.Add(properties);
			}
			PropertiesDictionary properties2 = ThreadContext.Properties.GetProperties(create: false);
			if (properties2 != null)
			{
				m_compositeProperties.Add(properties2);
			}
			m_compositeProperties.Add(GlobalContext.Properties.GetReadOnlyProperties());
		}

		private void CacheProperties()
		{
			if (m_data.Properties == null && m_cacheUpdatable)
			{
				if (m_compositeProperties == null)
				{
					CreateCompositeProperties();
				}
				PropertiesDictionary propertiesDictionary = m_compositeProperties.Flatten();
				PropertiesDictionary propertiesDictionary2 = new PropertiesDictionary();
				foreach (DictionaryEntry item in (IEnumerable)propertiesDictionary)
				{
					string text = item.Key as string;
					if (text != null)
					{
						object obj = item.Value;
						IFixingRequired fixingRequired = obj as IFixingRequired;
						if (fixingRequired != null)
						{
							obj = fixingRequired.GetFixedObject();
						}
						if (obj != null)
						{
							propertiesDictionary2[text] = obj;
						}
					}
				}
				m_data.Properties = propertiesDictionary2;
			}
		}

		/// <summary>
		/// Lookup a composite property in this event
		/// </summary>
		/// <param name="key">the key for the property to lookup</param>
		/// <returns>the value for the property</returns>
		/// <remarks>
		/// <para>
		/// This event has composite properties that combine together properties from
		/// several different contexts in the following order:
		/// <list type="definition">
		/// 	<item>
		/// 		<term>this events properties</term>
		/// 		<description>
		/// 		This event has <see cref="P:log4net.Core.LoggingEvent.Properties" /> that can be set. These 
		/// 		properties are specific to this event only.
		/// 		</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>the thread properties</term>
		/// 		<description>
		/// 		The <see cref="P:log4net.ThreadContext.Properties" /> that are set on the current
		/// 		thread. These properties are shared by all events logged on this thread.
		/// 		</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>the global properties</term>
		/// 		<description>
		/// 		The <see cref="P:log4net.GlobalContext.Properties" /> that are set globally. These 
		/// 		properties are shared by all the threads in the AppDomain.
		/// 		</description>
		/// 	</item>
		/// </list>
		/// </para>
		/// </remarks>
		public object LookupProperty(string key)
		{
			if (m_data.Properties != null)
			{
				return m_data.Properties[key];
			}
			if (m_compositeProperties == null)
			{
				CreateCompositeProperties();
			}
			return m_compositeProperties[key];
		}

		/// <summary>
		/// Get all the composite properties in this event
		/// </summary>
		/// <returns>the <see cref="T:log4net.Util.PropertiesDictionary" /> containing all the properties</returns>
		/// <remarks>
		/// <para>
		/// See <see cref="M:log4net.Core.LoggingEvent.LookupProperty(System.String)" /> for details of the composite properties 
		/// stored by the event.
		/// </para>
		/// <para>
		/// This method returns a single <see cref="T:log4net.Util.PropertiesDictionary" /> containing all the
		/// properties defined for this event.
		/// </para>
		/// </remarks>
		public PropertiesDictionary GetProperties()
		{
			if (m_data.Properties != null)
			{
				return m_data.Properties;
			}
			if (m_compositeProperties == null)
			{
				CreateCompositeProperties();
			}
			return m_compositeProperties.Flatten();
		}
	}
}
