using log4net.Appender;
using log4net.Core;
using log4net.Repository;
using log4net.Util;
using System;
using System.Runtime.Remoting;

namespace log4net.Plugin
{
	/// <summary>
	/// Plugin that listens for events from the <see cref="T:log4net.Appender.RemotingAppender" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// This plugin publishes an instance of <see cref="T:log4net.Appender.RemotingAppender.IRemoteLoggingSink" /> 
	/// on a specified <see cref="P:log4net.Plugin.RemoteLoggingServerPlugin.SinkUri" />. This listens for logging events delivered from
	/// a remote <see cref="T:log4net.Appender.RemotingAppender" />.
	/// </para>
	/// <para>
	/// When an event is received it is relogged within the attached repository
	/// as if it had been raised locally.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class RemoteLoggingServerPlugin : PluginSkeleton
	{
		/// <summary>
		/// Delivers <see cref="T:log4net.Core.LoggingEvent" /> objects to a remote sink.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Internal class used to listen for logging events
		/// and deliver them to the local repository.
		/// </para>
		/// </remarks>
		private class RemoteLoggingSinkImpl : MarshalByRefObject, RemotingAppender.IRemoteLoggingSink
		{
			/// <summary>
			/// The underlying <see cref="T:log4net.Repository.ILoggerRepository" /> that events should
			/// be logged to.
			/// </summary>
			private readonly ILoggerRepository m_repository;

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="repository">The repository to log to.</param>
			/// <remarks>
			/// <para>
			/// Initializes a new instance of the <see cref="T:log4net.Plugin.RemoteLoggingServerPlugin.RemoteLoggingSinkImpl" /> for the
			/// specified <see cref="T:log4net.Repository.ILoggerRepository" />.
			/// </para>
			/// </remarks>
			public RemoteLoggingSinkImpl(ILoggerRepository repository)
			{
				m_repository = repository;
			}

			/// <summary>
			/// Logs the events to the repository.
			/// </summary>
			/// <param name="events">The events to log.</param>
			/// <remarks>
			/// <para>
			/// The events passed are logged to the <see cref="T:log4net.Repository.ILoggerRepository" />
			/// </para>
			/// </remarks>
			public void LogEvents(LoggingEvent[] events)
			{
				if (events == null)
				{
					return;
				}
				foreach (LoggingEvent loggingEvent in events)
				{
					if (loggingEvent != null)
					{
						m_repository.Log(loggingEvent);
					}
				}
			}

			/// <summary>
			/// Obtains a lifetime service object to control the lifetime 
			/// policy for this instance.
			/// </summary>
			/// <returns><c>null</c> to indicate that this instance should live forever.</returns>
			/// <remarks>
			/// <para>
			/// Obtains a lifetime service object to control the lifetime 
			/// policy for this instance. This object should live forever
			/// therefore this implementation returns <c>null</c>.
			/// </para>
			/// </remarks>
			public override object InitializeLifetimeService()
			{
				return null;
			}
		}

		private RemoteLoggingSinkImpl m_sink;

		private string m_sinkUri;

		/// <summary>
		/// Gets or sets the URI of this sink.
		/// </summary>
		/// <value>
		/// The URI of this sink.
		/// </value>
		/// <remarks>
		/// <para>
		/// This is the name under which the object is marshaled.
		/// <see cref="M:System.Runtime.Remoting.RemotingServices.Marshal(System.MarshalByRefObject,System.String,System.Type)" />
		/// </para>
		/// </remarks>
		public virtual string SinkUri
		{
			get
			{
				return m_sinkUri;
			}
			set
			{
				m_sinkUri = value;
			}
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Plugin.RemoteLoggingServerPlugin" /> class.
		/// </para>
		/// <para>
		/// The <see cref="P:log4net.Plugin.RemoteLoggingServerPlugin.SinkUri" /> property must be set.
		/// </para>
		/// </remarks>
		public RemoteLoggingServerPlugin()
			: base("RemoteLoggingServerPlugin:Unset URI")
		{
		}

		/// <summary>
		/// Construct with sink Uri.
		/// </summary>
		/// <param name="sinkUri">The name to publish the sink under in the remoting infrastructure. 
		/// See <see cref="P:log4net.Plugin.RemoteLoggingServerPlugin.SinkUri" /> for more details.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Plugin.RemoteLoggingServerPlugin" /> class
		/// with specified name.
		/// </para>
		/// </remarks>
		public RemoteLoggingServerPlugin(string sinkUri)
			: base("RemoteLoggingServerPlugin:" + sinkUri)
		{
			m_sinkUri = sinkUri;
		}

		/// <summary>
		/// Attaches this plugin to a <see cref="T:log4net.Repository.ILoggerRepository" />.
		/// </summary>
		/// <param name="repository">The <see cref="T:log4net.Repository.ILoggerRepository" /> that this plugin should be attached to.</param>
		/// <remarks>
		/// <para>
		/// A plugin may only be attached to a single repository.
		/// </para>
		/// <para>
		/// This method is called when the plugin is attached to the repository.
		/// </para>
		/// </remarks>
		public override void Attach(ILoggerRepository repository)
		{
			base.Attach(repository);
			m_sink = new RemoteLoggingSinkImpl(repository);
			try
			{
				RemotingServices.Marshal(m_sink, m_sinkUri, typeof(RemotingAppender.IRemoteLoggingSink));
			}
			catch (Exception exception)
			{
				LogLog.Error("RemoteLoggingServerPlugin: Failed to Marshal remoting sink", exception);
			}
		}

		/// <summary>
		/// Is called when the plugin is to shutdown.
		/// </summary>
		/// <remarks>
		/// <para>
		/// When the plugin is shutdown the remote logging
		/// sink is disconnected.
		/// </para>
		/// </remarks>
		public override void Shutdown()
		{
			RemotingServices.Disconnect(m_sink);
			m_sink = null;
			base.Shutdown();
		}
	}
}
