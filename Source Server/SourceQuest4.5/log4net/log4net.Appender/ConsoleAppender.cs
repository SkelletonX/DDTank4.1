using log4net.Core;
using log4net.Layout;
using System;
using System.Globalization;

namespace log4net.Appender
{
	/// <summary>
	/// Appends logging events to the console.
	/// </summary>
	/// <remarks>
	/// <para>
	/// ConsoleAppender appends log events to the standard output stream
	/// or the error output stream using a layout specified by the 
	/// user.
	/// </para>
	/// <para>
	/// By default, all output is written to the console's standard output stream.
	/// The <see cref="P:log4net.Appender.ConsoleAppender.Target" /> property can be set to direct the output to the
	/// error stream.
	/// </para>
	/// <para>
	/// NOTE: This appender writes each message to the <c>System.Console.Out</c> or 
	/// <c>System.Console.Error</c> that is set at the time the event is appended.
	/// Therefore it is possible to programmatically redirect the output of this appender 
	/// (for example NUnit does this to capture program output). While this is the desired
	/// behavior of this appender it may have security implications in your application. 
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class ConsoleAppender : AppenderSkeleton
	{
		/// <summary>
		/// The <see cref="P:log4net.Appender.ConsoleAppender.Target" /> to use when writing to the Console 
		/// standard output stream.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="P:log4net.Appender.ConsoleAppender.Target" /> to use when writing to the Console 
		/// standard output stream.
		/// </para>
		/// </remarks>
		public const string ConsoleOut = "Console.Out";

		/// <summary>
		/// The <see cref="P:log4net.Appender.ConsoleAppender.Target" /> to use when writing to the Console 
		/// standard error output stream.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="P:log4net.Appender.ConsoleAppender.Target" /> to use when writing to the Console 
		/// standard error output stream.
		/// </para>
		/// </remarks>
		public const string ConsoleError = "Console.Error";

		private bool m_writeToErrorStream = false;

		/// <summary>
		/// Target is the value of the console output stream.
		/// This is either <c>"Console.Out"</c> or <c>"Console.Error"</c>.
		/// </summary>
		/// <value>
		/// Target is the value of the console output stream.
		/// This is either <c>"Console.Out"</c> or <c>"Console.Error"</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// Target is the value of the console output stream.
		/// This is either <c>"Console.Out"</c> or <c>"Console.Error"</c>.
		/// </para>
		/// </remarks>
		public virtual string Target
		{
			get
			{
				return m_writeToErrorStream ? "Console.Error" : "Console.Out";
			}
			set
			{
				string strB = value.Trim();
				if (string.Compare("Console.Error", strB, ignoreCase: true, CultureInfo.InvariantCulture) == 0)
				{
					m_writeToErrorStream = true;
				}
				else
				{
					m_writeToErrorStream = false;
				}
			}
		}

		/// <summary>
		/// This appender requires a <see cref="N:log4net.Layout" /> to be set.
		/// </summary>
		/// <value><c>true</c></value>
		/// <remarks>
		/// <para>
		/// This appender requires a <see cref="N:log4net.Layout" /> to be set.
		/// </para>
		/// </remarks>
		protected override bool RequiresLayout => true;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Appender.ConsoleAppender" /> class.
		/// </summary>
		/// <remarks>
		/// The instance of the <see cref="T:log4net.Appender.ConsoleAppender" /> class is set up to write 
		/// to the standard output stream.
		/// </remarks>
		public ConsoleAppender()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Appender.ConsoleAppender" /> class
		/// with the specified layout.
		/// </summary>
		/// <param name="layout">the layout to use for this appender</param>
		/// <remarks>
		/// The instance of the <see cref="T:log4net.Appender.ConsoleAppender" /> class is set up to write 
		/// to the standard output stream.
		/// </remarks>
		[Obsolete("Instead use the default constructor and set the Layout property")]
		public ConsoleAppender(ILayout layout)
			: this(layout, writeToErrorStream: false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Appender.ConsoleAppender" /> class
		/// with the specified layout.
		/// </summary>
		/// <param name="layout">the layout to use for this appender</param>
		/// <param name="writeToErrorStream">flag set to <c>true</c> to write to the console error stream</param>
		/// <remarks>
		/// When <paramref name="writeToErrorStream" /> is set to <c>true</c>, output is written to
		/// the standard error output stream.  Otherwise, output is written to the standard
		/// output stream.
		/// </remarks>
		[Obsolete("Instead use the default constructor and set the Layout & Target properties")]
		public ConsoleAppender(ILayout layout, bool writeToErrorStream)
		{
			Layout = layout;
			m_writeToErrorStream = writeToErrorStream;
		}

		/// <summary>
		/// This method is called by the <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)" /> method.
		/// </summary>
		/// <param name="loggingEvent">The event to log.</param>
		/// <remarks>
		/// <para>
		/// Writes the event to the console.
		/// </para>
		/// <para>
		/// The format of the output will depend on the appender's layout.
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent loggingEvent)
		{
			if (m_writeToErrorStream)
			{
				Console.Error.Write(RenderLoggingEvent(loggingEvent));
			}
			else
			{
				Console.Write(RenderLoggingEvent(loggingEvent));
			}
		}
	}
}
