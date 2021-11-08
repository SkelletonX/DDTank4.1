using System;

namespace log4net.Repository
{
	/// <summary>
	/// Delegate used to handle logger repository shutdown event notifications
	/// </summary>
	/// <param name="sender">The <see cref="T:log4net.Repository.ILoggerRepository" /> that is shutting down.</param>
	/// <param name="e">Empty event args</param>
	/// <remarks>
	/// <para>
	/// Delegate used to handle logger repository shutdown event notifications.
	/// </para>
	/// </remarks>
	public delegate void LoggerRepositoryShutdownEventHandler(object sender, EventArgs e);
}
