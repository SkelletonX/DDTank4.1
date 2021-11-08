using log4net.Core;
using log4net.Layout;
using log4net.Util;
using System;
using System.IO;

namespace log4net.Appender
{
	/// <summary>
	/// Sends logging events to a <see cref="T:System.IO.TextWriter" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// An Appender that writes to a <see cref="T:System.IO.TextWriter" />.
	/// </para>
	/// <para>
	/// This appender may be used stand alone if initialized with an appropriate
	/// writer, however it is typically used as a base class for an appender that
	/// can open a <see cref="T:System.IO.TextWriter" /> to write to.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	/// <author>Douglas de la Torre</author>
	public class TextWriterAppender : AppenderSkeleton
	{
		/// <summary>
		/// This is the <see cref="T:log4net.Util.QuietTextWriter" /> where logging events
		/// will be written to. 
		/// </summary>
		private QuietTextWriter m_qtw;

		/// <summary>
		/// Immediate flush means that the underlying <see cref="T:System.IO.TextWriter" /> 
		/// or output stream will be flushed at the end of each append operation.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Immediate flush is slower but ensures that each append request is 
		/// actually written. If <see cref="P:log4net.Appender.TextWriterAppender.ImmediateFlush" /> is set to
		/// <c>false</c>, then there is a good chance that the last few
		/// logging events are not actually persisted if and when the application 
		/// crashes.
		/// </para>
		/// <para>
		/// The default value is <c>true</c>.
		/// </para>
		/// </remarks>
		private bool m_immediateFlush = true;

		/// <summary>
		/// Gets or set whether the appender will flush at the end 
		/// of each append operation.
		/// </summary>
		/// <value>
		/// <para>
		/// The default behavior is to flush at the end of each 
		/// append operation.
		/// </para>
		/// <para>
		/// If this option is set to <c>false</c>, then the underlying 
		/// stream can defer persisting the logging event to a later 
		/// time.
		/// </para>
		/// </value>
		/// <remarks>
		/// Avoiding the flush operation at the end of each append results in
		/// a performance gain of 10 to 20 percent. However, there is safety
		/// trade-off involved in skipping flushing. Indeed, when flushing is
		/// skipped, then it is likely that the last few log events will not
		/// be recorded on disk when the application exits. This is a high
		/// price to pay even for a 20% performance gain.
		/// </remarks>
		public bool ImmediateFlush
		{
			get
			{
				return m_immediateFlush;
			}
			set
			{
				m_immediateFlush = value;
			}
		}

		/// <summary>
		/// Sets the <see cref="T:System.IO.TextWriter" /> where the log output will go.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The specified <see cref="T:System.IO.TextWriter" /> must be open and writable.
		/// </para>
		/// <para>
		/// The <see cref="T:System.IO.TextWriter" /> will be closed when the appender 
		/// instance is closed.
		/// </para>
		/// <para>
		/// <b>Note:</b> Logging to an unopened <see cref="T:System.IO.TextWriter" /> will fail.
		/// </para>
		/// </remarks>
		public virtual TextWriter Writer
		{
			get
			{
				return m_qtw;
			}
			set
			{
				lock (this)
				{
					Reset();
					if (value != null)
					{
						m_qtw = new QuietTextWriter(value, ErrorHandler);
						WriteHeader();
					}
				}
			}
		}

