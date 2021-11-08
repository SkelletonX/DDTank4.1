using ProtoBuf.Meta;
using System;

namespace ProtoBuf.Serializers
{
	internal sealed class CharSerializer : UInt16Serializer
	{
		private static readonly Type expectedType = typeof(char);

		public override Type ExpectedType => expectedType;

		public CharSerializer(TypeModel model)
			: base(model)
		{
		}

		public override void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteUInt16((char)value, dest);
		}

		public override object Read(object value, ProtoReader source)
		{
			return (char)source.ReadUInt16();
		}
	}
}
