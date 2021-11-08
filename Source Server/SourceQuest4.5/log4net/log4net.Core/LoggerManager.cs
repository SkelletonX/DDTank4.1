using log4net.Repository;
using log4net.Repository.Hierarchy;
using log4net.Util;
using System;
using System.Reflection;
using System.Security;
using System.Text;

namespace log4net.Core
{
	/// <summary>
	/// Static manager that controls the creation of repositories
	/// </summary>
	/// <remarks>
	/// <para>
	/// Static manager that controls the creation of repositories
	/// </para>
	/// <para>
	/// This class is used by the wrapper managers (e.g. <see cref="T:log4net.LogManager" />)
	/// to provide access to the <see cref="T:log4net.Core.ILogger" /> objects.
	/// </para>
	/// <para>
	/// This manager also holds the <see cref="T:log4net.Core.IRepositorySelector" /> that is used to
	/// lookup and create repositories. The selector can be set either programmatically using
	/// the <see cref="P:log4net.Core.LoggerManager.RepositorySelector" /> property, or by setting the <c>log4net.RepositorySelector</c>
	/// AppSetting in the applications config file to the fully qualified type name of the
	/// selector to use. 
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public sealed class LoggerManager
	{
		/// <summary>
		/// Initialize the default repository selector
		/// </summary>
		private static IRepositorySelector s_repositorySelector;

		/// <summary>
		/// Gets or sets the repository selector used by the <see cref="T:log4net.LogManager" />.
		/// </summary>
		/// <value>
		/// The repository selector used by the <see cref="T:log4net.LogManager" />.
		/// </value>
		/// <remarks>
		/// <para>
		/// The repository selector (<see cref="T:log4net.Core.IRepositorySelector" />) is used by 
		/// the <see cref="T:log4net.LogManager" /> to create and select repositories 
		/// (<see cref="T:log4net.Repository.ILoggerRepository" />).
		/// </para>
		/// <para>
		/// The caller to <see cref="T:log4net.LogManager" /> supplies either a string name 
		/// or an assembly (if not supplied the assembly is inferred using 
		/// <see cref="M:System.Reflection.Assembly.GetCallingAssembly" />).
		/// </para>
		/// <para>
		/// This context is used by the selector to lookup a specific repository.
		/// </para>
		/// <para>
		/// For the full .NET Framework, the default repository is <c>DefaultRepositorySelector</c>;
		/// for the .NET Compact Framework <c>CompactRepositorySelector</c> is the default
		/// repository.
		/// </para>
		/// </remarks>
		public static IRepositorySelector RepositorySelector
		{
			get
			{
				return s_repositorySelector;
			}
			set
			{
				s_repositorySelector = value;
			}
		}

		/// <summary>
		/// Private constructor to prevent instances. Only static methods should be used.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Private constructor to prevent instances. Only static methods should be used.
		/// </para>
		/// </remarks>
		private LoggerManager()
		{
		}

		/// <summary>
		/// Hook the shutdown event
		/// </summary>
		/// <remarks>
		/// <para>
		/// On the full .NET runtime, the static constructor hooks up the 
		/// <c>AppDomain.ProcessExit</c> and <c>AppDomain.DomainUnload</c>&gt; events. 
		/// These are used to shutdown the log4net system as the application exits.
		/// </para>
		/// </remarks>
		static LoggerManager()
		{
			try
			{
				RegisterAppDomainEvents();
			}
			catch (SecurityException)
			{
				LogLog.Debug("LoggerManager: Security Exception (ControlAppDomain LinkDemand) while trying to register Shutdown handler with the AppDomain. LoggerManager.Shutdown() will not be called automatically when the AppDomain exits. It must be called programmatically.");
			}
			LogLog.Debug(GetVersionInfo());
			string appSetting = SystemInfo.GetAppSetting("log4net.RepositorySelector");
			if (appSetting != null && appSetting.Length > 0)
			{
				Type type = null;
				try
				{
					type = SystemInfo.GetTypeFromString(appSetting, throwOnError: false, ignoreCase: true);
				}
				catch (Exception exception)
				{
					LogLog.Error("LoggerManager: Exception while resolving RepositorySelector Type [" + appSetting + "]", exception);
				}
				if ((object)type != null)
				{
					object obj = null;
					try
					{
						obj = Activator.CreateInstance(type);
					}
					catch (Exception exception)
					{
						LogLog.Error("LoggerManager: Exception while creating RepositorySelector [" + type.FullName + "]", exception);
					}
					if (obj != null && obj is IRepositorySelector)
					{
						s_repositorySelector = (IRepositorySelector)obj;
					}
					else
					{
						LogLog.Error("LoggerManager: RepositorySelector Type [" + type.FullName + "] is not an IRepositorySelector");
					}
				}
			}
			if (s_repositorySelector == null)
			{
				s_repositorySelector = new DefaultRepositorySelector(typeof(Hierarchy));
			}
		}

		/// <summary>
		/// Register for ProcessExit and DomainUnload events on the AppDomain
		/// </summary>
		/// <remarks>
		/// <para>
		/// This needs to be in a separate method because the events make
		/// a LinkDemand for the ControlAppDomain SecurityPermission. Because
		/// this is a LinkDemand it is demanded at JIT time. Therefore we cannot
		/// catch the exception in the method itself, we have to catch it in the
		/// caller.
		/// </para>
		/// </remarks>
		private static void RegisterAppDomainEvents()
		{
			AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
			AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
		}

		/// <summary>
		/// Return the default <see cref="T:log4net.Repository.ILoggerRepository" /> instance.
		/// </summary>
		/// <param name="repository">the repository to lookup in</param>
		/// <returns>Return the default <see cref="T:log4net.Repository.ILoggerRepository" /> instance</returns>
		/// <remarks>
		/// <para>
		/// Gets the <see cref="T:log4net.Repository.ILoggerRepository" /> for the repository specified
		/// by the <paramref name="repository" /> argument.
		/// </para>
		/// </remarks>
		[Obsolete("Use GetRepository instead of GetLoggerRepository")]
		public static ILoggerRepository GetLoggerRepository(string repository)
		{
			return GetRepository(repository);
		}

		/// <summary>
		/// Returns the default <see cref="T:log4net.Repository.ILoggerRepository" /> instance.
		/// </summary>
		/// <param name="repositoryAssembly">The assembly to use to lookup the repository.</param>
		/// <returns>The default <see cref="T:log4net.Repository.ILoggerRepository" /> instance.</returns>
		[Obsolete("Use GetRepository instead of GetLoggerRepository")]
		public static ILoggerRepository GetLoggerRepository(Assembly repositoryAssembly)
		{
			return GetRepository(repositoryAssembly);
		}

		/// <summary>
		/// Return the default <see cref="T:log4net.Repository.ILoggerRepository" /> instance.
		/// </summary>
		/// <param name="repository">the repository to lookup in</param>
		/// <returns>Return the default <see cref="T:log4net.Repository.ILoggerRepository" /> instance</returns>
		/// <remarks>
		/// <para>
		/// Gets the <see cref="T:log4net.Repository.ILoggerRepository" /> for the repository specified
		/// by the <paramref name="repository" /> argument.
		/// </para>
		/// </remarks>
		public static ILoggerRepository GetRepository(string repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			return RepositorySelector.GetRepository(repository);
		}

		/// <summary>
		/// Returns the default <see cref="T:log4net.Repository.ILoggerRepository" /> instance.
		/// </summary>
		/// <param name="repositoryAssembly">The assembly to use to lookup the repository.</param>
		/// <returns>The default <see cref="T:log4net.Repository.ILoggerRepository" /> instance.</returns>
		/// <remarks>
		/// <para>
		/// Returns the default <see cref="T:log4net.Repository.ILoggerRepository" /> instance.
		/// </para>
		/// </remarks>
		public static ILoggerRepository GetRepository(Assembly repositoryAssembly)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			return RepositorySelector.GetRepository(repositoryAssembly);
		}

