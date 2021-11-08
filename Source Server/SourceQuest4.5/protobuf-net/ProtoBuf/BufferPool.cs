using System.Threading;

namespace ProtoBuf
{
	internal sealed class BufferPool
	{
		private const int PoolSize = 20;

		internal const int BufferLength = 1024;

		private static readonly object[] pool = new object[20];

		internal static void Flush()
		{
			for (int i = 0; i < pool.Length; i++)
			{
				Interlocked.Exchange(ref pool[i], null);
			}
		}

		private BufferPool()
		{
		}

		internal static byte[] GetBuffer()
		{
			for (int i = 0; i < pool.Length; i++)
			{
				object tmp;
				if ((tmp = Interlocked.Exchange(ref pool[i], null)) != null)
				{
					return (byte[])tmp;
				}
			}
			return new byte[1024];
		}

		internal static void ResizeAndFlushLeft(ref byte[] buffer, int toFitAtLeastBytes, int copyFromIndex, int copyBytes)
		{
			int newLength = buffer.Length * 2;
			if (newLength < toFitAtLeastBytes)
			{
				newLength = toFitAtLeastBytes;
			}
			byte[] newBuffer = new byte[newLength];
			if (copyBytes > 0)
			{
				Helpers.BlockCopy(buffer, copyFromIndex, newBuffer, 0, copyBytes);
			}
			if (buffer.Length == 1024)
			{
				ReleaseBufferToPool(ref buffer);
			}
			buffer = newBuffer;
		}

		internal static void ReleaseBufferToPool(ref byte[] buffer)
		{
			if (buffer == null)
			{
				return;
			}
			if (buffer.Length == 1024)
			{
				for (int i = 0; i < pool.Length && Interlocked.CompareExchange(ref pool[i], buffer, null) != null; i++)
				{
				}
			}
			buffer = null;
		}
	}
}
