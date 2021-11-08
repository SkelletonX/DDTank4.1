using System;

namespace zlib
{
	internal sealed class InfBlocks
	{
		private const int MANY = 1440;

		private const int Z_OK = 0;

		private const int Z_STREAM_END = 1;

		private const int Z_NEED_DICT = 2;

		private const int Z_ERRNO = -1;

		private const int Z_STREAM_ERROR = -2;

		private const int Z_DATA_ERROR = -3;

		private const int Z_MEM_ERROR = -4;

		private const int Z_BUF_ERROR = -5;

		private const int Z_VERSION_ERROR = -6;

		private const int TYPE = 0;

		private const int LENS = 1;

		private const int STORED = 2;

		private const int TABLE = 3;

		private const int BTREE = 4;

		private const int DTREE = 5;

		private const int CODES = 6;

		private const int DRY = 7;

		private const int DONE = 8;

		private const int BAD = 9;

		private static readonly int[] inflate_mask = new int[17]
		{
			0,
			1,
			3,
			7,
			15,
			31,
			63,
			127,
			255,
			511,
			1023,
			2047,
			4095,
			8191,
			16383,
			32767,
			65535
		};

		internal static readonly int[] border = new int[19]
		{
			16,
			17,
			18,
			0,
			8,
			7,
			9,
			6,
			10,
			5,
			11,
			4,
			12,
			3,
			13,
			2,
			14,
			1,
			15
		};

		internal int mode;

		internal int left;

		internal int table;

		internal int index;

		internal int[] blens;

		internal int[] bb = new int[1];

		internal int[] tb = new int[1];

		internal InfCodes codes;

		internal int last;

		internal int bitk;

		internal int bitb;

		internal int[] hufts;

		internal byte[] window;

		internal int end;

		internal int read;

		internal int write;

		internal object checkfn;

		internal long check;

		internal InfBlocks(ZStream z, object checkfn, int w)
		{
			hufts = new int[4320];
			window = new byte[w];
			end = w;
			this.checkfn = checkfn;
			mode = 0;
			reset(z, null);
		}

		internal void reset(ZStream z, long[] c)
		{
			if (c != null)
			{
				c[0] = check;
			}
			if (mode == 4 || mode == 5)
			{
				blens = null;
			}
			if (mode == 6)
			{
				codes.free(z);
			}
			mode = 0;
			bitk = 0;
			bitb = 0;
			read = (write = 0);
			if (checkfn != null)
			{
				z.adler = (check = z._adler.adler32(0L, null, 0, 0));
			}
		}

