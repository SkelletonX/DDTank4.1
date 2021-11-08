using System;

namespace ProtoBuf
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
	public sealed class ProtoContractAttribute : Attribute
	{
		private string name;

		private int implicitFirstTag;

		private ImplicitFields implicitFields;

		private int dataMemberOffset;

		private ushort flags;

		private const ushort OPTIONS_InferTagFromName = 1;

		private const ushort OPTIONS_InferTagFromNameHasValue = 2;

		private const ushort OPTIONS_UseProtoMembersOnly = 4;

		private const ushort OPTIONS_SkipConstructor = 8;

		private const ushort OPTIONS_IgnoreListHandling = 16;

		private const ushort OPTIONS_AsReferenceDefault = 32;

		private const ushort OPTIONS_EnumPassthru = 64;

		private const ushort OPTIONS_EnumPassthruHasValue = 128;

		private const ushort OPTIONS_IsGroup = 256;

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public int ImplicitFirstTag
		{
			get
			{
				return implicitFirstTag;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("ImplicitFirstTag");
				}
				implicitFirstTag = value;
			}
		}

		public bool UseProtoMembersOnly
		{
			get
			{
				return HasFlag(4);
			}
			set
			{
				SetFlag(4, value);
			}
		}

		public bool IgnoreListHandling
		{
			get
			{
				return HasFlag(16);
			}
			set
			{
				SetFlag(16, value);
			}
		}

		public ImplicitFields ImplicitFields
		{
			get
			{
				return implicitFields;
			}
			set
			{
				implicitFields = value;
			}
		}

		public bool InferTagFromName
		{
			get
			{
				return HasFlag(1);
			}
			set
			{
				SetFlag(1, value);
				SetFlag(2, value: true);
			}
		}

		internal bool InferTagFromNameHasValue => HasFlag(2);

		public int DataMemberOffset
		{
			get
			{
				return dataMemberOffset;
			}
			set
			{
				dataMemberOffset = value;
			}
		}

		public bool SkipConstructor
		{
			get
			{
				return HasFlag(8);
			}
			set
			{
				SetFlag(8, value);
			}
		}

		public bool AsReferenceDefault
		{
			get
			{
				return HasFlag(32);
			}
			set
			{
				SetFlag(32, value);
			}
		}

		public bool IsGroup
		{
			get
			{
				return HasFlag(256);
			}
			set
			{
				SetFlag(256, value);
			}
		}

		public bool EnumPassthru
		{
			get
			{
				return HasFlag(64);
			}
			set
			{
				SetFlag(64, value);
				SetFlag(128, value: true);
			}
		}

		internal bool EnumPassthruHasValue => HasFlag(128);

		private bool HasFlag(ushort flag)
		{
			return (flags & flag) == flag;
		}

		private void SetFlag(ushort flag, bool value)
		{
			if (value)
			{
				flags |= flag;
			}
			else
			{
				flags = (ushort)(flags & ~flag);
			}
		}
	}
}
