using System;

namespace ProtoBuf
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class ProtoMapAttribute : Attribute
	{
		public DataFormat KeyFormat
		{
			get;
			set;
		}

		public DataFormat ValueFormat
		{
			get;
			set;
		}

		public bool DisableMap
		{
			get;
			set;
		}
	}
}
