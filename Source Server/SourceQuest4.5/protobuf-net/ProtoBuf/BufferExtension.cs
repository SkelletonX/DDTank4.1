using System.IO;

namespace ProtoBuf
{
	public sealed class BufferExtension : IExtension, IExtensionResettable
	{
		private byte[] buffer;

		void IExtensionResettable.Reset()
		{
			buffer = null;
		}

		int IExtension.GetLength()
		{
			if (buffer != null)
			{
				return buffer.Length;
			}
			return 0;
		}

		Stream IExtension.BeginAppend()
		{
			return new MemoryStream();
		}

		void IExtension.EndAppend(Stream stream, bool commit)
		{
			using (stream)
			{
				int len;
				if (commit && (len = (int)stream.Length) > 0)
				{
					MemoryStream ms = (MemoryStream)stream;
					if (buffer == null)
					{
						buffer = ms.ToArray();
					}
					else
					{
						int offset = buffer.Length;
						byte[] tmp = new byte[offset + len];
						Helpers.BlockCopy(buffer, 0, tmp, 0, offset);
						Helpers.BlockCopy(Helpers.GetBuffer(ms), 0, tmp, offset, len);
						buffer = tmp;
					}
				}
			}
		}

		Stream IExtension.BeginQuery()
		{
			if (buffer != null)
			{
				return new MemoryStream(buffer);
			}
			return Stream.Null;
		}

		void IExtension.EndQuery(Stream stream)
		{
			using (stream)
			{
			}
		}
	}
}
