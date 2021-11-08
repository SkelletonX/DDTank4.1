using System.Text.RegularExpressions;
using System.Xml;

namespace log4net.Util
{
	/// <summary>
	/// Utility class for transforming strings.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Utility class for transforming strings.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public sealed class Transform
	{
		private const string CDATA_END = "]]>";

		private const string CDATA_UNESCAPABLE_TOKEN = "]]";

		private static Regex INVALIDCHARS = new Regex("[^\\x09\\x0A\\x0D\\x20-\\xFF\\u00FF-\\u07FF\\uE000-\\uFFFD]", RegexOptions.Compiled);

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Util.Transform" /> class. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// Uses a private access modifier to prevent instantiation of this class.
		/// </para>
		/// </remarks>
		private Transform()
		{
		}

		/// <summary>
		/// Write a string to an <see cref="T:System.Xml.XmlWriter" />
		/// </summary>
		/// <param name="writer">the writer to write to</param>
		/// <param name="textData">the string to write</param>
		/// <param name="invalidCharReplacement">The string to replace non XML compliant chars with</param>
		/// <remarks>
		/// <para>
		/// The test is escaped either using XML escape entities
		/// or using CDATA sections.
		/// </para>
		/// </remarks>
		public static void WriteEscapedXmlString(XmlWriter writer, string textData, string invalidCharReplacement)
		{
			string text = MaskXmlInvalidCharacters(textData, invalidCharReplacement);
			int num = 12 * (1 + CountSubstrings(text, "]]>"));
			int num2 = 3 * (CountSubstrings(text, "<") + CountSubstrings(text, ">")) + 4 * CountSubstrings(text, "&");
			if (num2 <= num)
			{
				writer.WriteString(text);
				return;
			}
			int num3 = text.IndexOf("]]>");
			if (num3 < 0)
			{
				writer.WriteCData(text);
				return;
			}
			int num4 = 0;
			while (num3 > -1)
			{
				writer.WriteCData(text.Substring(num4, num3 - num4));
				if (num3 == text.Length - 3)
				{
					num4 = text.Length;
					writer.WriteString("]]>");
					break;
				}
				writer.WriteString("]]");
				num4 = num3 + 2;
				num3 = text.IndexOf("]]>", num4);
			}
			if (num4 < text.Length)
			{
				writer.WriteCData(text.Substring(num4));
			}
		}

		/// <summary>
		/// Replace invalid XML characters in text string
		/// </summary>
		/// <param name="textData">the XML text input string</param>
		/// <param name="mask">the string to use in place of invalid characters</param>
		/// <returns>A string that does not contain invalid XML characters.</returns>
		/// <remarks>
		/// <para>
		/// Certain Unicode code points are not allowed in the XML InfoSet, for
		/// details see: <a href="http://www.w3.org/TR/REC-xml/#charsets">http://www.w3.org/TR/REC-xml/#charsets</a>.
		/// </para>
		/// <para>
		/// This method replaces any illegal characters in the input string
		/// with the mask string specified.
		/// </para>
		/// </remarks>
		public static string MaskXmlInvalidCharacters(string textData, string mask)
		{
			return INVALIDCHARS.Replace(textData, mask);
		}

		/// <summary>
		/// Count the number of times that the substring occurs in the text
		/// </summary>
		/// <param name="text">the text to search</param>
		/// <param name="substring">the substring to find</param>
		/// <returns>the number of times the substring occurs in the text</returns>
		/// <remarks>
		/// <para>
		/// The substring is assumed to be non repeating within itself.
		/// </para>
		/// </remarks>
		private static int CountSubstrings(string text, string substring)
		{
			int num = 0;
			int num2 = 0;
			int length = text.Length;
			int length2 = substring.Length;
			if (length == 0)
			{
				return 0;
			}
			if (length2 == 0)
			{
				return 0;
			}
			while (num2 < length)
			{
				int num3 = text.IndexOf(substring, num2);
				if (num3 == -1)
				{
					break;
				}
				num++;
				num2 = num3 + length2;
			}
			return num;
		}
	}
}
