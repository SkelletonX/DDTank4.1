using log4net.Core;
using System;
using System.IO;

namespace log4net.Util
{
	/// <summary>
	/// <see cref="T:System.IO.TextWriter" /> that does not leak exceptions
	/// </summary>
	/// <remarks>
	/// <para>
	/// <see cref="T:log4net.Util.QuietTextWriter" /> does not throw exceptions when things go wrong. 
	/// Instead, it delegates error handling to its <see cref="T:log4net.Core.IErrorHandler" />.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class QuietTextWriter : TextWriterAdapter
	{
		/// <summary>
		/// The error handler instance to pass all errors to
		/// </summary>
		private IErrorHandler m_errorHandler;

		/// <summary>
		/// Flag to indicate if this writer is closed
		/// </summary>
		private bool m_closed = false;

		/// <summary>
		/// Gets or sets the error handler that all errors are passed to.
		/// </summary>
		/// <value>
		/// The error handler that all errors are passed to.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets or sets the error handler that all errors are passed to.
		/// </para>
		/// </remarks>
		public IErrorHandler ErrorHandler
		{
			get
			{
				return m_errorHandler;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				m_errorHandler = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this writer is closed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this writer is closed, otherwise <c>false</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets a value indicating whether this writer is closed.
		/// </para>
		/// </remarks>
		public bool Closed => m_closed;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="writer">the writer to actually write to</param>
		/// <param name="errorHandler">the error handler to report error to</param>
		/// <remarks>
		/// <para>
		/// Create a new QuietTextWriter using a writer and error handler
		/// </para>
		/// </remarks>
		public QuietTextWriter(TextWriter writer, IErrorHandler errorHandler)
			: base(writer)
		{
			if (errorHandler == null)
			{
				throw new ArgumentNullException("errorHandler");
			}
			ErrorHandler = errorHandler;
		}

		/// <summary>
		/// Writes a character to the underlying writer
		/// </summary>
		/// <param name="value">the char to write</param>
		/// <remarks>
		/// <para>
		/// Writes a character to the underlying writer
		/// </para>
		/// </remarks>
		public override void Write(char value)
		{
			try
			{
				base.Write(value);
			}
			catch (Exception e)
			{
				m_errorHandler.Error("Failed to write [" + value + "].", e, ErrorCode.WriteFailure);
			}
		}

		/// <summary>
		/// Writes a buffer to the underlying writer
		/// </summary>
		/// <param name="buffer">the buffer to write</param>
		/// <param name="index">the start index to write from</param>
		/// <param name="count">the number of characters to write</param>
		/// <remarks>
		/// <para>
		/// Writes a buffer to the underlying writer
		/// </para>
		/// </remarks>
		public override void Write(char[] buffer, int index, int count)
		{
			try
			{
				base.Write(buffer, index, count);
			}
			catch (Exception e)
			{
				m_errorHandler.Error("Failed to write buffer.", e, ErrorCode.WriteFailure);
			}
		}

		/// <summary>
		/// Writes a string to the output.
		/// </summary>
		/// <param name="value">The string data to write to the output.</param>
		/// <remarks>
		/// <para>
		/// Writes a string to the output.
		/// </para>
		/// </remarks>
		public override void Write(string value)
		{
			try
			{
				base.Write(value);
			}
			catch (Exception e)
			{
				m_errorHandler.Error("Failed to write [" + value + "].", e, ErrorCode.WriteFailure);
			}
		}

		/// <summary>
		/// Closes the underlying output writer.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Closes the underlying output writer.
		/// </para>
		/// </remarks>
		public override void Close()
		{
			m_closed = true;
			base.Close();
		}
	}
}
