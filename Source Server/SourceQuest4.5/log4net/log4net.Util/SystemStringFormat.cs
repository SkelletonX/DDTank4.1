using System;
using System.Text;

namespace log4net.Util
{
	/// <summary>
	/// Utility class that represents a format string.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Utility class that represents a format string.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public sealed class SystemStringFormat
	{
		private readonly IFormatProvider m_provider;

		private readonly string m_format;

		private readonly object[] m_args;

		/// <summary>
		/// Initialise the <see cref="T:log4net.Util.SystemStringFormat" />
		/// </summary>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information.</param>
		/// <param name="format">A <see cref="T:System.String" /> containing zero or more format items.</param>
		/// <param name="args">An <see cref="T:System.Object" /> array containing zero or more objects to format.</param>
		public SystemStringFormat(IFormatProvider provider, string format, params object[] args)
		{
			m_provider = provider;
			m_format = format;
			m_args = args;
		}

		/// <summary>
		/// Format the string and arguments
		/// </summary>
		/// <returns>the formatted string</returns>
		public override string ToString()
		{
			return StringFormat(m_provider, m_format, m_args);
		}

		/// <summary>
		/// Replaces the format item in a specified <see cref="T:System.String" /> with the text equivalent 
		/// of the value of a corresponding <see cref="T:System.Object" /> instance in a specified array.
		/// A specified parameter supplies culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific formatting information.</param>
		/// <param name="format">A <see cref="T:System.String" /> containing zero or more format items.</param>
		/// <param name="args">An <see cref="T:System.Object" /> array containing zero or more objects to format.</param>
		/// <returns>
		/// A copy of format in which the format items have been replaced by the <see cref="T:System.String" /> 
		/// equivalent of the corresponding instances of <see cref="T:System.Object" /> in args.
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method does not throw exceptions. If an exception thrown while formatting the result the
		/// exception and arguments are returned in the result string.
		/// </para>
		/// </remarks>
		private static string StringFormat(IFormatProvider provider, string format, params object[] args)
		{
			try
			{
				if (format == null)
				{
					return null;
				}
				if (args == null)
				{
					return format;
				}
				return string.Format(provider, format, args);
			}
			catch (Exception ex)
			{
				LogLog.Warn("StringFormat: Exception while rendering format [" + format + "]", ex);
				return StringFormatError(ex, format, args);
			}
			catch
			{
				LogLog.Warn("StringFormat: Exception while rendering format [" + format + "]");
				return StringFormatError(null, format, args);
			}
		}

		/// <summary>
		/// Process an error during StringFormat
		/// </summary>
		private static string StringFormatError(Exception formatException, string format, object[] args)
		{
			try
			{
				StringBuilder stringBuilder = new StringBuilder("<log4net.Error>");
				if (formatException != null)
				{
					stringBuilder.Append("Exception during StringFormat: ").Append(formatException.Message);
				}
				else
				{
					stringBuilder.Append("Exception during StringFormat");
				}
				stringBuilder.Append(" <format>").Append(format).Append("</format>");
				stringBuilder.Append("<args>");
				RenderArray(args, stringBuilder);
				stringBuilder.Append("</args>");
				stringBuilder.Append("</log4net.Error>");
				return stringBuilder.ToString();
			}
			catch (Exception exception)
			{
				LogLog.Error("StringFormat: INTERNAL ERROR during StringFormat error handling", exception);
				return "<log4net.Error>Exception during StringFormat. See Internal Log.</log4net.Error>";
			}
			catch
			{
				LogLog.Error("StringFormat: INTERNAL ERROR during StringFormat error handling");
				return "<log4net.Error>Exception during StringFormat. See Internal Log.</log4net.Error>";
			}
		}

		/// <summary>
		/// Dump the contents of an array into a string builder
		/// </summary>
		private static void RenderArray(Array array, StringBuilder buffer)
		{
			if (array == null)
			{
				buffer.Append(SystemInfo.NullText);
				return;
			}
			if (array.Rank != 1)
			{
				buffer.Append(array.ToString());
				return;
			}
			buffer.Append("{");
			int length = array.Length;
			if (length > 0)
			{
				RenderObject(array.GetValue(0), buffer);
				for (int i = 1; i < length; i++)
				{
					buffer.Append(", ");
					RenderObject(array.GetValue(i), buffer);
				}
			}
			buffer.Append("}");
		}

		/// <summary>
		/// Dump an object to a string
		/// </summary>
		private static void RenderObject(object obj, StringBuilder buffer)
		{
			if (obj == null)
			{
				buffer.Append(SystemInfo.NullText);
			}
			else
			{
				try
				{
					buffer.Append(obj);
				}
				catch (Exception ex)
				{
					buffer.Append("<Exception: ").Append(ex.Message).Append(">");
				}
				catch
				{
					buffer.Append("<Exception>");
				}
			}
		}
	}
}
