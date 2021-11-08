using log4net.Layout;
using System;

namespace log4net.Util.TypeConverters
{
	/// <summary>
	/// Supports conversion from string to <see cref="T:log4net.Layout.PatternLayout" /> type.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Supports conversion from string to <see cref="T:log4net.Layout.PatternLayout" /> type.
	/// </para>
	/// <para>
	/// The string is used as the <see cref="P:log4net.Layout.PatternLayout.ConversionPattern" /> 
	/// of the <see cref="T:log4net.Layout.PatternLayout" />.
	/// </para>
	/// </remarks>
	/// <seealso cref="T:log4net.Util.TypeConverters.ConverterRegistry" />
	/// <seealso cref="T:log4net.Util.TypeConverters.IConvertFrom" />
	/// <seealso cref="T:log4net.Util.TypeConverters.IConvertTo" />
	/// <author>Nicko Cadell</author>
	internal class PatternLayoutConverter : IConvertFrom
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
		/// <param name="source">the object to convert to a PatternLayout</param>
		/// <returns>the PatternLayout</returns>
		/// <remarks>
		/// <para>
		/// Creates and returns a new <see cref="T:log4net.Layout.PatternLayout" /> using
		/// the <paramref name="source" /> <see cref="T:System.String" /> as the
		/// <see cref="P:log4net.Layout.PatternLayout.ConversionPattern" />.
		/// </para>
		/// </remarks>
		/// <exception cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException">
		/// The <paramref name="source" /> object cannot be converted to the
		/// target type. To check for this condition use the <see cref="M:log4net.Util.TypeConverters.PatternLayoutConverter.CanConvertFrom(System.Type)" />
		/// method.
		/// </exception>
		public object ConvertFrom(object source)
		{
			string text = source as string;
			if (text != null)
			{
				return new PatternLayout(text);
			}
			throw ConversionNotSupportedException.Create(typeof(PatternLayout), source);
		}
	}
}
