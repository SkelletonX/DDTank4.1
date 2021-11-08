using System;

namespace log4net.Util.TypeConverters
{
	/// <summary>
	/// Convert between string and <see cref="T:log4net.Util.PatternString" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// Supports conversion from string to <see cref="T:log4net.Util.PatternString" /> type, 
	/// and from a <see cref="T:log4net.Util.PatternString" /> type to a string.
	/// </para>
	/// <para>
	/// The string is used as the <see cref="P:log4net.Util.PatternString.ConversionPattern" /> 
	/// of the <see cref="T:log4net.Util.PatternString" />.
	/// </para>
	/// </remarks>
	/// <seealso cref="T:log4net.Util.TypeConverters.ConverterRegistry" />
	/// <seealso cref="T:log4net.Util.TypeConverters.IConvertFrom" />
	/// <seealso cref="T:log4net.Util.TypeConverters.IConvertTo" />
	/// <author>Nicko Cadell</author>
	internal class PatternStringConverter : IConvertTo, IConvertFrom
	{
		/// <summary>
		/// Can the target type be converted to the type supported by this object
		/// </summary>
		/// <param name="targetType">A <see cref="T:System.Type" /> that represents the type you want to convert to</param>
		/// <returns>true if the conversion is possible</returns>
		/// <remarks>
		/// <para>
		/// Returns <c>true</c> if the <paramref name="targetType" /> is
		/// assignable from a <see cref="T:System.String" /> type.
		/// </para>
		/// </remarks>
		public bool CanConvertTo(Type targetType)
		{
			return typeof(string).IsAssignableFrom(targetType);
		}

		/// <summary>
		/// Converts the given value object to the specified type, using the arguments
		/// </summary>
		/// <param name="source">the object to convert</param>
		/// <param name="targetType">The Type to convert the value parameter to</param>
		/// <returns>the converted object</returns>
		/// <remarks>
		/// <para>
		/// Uses the <see cref="M:log4net.Util.PatternString.Format" /> method to convert the
		/// <see cref="T:log4net.Util.PatternString" /> argument to a <see cref="T:System.String" />.
		/// </para>
		/// </remarks>
		/// <exception cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException">
		/// The <paramref name="source" /> object cannot be converted to the
		/// <paramref name="targetType" />. To check for this condition use the 
		/// <see cref="M:log4net.Util.TypeConverters.PatternStringConverter.CanConvertTo(System.Type)" /> method.
		/// </exception>
		public object ConvertTo(object source, Type targetType)
		{
			PatternString patternString = source as PatternString;
			if (patternString != null && CanConvertTo(targetType))
			{
				return patternString.Format();
			}
			throw ConversionNotSupportedException.Create(targetType, source);
		}

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
		/// <param name="source">the object to convert to a PatternString</param>
		/// <returns>the PatternString</returns>
		/// <remarks>
		/// <para>
		/// Creates and returns a new <see cref="T:log4net.Util.PatternString" /> using
		/// the <paramref name="source" /> <see cref="T:System.String" /> as the
		/// <see cref="P:log4net.Util.PatternString.ConversionPattern" />.
		/// </para>
		/// </remarks>
		/// <exception cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException">
		/// The <paramref name="source" /> object cannot be converted to the
		/// target type. To check for this condition use the <see cref="M:log4net.Util.TypeConverters.PatternStringConverter.CanConvertFrom(System.Type)" />
		/// method.
		/// </exception>
		public object ConvertFrom(object source)
		{
			string text = source as string;
			if (text != null)
			{
				return new PatternString(text);
			}
			throw ConversionNotSupportedException.Create(typeof(PatternString), source);
		}
	}
}
