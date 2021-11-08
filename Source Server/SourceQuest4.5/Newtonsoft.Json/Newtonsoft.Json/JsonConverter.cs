using Newtonsoft.Json.Schema;
using System;

namespace Newtonsoft.Json
{
	public abstract class JsonConverter
	{
		public virtual bool CanRead => true;

		public virtual bool CanWrite => true;

		public abstract void WriteJson(JsonWriter writer, object value, JsonSerializer serializer);

		public abstract object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);

		public abstract bool CanConvert(Type objectType);

		public virtual JsonSchema GetSchema()
		{
			return null;
		}
	}
}
