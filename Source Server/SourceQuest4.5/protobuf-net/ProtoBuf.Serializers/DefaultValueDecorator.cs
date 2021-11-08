using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Reflection;

namespace ProtoBuf.Serializers
{
	internal sealed class DefaultValueDecorator : ProtoDecoratorBase
	{
		private readonly object defaultValue;

		public override Type ExpectedType => Tail.ExpectedType;

		public override bool RequiresOldValue => Tail.RequiresOldValue;

		public override bool ReturnsValue => Tail.ReturnsValue;

		public DefaultValueDecorator(TypeModel model, object defaultValue, IProtoSerializer tail)
			: base(tail)
		{
			if (defaultValue == null)
			{
				throw new ArgumentNullException("defaultValue");
			}
			Type type = model.MapType(defaultValue.GetType());
			if (type != tail.ExpectedType)
			{
				throw new ArgumentException("Default value is of incorrect type", "defaultValue");
			}
			this.defaultValue = defaultValue;
		}

		public override void Write(object value, ProtoWriter dest)
		{
			if (!object.Equals(value, defaultValue))
			{
				Tail.Write(value, dest);
			}
		}

		public override object Read(object value, ProtoReader source)
		{
			return Tail.Read(value, source);
		}

		protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			CodeLabel done = ctx.DefineLabel();
			if (valueFrom == null)
			{
				ctx.CopyValue();
				CodeLabel needToPop = ctx.DefineLabel();
				EmitBranchIfDefaultValue(ctx, needToPop);
				Tail.EmitWrite(ctx, null);
				ctx.Branch(done, @short: true);
				ctx.MarkLabel(needToPop);
				ctx.DiscardValue();
			}
			else
			{
				ctx.LoadValue(valueFrom);
				EmitBranchIfDefaultValue(ctx, done);
				Tail.EmitWrite(ctx, valueFrom);
			}
			ctx.MarkLabel(done);
		}

		private void EmitBeq(CompilerContext ctx, CodeLabel label, Type type)
		{
			ProtoTypeCode typeCode = Helpers.GetTypeCode(type);
			if ((uint)(typeCode - 3) <= 11u)
			{
				ctx.BranchIfEqual(label, @short: false);
				return;
			}
			MethodInfo method = type.GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public, null, new Type[2]
			{
				type,
				type
			}, null);
			if (method == null || method.ReturnType != ctx.MapType(typeof(bool)))
			{
				throw new InvalidOperationException("No suitable equality operator found for default-values of type: " + type.FullName);
			}
			ctx.EmitCall(method);
			ctx.BranchIfTrue(label, @short: false);
		}

		private void EmitBranchIfDefaultValue(CompilerContext ctx, CodeLabel label)
		{
			Type expected = ExpectedType;
			switch (Helpers.GetTypeCode(expected))
			{
			case ProtoTypeCode.Boolean:
				if ((bool)defaultValue)
				{
					ctx.BranchIfTrue(label, @short: false);
				}
				else
				{
					ctx.BranchIfFalse(label, @short: false);
				}
				break;
			case ProtoTypeCode.Byte:
				if ((byte)defaultValue == 0)
				{
					ctx.BranchIfFalse(label, @short: false);
					break;
				}
				ctx.LoadValue((byte)defaultValue);
				EmitBeq(ctx, label, expected);
				break;
			case ProtoTypeCode.SByte:
				if ((sbyte)defaultValue == 0)
				{
					ctx.BranchIfFalse(label, @short: false);
					break;
				}
				ctx.LoadValue((sbyte)defaultValue);
				EmitBeq(ctx, label, expected);
				break;
			case ProtoTypeCode.Int16:
				if ((short)defaultValue == 0)
				{
					ctx.BranchIfFalse(label, @short: false);
					break;
				}
				ctx.LoadValue((short)defaultValue);
				EmitBeq(ctx, label, expected);
				break;
			case ProtoTypeCode.UInt16:
				if ((ushort)defaultValue == 0)
				{
					ctx.BranchIfFalse(label, @short: false);
					break;
				}
				ctx.LoadValue((ushort)defaultValue);
				EmitBeq(ctx, label, expected);
				break;
			case ProtoTypeCode.Int32:
				if ((int)defaultValue == 0)
				{
					ctx.BranchIfFalse(label, @short: false);
					break;
				}
				ctx.LoadValue((int)defaultValue);
				EmitBeq(ctx, label, expected);
				break;
			case ProtoTypeCode.UInt32:
				if ((uint)defaultValue == 0)
				{
					ctx.BranchIfFalse(label, @short: false);
					break;
				}
				ctx.LoadValue((int)(uint)defaultValue);
				EmitBeq(ctx, label, expected);
				break;
			case ProtoTypeCode.Char:
				if ((char)defaultValue == '\0')
				{
					ctx.BranchIfFalse(label, @short: false);
					break;
				}
				ctx.LoadValue((char)defaultValue);
				EmitBeq(ctx, label, expected);
				break;
			case ProtoTypeCode.Int64:
				ctx.LoadValue((long)defaultValue);
				EmitBeq(ctx, label, expected);
				break;
			case ProtoTypeCode.UInt64:
				ctx.LoadValue((long)(ulong)defaultValue);
				EmitBeq(ctx, label, expected);
				break;
			case ProtoTypeCode.Double:
				ctx.LoadValue((double)defaultValue);
				EmitBeq(ctx, label, expected);
				break;
			case ProtoTypeCode.Single:
				ctx.LoadValue((float)defaultValue);
				EmitBeq(ctx, label, expected);
				break;
			case ProtoTypeCode.String:
				ctx.LoadValue((string)defaultValue);
				EmitBeq(ctx, label, expected);
				break;
			case ProtoTypeCode.Decimal:
			{
				decimal d = (decimal)defaultValue;
				ctx.LoadValue(d);
				EmitBeq(ctx, label, expected);
				break;
			}
			case ProtoTypeCode.TimeSpan:
			{
				TimeSpan ts = (TimeSpan)defaultValue;
				if (ts == TimeSpan.Zero)
				{
					ctx.LoadValue(typeof(TimeSpan).GetField("Zero"));
				}
				else
				{
					ctx.LoadValue(ts.Ticks);
					ctx.EmitCall(ctx.MapType(typeof(TimeSpan)).GetMethod("FromTicks"));
				}
				EmitBeq(ctx, label, expected);
				break;
			}
			case ProtoTypeCode.Guid:
				ctx.LoadValue((Guid)defaultValue);
				EmitBeq(ctx, label, expected);
				break;
			case ProtoTypeCode.DateTime:
				ctx.LoadValue(((DateTime)defaultValue).ToBinary());
				ctx.EmitCall(ctx.MapType(typeof(DateTime)).GetMethod("FromBinary"));
				EmitBeq(ctx, label, expected);
				break;
			default:
				throw new NotSupportedException("Type cannot be represented as a default value: " + expected.FullName);
			}
		}

		protected override void EmitRead(CompilerContext ctx, Local valueFrom)
		{
			Tail.EmitRead(ctx, valueFrom);
		}
	}
}
