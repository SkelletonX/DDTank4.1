using System;

namespace zlib
{
	public sealed class Deflate
	{
		internal class Config
		{
			internal int good_length;

			internal int max_lazy;

			internal int nice_length;

			internal int max_chain;

			internal int func;

			internal Config(int good_length, int max_lazy, int nice_length, int max_chain, int func)
			{
				this.good_length = good_length;
				this.max_lazy = max_lazy;
				this.nice_length = nice_length;
				this.max_chain = max_chain;
				this.func = func;
			}
		}

		private const int MAX_MEM_LEVEL = 9;

		private const int Z_DEFAULT_COMPRESSION = -1;

		private const int MAX_WBITS = 15;

		private const int DEF_MEM_LEVEL = 8;

		private const int STORED = 0;

		private const int FAST = 1;

		private const int SLOW = 2;

		private const int NeedMore = 0;

		private const int BlockDone = 1;

		private const int FinishStarted = 2;

		private const int FinishDone = 3;

		private const int PRESET_DICT = 32;

		private const int Z_FILTERED = 1;

		private const int Z_HUFFMAN_ONLY = 2;

		private const int Z_DEFAULT_STRATEGY = 0;

		private const int Z_NO_FLUSH = 0;

		private const int Z_PARTIAL_FLUSH = 1;

		private const int Z_SYNC_FLUSH = 2;

		private const int Z_FULL_FLUSH = 3;

		private const int Z_FINISH = 4;

		private const int Z_OK = 0;

		private const int Z_STREAM_END = 1;

		private const int Z_NEED_DICT = 2;

		private const int Z_ERRNO = -1;

		private const int Z_STREAM_ERROR = -2;

		private const int Z_DATA_ERROR = -3;

		private const int Z_MEM_ERROR = -4;

		private const int Z_BUF_ERROR = -5;

		private const int Z_VERSION_ERROR = -6;

		private const int INIT_STATE = 42;

		private const int BUSY_STATE = 113;

		private const int FINISH_STATE = 666;

		private const int Z_DEFLATED = 8;

		private const int STORED_BLOCK = 0;

		private const int STATIC_TREES = 1;

		private const int DYN_TREES = 2;

		private const int Z_BINARY = 0;

		private const int Z_ASCII = 1;

		private const int Z_UNKNOWN = 2;

		private const int Buf_size = 16;

		private const int REP_3_6 = 16;

		private const int REPZ_3_10 = 17;

		private const int REPZ_11_138 = 18;

		private const int MIN_MATCH = 3;

		private const int MAX_MATCH = 258;

		private const int MAX_BITS = 15;

		private const int D_CODES = 30;

		private const int BL_CODES = 19;

		private const int LENGTH_CODES = 29;

		private const int LITERALS = 256;

		private const int END_BLOCK = 256;

		private static Config[] config_table;

		private static readonly string[] z_errmsg;

		private static readonly int MIN_LOOKAHEAD;

		private static readonly int L_CODES;

		private static readonly int HEAP_SIZE;

		internal ZStream strm;

		internal int status;

		internal byte[] pending_buf;

		internal int pending_buf_size;

		internal int pending_out;

		internal int pending;

		internal int noheader;

		internal byte data_type;

		internal byte method;

		internal int last_flush;

		internal int w_size;

		internal int w_bits;

		internal int w_mask;

		internal byte[] window;

		internal int window_size;

		internal short[] prev;

		internal short[] head;

		internal int ins_h;

		internal int hash_size;

		internal int hash_bits;

		internal int hash_mask;

		internal int hash_shift;

		internal int block_start;

		internal int match_length;

		internal int prev_match;

		internal int match_available;

		internal int strstart;

		internal int match_start;

		internal int lookahead;

		internal int prev_length;

		internal int max_chain_length;

		internal int max_lazy_match;

		internal int level;

		internal int strategy;

		internal int good_match;

		internal int nice_match;

		internal short[] dyn_ltree;

		internal short[] dyn_dtree;

		internal short[] bl_tree;

		internal Tree l_desc = new Tree();

		internal Tree d_desc = new Tree();

		internal Tree bl_desc = new Tree();

		internal short[] bl_count = new short[16];

		internal int[] heap = new int[2 * L_CODES + 1];

		internal int heap_len;

		internal int heap_max;

		internal byte[] depth = new byte[2 * L_CODES + 1];

		internal int l_buf;

		internal int lit_bufsize;

		internal int last_lit;

		internal int d_buf;

