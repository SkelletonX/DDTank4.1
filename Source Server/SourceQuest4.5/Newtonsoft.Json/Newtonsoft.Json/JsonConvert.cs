using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json
{
	public static class JsonConvert
	{
		public static readonly string True = "true";

		public static readonly string False = "false";

		public static readonly string Null = "null";

		public static readonly string Undefined = "undefined";

		public static readonly string PositiveInfinity = "Infinity";

		public static readonly string NegativeInfinity = "-Infinity";

		public static readonly string NaN = "NaN";

		public static Func<JsonSerializerSettings> DefaultSettings
		{
			get;
			set;
		}

		public static string ToString(DateTime value)
		{
			return ToString(value, DateFormatHandling.IsoDateFormat, DateTimeZoneHandling.RoundtripKind);
		}

		public static string ToString(DateTime value, DateFormatHandling format, DateTimeZoneHandling timeZoneHandling)
		{
			DateTime value2 = DateTimeUtils.EnsureDateTime(value, timeZoneHandling);
			using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
			{
				stringWriter.Write('"');
				DateTimeUtils.WriteDateTimeString(stringWriter, value2, format, null, CultureInfo.InvariantCulture);
				stringWriter.Write('"');
				return stringWriter.ToString();
			}
		}

		public static string ToString(DateTimeOffset value)
		{
			return ToString(value, DateFormatHandling.IsoDateFormat);
		}

		public static string ToString(DateTimeOffset value, DateFormatHandling format)
		{
			using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
			{
				stringWriter.Write('"');
				DateTimeUtils.WriteDateTimeOffsetString(stringWriter, value, format, null, CultureInfo.InvariantCulture);
				stringWriter.Write('"');
				return stringWriter.ToString();
			}
		}

		public static string ToString(bool value)
		{
			if (!value)
			{
				return False;
			}
			return True;
		}

		public static string ToString(char value)
		{
			return ToString(char.ToString(value));
		}

		public static string ToString(Enum value)
		{
			return value.ToString("D");
		}

		public static string ToString(int value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		public static string ToString(short value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static string ToString(ushort value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static string ToString(uint value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		public static string ToString(long value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		private static string ToStringInternal(BigInteger value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static string ToString(ulong value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		public static string ToString(float value)
		{
			return EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture));
		}

		internal static string ToString(float value, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			return EnsureFloatFormat(value, EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
		}

		private static string EnsureFloatFormat(double value, string text, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			if (floatFormatHandling == FloatFormatHandling.Symbol || (!double.IsInfinity(value) && !double.IsNaN(value)))
			{
				return text;
			}
			if (floatFormatHandling == FloatFormatHandling.DefaultValue)
			{
				if (nullable)
				{
					return Null;
				}
				return "0.0";
			}
			return quoteChar + text + quoteChar;
		}

		public static string ToString(double value)
		{
			return EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture));
		}

		internal static string ToString(double value, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			return EnsureFloatFormat(value, EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
		}

		private static string EnsureDecimalPlace(double value, string text)
		{
			if (double.IsNaN(value) || double.IsInfinity(value) || text.IndexOf('.') != -1 || text.IndexOf('E') != -1 || text.IndexOf('e') != -1)
			{
				return text;
			}
			return text + ".0";
		}

		private static string EnsureDecimalPlace(string text)
		{
			if (text.IndexOf('.') != -1)
			{
				return text;
			}
			return text + ".0";
		}

		public static string ToString(byte value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static string ToString(sbyte value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		public static string ToString(decimal value)
		{
			return EnsureDecimalPlace(value.ToString(null, CultureInfo.InvariantCulture));
		}

		public static string ToString(Guid value)
		{
			return ToString(value, '"');
		}

		internal static string ToString(Guid value, char quoteChar)
		{
			string str = value.ToString("D", CultureInfo.InvariantCulture);
			string text = quoteChar.ToString(CultureInfo.InvariantCulture);
			return text + str + text;
		}

		public static string ToString(TimeSpan value)
		{
			return ToString(value, '"');
		}

		internal static string ToString(TimeSpan value, char quoteChar)
		{
			return ToString(value.ToString(), quoteChar);
		}

		public static string ToString(Uri value)
		{
			if (value == null)
			{
				return Null;
			}
			return ToString(value, '"');
		}

		internal static string ToString(Uri value, char quoteChar)
		{
			return ToString(value.OriginalString, quoteChar);
		}

		public static string ToString(string value)
		{
			return ToString(value, '"');
		}

		public static string ToString(string value, char delimiter)
		{
			return ToString(value, delimiter, StringEscapeHandling.Default);
		}

		public static string ToString(string value, char delimiter, StringEscapeHandling stringEscapeHandling)
		{
			if (delimiter != '"' && delimiter != '\'')
			{
				throw new ArgumentException("Delimiter must be a single or double quote.", "delimiter");
			}
			return JavaScriptUtils.ToEscapedJavaScriptString(value, delimiter, appendDelimiters: true, stringEscapeHandling);
		}

		public static string ToString(object value)
		{
			if (value == null)
			{
				return Null;
			}
			switch (ConvertUtils.GetTypeCode(value.GetType()))
			{
			case PrimitiveTypeCode.String:
				return ToString((string)value);
			case PrimitiveTypeCode.Char:
				return ToString((char)value);
			case PrimitiveTypeCode.Boolean:
				return ToString((bool)value);
			case PrimitiveTypeCode.SByte:
				return ToString((sbyte)value);
			case PrimitiveTypeCode.Int16:
				return ToString((short)value);
			case PrimitiveTypeCode.UInt16:
				return ToString((ushort)value);
			case PrimitiveTypeCode.Int32:
				return ToString((int)value);
			case PrimitiveTypeCode.Byte:
				return ToString((byte)value);
			case PrimitiveTypeCode.UInt32:
				return ToString((uint)value);
			case PrimitiveTypeCode.Int64:
				return ToString((long)value);
			case PrimitiveTypeCode.UInt64:
				return ToString((ulong)value);
			case PrimitiveTypeCode.Single:
				return ToString((float)value);
			case PrimitiveTypeCode.Double:
				return ToString((double)value);
			case PrimitiveTypeCode.DateTime:
				return ToString((DateTime)value);
			case PrimitiveTypeCode.Decimal:
				return ToString((decimal)value);
			case PrimitiveTypeCode.DBNull:
				return Null;
			case PrimitiveTypeCode.DateTimeOffset:
				return ToString((DateTimeOffset)value);
			case PrimitiveTypeCode.Guid:
				return ToString((Guid)value);
			case PrimitiveTypeCode.Uri:
				return ToString((Uri)value);
			case PrimitiveTypeCode.TimeSpan:
				return ToString((TimeSpan)value);
			case PrimitiveTypeCode.BigInteger:
				return ToStringInternal((BigInteger)value);
			default:
				throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
			}
		}

		public static string SerializeObject(object value)
		{
			return SerializeObject(value, (Type)null, (JsonSerializerSettings)null);
		}

		public static string SerializeObject(object value, Formatting formatting)
		{
			return SerializeObject(value, formatting, (JsonSerializerSettings)null);
		}

		public static string SerializeObject(object value, params JsonConverter[] converters)
		{
			object obj;
			if (converters == null || converters.Length <= 0)
			{
				obj = null;
			}
			else
			{
				JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
				jsonSerializerSettings.Converters = converters;
				obj = jsonSerializerSettings;
			}
			JsonSerializerSettings settings = (JsonSerializerSettings)obj;
			return SerializeObject(value, null, settings);
		}

		public static string SerializeObject(object value, Formatting formatting, params JsonConverter[] converters)
		{
			object obj;
			if (converters == null || converters.Length <= 0)
			{
				obj = null;
			}
			else
			{
				JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
				jsonSerializerSettings.Converters = converters;
				obj = jsonSerializerSettings;
			}
			JsonSerializerSettings settings = (JsonSerializerSettings)obj;
			return SerializeObject(value, null, formatting, settings);
		}

		public static string SerializeObject(object value, JsonSerializerSettings settings)
		{
			return SerializeObject(value, null, settings);
		}

		public static string SerializeObject(object value, Type type, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			return SerializeObjectInternal(value, type, jsonSerializer);
		}

		public static string SerializeObject(object value, Formatting formatting, JsonSerializerSettings settings)
		{
			return SerializeObject(value, null, formatting, settings);
		}

		public static string SerializeObject(object value, Type type, Formatting formatting, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			jsonSerializer.Formatting = formatting;
			return SerializeObjectInternal(value, type, jsonSerializer);
		}

		private static string SerializeObjectInternal(object value, Type type, JsonSerializer jsonSerializer)
		{
			StringBuilder sb = new StringBuilder(256);
			StringWriter stringWriter = new StringWriter(sb, CultureInfo.InvariantCulture);
			using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
			{
				jsonTextWriter.Formatting = jsonSerializer.Formatting;
				jsonSerializer.Serialize(jsonTextWriter, value, type);
			}
			return stringWriter.ToString();
		}

		[Obsolete("SerializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to serialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.SerializeObject(value))")]
		public static Task<string> SerializeObjectAsync(object value)
		{
			return SerializeObjectAsync(value, Formatting.None, null);
		}

		[Obsolete("SerializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to serialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, formatting))")]
		public static Task<string> SerializeObjectAsync(object value, Formatting formatting)
		{
			return SerializeObjectAsync(value, formatting, null);
		}

		[Obsolete("SerializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to serialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, formatting, settings))")]
		public static Task<string> SerializeObjectAsync(object value, Formatting formatting, JsonSerializerSettings settings)
		{
			return Task.Factory.StartNew(() => SerializeObject(value, formatting, settings));
		}

		public static object DeserializeObject(string value)
		{
			return DeserializeObject(value, (Type)null, (JsonSerializerSettings)null);
		}

		public static object DeserializeObject(string value, JsonSerializerSettings settings)
		{
			return DeserializeObject(value, null, settings);
		}

		public static object DeserializeObject(string value, Type type)
		{
			return DeserializeObject(value, type, (JsonSerializerSettings)null);
		}

		public static T DeserializeObject<T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value, (JsonSerializerSettings)null);
		}

		public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject)
		{
			return DeserializeObject<T>(value);
		}

		public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject, JsonSerializerSettings settings)
		{
			return DeserializeObject<T>(value, settings);
		}

		public static T DeserializeObject<T>(string value, params JsonConverter[] converters)
		{
			return (T)DeserializeObject(value, typeof(T), converters);
		}

		public static T DeserializeObject<T>(string value, JsonSerializerSettings settings)
		{
			return (T)DeserializeObject(value, typeof(T), settings);
		}

		public static object DeserializeObject(string value, Type type, params JsonConverter[] converters)
		{
			object obj;
			if (converters == null || converters.Length <= 0)
			{
				obj = null;
			}
			else
			{
				JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
				jsonSerializerSettings.Converters = converters;
				obj = jsonSerializerSettings;
			}
			JsonSerializerSettings settings = (JsonSerializerSettings)obj;
			return DeserializeObject(value, type, settings);
		}

		public static object DeserializeObject(string value, Type type, JsonSerializerSettings settings)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			if (!jsonSerializer.IsCheckAdditionalContentSet())
			{
				jsonSerializer.CheckAdditionalContent = true;
			}
			using (JsonTextReader reader = new JsonTextReader(new StringReader(value)))
			{
				return jsonSerializer.Deserialize(reader, type);
			}
		}

		[Obsolete("DeserializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to deserialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value))")]
		public static Task<T> DeserializeObjectAsync<T>(string value)
		{
			return DeserializeObjectAsync<T>(value, null);
		}

		[Obsolete("DeserializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to deserialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value, settings))")]
		public static Task<T> DeserializeObjectAsync<T>(string value, JsonSerializerSettings settings)
		{
			return Task.Factory.StartNew(() => DeserializeObject<T>(value, settings));
		}

		[Obsolete("DeserializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to deserialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value))")]
		public static Task<object> DeserializeObjectAsync(string value)
		{
			return DeserializeObjectAsync(value, null, null);
		}

		[Obsolete("DeserializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to deserialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value, type, settings))")]
		public static Task<object> DeserializeObjectAsync(string value, Type type, JsonSerializerSettings settings)
		{
			return Task.Factory.StartNew(() => DeserializeObject(value, type, settings));
		}

		public static void PopulateObject(string value, object target)
		{
			PopulateObject(value, target, null);
		}

		public static void PopulateObject(string value, object target, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(value)))
			{
				jsonSerializer.Populate(jsonReader, target);
				if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
				{
					throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
				}
			}
		}

		[Obsolete("PopulateObjectAsync is obsolete. Use the Task.Factory.StartNew method to populate an object with JSON values asynchronously: Task.Factory.StartNew(() => JsonConvert.PopulateObject(value, target, settings))")]
		public static Task PopulateObjectAsync(string value, object target, JsonSerializerSettings settings)
		{
			return Task.Factory.StartNew(delegate
			{
				PopulateObject(value, target, settings);
			});
		}

		public static string SerializeXmlNode(XmlNode node)
		{
			return SerializeXmlNode(node, Formatting.None);
		}

		public static string SerializeXmlNode(XmlNode node, Formatting formatting)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			return SerializeObject(node, formatting, xmlNodeConverter);
		}

		public static string SerializeXmlNode(XmlNode node, Formatting formatting, bool omitRootObject)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.OmitRootObject = omitRootObject;
			XmlNodeConverter xmlNodeConverter2 = xmlNodeConverter;
			return SerializeObject(node, formatting, xmlNodeConverter2);
		}

		public static XmlDocument DeserializeXmlNode(string value)
		{
			return DeserializeXmlNode(value, null);
		}

		public static XmlDocument DeserializeXmlNode(string value, string deserializeRootElementName)
		{
			return DeserializeXmlNode(value, deserializeRootElementName, writeArrayAttribute: false);
		}

		public static XmlDocument DeserializeXmlNode(string value, string deserializeRootElementName, bool writeArrayAttribute)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.DeserializeRootElementName = deserializeRootElementName;
			xmlNodeConverter.WriteArrayAttribute = writeArrayAttribute;
			return (XmlDocument)DeserializeObject(value, typeof(XmlDocument), xmlNodeConverter);
		}

		public static string SerializeXNode(XObject node)
		{
			return SerializeXNode(node, Formatting.None);
		}

		public static string SerializeXNode(XObject node, Formatting formatting)
		{
			return SerializeXNode(node, formatting, omitRootObject: false);
		}

		public static string SerializeXNode(XObject node, Formatting formatting, bool omitRootObject)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.OmitRootObject = omitRootObject;
			XmlNodeConverter xmlNodeConverter2 = xmlNodeConverter;
			return SerializeObject(node, formatting, xmlNodeConverter2);
		}

		public static XDocument DeserializeXNode(string value)
		{
			return DeserializeXNode(value, null);
		}

		public static XDocument DeserializeXNode(string value, string deserializeRootElementName)
		{
			return DeserializeXNode(value, deserializeRootElementName, writeArrayAttribute: false);
		}

		public static XDocument DeserializeXNode(string value, string deserializeRootElementName, bool writeArrayAttribute)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.DeserializeRootElementName = deserializeRootElementName;
			xmlNodeConverter.WriteArrayAttribute = writeArrayAttribute;
			return (XDocument)DeserializeObject(value, typeof(XDocument), xmlNodeConverter);
		}
	}
}
