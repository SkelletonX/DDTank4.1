using System;
using System.Runtime.Serialization;

namespace Microsoft.QualityTools.Testing.Fakes.UnitTestIsolation
{
	[Serializable]
	public class UnitTestIsolationException : Exception
	{
		public UnitTestIsolationException()
		{
		}

		public UnitTestIsolationException(string message)
			: base(message)
		{
		}

		public UnitTestIsolationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected UnitTestIsolationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
