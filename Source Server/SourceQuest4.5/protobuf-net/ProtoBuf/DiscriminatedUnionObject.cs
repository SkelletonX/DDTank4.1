namespace ProtoBuf
{
	public struct DiscriminatedUnionObject
	{
		private readonly int _discriminator;

		public readonly object Object;

		public bool Is(int discriminator)
		{
			return _discriminator == ~discriminator;
		}

		public DiscriminatedUnionObject(int discriminator, object value)
		{
			_discriminator = ~discriminator;
			Object = value;
		}

		public static void Reset(ref DiscriminatedUnionObject value, int discriminator)
		{
			if (value.Is(discriminator))
			{
				value = default(DiscriminatedUnionObject);
			}
		}
	}
}
