namespace log4net.Core
{
	/// <summary>
	/// Base interface for all wrappers
	/// </summary>
	/// <remarks>
	/// <para>
	/// Base interface for all wrappers.
	/// </para>
	/// <para>
	/// All wrappers must implement this interface.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public interface ILoggerWrapper
	{
		/// <summary>
		/// Get the implementation behind this wrapper object.
		/// </summary>
		/// <value>
		/// The <see cref="T:log4net.Core.ILogger" /> object that in implementing this object.
		/// </value>
		/// <remarks>
		/// <para>
		/// The <see cref="T:log4net.Core.ILogger" /> object that in implementing this
		/// object. The <c>Logger</c> object may not 
		/// be the same object as this object because of logger decorators.
		/// This gets the actual underlying objects that is used to process
		/// the log events.
		/// </para>
		/// </remarks>
		ILogger Logger
		{
			get;
		}
	}
}
