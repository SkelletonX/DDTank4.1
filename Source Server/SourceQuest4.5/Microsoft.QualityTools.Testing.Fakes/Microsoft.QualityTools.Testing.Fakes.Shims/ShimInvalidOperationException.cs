using System;
using System.Runtime.Serialization;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	[Serializable]
	public class ShimInvalidOperationException : Exception
	{
		public ShimInvalidOperationException()
		{
		}

		public ShimInvalidOperationException(string message)
			: base(message)
		{
		}

		public ShimInvalidOperationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected ShimInvalidOperationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
