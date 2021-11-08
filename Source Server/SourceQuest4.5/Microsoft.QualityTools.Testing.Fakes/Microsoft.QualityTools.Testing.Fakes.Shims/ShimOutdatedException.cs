using System;
using System.Runtime.Serialization;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	[Serializable]
	public class ShimOutdatedException : Exception
	{
		public ShimOutdatedException()
			: this(FakesFrameworkResources.AMethodFromAMoleWasNotResolvedPleaseRegenerateTheMoles)
		{
		}

		public ShimOutdatedException(string message)
			: base(message)
		{
		}

		public ShimOutdatedException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected ShimOutdatedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
