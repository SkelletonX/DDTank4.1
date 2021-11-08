using System;

namespace ProtoBuf
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ProtoEnumAttribute : Attribute
	{
		private bool hasValue;

		private int enumValue;

		private string name;

		public int Value
		{
			get
			{
				return enumValue;
			}
			set
			{
				enumValue = value;
				hasValue = true;
			}
		}

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

		public bool HasValue()
		{
			return hasValue;
		}
	}
}
