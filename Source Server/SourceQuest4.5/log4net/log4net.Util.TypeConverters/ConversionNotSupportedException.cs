using System;
using System.Runtime.Serialization;

namespace log4net.Util.TypeConverters
{
	/// <summary>
	/// Exception base type for conversion errors.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This type extends <see cref="T:System.ApplicationException" />. It
	/// does not add any new functionality but does differentiate the
	/// type of exception being thrown.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	[Serializable]
	public class ConversionNotSupportedException : ApplicationException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException" /> class.
		/// </para>
		/// </remarks>
		public ConversionNotSupportedException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">A message to include with the exception.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException" /> class
		/// with the specified message.
		/// </para>
		/// </remarks>
		public ConversionNotSupportedException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">A message to include with the exception.</param>
		/// <param name="innerException">A nested exception to include.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException" /> class
		/// with the specified message and inner exception.
		/// </para>
		/// </remarks>
		public ConversionNotSupportedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Serialization constructor
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException" /> class 
		/// with serialized data.
		/// </para>
		/// </remarks>
		protected ConversionNotSupportedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException" /> class.
		/// </summary>
		/// <param name="destinationType">The conversion destination type.</param>
		/// <param name="sourceValue">The value to convert.</param>
		/// <returns>An instance of the <see cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException" />.</returns>
		/// <remarks>
		/// <para>
		/// Creates a new instance of the <see cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException" /> class.
		/// </para>
		/// </remarks>
		public static ConversionNotSupportedException Create(Type destinationType, object sourceValue)
		{
			return Create(destinationType, sourceValue, null);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException" /> class.
		/// </summary>
		/// <param name="destinationType">The conversion destination type.</param>
		/// <param name="sourceValue">The value to convert.</param>
		/// <param name="innerException">A nested exception to include.</param>
		/// <returns>An instance of the <see cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException" />.</returns>
		/// <remarks>
		/// <para>
		/// Creates a new instance of the <see cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException" /> class.
		/// </para>
		/// </remarks>
		public static ConversionNotSupportedException Create(Type destinationType, object sourceValue, Exception innerException)
		{
			if (sourceValue == null)
			{
				return new ConversionNotSupportedException(string.Concat("Cannot convert value [null] to type [", destinationType, "]"), innerException);
			}
			return new ConversionNotSupportedException(string.Concat("Cannot convert from type [", sourceValue.GetType(), "] value [", sourceValue, "] to type [", destinationType, "]"), innerException);
		}
	}
}
