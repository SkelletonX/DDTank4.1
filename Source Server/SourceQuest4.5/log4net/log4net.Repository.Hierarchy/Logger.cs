using log4net.Appender;
using log4net.Core;
using log4net.Util;
using System;
using System.Security;

namespace log4net.Repository.Hierarchy
{
	/// <summary>
	/// Implementation of <see cref="T:log4net.Core.ILogger" /> used by <see cref="P:log4net.Repository.Hierarchy.Logger.Hierarchy" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// Internal class used to provide implementation of <see cref="T:log4net.Core.ILogger" />
	/// interface. Applications should use <see cref="T:log4net.LogManager" /> to get
	/// logger instances.
	/// </para>
	/// <para>
	/// This is one of the central classes in the log4net implementation. One of the
	/// distinctive features of log4net are hierarchical loggers and their
	/// evaluation. The <see cref="P:log4net.Repository.Hierarchy.Logger.Hierarchy" /> organizes the <see cref="T:log4net.Repository.Hierarchy.Logger" />
	/// instances into a rooted tree hierarchy.
	/// </para>
	/// <para>
	/// The <see cref="T:log4net.Repository.Hierarchy.Logger" /> class is abstract. Only concrete subclasses of
	/// <see cref="T:log4net.Repository.Hierarchy.Logger" /> can be created. The <see cref="T:log4net.Repository.Hierarchy.ILoggerFactory" />
	/// is used to create instances of this type for the <see cref="P:log4net.Repository.Hierarchy.Logger.Hierarchy" />.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	/// <author>Aspi Havewala</author>
	/// <author>Douglas de la Torre</author>
	public abstract class Logger : IAppenderAttachable, ILogger
	{
		/// <summary>
		/// The fully qualified type of the Logger class.
		/// </summary>
		private static readonly Type ThisDeclaringType = typeof(Logger);

		/// <summary>
		/// The name of this logger.
		/// </summary>
		private readonly string m_name;

		/// <summary>
		/// The assigned level of this logger. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <c>level</c> variable need not be 
		/// assigned a value in which case it is inherited 
		/// form the hierarchy.
		/// </para>
		/// </remarks>
		private Level m_level;

		/// <summary>
		/// The parent of this logger.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The parent of this logger. 
		/// All loggers have at least one ancestor which is the root logger.
		/// </para>
		/// </remarks>
		private Logger m_parent;

		/// <summary>
		/// Loggers need to know what Hierarchy they are in.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Loggers need to know what Hierarchy they are in.
		/// The hierarchy that this logger is a member of is stored
		/// here.
		/// </para>
		/// </remarks>
		private Hierarchy m_hierarchy;

		/// <summary>
		/// Helper implementation of the <see cref="T:log4net.Core.IAppenderAttachable" /> interface
		/// </summary>
		private AppenderAttachedImpl m_appenderAttachedImpl;

		/// <summary>
		/// Flag indicating if child loggers inherit their parents appenders
		/// </summary>
		/// <remarks>
		/// <para>
		/// Additivity is set to true by default, that is children inherit
		/// the appenders of their ancestors by default. If this variable is
		/// set to <c>false</c> then the appenders found in the
		/// ancestors of this logger are not used. However, the children
		/// of this logger will inherit its appenders, unless the children
		/// have their additivity flag set to <c>false</c> too. See
		/// the user manual for more details.
		/// </para>
		/// </remarks>
		private bool m_additive = true;

		/// <summary>
		/// Lock to protect AppenderAttachedImpl variable m_appenderAttachedImpl
		/// </summary>
		private readonly ReaderWriterLock m_appenderLock = new ReaderWriterLock();

