using System;
using System.Runtime.InteropServices;

namespace ProtoBuf
{
	[StructLayout(LayoutKind.Explicit)]
	public struct DiscriminatedUnion64Object
	{
		[FieldOffset(0)]
		private readonly int _discriminator;

		[FieldOffset(8)]
		public readonly long Int64;

		[FieldOffset(8)]
		public readonly ulong UInt64;

		[FieldOffset(8)]
		public readonly int Int32;

		[FieldOffset(8)]
		public readonly uint UInt32;

		[FieldOffset(8)]
		public readonly bool Boolean;

		[FieldOffset(8)]
		public readonly float Single;

		[FieldOffset(8)]
		public readonly double Double;

		[FieldOffset(8)]
		public readonly DateTime DateTime;

		[FieldOffset(8)]
		public readonly TimeSpan TimeSpan;

		[FieldOffset(16)]
		public readonly object Object;

		unsafe static DiscriminatedUnion64Object()
		{
			if (sizeof(DateTime) > 8)
			{
				throw new InvalidOperationException("DateTime was unexpectedly too big for DiscriminatedUnion64Object");
			}
			if (sizeof(TimeSpan) > 8)
			{
				throw new InvalidOperationException("TimeSpan was unexpectedly too big for DiscriminatedUnion64Object");
			}
		}

		private DiscriminatedUnion64Object(int discriminator)
		{
			this = default(DiscriminatedUnion64Object);
			_discriminator = ~discriminator;
		}

		public bool Is(int discriminator)
		{
			return _discriminator == ~discriminator;
		}

		public DiscriminatedUnion64Object(int discriminator, long value)
			: this(discriminator)
		{
			Int64 = value;
		}

		public DiscriminatedUnion64Object(int discriminator, int value)
			: this(discriminator)
		{
			Int32 = value;
		}

		public DiscriminatedUnion64Object(int discriminator, ulong value)
			: this(discriminator)
		{
			UInt64 = value;
		}

		public DiscriminatedUnion64Object(int discriminator, uint value)
			: this(discriminator)
		{
			UInt32 = value;
		}

		public DiscriminatedUnion64Object(int discriminator, float value)
			: this(discriminator)
		{
			Single = value;
		}

		public DiscriminatedUnion64Object(int discriminator, double value)
			: this(discriminator)
		{
			Double = value;
		}

		public DiscriminatedUnion64Object(int discriminator, bool value)
			: this(discriminator)
		{
			Boolean = value;
		}

		public DiscriminatedUnion64Object(int discriminator, object value)
			: this((value != null) ? discriminator : (-1))
		{
			Object = value;
		}

		public DiscriminatedUnion64Object(int discriminator, DateTime? value)
			: this(value.HasValue ? discriminator : (-1))
		{
			DateTime = value.GetValueOrDefault();
		}

		public DiscriminatedUnion64Object(int discriminator, TimeSpan? value)
			: this(value.HasValue ? discriminator : (-1))
		{
			TimeSpan = value.GetValueOrDefault();
		}

		public static void Reset(ref DiscriminatedUnion64Object value, int discriminator)
		{
			if (value.Is(discriminator))
			{
				value = default(DiscriminatedUnion64Object);
			}
		}
	}
}
