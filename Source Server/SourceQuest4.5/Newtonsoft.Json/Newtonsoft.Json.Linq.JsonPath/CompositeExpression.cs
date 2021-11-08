using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class CompositeExpression : QueryExpression
	{
		public List<QueryExpression> Expressions
		{
			get;
			set;
		}

		public CompositeExpression()
		{
			Expressions = new List<QueryExpression>();
		}

		public override bool IsMatch(JToken t)
		{
			switch (base.Operator)
			{
			case QueryOperator.And:
				foreach (QueryExpression expression in Expressions)
				{
					if (!expression.IsMatch(t))
					{
						return false;
					}
				}
				return true;
			case QueryOperator.Or:
				foreach (QueryExpression expression2 in Expressions)
				{
					if (expression2.IsMatch(t))
					{
						return true;
					}
				}
				return false;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
