using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
	public class DataTableConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			DataTable dataTable = (DataTable)value;
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			writer.WriteStartArray();
			foreach (DataRow row in dataTable.Rows)
			{
				writer.WriteStartObject();
				foreach (DataColumn column in row.Table.Columns)
				{
					if (serializer.NullValueHandling != NullValueHandling.Ignore || (row[column] != null && row[column] != DBNull.Value))
					{
						writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName(column.ColumnName) : column.ColumnName);
						serializer.Serialize(writer, row[column]);
					}
				}
				writer.WriteEndObject();
			}
			writer.WriteEndArray();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			DataTable dataTable = existingValue as DataTable;
			if (dataTable == null)
			{
				dataTable = ((objectType == typeof(DataTable)) ? new DataTable() : ((DataTable)Activator.CreateInstance(objectType)));
			}
			if (reader.TokenType == JsonToken.PropertyName)
			{
				dataTable.TableName = (string)reader.Value;
				CheckedRead(reader);
			}
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable. Expected StartArray, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			CheckedRead(reader);
			while (reader.TokenType != JsonToken.EndArray)
			{
				CreateRow(reader, dataTable);
				CheckedRead(reader);
			}
			return dataTable;
		}

		private static void CreateRow(JsonReader reader, DataTable dt)
		{
			DataRow dataRow = dt.NewRow();
			CheckedRead(reader);
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string text = (string)reader.Value;
				CheckedRead(reader);
				DataColumn dataColumn = dt.Columns[text];
				if (dataColumn == null)
				{
					Type columnDataType = GetColumnDataType(reader);
					dataColumn = new DataColumn(text, columnDataType);
					dt.Columns.Add(dataColumn);
				}
				if (dataColumn.DataType == typeof(DataTable))
				{
					if (reader.TokenType == JsonToken.StartArray)
					{
						CheckedRead(reader);
					}
					DataTable dataTable = new DataTable();
					while (reader.TokenType != JsonToken.EndArray)
					{
						CreateRow(reader, dataTable);
						CheckedRead(reader);
					}
					dataRow[text] = dataTable;
				}
				else if (dataColumn.DataType.IsArray && dataColumn.DataType != typeof(byte[]))
				{
					if (reader.TokenType == JsonToken.StartArray)
					{
						CheckedRead(reader);
					}
					List<object> list = new List<object>();
					while (reader.TokenType != JsonToken.EndArray)
					{
						list.Add(reader.Value);
						CheckedRead(reader);
					}
					Array array = Array.CreateInstance(dataColumn.DataType.GetElementType(), list.Count);
					Array.Copy(list.ToArray(), array, list.Count);
					dataRow[text] = array;
				}
				else
				{
					dataRow[text] = (reader.Value ?? DBNull.Value);
				}
				CheckedRead(reader);
			}
			dataRow.EndEdit();
			dt.Rows.Add(dataRow);
		}

		private static Type GetColumnDataType(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			switch (tokenType)
			{
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Date:
			case JsonToken.Bytes:
				return reader.ValueType;
			case JsonToken.Null:
			case JsonToken.Undefined:
				return typeof(string);
			case JsonToken.StartArray:
			{
				CheckedRead(reader);
				if (reader.TokenType == JsonToken.StartObject)
				{
					return typeof(DataTable);
				}
				Type columnDataType = GetColumnDataType(reader);
				return columnDataType.MakeArrayType();
			}
			default:
				throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable: {0}".FormatWith(CultureInfo.InvariantCulture, tokenType));
			}
		}

		private static void CheckedRead(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading DataTable.");
			}
		}

		public override bool CanConvert(Type valueType)
		{
			return typeof(DataTable).IsAssignableFrom(valueType);
		}
	}
}
