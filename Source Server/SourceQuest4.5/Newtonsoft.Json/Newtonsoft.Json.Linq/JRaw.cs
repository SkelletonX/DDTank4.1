using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Linq
{
	public class JRaw : JValue
	{
		public JRaw(JRaw other)
			: base(other)
		{
		}

		public JRaw(object rawJson)
			: base(rawJson, JTokenType.Raw)
		{
		}

		public static JRaw Create(JsonReader reader)
		{
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
				{
					jsonTextWriter.WriteToken(reader);
					return new JRaw(stringWriter.ToString());
				}
			}
		}

		internal override JToken CloneToken()
		{
			return new JRaw(this);
		}
	}
}
