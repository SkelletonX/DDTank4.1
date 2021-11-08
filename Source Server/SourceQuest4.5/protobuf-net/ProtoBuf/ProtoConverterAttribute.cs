using System;

namespace ProtoBuf
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ProtoConverterAttribute : Attribute
	{
	}
}