		/// <summary>
		/// Gets or set the <see cref="T:log4net.Core.IErrorHandler" /> and the underlying 
		/// <see cref="T:log4net.Util.QuietTextWriter" />, if any, for this appender. 
		/// </summary>
		/// <value>
		/// The <see cref="T:log4net.Core.IErrorHandler" /> for this appender.
		/// </value>
		public override IErrorHandler ErrorHandler
		{
			get
			{
				return base.ErrorHandler;
			}
			set
			{
				lock (this)
				{
					if (value == null)
					{
						LogLog.Warn("TextWriterAppender: You have tried to set a null error-handler.");
					}
					else
					{
						base.ErrorHandler = value;
						if (m_qtw != null)
						{
							m_qtw.ErrorHandler = value;
						}
					}
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
		/// Gets or sets the <see cref="T:log4net.Util.QuietTextWriter" /> where logging events
		/// will be written to. 
		/// </summary>
		/// <value>
		/// The <see cref="T:log4net.Util.QuietTextWriter" /> where logging events are written.
		/// </value>
		/// <remarks>
		/// <para>
		/// This is the <see cref="T:log4net.Util.QuietTextWriter" /> where logging events
		/// will be written to. 
		/// </para>
		/// </remarks>
		protected QuietTextWriter QuietWriter
		{
			get
			{
				return m_qtw;
			}
			set
			{
				m_qtw = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Appender.TextWriterAppender" /> class.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Default constructor.
		/// </para>
		/// </remarks>
		public TextWriterAppender()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Appender.TextWriterAppender" /> class and
		/// sets the output destination to a new <see cref="T:System.IO.StreamWriter" /> initialized 
		/// with the specified <see cref="T:System.IO.Stream" />.
		/// </summary>
		/// <param name="layout">The layout to use with this appender.</param>
		/// <param name="os">The <see cref="T:System.IO.Stream" /> to output to.</param>
		/// <remarks>
		/// <para>
		/// Obsolete constructor.
		/// </para>
		/// </remarks>
		[Obsolete("Instead use the default constructor and set the Layout & Writer properties")]
		public TextWriterAppender(ILayout layout, Stream os)
			: this(layout, new StreamWriter(os))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Appender.TextWriterAppender" /> class and sets
		/// the output destination to the specified <see cref="T:System.IO.StreamWriter" />.
		/// </summary>
		/// <param name="layout">The layout to use with this appender</param>
		/// <param name="writer">The <see cref="T:System.IO.TextWriter" /> to output to</param>
		/// <remarks>
		/// The <see cref="T:System.IO.TextWriter" /> must have been previously opened.
		/// </remarks>
		/// <remarks>
		/// <para>
		/// Obsolete constructor.
		/// </para>
		/// </remarks>
		[Obsolete("Instead use the default constructor and set the Layout & Writer properties")]
		public TextWriterAppender(ILayout layout, TextWriter writer)
		{
			Layout = layout;
			Writer = writer;
		}

		/// <summary>
		/// This method determines if there is a sense in attempting to append.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method checked if an output target has been set and if a
		/// layout has been set. 
		/// </para>
		/// </remarks>
		/// <returns><c>false</c> if any of the preconditions fail.</returns>
		protected override bool PreAppendCheck()
		{
			if (!base.PreAppendCheck())
			{
				return false;
			}
			if (m_qtw == null)
			{
				PrepareWriter();
				if (m_qtw == null)
				{
					ErrorHandler.Error("No output stream or file set for the appender named [" + base.Name + "].");
					return false;
				}
			}
			if (m_qtw.Closed)
			{
				ErrorHandler.Error("Output stream for appender named [" + base.Name + "] has been closed.");
				return false;
			}
			return true;
		}

		/// <summary>
		/// This method is called by the <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)" />
		/// method. 
		/// </summary>
		/// <param name="loggingEvent">The event to log.</param>
		/// <remarks>
		/// <para>
		/// Writes a log statement to the output stream if the output stream exists 
		/// and is writable.  
		/// </para>
		/// <para>
		/// The format of the output will depend on the appender's layout.
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent loggingEvent)
		{
			RenderLoggingEvent(m_qtw, loggingEvent);
			if (m_immediateFlush)
			{
				m_qtw.Flush();
			}
		}

		/// <summary>
		/// This method is called by the <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent[])" />
		/// method. 
		/// </summary>
		/// <param name="loggingEvents">The array of events to log.</param>
		/// <remarks>
		/// <para>
		/// This method writes all the bulk logged events to the output writer
		/// before flushing the stream.
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent[] loggingEvents)
		{
			foreach (LoggingEvent loggingEvent in loggingEvents)
			{
				RenderLoggingEvent(m_qtw, loggingEvent);
			}
			if (m_immediateFlush)
			{
				m_qtw.Flush();
			}
		}

		/// <summary>
		/// Close this appender instance. The underlying stream or writer is also closed.
		/// </summary>
		/// <remarks>
		/// Closed appenders cannot be reused.
		/// </remarks>
		protected override void OnClose()
		{
			lock (this)
			{
				Reset();
			}
		}

		/// <summary>
		/// Writes the footer and closes the underlying <see cref="T:System.IO.TextWriter" />.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Writes the footer and closes the underlying <see cref="T:System.IO.TextWriter" />.
		/// </para>
		/// </remarks>
		protected virtual void WriteFooterAndCloseWriter()
		{
			WriteFooter();
			CloseWriter();
		}

		/// <summary>
		/// Closes the underlying <see cref="T:System.IO.TextWriter" />.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Closes the underlying <see cref="T:System.IO.TextWriter" />.
		/// </para>
		/// </remarks>
		protected virtual void CloseWriter()
		{
			if (m_qtw != null)
			{
				try
				{
					m_qtw.Close();
				}
				catch (Exception e)
				{
					ErrorHandler.Error(string.Concat("Could not close writer [", m_qtw, "]"), e);
				}
			}
		}

		/// <summary>
		/// Clears internal references to the underlying <see cref="T:System.IO.TextWriter" /> 
		/// and other variables.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Subclasses can override this method for an alternate closing behavior.
		/// </para>
		/// </remarks>
		protected virtual void Reset()
		{
			WriteFooterAndCloseWriter();
			m_qtw = null;
		}

		/// <summary>
		/// Writes a footer as produced by the embedded layout's <see cref="P:log4net.Layout.ILayout.Footer" /> property.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Writes a footer as produced by the embedded layout's <see cref="P:log4net.Layout.ILayout.Footer" /> property.
		/// </para>
		/// </remarks>
		protected virtual void WriteFooter()
		{
			if (Layout != null && m_qtw != null && !m_qtw.Closed)
			{
				string footer = Layout.Footer;
				if (footer != null)
				{
					m_qtw.Write(footer);
				}
			}
		}

		/// <summary>
		/// Writes a header produced by the embedded layout's <see cref="P:log4net.Layout.ILayout.Header" /> property.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Writes a header produced by the embedded layout's <see cref="P:log4net.Layout.ILayout.Header" /> property.
		/// </para>
		/// </remarks>
		protected virtual void WriteHeader()
		{
			if (Layout != null && m_qtw != null && !m_qtw.Closed)
			{
				string header = Layout.Header;
				if (header != null)
				{
					m_qtw.Write(header);
				}
			}
		}

		/// <summary>
		/// Called to allow a subclass to lazily initialize the writer
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method is called when an event is logged and the <see cref="P:log4net.Appender.TextWriterAppender.Writer" /> or
		/// <see cref="P:log4net.Appender.TextWriterAppender.QuietWriter" /> have not been set. This allows a subclass to
		/// attempt to initialize the writer multiple times.
		/// </para>
		/// </remarks>
		protected virtual void PrepareWriter()
		{
		}
	}
}
