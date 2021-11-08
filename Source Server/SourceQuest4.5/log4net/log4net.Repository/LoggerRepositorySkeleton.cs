using log4net.Appender;
using log4net.Core;
using log4net.ObjectRenderer;
using log4net.Plugin;
using log4net.Util;
using System;

namespace log4net.Repository
{
	/// <summary>
	/// Base implementation of <see cref="T:log4net.Repository.ILoggerRepository" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// Default abstract implementation of the <see cref="T:log4net.Repository.ILoggerRepository" /> interface.
	/// </para>
	/// <para>
	/// Skeleton implementation of the <see cref="T:log4net.Repository.ILoggerRepository" /> interface.
	/// All <see cref="T:log4net.Repository.ILoggerRepository" /> types can extend this type.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public abstract class LoggerRepositorySkeleton : ILoggerRepository
	{
		private string m_name;

		private RendererMap m_rendererMap;

		private PluginMap m_pluginMap;

		private LevelMap m_levelMap;

		private Level m_threshold;

		private bool m_configured;

		private PropertiesDictionary m_properties;

		/// <summary>
		/// The name of the repository
		/// </summary>
		/// <value>
		/// The string name of the repository
		/// </value>
		/// <remarks>
		/// <para>
		/// The name of this repository. The name is
		/// used to store and lookup the repositories 
		/// stored by the <see cref="T:log4net.Core.IRepositorySelector" />.
		/// </para>
		/// </remarks>
		public virtual string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		/// <summary>
		/// The threshold for all events in this repository
		/// </summary>
		/// <value>
		/// The threshold for all events in this repository
		/// </value>
		/// <remarks>
		/// <para>
		/// The threshold for all events in this repository
		/// </para>
		/// </remarks>
		public virtual Level Threshold
		{
			get
			{
				return m_threshold;
			}
			set
			{
				if (value != null)
				{
					m_threshold = value;
					return;
				}
				LogLog.Warn("LoggerRepositorySkeleton: Threshold cannot be set to null. Setting to ALL");
				m_threshold = Level.All;
			}
		}

		/// <summary>
		/// RendererMap accesses the object renderer map for this repository.
		/// </summary>
		/// <value>
		/// RendererMap accesses the object renderer map for this repository.
		/// </value>
		/// <remarks>
		/// <para>
		/// RendererMap accesses the object renderer map for this repository.
		/// </para>
		/// <para>
		/// The RendererMap holds a mapping between types and
		/// <see cref="T:log4net.ObjectRenderer.IObjectRenderer" /> objects.
		/// </para>
		/// </remarks>
		public virtual RendererMap RendererMap => m_rendererMap;

		/// <summary>
		/// The plugin map for this repository.
		/// </summary>
		/// <value>
		/// The plugin map for this repository.
		/// </value>
		/// <remarks>
		/// <para>
		/// The plugin map holds the <see cref="T:log4net.Plugin.IPlugin" /> instances
		/// that have been attached to this repository.
		/// </para>
		/// </remarks>
		public virtual PluginMap PluginMap => m_pluginMap;

		/// <summary>
		/// Get the level map for the Repository.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Get the level map for the Repository.
		/// </para>
		/// <para>
		/// The level map defines the mappings between
		/// level names and <see cref="T:log4net.Core.Level" /> objects in
		/// this repository.
		/// </para>
		/// </remarks>
		public virtual LevelMap LevelMap => m_levelMap;

		/// <summary>
		/// Flag indicates if this repository has been configured.
		/// </summary>
		/// <value>
		/// Flag indicates if this repository has been configured.
		/// </value>
		/// <remarks>
		/// <para>
		/// Flag indicates if this repository has been configured.
		/// </para>
		/// </remarks>
		public virtual bool Configured
		{
			get
			{
				return m_configured;
			}
			set
			{
				m_configured = value;
			}
		}

		/// <summary>
		/// Repository specific properties
		/// </summary>
		/// <value>
		/// Repository specific properties
		/// </value>
		/// <remarks>
		/// These properties can be specified on a repository specific basis
		/// </remarks>
		public PropertiesDictionary Properties => m_properties;

		private event LoggerRepositoryShutdownEventHandler m_shutdownEvent;

		private event LoggerRepositoryConfigurationResetEventHandler m_configurationResetEvent;

		private event LoggerRepositoryConfigurationChangedEventHandler m_configurationChangedEvent;

		/// <summary>
		/// Event to notify that the repository has been shutdown.
		/// </summary>
		/// <value>
		/// Event to notify that the repository has been shutdown.
		/// </value>
		/// <remarks>
		/// <para>
		/// Event raised when the repository has been shutdown.
		/// </para>
		/// </remarks>
		public event LoggerRepositoryShutdownEventHandler ShutdownEvent
		{
			add
			{
				this.m_shutdownEvent = (LoggerRepositoryShutdownEventHandler)Delegate.Combine(this.m_shutdownEvent, value);
			}
			remove
			{
				this.m_shutdownEvent = (LoggerRepositoryShutdownEventHandler)Delegate.Remove(this.m_shutdownEvent, value);
			}
		}

		/// <summary>
		/// Event to notify that the repository has had its configuration reset.
		/// </summary>
		/// <value>
		/// Event to notify that the repository has had its configuration reset.
		/// </value>
		/// <remarks>
		/// <para>
		/// Event raised when the repository's configuration has been
		/// reset to default.
		/// </para>
		/// </remarks>
		public event LoggerRepositoryConfigurationResetEventHandler ConfigurationReset
		{
			add
			{
				this.m_configurationResetEvent = (LoggerRepositoryConfigurationResetEventHandler)Delegate.Combine(this.m_configurationResetEvent, value);
			}
			remove
			{
				this.m_configurationResetEvent = (LoggerRepositoryConfigurationResetEventHandler)Delegate.Remove(this.m_configurationResetEvent, value);
			}
		}

		/// <summary>
		/// Event to notify that the repository has had its configuration changed.
		/// </summary>
		/// <value>
		/// Event to notify that the repository has had its configuration changed.
		/// </value>
		/// <remarks>
		/// <para>
		/// Event raised when the repository's configuration has been changed.
		/// </para>
		/// </remarks>
		public event LoggerRepositoryConfigurationChangedEventHandler ConfigurationChanged
		{
			add
			{
				this.m_configurationChangedEvent = (LoggerRepositoryConfigurationChangedEventHandler)Delegate.Combine(this.m_configurationChangedEvent, value);
			}
			remove
			{
				this.m_configurationChangedEvent = (LoggerRepositoryConfigurationChangedEventHandler)Delegate.Remove(this.m_configurationChangedEvent, value);
			}
		}

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes the repository with default (empty) properties.
		/// </para>
		/// </remarks>
		protected LoggerRepositorySkeleton()
			: this(new PropertiesDictionary())
		{
		}

		/// <summary>
		/// Construct the repository using specific properties
		/// </summary>
		/// <param name="properties">the properties to set for this repository</param>
		/// <remarks>
		/// <para>
		/// Initializes the repository with specified properties.
		/// </para>
		/// </remarks>
		protected LoggerRepositorySkeleton(PropertiesDictionary properties)
		{
			m_properties = properties;
			m_rendererMap = new RendererMap();
			m_pluginMap = new PluginMap(this);
			m_levelMap = new LevelMap();
			m_configured = false;
			AddBuiltinLevels();
			m_threshold = Level.All;
		}

		/// <summary>
		/// Test if logger exists
		/// </summary>
		/// <param name="name">The name of the logger to lookup</param>
		/// <returns>The Logger object with the name specified</returns>
		/// <remarks>
		/// <para>
		/// Check if the named logger exists in the repository. If so return
		/// its reference, otherwise returns <c>null</c>.
		/// </para>
		/// </remarks>
		public abstract ILogger Exists(string name);

		/// <summary>
		/// Returns all the currently defined loggers in the repository
		/// </summary>
		/// <returns>All the defined loggers</returns>
		/// <remarks>
		/// <para>
		/// Returns all the currently defined loggers in the repository as an Array.
		/// </para>
		/// </remarks>
		public abstract ILogger[] GetCurrentLoggers();

		/// <summary>
		/// Return a new logger instance
		/// </summary>
		/// <param name="name">The name of the logger to retrieve</param>
		/// <returns>The logger object with the name specified</returns>
		/// <remarks>
		/// <para>
		/// Return a new logger instance.
		/// </para>
		/// <para>
		/// If a logger of that name already exists, then it will be
		/// returned. Otherwise, a new logger will be instantiated and
		/// then linked with its existing ancestors as well as children.
		/// </para>
		/// </remarks>
		public abstract ILogger GetLogger(string name);

		/// <summary>
		/// Shutdown the repository
		/// </summary>
		/// <remarks>
		/// <para>
		/// Shutdown the repository. Can be overridden in a subclass.
		/// This base class implementation notifies the <see cref="E:log4net.Repository.LoggerRepositorySkeleton.ShutdownEvent" />
		/// listeners and all attached plugins of the shutdown event.
		/// </para>
		/// </remarks>
		public virtual void Shutdown()
		{
			PluginCollection.IPluginCollectionEnumerator enumerator = PluginMap.AllPlugins.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IPlugin current = enumerator.Current;
					current.Shutdown();
				}
			}
			finally
			{
				(enumerator as IDisposable)?.Dispose();
			}
			OnShutdown(null);
		}

		/// <summary>
		/// Reset the repositories configuration to a default state
		/// </summary>
		/// <remarks>
		/// <para>
		/// Reset all values contained in this instance to their
		/// default state.
		/// </para>
		/// <para>
		/// Existing loggers are not removed. They are just reset.
		/// </para>
		/// <para>
		/// This method should be used sparingly and with care as it will
		/// block all logging until it is completed.
		/// </para>
		/// </remarks>
		public virtual void ResetConfiguration()
		{
			m_rendererMap.Clear();
			m_levelMap.Clear();
			AddBuiltinLevels();
			Configured = false;
			OnConfigurationReset(null);
		}

		/// <summary>
		/// Log the logEvent through this repository.
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
		public abstract void Log(LoggingEvent logEvent);

		/// <summary>
		/// Returns all the Appenders that are configured as an Array.
		/// </summary>
		/// <returns>All the Appenders</returns>
		/// <remarks>
		/// <para>
		/// Returns all the Appenders that are configured as an Array.
		/// </para>
		/// </remarks>
		public abstract IAppender[] GetAppenders();

		private void AddBuiltinLevels()
		{
			m_levelMap.Add(Level.Off);
			m_levelMap.Add(Level.Emergency);
			m_levelMap.Add(Level.Fatal);
			m_levelMap.Add(Level.Alert);
			m_levelMap.Add(Level.Critical);
			m_levelMap.Add(Level.Severe);
			m_levelMap.Add(Level.Error);
			m_levelMap.Add(Level.Warn);
			m_levelMap.Add(Level.Notice);
			m_levelMap.Add(Level.Info);
			m_levelMap.Add(Level.Debug);
			m_levelMap.Add(Level.Fine);
			m_levelMap.Add(Level.Trace);
			m_levelMap.Add(Level.Finer);
			m_levelMap.Add(Level.Verbose);
			m_levelMap.Add(Level.Finest);
			m_levelMap.Add(Level.All);
		}

		/// <summary>
		/// Adds an object renderer for a specific class. 
		/// </summary>
		/// <param name="typeToRender">The type that will be rendered by the renderer supplied.</param>
		/// <param name="rendererInstance">The object renderer used to render the object.</param>
		/// <remarks>
		/// <para>
		/// Adds an object renderer for a specific class. 
		/// </para>
		/// </remarks>
		public virtual void AddRenderer(Type typeToRender, IObjectRenderer rendererInstance)
		{
			if ((object)typeToRender == null)
			{
				throw new ArgumentNullException("typeToRender");
			}
			if (rendererInstance == null)
			{
				throw new ArgumentNullException("rendererInstance");
			}
			m_rendererMap.Put(typeToRender, rendererInstance);
		}

		/// <summary>
		/// Notify the registered listeners that the repository is shutting down
		/// </summary>
		/// <param name="e">Empty EventArgs</param>
		/// <remarks>
		/// <para>
		/// Notify any listeners that this repository is shutting down.
		/// </para>
		/// </remarks>
		protected virtual void OnShutdown(EventArgs e)
		{
			if (e == null)
			{
				e = EventArgs.Empty;
			}
			this.m_shutdownEvent?.Invoke(this, e);
		}

		/// <summary>
		/// Notify the registered listeners that the repository has had its configuration reset
		/// </summary>
		/// <param name="e">Empty EventArgs</param>
		/// <remarks>
		/// <para>
		/// Notify any listeners that this repository's configuration has been reset.
		/// </para>
		/// </remarks>
		protected virtual void OnConfigurationReset(EventArgs e)
		{
			if (e == null)
			{
				e = EventArgs.Empty;
			}
			this.m_configurationResetEvent?.Invoke(this, e);
		}

		/// <summary>
		/// Notify the registered listeners that the repository has had its configuration changed
		/// </summary>
		/// <param name="e">Empty EventArgs</param>
		/// <remarks>
		/// <para>
		/// Notify any listeners that this repository's configuration has changed.
		/// </para>
		/// </remarks>
		protected virtual void OnConfigurationChanged(EventArgs e)
		{
			if (e == null)
			{
				e = EventArgs.Empty;
			}
			this.m_configurationChangedEvent?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raise a configuration changed event on this repository
		/// </summary>
		/// <param name="e">EventArgs.Empty</param>
		/// <remarks>
		/// <para>
		/// Applications that programmatically change the configuration of the repository should
		/// raise this event notification to notify listeners.
		/// </para>
		/// </remarks>
		public void RaiseConfigurationChanged(EventArgs e)
		{
			OnConfigurationChanged(e);
		}
	}
}
