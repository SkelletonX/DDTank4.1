using ProtoBuf.Compiler;
using System;

namespace ProtoBuf.Serializers
{
	internal interface IProtoSerializer
	{
		Type ExpectedType
		{
			get;
		}

		bool RequiresOldValue
		{
			get;
		}

		bool ReturnsValue
		{
			get;
		}

		void Write(object value, ProtoWriter dest);

		object Read(object value, ProtoReader source);

		void EmitWrite(CompilerContext ctx, Local valueFrom);

		void EmitRead(CompilerContext ctx, Local entity);
	}
}
