using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace ProtoBuf
{
	internal sealed class Helpers
	{
		public static readonly Type[] EmptyTypes = Type.EmptyTypes;

		private Helpers()
		{
		}

		public static StringBuilder AppendLine(StringBuilder builder)
		{
			return builder.AppendLine();
		}

		public static bool IsNullOrEmpty(string value)
		{
			if (value != null)
			{
				return value.Length == 0;
			}
			return true;
		}

		[Conditional("DEBUG")]
		public static void DebugWriteLine(string message, object obj)
		{
		}

		[Conditional("DEBUG")]
		public static void DebugWriteLine(string message)
		{
		}

		[Conditional("TRACE")]
		public static void TraceWriteLine(string message)
		{
		}

		[Conditional("DEBUG")]
		public static void DebugAssert(bool condition, string message)
		{
		}

		[Conditional("DEBUG")]
		public static void DebugAssert(bool condition, string message, params object[] args)
		{
		}

		[Conditional("DEBUG")]
		public static void DebugAssert(bool condition)
		{
		}

		public static void Sort(int[] keys, object[] values)
		{
			bool swapped;
			do
			{
				swapped = false;
				for (int i = 1; i < keys.Length; i++)
				{
					if (keys[i - 1] > keys[i])
					{
						int tmpKey = keys[i];
						keys[i] = keys[i - 1];
						keys[i - 1] = tmpKey;
						object tmpValue = values[i];
						values[i] = values[i - 1];
						values[i - 1] = tmpValue;
						swapped = true;
					}
				}
			}
			while (swapped);
		}

		public static void BlockCopy(byte[] from, int fromIndex, byte[] to, int toIndex, int count)
		{
			Buffer.BlockCopy(from, fromIndex, to, toIndex, count);
		}

		public static bool IsInfinity(float value)
		{
			return float.IsInfinity(value);
		}

		internal static MethodInfo GetInstanceMethod(Type declaringType, string name)
		{
			return declaringType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		}

		internal static MethodInfo GetStaticMethod(Type declaringType, string name)
		{
			return declaringType.GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		internal static MethodInfo GetStaticMethod(Type declaringType, string name, Type[] parameterTypes)
		{
			return declaringType.GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, parameterTypes, null);
		}

		internal static MethodInfo GetInstanceMethod(Type declaringType, string name, Type[] types)
		{
			if (types == null)
			{
				types = EmptyTypes;
			}
			return declaringType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, types, null);
		}

		internal static bool IsSubclassOf(Type type, Type baseClass)
		{
			return type.IsSubclassOf(baseClass);
		}

		public static bool IsInfinity(double value)
		{
			return double.IsInfinity(value);
		}

		public static ProtoTypeCode GetTypeCode(Type type)
		{
			TypeCode code = Type.GetTypeCode(type);
			switch (code)
			{
			case TypeCode.Empty:
			case TypeCode.Boolean:
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
			case TypeCode.DateTime:
			case TypeCode.String:
				return (ProtoTypeCode)code;
			default:
				if (type == typeof(TimeSpan))
				{
					return ProtoTypeCode.TimeSpan;
				}
				if (type == typeof(Guid))
				{
					return ProtoTypeCode.Guid;
				}
				if (type == typeof(Uri))
				{
					return ProtoTypeCode.Uri;
				}
				if (type == typeof(byte[]))
				{
					return ProtoTypeCode.ByteArray;
				}
				if (type == typeof(Type))
				{
					return ProtoTypeCode.Type;
				}
				return ProtoTypeCode.Unknown;
			}
		}

		internal static Type GetUnderlyingType(Type type)
		{
			return Nullable.GetUnderlyingType(type);
		}

		internal static bool IsValueType(Type type)
		{
			return type.IsValueType;
		}

		internal static bool IsSealed(Type type)
		{
			return type.IsSealed;
		}

		internal static bool IsClass(Type type)
		{
			return type.IsClass;
		}

		internal static bool IsEnum(Type type)
		{
			return type.IsEnum;
		}

		internal static MethodInfo GetGetMethod(PropertyInfo property, bool nonPublic, bool allowInternal)
		{
			if (property == null)
			{
				return null;
			}
			MethodInfo method = property.GetGetMethod(nonPublic);
			if (method == null && !nonPublic && allowInternal)
			{
				method = property.GetGetMethod(nonPublic: true);
				if (method == null && !method.IsAssembly && !method.IsFamilyOrAssembly)
				{
					method = null;
				}
			}
			return method;
		}

		internal static MethodInfo GetSetMethod(PropertyInfo property, bool nonPublic, bool allowInternal)
		{
			if (property == null)
			{
				return null;
			}
			MethodInfo method = property.GetSetMethod(nonPublic);
			if (method == null && !nonPublic && allowInternal)
			{
				method = property.GetGetMethod(nonPublic: true);
				if (method == null && !method.IsAssembly && !method.IsFamilyOrAssembly)
				{
					method = null;
				}
			}
			return method;
		}

		internal static ConstructorInfo GetConstructor(Type type, Type[] parameterTypes, bool nonPublic)
		{
			return type.GetConstructor(nonPublic ? (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : (BindingFlags.Instance | BindingFlags.Public), null, parameterTypes, null);
		}

		internal static ConstructorInfo[] GetConstructors(Type type, bool nonPublic)
		{
			return type.GetConstructors(nonPublic ? (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : (BindingFlags.Instance | BindingFlags.Public));
		}

		internal static PropertyInfo GetProperty(Type type, string name, bool nonPublic)
		{
			return type.GetProperty(name, nonPublic ? (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : (BindingFlags.Instance | BindingFlags.Public));
		}

		internal static object ParseEnum(Type type, string value)
		{
			return Enum.Parse(type, value, ignoreCase: true);
		}

		internal static MemberInfo[] GetInstanceFieldsAndProperties(Type type, bool publicOnly)
		{
			BindingFlags flags = publicOnly ? (BindingFlags.Instance | BindingFlags.Public) : (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			PropertyInfo[] props = type.GetProperties(flags);
			FieldInfo[] fields = type.GetFields(flags);
			MemberInfo[] members = new MemberInfo[fields.Length + props.Length];
			props.CopyTo(members, 0);
			fields.CopyTo(members, props.Length);
			return members;
		}

		internal static Type GetMemberType(MemberInfo member)
		{
			switch (member.MemberType)
			{
			case MemberTypes.Field:
				return ((FieldInfo)member).FieldType;
			case MemberTypes.Property:
				return ((PropertyInfo)member).PropertyType;
			default:
				return null;
			}
		}

		internal static bool IsAssignableFrom(Type target, Type type)
		{
			return target.IsAssignableFrom(type);
		}

		internal static Assembly GetAssembly(Type type)
		{
			return type.Assembly;
		}

		internal static byte[] GetBuffer(MemoryStream ms)
		{
			return ms.GetBuffer();
		}
	}
}
