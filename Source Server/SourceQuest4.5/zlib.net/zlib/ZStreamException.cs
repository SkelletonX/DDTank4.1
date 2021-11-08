using System;
using System.IO;

namespace zlib
{
	[Serializable]
	public class ZStreamException : IOException
	{
		public ZStreamException()
		{
		}

		public ZStreamException(string s)
			: base(s)
		{
		}
	}
}
