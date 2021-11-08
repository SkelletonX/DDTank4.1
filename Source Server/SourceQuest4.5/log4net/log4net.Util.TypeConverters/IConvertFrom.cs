using System;

namespace log4net.Util.TypeConverters
{
	/// <summary>
	/// Interface supported by type converters
	/// </summary>
	/// <remarks>
	/// <para>
	/// This interface supports conversion from arbitrary types
	/// to a single target type. See <see cref="T:log4net.Util.TypeConverters.TypeConverterAttribute" />.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public interface IConvertFrom
	{
		/// <summary>
		/// Can the source type be converted to the type supported by this object
		/// </summary>
		/// <param name="sourceType">the type to convert</param>
		/// <returns>true if the conversion is possible</returns>
		/// <remarks>
		/// <para>
		/// Test if the <paramref name="sourceType" /> can be converted to the
		/// type supported by this converter.
		/// </para>
		/// </remarks>
		bool CanConvertFrom(Type sourceType);

		/// <summary>
		/// Convert the source object to the type supported by this object
		/// </summary>
		/// <param name="source">the object to convert</param>
		/// <returns>the converted object</returns>
		/// <remarks>
		/// <para>
		/// Converts the <paramref name="source" /> to the type supported
		/// by this converter.
		/// </para>
		/// </remarks>
		object ConvertFrom(object source);
	}
}
