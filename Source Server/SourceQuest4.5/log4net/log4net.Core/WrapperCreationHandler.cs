namespace log4net.Core
{
	/// <summary>
	/// Delegate used to handle creation of new wrappers.
	/// </summary>
	/// <param name="logger">The logger to wrap in a wrapper.</param>
	/// <remarks>
	/// <para>
	/// Delegate used to handle creation of new wrappers. This delegate
	/// is called from the <see cref="M:log4net.Core.WrapperMap.CreateNewWrapperObject(log4net.Core.ILogger)" />
	/// method to construct the wrapper for the specified logger.
	/// </para>
	/// <para>
	/// The delegate to use is supplied to the <see cref="T:log4net.Core.WrapperMap" />
	/// constructor.
	/// </para>
	/// </remarks>
	public delegate ILoggerWrapper WrapperCreationHandler(ILogger logger);
}
