using System;

namespace Microsoft.QualityTools.Testing.Fakes.Utils
{
	[Serializable]
	internal enum TypeSpec
	{
		SzArray,
		MdArray,
		Pointer,
		ManagedPointer,
		ValueType,
		Class,
		GenericParameter
	}
}
