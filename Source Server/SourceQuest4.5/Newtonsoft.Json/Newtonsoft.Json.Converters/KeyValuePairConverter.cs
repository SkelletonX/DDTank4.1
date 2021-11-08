using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Converters
{
	public class KeyValuePairConverter : JsonConverter
	{
		private const string KeyName = "Key";

		private const string ValueName = "Value";

		private static readonly ThreadSafeStore<Type, ReflectionObject> ReflectionObjectPerType = new ThreadSafeStore<Type, ReflectionObject>(InitializeReflectionObject);

		private static ReflectionObject InitializeReflectionObject(Type t)
		{
			IList<Type> genericArguments = t.GetGenericArguments();
			Type type = genericArguments[0];
			Type type2 = genericArguments[1];
			return ReflectionObject.Create(t, t.GetConstructor(new Type[2]
			{
				type,
				type2
			}), "Key", "Value");
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			ReflectionObject reflectionObject = ReflectionObjectPerType.Get(value.GetType());
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Key") : "Key");
			serializer.Serialize(writer, reflectionObject.GetValue(value, "Key"), reflectionObject.GetType("Key"));
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Value") : "Value");
			serializer.Serialize(writer, reflectionObject.GetValue(value, "Value"), reflectionObject.GetType("Value"));
			writer.WriteEndObject();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			bool flag = ReflectionUtils.IsNullableType(objectType);
			Type key = flag ? Nullable.GetUnderlyingType(objectType) : objectType;
			ReflectionObject reflectionObject = ReflectionObjectPerType.Get(key);
			if (reader.TokenType == JsonToken.Null)
			{
				if (!flag)
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to KeyValuePair.");
				}
				return null;
			}
			object obj = null;
			object obj2 = null;
			ReadAndAssert(reader);
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string a = reader.Value.ToString();
				if (string.Equals(a, "Key", StringComparison.OrdinalIgnoreCase))
				{
					ReadAndAssert(reader);
					obj = serializer.Deserialize(reader, reflectionObject.GetType("Key"));
				}
				else if (string.Equals(a, "Value", StringComparison.OrdinalIgnoreCase))
				{
					ReadAndAssert(reader);
					obj2 = serializer.Deserialize(reader, reflectionObject.GetType("Value"));
				}
				else
				{
					reader.Skip();
				}
				ReadAndAssert(reader);
			}
			return reflectionObject.Creator(obj, obj2);
		}

		public override bool CanConvert(Type objectType)
		{
			Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
			if (type.IsValueType() && type.IsGenericType())
			{
				return type.GetGenericTypeDefinition() == typeof(KeyValuePair<, >);
			}
			return false;
		}

		private static void ReadAndAssert(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading KeyValuePair.");
			}
		}
	}
}
