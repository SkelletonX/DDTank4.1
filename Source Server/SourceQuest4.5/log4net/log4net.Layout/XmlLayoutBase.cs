using log4net.Core;
using log4net.Util;
using System;
using System.IO;
using System.Xml;

namespace log4net.Layout
{
	/// <summary>
	/// Layout that formats the log events as XML elements.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is an abstract class that must be subclassed by an implementation 
	/// to conform to a specific schema.
	/// </para>
	/// <para>
	/// Deriving classes must implement the <see cref="M:log4net.Layout.XmlLayoutBase.FormatXml(System.Xml.XmlWriter,log4net.Core.LoggingEvent)" /> method.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public abstract class XmlLayoutBase : LayoutSkeleton
	{
		/// <summary>
		/// Flag to indicate if location information should be included in
		/// the XML events.
		/// </summary>
		private bool m_locationInfo = false;

		/// <summary>
		/// Writer adapter that ignores Close
		/// </summary>
		private readonly ProtectCloseTextWriter m_protectCloseTextWriter = new ProtectCloseTextWriter(null);

		/// <summary>
		/// The string to replace invalid chars with
		/// </summary>
		private string m_invalidCharReplacement = "?";

		/// <summary>
		/// Gets a value indicating whether to include location information in 
		/// the XML events.
		/// </summary>
		/// <value>
		/// <c>true</c> if location information should be included in the XML 
		/// events; otherwise, <c>false</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// If <see cref="P:log4net.Layout.XmlLayoutBase.LocationInfo" /> is set to <c>true</c>, then the file 
		/// name and line number of the statement at the origin of the log 
		/// statement will be output. 
		/// </para>
		/// <para>
		/// If you are embedding this layout within an <c>SMTPAppender</c>
		/// then make sure to set the <b>LocationInfo</b> option of that 
		/// appender as well.
		/// </para>
		/// </remarks>
		public bool LocationInfo
		{
			get
			{
				return m_locationInfo;
			}
			set
			{
				m_locationInfo = value;
			}
		}

		/// <summary>
		/// The string to replace characters that can not be expressed in XML with.
		/// <remarks>
		/// <para>
		/// Not all characters may be expressed in XML. This property contains the
		/// string to replace those that can not with. This defaults to a ?. Set it
		/// to the empty string to simply remove offending characters. For more
		/// details on the allowed character ranges see http://www.w3.org/TR/REC-xml/#charsets
		/// Character replacement will occur in  the log message, the property names 
		/// and the property values.
		/// </para>
		/// </remarks>
		/// </summary>
		public string InvalidCharReplacement
		{
			get
			{
				return m_invalidCharReplacement;
			}
			set
			{
				m_invalidCharReplacement = value;
			}
		}

		/// <summary>
		/// Gets the content type output by this layout. 
		/// </summary>
		/// <value>
		/// As this is the XML layout, the value is always <c>"text/xml"</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// As this is the XML layout, the value is always <c>"text/xml"</c>.
		/// </para>
		/// </remarks>
		public override string ContentType => "text/xml";

		/// <summary>
		/// Protected constructor to support subclasses
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Layout.XmlLayoutBase" /> class
		/// with no location info.
		/// </para>
		/// </remarks>
		protected XmlLayoutBase()
			: this(locationInfo: false)
		{
			IgnoresException = false;
		}

		/// <summary>
		/// Protected constructor to support subclasses
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <paramref name="locationInfo" /> parameter determines whether 
		/// location information will be output by the layout. If 
		/// <paramref name="locationInfo" /> is set to <c>true</c>, then the 
		/// file name and line number of the statement at the origin of the log 
		/// statement will be output. 
		/// </para>
		/// <para>
		/// If you are embedding this layout within an SMTPAppender
		/// then make sure to set the <b>LocationInfo</b> option of that 
		/// appender as well.
		/// </para>
		/// </remarks>
		protected XmlLayoutBase(bool locationInfo)
		{
			IgnoresException = false;
			m_locationInfo = locationInfo;
		}

		/// <summary>
		/// Initialize layout options
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Layout.XmlLayoutBase.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Layout.XmlLayoutBase.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Layout.XmlLayoutBase.ActivateOptions" /> must be called again.
		/// </para>
		/// </remarks>
		public override void ActivateOptions()
		{
		}

		/// <summary>
		/// Produces a formatted string.
		/// </summary>
		/// <param name="loggingEvent">The event being logged.</param>
		/// <param name="writer">The TextWriter to write the formatted event to</param>
		/// <remarks>
		/// <para>
		/// Format the <see cref="T:log4net.Core.LoggingEvent" /> and write it to the <see cref="T:System.IO.TextWriter" />.
		/// </para>
		/// <para>
		/// This method creates an <see cref="T:System.Xml.XmlTextWriter" /> that writes to the
		/// <paramref name="writer" />. The <see cref="T:System.Xml.XmlTextWriter" /> is passed 
		/// to the <see cref="M:log4net.Layout.XmlLayoutBase.FormatXml(System.Xml.XmlWriter,log4net.Core.LoggingEvent)" /> method. Subclasses should override the
		/// <see cref="M:log4net.Layout.XmlLayoutBase.FormatXml(System.Xml.XmlWriter,log4net.Core.LoggingEvent)" /> method rather than this method.
		/// </para>
		/// </remarks>
		public override void Format(TextWriter writer, LoggingEvent loggingEvent)
		{
			if (loggingEvent == null)
			{
				throw new ArgumentNullException("loggingEvent");
			}
			m_protectCloseTextWriter.Attach(writer);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(m_protectCloseTextWriter);
			xmlTextWriter.Formatting = Formatting.None;
			xmlTextWriter.Namespaces = false;
			FormatXml(xmlTextWriter, loggingEvent);
			xmlTextWriter.WriteWhitespace(SystemInfo.NewLine);
			xmlTextWriter.Close();
			m_protectCloseTextWriter.Attach(null);
		}

		/// <summary>
		/// Does the actual writing of the XML.
		/// </summary>
		/// <param name="writer">The writer to use to output the event to.</param>
		/// <param name="loggingEvent">The event to write.</param>
		/// <remarks>
		/// <para>
		/// Subclasses should override this method to format
		/// the <see cref="T:log4net.Core.LoggingEvent" /> as XML.
		/// </para>
		/// </remarks>
		protected abstract void FormatXml(XmlWriter writer, LoggingEvent loggingEvent);
	}
}
