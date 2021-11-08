using System.Linq.Expressions;

namespace Newtonsoft.Json.Utilities
{
	internal class NoThrowExpressionVisitor : ExpressionVisitor
	{
		internal static readonly object ErrorResult = new object();

		protected override Expression VisitConditional(ConditionalExpression node)
		{
			if (node.IfFalse.NodeType == ExpressionType.Throw)
			{
				return Expression.Condition(node.Test, node.IfTrue, Expression.Constant(ErrorResult));
			}
			return base.VisitConditional(node);
		}
	}
}
