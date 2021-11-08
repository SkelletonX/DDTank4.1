using System.Reflection.Emit;

namespace ProtoBuf.Compiler
{
	internal struct CodeLabel
	{
		public readonly Label Value;

		public readonly int Index;

		public CodeLabel(Label value, int index)
		{
			Value = value;
			Index = index;
		}
	}
}
