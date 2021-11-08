namespace zlib
{
	internal sealed class Adler32
	{
		private const int BASE = 65521;

		private const int NMAX = 5552;

		internal long adler32(long adler, byte[] buf, int index, int len)
		{
			if (buf == null)
			{
				return 1L;
			}
			long num = adler & 0xFFFF;
			long num2 = (adler >> 16) & 0xFFFF;
			while (len > 0)
			{
				int num3 = (len < 5552) ? len : 5552;
				len -= num3;
				while (num3 >= 16)
				{
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num += (buf[index++] & 0xFF);
					num2 += num;
					num3 -= 16;
				}
				if (num3 != 0)
				{
					do
					{
						num += (buf[index++] & 0xFF);
						num2 += num;
					}
					while (--num3 != 0);
				}
				num %= 65521;
				num2 %= 65521;
			}
			return (num2 << 16) | num;
		}
	}
}
