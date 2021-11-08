using ProtoBuf.Meta;
using System;
using System.ComponentModel;

namespace ProtoBuf
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
	public sealed class ProtoIncludeAttribute : Attribute
	{
		private readonly int tag;

		private readonly string knownTypeName;

		private DataFormat dataFormat;

		public int Tag => tag;

		public string KnownTypeName => knownTypeName;

		public Type KnownType => TypeModel.ResolveKnownType(KnownTypeName, null, null);

		[DefaultValue(DataFormat.Default)]
		public DataFormat DataFormat
		{
			get
			{
				return dataFormat;
			}
			set
			{
				dataFormat = value;
			}
		}

		public ProtoIncludeAttribute(int tag, Type knownType)
			: this(tag, (knownType == null) ? "" : knownType.AssemblyQualifiedName)
		{
		}

		public ProtoIncludeAttribute(int tag, string knownTypeName)
		{
			if (tag <= 0)
			{
				throw new ArgumentOutOfRangeException("tag", "Tags must be positive integers");
			}
			if (Helpers.IsNullOrEmpty(knownTypeName))
			{
				throw new ArgumentNullException("knownTypeName", "Known type cannot be blank");
			}
			this.tag = tag;
			this.knownTypeName = knownTypeName;
		}
	}
}
