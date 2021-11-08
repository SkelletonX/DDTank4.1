using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	internal class JsonSchemaModelBuilder
	{
		private JsonSchemaNodeCollection _nodes = new JsonSchemaNodeCollection();

		private Dictionary<JsonSchemaNode, JsonSchemaModel> _nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();

		private JsonSchemaNode _node;

		public JsonSchemaModel Build(JsonSchema schema)
		{
			_nodes = new JsonSchemaNodeCollection();
			_node = AddSchema(null, schema);
			_nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();
			return BuildNodeModel(_node);
		}

		public JsonSchemaNode AddSchema(JsonSchemaNode existingNode, JsonSchema schema)
		{
			string id;
			if (existingNode != null)
			{
				if (existingNode.Schemas.Contains(schema))
				{
					return existingNode;
				}
				id = JsonSchemaNode.GetId(existingNode.Schemas.Union(new JsonSchema[1]
				{
					schema
				}));
			}
			else
			{
				id = JsonSchemaNode.GetId(new JsonSchema[1]
				{
					schema
				});
			}
			if (_nodes.Contains(id))
			{
				return _nodes[id];
			}
			JsonSchemaNode jsonSchemaNode = (existingNode != null) ? existingNode.Combine(schema) : new JsonSchemaNode(schema);
			_nodes.Add(jsonSchemaNode);
			AddProperties(schema.Properties, jsonSchemaNode.Properties);
			AddProperties(schema.PatternProperties, jsonSchemaNode.PatternProperties);
			if (schema.Items != null)
			{
				for (int i = 0; i < schema.Items.Count; i++)
				{
					AddItem(jsonSchemaNode, i, schema.Items[i]);
				}
			}
			if (schema.AdditionalItems != null)
			{
				AddAdditionalItems(jsonSchemaNode, schema.AdditionalItems);
			}
			if (schema.AdditionalProperties != null)
			{
				AddAdditionalProperties(jsonSchemaNode, schema.AdditionalProperties);
			}
			if (schema.Extends != null)
			{
				foreach (JsonSchema extend in schema.Extends)
				{
					jsonSchemaNode = AddSchema(jsonSchemaNode, extend);
				}
				return jsonSchemaNode;
			}
			return jsonSchemaNode;
		}

		public void AddProperties(IDictionary<string, JsonSchema> source, IDictionary<string, JsonSchemaNode> target)
		{
			if (source != null)
			{
				foreach (KeyValuePair<string, JsonSchema> item in source)
				{
					AddProperty(target, item.Key, item.Value);
				}
			}
		}

		public void AddProperty(IDictionary<string, JsonSchemaNode> target, string propertyName, JsonSchema schema)
		{
			target.TryGetValue(propertyName, out JsonSchemaNode value);
			target[propertyName] = AddSchema(value, schema);
		}

		public void AddItem(JsonSchemaNode parentNode, int index, JsonSchema schema)
		{
			JsonSchemaNode existingNode = (parentNode.Items.Count > index) ? parentNode.Items[index] : null;
			JsonSchemaNode jsonSchemaNode = AddSchema(existingNode, schema);
			if (parentNode.Items.Count <= index)
			{
				parentNode.Items.Add(jsonSchemaNode);
			}
			else
			{
				parentNode.Items[index] = jsonSchemaNode;
			}
		}

		public void AddAdditionalProperties(JsonSchemaNode parentNode, JsonSchema schema)
		{
			parentNode.AdditionalProperties = AddSchema(parentNode.AdditionalProperties, schema);
		}

		public void AddAdditionalItems(JsonSchemaNode parentNode, JsonSchema schema)
		{
			parentNode.AdditionalItems = AddSchema(parentNode.AdditionalItems, schema);
		}

		private JsonSchemaModel BuildNodeModel(JsonSchemaNode node)
		{
			if (_nodeModels.TryGetValue(node, out JsonSchemaModel value))
			{
				return value;
			}
			value = JsonSchemaModel.Create(node.Schemas);
			_nodeModels[node] = value;
			foreach (KeyValuePair<string, JsonSchemaNode> property in node.Properties)
			{
				if (value.Properties == null)
				{
					value.Properties = new Dictionary<string, JsonSchemaModel>();
				}
				value.Properties[property.Key] = BuildNodeModel(property.Value);
			}
			foreach (KeyValuePair<string, JsonSchemaNode> patternProperty in node.PatternProperties)
			{
				if (value.PatternProperties == null)
				{
					value.PatternProperties = new Dictionary<string, JsonSchemaModel>();
				}
				value.PatternProperties[patternProperty.Key] = BuildNodeModel(patternProperty.Value);
			}
			foreach (JsonSchemaNode item in node.Items)
			{
				if (value.Items == null)
				{
					value.Items = new List<JsonSchemaModel>();
				}
				value.Items.Add(BuildNodeModel(item));
			}
			if (node.AdditionalProperties != null)
			{
				value.AdditionalProperties = BuildNodeModel(node.AdditionalProperties);
			}
			if (node.AdditionalItems != null)
			{
				value.AdditionalItems = BuildNodeModel(node.AdditionalItems);
			}
			return value;
		}
	}
}
