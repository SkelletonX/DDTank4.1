using log4net.Repository;
using System;
using System.Reflection;

namespace log4net.Core
{
	/// <summary>
	/// Interface used by the <see cref="T:log4net.LogManager" /> to select the <see cref="T:log4net.Repository.ILoggerRepository" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <see cref="T:log4net.LogManager" /> uses a <see cref="T:log4net.Core.IRepositorySelector" /> 
	/// to specify the policy for selecting the correct <see cref="T:log4net.Repository.ILoggerRepository" /> 
	/// to return to the caller.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public interface IRepositorySelector
	{
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
		event LoggerRepositoryCreationEventHandler LoggerRepositoryCreatedEvent;

		/// <summary>
		/// Gets the <see cref="T:log4net.Repository.ILoggerRepository" /> for the specified assembly.
		/// </summary>
		/// <param name="assembly">The assembly to use to lookup to the <see cref="T:log4net.Repository.ILoggerRepository" /></param>
		/// <returns>The <see cref="T:log4net.Repository.ILoggerRepository" /> for the assembly.</returns>
		/// <remarks>
		/// <para>
		/// Gets the <see cref="T:log4net.Repository.ILoggerRepository" /> for the specified assembly.
		/// </para>
		/// <para>
		/// How the association between <see cref="T:System.Reflection.Assembly" /> and <see cref="T:log4net.Repository.ILoggerRepository" />
		/// is made is not defined. The implementation may choose any method for
		/// this association. The results of this method must be repeatable, i.e.
		/// when called again with the same arguments the result must be the
		/// save value.
		/// </para>
		/// </remarks>
		ILoggerRepository GetRepository(Assembly assembly);

		/// <summary>
		/// Gets the named <see cref="T:log4net.Repository.ILoggerRepository" />.
		/// </summary>
		/// <param name="repositoryName">The name to use to lookup to the <see cref="T:log4net.Repository.ILoggerRepository" />.</param>
		/// <returns>The named <see cref="T:log4net.Repository.ILoggerRepository" /></returns>
		/// <remarks>
		/// Lookup a named <see cref="T:log4net.Repository.ILoggerRepository" />. This is the repository created by
		/// calling <see cref="M:log4net.Core.IRepositorySelector.CreateRepository(System.String,System.Type)" />.
		/// </remarks>
		ILoggerRepository GetRepository(string repositoryName);

		/// <summary>
		/// Creates a new repository for the assembly specified.
		/// </summary>
		/// <param name="assembly">The assembly to use to create the domain to associate with the <see cref="T:log4net.Repository.ILoggerRepository" />.</param>
		/// <param name="repositoryType">The type of repository to create, must implement <see cref="T:log4net.Repository.ILoggerRepository" />.</param>
		/// <returns>The repository created.</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="T:log4net.Repository.ILoggerRepository" /> created will be associated with the domain
		/// specified such that a call to <see cref="M:log4net.Core.IRepositorySelector.GetRepository(System.Reflection.Assembly)" /> with the
		/// same assembly specified will return the same repository instance.
		/// </para>
		/// <para>
		/// How the association between <see cref="T:System.Reflection.Assembly" /> and <see cref="T:log4net.Repository.ILoggerRepository" />
		/// is made is not defined. The implementation may choose any method for
		/// this association.
		/// </para>
		/// </remarks>
		ILoggerRepository CreateRepository(Assembly assembly, Type repositoryType);

		/// <summary>
		/// Creates a new repository with the name specified.
		/// </summary>
		/// <param name="repositoryName">The name to associate with the <see cref="T:log4net.Repository.ILoggerRepository" />.</param>
		/// <param name="repositoryType">The type of repository to create, must implement <see cref="T:log4net.Repository.ILoggerRepository" />.</param>
		/// <returns>The repository created.</returns>
		/// <remarks>
		/// <para>
		/// The <see cref="T:log4net.Repository.ILoggerRepository" /> created will be associated with the name
		/// specified such that a call to <see cref="M:log4net.Core.IRepositorySelector.GetRepository(System.String)" /> with the
		/// same name will return the same repository instance.
		/// </para>
		/// </remarks>
		ILoggerRepository CreateRepository(string repositoryName, Type repositoryType);

		/// <summary>
		/// Test if a named repository exists
		/// </summary>
		/// <param name="repositoryName">the named repository to check</param>
		/// <returns><c>true</c> if the repository exists</returns>
		/// <remarks>
		/// <para>
		/// Test if a named repository exists. Use <see cref="M:log4net.Core.IRepositorySelector.CreateRepository(System.Reflection.Assembly,System.Type)" />
		/// to create a new repository and <see cref="M:log4net.Core.IRepositorySelector.GetRepository(System.Reflection.Assembly)" /> to retrieve 
		/// a repository.
		/// </para>
		/// </remarks>
		bool ExistsRepository(string repositoryName);

		/// <summary>
		/// Gets an array of all currently defined repositories.
		/// </summary>
		/// <returns>
		/// An array of the <see cref="T:log4net.Repository.ILoggerRepository" /> instances created by 
		/// this <see cref="T:log4net.Core.IRepositorySelector" />.</returns>
		/// <remarks>
		/// <para>
		/// Gets an array of all of the repositories created by this selector.
		/// </para>
		/// </remarks>
		ILoggerRepository[] GetAllRepositories();
	}
}
