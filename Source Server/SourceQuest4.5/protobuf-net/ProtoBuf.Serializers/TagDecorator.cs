using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

namespace ProtoBuf.Serializers
{
	internal sealed class TagDecorator : ProtoDecoratorBase, IProtoTypeSerializer, IProtoSerializer
	{
		private readonly bool strict;

		private readonly int fieldNumber;

		private readonly WireType wireType;

		public override Type ExpectedType => Tail.ExpectedType;

		public override bool RequiresOldValue => Tail.RequiresOldValue;

		public override bool ReturnsValue => Tail.ReturnsValue;

		private bool NeedsHint => (wireType & (WireType)(-8)) != 0;

		public bool HasCallbacks(TypeModel.CallbackType callbackType)
		{
			return (Tail as IProtoTypeSerializer)?.HasCallbacks(callbackType) ?? false;
		}

		public bool CanCreateInstance()
		{
			return (Tail as IProtoTypeSerializer)?.CanCreateInstance() ?? false;
		}

		public object CreateInstance(ProtoReader source)
		{
			return ((IProtoTypeSerializer)Tail).CreateInstance(source);
		}

		public void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
		{
			(Tail as IProtoTypeSerializer)?.Callback(value, callbackType, context);
		}

		public void EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType)
		{
			((IProtoTypeSerializer)Tail).EmitCallback(ctx, valueFrom, callbackType);
		}

		public void EmitCreateInstance(CompilerContext ctx)
		{
			((IProtoTypeSerializer)Tail).EmitCreateInstance(ctx);
		}

		public TagDecorator(int fieldNumber, WireType wireType, bool strict, IProtoSerializer tail)
			: base(tail)
		{
			this.fieldNumber = fieldNumber;
			this.wireType = wireType;
			this.strict = strict;
		}

		public override object Read(object value, ProtoReader source)
		{
			if (strict)
			{
				source.Assert(wireType);
			}
			else if (NeedsHint)
			{
				source.Hint(wireType);
			}
			return Tail.Read(value, source);
		}

		public override void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteFieldHeader(fieldNumber, wireType, dest);
			Tail.Write(value, dest);
		}

		protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			ctx.LoadValue(fieldNumber);
			ctx.LoadValue((int)wireType);
			ctx.LoadReaderWriter();
			ctx.EmitCall(ctx.MapType(typeof(ProtoWriter)).GetMethod("WriteFieldHeader"));
			Tail.EmitWrite(ctx, valueFrom);
		}

		protected override void EmitRead(CompilerContext ctx, Local valueFrom)
		{
			if (strict || NeedsHint)
			{
				ctx.LoadReaderWriter();
				ctx.LoadValue((int)wireType);
				ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod(strict ? "Assert" : "Hint"));
			}
			Tail.EmitRead(ctx, valueFrom);
		}
	}
}
