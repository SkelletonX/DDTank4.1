using log4net.Repository;
using log4net.Util;
using System;
using System.Collections;
using System.Reflection;

namespace log4net.Core
{
	/// <summary>
	/// The implementation of the <see cref="T:log4net.Core.IRepositorySelector" /> interface suitable
	/// for use with the compact framework
	/// </summary>
	/// <remarks>
	/// <para>
	/// This <see cref="T:log4net.Core.IRepositorySelector" /> implementation is a simple
	/// mapping between repository name and <see cref="T:log4net.Repository.ILoggerRepository" />
	/// object.
	/// </para>
	/// <para>
	/// The .NET Compact Framework 1.0 does not support retrieving assembly
	/// level attributes therefore unlike the <c>DefaultRepositorySelector</c>
	/// this selector does not examine the calling assembly for attributes.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public class CompactRepositorySelector : IRepositorySelector
	{
		private const string DefaultRepositoryName = "log4net-default-repository";

		private readonly Hashtable m_name2repositoryMap = new Hashtable();

		private readonly Type m_defaultRepositoryType;

		private event LoggerRepositoryCreationEventHandler m_loggerRepositoryCreatedEvent;

		/// <summary>
		/// Event to notify that a logger repository has been created.
		/// </summary>
		/// <value>
		/// Event to notify that a logger repository has been created.
		/// </value>
		/// <remarks>
		/// <para>
		/// Event raised when a new repository is created.
		/// The event source will be this selector. The event args will
		/// be a <see cref="T:log4net.Core.LoggerRepositoryCreationEventArgs" /> which
		/// holds the newly created <see cref="T:log4net.Repository.ILoggerRepository" />.
		/// </para>
		/// </remarks>
		public event LoggerRepositoryCreationEventHandler LoggerRepositoryCreatedEvent
		{
			add
			{
				this.m_loggerRepositoryCreatedEvent = (LoggerRepositoryCreationEventHandler)Delegate.Combine(this.m_loggerRepositoryCreatedEvent, value);
			}
			remove
			{
				this.m_loggerRepositoryCreatedEvent = (LoggerRepositoryCreationEventHandler)Delegate.Remove(this.m_loggerRepositoryCreatedEvent, value);
			}
		}

		/// <summary>
		/// Create a new repository selector
		/// </summary>
		/// <param name="defaultRepositoryType">the type of the repositories to create, must implement <see cref="T:log4net.Repository.ILoggerRepository" /></param>
		/// <remarks>
		/// <para>
		/// Create an new compact repository selector.
		/// The default type for repositories must be specified,
		/// an appropriate value would be <see cref="T:log4net.Repository.Hierarchy.Hierarchy" />.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">throw if <paramref name="defaultRepositoryType" /> is null</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">throw if <paramref name="defaultRepositoryType" /> does not implement <see cref="T:log4net.Repository.ILoggerRepository" /></exception>
		public CompactRepositorySelector(Type defaultRepositoryType)
		{
			if ((object)defaultRepositoryType == null)
			{
				throw new ArgumentNullException("defaultRepositoryType");
			}
			if (!typeof(ILoggerRepository).IsAssignableFrom(defaultRepositoryType))
			{
				throw SystemInfo.CreateArgumentOutOfRangeException("defaultRepositoryType", defaultRepositoryType, string.Concat("Parameter: defaultRepositoryType, Value: [", defaultRepositoryType, "] out of range. Argument must implement the ILoggerRepository interface"));
			}
			m_defaultRepositoryType = defaultRepositoryType;
			LogLog.Debug(string.Concat("CompactRepositorySelector: defaultRepositoryType [", m_defaultRepositoryType, "]"));
		}

		/// <summary>
		/// Get the <see cref="T:log4net.Repository.ILoggerRepository" /> for the specified assembly
		/// </summary>
		/// <param name="assembly">not used</param>
		/// <returns>The default <see cref="T:log4net.Repository.ILoggerRepository" /></returns>
		/// <remarks>
		/// <para>
		/// The <paramref name="assembly" /> argument is not used. This selector does not create a
		/// separate repository for each assembly. 
		/// </para>
		/// <para>
		/// As a named repository is not specified the default repository is 
		/// returned. The default repository is named <c>log4net-default-repository</c>.
		/// </para>
		/// </remarks>
		public ILoggerRepository GetRepository(Assembly assembly)
		{
			return CreateRepository(assembly, m_defaultRepositoryType);
		}

		/// <summary>
		/// Get the named <see cref="T:log4net.Repository.ILoggerRepository" />
		/// </summary>
		/// <param name="repositoryName">the name of the repository to lookup</param>
		/// <returns>The named <see cref="T:log4net.Repository.ILoggerRepository" /></returns>
		/// <remarks>
		/// <para>
		/// Get the named <see cref="T:log4net.Repository.ILoggerRepository" />. The default 
		/// repository is <c>log4net-default-repository</c>. Other repositories 
		/// must be created using the <see cref="M:log4net.Core.CompactRepositorySelector.CreateRepository(System.String,System.Type)" />.
		/// If the named repository does not exist an exception is thrown.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">throw if <paramref name="repositoryName" /> is null</exception>
		/// <exception cref="T:log4net.Core.LogException">throw if the <paramref name="repositoryName" /> does not exist</exception>
		public ILoggerRepository GetRepository(string repositoryName)
		{
			if (repositoryName == null)
			{
				throw new ArgumentNullException("repositoryName");
			}
			lock (this)
			{
				ILoggerRepository loggerRepository = m_name2repositoryMap[repositoryName] as ILoggerRepository;
				if (loggerRepository == null)
				{
					throw new LogException("Repository [" + repositoryName + "] is NOT defined.");
				}
				return loggerRepository;
			}
		}

		/// <summary>
		/// Create a new repository for the assembly specified 
		/// </summary>
		/// <param name="assembly">not used</param>
		/// <param name="repositoryType">the type of repository to create, must implement <see cref="T:log4net.Repository.ILoggerRepository" /></param>
		/// <returns>the repository created</returns>
		/// <remarks>
		/// <para>
		/// The <paramref name="assembly" /> argument is not used. This selector does not create a
		/// separate repository for each assembly. 
		/// </para>
		/// <para>
		/// If the <paramref name="repositoryType" /> is <c>null</c> then the
		/// default repository type specified to the constructor is used.
		/// </para>
		/// <para>
		/// As a named repository is not specified the default repository is 
		/// returned. The default repository is named <c>log4net-default-repository</c>.
		/// </para>
		/// </remarks>
		public ILoggerRepository CreateRepository(Assembly assembly, Type repositoryType)
		{
			if ((object)repositoryType == null)
			{
				repositoryType = m_defaultRepositoryType;
			}
			lock (this)
			{
				ILoggerRepository loggerRepository = m_name2repositoryMap["log4net-default-repository"] as ILoggerRepository;
				if (loggerRepository == null)
				{
					loggerRepository = CreateRepository("log4net-default-repository", repositoryType);
				}
				return loggerRepository;
			}
		}

		/// <summary>
		/// Create a new repository for the repository specified
		/// </summary>
		/// <param name="repositoryName">the repository to associate with the <see cref="T:log4net.Repository.ILoggerRepository" /></param>
		/// <param name="repositoryType">the type of repository to create, must implement <see cref="T:log4net.Repository.ILoggerRepository" />.
		/// If this param is null then the default repository type is used.</param>
		/// <returns>the repository created</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="T:log4net.Repository.ILoggerRepository" /> created will be associated with the repository
		/// specified such that a call to <see cref="M:log4net.Core.CompactRepositorySelector.GetRepository(System.String)" /> with the
		/// same repository specified will return the same repository instance.
		/// </para>
		/// <para>
		/// If the named repository already exists an exception will be thrown.
		/// </para>
		/// <para>
		/// If <paramref name="repositoryType" /> is <c>null</c> then the default 
		/// repository type specified to the constructor is used.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">throw if <paramref name="repositoryName" /> is null</exception>
		/// <exception cref="T:log4net.Core.LogException">throw if the <paramref name="repositoryName" /> already exists</exception>
		public ILoggerRepository CreateRepository(string repositoryName, Type repositoryType)
		{
			if (repositoryName == null)
			{
				throw new ArgumentNullException("repositoryName");
			}
			if ((object)repositoryType == null)
			{
				repositoryType = m_defaultRepositoryType;
			}
			lock (this)
			{
				ILoggerRepository loggerRepository = null;
				loggerRepository = (m_name2repositoryMap[repositoryName] as ILoggerRepository);
				if (loggerRepository != null)
				{
					throw new LogException("Repository [" + repositoryName + "] is already defined. Repositories cannot be redefined.");
				}
				LogLog.Debug(string.Concat("DefaultRepositorySelector: Creating repository [", repositoryName, "] using type [", repositoryType, "]"));
				loggerRepository = (ILoggerRepository)Activator.CreateInstance(repositoryType);
				loggerRepository.Name = repositoryName;
				m_name2repositoryMap[repositoryName] = loggerRepository;
				OnLoggerRepositoryCreatedEvent(loggerRepository);
				return loggerRepository;
			}
		}

		/// <summary>
		/// Test if a named repository exists
		/// </summary>
		/// <param name="repositoryName">the named repository to check</param>
		/// <returns><c>true</c> if the repository exists</returns>
		/// <remarks>
		/// <para>
		/// Test if a named repository exists. Use <see cref="M:log4net.Core.CompactRepositorySelector.CreateRepository(System.String,System.Type)" />
		/// to create a new repository and <see cref="M:log4net.Core.CompactRepositorySelector.GetRepository(System.String)" /> to retrieve 
		/// a repository.
		/// </para>
		/// </remarks>
		public bool ExistsRepository(string repositoryName)
		{
			lock (this)
			{
				return m_name2repositoryMap.ContainsKey(repositoryName);
			}
		}

		/// <summary>
		/// Gets a list of <see cref="T:log4net.Repository.ILoggerRepository" /> objects
		/// </summary>
		/// <returns>an array of all known <see cref="T:log4net.Repository.ILoggerRepository" /> objects</returns>
		/// <remarks>
		/// <para>
		/// Gets an array of all of the repositories created by this selector.
		/// </para>
		/// </remarks>
		public ILoggerRepository[] GetAllRepositories()
		{
			lock (this)
			{
				ICollection values = m_name2repositoryMap.Values;
				ILoggerRepository[] array = new ILoggerRepository[values.Count];
				values.CopyTo(array, 0);
				return array;
			}
		}

		/// <summary>
		/// Notify the registered listeners that the repository has been created
		/// </summary>
		/// <param name="repository">The repository that has been created</param>
		/// <remarks>
		/// <para>
		/// Raises the <event cref="E:log4net.Core.CompactRepositorySelector.LoggerRepositoryCreatedEvent">LoggerRepositoryCreatedEvent</event>
		/// event.
		/// </para>
		/// </remarks>
		protected virtual void OnLoggerRepositoryCreatedEvent(ILoggerRepository repository)
		{
			this.m_loggerRepositoryCreatedEvent?.Invoke(this, new LoggerRepositoryCreationEventArgs(repository));
		}
	}
}
