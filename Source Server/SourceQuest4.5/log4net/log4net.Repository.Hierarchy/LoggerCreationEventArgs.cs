using System;

namespace log4net.Repository.Hierarchy
{
	/// <summary>
	/// Provides data for the <see cref="E:log4net.Repository.Hierarchy.Hierarchy.LoggerCreatedEvent" /> event.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A <see cref="E:log4net.Repository.Hierarchy.Hierarchy.LoggerCreatedEvent" /> event is raised every time a
	/// <see cref="P:log4net.Repository.Hierarchy.LoggerCreationEventArgs.Logger" /> is created.
	/// </para>
	/// </remarks>
	public class LoggerCreationEventArgs : EventArgs
	{
		/// <summary>
		/// The <see cref="P:log4net.Repository.Hierarchy.LoggerCreationEventArgs.Logger" /> created
		/// </summary>
		private Logger m_log;

		/// <summary>
		/// Gets the <see cref="P:log4net.Repository.Hierarchy.LoggerCreationEventArgs.Logger" /> that has been created.
		/// </summary>
		/// <value>
		/// The <see cref="P:log4net.Repository.Hierarchy.LoggerCreationEventArgs.Logger" /> that has been created.
		/// </value>
		/// <remarks>
		/// <para>
		/// The <see cref="P:log4net.Repository.Hierarchy.LoggerCreationEventArgs.Logger" /> that has been created.
		/// </para>
		/// </remarks>
		public Logger Logger => m_log;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="log">The <see cref="P:log4net.Repository.Hierarchy.LoggerCreationEventArgs.Logger" /> that has been created.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Repository.Hierarchy.LoggerCreationEventArgs" /> event argument 
		/// class,with the specified <see cref="P:log4net.Repository.Hierarchy.LoggerCreationEventArgs.Logger" />.
		/// </para>
		/// </remarks>
		public LoggerCreationEventArgs(Logger log)
		{
			m_log = log;
		}
	}
}
