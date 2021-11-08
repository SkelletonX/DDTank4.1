using System;
using System.ComponentModel;

namespace ProtoBuf
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	[ImmutableObject(true)]
	public sealed class ProtoAfterDeserializationAttribute : Attribute
	{
	}
}
