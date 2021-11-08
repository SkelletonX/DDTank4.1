using System.Runtime.InteropServices;

namespace ProtoBuf
{
	[StructLayout(LayoutKind.Explicit)]
	public struct DiscriminatedUnion32Object
	{
		[FieldOffset(0)]
		private readonly int _discriminator;

		[FieldOffset(4)]
		public readonly int Int32;

		[FieldOffset(4)]
		public readonly uint UInt32;

		[FieldOffset(4)]
		public readonly bool Boolean;

		[FieldOffset(4)]
		public readonly float Single;

		[FieldOffset(8)]
		public readonly object Object;

		private DiscriminatedUnion32Object(int discriminator)
		{
			this = default(DiscriminatedUnion32Object);
			_discriminator = ~discriminator;
		}

		public bool Is(int discriminator)
		{
			return _discriminator == ~discriminator;
		}

		public DiscriminatedUnion32Object(int discriminator, int value)
			: this(discriminator)
		{
			Int32 = value;
		}

		public DiscriminatedUnion32Object(int discriminator, uint value)
			: this(discriminator)
		{
			UInt32 = value;
		}

		public DiscriminatedUnion32Object(int discriminator, float value)
			: this(discriminator)
		{
			Single = value;
		}

		public DiscriminatedUnion32Object(int discriminator, bool value)
			: this(discriminator)
		{
			Boolean = value;
		}

		public DiscriminatedUnion32Object(int discriminator, object value)
			: this((value != null) ? discriminator : (-1))
		{
			Object = value;
		}

		public static void Reset(ref DiscriminatedUnion32Object value, int discriminator)
		{
			if (value.Is(discriminator))
			{
				value = default(DiscriminatedUnion32Object);
			}
		}
	}
}
