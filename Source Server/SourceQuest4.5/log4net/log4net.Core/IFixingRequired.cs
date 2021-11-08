namespace log4net.Core
{
	/// <summary>
	/// Interface for objects that require fixing.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Interface that indicates that the object requires fixing before it
	/// can be taken outside the context of the appender's 
	/// <see cref="M:log4net.Appender.IAppender.DoAppend(log4net.Core.LoggingEvent)" /> method.
	/// </para>
	/// <para>
	/// When objects that implement this interface are stored 
	/// in the context properties maps <see cref="T:log4net.GlobalContext" />
	/// <see cref="P:log4net.GlobalContext.Properties" /> and <see cref="T:log4net.ThreadContext" />
	/// <see cref="P:log4net.ThreadContext.Properties" /> are fixed 
	/// (see <see cref="P:log4net.Core.LoggingEvent.Fix" />) the <see cref="M:log4net.Core.IFixingRequired.GetFixedObject" />
	/// method will be called.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public interface IFixingRequired
	{
		/// <summary>
		/// Get a portable version of this object
		/// </summary>
		/// <returns>the portable instance of this object</returns>
		/// <remarks>
		/// <para>
		/// Get a portable instance object that represents the current
		/// state of this object. The portable object can be stored
		/// and logged from any thread with identical results.
		/// </para>
		/// </remarks>
		object GetFixedObject();
	}
}
