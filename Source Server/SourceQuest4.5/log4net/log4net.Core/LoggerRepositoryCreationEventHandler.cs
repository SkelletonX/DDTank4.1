namespace log4net.Core
{
	/// <summary>
	/// Delegate used to handle logger repository creation event notifications
	/// </summary>
	/// <param name="sender">The <see cref="T:log4net.Core.IRepositorySelector" /> which created the repository.</param>
	/// <param name="e">The <see cref="T:log4net.Core.LoggerRepositoryCreationEventArgs" /> event args
	/// that holds the <see cref="T:log4net.Repository.ILoggerRepository" /> instance that has been created.</param>
	/// <remarks>
	/// <para>
	/// Delegate used to handle logger repository creation event notifications.
	/// </para>
	/// </remarks>
	public delegate void LoggerRepositoryCreationEventHandler(object sender, LoggerRepositoryCreationEventArgs e);
}
