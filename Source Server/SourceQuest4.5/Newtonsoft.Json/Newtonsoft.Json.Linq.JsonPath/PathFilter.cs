using Newtonsoft.Json.Utilities;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal abstract class PathFilter
	{
		public abstract IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch);

		protected static JToken GetTokenIndex(JToken t, bool errorWhenNoMatch, int index)
		{
			JArray jArray = t as JArray;
			JConstructor jConstructor = t as JConstructor;
			if (jArray != null)
			{
				if (jArray.Count <= index)
				{
					if (errorWhenNoMatch)
					{
						throw new JsonException("Index {0} outside the bounds of JArray.".FormatWith(CultureInfo.InvariantCulture, index));
					}
					return null;
				}
				return jArray[index];
			}
			if (jConstructor != null)
			{
				if (jConstructor.Count <= index)
				{
					if (errorWhenNoMatch)
					{
						throw new JsonException("Index {0} outside the bounds of JConstructor.".FormatWith(CultureInfo.InvariantCulture, index));
					}
					return null;
				}
				return jConstructor[index];
			}
			if (errorWhenNoMatch)
			{
				throw new JsonException("Index {0} not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, index, t.GetType().Name));
			}
			return null;
		}
	}
}
