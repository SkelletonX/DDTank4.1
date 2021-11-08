using System;
using System.IO;
using System.Text;

namespace log4net.Util
{
	/// <summary>
	/// A <see cref="T:System.IO.StringWriter" /> that can be <see cref="M:log4net.Util.ReusableStringWriter.Reset(System.Int32,System.Int32)" /> and reused
	/// </summary>
	/// <remarks>
	/// <para>
	/// A <see cref="T:System.IO.StringWriter" /> that can be <see cref="M:log4net.Util.ReusableStringWriter.Reset(System.Int32,System.Int32)" /> and reused.
	/// This uses a single buffer for string operations.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public class ReusableStringWriter : StringWriter
	{
		/// <summary>
		/// Create an instance of <see cref="T:log4net.Util.ReusableStringWriter" />
		/// </summary>
		/// <param name="formatProvider">the format provider to use</param>
		/// <remarks>
		/// <para>
		/// Create an instance of <see cref="T:log4net.Util.ReusableStringWriter" />
		/// </para>
		/// </remarks>
		public ReusableStringWriter(IFormatProvider formatProvider)
			: base(formatProvider)
		{
		}

		/// <summary>
		/// Override Dispose to prevent closing of writer
		/// </summary>
		/// <param name="disposing">flag</param>
		/// <remarks>
		/// <para>
		/// Override Dispose to prevent closing of writer
		/// </para>
		/// </remarks>
		protected override void Dispose(bool disposing)
		{
		}

		/// <summary>
		/// Reset this string writer so that it can be reused.
		/// </summary>
		/// <param name="maxCapacity">the maximum buffer capacity before it is trimmed</param>
		/// <param name="defaultSize">the default size to make the buffer</param>
		/// <remarks>
		/// <para>
		/// Reset this string writer so that it can be reused.
		/// The internal buffers are cleared and reset.
		/// </para>
		/// </remarks>
		public void Reset(int maxCapacity, int defaultSize)
		{
			StringBuilder stringBuilder = GetStringBuilder();
			stringBuilder.Length = 0;
			if (stringBuilder.Capacity > maxCapacity)
			{
				stringBuilder.Capacity = defaultSize;
			}
		}
	}
}
