using System;

namespace Newtonsoft.Json
{
	[Flags]
	public enum PreserveReferencesHandling
	{
		None = 0x0,
		Objects = 0x1,
		Arrays = 0x2,
		All = 0x3
	}
}
