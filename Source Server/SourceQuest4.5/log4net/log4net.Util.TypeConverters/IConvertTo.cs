using System;

namespace log4net.Util.TypeConverters
{
	/// <summary>
	/// Interface supported by type converters
	/// </summary>
	/// <remarks>
	/// <para>
	/// This interface supports conversion from a single type to arbitrary types.
	/// See <see cref="T:log4net.Util.TypeConverters.TypeConverterAttribute" />.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public interface IConvertTo
	{
		/// <summary>
		/// Returns whether this converter can convert the object to the specified type
		/// </summary>
		/// <param name="targetType">A Type that represents the type you want to convert to</param>
		/// <returns>true if the conversion is possible</returns>
		/// <remarks>
		/// <para>
		/// Test if the type supported by this converter can be converted to the
		/// <paramref name="targetType" />.
		/// </para>
		/// </remarks>
		bool CanConvertTo(Type targetType);

		/// <summary>
		/// Converts the given value object to the specified type, using the arguments
		/// </summary>
		/// <param name="source">the object to convert</param>
		/// <param name="targetType">The Type to convert the value parameter to</param>
		/// <returns>the converted object</returns>
		/// <remarks>
		/// <para>
		/// Converts the <paramref name="source" /> (which must be of the type supported
		/// by this converter) to the <paramref name="targetType" /> specified..
		/// </para>
		/// </remarks>
		object ConvertTo(object source, Type targetType);
	}
}
