using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

namespace ProtoBuf.Serializers
{
	internal sealed class NullDecorator : ProtoDecoratorBase
	{
		private readonly Type expectedType;

		public const int Tag = 1;

		public override Type ExpectedType => expectedType;

		public override bool ReturnsValue => true;

		public override bool RequiresOldValue => true;

		public NullDecorator(TypeModel model, IProtoSerializer tail)
			: base(tail)
		{
			if (!tail.ReturnsValue)
			{
				throw new NotSupportedException("NullDecorator only supports implementations that return values");
			}
			Type tailType = tail.ExpectedType;
			if (Helpers.IsValueType(tailType))
			{
				expectedType = model.MapType(typeof(Nullable<>)).MakeGenericType(tailType);
			}
			else
			{
				expectedType = tailType;
			}
		}

		protected override void EmitRead(CompilerContext ctx, Local valueFrom)
		{
			using (Local oldValue = ctx.GetLocalWithValue(expectedType, valueFrom))
			{
				using (Local token = new Local(ctx, ctx.MapType(typeof(SubItemToken))))
				{
					using (Local field = new Local(ctx, ctx.MapType(typeof(int))))
					{
						ctx.LoadReaderWriter();
						ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("StartSubItem"));
						ctx.StoreValue(token);
						CodeLabel next = ctx.DefineLabel();
						CodeLabel processField = ctx.DefineLabel();
						CodeLabel end = ctx.DefineLabel();
						ctx.MarkLabel(next);
						ctx.EmitBasicRead("ReadFieldHeader", ctx.MapType(typeof(int)));
						ctx.CopyValue();
						ctx.StoreValue(field);
						ctx.LoadValue(1);
						ctx.BranchIfEqual(processField, @short: true);
						ctx.LoadValue(field);
						ctx.LoadValue(1);
						ctx.BranchIfLess(end, @short: false);
						ctx.LoadReaderWriter();
						ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("SkipField"));
						ctx.Branch(next, @short: true);
						ctx.MarkLabel(processField);
						if (Tail.RequiresOldValue)
						{
							if (Helpers.IsValueType(expectedType))
							{
								ctx.LoadAddress(oldValue, expectedType);
								ctx.EmitCall(expectedType.GetMethod("GetValueOrDefault", Helpers.EmptyTypes));
							}
							else
							{
								ctx.LoadValue(oldValue);
							}
						}
						Tail.EmitRead(ctx, null);
						if (Helpers.IsValueType(expectedType))
						{
							ctx.EmitCtor(expectedType, Tail.ExpectedType);
						}
						ctx.StoreValue(oldValue);
						ctx.Branch(next, @short: false);
						ctx.MarkLabel(end);
						ctx.LoadValue(token);
						ctx.LoadReaderWriter();
						ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("EndSubItem"));
						ctx.LoadValue(oldValue);
					}
				}
			}
		}

		protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			using (Local valOrNull = ctx.GetLocalWithValue(expectedType, valueFrom))
			{
				using (Local token = new Local(ctx, ctx.MapType(typeof(SubItemToken))))
				{
					ctx.LoadNullRef();
					ctx.LoadReaderWriter();
					ctx.EmitCall(ctx.MapType(typeof(ProtoWriter)).GetMethod("StartSubItem"));
					ctx.StoreValue(token);
					if (Helpers.IsValueType(expectedType))
					{
						ctx.LoadAddress(valOrNull, expectedType);
						ctx.LoadValue(expectedType.GetProperty("HasValue"));
					}
					else
					{
						ctx.LoadValue(valOrNull);
					}
					CodeLabel end = ctx.DefineLabel();
					ctx.BranchIfFalse(end, @short: false);
					if (Helpers.IsValueType(expectedType))
					{
						ctx.LoadAddress(valOrNull, expectedType);
						ctx.EmitCall(expectedType.GetMethod("GetValueOrDefault", Helpers.EmptyTypes));
					}
					else
					{
						ctx.LoadValue(valOrNull);
					}
					Tail.EmitWrite(ctx, null);
					ctx.MarkLabel(end);
					ctx.LoadValue(token);
					ctx.LoadReaderWriter();
					ctx.EmitCall(ctx.MapType(typeof(ProtoWriter)).GetMethod("EndSubItem"));
				}
			}
		}

		public override object Read(object value, ProtoReader source)
		{
			SubItemToken tok = ProtoReader.StartSubItem(source);
			int field;
			while ((field = source.ReadFieldHeader()) > 0)
			{
				if (field == 1)
				{
					value = Tail.Read(value, source);
				}
				else
				{
					source.SkipField();
				}
			}
			ProtoReader.EndSubItem(tok, source);
			return value;
		}

		public override void Write(object value, ProtoWriter dest)
		{
			SubItemToken token = ProtoWriter.StartSubItem(null, dest);
			if (value != null)
			{
				Tail.Write(value, dest);
			}
			ProtoWriter.EndSubItem(token, dest);
		}
	}
}
