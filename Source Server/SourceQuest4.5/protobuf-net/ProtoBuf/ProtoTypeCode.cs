namespace ProtoBuf
{
	internal enum ProtoTypeCode
	{
		Empty = 0,
		Unknown = 1,
		Boolean = 3,
		Char = 4,
		SByte = 5,
		Byte = 6,
		Int16 = 7,
		UInt16 = 8,
		Int32 = 9,
		UInt32 = 10,
		Int64 = 11,
		UInt64 = 12,
		Single = 13,
		Double = 14,
		Decimal = 0xF,
		DateTime = 0x10,
		String = 18,
		TimeSpan = 100,
		ByteArray = 101,
		Guid = 102,
		Uri = 103,
		Type = 104
	}
}