		/// <summary>
		/// Gets or sets the parent logger in the hierarchy.
		/// </summary>
		/// <value>
		/// The parent logger in the hierarchy.
		/// </value>
		/// <remarks>
		/// <para>
		/// Part of the Composite pattern that makes the hierarchy.
		/// The hierarchy is parent linked rather than child linked.
		/// </para>
		/// </remarks>
		public virtual Logger Parent
		{
			get
			{
				return m_parent;
			}
			set
			{
				m_parent = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating if child loggers inherit their parent's appenders.
		/// </summary>
		/// <value>
		/// <c>true</c> if child loggers inherit their parent's appenders.
		/// </value>
		/// <remarks>
		/// <para>
		/// Additivity is set to <c>true</c> by default, that is children inherit
		/// the appenders of their ancestors by default. If this variable is
		/// set to <c>false</c> then the appenders found in the
		/// ancestors of this logger are not used. However, the children
		/// of this logger will inherit its appenders, unless the children
		/// have their additivity flag set to <c>false</c> too. See
		/// the user manual for more details.
		/// </para>
		/// </remarks>
		public virtual bool Additivity
		{
			get
			{
				return m_additive;
			}
			set
			{
				m_additive = value;
			}
		}

		/// <summary>
		/// Gets the effective level for this logger.
		/// </summary>
		/// <returns>The nearest level in the logger hierarchy.</returns>
		/// <remarks>
		/// <para>
		/// Starting from this logger, searches the logger hierarchy for a
		/// non-null level and returns it. Otherwise, returns the level of the
		/// root logger.
		/// </para>
		/// <para>The Logger class is designed so that this method executes as
		/// quickly as possible.</para>
		/// </remarks>
		public virtual Level EffectiveLevel
		{
			get
			{
				for (Logger logger = this; logger != null; logger = logger.m_parent)
				{
					Level level = logger.m_level;
					if ((object)level != null)
					{
						return level;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="P:log4net.Repository.Hierarchy.Logger.Hierarchy" /> where this 
		/// <c>Logger</c> instance is attached to.
		/// </summary>
		/// <value>The hierarchy that this logger belongs to.</value>
		/// <remarks>
		/// <para>
		/// This logger must be attached to a single <see cref="P:log4net.Repository.Hierarchy.Logger.Hierarchy" />.
		/// </para>
		/// </remarks>
		public virtual Hierarchy Hierarchy
		{
			get
			{
				return m_hierarchy;
			}
			set
			{
				m_hierarchy = value;
			}
		}

		/// <summary>
		/// Gets or sets the assigned <see cref="P:log4net.Repository.Hierarchy.Logger.Level" />, if any, for this Logger.  
		/// </summary>
		/// <value>
		/// The <see cref="P:log4net.Repository.Hierarchy.Logger.Level" /> of this logger.
		/// </value>
		/// <remarks>
		/// <para>
		/// The assigned <see cref="P:log4net.Repository.Hierarchy.Logger.Level" /> can be <c>null</c>.
		/// </para>
		/// </remarks>
		public virtual Level Level
		{
			get
			{
				return m_level;
			}
			set
			{
				m_level = value;
			}
		}

		/// <summary>
		/// Get the appenders contained in this logger as an 
		/// <see cref="T:System.Collections.ICollection" />.
		/// </summary>
		/// <returns>A collection of the appenders in this logger</returns>
		/// <remarks>
		/// <para>
		/// Get the appenders contained in this logger as an 
		/// <see cref="T:System.Collections.ICollection" />. If no appenders 
		/// can be found, then a <see cref="T:log4net.Util.EmptyCollection" /> is returned.
		/// </para>
		/// </remarks>
		public virtual AppenderCollection Appenders
		{
			get
			{
				m_appenderLock.AcquireReaderLock();
				try
				{
					if (m_appenderAttachedImpl == null)
					{
						return AppenderCollection.EmptyCollection;
					}
					return m_appenderAttachedImpl.Appenders;
				}
				finally
				{
					m_appenderLock.ReleaseReaderLock();
				}
			}
		}

		/// <summary>
		/// Gets the logger name.
		/// </summary>
		/// <value>
		/// The name of the logger.
		/// </value>
		/// <remarks>
		/// <para>
		/// The name of this logger
		/// </para>
		/// </remarks>
		public virtual string Name => m_name;

		/// <summary>
		/// Gets the <see cref="T:log4net.Repository.ILoggerRepository" /> where this 
		/// <c>Logger</c> instance is attached to.
		/// </summary>
		/// <value>
		/// The <see cref="T:log4net.Repository.ILoggerRepository" /> that this logger belongs to.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets the <see cref="T:log4net.Repository.ILoggerRepository" /> where this 
		/// <c>Logger</c> instance is attached to.
		/// </para>
		/// </remarks>
		public ILoggerRepository Repository => m_hierarchy;

		/// <summary>
		/// This constructor created a new <see cref="T:log4net.Repository.Hierarchy.Logger" /> instance and
		/// sets its name.
		/// </summary>
		/// <param name="name">The name of the <see cref="T:log4net.Repository.Hierarchy.Logger" />.</param>
		/// <remarks>
		/// <para>
		/// This constructor is protected and designed to be used by
		/// a subclass that is not abstract.
		/// </para>
		/// <para>
		/// Loggers are constructed by <see cref="T:log4net.Repository.Hierarchy.ILoggerFactory" /> 
		/// objects. See <see cref="T:log4net.Repository.Hierarchy.DefaultLoggerFactory" /> for the default
		/// logger creator.
		/// </para>
		/// </remarks>
		protected Logger(string name)
		{
			m_name = string.Intern(name);
		}

		/// <summary>
		/// Add <paramref name="newAppender" /> to the list of appenders of this
		/// Logger instance.
		/// </summary>
		/// <param name="newAppender">An appender to add to this logger</param>
		/// <remarks>
		/// <para>
		/// Add <paramref name="newAppender" /> to the list of appenders of this
		/// Logger instance.
		/// </para>
		/// <para>
		/// If <paramref name="newAppender" /> is already in the list of
		/// appenders, then it won't be added again.
		/// </para>
		/// </remarks>
		public virtual void AddAppender(IAppender newAppender)
		{
			if (newAppender == null)
			{
				throw new ArgumentNullException("newAppender");
			}
			m_appenderLock.AcquireWriterLock();
			try
			{
				if (m_appenderAttachedImpl == null)
				{
					m_appenderAttachedImpl = new AppenderAttachedImpl();
				}
				m_appenderAttachedImpl.AddAppender(newAppender);
			}
			finally
			{
				m_appenderLock.ReleaseWriterLock();
			}
		}

		/// <summary>
		/// Look for the appender named as <c>name</c>
		/// </summary>
		/// <param name="name">The name of the appender to lookup</param>
		/// <returns>The appender with the name specified, or <c>null</c>.</returns>
		/// <remarks>
		/// <para>
		/// Returns the named appender, or null if the appender is not found.
		/// </para>
		/// </remarks>
		public virtual IAppender GetAppender(string name)
		{
			m_appenderLock.AcquireReaderLock();
			try
			{
				if (m_appenderAttachedImpl == null || name == null)
				{
					return null;
				}
				return m_appenderAttachedImpl.GetAppender(name);
			}
			finally
			{
				m_appenderLock.ReleaseReaderLock();
			}
		}

		/// <summary>
		/// Remove all previously added appenders from this Logger instance.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Remove all previously added appenders from this Logger instance.
		/// </para>
		/// <para>
		/// This is useful when re-reading configuration information.
		/// </para>
		/// </remarks>
		public virtual void RemoveAllAppenders()
		{
			m_appenderLock.AcquireWriterLock();
			try
			{
				if (m_appenderAttachedImpl != null)
				{
					m_appenderAttachedImpl.RemoveAllAppenders();
					m_appenderAttachedImpl = null;
				}
			}
			finally
			{
				m_appenderLock.ReleaseWriterLock();
			}
		}

		/// <summary>
		/// Remove the appender passed as parameter form the list of appenders.
		/// </summary>
		/// <param name="appender">The appender to remove</param>
		/// <returns>The appender removed from the list</returns>
		/// <remarks>
		/// <para>
		/// Remove the appender passed as parameter form the list of appenders.
		/// The appender removed is not closed.
		/// If you are discarding the appender you must call
		/// <see cref="M:log4net.Appender.IAppender.Close" /> on the appender removed.
		/// </para>
		/// </remarks>
		public virtual IAppender RemoveAppender(IAppender appender)
		{
			m_appenderLock.AcquireWriterLock();
			try
			{
				if (appender != null && m_appenderAttachedImpl != null)
				{
					return m_appenderAttachedImpl.RemoveAppender(appender);
				}
			}
			finally
			{
				m_appenderLock.ReleaseWriterLock();
			}
			return null;
		}

		/// <summary>
		/// Remove the appender passed as parameter form the list of appenders.
		/// </summary>
		/// <param name="name">The name of the appender to remove</param>
		/// <returns>The appender removed from the list</returns>
		/// <remarks>
		/// <para>
		/// Remove the named appender passed as parameter form the list of appenders.
		/// The appender removed is not closed.
		/// If you are discarding the appender you must call
		/// <see cref="M:log4net.Appender.IAppender.Close" /> on the appender removed.
		/// </para>
		/// </remarks>
		public virtual IAppender RemoveAppender(string name)
		{
			m_appenderLock.AcquireWriterLock();
			try
			{
				if (name != null && m_appenderAttachedImpl != null)
				{
					return m_appenderAttachedImpl.RemoveAppender(name);
				}
			}
			finally
			{
				m_appenderLock.ReleaseWriterLock();
			}
			return null;
		}

		/// <summary>
		/// This generic form is intended to be used by wrappers.
		/// </summary>
		/// <param name="callerStackBoundaryDeclaringType">The declaring type of the method that is
		/// the stack boundary into the logging system for this call.</param>
		/// <param name="level">The level of the message to be logged.</param>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		/// <remarks>
		/// <para>
		/// Generate a logging event for the specified <paramref name="level" /> using
		/// the <paramref name="message" /> and <paramref name="exception" />.
		/// </para>
		/// <para>
		/// This method must not throw any exception to the caller.
		/// </para>
		/// </remarks>
		public virtual void Log(Type callerStackBoundaryDeclaringType, Level level, object message, Exception exception)
		{
			try
			{
				if (IsEnabledFor(level))
				{
					ForcedLog(((object)callerStackBoundaryDeclaringType != null) ? callerStackBoundaryDeclaringType : ThisDeclaringType, level, message, exception);
				}
			}
			catch (Exception exception2)
			{
				LogLog.Error("Log: Exception while logging", exception2);
			}
			catch
			{
				LogLog.Error("Log: Exception while logging");
			}
		}

		/// <summary>
		/// This is the most generic printing method that is intended to be used 
		/// by wrappers.
		/// </summary>
		/// <param name="logEvent">The event being logged.</param>
		/// <remarks>
		/// <para>
		/// Logs the specified logging event through this logger.
		/// </para>
		/// <para>
		/// This method must not throw any exception to the caller.
		/// </para>
		/// </remarks>
		public virtual void Log(LoggingEvent logEvent)
		{
			try
			{
				if (logEvent != null && IsEnabledFor(logEvent.Level))
				{
					ForcedLog(logEvent);
				}
			}
			catch (Exception exception)
			{
				LogLog.Error("Log: Exception while logging", exception);
			}
			catch
			{
				LogLog.Error("Log: Exception while logging");
			}
		}

		/// <summary>
		/// Checks if this logger is enabled for a given <see cref="P:log4net.Repository.Hierarchy.Logger.Level" /> passed as parameter.
		/// </summary>
		/// <param name="level">The level to check.</param>
		/// <returns>
		/// <c>true</c> if this logger is enabled for <c>level</c>, otherwise <c>false</c>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Test if this logger is going to log events of the specified <paramref name="level" />.
		/// </para>
		/// <para>
		/// This method must not throw any exception to the caller.
		/// </para>
		/// </remarks>
		public virtual bool IsEnabledFor(Level level)
		{
			try
			{
				if (level != null)
				{
					if (m_hierarchy.IsDisabled(level))
					{
						return false;
					}
					return level >= EffectiveLevel;
				}
			}
			catch (Exception exception)
			{
				LogLog.Error("Log: Exception while logging", exception);
			}
			catch
			{
				LogLog.Error("Log: Exception while logging");
			}
			return false;
		}

		/// <summary>
		/// Deliver the <see cref="T:log4net.Core.LoggingEvent" /> to the attached appenders.
		/// </summary>
		/// <param name="loggingEvent">The event to log.</param>
		/// <remarks>
		/// <para>
		/// Call the appenders in the hierarchy starting at
		/// <c>this</c>. If no appenders could be found, emit a
		/// warning.
		/// </para>
		/// <para>
		/// This method calls all the appenders inherited from the
		/// hierarchy circumventing any evaluation of whether to log or not
		/// to log the particular log request.
		/// </para>
		/// </remarks>
		protected virtual void CallAppenders(LoggingEvent loggingEvent)
		{
			if (loggingEvent == null)
			{
				throw new ArgumentNullException("loggingEvent");
			}
			int num = 0;
			for (Logger logger = this; logger != null; logger = logger.m_parent)
			{
				if (logger.m_appenderAttachedImpl != null)
				{
					logger.m_appenderLock.AcquireReaderLock();
					try
					{
						if (logger.m_appenderAttachedImpl != null)
						{
							num += logger.m_appenderAttachedImpl.AppendLoopOnAppenders(loggingEvent);
						}
					}
					finally
					{
						logger.m_appenderLock.ReleaseReaderLock();
					}
				}
				if (!logger.m_additive)
				{
					break;
				}
			}
			if (!m_hierarchy.EmittedNoAppenderWarning && num == 0)
			{
				LogLog.Debug("Logger: No appenders could be found for logger [" + Name + "] repository [" + Repository.Name + "]");
				LogLog.Debug("Logger: Please initialize the log4net system properly.");
				try
				{
					LogLog.Debug("Logger:    Current AppDomain context information: ");
					LogLog.Debug("Logger:       BaseDirectory   : " + SystemInfo.ApplicationBaseDirectory);
					LogLog.Debug("Logger:       FriendlyName    : " + AppDomain.CurrentDomain.FriendlyName);
					LogLog.Debug("Logger:       DynamicDirectory: " + AppDomain.CurrentDomain.DynamicDirectory);
				}
				catch (SecurityException)
				{
				}
				m_hierarchy.EmittedNoAppenderWarning = true;
			}
		}

		/// <summary>
		/// Closes all attached appenders implementing the <see cref="T:log4net.Core.IAppenderAttachable" /> interface.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Used to ensure that the appenders are correctly shutdown.
		/// </para>
		/// </remarks>
		public virtual void CloseNestedAppenders()
		{
			m_appenderLock.AcquireWriterLock();
			try
			{
				if (m_appenderAttachedImpl != null)
				{
					AppenderCollection appenders = m_appenderAttachedImpl.Appenders;
					AppenderCollection.IAppenderCollectionEnumerator enumerator = appenders.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							IAppender current = enumerator.Current;
							if (current is IAppenderAttachable)
							{
								current.Close();
							}
						}
					}
					finally
					{
						(enumerator as IDisposable)?.Dispose();
					}
				}
			}
			finally
			{
				m_appenderLock.ReleaseWriterLock();
			}
		}

		/// <summary>
		/// This is the most generic printing method. This generic form is intended to be used by wrappers
		/// </summary>
		/// <param name="level">The level of the message to be logged.</param>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		/// <remarks>
		/// <para>
		/// Generate a logging event for the specified <paramref name="level" /> using
		/// the <paramref name="message" />.
		/// </para>
		/// </remarks>
		public virtual void Log(Level level, object message, Exception exception)
		{
			if (IsEnabledFor(level))
			{
				ForcedLog(ThisDeclaringType, level, message, exception);
			}
		}

		/// <summary>
		/// Creates a new logging event and logs the event without further checks.
		/// </summary>
		/// <param name="callerStackBoundaryDeclaringType">The declaring type of the method that is
		/// the stack boundary into the logging system for this call.</param>
		/// <param name="level">The level of the message to be logged.</param>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">The exception to log, including its stack trace.</param>
		/// <remarks>
		/// <para>
		/// Generates a logging event and delivers it to the attached
		/// appenders.
		/// </para>
		/// </remarks>
		protected virtual void ForcedLog(Type callerStackBoundaryDeclaringType, Level level, object message, Exception exception)
		{
			CallAppenders(new LoggingEvent(callerStackBoundaryDeclaringType, Hierarchy, Name, level, message, exception));
		}

		/// <summary>
		/// Creates a new logging event and logs the event without further checks.
		/// </summary>
		/// <param name="logEvent">The event being logged.</param>
		/// <remarks>
		/// <para>
		/// Delivers the logging event to the attached appenders.
		/// </para>
		/// </remarks>
		protected virtual void ForcedLog(LoggingEvent logEvent)
		{
			logEvent.EnsureRepository(Hierarchy);
			CallAppenders(logEvent);
		}
	}
}
