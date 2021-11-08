using log4net.Repository;
using System;

namespace log4net.Core
{
	/// <summary>
	/// Provides data for the <see cref="E:log4net.Core.IRepositorySelector.LoggerRepositoryCreatedEvent" /> event.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A <see cref="E:log4net.Core.IRepositorySelector.LoggerRepositoryCreatedEvent" /> 
	/// event is raised every time a <see cref="T:log4net.Repository.ILoggerRepository" /> is created.
	/// </para>
	/// </remarks>
	public class LoggerRepositoryCreationEventArgs : EventArgs
	{
		/// <summary>
		/// The <see cref="T:log4net.Repository.ILoggerRepository" /> created
		/// </summary>
		private ILoggerRepository m_repository;

		/// <summary>
		/// The <see cref="T:log4net.Repository.ILoggerRepository" /> that has been created
		/// </summary>
		/// <value>
		/// The <see cref="T:log4net.Repository.ILoggerRepository" /> that has been created
		/// </value>
		/// <remarks>
		/// <para>
		/// The <see cref="T:log4net.Repository.ILoggerRepository" /> that has been created
		/// </para>
		/// </remarks>
		public ILoggerRepository LoggerRepository => m_repository;

		/// <summary>
		/// Construct instance using <see cref="T:log4net.Repository.ILoggerRepository" /> specified
		/// </summary>
		/// <param name="repository">the <see cref="T:log4net.Repository.ILoggerRepository" /> that has been created</param>
		/// <remarks>
		/// <para>
		/// Construct instance using <see cref="T:log4net.Repository.ILoggerRepository" /> specified
		/// </para>
		/// </remarks>
		public LoggerRepositoryCreationEventArgs(ILoggerRepository repository)
		{
			m_repository = repository;
		}
	}
}
