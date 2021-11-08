namespace System.Diagnostics.Contracts
{
	internal static class ReflectionContract
	{
		[Pure]
		public static bool TryGetIsInterface(Type type, out bool result)
		{
			try
			{
				result = type.IsInterface;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		[Pure]
		public static bool TryGetIsClass(Type type, out bool result)
		{
			try
			{
				result = type.IsClass;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		[Pure]
		public static bool TryGetIsValueType(Type type, out bool result)
		{
			try
			{
				result = type.IsValueType;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		public static bool IsGenericType(Type type)
		{
			if (TryGetIsGenericType(type, out bool result))
			{
				return result;
			}
			return false;
		}

		public static bool TryGetIsGenericType(Type type, out bool result)
		{
			try
			{
				result = type.IsGenericType;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		[Pure]
		public static bool TryGetIsGenericTypeDefinition(Type type, out bool result)
		{
			try
			{
				result = type.IsGenericTypeDefinition;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		[Pure]
		public static bool IsGenericTypeDefinition(Type type)
		{
			if (TryGetIsGenericTypeDefinition(type, out bool result))
			{
				return result;
			}
			return false;
		}

		[Pure]
		public static bool TryGetContainsGenericParameters(Type type, out bool result)
		{
			try
			{
				result = type.ContainsGenericParameters;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		[Pure]
		public static bool ContainsGenericParameters(Type type)
		{
			if (TryGetContainsGenericParameters(type, out bool result))
			{
				return result;
			}
			return false;
		}

		[Pure]
		public static bool IsClassOrValueType(Type type)
		{
			if (!TryGetIsClass(type, out bool result) || !TryGetIsValueType(type, out bool result2))
			{
				return false;
			}
			if (!result2 && !result)
			{
				return false;
			}
			return true;
		}

		[Pure]
		public static bool IsInterface(Type type)
		{
			if (TryGetIsInterface(type, out bool result))
			{
				return result;
			}
			return false;
		}

		[Pure]
		public static bool IsClassOrInterface(Type type)
		{
			if (!TryGetIsClass(type, out bool result) || !TryGetIsInterface(type, out bool result2))
			{
				return false;
			}
			if (!result2 && !result)
			{
				return false;
			}
			return true;
		}

		[Pure]
		public static bool IsClassOrInterfaceOrValueType(Type type)
		{
			if (!TryGetIsClass(type, out bool result) || !TryGetIsInterface(type, out bool result2) || !TryGetIsValueType(type, out bool result3))
			{
				return false;
			}
			if (!result3 && !result && !result2)
			{
				return false;
			}
			return true;
		}

		[Pure]
		public static bool IsTypeDefinition(Type type)
		{
			if (!IsGenericTypeDefinition(type))
			{
				return !ContainsGenericParameters(type);
			}
			return true;
		}

		[Pure]
		public static bool IsDelegate(Type type)
		{
			if ((object)type.BaseType != typeof(Delegate))
			{
				return (object)type.BaseType == typeof(MulticastDelegate);
			}
			return true;
		}
	}
}