		internal int opt_len;

		internal int static_len;

		internal int matches;

		internal int last_eob_len;

		internal short bi_buf;

		internal int bi_valid;

		internal Deflate()
		{
			dyn_ltree = new short[HEAP_SIZE * 2];
			dyn_dtree = new short[122];
			bl_tree = new short[78];
		}

		internal void lm_init()
		{
			window_size = 2 * w_size;
			head[hash_size - 1] = 0;
			for (int i = 0; i < hash_size - 1; i++)
			{
				head[i] = 0;
			}
			max_lazy_match = config_table[level].max_lazy;
			good_match = config_table[level].good_length;
			nice_match = config_table[level].nice_length;
			max_chain_length = config_table[level].max_chain;
			strstart = 0;
			block_start = 0;
			lookahead = 0;
			match_length = (prev_length = 2);
			match_available = 0;
			ins_h = 0;
		}

		internal void tr_init()
		{
			l_desc.dyn_tree = dyn_ltree;
			l_desc.stat_desc = StaticTree.static_l_desc;
			d_desc.dyn_tree = dyn_dtree;
			d_desc.stat_desc = StaticTree.static_d_desc;
			bl_desc.dyn_tree = bl_tree;
			bl_desc.stat_desc = StaticTree.static_bl_desc;
			bi_buf = 0;
			bi_valid = 0;
			last_eob_len = 8;
			init_block();
		}

		internal void init_block()
		{
			for (int i = 0; i < L_CODES; i++)
			{
				dyn_ltree[i * 2] = 0;
			}
			for (int j = 0; j < 30; j++)
			{
				dyn_dtree[j * 2] = 0;
			}
			for (int k = 0; k < 19; k++)
			{
				bl_tree[k * 2] = 0;
			}
			dyn_ltree[512] = 1;
			opt_len = (static_len = 0);
			last_lit = (matches = 0);
		}

		internal void pqdownheap(short[] tree, int k)
		{
			int num = heap[k];
			for (int num2 = k << 1; num2 <= heap_len; num2 <<= 1)
			{
				if (num2 < heap_len && smaller(tree, heap[num2 + 1], heap[num2], depth))
				{
					num2++;
				}
				if (smaller(tree, num, heap[num2], depth))
				{
					break;
				}
				heap[k] = heap[num2];
				k = num2;
			}
			heap[k] = num;
		}

		internal static bool smaller(short[] tree, int n, int m, byte[] depth)
		{
			if (tree[n * 2] >= tree[m * 2])
			{
				if (tree[n * 2] == tree[m * 2])
				{
					return depth[n] <= depth[m];
				}
				return false;
			}
			return true;
		}

		internal void scan_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			tree[(max_code + 1) * 2 + 1] = (short)SupportClass.Identity(65535L);
			for (int i = 0; i <= max_code; i++)
			{
				int num6 = num2;
				num2 = tree[(i + 1) * 2 + 1];
				if (++num3 < num4 && num6 == num2)
				{
					continue;
				}
				if (num3 < num5)
				{
					bl_tree[num6 * 2] = (short)(bl_tree[num6 * 2] + num3);
				}
				else if (num6 != 0)
				{
					short[] array;
					if (num6 != num)
					{
						short[] array2 = array = bl_tree;
						int num7 = num6 * 2;
						IntPtr intPtr = (IntPtr)num7;
						array2[num7] = (short)(array[(long)intPtr] + 1);
					}
					(array = bl_tree)[32] = (short)(array[32] + 1);
				}
				else if (num3 <= 10)
				{
					short[] array;
					(array = bl_tree)[34] = (short)(array[34] + 1);
				}
				else
				{
					short[] array;
					(array = bl_tree)[36] = (short)(array[36] + 1);
				}
				num3 = 0;
				num = num6;
				if (num2 == 0)
				{
					num4 = 138;
					num5 = 3;
				}
				else if (num6 == num2)
				{
					num4 = 6;
					num5 = 3;
				}
				else
				{
					num4 = 7;
					num5 = 4;
				}
			}
		}

		internal int build_bl_tree()
		{
			scan_tree(dyn_ltree, l_desc.max_code);
			scan_tree(dyn_dtree, d_desc.max_code);
			bl_desc.build_tree(this);
			int num = 18;
			while (num >= 3 && bl_tree[Tree.bl_order[num] * 2 + 1] == 0)
			{
				num--;
			}
			opt_len += 3 * (num + 1) + 5 + 5 + 4;
			return num;
		}

