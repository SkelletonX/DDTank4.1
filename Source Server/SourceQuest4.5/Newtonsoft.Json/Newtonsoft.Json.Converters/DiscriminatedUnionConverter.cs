using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Converters
{
	public class DiscriminatedUnionConverter : JsonConverter
	{
		private const string CasePropertyName = "Case";

		private const string FieldsPropertyName = "Fields";

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			Type type = value.GetType();
			object arg = FSharpUtils.GetUnionFields(null, value, type, null);
			object arg2 = FSharpUtils.GetUnionCaseInfo(arg);
			object obj = FSharpUtils.GetUnionCaseFields(arg);
			object obj2 = FSharpUtils.GetUnionCaseInfoName(arg2);
			object[] array = obj as object[];
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Case") : "Case");
			writer.WriteValue((string)obj2);
			if (array != null && array.Length > 0)
			{
				writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Fields") : "Fields");
				serializer.Serialize(writer, obj);
			}
			writer.WriteEndObject();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			object obj = null;
			string text = null;
			JArray jArray = null;
			ReadAndAssert(reader);
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string text2 = reader.Value.ToString();
				if (string.Equals(text2, "Case", StringComparison.OrdinalIgnoreCase))
				{
					ReadAndAssert(reader);
					IEnumerable enumerable = (IEnumerable)FSharpUtils.GetUnionCases(null, objectType, null);
					text = reader.Value.ToString();
					foreach (object item in enumerable)
					{
						if ((string)FSharpUtils.GetUnionCaseInfoName(item) == text)
						{
							obj = item;
							break;
						}
					}
					if (obj == null)
					{
						throw JsonSerializationException.Create(reader, "No union type found with the name '{0}'.".FormatWith(CultureInfo.InvariantCulture, text));
					}
				}
				else
				{
					if (!string.Equals(text2, "Fields", StringComparison.OrdinalIgnoreCase))
					{
						throw JsonSerializationException.Create(reader, "Unexpected property '{0}' found when reading union.".FormatWith(CultureInfo.InvariantCulture, text2));
					}
					ReadAndAssert(reader);
					if (reader.TokenType != JsonToken.StartArray)
					{
						throw JsonSerializationException.Create(reader, "Union fields must been an array.");
					}
					jArray = (JArray)JToken.ReadFrom(reader);
				}
				ReadAndAssert(reader);
			}
			if (obj == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union name found.".FormatWith(CultureInfo.InvariantCulture, "Case"));
			}
			PropertyInfo[] array = (PropertyInfo[])FSharpUtils.GetUnionCaseInfoFields(obj);
			object[] array2 = new object[array.Length];
			if (array.Length > 0 && jArray == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union fields found.".FormatWith(CultureInfo.InvariantCulture, "Fields"));
			}
			if (jArray != null)
			{
				if (array.Length != jArray.Count)
				{
					throw JsonSerializationException.Create(reader, "The number of field values does not match the number of properties definied by union '{0}'.".FormatWith(CultureInfo.InvariantCulture, text));
				}
				for (int i = 0; i < jArray.Count; i++)
				{
					JToken jToken = jArray[i];
					PropertyInfo propertyInfo = array[i];
					array2[i] = jToken.ToObject(propertyInfo.PropertyType, serializer);
				}
			}
			return FSharpUtils.MakeUnion(null, obj, array2, null);
		}

		public override bool CanConvert(Type objectType)
		{
			if (typeof(IEnumerable).IsAssignableFrom(objectType))
			{
				return false;
			}
			object[] customAttributes = objectType.GetCustomAttributes(inherit: true);
			bool flag = false;
			object[] array = customAttributes;
			foreach (object obj in array)
			{
				Type type = obj.GetType();
				if (type.FullName == "Microsoft.FSharp.Core.CompilationMappingAttribute")
				{
					FSharpUtils.EnsureInitialized(type.Assembly());
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			return (bool)FSharpUtils.IsUnion(null, objectType, null);
		}

		private static void ReadAndAssert(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading union.");
			}
		}
	}
}
