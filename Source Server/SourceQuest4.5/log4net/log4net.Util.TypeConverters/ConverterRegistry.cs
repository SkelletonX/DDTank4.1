using log4net.Layout;
using System;
using System.Collections;
using System.Net;
using System.Text;

namespace log4net.Util.TypeConverters
{
	/// <summary>
	/// Register of type converters for specific types.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Maintains a registry of type converters used to convert between
	/// types.
	/// </para>
	/// <para>
	/// Use the <see cref="M:log4net.Util.TypeConverters.ConverterRegistry.AddConverter(System.Type,System.Object)" /> and 
	/// <see cref="M:log4net.Util.TypeConverters.ConverterRegistry.AddConverter(System.Type,System.Type)" /> methods to register new converters.
	/// The <see cref="M:log4net.Util.TypeConverters.ConverterRegistry.GetConvertTo(System.Type,System.Type)" /> and <see cref="M:log4net.Util.TypeConverters.ConverterRegistry.GetConvertFrom(System.Type)" /> methods
	/// lookup appropriate converters to use.
	/// </para>
	/// </remarks>
	/// <seealso cref="T:log4net.Util.TypeConverters.IConvertFrom" />
	/// <seealso cref="T:log4net.Util.TypeConverters.IConvertTo" />
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public sealed class ConverterRegistry
	{
		/// <summary>
		/// Mapping from <see cref="T:System.Type" /> to type converter.
		/// </summary>
		private static Hashtable s_type2converter;

		/// <summary>
		/// Private constructor
		/// </summary>
		/// <remarks>
		/// Initializes a new instance of the <see cref="T:log4net.Util.TypeConverters.ConverterRegistry" /> class.
		/// </remarks>
		private ConverterRegistry()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This constructor defines the intrinsic type converters.
		/// </para>
		/// </remarks>
		static ConverterRegistry()
		{
			s_type2converter = new Hashtable();
			AddConverter(typeof(bool), typeof(BooleanConverter));
			AddConverter(typeof(Encoding), typeof(EncodingConverter));
			AddConverter(typeof(Type), typeof(TypeConverter));
			AddConverter(typeof(PatternLayout), typeof(PatternLayoutConverter));
			AddConverter(typeof(PatternString), typeof(PatternStringConverter));
			AddConverter(typeof(IPAddress), typeof(IPAddressConverter));
		}

		/// <summary>
		/// Adds a converter for a specific type.
		/// </summary>
		/// <param name="destinationType">The type being converted to.</param>
		/// <param name="converter">The type converter to use to convert to the destination type.</param>
		/// <remarks>
		/// <para>
		/// Adds a converter instance for a specific type.
		/// </para>
		/// </remarks>
		public static void AddConverter(Type destinationType, object converter)
		{
			if ((object)destinationType != null && converter != null)
			{
				lock (s_type2converter)
				{
					s_type2converter[destinationType] = converter;
				}
			}
		}

		/// <summary>
		/// Adds a converter for a specific type.
		/// </summary>
		/// <param name="destinationType">The type being converted to.</param>
		/// <param name="converterType">The type of the type converter to use to convert to the destination type.</param>
		/// <remarks>
		/// <para>
		/// Adds a converter <see cref="T:System.Type" /> for a specific type.
		/// </para>
		/// </remarks>
		public static void AddConverter(Type destinationType, Type converterType)
		{
			AddConverter(destinationType, CreateConverterInstance(converterType));
		}

		/// <summary>
		/// Gets the type converter to use to convert values to the destination type.
		/// </summary>
		/// <param name="sourceType">The type being converted from.</param>
		/// <param name="destinationType">The type being converted to.</param>
		/// <returns>
		/// The type converter instance to use for type conversions or <c>null</c> 
		/// if no type converter is found.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Gets the type converter to use to convert values to the destination type.
		/// </para>
		/// </remarks>
		public static IConvertTo GetConvertTo(Type sourceType, Type destinationType)
		{
			lock (s_type2converter)
			{
				IConvertTo convertTo = s_type2converter[sourceType] as IConvertTo;
				if (convertTo == null)
				{
					convertTo = (GetConverterFromAttribute(sourceType) as IConvertTo);
					if (convertTo != null)
					{
						s_type2converter[sourceType] = convertTo;
					}
				}
				return convertTo;
			}
		}

		/// <summary>
		/// Gets the type converter to use to convert values to the destination type.
		/// </summary>
		/// <param name="destinationType">The type being converted to.</param>
		/// <returns>
		/// The type converter instance to use for type conversions or <c>null</c> 
		/// if no type converter is found.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Gets the type converter to use to convert values to the destination type.
		/// </para>
		/// </remarks>
		public static IConvertFrom GetConvertFrom(Type destinationType)
		{
			lock (s_type2converter)
			{
				IConvertFrom convertFrom = s_type2converter[destinationType] as IConvertFrom;
				if (convertFrom == null)
				{
					convertFrom = (GetConverterFromAttribute(destinationType) as IConvertFrom);
					if (convertFrom != null)
					{
						s_type2converter[destinationType] = convertFrom;
					}
				}
				return convertFrom;
			}
		}

		/// <summary>
		/// Lookups the type converter to use as specified by the attributes on the 
		/// destination type.
		/// </summary>
		/// <param name="destinationType">The type being converted to.</param>
		/// <returns>
		/// The type converter instance to use for type conversions or <c>null</c> 
		/// if no type converter is found.
		/// </returns>
		private static object GetConverterFromAttribute(Type destinationType)
		{
			object[] customAttributes = destinationType.GetCustomAttributes(typeof(TypeConverterAttribute), inherit: true);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				TypeConverterAttribute typeConverterAttribute = customAttributes[0] as TypeConverterAttribute;
				if (typeConverterAttribute != null)
				{
					Type typeFromString = SystemInfo.GetTypeFromString(destinationType, typeConverterAttribute.ConverterTypeName, throwOnError: false, ignoreCase: true);
					return CreateConverterInstance(typeFromString);
				}
			}
			return null;
		}

		/// <summary>
		/// Creates the instance of the type converter.
		/// </summary>
		/// <param name="converterType">The type of the type converter.</param>
		/// <returns>
		/// The type converter instance to use for type conversions or <c>null</c> 
		/// if no type converter is found.
		/// </returns>
		/// <remarks>
		/// <para>
		/// The type specified for the type converter must implement 
		/// the <see cref="T:log4net.Util.TypeConverters.IConvertFrom" /> or <see cref="T:log4net.Util.TypeConverters.IConvertTo" /> interfaces 
		/// and must have a public default (no argument) constructor.
		/// </para>
		/// </remarks>
		private static object CreateConverterInstance(Type converterType)
		{
			if ((object)converterType == null)
			{
				throw new ArgumentNullException("converterType", "CreateConverterInstance cannot create instance, converterType is null");
			}
			if (typeof(IConvertFrom).IsAssignableFrom(converterType) || typeof(IConvertTo).IsAssignableFrom(converterType))
			{
				try
				{
					return Activator.CreateInstance(converterType);
				}
				catch (Exception exception)
				{
					LogLog.Error("ConverterRegistry: Cannot CreateConverterInstance of type [" + converterType.FullName + "], Exception in call to Activator.CreateInstance", exception);
				}
			}
			else
			{
				LogLog.Error("ConverterRegistry: Cannot CreateConverterInstance of type [" + converterType.FullName + "], type does not implement IConvertFrom or IConvertTo");
			}
			return null;
		}
	}
}
