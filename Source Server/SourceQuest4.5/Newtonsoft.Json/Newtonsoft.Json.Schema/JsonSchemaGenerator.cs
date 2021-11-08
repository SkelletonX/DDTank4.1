using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	public class JsonSchemaGenerator
	{
		private class TypeSchema
		{
			public Type Type
			{
				get;
				private set;
			}

			public JsonSchema Schema
			{
				get;
				private set;
			}

			public TypeSchema(Type type, JsonSchema schema)
			{
				ValidationUtils.ArgumentNotNull(type, "type");
				ValidationUtils.ArgumentNotNull(schema, "schema");
				Type = type;
				Schema = schema;
			}
		}

		private IContractResolver _contractResolver;

		private JsonSchemaResolver _resolver;

		private readonly IList<TypeSchema> _stack = new List<TypeSchema>();

		private JsonSchema _currentSchema;

		public UndefinedSchemaIdHandling UndefinedSchemaIdHandling
		{
			get;
			set;
		}

		public IContractResolver ContractResolver
		{
			get
			{
				if (_contractResolver == null)
				{
					return DefaultContractResolver.Instance;
				}
				return _contractResolver;
			}
			set
			{
				_contractResolver = value;
			}
		}

		private JsonSchema CurrentSchema => _currentSchema;

		private void Push(TypeSchema typeSchema)
		{
			_currentSchema = typeSchema.Schema;
			_stack.Add(typeSchema);
			_resolver.LoadedSchemas.Add(typeSchema.Schema);
		}

		private TypeSchema Pop()
		{
			TypeSchema result = _stack[_stack.Count - 1];
			_stack.RemoveAt(_stack.Count - 1);
			TypeSchema typeSchema = _stack.LastOrDefault();
			if (typeSchema != null)
			{
				_currentSchema = typeSchema.Schema;
			}
			else
			{
				_currentSchema = null;
			}
			return result;
		}

		public JsonSchema Generate(Type type)
		{
			return Generate(type, new JsonSchemaResolver(), rootSchemaNullable: false);
		}

		public JsonSchema Generate(Type type, JsonSchemaResolver resolver)
		{
			return Generate(type, resolver, rootSchemaNullable: false);
		}

		public JsonSchema Generate(Type type, bool rootSchemaNullable)
		{
			return Generate(type, new JsonSchemaResolver(), rootSchemaNullable);
		}

		public JsonSchema Generate(Type type, JsonSchemaResolver resolver, bool rootSchemaNullable)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			_resolver = resolver;
			return GenerateInternal(type, (!rootSchemaNullable) ? Required.Always : Required.Default, required: false);
		}

		private string GetTitle(Type type)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (cachedAttribute != null && !string.IsNullOrEmpty(cachedAttribute.Title))
			{
				return cachedAttribute.Title;
			}
			return null;
		}

		private string GetDescription(Type type)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (cachedAttribute != null && !string.IsNullOrEmpty(cachedAttribute.Description))
			{
				return cachedAttribute.Description;
			}
			return ReflectionUtils.GetAttribute<DescriptionAttribute>(type)?.Description;
		}

		private string GetTypeId(Type type, bool explicitOnly)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (cachedAttribute != null && !string.IsNullOrEmpty(cachedAttribute.Id))
			{
				return cachedAttribute.Id;
			}
			if (explicitOnly)
			{
				return null;
			}
			switch (UndefinedSchemaIdHandling)
			{
			case UndefinedSchemaIdHandling.UseTypeName:
				return type.FullName;
			case UndefinedSchemaIdHandling.UseAssemblyQualifiedName:
				return type.AssemblyQualifiedName;
			default:
				return null;
			}
		}

		private JsonSchema GenerateInternal(Type type, Required valueRequired, bool required)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			string typeId = GetTypeId(type, explicitOnly: false);
			string typeId2 = GetTypeId(type, explicitOnly: true);
			if (!string.IsNullOrEmpty(typeId))
			{
				JsonSchema schema = _resolver.GetSchema(typeId);
				if (schema != null)
				{
					if (valueRequired != Required.Always && !HasFlag(schema.Type, JsonSchemaType.Null))
					{
						schema.Type |= JsonSchemaType.Null;
					}
					if (required && schema.Required != true)
					{
						schema.Required = true;
					}
					return schema;
				}
			}
			if (_stack.Any((TypeSchema tc) => tc.Type == type))
			{
				throw new JsonException("Unresolved circular reference for type '{0}'. Explicitly define an Id for the type using a JsonObject/JsonArray attribute or automatically generate a type Id using the UndefinedSchemaIdHandling property.".FormatWith(CultureInfo.InvariantCulture, type));
			}
			JsonContract jsonContract = ContractResolver.ResolveContract(type);
			JsonConverter jsonConverter;
			if ((jsonConverter = jsonContract.Converter) != null || (jsonConverter = jsonContract.InternalConverter) != null)
			{
				JsonSchema schema2 = jsonConverter.GetSchema();
				if (schema2 != null)
				{
					return schema2;
				}
			}
			Push(new TypeSchema(type, new JsonSchema()));
			if (typeId2 != null)
			{
				CurrentSchema.Id = typeId2;
			}
			if (required)
			{
				CurrentSchema.Required = true;
			}
			CurrentSchema.Title = GetTitle(type);
			CurrentSchema.Description = GetDescription(type);
			if (jsonConverter != null)
			{
				CurrentSchema.Type = JsonSchemaType.Any;
			}
			else
			{
				switch (jsonContract.ContractType)
				{
				case JsonContractType.Object:
					CurrentSchema.Type = AddNullType(JsonSchemaType.Object, valueRequired);
					CurrentSchema.Id = GetTypeId(type, explicitOnly: false);
					GenerateObjectSchema(type, (JsonObjectContract)jsonContract);
					break;
				case JsonContractType.Array:
				{
					CurrentSchema.Type = AddNullType(JsonSchemaType.Array, valueRequired);
					CurrentSchema.Id = GetTypeId(type, explicitOnly: false);
					bool flag = JsonTypeReflector.GetCachedAttribute<JsonArrayAttribute>(type)?.AllowNullItems ?? true;
					Type collectionItemType = ReflectionUtils.GetCollectionItemType(type);
					if (collectionItemType != null)
					{
						CurrentSchema.Items = new List<JsonSchema>();
						CurrentSchema.Items.Add(GenerateInternal(collectionItemType, (!flag) ? Required.Always : Required.Default, required: false));
					}
					break;
				}
				case JsonContractType.Primitive:
					CurrentSchema.Type = GetJsonSchemaType(type, valueRequired);
					if (CurrentSchema.Type == JsonSchemaType.Integer && type.IsEnum() && !type.IsDefined(typeof(FlagsAttribute), inherit: true))
					{
						CurrentSchema.Enum = new List<JToken>();
						IList<EnumValue<long>> namesAndValues = EnumUtils.GetNamesAndValues<long>(type);
						foreach (EnumValue<long> item2 in namesAndValues)
						{
							JToken item = JToken.FromObject(item2.Value);
							CurrentSchema.Enum.Add(item);
						}
					}
					break;
				case JsonContractType.String:
				{
					JsonSchemaType value = (!ReflectionUtils.IsNullable(jsonContract.UnderlyingType)) ? JsonSchemaType.String : AddNullType(JsonSchemaType.String, valueRequired);
					CurrentSchema.Type = value;
					break;
				}
				case JsonContractType.Dictionary:
				{
					CurrentSchema.Type = AddNullType(JsonSchemaType.Object, valueRequired);
					ReflectionUtils.GetDictionaryKeyValueTypes(type, out Type keyType, out Type valueType);
					if (keyType != null)
					{
						JsonContract jsonContract2 = ContractResolver.ResolveContract(keyType);
						if (jsonContract2.ContractType == JsonContractType.Primitive)
						{
							CurrentSchema.AdditionalProperties = GenerateInternal(valueType, Required.Default, required: false);
						}
					}
					break;
				}
				case JsonContractType.Serializable:
					CurrentSchema.Type = AddNullType(JsonSchemaType.Object, valueRequired);
					CurrentSchema.Id = GetTypeId(type, explicitOnly: false);
					GenerateISerializableContract(type, (JsonISerializableContract)jsonContract);
					break;
				case JsonContractType.Dynamic:
				case JsonContractType.Linq:
					CurrentSchema.Type = JsonSchemaType.Any;
					break;
				default:
					throw new JsonException("Unexpected contract type: {0}".FormatWith(CultureInfo.InvariantCulture, jsonContract));
				}
			}
			return Pop().Schema;
		}

		private JsonSchemaType AddNullType(JsonSchemaType type, Required valueRequired)
		{
			if (valueRequired != Required.Always)
			{
				return type | JsonSchemaType.Null;
			}
			return type;
		}

		private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		private void GenerateObjectSchema(Type type, JsonObjectContract contract)
		{
			CurrentSchema.Properties = new Dictionary<string, JsonSchema>();
			foreach (JsonProperty property in contract.Properties)
			{
				if (!property.Ignored)
				{
					bool flag = property.NullValueHandling == NullValueHandling.Ignore || HasFlag(property.DefaultValueHandling.GetValueOrDefault(), DefaultValueHandling.Ignore) || property.ShouldSerialize != null || property.GetIsSpecified != null;
					JsonSchema jsonSchema = GenerateInternal(property.PropertyType, property.Required, !flag);
					if (property.DefaultValue != null)
					{
						jsonSchema.Default = JToken.FromObject(property.DefaultValue);
					}
					CurrentSchema.Properties.Add(property.PropertyName, jsonSchema);
				}
			}
			if (type.IsSealed())
			{
				CurrentSchema.AllowAdditionalProperties = false;
			}
		}

		private void GenerateISerializableContract(Type type, JsonISerializableContract contract)
		{
			CurrentSchema.AllowAdditionalProperties = true;
		}

		internal static bool HasFlag(JsonSchemaType? value, JsonSchemaType flag)
		{
			if (!value.HasValue)
			{
				return true;
			}
			if ((value & flag) == flag)
			{
				return true;
			}
			if (flag == JsonSchemaType.Integer && ((int?)value & 2) == 2)
			{
				return true;
			}
			return false;
		}

		private JsonSchemaType GetJsonSchemaType(Type type, Required valueRequired)
		{
			JsonSchemaType jsonSchemaType = JsonSchemaType.None;
			if (valueRequired != Required.Always && ReflectionUtils.IsNullable(type))
			{
				jsonSchemaType = JsonSchemaType.Null;
				if (ReflectionUtils.IsNullableType(type))
				{
					type = Nullable.GetUnderlyingType(type);
				}
			}
			PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(type);
			switch (typeCode)
			{
			case PrimitiveTypeCode.Empty:
			case PrimitiveTypeCode.Object:
				return jsonSchemaType | JsonSchemaType.String;
			case PrimitiveTypeCode.DBNull:
				return jsonSchemaType | JsonSchemaType.Null;
			case PrimitiveTypeCode.Boolean:
				return jsonSchemaType | JsonSchemaType.Boolean;
			case PrimitiveTypeCode.Char:
				return jsonSchemaType | JsonSchemaType.String;
			case PrimitiveTypeCode.SByte:
			case PrimitiveTypeCode.Int16:
			case PrimitiveTypeCode.UInt16:
			case PrimitiveTypeCode.Int32:
			case PrimitiveTypeCode.Byte:
			case PrimitiveTypeCode.UInt32:
			case PrimitiveTypeCode.Int64:
			case PrimitiveTypeCode.UInt64:
			case PrimitiveTypeCode.BigInteger:
				return jsonSchemaType | JsonSchemaType.Integer;
			case PrimitiveTypeCode.Single:
			case PrimitiveTypeCode.Double:
			case PrimitiveTypeCode.Decimal:
				return jsonSchemaType | JsonSchemaType.Float;
			case PrimitiveTypeCode.DateTime:
			case PrimitiveTypeCode.DateTimeOffset:
				return jsonSchemaType | JsonSchemaType.String;
			case PrimitiveTypeCode.Guid:
			case PrimitiveTypeCode.TimeSpan:
			case PrimitiveTypeCode.Uri:
			case PrimitiveTypeCode.String:
			case PrimitiveTypeCode.Bytes:
				return jsonSchemaType | JsonSchemaType.String;
			default:
				throw new JsonException("Unexpected type code '{0}' for type '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeCode, type));
			}
		}
	}
}
