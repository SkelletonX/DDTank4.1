using ProtoBuf.Compiler;
using System;
using System.Reflection;

namespace ProtoBuf.Serializers
{
	internal sealed class FieldDecorator : ProtoDecoratorBase
	{
		private readonly FieldInfo field;

		private readonly Type forType;

		public override Type ExpectedType => forType;

		public override bool RequiresOldValue => true;

		public override bool ReturnsValue => false;

		public FieldDecorator(Type forType, FieldInfo field, IProtoSerializer tail)
			: base(tail)
		{
			this.forType = forType;
			this.field = field;
		}

		public override void Write(object value, ProtoWriter dest)
		{
			value = field.GetValue(value);
			if (value != null)
			{
				Tail.Write(value, dest);
			}
		}

		public override object Read(object value, ProtoReader source)
		{
			object newValue = Tail.Read(Tail.RequiresOldValue ? field.GetValue(value) : null, source);
			if (newValue != null)
			{
				field.SetValue(value, newValue);
			}
			return null;
		}

		protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			ctx.LoadAddress(valueFrom, ExpectedType);
			ctx.LoadValue(field);
			ctx.WriteNullCheckedTail(field.FieldType, Tail, null);
		}

		protected override void EmitRead(CompilerContext ctx, Local valueFrom)
		{
			using (Local loc = ctx.GetLocalWithValue(ExpectedType, valueFrom))
			{
				if (Tail.RequiresOldValue)
				{
					ctx.LoadAddress(loc, ExpectedType);
					ctx.LoadValue(field);
				}
				ctx.ReadNullCheckedTail(field.FieldType, Tail, null);
				MemberInfo member = field;
				ctx.CheckAccessibility(ref member);
				if (member is FieldInfo)
				{
					if (Tail.ReturnsValue)
					{
						using (Local newVal = new Local(ctx, field.FieldType))
						{
							ctx.StoreValue(newVal);
							if (Helpers.IsValueType(field.FieldType))
							{
								ctx.LoadAddress(loc, ExpectedType);
								ctx.LoadValue(newVal);
								ctx.StoreValue(field);
							}
							else
							{
								CodeLabel allDone = ctx.DefineLabel();
								ctx.LoadValue(newVal);
								ctx.BranchIfFalse(allDone, @short: true);
								ctx.LoadAddress(loc, ExpectedType);
								ctx.LoadValue(newVal);
								ctx.StoreValue(field);
								ctx.MarkLabel(allDone);
							}
						}
					}
				}
				else if (Tail.ReturnsValue)
				{
					ctx.DiscardValue();
				}
			}
		}
	}
}
