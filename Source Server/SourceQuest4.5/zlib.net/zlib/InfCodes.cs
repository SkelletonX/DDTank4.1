using System;

namespace zlib
{
	internal sealed class InfCodes
	{
		private const int Z_OK = 0;

		private const int Z_STREAM_END = 1;

		private const int Z_NEED_DICT = 2;

		private const int Z_ERRNO = -1;

		private const int Z_STREAM_ERROR = -2;

		private const int Z_DATA_ERROR = -3;

		private const int Z_MEM_ERROR = -4;

		private const int Z_BUF_ERROR = -5;

		private const int Z_VERSION_ERROR = -6;

		private const int START = 0;

		private const int LEN = 1;

		private const int LENEXT = 2;

		private const int DIST = 3;

		private const int DISTEXT = 4;

		private const int COPY = 5;

		private const int LIT = 6;

		private const int WASH = 7;

		private const int END = 8;

		private const int BADCODE = 9;

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

		internal int mode;

		internal int len;

		internal int[] tree;

		internal int tree_index = 0;

		internal int need;

		internal int lit;

		internal int get_Renamed;

		internal int dist;

		internal byte lbits;

		internal byte dbits;

		internal int[] ltree;

		internal int ltree_index;

		internal int[] dtree;

		internal int dtree_index;

		internal InfCodes(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, ZStream z)
		{
			mode = 0;
			lbits = (byte)bl;
			dbits = (byte)bd;
			ltree = tl;
			ltree_index = tl_index;
			dtree = td;
			dtree_index = td_index;
		}

		internal InfCodes(int bl, int bd, int[] tl, int[] td, ZStream z)
		{
			mode = 0;
			lbits = (byte)bl;
			dbits = (byte)bd;
			ltree = tl;
			ltree_index = 0;
			dtree = td;
			dtree_index = 0;
		}

