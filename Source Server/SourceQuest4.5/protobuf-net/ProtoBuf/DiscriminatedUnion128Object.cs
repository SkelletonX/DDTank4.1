using System;
using System.Runtime.InteropServices;

namespace ProtoBuf
{
	[StructLayout(LayoutKind.Explicit)]
	public struct DiscriminatedUnion128Object
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

		[FieldOffset(8)]
		public readonly Guid Guid;

		[FieldOffset(24)]
		public readonly object Object;

		unsafe static DiscriminatedUnion128Object()
		{
			if (sizeof(DateTime) > 16)
			{
				throw new InvalidOperationException("DateTime was unexpectedly too big for DiscriminatedUnion128Object");
			}
			if (sizeof(TimeSpan) > 16)
			{
				throw new InvalidOperationException("TimeSpan was unexpectedly too big for DiscriminatedUnion128Object");
			}
			if (sizeof(Guid) > 16)
			{
				throw new InvalidOperationException("Guid was unexpectedly too big for DiscriminatedUnion128Object");
			}
		}

		private DiscriminatedUnion128Object(int discriminator)
		{
			this = default(DiscriminatedUnion128Object);
			_discriminator = ~discriminator;
		}

		public bool Is(int discriminator)
		{
			return _discriminator == ~discriminator;
		}

		public DiscriminatedUnion128Object(int discriminator, long value)
			: this(discriminator)
		{
			Int64 = value;
		}

		public DiscriminatedUnion128Object(int discriminator, int value)
			: this(discriminator)
		{
			Int32 = value;
		}

		public DiscriminatedUnion128Object(int discriminator, ulong value)
			: this(discriminator)
		{
			UInt64 = value;
		}

		public DiscriminatedUnion128Object(int discriminator, uint value)
			: this(discriminator)
		{
			UInt32 = value;
		}

		public DiscriminatedUnion128Object(int discriminator, float value)
			: this(discriminator)
		{
			Single = value;
		}

		public DiscriminatedUnion128Object(int discriminator, double value)
			: this(discriminator)
		{
			Double = value;
		}

		public DiscriminatedUnion128Object(int discriminator, bool value)
			: this(discriminator)
		{
			Boolean = value;
		}

		public DiscriminatedUnion128Object(int discriminator, object value)
			: this((value != null) ? discriminator : (-1))
		{
			Object = value;
		}

		public DiscriminatedUnion128Object(int discriminator, DateTime? value)
			: this(value.HasValue ? discriminator : (-1))
		{
			DateTime = value.GetValueOrDefault();
		}

		public DiscriminatedUnion128Object(int discriminator, TimeSpan? value)
			: this(value.HasValue ? discriminator : (-1))
		{
			TimeSpan = value.GetValueOrDefault();
		}

		public DiscriminatedUnion128Object(int discriminator, Guid? value)
			: this(value.HasValue ? discriminator : (-1))
		{
			Guid = value.GetValueOrDefault();
		}

		public static void Reset(ref DiscriminatedUnion128Object value, int discriminator)
		{
			if (value.Is(discriminator))
			{
				value = default(DiscriminatedUnion128Object);
			}
		}
	}
}