		/// <summary>
		/// Returns the named logger if it exists.
		/// </summary>
		/// <param name="repository">The repository to lookup in.</param>
		/// <param name="name">The fully qualified logger name to look for.</param>
		/// <returns>
		/// The logger found, or <c>null</c> if the named logger does not exist in the
		/// specified repository.
		/// </returns>
		/// <remarks>
		/// <para>
		/// If the named logger exists (in the specified repository) then it
		/// returns a reference to the logger, otherwise it returns
		/// <c>null</c>.
		/// </para>
		/// </remarks>
		public static ILogger Exists(string repository, string name)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return RepositorySelector.GetRepository(repository).Exists(name);
		}

		/// <summary>
		/// Returns the named logger if it exists.
		/// </summary>
		/// <param name="repositoryAssembly">The assembly to use to lookup the repository.</param>
		/// <param name="name">The fully qualified logger name to look for.</param>
		/// <returns>
		/// The logger found, or <c>null</c> if the named logger does not exist in the
		/// specified assembly's repository.
		/// </returns>
		/// <remarks>
		/// <para>
		/// If the named logger exists (in the specified assembly's repository) then it
		/// returns a reference to the logger, otherwise it returns
		/// <c>null</c>.
		/// </para>
		/// </remarks>
		public static ILogger Exists(Assembly repositoryAssembly, string name)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return RepositorySelector.GetRepository(repositoryAssembly).Exists(name);
		}

		/// <summary>
		/// Returns all the currently defined loggers in the specified repository.
		/// </summary>
		/// <param name="repository">The repository to lookup in.</param>
		/// <returns>All the defined loggers.</returns>
		/// <remarks>
		/// <para>
		/// The root logger is <b>not</b> included in the returned array.
		/// </para>
		/// </remarks>
		public static ILogger[] GetCurrentLoggers(string repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			return RepositorySelector.GetRepository(repository).GetCurrentLoggers();
		}

		/// <summary>
		/// Returns all the currently defined loggers in the specified assembly's repository.
		/// </summary>
		/// <param name="repositoryAssembly">The assembly to use to lookup the repository.</param>
		/// <returns>All the defined loggers.</returns>
		/// <remarks>
		/// <para>
		/// The root logger is <b>not</b> included in the returned array.
		/// </para>
		/// </remarks>
		public static ILogger[] GetCurrentLoggers(Assembly repositoryAssembly)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			return RepositorySelector.GetRepository(repositoryAssembly).GetCurrentLoggers();
		}

		/// <summary>
		/// Retrieves or creates a named logger.
		/// </summary>
		/// <param name="repository">The repository to lookup in.</param>
		/// <param name="name">The name of the logger to retrieve.</param>
		/// <returns>The logger with the name specified.</returns>
		/// <remarks>
		/// <para>
		/// Retrieves a logger named as the <paramref name="name" />
		/// parameter. If the named logger already exists, then the
		/// existing instance will be returned. Otherwise, a new instance is
		/// created.
		/// </para>
		/// <para>
		/// By default, loggers do not have a set level but inherit
		/// it from the hierarchy. This is one of the central features of
		/// log4net.
		/// </para>
		/// </remarks>
		public static ILogger GetLogger(string repository, string name)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return RepositorySelector.GetRepository(repository).GetLogger(name);
		}

		/// <summary>
		/// Retrieves or creates a named logger.
		/// </summary>
		/// <param name="repositoryAssembly">The assembly to use to lookup the repository.</param>
		/// <param name="name">The name of the logger to retrieve.</param>
		/// <returns>The logger with the name specified.</returns>
		/// <remarks>
		/// <para>
		/// Retrieves a logger named as the <paramref name="name" />
		/// parameter. If the named logger already exists, then the
		/// existing instance will be returned. Otherwise, a new instance is
		/// created.
		/// </para>
		/// <para>
		/// By default, loggers do not have a set level but inherit
		/// it from the hierarchy. This is one of the central features of
		/// log4net.
		/// </para>
		/// </remarks>
		public static ILogger GetLogger(Assembly repositoryAssembly, string name)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return RepositorySelector.GetRepository(repositoryAssembly).GetLogger(name);
		}

		/// <summary>
		/// Shorthand for <see cref="M:log4net.LogManager.GetLogger(System.String)" />.
		/// </summary>
		/// <param name="repository">The repository to lookup in.</param>
		/// <param name="type">The <paramref name="type" /> of which the fullname will be used as the name of the logger to retrieve.</param>
		/// <returns>The logger with the name specified.</returns>
		/// <remarks>
		/// <para>
		/// Gets the logger for the fully qualified name of the type specified.
		/// </para>
		/// </remarks>
		public static ILogger GetLogger(string repository, Type type)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			if ((object)type == null)
			{
				throw new ArgumentNullException("type");
			}
			return RepositorySelector.GetRepository(repository).GetLogger(type.FullName);
		}

		/// <summary>
		/// Shorthand for <see cref="M:log4net.LogManager.GetLogger(System.String)" />.
		/// </summary>
		/// <param name="repositoryAssembly">the assembly to use to lookup the repository</param>
		/// <param name="type">The <paramref name="type" /> of which the fullname will be used as the name of the logger to retrieve.</param>
		/// <returns>The logger with the name specified.</returns>
		/// <remarks>
		/// <para>
		/// Gets the logger for the fully qualified name of the type specified.
		/// </para>
		/// </remarks>
		public static ILogger GetLogger(Assembly repositoryAssembly, Type type)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			if ((object)type == null)
			{
				throw new ArgumentNullException("type");
			}
			return RepositorySelector.GetRepository(repositoryAssembly).GetLogger(type.FullName);
		}

		/// <summary>
		/// Shuts down the log4net system.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Calling this method will <b>safely</b> close and remove all
		/// appenders in all the loggers including root contained in all the
		/// default repositories.
		/// </para>
		/// <para>
		/// Some appenders need to be closed before the application exists. 
		/// Otherwise, pending logging events might be lost.
		/// </para>
		/// <para>
		/// The <c>shutdown</c> method is careful to close nested
		/// appenders before closing regular appenders. This is allows
		/// configurations where a regular appender is attached to a logger
		/// and again to a nested appender.
		/// </para>
		/// </remarks>
		public static void Shutdown()
		{
			ILoggerRepository[] allRepositories = GetAllRepositories();
			foreach (ILoggerRepository loggerRepository in allRepositories)
			{
				loggerRepository.Shutdown();
			}
		}

		/// <summary>
		/// Shuts down the repository for the repository specified.
		/// </summary>
		/// <param name="repository">The repository to shutdown.</param>
		/// <remarks>
		/// <para>
		/// Calling this method will <b>safely</b> close and remove all
		/// appenders in all the loggers including root contained in the
		/// repository for the <paramref name="repository" /> specified.
		/// </para>
		/// <para>
		/// Some appenders need to be closed before the application exists. 
		/// Otherwise, pending logging events might be lost.
		/// </para>
		/// <para>
		/// The <c>shutdown</c> method is careful to close nested
		/// appenders before closing regular appenders. This is allows
		/// configurations where a regular appender is attached to a logger
		/// and again to a nested appender.
		/// </para>
		/// </remarks>
		public static void ShutdownRepository(string repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			RepositorySelector.GetRepository(repository).Shutdown();
		}

		/// <summary>
		/// Shuts down the repository for the repository specified.
		/// </summary>
		/// <param name="repositoryAssembly">The assembly to use to lookup the repository.</param>
		/// <remarks>
		/// <para>
		/// Calling this method will <b>safely</b> close and remove all
		/// appenders in all the loggers including root contained in the
		/// repository for the repository. The repository is looked up using
		/// the <paramref name="repositoryAssembly" /> specified.
		/// </para>
		/// <para>
		/// Some appenders need to be closed before the application exists. 
		/// Otherwise, pending logging events might be lost.
		/// </para>
		/// <para>
		/// The <c>shutdown</c> method is careful to close nested
		/// appenders before closing regular appenders. This is allows
		/// configurations where a regular appender is attached to a logger
		/// and again to a nested appender.
		/// </para>
		/// </remarks>
		public static void ShutdownRepository(Assembly repositoryAssembly)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			RepositorySelector.GetRepository(repositoryAssembly).Shutdown();
		}

		/// <summary>
		/// Resets all values contained in this repository instance to their defaults.
		/// </summary>
		/// <param name="repository">The repository to reset.</param>
		/// <remarks>
		/// <para>
		/// Resets all values contained in the repository instance to their
		/// defaults.  This removes all appenders from all loggers, sets
		/// the level of all non-root loggers to <c>null</c>,
		/// sets their additivity flag to <c>true</c> and sets the level
		/// of the root logger to <see cref="F:log4net.Core.Level.Debug" />. Moreover,
		/// message disabling is set its default "off" value.
		/// </para>		
		/// </remarks>
		public static void ResetConfiguration(string repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			RepositorySelector.GetRepository(repository).ResetConfiguration();
		}

		/// <summary>
		/// Resets all values contained in this repository instance to their defaults.
		/// </summary>
		/// <param name="repositoryAssembly">The assembly to use to lookup the repository to reset.</param>
		/// <remarks>
		/// <para>
		/// Resets all values contained in the repository instance to their
		/// defaults.  This removes all appenders from all loggers, sets
		/// the level of all non-root loggers to <c>null</c>,
		/// sets their additivity flag to <c>true</c> and sets the level
		/// of the root logger to <see cref="F:log4net.Core.Level.Debug" />. Moreover,
		/// message disabling is set its default "off" value.
		/// </para>		
		/// </remarks>
		public static void ResetConfiguration(Assembly repositoryAssembly)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			RepositorySelector.GetRepository(repositoryAssembly).ResetConfiguration();
		}

		/// <summary>
		/// Creates a repository with the specified name.
		/// </summary>
		/// <param name="repository">The name of the repository, this must be unique amongst repositories.</param>
		/// <returns>The <see cref="T:log4net.Repository.ILoggerRepository" /> created for the repository.</returns>
		/// <remarks>
		/// <para>
		/// <b>CreateDomain is obsolete. Use CreateRepository instead of CreateDomain.</b>
		/// </para>
		/// <para>
		/// Creates the default type of <see cref="T:log4net.Repository.ILoggerRepository" /> which is a
		/// <see cref="T:log4net.Repository.Hierarchy.Hierarchy" /> object.
		/// </para>
		/// <para>
		/// The <paramref name="repository" /> name must be unique. Repositories cannot be redefined.
		/// An <see cref="T:System.Exception" /> will be thrown if the repository already exists.
		/// </para>
		/// </remarks>
		/// <exception cref="T:log4net.Core.LogException">The specified repository already exists.</exception>
		[Obsolete("Use CreateRepository instead of CreateDomain")]
		public static ILoggerRepository CreateDomain(string repository)
		{
			return CreateRepository(repository);
		}

		/// <summary>
		/// Creates a repository with the specified name.
		/// </summary>
		/// <param name="repository">The name of the repository, this must be unique amongst repositories.</param>
		/// <returns>The <see cref="T:log4net.Repository.ILoggerRepository" /> created for the repository.</returns>
		/// <remarks>
		/// <para>
		/// Creates the default type of <see cref="T:log4net.Repository.ILoggerRepository" /> which is a
		/// <see cref="T:log4net.Repository.Hierarchy.Hierarchy" /> object.
		/// </para>
		/// <para>
		/// The <paramref name="repository" /> name must be unique. Repositories cannot be redefined.
		/// An <see cref="T:System.Exception" /> will be thrown if the repository already exists.
		/// </para>
		/// </remarks>
		/// <exception cref="T:log4net.Core.LogException">The specified repository already exists.</exception>
		public static ILoggerRepository CreateRepository(string repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			return RepositorySelector.CreateRepository(repository, null);
		}

		/// <summary>
		/// Creates a repository with the specified name and repository type.
		/// </summary>
		/// <param name="repository">The name of the repository, this must be unique to the repository.</param>
		/// <param name="repositoryType">A <see cref="T:System.Type" /> that implements <see cref="T:log4net.Repository.ILoggerRepository" />
		/// and has a no arg constructor. An instance of this type will be created to act
		/// as the <see cref="T:log4net.Repository.ILoggerRepository" /> for the repository specified.</param>
		/// <returns>The <see cref="T:log4net.Repository.ILoggerRepository" /> created for the repository.</returns>
		/// <remarks>
		/// <para>
		/// <b>CreateDomain is obsolete. Use CreateRepository instead of CreateDomain.</b>
		/// </para>
		/// <para>
		/// The <paramref name="repository" /> name must be unique. Repositories cannot be redefined.
		/// An Exception will be thrown if the repository already exists.
		/// </para>
		/// </remarks>
		/// <exception cref="T:log4net.Core.LogException">The specified repository already exists.</exception>
		[Obsolete("Use CreateRepository instead of CreateDomain")]
		public static ILoggerRepository CreateDomain(string repository, Type repositoryType)
		{
			return CreateRepository(repository, repositoryType);
		}

		/// <summary>
		/// Creates a repository with the specified name and repository type.
		/// </summary>
		/// <param name="repository">The name of the repository, this must be unique to the repository.</param>
		/// <param name="repositoryType">A <see cref="T:System.Type" /> that implements <see cref="T:log4net.Repository.ILoggerRepository" />
		/// and has a no arg constructor. An instance of this type will be created to act
		/// as the <see cref="T:log4net.Repository.ILoggerRepository" /> for the repository specified.</param>
		/// <returns>The <see cref="T:log4net.Repository.ILoggerRepository" /> created for the repository.</returns>
		/// <remarks>
		/// <para>
		/// The <paramref name="repository" /> name must be unique. Repositories cannot be redefined.
		/// An Exception will be thrown if the repository already exists.
		/// </para>
		/// </remarks>
		/// <exception cref="T:log4net.Core.LogException">The specified repository already exists.</exception>
		public static ILoggerRepository CreateRepository(string repository, Type repositoryType)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			if ((object)repositoryType == null)
			{
				throw new ArgumentNullException("repositoryType");
			}
			return RepositorySelector.CreateRepository(repository, repositoryType);
		}

		/// <summary>
		/// Creates a repository for the specified assembly and repository type.
		/// </summary>
		/// <param name="repositoryAssembly">The assembly to use to get the name of the repository.</param>
		/// <param name="repositoryType">A <see cref="T:System.Type" /> that implements <see cref="T:log4net.Repository.ILoggerRepository" />
		/// and has a no arg constructor. An instance of this type will be created to act
		/// as the <see cref="T:log4net.Repository.ILoggerRepository" /> for the repository specified.</param>
		/// <returns>The <see cref="T:log4net.Repository.ILoggerRepository" /> created for the repository.</returns>
		/// <remarks>
		/// <para>
		/// <b>CreateDomain is obsolete. Use CreateRepository instead of CreateDomain.</b>
		/// </para>
		/// <para>
		/// The <see cref="T:log4net.Repository.ILoggerRepository" /> created will be associated with the repository
		/// specified such that a call to <see cref="M:log4net.Core.LoggerManager.GetRepository(System.Reflection.Assembly)" /> with the
		/// same assembly specified will return the same repository instance.
		/// </para>
		/// </remarks>
		[Obsolete("Use CreateRepository instead of CreateDomain")]
		public static ILoggerRepository CreateDomain(Assembly repositoryAssembly, Type repositoryType)
		{
			return CreateRepository(repositoryAssembly, repositoryType);
		}

		/// <summary>
		/// Creates a repository for the specified assembly and repository type.
		/// </summary>
		/// <param name="repositoryAssembly">The assembly to use to get the name of the repository.</param>
		/// <param name="repositoryType">A <see cref="T:System.Type" /> that implements <see cref="T:log4net.Repository.ILoggerRepository" />
		/// and has a no arg constructor. An instance of this type will be created to act
		/// as the <see cref="T:log4net.Repository.ILoggerRepository" /> for the repository specified.</param>
		/// <returns>The <see cref="T:log4net.Repository.ILoggerRepository" /> created for the repository.</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="T:log4net.Repository.ILoggerRepository" /> created will be associated with the repository
		/// specified such that a call to <see cref="M:log4net.Core.LoggerManager.GetRepository(System.Reflection.Assembly)" /> with the
		/// same assembly specified will return the same repository instance.
		/// </para>
		/// </remarks>
		public static ILoggerRepository CreateRepository(Assembly repositoryAssembly, Type repositoryType)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			if ((object)repositoryType == null)
			{
				throw new ArgumentNullException("repositoryType");
			}
			return RepositorySelector.CreateRepository(repositoryAssembly, repositoryType);
		}

		/// <summary>
		/// Gets an array of all currently defined repositories.
		/// </summary>
		/// <returns>An array of all the known <see cref="T:log4net.Repository.ILoggerRepository" /> objects.</returns>
		/// <remarks>
		/// <para>
		/// Gets an array of all currently defined repositories.
		/// </para>
		/// </remarks>
		public static ILoggerRepository[] GetAllRepositories()
		{
			return RepositorySelector.GetAllRepositories();
		}

		/// <summary>
		/// Internal method to get pertinent version info.
		/// </summary>
		/// <returns>A string of version info.</returns>
		private static string GetVersionInfo()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			stringBuilder.Append("log4net assembly [").Append(executingAssembly.FullName).Append("]. ");
			stringBuilder.Append("Loaded from [").Append(SystemInfo.AssemblyLocationInfo(executingAssembly)).Append("]. ");
			stringBuilder.Append("(.NET Runtime [").Append(Environment.Version.ToString()).Append("]");
			stringBuilder.Append(" on ").Append(Environment.OSVersion.ToString());
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		/// <summary>
		/// Called when the <see cref="E:System.AppDomain.DomainUnload" /> event fires
		/// </summary>
		/// <param name="sender">the <see cref="T:System.AppDomain" /> that is exiting</param>
		/// <param name="e">null</param>
		/// <remarks>
		/// <para>
		/// Called when the <see cref="E:System.AppDomain.DomainUnload" /> event fires.
		/// </para>
		/// <para>
		/// When the event is triggered the log4net system is <see cref="M:log4net.Core.LoggerManager.Shutdown" />.
		/// </para>
		/// </remarks>
		private static void OnDomainUnload(object sender, EventArgs e)
		{
			Shutdown();
		}

		/// <summary>
		/// Called when the <see cref="E:System.AppDomain.ProcessExit" /> event fires
		/// </summary>
		/// <param name="sender">the <see cref="T:System.AppDomain" /> that is exiting</param>
		/// <param name="e">null</param>
		/// <remarks>
		/// <para>
		/// Called when the <see cref="E:System.AppDomain.ProcessExit" /> event fires.
		/// </para>
		/// <para>
		/// When the event is triggered the log4net system is <see cref="M:log4net.Core.LoggerManager.Shutdown" />.
		/// </para>
		/// </remarks>
		private static void OnProcessExit(object sender, EventArgs e)
		{
			Shutdown();
		}
	}
}
