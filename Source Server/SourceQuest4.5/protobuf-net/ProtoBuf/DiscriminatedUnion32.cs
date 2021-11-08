using System.Runtime.InteropServices;

namespace ProtoBuf
{
	[StructLayout(LayoutKind.Explicit)]
	public struct DiscriminatedUnion32
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

		private DiscriminatedUnion32(int discriminator)
		{
			this = default(DiscriminatedUnion32);
			_discriminator = ~discriminator;
		}

		public bool Is(int discriminator)
		{
			return _discriminator == ~discriminator;
		}

		public DiscriminatedUnion32(int discriminator, int value)
			: this(discriminator)
		{
			Int32 = value;
		}

		public DiscriminatedUnion32(int discriminator, uint value)
			: this(discriminator)
		{
			UInt32 = value;
		}

		public DiscriminatedUnion32(int discriminator, float value)
			: this(discriminator)
		{
			Single = value;
		}

		public DiscriminatedUnion32(int discriminator, bool value)
			: this(discriminator)
		{
			Boolean = value;
		}

		public static void Reset(ref DiscriminatedUnion32 value, int discriminator)
		{
			if (value.Is(discriminator))
			{
				value = default(DiscriminatedUnion32);
			}
		}
	}
}
