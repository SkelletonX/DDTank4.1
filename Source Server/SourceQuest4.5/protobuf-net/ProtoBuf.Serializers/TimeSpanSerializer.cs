using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

namespace ProtoBuf.Serializers
{
	internal sealed class TimeSpanSerializer : IProtoSerializer
	{
		private static readonly Type expectedType = typeof(TimeSpan);

		private readonly bool wellKnown;

		public Type ExpectedType => expectedType;

		bool IProtoSerializer.RequiresOldValue => false;

		bool IProtoSerializer.ReturnsValue => true;

		public TimeSpanSerializer(DataFormat dataFormat, TypeModel model)
		{
			wellKnown = (dataFormat == DataFormat.WellKnown);
		}

		public object Read(object value, ProtoReader source)
		{
			if (wellKnown)
			{
				return BclHelpers.ReadDuration(source);
			}
			return BclHelpers.ReadTimeSpan(source);
		}

		public void Write(object value, ProtoWriter dest)
		{
			if (wellKnown)
			{
				BclHelpers.WriteDuration((TimeSpan)value, dest);
			}
			else
			{
				BclHelpers.WriteTimeSpan((TimeSpan)value, dest);
			}
		}

		void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			ctx.EmitWrite(ctx.MapType(typeof(BclHelpers)), wellKnown ? "WriteDuration" : "WriteTimeSpan", valueFrom);
		}

		void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
		{
			if (wellKnown)
			{
				ctx.LoadValue(valueFrom);
			}
			ctx.EmitBasicRead(ctx.MapType(typeof(BclHelpers)), wellKnown ? "ReadDuration" : "ReadTimeSpan", ExpectedType);
		}
	}
}
