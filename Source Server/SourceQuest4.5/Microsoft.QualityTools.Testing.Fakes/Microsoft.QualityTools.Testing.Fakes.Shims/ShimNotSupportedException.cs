using System;
using System.Runtime.Serialization;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	[Serializable]
	public class ShimNotSupportedException : Exception
	{
		public ShimNotSupportedException()
		{
		}

		public ShimNotSupportedException(string message)
			: base(message)
		{
		}

		public ShimNotSupportedException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected ShimNotSupportedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
