using Newtonsoft.Json.Utilities;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class ArrayIndexFilter : PathFilter
	{
		public int? Index
		{
			get;
			set;
		}

		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken t in current)
			{
				if (Index.HasValue)
				{
					JToken v = PathFilter.GetTokenIndex(t, errorWhenNoMatch, Index.Value);
					if (v != null)
					{
						yield return v;
					}
				}
				else if (t is JArray || t is JConstructor)
				{
					foreach (JToken item in (IEnumerable<JToken>)t)
					{
						yield return item;
					}
				}
				else if (errorWhenNoMatch)
				{
					throw new JsonException("Index * not valid on {0}.".FormatWith(CultureInfo.InvariantCulture, t.GetType().Name));
				}
			}
		}
	}
}
