using log4net.Repository;
using System;
using System.Collections;

namespace log4net.Core
{
	/// <summary>
	/// Maps between logger objects and wrapper objects.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class maintains a mapping between <see cref="T:log4net.Core.ILogger" /> objects and
	/// <see cref="T:log4net.Core.ILoggerWrapper" /> objects. Use the <see cref="M:log4net.Core.WrapperMap.GetWrapper(log4net.Core.ILogger)" /> method to 
	/// lookup the <see cref="T:log4net.Core.ILoggerWrapper" /> for the specified <see cref="T:log4net.Core.ILogger" />.
	/// </para>
	/// <para>
	/// New wrapper instances are created by the <see cref="M:log4net.Core.WrapperMap.CreateNewWrapperObject(log4net.Core.ILogger)" />
	/// method. The default behavior is for this method to delegate construction
	/// of the wrapper to the <see cref="T:log4net.Core.WrapperCreationHandler" /> delegate supplied
	/// to the constructor. This allows specialization of the behavior without
	/// requiring subclassing of this type.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class WrapperMap
	{
		/// <summary>
		/// Map of logger repositories to hashtables of ILogger to ILoggerWrapper mappings
		/// </summary>
		private readonly Hashtable m_repositories = new Hashtable();

		/// <summary>
		/// The handler to use to create the extension wrapper objects.
		/// </summary>
		private readonly WrapperCreationHandler m_createWrapperHandler;

		/// <summary>
		/// Internal reference to the delegate used to register for repository shutdown events.
		/// </summary>
		private readonly LoggerRepositoryShutdownEventHandler m_shutdownHandler;

		/// <summary>
		/// Gets the map of logger repositories.
		/// </summary>
		/// <value>
		/// Map of logger repositories.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets the hashtable that is keyed on <see cref="T:log4net.Repository.ILoggerRepository" />. The
		/// values are hashtables keyed on <see cref="T:log4net.Core.ILogger" /> with the
		/// value being the corresponding <see cref="T:log4net.Core.ILoggerWrapper" />.
		/// </para>
		/// </remarks>
		protected Hashtable Repositories => m_repositories;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Core.WrapperMap" />
		/// </summary>
		/// <param name="createWrapperHandler">The handler to use to create the wrapper objects.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Core.WrapperMap" /> class with 
		/// the specified handler to create the wrapper objects.
		/// </para>
		/// </remarks>
		public WrapperMap(WrapperCreationHandler createWrapperHandler)
		{
			m_createWrapperHandler = createWrapperHandler;
			m_shutdownHandler = ILoggerRepository_Shutdown;
		}

		/// <summary>
		/// Gets the wrapper object for the specified logger.
		/// </summary>
		/// <returns>The wrapper object for the specified logger</returns>
		/// <remarks>
		/// <para>
		/// If the logger is null then the corresponding wrapper is null.
		/// </para>
		/// <para>
		/// Looks up the wrapper it it has previously been requested and
		/// returns it. If the wrapper has never been requested before then
		/// the <see cref="M:log4net.Core.WrapperMap.CreateNewWrapperObject(log4net.Core.ILogger)" /> virtual method is
		/// called.
		/// </para>
		/// </remarks>
		public virtual ILoggerWrapper GetWrapper(ILogger logger)
		{
			if (logger == null)
			{
				return null;
			}
			lock (this)
			{
				Hashtable hashtable = (Hashtable)m_repositories[logger.Repository];
				if (hashtable == null)
				{
					hashtable = new Hashtable();
					m_repositories[logger.Repository] = hashtable;
					logger.Repository.ShutdownEvent += m_shutdownHandler;
				}
				ILoggerWrapper loggerWrapper = hashtable[logger] as ILoggerWrapper;
				if (loggerWrapper == null)
				{
					loggerWrapper = (ILoggerWrapper)(hashtable[logger] = CreateNewWrapperObject(logger));
				}
				return loggerWrapper;
			}
		}

		/// <summary>
		/// Creates the wrapper object for the specified logger.
		/// </summary>
		/// <param name="logger">The logger to wrap in a wrapper.</param>
		/// <returns>The wrapper object for the logger.</returns>
		/// <remarks>
		/// <para>
		/// This implementation uses the <see cref="T:log4net.Core.WrapperCreationHandler" />
		/// passed to the constructor to create the wrapper. This method
		/// can be overridden in a subclass.
		/// </para>
		/// </remarks>
		protected virtual ILoggerWrapper CreateNewWrapperObject(ILogger logger)
		{
			if (m_createWrapperHandler != null)
			{
				return m_createWrapperHandler(logger);
			}
			return null;
		}

		/// <summary>
		/// Called when a monitored repository shutdown event is received.
		/// </summary>
		/// <param name="repository">The <see cref="T:log4net.Repository.ILoggerRepository" /> that is shutting down</param>
		/// <remarks>
		/// <para>
		/// This method is called when a <see cref="T:log4net.Repository.ILoggerRepository" /> that this
		/// <see cref="T:log4net.Core.WrapperMap" /> is holding loggers for has signaled its shutdown
		/// event <see cref="E:log4net.Repository.ILoggerRepository.ShutdownEvent" />. The default
		/// behavior of this method is to release the references to the loggers
		/// and their wrappers generated for this repository.
		/// </para>
		/// </remarks>
		protected virtual void RepositoryShutdown(ILoggerRepository repository)
		{
			lock (this)
			{
				m_repositories.Remove(repository);
				repository.ShutdownEvent -= m_shutdownHandler;
			}
		}

		/// <summary>
		/// Event handler for repository shutdown event.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event args.</param>
		private void ILoggerRepository_Shutdown(object sender, EventArgs e)
		{
			ILoggerRepository loggerRepository = sender as ILoggerRepository;
			if (loggerRepository != null)
			{
				RepositoryShutdown(loggerRepository);
			}
		}
	}
}
