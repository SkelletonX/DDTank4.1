using System;

namespace log4net.Repository
{
	/// <summary>
	/// Delegate used to handle event notifications for logger repository configuration changes.
	/// </summary>
	/// <param name="sender">The <see cref="T:log4net.Repository.ILoggerRepository" /> that has had its configuration changed.</param>
	/// <param name="e">Empty event arguments.</param>
	/// <remarks>
	/// <para>
	/// Delegate used to handle event notifications for logger repository configuration changes.
	/// </para>
	/// </remarks>
	public delegate void LoggerRepositoryConfigurationChangedEventHandler(object sender, EventArgs e);
}
