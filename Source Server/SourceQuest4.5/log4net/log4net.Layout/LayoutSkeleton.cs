using log4net.Core;
using System.IO;

namespace log4net.Layout
{
	/// <summary>
	/// Extend this abstract class to create your own log layout format.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is the base implementation of the <see cref="T:log4net.Layout.ILayout" />
	/// interface. Most layout objects should extend this class.
	/// </para>
	/// </remarks>
	/// <remarks>
	/// <note type="inheritinfo">
	/// <para>
	/// Subclasses must implement the <see cref="M:log4net.Layout.LayoutSkeleton.Format(System.IO.TextWriter,log4net.Core.LoggingEvent)" />
	/// method.
	/// </para>
	/// <para>
	/// Subclasses should set the <see cref="P:log4net.Layout.LayoutSkeleton.IgnoresException" /> in their default
	/// constructor.
	/// </para>
	/// </note>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public abstract class LayoutSkeleton : ILayout, IOptionHandler
	{
		/// <summary>
		/// The header text
		/// </summary>
		/// <remarks>
		/// <para>
		/// See <see cref="P:log4net.Layout.LayoutSkeleton.Header" /> for more information.
		/// </para>
		/// </remarks>
		private string m_header = null;

		/// <summary>
		/// The footer text
		/// </summary>
		/// <remarks>
		/// <para>
		/// See <see cref="P:log4net.Layout.LayoutSkeleton.Footer" /> for more information.
		/// </para>
		/// </remarks>
		private string m_footer = null;

		/// <summary>
		/// Flag indicating if this layout handles exceptions
		/// </summary>
		/// <remarks>
		/// <para>
		/// <c>false</c> if this layout handles exceptions
		/// </para>
		/// </remarks>
		private bool m_ignoresException = true;

		/// <summary>
		/// The content type output by this layout. 
		/// </summary>
		/// <value>The content type is <c>"text/plain"</c></value>
		/// <remarks>
		/// <para>
		/// The content type output by this layout.
		/// </para>
		/// <para>
		/// This base class uses the value <c>"text/plain"</c>.
		/// To change this value a subclass must override this
		/// property.
		/// </para>
		/// </remarks>
		public virtual string ContentType => "text/plain";

		/// <summary>
		/// The header for the layout format.
		/// </summary>
		/// <value>the layout header</value>
		/// <remarks>
		/// <para>
		/// The Header text will be appended before any logging events
		/// are formatted and appended.
		/// </para>
		/// </remarks>
		public virtual string Header
		{
			get
			{
				return m_header;
			}
			set
			{
				m_header = value;
			}
		}

		/// <summary>
		/// The footer for the layout format.
		/// </summary>
		/// <value>the layout footer</value>
		/// <remarks>
		/// <para>
		/// The Footer text will be appended after all the logging events
		/// have been formatted and appended.
		/// </para>
		/// </remarks>
		public virtual string Footer
		{
			get
			{
				return m_footer;
			}
			set
			{
				m_footer = value;
			}
		}

		/// <summary>
		/// Flag indicating if this layout handles exceptions
		/// </summary>
		/// <value><c>false</c> if this layout handles exceptions</value>
		/// <remarks>
		/// <para>
		/// If this layout handles the exception object contained within
		/// <see cref="T:log4net.Core.LoggingEvent" />, then the layout should return
		/// <c>false</c>. Otherwise, if the layout ignores the exception
		/// object, then the layout should return <c>true</c>.
		/// </para>
		/// <para>
		/// Set this value to override a this default setting. The default
		/// value is <c>true</c>, this layout does not handle the exception.
		/// </para>
		/// </remarks>
		public virtual bool IgnoresException
		{
			get
			{
				return m_ignoresException;
			}
			set
			{
				m_ignoresException = value;
			}
		}

		/// <summary>
		/// Activate component options
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Layout.LayoutSkeleton.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Layout.LayoutSkeleton.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Layout.LayoutSkeleton.ActivateOptions" /> must be called again.
		/// </para>
		/// <para>
		/// This method must be implemented by the subclass.
		/// </para>
		/// </remarks>
		public abstract void ActivateOptions();

		/// <summary>
		/// Implement this method to create your own layout format.
		/// </summary>
		/// <param name="writer">The TextWriter to write the formatted event to</param>
		/// <param name="loggingEvent">The event to format</param>
		/// <remarks>
		/// <para>
		/// This method is called by an appender to format
		/// the <paramref name="loggingEvent" /> as text.
		/// </para>
		/// </remarks>
		public abstract void Format(TextWriter writer, LoggingEvent loggingEvent);
	}
}
