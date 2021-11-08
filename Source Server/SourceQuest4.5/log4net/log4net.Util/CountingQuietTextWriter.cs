using log4net.Core;
using System;
using System.IO;

namespace log4net.Util
{
	/// <summary>
	/// Subclass of <see cref="T:log4net.Util.QuietTextWriter" /> that maintains a count of 
	/// the number of bytes written.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This writer counts the number of bytes written.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class CountingQuietTextWriter : QuietTextWriter
	{
		/// <summary>
		/// Total number of bytes written.
		/// </summary>
		private long m_countBytes;

		/// <summary>
		/// Gets or sets the total number of bytes written.
		/// </summary>
		/// <value>
		/// The total number of bytes written.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets or sets the total number of bytes written.
		/// </para>
		/// </remarks>
		public long Count
		{
			get
			{
				return m_countBytes;
			}
			set
			{
				m_countBytes = value;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="writer">The <see cref="T:System.IO.TextWriter" /> to actually write to.</param>
		/// <param name="errorHandler">The <see cref="T:log4net.Core.IErrorHandler" /> to report errors to.</param>
		/// <remarks>
		/// <para>
		/// Creates a new instance of the <see cref="T:log4net.Util.CountingQuietTextWriter" /> class 
		/// with the specified <see cref="T:System.IO.TextWriter" /> and <see cref="T:log4net.Core.IErrorHandler" />.
		/// </para>
		/// </remarks>
		public CountingQuietTextWriter(TextWriter writer, IErrorHandler errorHandler)
			: base(writer, errorHandler)
		{
			m_countBytes = 0L;
		}

		/// <summary>
		/// Writes a character to the underlying writer and counts the number of bytes written.
		/// </summary>
		/// <param name="value">the char to write</param>
		/// <remarks>
		/// <para>
		/// Overrides implementation of <see cref="T:log4net.Util.QuietTextWriter" />. Counts
		/// the number of bytes written.
		/// </para>
		/// </remarks>
		public override void Write(char value)
		{
			try
			{
				base.Write(value);
				m_countBytes += Encoding.GetByteCount(new char[1]
				{
					value
				});
			}
			catch (Exception e)
			{
				base.ErrorHandler.Error("Failed to write [" + value + "].", e, ErrorCode.WriteFailure);
			}
		}

		/// <summary>
		/// Writes a buffer to the underlying writer and counts the number of bytes written.
		/// </summary>
		/// <param name="buffer">the buffer to write</param>
		/// <param name="index">the start index to write from</param>
		/// <param name="count">the number of characters to write</param>
		/// <remarks>
		/// <para>
		/// Overrides implementation of <see cref="T:log4net.Util.QuietTextWriter" />. Counts
		/// the number of bytes written.
		/// </para>
		/// </remarks>
		public override void Write(char[] buffer, int index, int count)
		{
			if (count > 0)
			{
				try
				{
					base.Write(buffer, index, count);
					m_countBytes += Encoding.GetByteCount(buffer, index, count);
				}
				catch (Exception e)
				{
					base.ErrorHandler.Error("Failed to write buffer.", e, ErrorCode.WriteFailure);
				}
			}
		}

		/// <summary>
		/// Writes a string to the output and counts the number of bytes written.
		/// </summary>
		/// <param name="str">The string data to write to the output.</param>
		/// <remarks>
		/// <para>
		/// Overrides implementation of <see cref="T:log4net.Util.QuietTextWriter" />. Counts
		/// the number of bytes written.
		/// </para>
		/// </remarks>
		public override void Write(string str)
		{
			if (str != null && str.Length > 0)
			{
				try
				{
					base.Write(str);
					m_countBytes += Encoding.GetByteCount(str);
				}
				catch (Exception e)
				{
					base.ErrorHandler.Error("Failed to write [" + str + "].", e, ErrorCode.WriteFailure);
				}
			}
		}
	}
}
