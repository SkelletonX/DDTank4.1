using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace Newtonsoft.Json.Utilities
{
	internal sealed class DynamicProxyMetaObject<T> : DynamicMetaObject
	{
		private delegate DynamicMetaObject Fallback(DynamicMetaObject errorSuggestion);

		private sealed class GetBinderAdapter : GetMemberBinder
		{
			internal GetBinderAdapter(InvokeMemberBinder binder)
				: base(binder.Name, binder.IgnoreCase)
			{
			}

			public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
			{
				throw new NotSupportedException();
			}
		}

		private readonly DynamicProxy<T> _proxy;

		private readonly bool _dontFallbackFirst;

		private static readonly Expression[] NoArgs = new Expression[0];

		private new T Value => (T)base.Value;

		internal DynamicProxyMetaObject(Expression expression, T value, DynamicProxy<T> proxy, bool dontFallbackFirst)
			: base(expression, BindingRestrictions.Empty, value)
		{
			_proxy = proxy;
			_dontFallbackFirst = dontFallbackFirst;
		}

		private bool IsOverridden(string method)
		{
			return ReflectionUtils.IsMethodOverridden(_proxy.GetType(), typeof(DynamicProxy<T>), method);
		}

		public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
		{
			if (!IsOverridden("TryGetMember"))
			{
				return base.BindGetMember(binder);
			}
			return CallMethodWithResult("TryGetMember", binder, NoArgs, (DynamicMetaObject e) => binder.FallbackGetMember(this, e));
		}

		public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
		{
			if (!IsOverridden("TrySetMember"))
			{
				return base.BindSetMember(binder, value);
			}
			return CallMethodReturnLast("TrySetMember", binder, GetArgs(value), (DynamicMetaObject e) => binder.FallbackSetMember(this, value, e));
		}

		public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
		{
			if (!IsOverridden("TryDeleteMember"))
			{
				return base.BindDeleteMember(binder);
			}
			return CallMethodNoResult("TryDeleteMember", binder, NoArgs, (DynamicMetaObject e) => binder.FallbackDeleteMember(this, e));
		}

		public override DynamicMetaObject BindConvert(ConvertBinder binder)
		{
			if (!IsOverridden("TryConvert"))
			{
				return base.BindConvert(binder);
			}
			return CallMethodWithResult("TryConvert", binder, NoArgs, (DynamicMetaObject e) => binder.FallbackConvert(this, e));
		}

		public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
		{
			if (!IsOverridden("TryInvokeMember"))
			{
				return base.BindInvokeMember(binder, args);
			}
			Fallback fallback = (DynamicMetaObject e) => binder.FallbackInvokeMember(this, args, e);
			DynamicMetaObject dynamicMetaObject = BuildCallMethodWithResult("TryInvokeMember", binder, GetArgArray(args), BuildCallMethodWithResult("TryGetMember", new GetBinderAdapter(binder), NoArgs, fallback(null), (DynamicMetaObject e) => binder.FallbackInvoke(e, args, null)), null);
			if (!_dontFallbackFirst)
			{
				return fallback(dynamicMetaObject);
			}
			return dynamicMetaObject;
		}

		public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
		{
			if (!IsOverridden("TryCreateInstance"))
			{
				return base.BindCreateInstance(binder, args);
			}
			return CallMethodWithResult("TryCreateInstance", binder, GetArgArray(args), (DynamicMetaObject e) => binder.FallbackCreateInstance(this, args, e));
		}

		public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
		{
			if (!IsOverridden("TryInvoke"))
			{
				return base.BindInvoke(binder, args);
			}
			return CallMethodWithResult("TryInvoke", binder, GetArgArray(args), (DynamicMetaObject e) => binder.FallbackInvoke(this, args, e));
		}

		public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
		{
			if (!IsOverridden("TryBinaryOperation"))
			{
				return base.BindBinaryOperation(binder, arg);
			}
			return CallMethodWithResult("TryBinaryOperation", binder, GetArgs(arg), (DynamicMetaObject e) => binder.FallbackBinaryOperation(this, arg, e));
		}

		public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
		{
			if (!IsOverridden("TryUnaryOperation"))
			{
				return base.BindUnaryOperation(binder);
			}
			return CallMethodWithResult("TryUnaryOperation", binder, NoArgs, (DynamicMetaObject e) => binder.FallbackUnaryOperation(this, e));
		}

		public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
		{
			if (!IsOverridden("TryGetIndex"))
			{
				return base.BindGetIndex(binder, indexes);
			}
			return CallMethodWithResult("TryGetIndex", binder, GetArgArray(indexes), (DynamicMetaObject e) => binder.FallbackGetIndex(this, indexes, e));
		}

		public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
		{
			if (!IsOverridden("TrySetIndex"))
			{
				return base.BindSetIndex(binder, indexes, value);
			}
			return CallMethodReturnLast("TrySetIndex", binder, GetArgArray(indexes, value), (DynamicMetaObject e) => binder.FallbackSetIndex(this, indexes, value, e));
		}

		public override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
		{
			if (!IsOverridden("TryDeleteIndex"))
			{
				return base.BindDeleteIndex(binder, indexes);
			}
			return CallMethodNoResult("TryDeleteIndex", binder, GetArgArray(indexes), (DynamicMetaObject e) => binder.FallbackDeleteIndex(this, indexes, e));
		}

		private static Expression[] GetArgs(params DynamicMetaObject[] args)
		{
			return args.Select((DynamicMetaObject arg) => Expression.Convert(arg.Expression, typeof(object))).ToArray();
		}

		private static Expression[] GetArgArray(DynamicMetaObject[] args)
		{
			return new NewArrayExpression[1]
			{
				Expression.NewArrayInit(typeof(object), GetArgs(args))
			};
		}

		private static Expression[] GetArgArray(DynamicMetaObject[] args, DynamicMetaObject value)
		{
			return new Expression[2]
			{
				Expression.NewArrayInit(typeof(object), GetArgs(args)),
				Expression.Convert(value.Expression, typeof(object))
			};
		}

		private static ConstantExpression Constant(DynamicMetaObjectBinder binder)
		{
			Type type = binder.GetType();
			while (!type.IsVisible())
			{
				type = type.BaseType();
			}
			return Expression.Constant(binder, type);
		}

		private DynamicMetaObject CallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, Fallback fallback, Fallback fallbackInvoke = null)
		{
			DynamicMetaObject fallbackResult = fallback(null);
			DynamicMetaObject dynamicMetaObject = BuildCallMethodWithResult(methodName, binder, args, fallbackResult, fallbackInvoke);
			if (!_dontFallbackFirst)
			{
				return fallback(dynamicMetaObject);
			}
			return dynamicMetaObject;
		}

		private DynamicMetaObject BuildCallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, DynamicMetaObject fallbackResult, Fallback fallbackInvoke)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), null);
			IList<Expression> list = new List<Expression>();
			list.Add(Expression.Convert(base.Expression, typeof(T)));
			list.Add(Constant(binder));
			list.AddRange(args);
			list.Add(parameterExpression);
			DynamicMetaObject dynamicMetaObject = new DynamicMetaObject(parameterExpression, BindingRestrictions.Empty);
			if (binder.ReturnType != typeof(object))
			{
				UnaryExpression expression = Expression.Convert(dynamicMetaObject.Expression, binder.ReturnType);
				dynamicMetaObject = new DynamicMetaObject(expression, dynamicMetaObject.Restrictions);
			}
			if (fallbackInvoke != null)
			{
				dynamicMetaObject = fallbackInvoke(dynamicMetaObject);
			}
			return new DynamicMetaObject(Expression.Block(new ParameterExpression[1]
			{
				parameterExpression
			}, Expression.Condition(Expression.Call(Expression.Constant(_proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), dynamicMetaObject.Expression, fallbackResult.Expression, binder.ReturnType)), GetRestrictions().Merge(dynamicMetaObject.Restrictions).Merge(fallbackResult.Restrictions));
		}

		private DynamicMetaObject CallMethodReturnLast(string methodName, DynamicMetaObjectBinder binder, Expression[] args, Fallback fallback)
		{
			DynamicMetaObject dynamicMetaObject = fallback(null);
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), null);
			IList<Expression> list = new List<Expression>();
			list.Add(Expression.Convert(base.Expression, typeof(T)));
			list.Add(Constant(binder));
			list.AddRange(args);
			list[args.Length + 1] = Expression.Assign(parameterExpression, list[args.Length + 1]);
			DynamicMetaObject dynamicMetaObject2 = new DynamicMetaObject(Expression.Block(new ParameterExpression[1]
			{
				parameterExpression
			}, Expression.Condition(Expression.Call(Expression.Constant(_proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), parameterExpression, dynamicMetaObject.Expression, typeof(object))), GetRestrictions().Merge(dynamicMetaObject.Restrictions));
			if (!_dontFallbackFirst)
			{
				return fallback(dynamicMetaObject2);
			}
			return dynamicMetaObject2;
		}

		private DynamicMetaObject CallMethodNoResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, Fallback fallback)
		{
			DynamicMetaObject dynamicMetaObject = fallback(null);
			IList<Expression> list = new List<Expression>();
			list.Add(Expression.Convert(base.Expression, typeof(T)));
			list.Add(Constant(binder));
			list.AddRange(args);
			DynamicMetaObject dynamicMetaObject2 = new DynamicMetaObject(Expression.Condition(Expression.Call(Expression.Constant(_proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), Expression.Empty(), dynamicMetaObject.Expression, typeof(void)), GetRestrictions().Merge(dynamicMetaObject.Restrictions));
			if (!_dontFallbackFirst)
			{
				return fallback(dynamicMetaObject2);
			}
			return dynamicMetaObject2;
		}

		private BindingRestrictions GetRestrictions()
		{
			if (Value != null || !base.HasValue)
			{
				return BindingRestrictions.GetTypeRestriction(base.Expression, base.LimitType);
			}
			return BindingRestrictions.GetInstanceRestriction(base.Expression, null);
		}

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return _proxy.GetDynamicMemberNames(Value);
		}
	}
}
