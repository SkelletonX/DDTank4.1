namespace ProtoBuf
{
	public enum WireType
	{
		None = -1,
		Variant = 0,
		Fixed64 = 1,
		String = 2,
		StartGroup = 3,
		EndGroup = 4,
		Fixed32 = 5,
		SignedVariant = 8
	}
}
