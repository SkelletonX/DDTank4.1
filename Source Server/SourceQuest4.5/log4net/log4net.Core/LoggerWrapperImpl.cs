namespace log4net.Core
{
	/// <summary>
	/// Implementation of the <see cref="T:log4net.Core.ILoggerWrapper" /> interface.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class should be used as the base for all wrapper implementations.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public abstract class LoggerWrapperImpl : ILoggerWrapper
	{
		/// <summary>
		/// The logger that this object is wrapping
		/// </summary>
		private readonly ILogger m_logger;

		/// <summary>
		/// Gets the implementation behind this wrapper object.
		/// </summary>
		/// <value>
		/// The <see cref="T:log4net.Core.ILogger" /> object that this object is implementing.
		/// </value>
		/// <remarks>
		/// <para>
		/// The <c>Logger</c> object may not be the same object as this object 
		/// because of logger decorators.
		/// </para>
		/// <para>
		/// This gets the actual underlying objects that is used to process
		/// the log events.
		/// </para>
		/// </remarks>
		public virtual ILogger Logger => m_logger;

		/// <summary>
		/// Constructs a new wrapper for the specified logger.
		/// </summary>
		/// <param name="logger">The logger to wrap.</param>
		/// <remarks>
		/// <para>
		/// Constructs a new wrapper for the specified logger.
		/// </para>
		/// </remarks>
		protected LoggerWrapperImpl(ILogger logger)
		{
			m_logger = logger;
		}
	}
}
