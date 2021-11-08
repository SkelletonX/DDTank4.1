using System;
using System.Runtime.Serialization;

namespace ProtoBuf
{
	[Serializable]
	public class ProtoException : Exception
	{
		public ProtoException()
		{
		}

		public ProtoException(string message)
			: base(message)
		{
		}

		public ProtoException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ProtoException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
