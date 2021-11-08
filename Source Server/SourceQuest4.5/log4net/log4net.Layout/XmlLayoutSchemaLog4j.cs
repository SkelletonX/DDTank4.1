using log4net.Core;
using log4net.Util;
using System;
using System.Collections;
using System.Xml;

namespace log4net.Layout
{
	/// <summary>
	/// Layout that formats the log events as XML elements compatible with the log4j schema
	/// </summary>
	/// <remarks>
	/// <para>
	/// Formats the log events according to the http://logging.apache.org/log4j schema.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public class XmlLayoutSchemaLog4j : XmlLayoutBase
	{
		/// <summary>
		/// The 1st of January 1970 in UTC
		/// </summary>
		private static readonly DateTime s_date1970 = new DateTime(1970, 1, 1);

		/// <summary>
		/// The version of the log4j schema to use.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only version 1.2 of the log4j schema is supported.
		/// </para>
		/// </remarks>
		public string Version
		{
			get
			{
				return "1.2";
			}
			set
			{
				if (value != "1.2")
				{
					throw new ArgumentException("Only version 1.2 of the log4j schema is currently supported");
				}
			}
		}

		/// <summary>
		/// Constructs an XMLLayoutSchemaLog4j
		/// </summary>
		public XmlLayoutSchemaLog4j()
		{
		}

		/// <summary>
		/// Constructs an XMLLayoutSchemaLog4j.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <b>LocationInfo</b> option takes a boolean value. By
		/// default, it is set to false which means there will be no location
		/// information output by this layout. If the the option is set to
		/// true, then the file name and line number of the statement
		/// at the origin of the log statement will be output. 
		/// </para>
		/// <para>
		/// If you are embedding this layout within an SMTPAppender
		/// then make sure to set the <b>LocationInfo</b> option of that 
		/// appender as well.
		/// </para>
		/// </remarks>
		public XmlLayoutSchemaLog4j(bool locationInfo)
			: base(locationInfo)
		{
		}

		/// <summary>
		/// Actually do the writing of the xml
		/// </summary>
		/// <param name="writer">the writer to use</param>
		/// <param name="loggingEvent">the event to write</param>
		/// <remarks>
		/// <para>
		/// Generate XML that is compatible with the log4j schema.
		/// </para>
		/// </remarks>
		protected override void FormatXml(XmlWriter writer, LoggingEvent loggingEvent)
		{
			if (loggingEvent.LookupProperty("log4net:HostName") != null && loggingEvent.LookupProperty("log4jmachinename") == null)
			{
				loggingEvent.GetProperties()["log4jmachinename"] = loggingEvent.LookupProperty("log4net:HostName");
			}
			if (loggingEvent.LookupProperty("log4japp") == null && loggingEvent.Domain != null && loggingEvent.Domain.Length > 0)
			{
				loggingEvent.GetProperties()["log4japp"] = loggingEvent.Domain;
			}
			if (loggingEvent.Identity != null && loggingEvent.Identity.Length > 0 && loggingEvent.LookupProperty("log4net:Identity") == null)
			{
				loggingEvent.GetProperties()["log4net:Identity"] = loggingEvent.Identity;
			}
			if (loggingEvent.UserName != null && loggingEvent.UserName.Length > 0 && loggingEvent.LookupProperty("log4net:UserName") == null)
			{
				loggingEvent.GetProperties()["log4net:UserName"] = loggingEvent.UserName;
			}
			writer.WriteStartElement("log4j:event");
			writer.WriteAttributeString("logger", loggingEvent.LoggerName);
			writer.WriteAttributeString("timestamp", XmlConvert.ToString((long)(loggingEvent.TimeStamp.ToUniversalTime() - s_date1970).TotalMilliseconds));
			writer.WriteAttributeString("level", loggingEvent.Level.DisplayName);
			writer.WriteAttributeString("thread", loggingEvent.ThreadName);
			writer.WriteStartElement("log4j:message");
			Transform.WriteEscapedXmlString(writer, loggingEvent.RenderedMessage, base.InvalidCharReplacement);
			writer.WriteEndElement();
			object obj = loggingEvent.LookupProperty("NDC");
			if (obj != null)
			{
				string text = loggingEvent.Repository.RendererMap.FindAndRender(obj);
				if (text != null && text.Length > 0)
				{
					writer.WriteStartElement("log4j:NDC");
					Transform.WriteEscapedXmlString(writer, text, base.InvalidCharReplacement);
					writer.WriteEndElement();
				}
			}
			PropertiesDictionary properties = loggingEvent.GetProperties();
			if (properties.Count > 0)
			{
				writer.WriteStartElement("log4j:properties");
				foreach (DictionaryEntry item in (IEnumerable)properties)
				{
					writer.WriteStartElement("log4j:data");
					writer.WriteAttributeString("name", (string)item.Key);
					string text = loggingEvent.Repository.RendererMap.FindAndRender(item.Value);
					writer.WriteAttributeString("value", text);
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			string exceptionString = loggingEvent.GetExceptionString();
			if (exceptionString != null && exceptionString.Length > 0)
			{
				writer.WriteStartElement("log4j:throwable");
				Transform.WriteEscapedXmlString(writer, exceptionString, base.InvalidCharReplacement);
				writer.WriteEndElement();
			}
			if (base.LocationInfo)
			{
				LocationInfo locationInformation = loggingEvent.LocationInformation;
				writer.WriteStartElement("log4j:locationInfo");
				writer.WriteAttributeString("class", locationInformation.ClassName);
				writer.WriteAttributeString("method", locationInformation.MethodName);
				writer.WriteAttributeString("file", locationInformation.FileName);
				writer.WriteAttributeString("line", locationInformation.LineNumber);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}
	}
}
