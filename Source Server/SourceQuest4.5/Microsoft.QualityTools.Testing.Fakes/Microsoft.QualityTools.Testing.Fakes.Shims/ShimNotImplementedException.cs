using System;
using System.Runtime.Serialization;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	[Serializable]
	public class ShimNotImplementedException : Exception
	{
		public ShimNotImplementedException()
		{
		}

		public ShimNotImplementedException(string message)
			: base(message)
		{
		}

		public ShimNotImplementedException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected ShimNotImplementedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