		internal int proc(ZStream z, int r)
		{
			int num = z.next_in_index;
			int num2 = z.avail_in;
			int num3 = bitb;
			int i = bitk;
			int num4 = write;
			int num5 = (num4 < read) ? (read - num4 - 1) : (end - num4);
			while (true)
			{
				switch (mode)
				{
				case 0:
				{
					for (; i < 3; i += 8)
					{
						if (num2 != 0)
						{
							r = 0;
							num2--;
							num3 |= (z.next_in[num++] & 0xFF) << i;
							continue;
						}
						bitb = num3;
						bitk = i;
						z.avail_in = num2;
						z.total_in += num - z.next_in_index;
						z.next_in_index = num;
						write = num4;
						return inflate_flush(z, r);
					}
					int num6 = num3 & 7;
					last = (num6 & 1);
					switch (SupportClass.URShift(num6, 1))
					{
					case 0:
						num3 = SupportClass.URShift(num3, 3);
						i -= 3;
						num6 = (i & 7);
						num3 = SupportClass.URShift(num3, num6);
						i -= num6;
						mode = 1;
						break;
					case 1:
					{
						int[] array5 = new int[1];
						int[] array6 = new int[1];
						int[][] array7 = new int[1][];
						int[][] array8 = new int[1][];
						InfTree.inflate_trees_fixed(array5, array6, array7, array8, z);
						codes = new InfCodes(array5[0], array6[0], array7[0], array8[0], z);
						num3 = SupportClass.URShift(num3, 3);
						i -= 3;
						mode = 6;
						break;
					}
					case 2:
						num3 = SupportClass.URShift(num3, 3);
						i -= 3;
						mode = 3;
						break;
					case 3:
						num3 = SupportClass.URShift(num3, 3);
						i -= 3;
						mode = 9;
						z.msg = "invalid block type";
						r = -3;
						bitb = num3;
						bitk = i;
						z.avail_in = num2;
						z.total_in += num - z.next_in_index;
						z.next_in_index = num;
						write = num4;
						return inflate_flush(z, r);
					}
					break;
				}
				case 1:
					for (; i < 32; i += 8)
					{
						if (num2 != 0)
						{
							r = 0;
							num2--;
							num3 |= (z.next_in[num++] & 0xFF) << i;
							continue;
						}
						bitb = num3;
						bitk = i;
						z.avail_in = num2;
						z.total_in += num - z.next_in_index;
						z.next_in_index = num;
						write = num4;
						return inflate_flush(z, r);
					}
					if ((SupportClass.URShift(~num3, 16) & 0xFFFF) != (num3 & 0xFFFF))
					{
						mode = 9;
						z.msg = "invalid stored block lengths";
						r = -3;
						bitb = num3;
						bitk = i;
						z.avail_in = num2;
						z.total_in += num - z.next_in_index;
						z.next_in_index = num;
						write = num4;
						return inflate_flush(z, r);
					}
					left = (num3 & 0xFFFF);
					num3 = (i = 0);
					mode = ((left != 0) ? 2 : ((last != 0) ? 7 : 0));
					break;
				case 2:
				{
					if (num2 == 0)
					{
						bitb = num3;
						bitk = i;
						z.avail_in = num2;
						z.total_in += num - z.next_in_index;
						z.next_in_index = num;
						write = num4;
						return inflate_flush(z, r);
					}
					if (num5 == 0)
					{
						if (num4 == end && read != 0)
						{
							num4 = 0;
							num5 = ((num4 < read) ? (read - num4 - 1) : (end - num4));
						}
						if (num5 == 0)
						{
							write = num4;
							r = inflate_flush(z, r);
							num4 = write;
							num5 = ((num4 < read) ? (read - num4 - 1) : (end - num4));
							if (num4 == end && read != 0)
							{
								num4 = 0;
								num5 = ((num4 < read) ? (read - num4 - 1) : (end - num4));
							}
							if (num5 == 0)
							{
								bitb = num3;
								bitk = i;
								z.avail_in = num2;
								z.total_in += num - z.next_in_index;
								z.next_in_index = num;
								write = num4;
								return inflate_flush(z, r);
							}
						}
					}
					r = 0;
					int num6 = left;
					if (num6 > num2)
					{
						num6 = num2;
					}
					if (num6 > num5)
					{
						num6 = num5;
					}
					Array.Copy(z.next_in, num, window, num4, num6);
					num += num6;
					num2 -= num6;
					num4 += num6;
					num5 -= num6;
					if ((left -= num6) == 0)
					{
						mode = ((last != 0) ? 7 : 0);
					}
					break;
				}
				case 3:
				{
					for (; i < 14; i += 8)
					{
						if (num2 != 0)
						{
							r = 0;
							num2--;
							num3 |= (z.next_in[num++] & 0xFF) << i;
							continue;
						}
						bitb = num3;
						bitk = i;
						z.avail_in = num2;
						z.total_in += num - z.next_in_index;
						z.next_in_index = num;
						write = num4;
						return inflate_flush(z, r);
					}
					int num6 = table = (num3 & 0x3FFF);
					if ((num6 & 0x1F) > 29 || ((num6 >> 5) & 0x1F) > 29)
					{
						mode = 9;
						z.msg = "too many length or distance symbols";
						r = -3;
						bitb = num3;
						bitk = i;
						z.avail_in = num2;
						z.total_in += num - z.next_in_index;
						z.next_in_index = num;
						write = num4;
						return inflate_flush(z, r);
					}
					num6 = 258 + (num6 & 0x1F) + ((num6 >> 5) & 0x1F);
					blens = new int[num6];
					num3 = SupportClass.URShift(num3, 14);
					i -= 14;
					index = 0;
					mode = 4;
					goto case 4;
				}
				case 4:
				{
					while (index < 4 + SupportClass.URShift(table, 10))
					{
						for (; i < 3; i += 8)
						{
							if (num2 != 0)
							{
								r = 0;
								num2--;
								num3 |= (z.next_in[num++] & 0xFF) << i;
								continue;
							}
							bitb = num3;
							bitk = i;
							z.avail_in = num2;
							z.total_in += num - z.next_in_index;
							z.next_in_index = num;
							write = num4;
							return inflate_flush(z, r);
						}
						blens[border[index++]] = (num3 & 7);
						num3 = SupportClass.URShift(num3, 3);
						i -= 3;
					}
					while (index < 19)
					{
						blens[border[index++]] = 0;
					}
					bb[0] = 7;
					int num6 = InfTree.inflate_trees_bits(blens, bb, tb, hufts, z);
					if (num6 != 0)
					{
						r = num6;
						if (r == -3)
						{
							blens = null;
							mode = 9;
						}
						bitb = num3;
						bitk = i;
						z.avail_in = num2;
						z.total_in += num - z.next_in_index;
						z.next_in_index = num;
						write = num4;
						return inflate_flush(z, r);
					}
					index = 0;
					mode = 5;
					goto case 5;
				}
				case 5:
				{
					int num6;
					while (true)
					{
						num6 = table;
						if (index >= 258 + (num6 & 0x1F) + ((num6 >> 5) & 0x1F))
						{
							break;
						}
						for (num6 = bb[0]; i < num6; i += 8)
						{
							if (num2 != 0)
							{
								r = 0;
								num2--;
								num3 |= (z.next_in[num++] & 0xFF) << i;
								continue;
							}
							bitb = num3;
							bitk = i;
							z.avail_in = num2;
							z.total_in += num - z.next_in_index;
							z.next_in_index = num;
							write = num4;
							return inflate_flush(z, r);
						}
						_ = tb[0];
						_ = -1;
						num6 = hufts[(tb[0] + (num3 & inflate_mask[num6])) * 3 + 1];
						int num7 = hufts[(tb[0] + (num3 & inflate_mask[num6])) * 3 + 2];
						if (num7 < 16)
						{
							num3 = SupportClass.URShift(num3, num6);
							i -= num6;
							blens[index++] = num7;
							continue;
						}
						int num8 = (num7 == 18) ? 7 : (num7 - 14);
						int num9 = (num7 == 18) ? 11 : 3;
						for (; i < num6 + num8; i += 8)
						{
							if (num2 != 0)
							{
								r = 0;
								num2--;
								num3 |= (z.next_in[num++] & 0xFF) << i;
								continue;
							}
							bitb = num3;
							bitk = i;
							z.avail_in = num2;
							z.total_in += num - z.next_in_index;
							z.next_in_index = num;
							write = num4;
							return inflate_flush(z, r);
						}
						num3 = SupportClass.URShift(num3, num6);
						i -= num6;
						num9 += (num3 & inflate_mask[num8]);
						num3 = SupportClass.URShift(num3, num8);
						i -= num8;
						num8 = index;
						num6 = table;
						if (num8 + num9 > 258 + (num6 & 0x1F) + ((num6 >> 5) & 0x1F) || (num7 == 16 && num8 < 1))
						{
							blens = null;
							mode = 9;
							z.msg = "invalid bit length repeat";
							r = -3;
							bitb = num3;
							bitk = i;
							z.avail_in = num2;
							z.total_in += num - z.next_in_index;
							z.next_in_index = num;
							write = num4;
							return inflate_flush(z, r);
						}
						num7 = ((num7 == 16) ? blens[num8 - 1] : 0);
						do
						{
							blens[num8++] = num7;
						}
						while (--num9 != 0);
						index = num8;
					}
					tb[0] = -1;
					int[] array = new int[1];
					int[] array2 = new int[1];
					int[] array3 = new int[1];
					int[] array4 = new int[1];
					array[0] = 9;
					array2[0] = 6;
					num6 = table;
					num6 = InfTree.inflate_trees_dynamic(257 + (num6 & 0x1F), 1 + ((num6 >> 5) & 0x1F), blens, array, array2, array3, array4, hufts, z);
					if (num6 != 0)
					{
						if (num6 == -3)
						{
							blens = null;
							mode = 9;
						}
						r = num6;
						bitb = num3;
						bitk = i;
						z.avail_in = num2;
						z.total_in += num - z.next_in_index;
						z.next_in_index = num;
						write = num4;
						return inflate_flush(z, r);
					}
					codes = new InfCodes(array[0], array2[0], hufts, array3[0], hufts, array4[0], z);
					blens = null;
					mode = 6;
					goto case 6;
				}
				case 6:
					bitb = num3;
					bitk = i;
					z.avail_in = num2;
					z.total_in += num - z.next_in_index;
					z.next_in_index = num;
					write = num4;
					if ((r = codes.proc(this, z, r)) != 1)
					{
						return inflate_flush(z, r);
					}
					r = 0;
					codes.free(z);
					num = z.next_in_index;
					num2 = z.avail_in;
					num3 = bitb;
					i = bitk;
					num4 = write;
					num5 = ((num4 < read) ? (read - num4 - 1) : (end - num4));
					if (last == 0)
					{
						mode = 0;
						break;
					}
					mode = 7;
					goto case 7;
				case 7:
					write = num4;
					r = inflate_flush(z, r);
					num4 = write;
					num5 = ((num4 < read) ? (read - num4 - 1) : (end - num4));
					if (read != write)
					{
						bitb = num3;
						bitk = i;
						z.avail_in = num2;
						z.total_in += num - z.next_in_index;
						z.next_in_index = num;
						write = num4;
						return inflate_flush(z, r);
					}
					mode = 8;
					goto case 8;
				case 8:
					r = 1;
					bitb = num3;
					bitk = i;
					z.avail_in = num2;
					z.total_in += num - z.next_in_index;
					z.next_in_index = num;
					write = num4;
					return inflate_flush(z, r);
				case 9:
					r = -3;
					bitb = num3;
					bitk = i;
					z.avail_in = num2;
					z.total_in += num - z.next_in_index;
					z.next_in_index = num;
					write = num4;
					return inflate_flush(z, r);
				default:
					r = -2;
					bitb = num3;
					bitk = i;
					z.avail_in = num2;
					z.total_in += num - z.next_in_index;
					z.next_in_index = num;
					write = num4;
					return inflate_flush(z, r);
				}
			}
		}

