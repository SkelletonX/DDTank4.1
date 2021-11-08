using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
	public class EntityKeyMemberConverter : JsonConverter
	{
		private const string EntityKeyMemberFullTypeName = "System.Data.EntityKeyMember";

		private const string KeyPropertyName = "Key";

		private const string TypePropertyName = "Type";

		private const string ValuePropertyName = "Value";

		private static ReflectionObject _reflectionObject;

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			EnsureReflectionObject(value.GetType());
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			string value2 = (string)_reflectionObject.GetValue(value, "Key");
			object value3 = _reflectionObject.GetValue(value, "Value");
			Type type = value3?.GetType();
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Key") : "Key");
			writer.WriteValue(value2);
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Type") : "Type");
			writer.WriteValue((type != null) ? type.FullName : null);
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Value") : "Value");
			if (type != null)
			{
				if (JsonSerializerInternalWriter.TryConvertToString(value3, type, out string s))
				{
					writer.WriteValue(s);
				}
				else
				{
					writer.WriteValue(value3);
				}
			}
			else
			{
				writer.WriteNull();
			}
			writer.WriteEndObject();
		}

		private static void ReadAndAssertProperty(JsonReader reader, string propertyName)
		{
			ReadAndAssert(reader);
			if (reader.TokenType != JsonToken.PropertyName || !string.Equals(reader.Value.ToString(), propertyName, StringComparison.OrdinalIgnoreCase))
			{
				throw new JsonSerializationException("Expected JSON property '{0}'.".FormatWith(CultureInfo.InvariantCulture, propertyName));
			}
		}

		private static void ReadAndAssert(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw new JsonSerializationException("Unexpected end.");
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			EnsureReflectionObject(objectType);
			object obj = _reflectionObject.Creator();
			ReadAndAssertProperty(reader, "Key");
			ReadAndAssert(reader);
			_reflectionObject.SetValue(obj, "Key", reader.Value.ToString());
			ReadAndAssertProperty(reader, "Type");
			ReadAndAssert(reader);
			string typeName = reader.Value.ToString();
			Type type = Type.GetType(typeName);
			ReadAndAssertProperty(reader, "Value");
			ReadAndAssert(reader);
			_reflectionObject.SetValue(obj, "Value", serializer.Deserialize(reader, type));
			ReadAndAssert(reader);
			return obj;
		}

		private static void EnsureReflectionObject(Type objectType)
		{
			if (_reflectionObject == null)
			{
				_reflectionObject = ReflectionObject.Create(objectType, "Key", "Value");
			}
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType.AssignableToTypeName("System.Data.EntityKeyMember");
		}
	}
}
