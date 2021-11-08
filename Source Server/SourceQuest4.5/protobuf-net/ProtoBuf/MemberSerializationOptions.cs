using System;

namespace ProtoBuf
{
	[Flags]
	public enum MemberSerializationOptions
	{
		None = 0x0,
		Packed = 0x1,
		Required = 0x2,
		AsReference = 0x4,
		DynamicType = 0x8,
		OverwriteList = 0x10,
		AsReferenceHasValue = 0x20
	}
}