		internal void send_all_trees(int lcodes, int dcodes, int blcodes)
		{
			send_bits(lcodes - 257, 5);
			send_bits(dcodes - 1, 5);
			send_bits(blcodes - 4, 4);
			for (int i = 0; i < blcodes; i++)
			{
				send_bits(bl_tree[Tree.bl_order[i] * 2 + 1], 3);
			}
			send_tree(dyn_ltree, lcodes - 1);
			send_tree(dyn_dtree, dcodes - 1);
		}

		internal void send_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			for (int i = 0; i <= max_code; i++)
			{
				int num6 = num2;
				num2 = tree[(i + 1) * 2 + 1];
				if (++num3 < num4 && num6 == num2)
				{
					continue;
				}
				if (num3 < num5)
				{
					do
					{
						send_code(num6, bl_tree);
					}
					while (--num3 != 0);
				}
				else if (num6 != 0)
				{
					if (num6 != num)
					{
						send_code(num6, bl_tree);
						num3--;
					}
					send_code(16, bl_tree);
					send_bits(num3 - 3, 2);
				}
				else if (num3 <= 10)
				{
					send_code(17, bl_tree);
					send_bits(num3 - 3, 3);
				}
				else
				{
					send_code(18, bl_tree);
					send_bits(num3 - 11, 7);
				}
				num3 = 0;
				num = num6;
				if (num2 == 0)
				{
					num4 = 138;
					num5 = 3;
				}
				else if (num6 == num2)
				{
					num4 = 6;
					num5 = 3;
				}
				else
				{
					num4 = 7;
					num5 = 4;
				}
			}
		}

		internal void put_byte(byte[] p, int start, int len)
		{
			Array.Copy(p, start, pending_buf, pending, len);
			pending += len;
		}

		internal void put_byte(byte c)
		{
			pending_buf[pending++] = c;
		}

		internal void put_short(int w)
		{
			put_byte((byte)w);
			put_byte((byte)SupportClass.URShift(w, 8));
		}

		internal void putShortMSB(int b)
		{
			put_byte((byte)(b >> 8));
			put_byte((byte)b);
		}

		internal void send_code(int c, short[] tree)
		{
			send_bits(tree[c * 2] & 0xFFFF, tree[c * 2 + 1] & 0xFFFF);
		}

		internal void send_bits(int value_Renamed, int length)
		{
			if (bi_valid > 16 - length)
			{
				bi_buf = (short)((ushort)bi_buf | (ushort)((value_Renamed << bi_valid) & 0xFFFF));
				put_short(bi_buf);
				bi_buf = (short)SupportClass.URShift(value_Renamed, 16 - bi_valid);
				bi_valid += length - 16;
			}
			else
			{
				bi_buf = (short)((ushort)bi_buf | (ushort)((value_Renamed << bi_valid) & 0xFFFF));
				bi_valid += length;
			}
		}

		internal void _tr_align()
		{
			send_bits(2, 3);
			send_code(256, StaticTree.static_ltree);
			bi_flush();
			if (1 + last_eob_len + 10 - bi_valid < 9)
			{
				send_bits(2, 3);
				send_code(256, StaticTree.static_ltree);
				bi_flush();
			}
			last_eob_len = 7;
		}

		internal bool _tr_tally(int dist, int lc)
		{
			pending_buf[d_buf + last_lit * 2] = (byte)SupportClass.URShift(dist, 8);
			pending_buf[d_buf + last_lit * 2 + 1] = (byte)dist;
			pending_buf[l_buf + last_lit] = (byte)lc;
			last_lit++;
			if (dist == 0)
			{
				short[] array;
				short[] array2 = array = dyn_ltree;
				int num = lc * 2;
				IntPtr intPtr = (IntPtr)num;
				array2[num] = (short)(array[(long)intPtr] + 1);
			}
			else
			{
				matches++;
				dist--;
				short[] array;
				short[] array3 = array = dyn_ltree;
				int num2 = (Tree._length_code[lc] + 256 + 1) * 2;
				IntPtr intPtr = (IntPtr)num2;
				array3[num2] = (short)(array[(long)intPtr] + 1);
				short[] array4 = array = dyn_dtree;
				int num3 = Tree.d_code(dist) * 2;
				intPtr = (IntPtr)num3;
				array4[num3] = (short)(array[(long)intPtr] + 1);
			}
			if ((last_lit & 0x1FFF) == 0 && level > 2)
			{
				int num4 = last_lit * 8;
				int num5 = strstart - block_start;
				for (int i = 0; i < 30; i++)
				{
					num4 = (int)(num4 + dyn_dtree[i * 2] * (5L + (long)Tree.extra_dbits[i]));
				}
				num4 = SupportClass.URShift(num4, 3);
				if (matches < last_lit / 2 && num4 < num5 / 2)
				{
					return true;
				}
			}
			return last_lit == lit_bufsize - 1;
		}

		internal void compress_block(short[] ltree, short[] dtree)
		{
			int num = 0;
			if (last_lit != 0)
			{
				do
				{
					int num2 = ((pending_buf[d_buf + num * 2] << 8) & 0xFF00) | (pending_buf[d_buf + num * 2 + 1] & 0xFF);
					int num3 = pending_buf[l_buf + num] & 0xFF;
					num++;
					if (num2 == 0)
					{
						send_code(num3, ltree);
						continue;
					}
					int num4 = Tree._length_code[num3];
					send_code(num4 + 256 + 1, ltree);
					int num5 = Tree.extra_lbits[num4];
					if (num5 != 0)
					{
						num3 -= Tree.base_length[num4];
						send_bits(num3, num5);
					}
					num2--;
					num4 = Tree.d_code(num2);
					send_code(num4, dtree);
					num5 = Tree.extra_dbits[num4];
					if (num5 != 0)
					{
						num2 -= Tree.base_dist[num4];
						send_bits(num2, num5);
					}
				}
				while (num < last_lit);
			}
			send_code(256, ltree);
			last_eob_len = ltree[513];
		}

		internal void set_data_type()
		{
			int i = 0;
			int num = 0;
			int num2 = 0;
			for (; i < 7; i++)
			{
				num2 += dyn_ltree[i * 2];
			}
			for (; i < 128; i++)
			{
				num += dyn_ltree[i * 2];
			}
			for (; i < 256; i++)
			{
				num2 += dyn_ltree[i * 2];
			}
			data_type = (byte)((num2 <= SupportClass.URShift(num, 2)) ? 1 : 0);
		}

		internal void bi_flush()
		{
			if (bi_valid == 16)
			{
				put_short(bi_buf);
				bi_buf = 0;
				bi_valid = 0;
			}
			else if (bi_valid >= 8)
			{
				put_byte((byte)bi_buf);
				bi_buf = (short)SupportClass.URShift(bi_buf, 8);
				bi_valid -= 8;
			}
		}

		internal void bi_windup()
		{
			if (bi_valid > 8)
			{
				put_short(bi_buf);
			}
			else if (bi_valid > 0)
			{
				put_byte((byte)bi_buf);
			}
			bi_buf = 0;
			bi_valid = 0;
		}

		internal void copy_block(int buf, int len, bool header)
		{
			bi_windup();
			last_eob_len = 8;
			if (header)
			{
				put_short((short)len);
				put_short((short)(~len));
			}
			put_byte(window, buf, len);
		}

		internal void flush_block_only(bool eof)
		{
			_tr_flush_block((block_start >= 0) ? block_start : (-1), strstart - block_start, eof);
			block_start = strstart;
			strm.flush_pending();
		}

		internal int deflate_stored(int flush)
		{
			int num = 65535;
			if (num > pending_buf_size - 5)
			{
				num = pending_buf_size - 5;
			}
			while (true)
			{
				if (lookahead <= 1)
				{
					fill_window();
					if (lookahead == 0 && flush == 0)
					{
						return 0;
					}
					if (lookahead == 0)
					{
						break;
					}
				}
				strstart += lookahead;
				lookahead = 0;
				int num2 = block_start + num;
				if (strstart == 0 || strstart >= num2)
				{
					lookahead = strstart - num2;
					strstart = num2;
					flush_block_only(eof: false);
					if (strm.avail_out == 0)
					{
						return 0;
					}
				}
				if (strstart - block_start >= w_size - MIN_LOOKAHEAD)
				{
					flush_block_only(eof: false);
					if (strm.avail_out == 0)
					{
						return 0;
					}
				}
			}
			flush_block_only(flush == 4);
			if (strm.avail_out == 0)
			{
				if (flush != 4)
				{
					return 0;
				}
				return 2;
			}
			if (flush != 4)
			{
				return 1;
			}
			return 3;
		}

		internal void _tr_stored_block(int buf, int stored_len, bool eof)
		{
			send_bits(eof ? 1 : 0, 3);
			copy_block(buf, stored_len, header: true);
		}

		internal void _tr_flush_block(int buf, int stored_len, bool eof)
		{
			int num = 0;
			int num2;
			int num3;
			if (level > 0)
			{
				if (data_type == 2)
				{
					set_data_type();
				}
				l_desc.build_tree(this);
				d_desc.build_tree(this);
				num = build_bl_tree();
				num2 = SupportClass.URShift(opt_len + 3 + 7, 3);
				num3 = SupportClass.URShift(static_len + 3 + 7, 3);
				if (num3 <= num2)
				{
					num2 = num3;
				}
			}
			else
			{
				num2 = (num3 = stored_len + 5);
			}
			if (stored_len + 4 <= num2 && buf != -1)
			{
				_tr_stored_block(buf, stored_len, eof);
			}
			else if (num3 == num2)
			{
				send_bits(2 + (eof ? 1 : 0), 3);
				compress_block(StaticTree.static_ltree, StaticTree.static_dtree);
			}
			else
			{
				send_bits(4 + (eof ? 1 : 0), 3);
				send_all_trees(l_desc.max_code + 1, d_desc.max_code + 1, num + 1);
				compress_block(dyn_ltree, dyn_dtree);
			}
			init_block();
			if (eof)
			{
				bi_windup();
			}
		}

		internal void fill_window()
		{
			do
			{
				int num = window_size - lookahead - strstart;
				int num2;
				if (num == 0 && strstart == 0 && lookahead == 0)
				{
					num = w_size;
				}
				else if (num == -1)
				{
					num--;
				}
				else if (strstart >= w_size + w_size - MIN_LOOKAHEAD)
				{
					Array.Copy(window, w_size, window, 0, w_size);
					match_start -= w_size;
					strstart -= w_size;
					block_start -= w_size;
					num2 = hash_size;
					int num3 = num2;
					do
					{
						int num4 = head[--num3] & 0xFFFF;
						head[num3] = (short)((num4 >= w_size) ? (num4 - w_size) : 0);
					}
					while (--num2 != 0);
					num2 = w_size;
					num3 = num2;
					do
					{
						int num4 = prev[--num3] & 0xFFFF;
						prev[num3] = (short)((num4 >= w_size) ? (num4 - w_size) : 0);
					}
					while (--num2 != 0);
					num += w_size;
				}
				if (strm.avail_in == 0)
				{
					break;
				}
				num2 = strm.read_buf(window, strstart + lookahead, num);
				lookahead += num2;
				if (lookahead >= 3)
				{
					ins_h = (window[strstart] & 0xFF);
					ins_h = (((ins_h << hash_shift) ^ (window[strstart + 1] & 0xFF)) & hash_mask);
				}
			}
			while (lookahead < MIN_LOOKAHEAD && strm.avail_in != 0);
		}

		internal int deflate_fast(int flush)
		{
			int num = 0;
			while (true)
			{
				if (lookahead < MIN_LOOKAHEAD)
				{
					fill_window();
					if (lookahead < MIN_LOOKAHEAD && flush == 0)
					{
						return 0;
					}
					if (lookahead == 0)
					{
						break;
					}
				}
				if (lookahead >= 3)
				{
					ins_h = (((ins_h << hash_shift) ^ (window[strstart + 2] & 0xFF)) & hash_mask);
					num = (head[ins_h] & 0xFFFF);
					prev[strstart & w_mask] = head[ins_h];
					head[ins_h] = (short)strstart;
				}
				if ((long)num != 0 && ((strstart - num) & 0xFFFF) <= w_size - MIN_LOOKAHEAD && strategy != 2)
				{
					match_length = longest_match(num);
				}
				bool flag;
				if (match_length >= 3)
				{
					flag = _tr_tally(strstart - match_start, match_length - 3);
					lookahead -= match_length;
					if (match_length <= max_lazy_match && lookahead >= 3)
					{
						match_length--;
						do
						{
							strstart++;
							ins_h = (((ins_h << hash_shift) ^ (window[strstart + 2] & 0xFF)) & hash_mask);
							num = (head[ins_h] & 0xFFFF);
							prev[strstart & w_mask] = head[ins_h];
							head[ins_h] = (short)strstart;
						}
						while (--match_length != 0);
						strstart++;
					}
					else
					{
						strstart += match_length;
						match_length = 0;
						ins_h = (window[strstart] & 0xFF);
						ins_h = (((ins_h << hash_shift) ^ (window[strstart + 1] & 0xFF)) & hash_mask);
					}
				}
				else
				{
					flag = _tr_tally(0, window[strstart] & 0xFF);
					lookahead--;
					strstart++;
				}
				if (flag)
				{
					flush_block_only(eof: false);
					if (strm.avail_out == 0)
					{
						return 0;
					}
				}
			}
			flush_block_only(flush == 4);
			if (strm.avail_out == 0)
			{
				if (flush == 4)
				{
					return 2;
				}
				return 0;
			}
			if (flush != 4)
			{
				return 1;
			}
			return 3;
		}

		internal int deflate_slow(int flush)
		{
			int num = 0;
			while (true)
			{
				if (lookahead < MIN_LOOKAHEAD)
				{
					fill_window();
					if (lookahead < MIN_LOOKAHEAD && flush == 0)
					{
						return 0;
					}
					if (lookahead == 0)
					{
						break;
					}
				}
				if (lookahead >= 3)
				{
					ins_h = (((ins_h << hash_shift) ^ (window[strstart + 2] & 0xFF)) & hash_mask);
					num = (head[ins_h] & 0xFFFF);
					prev[strstart & w_mask] = head[ins_h];
					head[ins_h] = (short)strstart;
				}
				prev_length = match_length;
				prev_match = match_start;
				match_length = 2;
				if (num != 0 && prev_length < max_lazy_match && ((strstart - num) & 0xFFFF) <= w_size - MIN_LOOKAHEAD)
				{
					if (strategy != 2)
					{
						match_length = longest_match(num);
					}
					if (match_length <= 5 && (strategy == 1 || (match_length == 3 && strstart - match_start > 4096)))
					{
						match_length = 2;
					}
				}
				if (prev_length >= 3 && match_length <= prev_length)
				{
					int num2 = strstart + lookahead - 3;
					bool flag = _tr_tally(strstart - 1 - prev_match, prev_length - 3);
					lookahead -= prev_length - 1;
					prev_length -= 2;
					do
					{
						if (++strstart <= num2)
						{
							ins_h = (((ins_h << hash_shift) ^ (window[strstart + 2] & 0xFF)) & hash_mask);
							num = (head[ins_h] & 0xFFFF);
							prev[strstart & w_mask] = head[ins_h];
							head[ins_h] = (short)strstart;
						}
					}
					while (--prev_length != 0);
					match_available = 0;
					match_length = 2;
					strstart++;
					if (flag)
					{
						flush_block_only(eof: false);
						if (strm.avail_out == 0)
						{
							return 0;
						}
					}
				}
				else if (match_available != 0)
				{
					if (_tr_tally(0, window[strstart - 1] & 0xFF))
					{
						flush_block_only(eof: false);
					}
					strstart++;
					lookahead--;
					if (strm.avail_out == 0)
					{
						return 0;
					}
				}
				else
				{
					match_available = 1;
					strstart++;
					lookahead--;
				}
			}
			if (match_available != 0)
			{
				bool flag = _tr_tally(0, window[strstart - 1] & 0xFF);
				match_available = 0;
			}
			flush_block_only(flush == 4);
			if (strm.avail_out == 0)
			{
				if (flush == 4)
				{
					return 2;
				}
				return 0;
			}
			if (flush != 4)
			{
				return 1;
			}
			return 3;
		}

		internal int longest_match(int cur_match)
		{
			int num = max_chain_length;
			int num2 = strstart;
			int num3 = prev_length;
			int num4 = (strstart > w_size - MIN_LOOKAHEAD) ? (strstart - (w_size - MIN_LOOKAHEAD)) : 0;
			int num5 = nice_match;
			int num6 = w_mask;
			int num7 = strstart + 258;
			byte b = window[num2 + num3 - 1];
			byte b2 = window[num2 + num3];
			if (prev_length >= good_match)
			{
				num >>= 2;
			}
			if (num5 > lookahead)
			{
				num5 = lookahead;
			}
			do
			{
				int num8 = cur_match;
				if (window[num8 + num3] != b2 || window[num8 + num3 - 1] != b || window[num8] != window[num2] || window[++num8] != window[num2 + 1])
				{
					continue;
				}
				num2 += 2;
				num8++;
				while (window[++num2] == window[++num8] && window[++num2] == window[++num8] && window[++num2] == window[++num8] && window[++num2] == window[++num8] && window[++num2] == window[++num8] && window[++num2] == window[++num8] && window[++num2] == window[++num8] && window[++num2] == window[++num8] && num2 < num7)
				{
				}
				int num9 = 258 - (num7 - num2);
				num2 = num7 - 258;
				if (num9 > num3)
				{
					match_start = cur_match;
					num3 = num9;
					if (num9 >= num5)
					{
						break;
					}
					b = window[num2 + num3 - 1];
					b2 = window[num2 + num3];
				}
			}
			while ((cur_match = (prev[cur_match & num6] & 0xFFFF)) > num4 && --num != 0);
			if (num3 <= lookahead)
			{
				return num3;
			}
			return lookahead;
		}

		internal int deflateInit(ZStream strm, int level, int bits)
		{
			return deflateInit2(strm, level, 8, bits, 8, 0);
		}

		internal int deflateInit(ZStream strm, int level)
		{
			return deflateInit(strm, level, 15);
		}

		internal int deflateInit2(ZStream strm, int level, int method, int windowBits, int memLevel, int strategy)
		{
			int num = 0;
			strm.msg = null;
			if (level == -1)
			{
				level = 6;
			}
			if (windowBits < 0)
			{
				num = 1;
				windowBits = -windowBits;
			}
			if (memLevel < 1 || memLevel > 9 || method != 8 || windowBits < 9 || windowBits > 15 || level < 0 || level > 9 || strategy < 0 || strategy > 2)
			{
				return -2;
			}
			strm.dstate = this;
			noheader = num;
			w_bits = windowBits;
			w_size = 1 << w_bits;
			w_mask = w_size - 1;
			hash_bits = memLevel + 7;
			hash_size = 1 << hash_bits;
			hash_mask = hash_size - 1;
			hash_shift = (hash_bits + 3 - 1) / 3;
			window = new byte[w_size * 2];
			prev = new short[w_size];
			head = new short[hash_size];
			lit_bufsize = 1 << memLevel + 6;
			pending_buf = new byte[lit_bufsize * 4];
			pending_buf_size = lit_bufsize * 4;
			d_buf = lit_bufsize;
			l_buf = 3 * lit_bufsize;
			this.level = level;
			this.strategy = strategy;
			this.method = (byte)method;
			return deflateReset(strm);
		}

		internal int deflateReset(ZStream strm)
		{
			strm.total_in = (strm.total_out = 0L);
			strm.msg = null;
			strm.data_type = 2;
			pending = 0;
			pending_out = 0;
			if (noheader < 0)
			{
				noheader = 0;
			}
			status = ((noheader != 0) ? 113 : 42);
			strm.adler = strm._adler.adler32(0L, null, 0, 0);
			last_flush = 0;
			tr_init();
			lm_init();
			return 0;
		}

		internal int deflateEnd()
		{
			if (status != 42 && status != 113 && status != 666)
			{
				return -2;
			}
			pending_buf = null;
			head = null;
			prev = null;
			window = null;
			if (status != 113)
			{
				return 0;
			}
			return -3;
		}

		internal int deflateParams(ZStream strm, int _level, int _strategy)
		{
			int result = 0;
			if (_level == -1)
			{
				_level = 6;
			}
			if (_level < 0 || _level > 9 || _strategy < 0 || _strategy > 2)
			{
				return -2;
			}
			if (config_table[level].func != config_table[_level].func && strm.total_in != 0)
			{
				result = strm.deflate(1);
			}
			if (level != _level)
			{
				level = _level;
				max_lazy_match = config_table[level].max_lazy;
				good_match = config_table[level].good_length;
				nice_match = config_table[level].nice_length;
				max_chain_length = config_table[level].max_chain;
			}
			strategy = _strategy;
			return result;
		}

		internal int deflateSetDictionary(ZStream strm, byte[] dictionary, int dictLength)
		{
			int num = dictLength;
			int sourceIndex = 0;
			if (dictionary == null || status != 42)
			{
				return -2;
			}
			strm.adler = strm._adler.adler32(strm.adler, dictionary, 0, dictLength);
			if (num < 3)
			{
				return 0;
			}
			if (num > w_size - MIN_LOOKAHEAD)
			{
				num = w_size - MIN_LOOKAHEAD;
				sourceIndex = dictLength - num;
			}
			Array.Copy(dictionary, sourceIndex, window, 0, num);
			strstart = num;
			block_start = num;
			ins_h = (window[0] & 0xFF);
			ins_h = (((ins_h << hash_shift) ^ (window[1] & 0xFF)) & hash_mask);
			for (int i = 0; i <= num - 3; i++)
			{
				ins_h = (((ins_h << hash_shift) ^ (window[i + 2] & 0xFF)) & hash_mask);
				prev[i & w_mask] = head[ins_h];
				head[ins_h] = (short)i;
			}
			return 0;
		}

		internal int deflate(ZStream strm, int flush)
		{
			if (flush > 4 || flush < 0)
			{
				return -2;
			}
			if (strm.next_out == null || (strm.next_in == null && strm.avail_in != 0) || (status == 666 && flush != 4))
			{
				strm.msg = z_errmsg[4];
				return -2;
			}
			if (strm.avail_out == 0)
			{
				strm.msg = z_errmsg[7];
				return -5;
			}
			this.strm = strm;
			int num = last_flush;
			last_flush = flush;
			if (status == 42)
			{
				int num2 = 8 + (w_bits - 8 << 4) << 8;
				int num3 = ((level - 1) & 0xFF) >> 1;
				if (num3 > 3)
				{
					num3 = 3;
				}
				num2 |= num3 << 6;
				if (strstart != 0)
				{
					num2 |= 0x20;
				}
				num2 += 31 - num2 % 31;
				status = 113;
				putShortMSB(num2);
				if (strstart != 0)
				{
					putShortMSB((int)SupportClass.URShift(strm.adler, 16));
					putShortMSB((int)(strm.adler & 0xFFFF));
				}
				strm.adler = strm._adler.adler32(0L, null, 0, 0);
			}
			if (pending != 0)
			{
				strm.flush_pending();
				if (strm.avail_out == 0)
				{
					last_flush = -1;
					return 0;
				}
			}
			else if (strm.avail_in == 0 && flush <= num && flush != 4)
			{
				strm.msg = z_errmsg[7];
				return -5;
			}
			if (status == 666 && strm.avail_in != 0)
			{
				strm.msg = z_errmsg[7];
				return -5;
			}
			if (strm.avail_in != 0 || lookahead != 0 || (flush != 0 && status != 666))
			{
				int num4 = -1;
				switch (config_table[level].func)
				{
				case 0:
					num4 = deflate_stored(flush);
					break;
				case 1:
					num4 = deflate_fast(flush);
					break;
				case 2:
					num4 = deflate_slow(flush);
					break;
				}
				if (num4 == 2 || num4 == 3)
				{
					status = 666;
				}
				switch (num4)
				{
				case 0:
				case 2:
					if (strm.avail_out == 0)
					{
						last_flush = -1;
					}
					return 0;
				case 1:
					if (flush == 1)
					{
						_tr_align();
					}
					else
					{
						_tr_stored_block(0, 0, eof: false);
						if (flush == 3)
						{
							for (int i = 0; i < hash_size; i++)
							{
								head[i] = 0;
							}
						}
					}
					strm.flush_pending();
					if (strm.avail_out == 0)
					{
						last_flush = -1;
						return 0;
					}
					break;
				}
			}
			if (flush != 4)
			{
				return 0;
			}
			if (noheader != 0)
			{
				return 1;
			}
			putShortMSB((int)SupportClass.URShift(strm.adler, 16));
			putShortMSB((int)(strm.adler & 0xFFFF));
			strm.flush_pending();
			noheader = -1;
			if (pending == 0)
			{
				return 1;
			}
			return 0;
		}

		static Deflate()
		{
			z_errmsg = new string[10]
			{
				"need dictionary",
				"stream end",
				"",
				"file error",
				"stream error",
				"data error",
				"insufficient memory",
				"buffer error",
				"incompatible version",
				""
			};
			MIN_LOOKAHEAD = 262;
			L_CODES = 286;
			HEAP_SIZE = 2 * L_CODES + 1;
			config_table = new Config[10];
			config_table[0] = new Config(0, 0, 0, 0, 0);
			config_table[1] = new Config(4, 4, 8, 4, 1);
			config_table[2] = new Config(4, 5, 16, 8, 1);
			config_table[3] = new Config(4, 6, 32, 32, 1);
			config_table[4] = new Config(4, 4, 16, 16, 2);
			config_table[5] = new Config(8, 16, 32, 32, 2);
			config_table[6] = new Config(8, 16, 128, 128, 2);
			config_table[7] = new Config(8, 32, 128, 256, 2);
			config_table[8] = new Config(32, 128, 258, 1024, 2);
			config_table[9] = new Config(32, 258, 258, 4096, 2);
		}
	}
}
