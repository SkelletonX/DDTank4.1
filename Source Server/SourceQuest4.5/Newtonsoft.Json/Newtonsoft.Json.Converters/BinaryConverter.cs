using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
	public class BinaryConverter : JsonConverter
	{
		private const string BinaryTypeName = "System.Data.Linq.Binary";

		private const string BinaryToArrayName = "ToArray";

		private ReflectionObject _reflectionObject;

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			byte[] byteArray = GetByteArray(value);
			writer.WriteValue(byteArray);
		}

		private byte[] GetByteArray(object value)
		{
			if (value.GetType().AssignableToTypeName("System.Data.Linq.Binary"))
			{
				EnsureReflectionObject(value.GetType());
				return (byte[])_reflectionObject.GetValue(value, "ToArray");
			}
			if (value is SqlBinary)
			{
				return ((SqlBinary)value).Value;
			}
			throw new JsonSerializationException("Unexpected value type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
		}

		private void EnsureReflectionObject(Type t)
		{
			if (_reflectionObject == null)
			{
				_reflectionObject = ReflectionObject.Create(t, t.GetConstructor(new Type[1]
				{
					typeof(byte[])
				}), "ToArray");
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
			if (reader.TokenType == JsonToken.Null)
			{
				if (!ReflectionUtils.IsNullable(objectType))
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			byte[] array;
			if (reader.TokenType == JsonToken.StartArray)
			{
				array = ReadByteArray(reader);
			}
			else
			{
				if (reader.TokenType != JsonToken.String)
				{
					throw JsonSerializationException.Create(reader, "Unexpected token parsing binary. Expected String or StartArray, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
				}
				string s = reader.Value.ToString();
				array = Convert.FromBase64String(s);
			}
			if (type.AssignableToTypeName("System.Data.Linq.Binary"))
			{
				EnsureReflectionObject(type);
				return _reflectionObject.Creator(array);
			}
			if (type == typeof(SqlBinary))
			{
				return new SqlBinary(array);
			}
			throw JsonSerializationException.Create(reader, "Unexpected object type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, objectType));
		}

		private byte[] ReadByteArray(JsonReader reader)
		{
			List<byte> list = new List<byte>();
			while (reader.Read())
			{
				switch (reader.TokenType)
				{
				case JsonToken.Integer:
					list.Add(Convert.ToByte(reader.Value, CultureInfo.InvariantCulture));
					break;
				case JsonToken.EndArray:
					return list.ToArray();
				default:
					throw JsonSerializationException.Create(reader, "Unexpected token when reading bytes: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
				case JsonToken.Comment:
					break;
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading bytes.");
		}

		public override bool CanConvert(Type objectType)
		{
			if (objectType.AssignableToTypeName("System.Data.Linq.Binary"))
			{
				return true;
			}
			if (objectType == typeof(SqlBinary) || objectType == typeof(SqlBinary?))
			{
				return true;
			}
			return false;
		}
	}
}
