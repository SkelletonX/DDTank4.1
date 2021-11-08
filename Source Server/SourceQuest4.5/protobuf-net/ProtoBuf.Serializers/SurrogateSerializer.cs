using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Reflection;

namespace ProtoBuf.Serializers
{
	internal sealed class SurrogateSerializer : IProtoTypeSerializer, IProtoSerializer
	{
		private readonly Type forType;

		private readonly Type declaredType;

		private readonly MethodInfo toTail;

		private readonly MethodInfo fromTail;

		private IProtoTypeSerializer rootTail;

		public bool ReturnsValue => false;

		public bool RequiresOldValue => true;

		public Type ExpectedType => forType;

		bool IProtoTypeSerializer.HasCallbacks(TypeModel.CallbackType callbackType)
		{
			return false;
		}

		void IProtoTypeSerializer.EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType)
		{
		}

		void IProtoTypeSerializer.EmitCreateInstance(CompilerContext ctx)
		{
			throw new NotSupportedException();
		}

		bool IProtoTypeSerializer.CanCreateInstance()
		{
			return false;
		}

		object IProtoTypeSerializer.CreateInstance(ProtoReader source)
		{
			throw new NotSupportedException();
		}

		void IProtoTypeSerializer.Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
		{
		}

		public SurrogateSerializer(TypeModel model, Type forType, Type declaredType, IProtoTypeSerializer rootTail)
		{
			this.forType = forType;
			this.declaredType = declaredType;
			this.rootTail = rootTail;
			toTail = GetConversion(model, toTail: true);
			fromTail = GetConversion(model, toTail: false);
		}

		private static bool HasCast(TypeModel model, Type type, Type from, Type to, out MethodInfo op)
		{
			MethodInfo[] found = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			Type convertAttributeType = null;
			foreach (MethodInfo l in found)
			{
				if (l.ReturnType != to)
				{
					continue;
				}
				ParameterInfo[] paramTypes2 = l.GetParameters();
				if (paramTypes2.Length != 1 || !(paramTypes2[0].ParameterType == from))
				{
					continue;
				}
				if (convertAttributeType == null)
				{
					convertAttributeType = model.MapType(typeof(ProtoConverterAttribute), demand: false);
					if (convertAttributeType == null)
					{
						break;
					}
				}
				if (l.IsDefined(convertAttributeType, inherit: true))
				{
					op = l;
					return true;
				}
			}
			foreach (MethodInfo k in found)
			{
				if ((!(k.Name != "op_Implicit") || !(k.Name != "op_Explicit")) && !(k.ReturnType != to))
				{
					ParameterInfo[] paramTypes2 = k.GetParameters();
					if (paramTypes2.Length == 1 && paramTypes2[0].ParameterType == from)
					{
						op = k;
						return true;
					}
				}
			}
			op = null;
			return false;
		}

		public MethodInfo GetConversion(TypeModel model, bool toTail)
		{
			Type to = toTail ? declaredType : forType;
			Type from = toTail ? forType : declaredType;
			if (HasCast(model, declaredType, from, to, out MethodInfo op) || HasCast(model, forType, from, to, out op))
			{
				return op;
			}
			throw new InvalidOperationException("No suitable conversion operator found for surrogate: " + forType.FullName + " / " + declaredType.FullName);
		}

		public void Write(object value, ProtoWriter writer)
		{
			rootTail.Write(toTail.Invoke(null, new object[1]
			{
				value
			}), writer);
		}

		public object Read(object value, ProtoReader source)
		{
			object[] args = new object[1]
			{
				value
			};
			value = toTail.Invoke(null, args);
			args[0] = rootTail.Read(value, source);
			return fromTail.Invoke(null, args);
		}

		void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
		{
			using (Local converted = new Local(ctx, declaredType))
			{
				ctx.LoadValue(valueFrom);
				ctx.EmitCall(toTail);
				ctx.StoreValue(converted);
				rootTail.EmitRead(ctx, converted);
				ctx.LoadValue(converted);
				ctx.EmitCall(fromTail);
				ctx.StoreValue(valueFrom);
			}
		}

		void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			ctx.LoadValue(valueFrom);
			ctx.EmitCall(toTail);
			rootTail.EmitWrite(ctx, null);
		}
	}
}
