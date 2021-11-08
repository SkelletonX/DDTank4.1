using System;
using System.IO;
using System.Text;

namespace log4net.DateFormatter
{
	/// <summary>
	/// Formats a <see cref="T:System.DateTime" /> as <c>"HH:mm:ss,fff"</c>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Formats a <see cref="T:System.DateTime" /> in the format <c>"HH:mm:ss,fff"</c> for example, <c>"15:49:37,459"</c>.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class AbsoluteTimeDateFormatter : IDateFormatter
	{
		/// <summary>
		/// String constant used to specify AbsoluteTimeDateFormat in layouts. Current value is <b>ABSOLUTE</b>.
		/// </summary>
		public const string AbsoluteTimeDateFormat = "ABSOLUTE";

		/// <summary>
		/// String constant used to specify DateTimeDateFormat in layouts.  Current value is <b>DATE</b>.
		/// </summary>
		public const string DateAndTimeDateFormat = "DATE";

		/// <summary>
		/// String constant used to specify ISO8601DateFormat in layouts. Current value is <b>ISO8601</b>.
		/// </summary>
		public const string Iso8601TimeDateFormat = "ISO8601";

		/// <summary>
		/// Last stored time with precision up to the second.
		/// </summary>
		private static long s_lastTimeToTheSecond = 0L;

		/// <summary>
		/// Last stored time with precision up to the second, formatted
		/// as a string.
		/// </summary>
		private static StringBuilder s_lastTimeBuf = new StringBuilder();

		/// <summary>
		/// Last stored time with precision up to the second, formatted
		/// as a string.
		/// </summary>
		private static string s_lastTimeString;

		/// <summary>
		/// Renders the date into a string. Format is <c>"HH:mm:ss"</c>.
		/// </summary>
		/// <param name="dateToFormat">The date to render into a string.</param>
		/// <param name="buffer">The string builder to write to.</param>
		/// <remarks>
		/// <para>
		/// Subclasses should override this method to render the date
		/// into a string using a precision up to the second. This method
		/// will be called at most once per second and the result will be
		/// reused if it is needed again during the same second.
		/// </para>
		/// </remarks>
		protected virtual void FormatDateWithoutMillis(DateTime dateToFormat, StringBuilder buffer)
		{
			int hour = dateToFormat.Hour;
			if (hour < 10)
			{
				buffer.Append('0');
			}
			buffer.Append(hour);
			buffer.Append(':');
			int minute = dateToFormat.Minute;
			if (minute < 10)
			{
				buffer.Append('0');
			}
			buffer.Append(minute);
			buffer.Append(':');
			int second = dateToFormat.Second;
			if (second < 10)
			{
				buffer.Append('0');
			}
			buffer.Append(second);
		}

		/// <summary>
		/// Renders the date into a string. Format is "HH:mm:ss,fff".
		/// </summary>
		/// <param name="dateToFormat">The date to render into a string.</param>
		/// <param name="writer">The writer to write to.</param>
		/// <remarks>
		/// <para>
		/// Uses the <see cref="M:log4net.DateFormatter.AbsoluteTimeDateFormatter.FormatDateWithoutMillis(System.DateTime,System.Text.StringBuilder)" /> method to generate the
		/// time string up to the seconds and then appends the current
		/// milliseconds. The results from <see cref="M:log4net.DateFormatter.AbsoluteTimeDateFormatter.FormatDateWithoutMillis(System.DateTime,System.Text.StringBuilder)" /> are
		/// cached and <see cref="M:log4net.DateFormatter.AbsoluteTimeDateFormatter.FormatDateWithoutMillis(System.DateTime,System.Text.StringBuilder)" /> is called at most once
		/// per second.
		/// </para>
		/// <para>
		/// Sub classes should override <see cref="M:log4net.DateFormatter.AbsoluteTimeDateFormatter.FormatDateWithoutMillis(System.DateTime,System.Text.StringBuilder)" />
		/// rather than <see cref="M:log4net.DateFormatter.AbsoluteTimeDateFormatter.FormatDate(System.DateTime,System.IO.TextWriter)" />.
		/// </para>
		/// </remarks>
		public virtual void FormatDate(DateTime dateToFormat, TextWriter writer)
		{
			long num = dateToFormat.Ticks - dateToFormat.Ticks % 10000000;
			if (s_lastTimeToTheSecond != num)
			{
				lock (s_lastTimeBuf)
				{
					if (s_lastTimeToTheSecond != num)
					{
						s_lastTimeBuf.Length = 0;
						FormatDateWithoutMillis(dateToFormat, s_lastTimeBuf);
						string text = s_lastTimeString = s_lastTimeBuf.ToString();
						s_lastTimeToTheSecond = num;
					}
				}
			}
			writer.Write(s_lastTimeString);
			writer.Write(',');
			int millisecond = dateToFormat.Millisecond;
			if (millisecond < 100)
			{
				writer.Write('0');
			}
			if (millisecond < 10)
			{
				writer.Write('0');
			}
			writer.Write(millisecond);
		}
	}
}
