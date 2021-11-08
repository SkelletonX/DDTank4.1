namespace log4net.Core
{
	/// <summary>
	/// Test if an <see cref="T:log4net.Core.LoggingEvent" /> triggers an action
	/// </summary>
	/// <remarks>
	/// <para>
	/// Implementations of this interface allow certain appenders to decide
	/// when to perform an appender specific action.
	/// </para>
	/// <para>
	/// The action or behavior triggered is defined by the implementation.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public interface ITriggeringEventEvaluator
	{
		/// <summary>
		/// Test if this event triggers the action
		/// </summary>
		/// <param name="loggingEvent">The event to check</param>
		/// <returns><c>true</c> if this event triggers the action, otherwise <c>false</c></returns>
		/// <remarks>
		/// <para>
		/// Return <c>true</c> if this event triggers the action
		/// </para>
		/// </remarks>
		bool IsTriggeringEvent(LoggingEvent loggingEvent);
	}
}
