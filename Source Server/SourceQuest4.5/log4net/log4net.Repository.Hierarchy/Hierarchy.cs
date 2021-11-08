using log4net.Appender;
using log4net.Core;
using log4net.Util;
using System;
using System.Collections;
using System.Xml;

namespace log4net.Repository.Hierarchy
{
	/// <summary>
	/// Hierarchical organization of loggers
	/// </summary>
	/// <remarks>
	/// <para>
	/// <i>The casual user should not have to deal with this class
	/// directly.</i>
	/// </para>
	/// <para>
	/// This class is specialized in retrieving loggers by name and
	/// also maintaining the logger hierarchy. Implements the 
	/// <see cref="T:log4net.Repository.ILoggerRepository" /> interface.
	/// </para>
	/// <para>
	/// The structure of the logger hierarchy is maintained by the
	/// <see cref="M:log4net.Repository.Hierarchy.Hierarchy.GetLogger(System.String)" /> method. The hierarchy is such that children
	/// link to their parent but parents do not have any references to their
	/// children. Moreover, loggers can be instantiated in any order, in
	/// particular descendant before ancestor.
	/// </para>
	/// <para>
	/// In case a descendant is created before a particular ancestor,
	/// then it creates a provision node for the ancestor and adds itself
	/// to the provision node. Other descendants of the same ancestor add
	/// themselves to the previously created provision node.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class Hierarchy : LoggerRepositorySkeleton, IBasicRepositoryConfigurator, IXmlRepositoryConfigurator
	{
		/// <summary>
		/// A class to hold the value, name and display name for a level
		/// </summary>
		/// <remarks>
		/// <para>
		/// A class to hold the value, name and display name for a level
		/// </para>
		/// </remarks>
		internal class LevelEntry
		{
			private int m_levelValue = -1;

			private string m_levelName = null;

			private string m_levelDisplayName = null;

			/// <summary>
			/// Value of the level
			/// </summary>
			/// <remarks>
			/// <para>
			/// If the value is not set (defaults to -1) the value will be looked
			/// up for the current level with the same name.
			/// </para>
			/// </remarks>
			public int Value
			{
				get
				{
					return m_levelValue;
				}
				set
				{
					m_levelValue = value;
				}
			}

			/// <summary>
			/// Name of the level
			/// </summary>
			/// <value>
			/// The name of the level
			/// </value>
			/// <remarks>
			/// <para>
			/// The name of the level.
			/// </para>
			/// </remarks>
			public string Name
			{
				get
				{
					return m_levelName;
				}
				set
				{
					m_levelName = value;
				}
			}

			/// <summary>
			/// Display name for the level
			/// </summary>
			/// <value>
			/// The display name of the level
			/// </value>
			/// <remarks>
			/// <para>
			/// The display name of the level.
			/// </para>
			/// </remarks>
			public string DisplayName
			{
				get
				{
					return m_levelDisplayName;
				}
				set
				{
					m_levelDisplayName = value;
				}
			}

			/// <summary>
			/// Override <c>Object.ToString</c> to return sensible debug info
			/// </summary>
			/// <returns>string info about this object</returns>
			public override string ToString()
			{
				return "LevelEntry(Value=" + m_levelValue + ", Name=" + m_levelName + ", DisplayName=" + m_levelDisplayName + ")";
			}
		}

		/// <summary>
		/// A class to hold the key and data for a property set in the config file
		/// </summary>
		/// <remarks>
		/// <para>
		/// A class to hold the key and data for a property set in the config file
		/// </para>
		/// </remarks>
		internal class PropertyEntry
		{
			private string m_key = null;

			private object m_value = null;

			/// <summary>
			/// Property Key
			/// </summary>
			/// <value>
			/// Property Key
			/// </value>
			/// <remarks>
			/// <para>
			/// Property Key.
			/// </para>
			/// </remarks>
			public string Key
			{
				get
				{
					return m_key;
				}
				set
				{
					m_key = value;
				}
			}

			/// <summary>
			/// Property Value
			/// </summary>
			/// <value>
			/// Property Value
			/// </value>
			/// <remarks>
			/// <para>
			/// Property Value.
			/// </para>
			/// </remarks>
			public object Value
			{
				get
				{
					return m_value;
				}
				set
				{
					m_value = value;
				}
			}

			/// <summary>
			/// Override <c>Object.ToString</c> to return sensible debug info
			/// </summary>
			/// <returns>string info about this object</returns>
			public override string ToString()
			{
				return string.Concat("PropertyEntry(Key=", m_key, ", Value=", m_value, ")");
			}
		}

		private ILoggerFactory m_defaultFactory;

		private Hashtable m_ht;

		private Logger m_root;

		private bool m_emittedNoAppenderWarning = false;

		/// <summary>
		/// Has no appender warning been emitted
		/// </summary>
		/// <remarks>
		/// <para>
		/// Flag to indicate if we have already issued a warning
		/// about not having an appender warning.
		/// </para>
		/// </remarks>
		public bool EmittedNoAppenderWarning
		{
			get
			{
				return m_emittedNoAppenderWarning;
			}
			set
			{
				m_emittedNoAppenderWarning = value;
			}
		}

		/// <summary>
		/// Get the root of this hierarchy
		/// </summary>
		/// <remarks>
		/// <para>
		/// Get the root of this hierarchy.
		/// </para>
		/// </remarks>
		public Logger Root
		{
			get
			{
				if (m_root == null)
				{
					lock (this)
					{
						if (m_root == null)
						{
							Logger logger = m_defaultFactory.CreateLogger(null);
							logger.Hierarchy = this;
							m_root = logger;
						}
					}
				}
				return m_root;
			}
		}

		/// <summary>
		/// Gets or sets the default <see cref="T:log4net.Repository.Hierarchy.ILoggerFactory" /> instance.
		/// </summary>
		/// <value>The default <see cref="T:log4net.Repository.Hierarchy.ILoggerFactory" /></value>
		/// <remarks>
		/// <para>
		/// The logger factory is used to create logger instances.
		/// </para>
		/// </remarks>
		public ILoggerFactory LoggerFactory
		{
			get
			{
				return m_defaultFactory;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				m_defaultFactory = value;
			}
		}

		/// <summary>
		/// Event used to notify that a logger has been created.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Event raised when a logger is created.
		/// </para>
		/// </remarks>
		public event LoggerCreationEventHandler LoggerCreatedEvent
		{
			add
			{
				this.m_loggerCreatedEvent = (LoggerCreationEventHandler)Delegate.Combine(this.m_loggerCreatedEvent, value);
			}
			remove
			{
				this.m_loggerCreatedEvent = (LoggerCreationEventHandler)Delegate.Remove(this.m_loggerCreatedEvent, value);
			}
		}

		private event LoggerCreationEventHandler m_loggerCreatedEvent;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Repository.Hierarchy.Hierarchy" /> class.
		/// </para>
		/// </remarks>
		public Hierarchy()
			: this(new DefaultLoggerFactory())
		{
		}

		/// <summary>
		/// Construct with properties
		/// </summary>
		/// <param name="properties">The properties to pass to this repository.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Repository.Hierarchy.Hierarchy" /> class.
		/// </para>
		/// </remarks>
		public Hierarchy(PropertiesDictionary properties)
			: this(properties, new DefaultLoggerFactory())
		{
		}

		/// <summary>
		/// Construct with a logger factory
		/// </summary>
		/// <param name="loggerFactory">The factory to use to create new logger instances.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Repository.Hierarchy.Hierarchy" /> class with 
		/// the specified <see cref="T:log4net.Repository.Hierarchy.ILoggerFactory" />.
		/// </para>
		/// </remarks>
		public Hierarchy(ILoggerFactory loggerFactory)
			: this(new PropertiesDictionary(), loggerFactory)
		{
		}

		/// <summary>
		/// Construct with properties and a logger factory
		/// </summary>
		/// <param name="properties">The properties to pass to this repository.</param>
		/// <param name="loggerFactory">The factory to use to create new logger instances.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Repository.Hierarchy.Hierarchy" /> class with 
		/// the specified <see cref="T:log4net.Repository.Hierarchy.ILoggerFactory" />.
		/// </para>
		/// </remarks>
		public Hierarchy(PropertiesDictionary properties, ILoggerFactory loggerFactory)
			: base(properties)
		{
			if (loggerFactory == null)
			{
				throw new ArgumentNullException("loggerFactory");
			}
			m_defaultFactory = loggerFactory;
			m_ht = Hashtable.Synchronized(new Hashtable());
		}

		/// <summary>
		/// Test if a logger exists
		/// </summary>
		/// <param name="name">The name of the logger to lookup</param>
		/// <returns>The Logger object with the name specified</returns>
		/// <remarks>
		/// <para>
		/// Check if the named logger exists in the hierarchy. If so return
		/// its reference, otherwise returns <c>null</c>.
		/// </para>
		/// </remarks>
		public override ILogger Exists(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return m_ht[new LoggerKey(name)] as Logger;
		}

		/// <summary>
		/// Returns all the currently defined loggers in the hierarchy as an Array
		/// </summary>
		/// <returns>All the defined loggers</returns>
		/// <remarks>
		/// <para>
		/// Returns all the currently defined loggers in the hierarchy as an Array.
		/// The root logger is <b>not</b> included in the returned
		/// enumeration.
		/// </para>
		/// </remarks>
		public override ILogger[] GetCurrentLoggers()
		{
			ArrayList arrayList = new ArrayList(m_ht.Count);
			foreach (object value in m_ht.Values)
			{
				if (value is Logger)
				{
					arrayList.Add(value);
				}
			}
			return (Logger[])arrayList.ToArray(typeof(Logger));
		}

		/// <summary>
		/// Return a new logger instance named as the first parameter using
		/// the default factory.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Return a new logger instance named as the first parameter using
		/// the default factory.
		/// </para>
		/// <para>
		/// If a logger of that name already exists, then it will be
		/// returned.  Otherwise, a new logger will be instantiated and
		/// then linked with its existing ancestors as well as children.
		/// </para>
		/// </remarks>
		/// <param name="name">The name of the logger to retrieve</param>
		/// <returns>The logger object with the name specified</returns>
		public override ILogger GetLogger(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return GetLogger(name, m_defaultFactory);
		}

		/// <summary>
		/// Shutting down a hierarchy will <i>safely</i> close and remove
		/// all appenders in all loggers including the root logger.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Shutting down a hierarchy will <i>safely</i> close and remove
		/// all appenders in all loggers including the root logger.
		/// </para>
		/// <para>
		/// Some appenders need to be closed before the
		/// application exists. Otherwise, pending logging events might be
		/// lost.
		/// </para>
		/// <para>
		/// The <c>Shutdown</c> method is careful to close nested
		/// appenders before closing regular appenders. This is allows
		/// configurations where a regular appender is attached to a logger
		/// and again to a nested appender.
		/// </para>
		/// </remarks>
		public override void Shutdown()
		{
			LogLog.Debug("Hierarchy: Shutdown called on Hierarchy [" + Name + "]");
			Root.CloseNestedAppenders();
			lock (m_ht)
			{
				ILogger[] currentLoggers = GetCurrentLoggers();
				ILogger[] array = currentLoggers;
				for (int i = 0; i < array.Length; i++)
				{
					Logger logger = (Logger)array[i];
					logger.CloseNestedAppenders();
				}
				Root.RemoveAllAppenders();
				array = currentLoggers;
				for (int i = 0; i < array.Length; i++)
				{
					Logger logger = (Logger)array[i];
					logger.RemoveAllAppenders();
				}
			}
			base.Shutdown();
		}

		/// <summary>
		/// Reset all values contained in this hierarchy instance to their default.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Reset all values contained in this hierarchy instance to their
		/// default.  This removes all appenders from all loggers, sets
		/// the level of all non-root loggers to <c>null</c>,
		/// sets their additivity flag to <c>true</c> and sets the level
		/// of the root logger to <see cref="F:log4net.Core.Level.Debug" />. Moreover,
		/// message disabling is set its default "off" value.
		/// </para>
		/// <para>
		/// Existing loggers are not removed. They are just reset.
		/// </para>
		/// <para>
		/// This method should be used sparingly and with care as it will
		/// block all logging until it is completed.
		/// </para>
		/// </remarks>
		public override void ResetConfiguration()
		{
			Root.Level = Level.Debug;
			Threshold = Level.All;
			lock (m_ht)
			{
				Shutdown();
				ILogger[] currentLoggers = GetCurrentLoggers();
				for (int i = 0; i < currentLoggers.Length; i++)
				{
					Logger logger = (Logger)currentLoggers[i];
					logger.Level = null;
					logger.Additivity = true;
				}
			}
			base.ResetConfiguration();
			OnConfigurationChanged(null);
		}

		/// <summary>
		/// Log the logEvent through this hierarchy.
		/// </summary>
		/// <param name="logEvent">the event to log</param>
		/// <remarks>
		/// <para>
		/// This method should not normally be used to log.
		/// The <see cref="T:log4net.ILog" /> interface should be used 
		/// for routine logging. This interface can be obtained
		/// using the <see cref="M:log4net.LogManager.GetLogger(System.String)" /> method.
		/// </para>
		/// <para>
		/// The <c>logEvent</c> is delivered to the appropriate logger and
		/// that logger is then responsible for logging the event.
		/// </para>
		/// </remarks>
		public override void Log(LoggingEvent logEvent)
		{
			if (logEvent == null)
			{
				throw new ArgumentNullException("logEvent");
			}
			GetLogger(logEvent.LoggerName, m_defaultFactory).Log(logEvent);
		}

		/// <summary>
		/// Returns all the Appenders that are currently configured
		/// </summary>
		/// <returns>An array containing all the currently configured appenders</returns>
		/// <remarks>
		/// <para>
		/// Returns all the <see cref="T:log4net.Appender.IAppender" /> instances that are currently configured.
		/// All the loggers are searched for appenders. The appenders may also be containers
		/// for appenders and these are also searched for additional loggers.
		/// </para>
		/// <para>
		/// The list returned is unordered but does not contain duplicates.
		/// </para>
		/// </remarks>
		public override IAppender[] GetAppenders()
		{
			ArrayList arrayList = new ArrayList();
			CollectAppenders(arrayList, Root);
			ILogger[] currentLoggers = GetCurrentLoggers();
			for (int i = 0; i < currentLoggers.Length; i++)
			{
				Logger container = (Logger)currentLoggers[i];
				CollectAppenders(arrayList, container);
			}
			return (IAppender[])arrayList.ToArray(typeof(IAppender));
		}

		/// <summary>
		/// Collect the appenders from an <see cref="T:log4net.Core.IAppenderAttachable" />.
		/// The appender may also be a container.
		/// </summary>
		/// <param name="appenderList"></param>
		/// <param name="appender"></param>
		private static void CollectAppender(ArrayList appenderList, IAppender appender)
		{
			if (!appenderList.Contains(appender))
			{
				appenderList.Add(appender);
				IAppenderAttachable appenderAttachable = appender as IAppenderAttachable;
				if (appenderAttachable != null)
				{
					CollectAppenders(appenderList, appenderAttachable);
				}
			}
		}

		/// <summary>
		/// Collect the appenders from an <see cref="T:log4net.Core.IAppenderAttachable" /> container
		/// </summary>
		/// <param name="appenderList"></param>
		/// <param name="container"></param>
		private static void CollectAppenders(ArrayList appenderList, IAppenderAttachable container)
		{
			AppenderCollection.IAppenderCollectionEnumerator enumerator = container.Appenders.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IAppender current = enumerator.Current;
					CollectAppender(appenderList, current);
				}
			}
			finally
			{
				(enumerator as IDisposable)?.Dispose();
			}
		}

		/// <summary>
		/// Initialize the log4net system using the specified appender
		/// </summary>
		/// <param name="appender">the appender to use to log all logging events</param>
		void IBasicRepositoryConfigurator.Configure(IAppender appender)
		{
			BasicRepositoryConfigure(appender);
		}

		/// <summary>
		/// Initialize the log4net system using the specified appender
		/// </summary>
		/// <param name="appender">the appender to use to log all logging events</param>
		/// <remarks>
		/// <para>
		/// This method provides the same functionality as the 
		/// <see cref="M:log4net.Repository.IBasicRepositoryConfigurator.Configure(log4net.Appender.IAppender)" /> method implemented
		/// on this object, but it is protected and therefore can be called by subclasses.
		/// </para>
		/// </remarks>
		protected void BasicRepositoryConfigure(IAppender appender)
		{
			Root.AddAppender(appender);
			Configured = true;
			OnConfigurationChanged(null);
		}

		/// <summary>
		/// Initialize the log4net system using the specified config
		/// </summary>
		/// <param name="element">the element containing the root of the config</param>
		void IXmlRepositoryConfigurator.Configure(XmlElement element)
		{
			XmlRepositoryConfigure(element);
		}

		/// <summary>
		/// Initialize the log4net system using the specified config
		/// </summary>
		/// <param name="element">the element containing the root of the config</param>
		/// <remarks>
		/// <para>
		/// This method provides the same functionality as the 
		/// <see cref="M:log4net.Repository.IBasicRepositoryConfigurator.Configure(log4net.Appender.IAppender)" /> method implemented
		/// on this object, but it is protected and therefore can be called by subclasses.
		/// </para>
		/// </remarks>
		protected void XmlRepositoryConfigure(XmlElement element)
		{
			XmlHierarchyConfigurator xmlHierarchyConfigurator = new XmlHierarchyConfigurator(this);
			xmlHierarchyConfigurator.Configure(element);
			Configured = true;
			OnConfigurationChanged(null);
		}

		/// <summary>
		/// Test if this hierarchy is disabled for the specified <see cref="T:log4net.Core.Level" />.
		/// </summary>
		/// <param name="level">The level to check against.</param>
		/// <returns>
		/// <c>true</c> if the repository is disabled for the level argument, <c>false</c> otherwise.
		/// </returns>
		/// <remarks>
		/// <para>
		/// If this hierarchy has not been configured then this method will
		/// always return <c>true</c>.
		/// </para>
		/// <para>
		/// This method will return <c>true</c> if this repository is
		/// disabled for <c>level</c> object passed as parameter and
		/// <c>false</c> otherwise.
		/// </para>
		/// <para>
		/// See also the <see cref="P:log4net.Repository.ILoggerRepository.Threshold" /> property.
		/// </para>
		/// </remarks>
		public bool IsDisabled(Level level)
		{
			if ((object)level == null)
			{
				throw new ArgumentNullException("level");
			}
			if (Configured)
			{
				return Threshold > level;
			}
			return true;
		}

		/// <summary>
		/// Clear all logger definitions from the internal hashtable
		/// </summary>
		/// <remarks>
		/// <para>
		/// This call will clear all logger definitions from the internal
		/// hashtable. Invoking this method will irrevocably mess up the
		/// logger hierarchy.
		/// </para>
		/// <para>
		/// You should <b>really</b> know what you are doing before
		/// invoking this method.
		/// </para>
		/// </remarks>
		public void Clear()
		{
			m_ht.Clear();
		}

		/// <summary>
		/// Return a new logger instance named as the first parameter using
		/// <paramref name="factory" />.
		/// </summary>
		/// <param name="name">The name of the logger to retrieve</param>
		/// <param name="factory">The factory that will make the new logger instance</param>
		/// <returns>The logger object with the name specified</returns>
		/// <remarks>
		/// <para>
		/// If a logger of that name already exists, then it will be
		/// returned. Otherwise, a new logger will be instantiated by the
		/// <paramref name="factory" /> parameter and linked with its existing
		/// ancestors as well as children.
		/// </para>
		/// </remarks>
		public Logger GetLogger(string name, ILoggerFactory factory)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			LoggerKey key = new LoggerKey(name);
			lock (m_ht)
			{
				object obj = m_ht[key];
				if (obj == null)
				{
					Logger logger = factory.CreateLogger(name);
					logger.Hierarchy = this;
					m_ht[key] = logger;
					UpdateParents(logger);
					OnLoggerCreationEvent(logger);
					return logger;
				}
				Logger logger2 = obj as Logger;
				if (logger2 != null)
				{
					return logger2;
				}
				ProvisionNode provisionNode = obj as ProvisionNode;
				if (provisionNode != null)
				{
					Logger logger = factory.CreateLogger(name);
					logger.Hierarchy = this;
					m_ht[key] = logger;
					UpdateChildren(provisionNode, logger);
					UpdateParents(logger);
					OnLoggerCreationEvent(logger);
					return logger;
				}
				return null;
			}
		}

		/// <summary>
		/// Sends a logger creation event to all registered listeners
		/// </summary>
		/// <param name="logger">The newly created logger</param>
		/// <remarks>
		/// Raises the logger creation event.
		/// </remarks>
		protected virtual void OnLoggerCreationEvent(Logger logger)
		{
			this.m_loggerCreatedEvent?.Invoke(this, new LoggerCreationEventArgs(logger));
		}

		/// <summary>
		/// Updates all the parents of the specified logger
		/// </summary>
		/// <param name="log">The logger to update the parents for</param>
		/// <remarks>
		/// <para>
		/// This method loops through all the <i>potential</i> parents of
		/// <paramref name="log" />. There 3 possible cases:
		/// </para>
		/// <list type="number">
		/// 	<item>
		/// 		<term>No entry for the potential parent of <paramref name="log" /> exists</term>
		/// 		<description>
		/// 		We create a ProvisionNode for this potential 
		/// 		parent and insert <paramref name="log" /> in that provision node.
		/// 		</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>The entry is of type Logger for the potential parent.</term>
		/// 		<description>
		/// 		The entry is <paramref name="log" />'s nearest existing parent. We 
		/// 		update <paramref name="log" />'s parent field with this entry. We also break from 
		/// 		he loop because updating our parent's parent is our parent's 
		/// 		responsibility.
		/// 		</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>The entry is of type ProvisionNode for this potential parent.</term>
		/// 		<description>
		/// 		We add <paramref name="log" /> to the list of children for this 
		/// 		potential parent.
		/// 		</description>
		/// 	</item>
		/// </list>
		/// </remarks>
		private void UpdateParents(Logger log)
		{
			string name = log.Name;
			int length = name.Length;
			bool flag = false;
			for (int num = name.LastIndexOf('.', length - 1); num >= 0; num = name.LastIndexOf('.', num - 1))
			{
				string name2 = name.Substring(0, num);
				LoggerKey key = new LoggerKey(name2);
				object obj = m_ht[key];
				if (obj == null)
				{
					ProvisionNode value = new ProvisionNode(log);
					m_ht[key] = value;
				}
				else
				{
					Logger logger = obj as Logger;
					if (logger != null)
					{
						flag = true;
						log.Parent = logger;
						break;
					}
					ProvisionNode provisionNode = obj as ProvisionNode;
					if (provisionNode != null)
					{
						provisionNode.Add(log);
					}
					else
					{
						LogLog.Error(string.Concat("Hierarchy: Unexpected object type [", obj.GetType(), "] in ht."), new LogException());
					}
				}
			}
			if (!flag)
			{
				log.Parent = Root;
			}
		}

		/// <summary>
		/// Replace a <see cref="T:log4net.Repository.Hierarchy.ProvisionNode" /> with a <see cref="T:log4net.Repository.Hierarchy.Logger" /> in the hierarchy.
		/// </summary>
		/// <param name="pn"></param>
		/// <param name="log"></param>
		/// <remarks>
		/// <para>
		/// We update the links for all the children that placed themselves
		/// in the provision node 'pn'. The second argument 'log' is a
		/// reference for the newly created Logger, parent of all the
		/// children in 'pn'.
		/// </para>
		/// <para>
		/// We loop on all the children 'c' in 'pn'.
		/// </para>
		/// <para>
		/// If the child 'c' has been already linked to a child of
		/// 'log' then there is no need to update 'c'.
		/// </para>
		/// <para>
		/// Otherwise, we set log's parent field to c's parent and set
		/// c's parent field to log.
		/// </para>
		/// </remarks>
		private void UpdateChildren(ProvisionNode pn, Logger log)
		{
			for (int i = 0; i < pn.Count; i++)
			{
				Logger logger = (Logger)pn[i];
				if (!logger.Parent.Name.StartsWith(log.Name))
				{
					log.Parent = logger.Parent;
					logger.Parent = log;
				}
			}
		}

		/// <summary>
		/// Define or redefine a Level using the values in the <see cref="T:log4net.Repository.Hierarchy.Hierarchy.LevelEntry" /> argument
		/// </summary>
		/// <param name="levelEntry">the level values</param>
		/// <remarks>
		/// <para>
		/// Define or redefine a Level using the values in the <see cref="T:log4net.Repository.Hierarchy.Hierarchy.LevelEntry" /> argument
		/// </para>
		/// <para>
		/// Supports setting levels via the configuration file.
		/// </para>
		/// </remarks>
		internal void AddLevel(LevelEntry levelEntry)
		{
			if (levelEntry == null)
			{
				throw new ArgumentNullException("levelEntry");
			}
			if (levelEntry.Name == null)
			{
				throw new ArgumentNullException("levelEntry.Name");
			}
			if (levelEntry.Value == -1)
			{
				Level level = LevelMap[levelEntry.Name];
				if (level == null)
				{
					throw new InvalidOperationException("Cannot redefine level [" + levelEntry.Name + "] because it is not defined in the LevelMap. To define the level supply the level value.");
				}
				levelEntry.Value = level.Value;
			}
			LevelMap.Add(levelEntry.Name, levelEntry.Value, levelEntry.DisplayName);
		}

		/// <summary>
		/// Set a Property using the values in the <see cref="T:log4net.Repository.Hierarchy.Hierarchy.LevelEntry" /> argument
		/// </summary>
		/// <param name="propertyEntry">the property value</param>
		/// <remarks>
		/// <para>
		/// Set a Property using the values in the <see cref="T:log4net.Repository.Hierarchy.Hierarchy.LevelEntry" /> argument.
		/// </para>
		/// <para>
		/// Supports setting property values via the configuration file.
		/// </para>
		/// </remarks>
		internal void AddProperty(PropertyEntry propertyEntry)
		{
			if (propertyEntry == null)
			{
				throw new ArgumentNullException("propertyEntry");
			}
			if (propertyEntry.Key == null)
			{
				throw new ArgumentNullException("propertyEntry.Key");
			}
			base.Properties[propertyEntry.Key] = propertyEntry.Value;
		}
	}
}
