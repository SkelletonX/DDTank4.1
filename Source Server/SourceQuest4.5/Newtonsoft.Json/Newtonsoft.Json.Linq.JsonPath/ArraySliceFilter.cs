using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class ArraySliceFilter : PathFilter
	{
		public int? Start
		{
			get;
			set;
		}

		public int? End
		{
			get;
			set;
		}

		public int? Step
		{
			get;
			set;
		}

		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			int? step = Step;
			if (step.GetValueOrDefault() == 0 && step.HasValue)
			{
				throw new JsonException("Step cannot be zero.");
			}
			foreach (JToken t in current)
			{
				JArray a = t as JArray;
				if (a != null)
				{
					int stepCount = Step ?? 1;
					int startIndex3 = Start ?? ((stepCount <= 0) ? (a.Count - 1) : 0);
					int stopIndex3 = End ?? ((stepCount > 0) ? a.Count : (-1));
					if (Start < 0)
					{
						startIndex3 = a.Count + startIndex3;
					}
					if (End < 0)
					{
						stopIndex3 = a.Count + stopIndex3;
					}
					startIndex3 = Math.Max(startIndex3, (stepCount <= 0) ? int.MinValue : 0);
					startIndex3 = Math.Min(startIndex3, (stepCount > 0) ? a.Count : (a.Count - 1));
					stopIndex3 = Math.Max(stopIndex3, -1);
					stopIndex3 = Math.Min(stopIndex3, a.Count);
					bool positiveStep = stepCount > 0;
					if (IsValid(startIndex3, stopIndex3, positiveStep))
					{
						for (int i = startIndex3; IsValid(i, stopIndex3, positiveStep); i += stepCount)
						{
							yield return a[i];
						}
					}
					else if (errorWhenNoMatch)
					{
						throw new JsonException("Array slice of {0} to {1} returned no results.".FormatWith(CultureInfo.InvariantCulture, Start.HasValue ? Start.Value.ToString(CultureInfo.InvariantCulture) : "*", End.HasValue ? End.Value.ToString(CultureInfo.InvariantCulture) : "*"));
					}
				}
				else if (errorWhenNoMatch)
				{
					throw new JsonException("Array slice is not valid on {0}.".FormatWith(CultureInfo.InvariantCulture, t.GetType().Name));
				}
			}
		}

		private bool IsValid(int index, int stopIndex, bool positiveStep)
		{
			if (positiveStep)
			{
				return index < stopIndex;
			}
			return index > stopIndex;
		}
	}
}
