using System;
using System.Runtime.Serialization;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	[Serializable]
	public class ShimMethodNotBindableException : Exception
	{
		public ShimMethodNotBindableException()
		{
		}

		public ShimMethodNotBindableException(string message)
			: base(message)
		{
		}

		public ShimMethodNotBindableException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected ShimMethodNotBindableException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
