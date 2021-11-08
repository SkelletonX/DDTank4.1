using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class BooleanQueryExpression : QueryExpression
	{
		public List<PathFilter> Path
		{
			get;
			set;
		}

		public JValue Value
		{
			get;
			set;
		}

		public override bool IsMatch(JToken t)
		{
			IEnumerable<JToken> enumerable = JPath.Evaluate(Path, t, errorWhenNoMatch: false);
			foreach (JToken item in enumerable)
			{
				JValue jValue = item as JValue;
				switch (base.Operator)
				{
				case QueryOperator.Equals:
					if (jValue != null && jValue.Equals(Value))
					{
						return true;
					}
					break;
				case QueryOperator.NotEquals:
					if (jValue != null && !jValue.Equals(Value))
					{
						return true;
					}
					break;
				case QueryOperator.GreaterThan:
					if (jValue != null && jValue.CompareTo(Value) > 0)
					{
						return true;
					}
					break;
				case QueryOperator.GreaterThanOrEquals:
					if (jValue != null && jValue.CompareTo(Value) >= 0)
					{
						return true;
					}
					break;
				case QueryOperator.LessThan:
					if (jValue != null && jValue.CompareTo(Value) < 0)
					{
						return true;
					}
					break;
				case QueryOperator.LessThanOrEquals:
					if (jValue != null && jValue.CompareTo(Value) <= 0)
					{
						return true;
					}
					break;
				case QueryOperator.Exists:
					return true;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			return false;
		}
	}
}
