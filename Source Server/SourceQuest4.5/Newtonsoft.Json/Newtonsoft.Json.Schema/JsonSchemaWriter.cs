using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	internal class JsonSchemaWriter
	{
		private readonly JsonWriter _writer;

		private readonly JsonSchemaResolver _resolver;

		public JsonSchemaWriter(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			_writer = writer;
			_resolver = resolver;
		}

		private void ReferenceOrWriteSchema(JsonSchema schema)
		{
			if (schema.Id != null && _resolver.GetSchema(schema.Id) != null)
			{
				_writer.WriteStartObject();
				_writer.WritePropertyName("$ref");
				_writer.WriteValue(schema.Id);
				_writer.WriteEndObject();
			}
			else
			{
				WriteSchema(schema);
			}
		}

		public void WriteSchema(JsonSchema schema)
		{
			ValidationUtils.ArgumentNotNull(schema, "schema");
			if (!_resolver.LoadedSchemas.Contains(schema))
			{
				_resolver.LoadedSchemas.Add(schema);
			}
			_writer.WriteStartObject();
			WritePropertyIfNotNull(_writer, "id", schema.Id);
			WritePropertyIfNotNull(_writer, "title", schema.Title);
			WritePropertyIfNotNull(_writer, "description", schema.Description);
			WritePropertyIfNotNull(_writer, "required", schema.Required);
			WritePropertyIfNotNull(_writer, "readonly", schema.ReadOnly);
			WritePropertyIfNotNull(_writer, "hidden", schema.Hidden);
			WritePropertyIfNotNull(_writer, "transient", schema.Transient);
			if (schema.Type.HasValue)
			{
				WriteType("type", _writer, schema.Type.Value);
			}
			if (!schema.AllowAdditionalProperties)
			{
				_writer.WritePropertyName("additionalProperties");
				_writer.WriteValue(schema.AllowAdditionalProperties);
			}
			else if (schema.AdditionalProperties != null)
			{
				_writer.WritePropertyName("additionalProperties");
				ReferenceOrWriteSchema(schema.AdditionalProperties);
			}
			if (!schema.AllowAdditionalItems)
			{
				_writer.WritePropertyName("additionalItems");
				_writer.WriteValue(schema.AllowAdditionalItems);
			}
			else if (schema.AdditionalItems != null)
			{
				_writer.WritePropertyName("additionalItems");
				ReferenceOrWriteSchema(schema.AdditionalItems);
			}
			WriteSchemaDictionaryIfNotNull(_writer, "properties", schema.Properties);
			WriteSchemaDictionaryIfNotNull(_writer, "patternProperties", schema.PatternProperties);
			WriteItems(schema);
			WritePropertyIfNotNull(_writer, "minimum", schema.Minimum);
			WritePropertyIfNotNull(_writer, "maximum", schema.Maximum);
			WritePropertyIfNotNull(_writer, "exclusiveMinimum", schema.ExclusiveMinimum);
			WritePropertyIfNotNull(_writer, "exclusiveMaximum", schema.ExclusiveMaximum);
			WritePropertyIfNotNull(_writer, "minLength", schema.MinimumLength);
			WritePropertyIfNotNull(_writer, "maxLength", schema.MaximumLength);
			WritePropertyIfNotNull(_writer, "minItems", schema.MinimumItems);
			WritePropertyIfNotNull(_writer, "maxItems", schema.MaximumItems);
			WritePropertyIfNotNull(_writer, "divisibleBy", schema.DivisibleBy);
			WritePropertyIfNotNull(_writer, "format", schema.Format);
			WritePropertyIfNotNull(_writer, "pattern", schema.Pattern);
			if (schema.Enum != null)
			{
				_writer.WritePropertyName("enum");
				_writer.WriteStartArray();
				foreach (JToken item in schema.Enum)
				{
					item.WriteTo(_writer);
				}
				_writer.WriteEndArray();
			}
			if (schema.Default != null)
			{
				_writer.WritePropertyName("default");
				schema.Default.WriteTo(_writer);
			}
			if (schema.Disallow.HasValue)
			{
				WriteType("disallow", _writer, schema.Disallow.Value);
			}
			if (schema.Extends != null && schema.Extends.Count > 0)
			{
				_writer.WritePropertyName("extends");
				if (schema.Extends.Count == 1)
				{
					ReferenceOrWriteSchema(schema.Extends[0]);
				}
				else
				{
					_writer.WriteStartArray();
					foreach (JsonSchema extend in schema.Extends)
					{
						ReferenceOrWriteSchema(extend);
					}
					_writer.WriteEndArray();
				}
			}
			_writer.WriteEndObject();
		}

		private void WriteSchemaDictionaryIfNotNull(JsonWriter writer, string propertyName, IDictionary<string, JsonSchema> properties)
		{
			if (properties != null)
			{
				writer.WritePropertyName(propertyName);
				writer.WriteStartObject();
				foreach (KeyValuePair<string, JsonSchema> property in properties)
				{
					writer.WritePropertyName(property.Key);
					ReferenceOrWriteSchema(property.Value);
				}
				writer.WriteEndObject();
			}
		}

		private void WriteItems(JsonSchema schema)
		{
			if (schema.Items == null && !schema.PositionalItemsValidation)
			{
				return;
			}
			_writer.WritePropertyName("items");
			if (!schema.PositionalItemsValidation)
			{
				if (schema.Items != null && schema.Items.Count > 0)
				{
					ReferenceOrWriteSchema(schema.Items[0]);
					return;
				}
				_writer.WriteStartObject();
				_writer.WriteEndObject();
				return;
			}
			_writer.WriteStartArray();
			if (schema.Items != null)
			{
				foreach (JsonSchema item in schema.Items)
				{
					ReferenceOrWriteSchema(item);
				}
			}
			_writer.WriteEndArray();
		}

		private void WriteType(string propertyName, JsonWriter writer, JsonSchemaType type)
		{
			IList<JsonSchemaType> list2;
			if (Enum.IsDefined(typeof(JsonSchemaType), type))
			{
				List<JsonSchemaType> list = new List<JsonSchemaType>();
				list.Add(type);
				list2 = list;
			}
			else
			{
				list2 = (from v in EnumUtils.GetFlagsValues(type)
					where v != JsonSchemaType.None
					select v).ToList();
			}
			if (list2.Count != 0)
			{
				writer.WritePropertyName(propertyName);
				if (list2.Count == 1)
				{
					writer.WriteValue(JsonSchemaBuilder.MapType(list2[0]));
					return;
				}
				writer.WriteStartArray();
				foreach (JsonSchemaType item in list2)
				{
					writer.WriteValue(JsonSchemaBuilder.MapType(item));
				}
				writer.WriteEndArray();
			}
		}

		private void WritePropertyIfNotNull(JsonWriter writer, string propertyName, object value)
		{
			if (value != null)
			{
				writer.WritePropertyName(propertyName);
				writer.WriteValue(value);
			}
		}
	}
}