		internal int proc(InfBlocks s, ZStream z, int r)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			num3 = z.next_in_index;
			int num4 = z.avail_in;
			num = s.bitb;
			num2 = s.bitk;
			int num5 = s.write;
			int num6 = (num5 < s.read) ? (s.read - num5 - 1) : (s.end - num5);
			while (true)
			{
				switch (mode)
				{
				case 0:
					if (num6 >= 258 && num4 >= 10)
					{
						s.bitb = num;
						s.bitk = num2;
						z.avail_in = num4;
						z.total_in += num3 - z.next_in_index;
						z.next_in_index = num3;
						s.write = num5;
						r = inflate_fast(lbits, dbits, ltree, ltree_index, dtree, dtree_index, s, z);
						num3 = z.next_in_index;
						num4 = z.avail_in;
						num = s.bitb;
						num2 = s.bitk;
						num5 = s.write;
						num6 = ((num5 < s.read) ? (s.read - num5 - 1) : (s.end - num5));
						if (r != 0)
						{
							mode = ((r == 1) ? 7 : 9);
							break;
						}
					}
					need = lbits;
					tree = ltree;
					tree_index = ltree_index;
					mode = 1;
					goto case 1;
				case 1:
				{
					int num7;
					for (num7 = need; num2 < num7; num2 += 8)
					{
						if (num4 != 0)
						{
							r = 0;
							num4--;
							num |= (z.next_in[num3++] & 0xFF) << num2;
							continue;
						}
						s.bitb = num;
						s.bitk = num2;
						z.avail_in = num4;
						z.total_in += num3 - z.next_in_index;
						z.next_in_index = num3;
						s.write = num5;
						return s.inflate_flush(z, r);
					}
					int num8 = (tree_index + (num & inflate_mask[num7])) * 3;
					num = SupportClass.URShift(num, tree[num8 + 1]);
					num2 -= tree[num8 + 1];
					int num9 = tree[num8];
					if (num9 == 0)
					{
						lit = tree[num8 + 2];
						mode = 6;
						break;
					}
					if ((num9 & 0x10) != 0)
					{
						get_Renamed = (num9 & 0xF);
						len = tree[num8 + 2];
						mode = 2;
						break;
					}
					if ((num9 & 0x40) == 0)
					{
						need = num9;
						tree_index = num8 / 3 + tree[num8 + 2];
						break;
					}
					if ((num9 & 0x20) != 0)
					{
						mode = 7;
						break;
					}
					mode = 9;
					z.msg = "invalid literal/length code";
					r = -3;
					s.bitb = num;
					s.bitk = num2;
					z.avail_in = num4;
					z.total_in += num3 - z.next_in_index;
					z.next_in_index = num3;
					s.write = num5;
					return s.inflate_flush(z, r);
				}
				case 2:
				{
					int num7;
					for (num7 = get_Renamed; num2 < num7; num2 += 8)
					{
						if (num4 != 0)
						{
							r = 0;
							num4--;
							num |= (z.next_in[num3++] & 0xFF) << num2;
							continue;
						}
						s.bitb = num;
						s.bitk = num2;
						z.avail_in = num4;
						z.total_in += num3 - z.next_in_index;
						z.next_in_index = num3;
						s.write = num5;
						return s.inflate_flush(z, r);
					}
					len += (num & inflate_mask[num7]);
					num >>= num7;
					num2 -= num7;
					need = dbits;
					tree = dtree;
					tree_index = dtree_index;
					mode = 3;
					goto case 3;
				}
				case 3:
				{
					int num7;
					for (num7 = need; num2 < num7; num2 += 8)
					{
						if (num4 != 0)
						{
							r = 0;
							num4--;
							num |= (z.next_in[num3++] & 0xFF) << num2;
							continue;
						}
						s.bitb = num;
						s.bitk = num2;
						z.avail_in = num4;
						z.total_in += num3 - z.next_in_index;
						z.next_in_index = num3;
						s.write = num5;
						return s.inflate_flush(z, r);
					}
					int num8 = (tree_index + (num & inflate_mask[num7])) * 3;
					num >>= tree[num8 + 1];
					num2 -= tree[num8 + 1];
					int num9 = tree[num8];
					if ((num9 & 0x10) != 0)
					{
						get_Renamed = (num9 & 0xF);
						dist = tree[num8 + 2];
						mode = 4;
						break;
					}
					if ((num9 & 0x40) == 0)
					{
						need = num9;
						tree_index = num8 / 3 + tree[num8 + 2];
						break;
					}
					mode = 9;
					z.msg = "invalid distance code";
					r = -3;
					s.bitb = num;
					s.bitk = num2;
					z.avail_in = num4;
					z.total_in += num3 - z.next_in_index;
					z.next_in_index = num3;
					s.write = num5;
					return s.inflate_flush(z, r);
				}
				case 4:
				{
					int num7;
					for (num7 = get_Renamed; num2 < num7; num2 += 8)
					{
						if (num4 != 0)
						{
							r = 0;
							num4--;
							num |= (z.next_in[num3++] & 0xFF) << num2;
							continue;
						}
						s.bitb = num;
						s.bitk = num2;
						z.avail_in = num4;
						z.total_in += num3 - z.next_in_index;
						z.next_in_index = num3;
						s.write = num5;
						return s.inflate_flush(z, r);
					}
					dist += (num & inflate_mask[num7]);
					num >>= num7;
					num2 -= num7;
					mode = 5;
					goto case 5;
				}
				case 5:
				{
					int i;
					for (i = num5 - dist; i < 0; i += s.end)
					{
					}
					while (len != 0)
					{
						if (num6 == 0)
						{
							if (num5 == s.end && s.read != 0)
							{
								num5 = 0;
								num6 = ((num5 < s.read) ? (s.read - num5 - 1) : (s.end - num5));
							}
							if (num6 == 0)
							{
								s.write = num5;
								r = s.inflate_flush(z, r);
								num5 = s.write;
								num6 = ((num5 < s.read) ? (s.read - num5 - 1) : (s.end - num5));
								if (num5 == s.end && s.read != 0)
								{
									num5 = 0;
									num6 = ((num5 < s.read) ? (s.read - num5 - 1) : (s.end - num5));
								}
								if (num6 == 0)
								{
									s.bitb = num;
									s.bitk = num2;
									z.avail_in = num4;
									z.total_in += num3 - z.next_in_index;
									z.next_in_index = num3;
									s.write = num5;
									return s.inflate_flush(z, r);
								}
							}
						}
						s.window[num5++] = s.window[i++];
						num6--;
						if (i == s.end)
						{
							i = 0;
						}
						len--;
					}
					mode = 0;
					break;
				}
				case 6:
					if (num6 == 0)
					{
						if (num5 == s.end && s.read != 0)
						{
							num5 = 0;
							num6 = ((num5 < s.read) ? (s.read - num5 - 1) : (s.end - num5));
						}
						if (num6 == 0)
						{
							s.write = num5;
							r = s.inflate_flush(z, r);
							num5 = s.write;
							num6 = ((num5 < s.read) ? (s.read - num5 - 1) : (s.end - num5));
							if (num5 == s.end && s.read != 0)
							{
								num5 = 0;
								num6 = ((num5 < s.read) ? (s.read - num5 - 1) : (s.end - num5));
							}
							if (num6 == 0)
							{
								s.bitb = num;
								s.bitk = num2;
								z.avail_in = num4;
								z.total_in += num3 - z.next_in_index;
								z.next_in_index = num3;
								s.write = num5;
								return s.inflate_flush(z, r);
							}
						}
					}
					r = 0;
					s.window[num5++] = (byte)lit;
					num6--;
					mode = 0;
					break;
				case 7:
					if (num2 > 7)
					{
						num2 -= 8;
						num4++;
						num3--;
					}
					s.write = num5;
					r = s.inflate_flush(z, r);
					num5 = s.write;
					num6 = ((num5 < s.read) ? (s.read - num5 - 1) : (s.end - num5));
					if (s.read != s.write)
					{
						s.bitb = num;
						s.bitk = num2;
						z.avail_in = num4;
						z.total_in += num3 - z.next_in_index;
						z.next_in_index = num3;
						s.write = num5;
						return s.inflate_flush(z, r);
					}
					mode = 8;
					goto case 8;
				case 8:
					r = 1;
					s.bitb = num;
					s.bitk = num2;
					z.avail_in = num4;
					z.total_in += num3 - z.next_in_index;
					z.next_in_index = num3;
					s.write = num5;
					return s.inflate_flush(z, r);
				case 9:
					r = -3;
					s.bitb = num;
					s.bitk = num2;
					z.avail_in = num4;
					z.total_in += num3 - z.next_in_index;
					z.next_in_index = num3;
					s.write = num5;
					return s.inflate_flush(z, r);
				default:
					r = -2;
					s.bitb = num;
					s.bitk = num2;
					z.avail_in = num4;
					z.total_in += num3 - z.next_in_index;
					z.next_in_index = num3;
					s.write = num5;
					return s.inflate_flush(z, r);
				}
			}
		}

