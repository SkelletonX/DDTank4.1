using ProtoBuf.Compiler;
using System;

namespace ProtoBuf.Serializers
{
	internal abstract class ProtoDecoratorBase : IProtoSerializer
	{
		protected readonly IProtoSerializer Tail;

		public abstract Type ExpectedType
		{
			get;
		}

		public abstract bool ReturnsValue
		{
			get;
		}

		public abstract bool RequiresOldValue
		{
			get;
		}

		protected ProtoDecoratorBase(IProtoSerializer tail)
		{
			Tail = tail;
		}

		public abstract void Write(object value, ProtoWriter dest);

		public abstract object Read(object value, ProtoReader source);

		void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			EmitWrite(ctx, valueFrom);
		}

		protected abstract void EmitWrite(CompilerContext ctx, Local valueFrom);

		void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
		{
			EmitRead(ctx, valueFrom);
		}

		protected abstract void EmitRead(CompilerContext ctx, Local valueFrom);
	}
}
