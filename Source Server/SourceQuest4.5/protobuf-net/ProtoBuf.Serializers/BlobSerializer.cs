using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

namespace ProtoBuf.Serializers
{
	internal sealed class BlobSerializer : IProtoSerializer
	{
		private static readonly Type expectedType = typeof(byte[]);

		private readonly bool overwriteList;

		public Type ExpectedType => expectedType;

		bool IProtoSerializer.RequiresOldValue => !overwriteList;

		bool IProtoSerializer.ReturnsValue => true;

		public BlobSerializer(TypeModel model, bool overwriteList)
		{
			this.overwriteList = overwriteList;
		}

		public object Read(object value, ProtoReader source)
		{
			return ProtoReader.AppendBytes(overwriteList ? null : ((byte[])value), source);
		}

		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteBytes((byte[])value, dest);
		}

		void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			ctx.EmitBasicWrite("WriteBytes", valueFrom);
		}

		void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
		{
			if (overwriteList)
			{
				ctx.LoadNullRef();
			}
			else
			{
				ctx.LoadValue(valueFrom);
			}
			ctx.LoadReaderWriter();
			ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("AppendBytes"));
		}
	}
}