		internal void free(ZStream z)
		{
		}

		internal int inflate_fast(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, InfBlocks s, ZStream z)
		{
			int next_in_index = z.next_in_index;
			int num = z.avail_in;
			int num2 = s.bitb;
			int num3 = s.bitk;
			int num4 = s.write;
			int num5 = (num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4);
			int num6 = inflate_mask[bl];
			int num7 = inflate_mask[bd];
			int num11;
			while (true)
			{
				if (num3 < 20)
				{
					num--;
					num2 |= (z.next_in[next_in_index++] & 0xFF) << num3;
					num3 += 8;
					continue;
				}
				int num8 = num2 & num6;
				int[] array = tl;
				int num9 = tl_index;
				int num10;
				if ((num10 = array[(num9 + num8) * 3]) == 0)
				{
					num2 >>= array[(num9 + num8) * 3 + 1];
					num3 -= array[(num9 + num8) * 3 + 1];
					s.window[num4++] = (byte)array[(num9 + num8) * 3 + 2];
					num5--;
				}
				else
				{
					while (true)
					{
						num2 >>= array[(num9 + num8) * 3 + 1];
						num3 -= array[(num9 + num8) * 3 + 1];
						if ((num10 & 0x10) != 0)
						{
							num10 &= 0xF;
							num11 = array[(num9 + num8) * 3 + 2] + (num2 & inflate_mask[num10]);
							num2 >>= num10;
							for (num3 -= num10; num3 < 15; num3 += 8)
							{
								num--;
								num2 |= (z.next_in[next_in_index++] & 0xFF) << num3;
							}
							num8 = (num2 & num7);
							array = td;
							num9 = td_index;
							num10 = array[(num9 + num8) * 3];
							while (true)
							{
								num2 >>= array[(num9 + num8) * 3 + 1];
								num3 -= array[(num9 + num8) * 3 + 1];
								if ((num10 & 0x10) != 0)
								{
									break;
								}
								if ((num10 & 0x40) == 0)
								{
									num8 += array[(num9 + num8) * 3 + 2];
									num8 += (num2 & inflate_mask[num10]);
									num10 = array[(num9 + num8) * 3];
									continue;
								}
								z.msg = "invalid distance code";
								num11 = z.avail_in - num;
								num11 = ((num3 >> 3 < num11) ? (num3 >> 3) : num11);
								num += num11;
								next_in_index -= num11;
								num3 -= num11 << 3;
								s.bitb = num2;
								s.bitk = num3;
								z.avail_in = num;
								z.total_in += next_in_index - z.next_in_index;
								z.next_in_index = next_in_index;
								s.write = num4;
								return -3;
							}
							for (num10 &= 0xF; num3 < num10; num3 += 8)
							{
								num--;
								num2 |= (z.next_in[next_in_index++] & 0xFF) << num3;
							}
							int num12 = array[(num9 + num8) * 3 + 2] + (num2 & inflate_mask[num10]);
							num2 >>= num10;
							num3 -= num10;
							num5 -= num11;
							int num13;
							if (num4 >= num12)
							{
								num13 = num4 - num12;
								if (num4 - num13 > 0 && 2 > num4 - num13)
								{
									s.window[num4++] = s.window[num13++];
									num11--;
									s.window[num4++] = s.window[num13++];
									num11--;
								}
								else
								{
									Array.Copy(s.window, num13, s.window, num4, 2);
									num4 += 2;
									num13 += 2;
									num11 -= 2;
								}
							}
							else
							{
								num13 = num4 - num12;
								do
								{
									num13 += s.end;
								}
								while (num13 < 0);
								num10 = s.end - num13;
								if (num11 > num10)
								{
									num11 -= num10;
									if (num4 - num13 > 0 && num10 > num4 - num13)
									{
										do
										{
											s.window[num4++] = s.window[num13++];
										}
										while (--num10 != 0);
									}
									else
									{
										Array.Copy(s.window, num13, s.window, num4, num10);
										num4 += num10;
										num13 += num10;
										num10 = 0;
									}
									num13 = 0;
								}
							}
							if (num4 - num13 > 0 && num11 > num4 - num13)
							{
								do
								{
									s.window[num4++] = s.window[num13++];
								}
								while (--num11 != 0);
							}
							else
							{
								Array.Copy(s.window, num13, s.window, num4, num11);
								num4 += num11;
								num13 += num11;
								num11 = 0;
							}
							break;
						}
						if ((num10 & 0x40) == 0)
						{
							num8 += array[(num9 + num8) * 3 + 2];
							num8 += (num2 & inflate_mask[num10]);
							if ((num10 = array[(num9 + num8) * 3]) == 0)
							{
								num2 >>= array[(num9 + num8) * 3 + 1];
								num3 -= array[(num9 + num8) * 3 + 1];
								s.window[num4++] = (byte)array[(num9 + num8) * 3 + 2];
								num5--;
								break;
							}
							continue;
						}
						if ((num10 & 0x20) != 0)
						{
							num11 = z.avail_in - num;
							num11 = ((num3 >> 3 < num11) ? (num3 >> 3) : num11);
							num += num11;
							next_in_index -= num11;
							num3 -= num11 << 3;
							s.bitb = num2;
							s.bitk = num3;
							z.avail_in = num;
							z.total_in += next_in_index - z.next_in_index;
							z.next_in_index = next_in_index;
							s.write = num4;
							return 1;
						}
						z.msg = "invalid literal/length code";
						num11 = z.avail_in - num;
						num11 = ((num3 >> 3 < num11) ? (num3 >> 3) : num11);
						num += num11;
						next_in_index -= num11;
						num3 -= num11 << 3;
						s.bitb = num2;
						s.bitk = num3;
						z.avail_in = num;
						z.total_in += next_in_index - z.next_in_index;
						z.next_in_index = next_in_index;
						s.write = num4;
						return -3;
					}
				}
				if (num5 < 258 || num < 10)
				{
					break;
				}
			}
			num11 = z.avail_in - num;
			num11 = ((num3 >> 3 < num11) ? (num3 >> 3) : num11);
			num += num11;
			next_in_index -= num11;
			num3 -= num11 << 3;
			s.bitb = num2;
			s.bitk = num3;
			z.avail_in = num;
			z.total_in += next_in_index - z.next_in_index;
			z.next_in_index = next_in_index;
			s.write = num4;
			return 0;
		}
	}
}
