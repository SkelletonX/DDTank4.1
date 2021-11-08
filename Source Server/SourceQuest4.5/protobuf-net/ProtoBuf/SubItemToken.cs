namespace ProtoBuf
{
	public struct SubItemToken
	{
		internal readonly long value64;

		internal SubItemToken(int value)
		{
			value64 = value;
		}

		internal SubItemToken(long value)
		{
			value64 = value;
		}
	}
}
