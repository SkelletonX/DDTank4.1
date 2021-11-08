namespace zlib
{
	internal sealed class Inflate
	{
		private const int MAX_WBITS = 15;

		private const int PRESET_DICT = 32;

		internal const int Z_NO_FLUSH = 0;

		internal const int Z_PARTIAL_FLUSH = 1;

		internal const int Z_SYNC_FLUSH = 2;

		internal const int Z_FULL_FLUSH = 3;

		internal const int Z_FINISH = 4;

		private const int Z_DEFLATED = 8;

		private const int Z_OK = 0;

		private const int Z_STREAM_END = 1;

		private const int Z_NEED_DICT = 2;

		private const int Z_ERRNO = -1;

		private const int Z_STREAM_ERROR = -2;

		private const int Z_DATA_ERROR = -3;

		private const int Z_MEM_ERROR = -4;

		private const int Z_BUF_ERROR = -5;

		private const int Z_VERSION_ERROR = -6;

		private const int METHOD = 0;

		private const int FLAG = 1;

		private const int DICT4 = 2;

		private const int DICT3 = 3;

		private const int DICT2 = 4;

		private const int DICT1 = 5;

		private const int DICT0 = 6;

		private const int BLOCKS = 7;

		private const int CHECK4 = 8;

		private const int CHECK3 = 9;

		private const int CHECK2 = 10;

		private const int CHECK1 = 11;

		private const int DONE = 12;

		private const int BAD = 13;

		internal int mode;

		internal int method;

		internal long[] was = new long[1];

		internal long need;

		internal int marker;

		internal int nowrap;

		internal int wbits;

		internal InfBlocks blocks;

		private static byte[] mark = new byte[4]
		{
			0,
			0,
			(byte)SupportClass.Identity(255L),
			(byte)SupportClass.Identity(255L)
		};

		internal int inflateReset(ZStream z)
		{
			if (z == null || z.istate == null)
			{
				return -2;
			}
			z.total_in = (z.total_out = 0L);
			z.msg = null;
			z.istate.mode = ((z.istate.nowrap != 0) ? 7 : 0);
			z.istate.blocks.reset(z, null);
			return 0;
		}

		internal int inflateEnd(ZStream z)
		{
			if (blocks != null)
			{
				blocks.free(z);
			}
			blocks = null;
			return 0;
		}

		internal int inflateInit(ZStream z, int w)
		{
			z.msg = null;
			blocks = null;
			nowrap = 0;
			if (w < 0)
			{
				w = -w;
				nowrap = 1;
			}
			if (w < 8 || w > 15)
			{
				inflateEnd(z);
				return -2;
			}
			wbits = w;
			z.istate.blocks = new InfBlocks(z, (z.istate.nowrap != 0) ? null : this, 1 << w);
			inflateReset(z);
			return 0;
		}

		internal int inflate(ZStream z, int f)
		{
			if (z == null || z.istate == null || z.next_in == null)
			{
				return -2;
			}
			f = ((f == 4) ? (-5) : 0);
			int num = -5;
			while (true)
			{
				switch (z.istate.mode)
				{
				case 0:
					if (z.avail_in == 0)
					{
						return num;
					}
					num = f;
					z.avail_in--;
					z.total_in++;
					if (((z.istate.method = z.next_in[z.next_in_index++]) & 0xF) != 8)
					{
						z.istate.mode = 13;
						z.msg = "unknown compression method";
						z.istate.marker = 5;
						break;
					}
					if ((z.istate.method >> 4) + 8 > z.istate.wbits)
					{
						z.istate.mode = 13;
						z.msg = "invalid window size";
						z.istate.marker = 5;
						break;
					}
					z.istate.mode = 1;
					goto case 1;
				case 1:
				{
					if (z.avail_in == 0)
					{
						return num;
					}
					num = f;
					z.avail_in--;
					z.total_in++;
					int num2 = z.next_in[z.next_in_index++] & 0xFF;
					if (((z.istate.method << 8) + num2) % 31 != 0)
					{
						z.istate.mode = 13;
						z.msg = "incorrect header check";
						z.istate.marker = 5;
						break;
					}
					if ((num2 & 0x20) == 0)
					{
						z.istate.mode = 7;
						break;
					}
					z.istate.mode = 2;
					goto case 2;
				}
				case 2:
					if (z.avail_in == 0)
					{
						return num;
					}
					num = f;
					z.avail_in--;
					z.total_in++;
					z.istate.need = (((z.next_in[z.next_in_index++] & 0xFF) << 24) & -16777216);
					z.istate.mode = 3;
					goto case 3;
				case 3:
					if (z.avail_in == 0)
					{
						return num;
					}
					num = f;
					z.avail_in--;
					z.total_in++;
					z.istate.need += ((long)((z.next_in[z.next_in_index++] & 0xFF) << 16) & 16711680L);
					z.istate.mode = 4;
					goto case 4;
				case 4:
					if (z.avail_in == 0)
					{
						return num;
					}
					num = f;
					z.avail_in--;
					z.total_in++;
					z.istate.need += ((long)((z.next_in[z.next_in_index++] & 0xFF) << 8) & 65280L);
					z.istate.mode = 5;
					goto case 5;
				case 5:
					if (z.avail_in == 0)
					{
						return num;
					}
					num = f;
					z.avail_in--;
					z.total_in++;
					z.istate.need += ((long)z.next_in[z.next_in_index++] & 255L);
					z.adler = z.istate.need;
					z.istate.mode = 6;
					return 2;
				case 6:
					z.istate.mode = 13;
					z.msg = "need dictionary";
					z.istate.marker = 0;
					return -2;
				case 7:
					num = z.istate.blocks.proc(z, num);
					switch (num)
					{
					case -3:
						z.istate.mode = 13;
						z.istate.marker = 0;
						continue;
					case 0:
						num = f;
						break;
					}
					if (num != 1)
					{
						return num;
					}
					num = f;
					z.istate.blocks.reset(z, z.istate.was);
					if (z.istate.nowrap != 0)
					{
						z.istate.mode = 12;
						break;
					}
					z.istate.mode = 8;
					goto case 8;
				case 8:
					if (z.avail_in == 0)
					{
						return num;
					}
					num = f;
					z.avail_in--;
					z.total_in++;
					z.istate.need = (((z.next_in[z.next_in_index++] & 0xFF) << 24) & -16777216);
					z.istate.mode = 9;
					goto case 9;
				case 9:
					if (z.avail_in == 0)
					{
						return num;
					}
					num = f;
					z.avail_in--;
					z.total_in++;
					z.istate.need += ((long)((z.next_in[z.next_in_index++] & 0xFF) << 16) & 16711680L);
					z.istate.mode = 10;
					goto case 10;
				case 10:
					if (z.avail_in == 0)
					{
						return num;
					}
					num = f;
					z.avail_in--;
					z.total_in++;
					z.istate.need += ((long)((z.next_in[z.next_in_index++] & 0xFF) << 8) & 65280L);
					z.istate.mode = 11;
					goto case 11;
				case 11:
					if (z.avail_in == 0)
					{
						return num;
					}
					num = f;
					z.avail_in--;
					z.total_in++;
					z.istate.need += ((long)z.next_in[z.next_in_index++] & 255L);
					if ((int)z.istate.was[0] != (int)z.istate.need)
					{
						z.istate.mode = 13;
						z.msg = "incorrect data check";
						z.istate.marker = 5;
						break;
					}
					z.istate.mode = 12;
					goto case 12;
				case 12:
					return 1;
				case 13:
					return -3;
				default:
					return -2;
				}
			}
		}

		internal int inflateSetDictionary(ZStream z, byte[] dictionary, int dictLength)
		{
			int start = 0;
			int num = dictLength;
			if (z == null || z.istate == null || z.istate.mode != 6)
			{
				return -2;
			}
			if (z._adler.adler32(1L, dictionary, 0, dictLength) != z.adler)
			{
				return -3;
			}
			z.adler = z._adler.adler32(0L, null, 0, 0);
			if (num >= 1 << z.istate.wbits)
			{
				num = (1 << z.istate.wbits) - 1;
				start = dictLength - num;
			}
			z.istate.blocks.set_dictionary(dictionary, start, num);
			z.istate.mode = 7;
			return 0;
		}

		internal int inflateSync(ZStream z)
		{
			if (z == null || z.istate == null)
			{
				return -2;
			}
			if (z.istate.mode != 13)
			{
				z.istate.mode = 13;
				z.istate.marker = 0;
			}
			int num;
			if ((num = z.avail_in) == 0)
			{
				return -5;
			}
			int num2 = z.next_in_index;
			int num3 = z.istate.marker;
			while (num != 0 && num3 < 4)
			{
				num3 = ((z.next_in[num2] != mark[num3]) ? ((z.next_in[num2] == 0) ? (4 - num3) : 0) : (num3 + 1));
				num2++;
				num--;
			}
			z.total_in += num2 - z.next_in_index;
			z.next_in_index = num2;
			z.avail_in = num;
			z.istate.marker = num3;
			if (num3 != 4)
			{
				return -3;
			}
			long total_in = z.total_in;
			long total_out = z.total_out;
			inflateReset(z);
			z.total_in = total_in;
			z.total_out = total_out;
			z.istate.mode = 7;
			return 0;
		}

		internal int inflateSyncPoint(ZStream z)
		{
			if (z == null || z.istate == null || z.istate.blocks == null)
			{
				return -2;
			}
			return z.istate.blocks.sync_point();
		}
	}
}
