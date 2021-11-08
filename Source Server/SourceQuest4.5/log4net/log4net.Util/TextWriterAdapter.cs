using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace log4net.Util
{
	/// <summary>
	/// Adapter that extends <see cref="T:System.IO.TextWriter" /> and forwards all
	/// messages to an instance of <see cref="T:System.IO.TextWriter" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Adapter that extends <see cref="T:System.IO.TextWriter" /> and forwards all
	/// messages to an instance of <see cref="T:System.IO.TextWriter" />.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public abstract class TextWriterAdapter : TextWriter
	{
		/// <summary>
		/// The writer to forward messages to
		/// </summary>
		private TextWriter m_writer;

		/// <summary>
		/// Gets or sets the underlying <see cref="T:System.IO.TextWriter" />.
		/// </summary>
		/// <value>
		/// The underlying <see cref="T:System.IO.TextWriter" />.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets or sets the underlying <see cref="T:System.IO.TextWriter" />.
		/// </para>
		/// </remarks>
		protected TextWriter Writer
		{
			get
			{
				return m_writer;
			}
			set
			{
				m_writer = value;
			}
		}

		/// <summary>
		/// The Encoding in which the output is written
		/// </summary>
		/// <value>
		/// The <see cref="P:log4net.Util.TextWriterAdapter.Encoding" />
		/// </value>
		/// <remarks>
		/// <para>
		/// The Encoding in which the output is written
		/// </para>
		/// </remarks>
		public override Encoding Encoding => m_writer.Encoding;

		/// <summary>
		/// Gets an object that controls formatting
		/// </summary>
		/// <value>
		/// The format provider
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets an object that controls formatting
		/// </para>
		/// </remarks>
		public override IFormatProvider FormatProvider => m_writer.FormatProvider;

		/// <summary>
		/// Gets or sets the line terminator string used by the TextWriter
		/// </summary>
		/// <value>
		/// The line terminator to use
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets or sets the line terminator string used by the TextWriter
		/// </para>
		/// </remarks>
		public override string NewLine
		{
			get
			{
				return m_writer.NewLine;
			}
			set
			{
				m_writer.NewLine = value;
			}
		}

		/// <summary>
		/// Create an instance of <see cref="T:log4net.Util.TextWriterAdapter" /> that forwards all
		/// messages to a <see cref="T:System.IO.TextWriter" />.
		/// </summary>
		/// <param name="writer">The <see cref="T:System.IO.TextWriter" /> to forward to</param>
		/// <remarks>
		/// <para>
		/// Create an instance of <see cref="T:log4net.Util.TextWriterAdapter" /> that forwards all
		/// messages to a <see cref="T:System.IO.TextWriter" />.
		/// </para>
		/// </remarks>
		protected TextWriterAdapter(TextWriter writer)
			: base(CultureInfo.InvariantCulture)
		{
			m_writer = writer;
		}

		/// <summary>
		/// Closes the writer and releases any system resources associated with the writer
		/// </summary>
		/// <remarks>
		/// <para>
		/// </para>
		/// </remarks>
		public override void Close()
		{
			m_writer.Close();
		}

		/// <summary>
		/// Dispose this writer
		/// </summary>
		/// <param name="disposing">flag indicating if we are being disposed</param>
		/// <remarks>
		/// <para>
		/// Dispose this writer
		/// </para>
		/// </remarks>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				((IDisposable)m_writer).Dispose();
			}
		}

		/// <summary>
		/// Flushes any buffered output
		/// </summary>
		/// <remarks>
		/// <para>
		/// Clears all buffers for the writer and causes any buffered data to be written 
		/// to the underlying device
		/// </para>
		/// </remarks>
		public override void Flush()
		{
			m_writer.Flush();
		}

		/// <summary>
		/// Writes a character to the wrapped TextWriter
		/// </summary>
		/// <param name="value">the value to write to the TextWriter</param>
		/// <remarks>
		/// <para>
		/// Writes a character to the wrapped TextWriter
		/// </para>
		/// </remarks>
		public override void Write(char value)
		{
			m_writer.Write(value);
		}

		/// <summary>
		/// Writes a character buffer to the wrapped TextWriter
		/// </summary>
		/// <param name="buffer">the data buffer</param>
		/// <param name="index">the start index</param>
		/// <param name="count">the number of characters to write</param>
		/// <remarks>
		/// <para>
		/// Writes a character buffer to the wrapped TextWriter
		/// </para>
		/// </remarks>
		public override void Write(char[] buffer, int index, int count)
		{
			m_writer.Write(buffer, index, count);
		}

		/// <summary>
		/// Writes a string to the wrapped TextWriter
		/// </summary>
		/// <param name="value">the value to write to the TextWriter</param>
		/// <remarks>
		/// <para>
		/// Writes a string to the wrapped TextWriter
		/// </para>
		/// </remarks>
		public override void Write(string value)
		{
			m_writer.Write(value);
		}
	}
}
