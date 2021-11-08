using System;

namespace Newtonsoft.Json.Schema
{
	[Flags]
	public enum JsonSchemaType
	{
		None = 0x0,
		String = 0x1,
		Float = 0x2,
		Integer = 0x4,
		Boolean = 0x8,
		Object = 0x10,
		Array = 0x20,
		Null = 0x40,
		Any = 0x7F
	}
}
