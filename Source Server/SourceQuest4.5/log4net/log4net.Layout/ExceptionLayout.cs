using log4net.Core;
using System;
using System.IO;

namespace log4net.Layout
{
	/// <summary>
	/// A Layout that renders only the Exception text from the logging event
	/// </summary>
	/// <remarks>
	/// <para>
	/// A Layout that renders only the Exception text from the logging event.
	/// </para>
	/// <para>
	/// This Layout should only be used with appenders that utilize multiple
	/// layouts (e.g. <see cref="T:log4net.Appender.AdoNetAppender" />).
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class ExceptionLayout : LayoutSkeleton
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Constructs a ExceptionLayout
		/// </para>
		/// </remarks>
		public ExceptionLayout()
		{
			IgnoresException = false;
		}

		/// <summary>
		/// Activate component options
		/// </summary>
		/// <remarks>
		/// <para>
		/// Part of the <see cref="T:log4net.Core.IOptionHandler" /> component activation
		/// framework.
		/// </para>
		/// <para>
		/// This method does nothing as options become effective immediately.
		/// </para>
		/// </remarks>
		public override void ActivateOptions()
		{
		}

		/// <summary>
		/// Gets the exception text from the logging event
		/// </summary>
		/// <param name="writer">The TextWriter to write the formatted event to</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// <para>
		/// Write the exception string to the <see cref="T:System.IO.TextWriter" />.
		/// The exception string is retrieved from <see cref="M:log4net.Core.LoggingEvent.GetExceptionString" />.
		/// </para>
		/// </remarks>
		public override void Format(TextWriter writer, LoggingEvent loggingEvent)
		{
			if (loggingEvent == null)
			{
				throw new ArgumentNullException("loggingEvent");
			}
			writer.Write(loggingEvent.GetExceptionString());
		}
	}
}
