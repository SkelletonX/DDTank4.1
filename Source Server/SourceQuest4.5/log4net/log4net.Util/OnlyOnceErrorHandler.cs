using log4net.Core;
using System;

namespace log4net.Util
{
	/// <summary>
	/// Implements log4net's default error handling policy which consists 
	/// of emitting a message for the first error in an appender and 
	/// ignoring all subsequent errors.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The error message is printed on the standard error output stream.
	/// </para>
	/// <para>
	/// This policy aims at protecting an otherwise working application
	/// from being flooded with error messages when logging fails.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class OnlyOnceErrorHandler : IErrorHandler
	{
		/// <summary>
		/// Flag to indicate if it is the first error
		/// </summary>
		private bool m_firstTime = true;

		/// <summary>
		/// String to prefix each message with
		/// </summary>
		private readonly string m_prefix;

		/// <summary>
		/// Is error logging enabled
		/// </summary>
		/// <remarks>
		/// <para>
		/// Is error logging enabled. Logging is only enabled for the
		/// first error delivered to the <see cref="T:log4net.Util.OnlyOnceErrorHandler" />.
		/// </para>
		/// </remarks>
		private bool IsEnabled
		{
			get
			{
				if (m_firstTime)
				{
					m_firstTime = false;
					return true;
				}
				if (LogLog.InternalDebugging && !LogLog.QuietMode)
				{
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.OnlyOnceErrorHandler" /> class.
		/// </para>
		/// </remarks>
		public OnlyOnceErrorHandler()
		{
			m_prefix = "";
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="prefix">The prefix to use for each message.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.OnlyOnceErrorHandler" /> class
		/// with the specified prefix.
		/// </para>
		/// </remarks>
		public OnlyOnceErrorHandler(string prefix)
		{
			m_prefix = prefix;
		}

		/// <summary>
		/// Log an Error
		/// </summary>
		/// <param name="message">The error message.</param>
		/// <param name="e">The exception.</param>
		/// <param name="errorCode">The internal error code.</param>
		/// <remarks>
		/// <para>
		/// Prints the message and the stack trace of the exception on the standard
		/// error output stream.
		/// </para>
		/// </remarks>
		public void Error(string message, Exception e, ErrorCode errorCode)
		{
			if (IsEnabled)
			{
				LogLog.Error("[" + m_prefix + "] " + message, e);
			}
		}

		/// <summary>
		/// Log an Error
		/// </summary>
		/// <param name="message">The error message.</param>
		/// <param name="e">The exception.</param>
		/// <remarks>
		/// <para>
		/// Prints the message and the stack trace of the exception on the standard
		/// error output stream.
		/// </para>
		/// </remarks>
		public void Error(string message, Exception e)
		{
			if (IsEnabled)
			{
				LogLog.Error("[" + m_prefix + "] " + message, e);
			}
		}

		/// <summary>
		/// Log an error
		/// </summary>
		/// <param name="message">The error message.</param>
		/// <remarks>
		/// <para>
		/// Print a the error message passed as parameter on the standard
		/// error output stream.
		/// </para>
		/// </remarks>
		public void Error(string message)
		{
			if (IsEnabled)
			{
				LogLog.Error("[" + m_prefix + "] " + message);
			}
		}
	}
}
