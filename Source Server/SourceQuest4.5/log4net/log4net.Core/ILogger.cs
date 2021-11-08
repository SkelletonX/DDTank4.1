using log4net.Repository;
using System;

namespace log4net.Core
{
	/// <summary>
	/// Interface that all loggers implement
	/// </summary>
	/// <remarks>
	/// <para>
	/// This interface supports logging events and testing if a level
	/// is enabled for logging.
	/// </para>
	/// <para>
	/// These methods will not throw exceptions. Note to implementor, ensure
	/// that the implementation of these methods cannot allow an exception
	/// to be thrown to the caller.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public interface ILogger
	{
		/// <summary>
		/// Gets the name of the logger.
		/// </summary>
		/// <value>
		/// The name of the logger.
		/// </value>
		/// <remarks>
		/// <para>
		/// The name of this logger
		/// </para>
		/// </remarks>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the <see cref="T:log4net.Repository.ILoggerRepository" /> where this 
		/// <c>Logger</c> instance is attached to.
		/// </summary>
		/// <value>
		/// The <see cref="T:log4net.Repository.ILoggerRepository" /> that this logger belongs to.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets the <see cref="T:log4net.Repository.ILoggerRepository" /> where this 
		/// <c>Logger</c> instance is attached to.
		/// </para>
		/// </remarks>
		ILoggerRepository Repository
		{
			get;
		}

		/// <summary>
		/// This generic form is intended to be used by wrappers.
		/// </summary>
		/// <param name="callerStackBoundaryDeclaringType">The declaring type of the method that is
		/// the stack boundary into the logging system for this call.</param>
		/// <param name="level">The level of the message to be logged.</param>
		/// <param name="message">The message object to log.</param>
		/// <param name="exception">the exception to log, including its stack trace. Pass <c>null</c> to not log an exception.</param>
		/// <remarks>
		/// <para>
		/// Generates a logging event for the specified <paramref name="level" /> using
		/// the <paramref name="message" /> and <paramref name="exception" />.
		/// </para>
		/// </remarks>
		void Log(Type callerStackBoundaryDeclaringType, Level level, object message, Exception exception);

		/// <summary>
		/// This is the most generic printing method that is intended to be used 
		/// by wrappers.
		/// </summary>
		/// <param name="logEvent">The event being logged.</param>
		/// <remarks>
		/// <para>
		/// Logs the specified logging event through this logger.
		/// </para>
		/// </remarks>
		void Log(LoggingEvent logEvent);

		/// <summary>
		/// Checks if this logger is enabled for a given <see cref="T:log4net.Core.Level" /> passed as parameter.
		/// </summary>
		/// <param name="level">The level to check.</param>
		/// <returns>
		/// <c>true</c> if this logger is enabled for <c>level</c>, otherwise <c>false</c>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Test if this logger is going to log events of the specified <paramref name="level" />.
		/// </para>
		/// </remarks>
		bool IsEnabledFor(Level level);
	}
}
