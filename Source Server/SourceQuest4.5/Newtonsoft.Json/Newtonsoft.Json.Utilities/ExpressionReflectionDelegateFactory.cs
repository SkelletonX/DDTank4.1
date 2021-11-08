using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
	internal class ExpressionReflectionDelegateFactory : ReflectionDelegateFactory
	{
		private class ByRefParameter
		{
			public Expression Value;

			public ParameterExpression Variable;

			public bool IsOut;
		}

		private static readonly ExpressionReflectionDelegateFactory _instance = new ExpressionReflectionDelegateFactory();

		internal static ReflectionDelegateFactory Instance => _instance;

		public override ObjectConstructor<object> CreateParametrizedConstructor(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			Type typeFromHandle = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]), "args");
			Expression body = BuildMethodCall(method, typeFromHandle, null, parameterExpression);
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(ObjectConstructor<object>), body, parameterExpression);
			return (ObjectConstructor<object>)lambdaExpression.Compile();
		}

		public override MethodCall<T, object> CreateMethodCall<T>(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			Type typeFromHandle = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "target");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object[]), "args");
			Expression body = BuildMethodCall(method, typeFromHandle, parameterExpression, parameterExpression2);
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(MethodCall<T, object>), body, parameterExpression, parameterExpression2);
			return (MethodCall<T, object>)lambdaExpression.Compile();
		}

		private Expression BuildMethodCall(MethodBase method, Type type, ParameterExpression targetParameterExpression, ParameterExpression argsParameterExpression)
		{
			ParameterInfo[] parameters = method.GetParameters();
			Expression[] array = new Expression[parameters.Length];
			IList<ByRefParameter> list = new List<ByRefParameter>();
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				Type type2 = parameterInfo.ParameterType;
				bool flag = false;
				if (type2.IsByRef)
				{
					type2 = type2.GetElementType();
					flag = true;
				}
				Expression index = Expression.Constant(i);
				Expression expression = Expression.ArrayIndex(argsParameterExpression, index);
				Expression expression3;
				if (type2.IsValueType())
				{
					BinaryExpression expression2 = Expression.Coalesce(expression, Expression.New(type2));
					expression3 = EnsureCastExpression(expression2, type2);
				}
				else
				{
					expression3 = EnsureCastExpression(expression, type2);
				}
				if (flag)
				{
					ParameterExpression parameterExpression = Expression.Variable(type2);
					list.Add(new ByRefParameter
					{
						Value = expression3,
						Variable = parameterExpression,
						IsOut = parameterInfo.IsOut
					});
					expression3 = parameterExpression;
				}
				array[i] = expression3;
			}
			Expression expression4;
			if (method.IsConstructor)
			{
				expression4 = Expression.New((ConstructorInfo)method, array);
			}
			else if (method.IsStatic)
			{
				expression4 = Expression.Call((MethodInfo)method, array);
			}
			else
			{
				Expression instance = EnsureCastExpression(targetParameterExpression, method.DeclaringType);
				expression4 = Expression.Call(instance, (MethodInfo)method, array);
			}
			if (method is MethodInfo)
			{
				MethodInfo methodInfo = (MethodInfo)method;
				expression4 = ((!(methodInfo.ReturnType != typeof(void))) ? Expression.Block(expression4, Expression.Constant(null)) : EnsureCastExpression(expression4, type));
			}
			else
			{
				expression4 = EnsureCastExpression(expression4, type);
			}
			if (list.Count > 0)
			{
				IList<ParameterExpression> list2 = new List<ParameterExpression>();
				IList<Expression> list3 = new List<Expression>();
				foreach (ByRefParameter item in list)
				{
					if (!item.IsOut)
					{
						list3.Add(Expression.Assign(item.Variable, item.Value));
					}
					list2.Add(item.Variable);
				}
				list3.Add(expression4);
				expression4 = Expression.Block(list2, list3);
			}
			return expression4;
		}

		public override Func<T> CreateDefaultConstructor<T>(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsAbstract())
			{
				return () => (T)Activator.CreateInstance(type);
			}
			try
			{
				Type typeFromHandle = typeof(T);
				Expression expression = Expression.New(type);
				expression = EnsureCastExpression(expression, typeFromHandle);
				LambdaExpression lambdaExpression = Expression.Lambda(typeof(Func<T>), expression);
				return (Func<T>)lambdaExpression.Compile();
			}
			catch
			{
				return () => (T)Activator.CreateInstance(type);
			}
		}

		public override Func<T, object> CreateGet<T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "instance");
			MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);
			Expression expression;
			if (getMethod.IsStatic)
			{
				expression = Expression.MakeMemberAccess(null, propertyInfo);
			}
			else
			{
				Expression expression2 = EnsureCastExpression(parameterExpression, propertyInfo.DeclaringType);
				expression = Expression.MakeMemberAccess(expression2, propertyInfo);
			}
			expression = EnsureCastExpression(expression, typeFromHandle2);
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(Func<T, object>), expression, parameterExpression);
			return (Func<T, object>)lambdaExpression.Compile();
		}

		public override Func<T, object> CreateGet<T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "source");
			Expression expression;
			if (fieldInfo.IsStatic)
			{
				expression = Expression.Field(null, fieldInfo);
			}
			else
			{
				Expression expression2 = EnsureCastExpression(parameterExpression, fieldInfo.DeclaringType);
				expression = Expression.Field(expression2, fieldInfo);
			}
			expression = EnsureCastExpression(expression, typeof(object));
			return Expression.Lambda<Func<T, object>>(expression, new ParameterExpression[1]
			{
				parameterExpression
			}).Compile();
		}

		public override Action<T, object> CreateSet<T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			if (fieldInfo.DeclaringType.IsValueType() || fieldInfo.IsInitOnly)
			{
				return LateBoundReflectionDelegateFactory.Instance.CreateSet<T>(fieldInfo);
			}
			ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "source");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object), "value");
			Expression expression;
			if (fieldInfo.IsStatic)
			{
				expression = Expression.Field(null, fieldInfo);
			}
			else
			{
				Expression expression2 = EnsureCastExpression(parameterExpression, fieldInfo.DeclaringType);
				expression = Expression.Field(expression2, fieldInfo);
			}
			Expression right = EnsureCastExpression(parameterExpression2, expression.Type);
			BinaryExpression body = Expression.Assign(expression, right);
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(Action<T, object>), body, parameterExpression, parameterExpression2);
			return (Action<T, object>)lambdaExpression.Compile();
		}

		public override Action<T, object> CreateSet<T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			if (propertyInfo.DeclaringType.IsValueType())
			{
				return LateBoundReflectionDelegateFactory.Instance.CreateSet<T>(propertyInfo);
			}
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "instance");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeFromHandle2, "value");
			Expression expression = EnsureCastExpression(parameterExpression2, propertyInfo.PropertyType);
			MethodInfo setMethod = propertyInfo.GetSetMethod(nonPublic: true);
			Expression body;
			if (setMethod.IsStatic)
			{
				body = Expression.Call(setMethod, expression);
			}
			else
			{
				Expression instance = EnsureCastExpression(parameterExpression, propertyInfo.DeclaringType);
				body = Expression.Call(instance, setMethod, expression);
			}
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(Action<T, object>), body, parameterExpression, parameterExpression2);
			return (Action<T, object>)lambdaExpression.Compile();
		}

		private Expression EnsureCastExpression(Expression expression, Type targetType)
		{
			Type type = expression.Type;
			if (type == targetType || (!type.IsValueType() && targetType.IsAssignableFrom(type)))
			{
				return expression;
			}
			return Expression.Convert(expression, targetType);
		}
	}
}
