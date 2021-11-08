using System;
using System.Text;

namespace log4net.Util.TypeConverters
{
	/// <summary>
	/// Supports conversion from string to <see cref="T:System.Text.Encoding" /> type.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Supports conversion from string to <see cref="T:System.Text.Encoding" /> type.
	/// </para>
	/// </remarks>
	/// <seealso cref="T:log4net.Util.TypeConverters.ConverterRegistry" />
	/// <seealso cref="T:log4net.Util.TypeConverters.IConvertFrom" />
	/// <seealso cref="T:log4net.Util.TypeConverters.IConvertTo" />
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	internal class EncodingConverter : IConvertFrom
	{
		/// <summary>
		/// Can the source type be converted to the type supported by this object
		/// </summary>
		/// <param name="sourceType">the type to convert</param>
		/// <returns>true if the conversion is possible</returns>
		/// <remarks>
		/// <para>
		/// Returns <c>true</c> if the <paramref name="sourceType" /> is
		/// the <see cref="T:System.String" /> type.
		/// </para>
		/// </remarks>
		public bool CanConvertFrom(Type sourceType)
		{
			return (object)sourceType == typeof(string);
		}

		/// <summary>
		/// Overrides the ConvertFrom method of IConvertFrom.
		/// </summary>
		/// <param name="source">the object to convert to an encoding</param>
		/// <returns>the encoding</returns>
		/// <remarks>
		/// <para>
		/// Uses the <see cref="M:System.Text.Encoding.GetEncoding(System.String)" /> method to 
		/// convert the <see cref="T:System.String" /> argument to an <see cref="T:System.Text.Encoding" />.
		/// </para>
		/// </remarks>
		/// <exception cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException">
		/// The <paramref name="source" /> object cannot be converted to the
		/// target type. To check for this condition use the <see cref="M:log4net.Util.TypeConverters.EncodingConverter.CanConvertFrom(System.Type)" />
		/// method.
		/// </exception>
		public object ConvertFrom(object source)
		{
			string text = source as string;
			if (text != null)
			{
				return Encoding.GetEncoding(text);
			}
			throw ConversionNotSupportedException.Create(typeof(Encoding), source);
		}
	}
}