		internal void free(ZStream z)
		{
			reset(z, null);
			window = null;
			hufts = null;
		}

		internal void set_dictionary(byte[] d, int start, int n)
		{
			Array.Copy(d, start, window, 0, n);
			read = (write = n);
		}

		internal int sync_point()
		{
			if (mode != 1)
			{
				return 0;
			}
			return 1;
		}

		internal int inflate_flush(ZStream z, int r)
		{
			int next_out_index = z.next_out_index;
			int num = read;
			int num2 = ((num <= write) ? write : end) - num;
			if (num2 > z.avail_out)
			{
				num2 = z.avail_out;
			}
			if (num2 != 0 && r == -5)
			{
				r = 0;
			}
			z.avail_out -= num2;
			z.total_out += num2;
			if (checkfn != null)
			{
				z.adler = (check = z._adler.adler32(check, window, num, num2));
			}
			Array.Copy(window, num, z.next_out, next_out_index, num2);
			next_out_index += num2;
			num += num2;
			if (num == end)
			{
				num = 0;
				if (write == end)
				{
					write = 0;
				}
				num2 = write - num;
				if (num2 > z.avail_out)
				{
					num2 = z.avail_out;
				}
				if (num2 != 0 && r == -5)
				{
					r = 0;
				}
				z.avail_out -= num2;
				z.total_out += num2;
				if (checkfn != null)
				{
					z.adler = (check = z._adler.adler32(check, window, num, num2));
				}
				Array.Copy(window, num, z.next_out, next_out_index, num2);
				next_out_index += num2;
				num += num2;
			}
			z.next_out_index = next_out_index;
			read = num;
			return r;
		}
	}
}
