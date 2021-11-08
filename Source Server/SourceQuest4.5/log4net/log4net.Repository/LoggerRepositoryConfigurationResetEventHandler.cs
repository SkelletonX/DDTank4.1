using System;

namespace log4net.Repository
{
	/// <summary>
	/// Delegate used to handle logger repository configuration reset event notifications
	/// </summary>
	/// <param name="sender">The <see cref="T:log4net.Repository.ILoggerRepository" /> that has had its configuration reset.</param>
	/// <param name="e">Empty event args</param>
	/// <remarks>
	/// <para>
	/// Delegate used to handle logger repository configuration reset event notifications.
	/// </para>
	/// </remarks>
	public delegate void LoggerRepositoryConfigurationResetEventHandler(object sender, EventArgs e);
}
