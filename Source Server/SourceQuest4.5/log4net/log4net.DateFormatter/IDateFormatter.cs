using System;
using System.IO;

namespace log4net.DateFormatter
{
	/// <summary>
	/// Render a <see cref="T:System.DateTime" /> as a string.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Interface to abstract the rendering of a <see cref="T:System.DateTime" />
	/// instance into a string.
	/// </para>
	/// <para>
	/// The <see cref="M:log4net.DateFormatter.IDateFormatter.FormatDate(System.DateTime,System.IO.TextWriter)" /> method is used to render the
	/// date to a text writer.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public interface IDateFormatter
	{
		/// <summary>
		/// Formats the specified date as a string.
		/// </summary>
		/// <param name="dateToFormat">The date to format.</param>
		/// <param name="writer">The writer to write to.</param>
		/// <remarks>
		/// <para>
		/// Format the <see cref="T:System.DateTime" /> as a string and write it
		/// to the <see cref="T:System.IO.TextWriter" /> provided.
		/// </para>
		/// </remarks>
		void FormatDate(DateTime dateToFormat, TextWriter writer);
	}
}
