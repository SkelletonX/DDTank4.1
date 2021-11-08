using log4net.Core;
using log4net.Util.TypeConverters;
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace log4net.Util
{
	/// <summary>
	/// A convenience class to convert property values to specific types.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Utility functions for converting types and parsing values.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public sealed class OptionConverter
	{
		private const string DELIM_START = "${";

		private const char DELIM_STOP = '}';

		private const int DELIM_START_LEN = 2;

		private const int DELIM_STOP_LEN = 1;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Util.OptionConverter" /> class. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// Uses a private access modifier to prevent instantiation of this class.
		/// </para>
		/// </remarks>
		private OptionConverter()
		{
		}

		/// <summary>
		/// Converts a string to a <see cref="T:System.Boolean" /> value.
		/// </summary>
		/// <param name="argValue">String to convert.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The <see cref="T:System.Boolean" /> value of <paramref name="argValue" />.</returns>
		/// <remarks>
		/// <para>
		/// If <paramref name="argValue" /> is "true", then <c>true</c> is returned. 
		/// If <paramref name="argValue" /> is "false", then <c>false</c> is returned. 
		/// Otherwise, <paramref name="defaultValue" /> is returned.
		/// </para>
		/// </remarks>
		public static bool ToBoolean(string argValue, bool defaultValue)
		{
			if (argValue != null && argValue.Length > 0)
			{
				try
				{
					return bool.Parse(argValue);
				}
				catch (Exception exception)
				{
					LogLog.Error("OptionConverter: [" + argValue + "] is not in proper bool form.", exception);
				}
			}
			return defaultValue;
		}

		/// <summary>
		/// Parses a file size into a number.
		/// </summary>
		/// <param name="argValue">String to parse.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The <see cref="T:System.Int64" /> value of <paramref name="argValue" />.</returns>
		/// <remarks>
		/// <para>
		/// Parses a file size of the form: number[KB|MB|GB] into a
		/// long value. It is scaled with the appropriate multiplier.
		/// </para>
		/// <para>
		/// <paramref name="defaultValue" /> is returned when <paramref name="argValue" />
		/// cannot be converted to a <see cref="T:System.Int64" /> value.
		/// </para>
		/// </remarks>
		public static long ToFileSize(string argValue, long defaultValue)
		{
			if (argValue == null)
			{
				return defaultValue;
			}
			string text = argValue.Trim().ToUpper(CultureInfo.InvariantCulture);
			long num = 1L;
			int length;
			if ((length = text.IndexOf("KB")) != -1)
			{
				num = 1024L;
				text = text.Substring(0, length);
			}
			else if ((length = text.IndexOf("MB")) != -1)
			{
				num = 1048576L;
				text = text.Substring(0, length);
			}
			else if ((length = text.IndexOf("GB")) != -1)
			{
				num = 1073741824L;
				text = text.Substring(0, length);
			}
			if (text != null)
			{
				text = text.Trim();
				if (SystemInfo.TryParse(text, out long val))
				{
					return val * num;
				}
				LogLog.Error("OptionConverter: [" + text + "] is not in the correct file size syntax.");
			}
			return defaultValue;
		}

		/// <summary>
		/// Converts a string to an object.
		/// </summary>
		/// <param name="target">The target type to convert to.</param>
		/// <param name="txt">The string to convert to an object.</param>
		/// <returns>
		/// The object converted from a string or <c>null</c> when the 
		/// conversion failed.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Converts a string to an object. Uses the converter registry to try
		/// to convert the string value into the specified target type.
		/// </para>
		/// </remarks>
		public static object ConvertStringTo(Type target, string txt)
		{
			if ((object)target == null)
			{
				throw new ArgumentNullException("target");
			}
			if ((object)typeof(string) == target || (object)typeof(object) == target)
			{
				return txt;
			}
			IConvertFrom convertFrom = ConverterRegistry.GetConvertFrom(target);
			if (convertFrom != null && convertFrom.CanConvertFrom(typeof(string)))
			{
				return convertFrom.ConvertFrom(txt);
			}
			if (target.IsEnum)
			{
				return ParseEnum(target, txt, ignoreCase: true);
			}
			return target.GetMethod("Parse", new Type[1]
			{
				typeof(string)
			})?.Invoke(null, BindingFlags.InvokeMethod, null, new object[1]
			{
				txt
			}, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Checks if there is an appropriate type conversion from the source type to the target type.
		/// </summary>
		/// <param name="sourceType">The type to convert from.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <returns><c>true</c> if there is a conversion from the source type to the target type.</returns>
		/// <remarks>
		/// Checks if there is an appropriate type conversion from the source type to the target type.
		/// <para>
		/// </para>
		/// </remarks>
		public static bool CanConvertTypeTo(Type sourceType, Type targetType)
		{
			if ((object)sourceType == null || (object)targetType == null)
			{
				return false;
			}
			if (targetType.IsAssignableFrom(sourceType))
			{
				return true;
			}
			IConvertTo convertTo = ConverterRegistry.GetConvertTo(sourceType, targetType);
			if (convertTo != null && convertTo.CanConvertTo(targetType))
			{
				return true;
			}
			IConvertFrom convertFrom = ConverterRegistry.GetConvertFrom(targetType);
			if (convertFrom != null && convertFrom.CanConvertFrom(sourceType))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Converts an object to the target type.
		/// </summary>
		/// <param name="sourceInstance">The object to convert to the target type.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <returns>The converted object.</returns>
		/// <remarks>
		/// <para>
		/// Converts an object to the target type.
		/// </para>
		/// </remarks>
		public static object ConvertTypeTo(object sourceInstance, Type targetType)
		{
			Type type = sourceInstance.GetType();
			if (targetType.IsAssignableFrom(type))
			{
				return sourceInstance;
			}
			IConvertTo convertTo = ConverterRegistry.GetConvertTo(type, targetType);
			if (convertTo != null && convertTo.CanConvertTo(targetType))
			{
				return convertTo.ConvertTo(sourceInstance, targetType);
			}
			IConvertFrom convertFrom = ConverterRegistry.GetConvertFrom(targetType);
			if (convertFrom != null && convertFrom.CanConvertFrom(type))
			{
				return convertFrom.ConvertFrom(sourceInstance);
			}
			throw new ArgumentException("Cannot convert source object [" + sourceInstance.ToString() + "] to target type [" + targetType.Name + "]", "sourceInstance");
		}

		/// <summary>
		/// Instantiates an object given a class name.
		/// </summary>
		/// <param name="className">The fully qualified class name of the object to instantiate.</param>
		/// <param name="superClass">The class to which the new object should belong.</param>
		/// <param name="defaultValue">The object to return in case of non-fulfillment.</param>
		/// <returns>
		/// An instance of the <paramref name="className" /> or <paramref name="defaultValue" />
		/// if the object could not be instantiated.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Checks that the <paramref name="className" /> is a subclass of
		/// <paramref name="superClass" />. If that test fails or the object could
		/// not be instantiated, then <paramref name="defaultValue" /> is returned.
		/// </para>
		/// </remarks>
		public static object InstantiateByClassName(string className, Type superClass, object defaultValue)
		{
			if (className != null)
			{
				try
				{
					Type typeFromString = SystemInfo.GetTypeFromString(className, throwOnError: true, ignoreCase: true);
					if (!superClass.IsAssignableFrom(typeFromString))
					{
						LogLog.Error("OptionConverter: A [" + className + "] object is not assignable to a [" + superClass.FullName + "] variable.");
						return defaultValue;
					}
					return Activator.CreateInstance(typeFromString);
				}
				catch (Exception exception)
				{
					LogLog.Error("OptionConverter: Could not instantiate class [" + className + "].", exception);
				}
			}
			return defaultValue;
		}

		/// <summary>
		/// Performs variable substitution in string <paramref name="val" /> from the 
		/// values of keys found in <paramref name="props" />.
		/// </summary>
		/// <param name="value">The string on which variable substitution is performed.</param>
		/// <param name="props">The dictionary to use to lookup variables.</param>
		/// <returns>The result of the substitutions.</returns>
		/// <remarks>
		/// <para>
		/// The variable substitution delimiters are <b>${</b> and <b>}</b>.
		/// </para>
		/// <para>
		/// For example, if props contains <c>key=value</c>, then the call
		/// </para>
		/// <para>
		/// <code lang="C#">
		/// string s = OptionConverter.SubstituteVariables("Value of key is ${key}.");
		/// </code>
		/// </para>
		/// <para>
		/// will set the variable <c>s</c> to "Value of key is value.".
		/// </para>
		/// <para>
		/// If no value could be found for the specified key, then substitution 
		/// defaults to an empty string.
		/// </para>
		/// <para>
		/// For example, if system properties contains no value for the key
		/// "nonExistentKey", then the call
		/// </para>
		/// <para>
		/// <code lang="C#">
		/// string s = OptionConverter.SubstituteVariables("Value of nonExistentKey is [${nonExistentKey}]");
		/// </code>
		/// </para>
		/// <para>
		/// will set <s>s</s> to "Value of nonExistentKey is []".	 
		/// </para>
		/// <para>
		/// An Exception is thrown if <paramref name="value" /> contains a start 
		/// delimiter "${" which is not balanced by a stop delimiter "}". 
		/// </para>
		/// </remarks>
		public static string SubstituteVariables(string value, IDictionary props)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			int num2;
			while (true)
			{
				bool flag = true;
				num2 = value.IndexOf("${", num);
				if (num2 == -1)
				{
					if (num == 0)
					{
						return value;
					}
					stringBuilder.Append(value.Substring(num, value.Length - num));
					return stringBuilder.ToString();
				}
				stringBuilder.Append(value.Substring(num, num2 - num));
				int num3 = value.IndexOf('}', num2);
				if (num3 == -1)
				{
					break;
				}
				num2 += 2;
				string key = value.Substring(num2, num3 - num2);
				string text = props[key] as string;
				if (text != null)
				{
					stringBuilder.Append(text);
				}
				num = num3 + 1;
			}
			throw new LogException("[" + value + "] has no closing brace. Opening brace at position [" + num2 + "]");
		}

		/// <summary>
		/// Converts the string representation of the name or numeric value of one or 
		/// more enumerated constants to an equivalent enumerated object.
		/// </summary>
		/// <param name="enumType">The type to convert to.</param>
		/// <param name="value">The enum string value.</param>
		/// <param name="ignoreCase">If <c>true</c>, ignore case; otherwise, regard case.</param>
		/// <returns>An object of type <paramref name="enumType" /> whose value is represented by <paramref name="value" />.</returns>
		private static object ParseEnum(Type enumType, string value, bool ignoreCase)
		{
			return Enum.Parse(enumType, value, ignoreCase);
		}
	}
}
