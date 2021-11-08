using System;

namespace log4net.Util.TypeConverters
{
	/// <summary>
	/// Supports conversion from string to <see cref="T:System.Type" /> type.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Supports conversion from string to <see cref="T:System.Type" /> type.
	/// </para>
	/// </remarks>
	/// <seealso cref="T:log4net.Util.TypeConverters.ConverterRegistry" />
	/// <seealso cref="T:log4net.Util.TypeConverters.IConvertFrom" />
	/// <seealso cref="T:log4net.Util.TypeConverters.IConvertTo" />
	/// <author>Nicko Cadell</author>
	internal class TypeConverter : IConvertFrom
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
		/// <param name="source">the object to convert to a Type</param>
		/// <returns>the Type</returns>
		/// <remarks>
		/// <para>
		/// Uses the <see cref="M:System.Type.GetType(System.String,System.Boolean)" /> method to convert the
		/// <see cref="T:System.String" /> argument to a <see cref="T:System.Type" />.
		/// Additional effort is made to locate partially specified types
		/// by searching the loaded assemblies.
		/// </para>
		/// </remarks>
		/// <exception cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException">
		/// The <paramref name="source" /> object cannot be converted to the
		/// target type. To check for this condition use the <see cref="M:log4net.Util.TypeConverters.TypeConverter.CanConvertFrom(System.Type)" />
		/// method.
		/// </exception>
		public object ConvertFrom(object source)
		{
			string text = source as string;
			if (text != null)
			{
				return SystemInfo.GetTypeFromString(text, throwOnError: true, ignoreCase: true);
			}
			throw ConversionNotSupportedException.Create(typeof(Type), source);
		}
	}
}
