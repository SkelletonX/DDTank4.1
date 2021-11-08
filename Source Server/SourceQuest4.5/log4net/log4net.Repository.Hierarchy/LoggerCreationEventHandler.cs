namespace log4net.Repository.Hierarchy
{
	/// <summary>
	/// Delegate used to handle logger creation event notifications.
	/// </summary>
	/// <param name="sender">The <see cref="T:log4net.Repository.Hierarchy.Hierarchy" /> in which the <see cref="T:log4net.Repository.Hierarchy.Logger" /> has been created.</param>
	/// <param name="e">The <see cref="T:log4net.Repository.Hierarchy.LoggerCreationEventArgs" /> event args that hold the <see cref="T:log4net.Repository.Hierarchy.Logger" /> instance that has been created.</param>
	/// <remarks>
	/// <para>
	/// Delegate used to handle logger creation event notifications.
	/// </para>
	/// </remarks>
	public delegate void LoggerCreationEventHandler(object sender, LoggerCreationEventArgs e);
}
