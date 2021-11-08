using System;

namespace log4net.Core
{
	/// <summary>
	/// Appenders may delegate their error handling to an <see cref="T:log4net.Core.IErrorHandler" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Error handling is a particularly tedious to get right because by
	/// definition errors are hard to predict and to reproduce. 
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public interface IErrorHandler
	{
		/// <summary>
		/// Handles the error and information about the error condition is passed as 
		/// a parameter.
		/// </summary>
		/// <param name="message">The message associated with the error.</param>
		/// <param name="e">The <see cref="T:System.Exception" /> that was thrown when the error occurred.</param>
		/// <param name="errorCode">The error code associated with the error.</param>
		/// <remarks>
		/// <para>
		/// Handles the error and information about the error condition is passed as 
		/// a parameter.
		/// </para>
		/// </remarks>
		void Error(string message, Exception e, ErrorCode errorCode);

		/// <summary>
		/// Prints the error message passed as a parameter.
		/// </summary>
		/// <param name="message">The message associated with the error.</param>
		/// <param name="e">The <see cref="T:System.Exception" /> that was thrown when the error occurred.</param>
		/// <remarks>
		/// <para>
		/// See <see cref="M:log4net.Core.IErrorHandler.Error(System.String,System.Exception,log4net.Core.ErrorCode)" />.
		/// </para>
		/// </remarks>
		void Error(string message, Exception e);

		/// <summary>
		/// Prints the error message passed as a parameter.
		/// </summary>
		/// <param name="message">The message associated with the error.</param>
		/// <remarks>
		/// <para>
		/// See <see cref="M:log4net.Core.IErrorHandler.Error(System.String,System.Exception,log4net.Core.ErrorCode)" />.
		/// </para>
		/// </remarks>
		void Error(string message);
	}
}
