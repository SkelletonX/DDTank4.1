using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

namespace ProtoBuf.Serializers
{
	internal class UInt16Serializer : IProtoSerializer
	{
		private static readonly Type expectedType = typeof(ushort);

		public virtual Type ExpectedType => expectedType;

		bool IProtoSerializer.RequiresOldValue => false;

		bool IProtoSerializer.ReturnsValue => true;

		public UInt16Serializer(TypeModel model)
		{
		}

		public virtual object Read(object value, ProtoReader source)
		{
			return source.ReadUInt16();
		}

		public virtual void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteUInt16((ushort)value, dest);
		}

		void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			ctx.EmitBasicWrite("WriteUInt16", valueFrom);
		}

		void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
		{
			ctx.EmitBasicRead("ReadUInt16", ctx.MapType(typeof(ushort)));
		}
	}
}
