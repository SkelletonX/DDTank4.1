using System.IO;

namespace zlib
{
	public class ZInputStream : BinaryReader
	{
		protected internal ZStream z = new ZStream();

		protected internal int bufsize = 512;

		protected internal int flush;

		protected internal byte[] buf;

		protected internal byte[] buf1 = new byte[1];

		protected internal bool compress;

		private Stream in_Renamed = null;

		private bool nomoreinput = false;

		public virtual int FlushMode
		{
			get
			{
				return flush;
			}
			set
			{
				flush = value;
			}
		}

		public virtual long TotalIn => z.total_in;

		public virtual long TotalOut => z.total_out;

		private void InitBlock()
		{
			flush = 0;
			buf = new byte[bufsize];
		}

		public ZInputStream(Stream in_Renamed)
			: base(in_Renamed)
		{
			InitBlock();
			this.in_Renamed = in_Renamed;
			z.inflateInit();
			compress = false;
			z.next_in = buf;
			z.next_in_index = 0;
			z.avail_in = 0;
		}

		public ZInputStream(Stream in_Renamed, int level)
			: base(in_Renamed)
		{
			InitBlock();
			this.in_Renamed = in_Renamed;
			z.deflateInit(level);
			compress = true;
			z.next_in = buf;
			z.next_in_index = 0;
			z.avail_in = 0;
		}

		public override int Read()
		{
			if (read(buf1, 0, 1) == -1)
			{
				return -1;
			}
			return buf1[0] & 0xFF;
		}

		public int read(byte[] b, int off, int len)
		{
			if (len == 0)
			{
				return 0;
			}
			z.next_out = b;
			z.next_out_index = off;
			z.avail_out = len;
			int num;
			do
			{
				if (z.avail_in == 0 && !nomoreinput)
				{
					z.next_in_index = 0;
					z.avail_in = SupportClass.ReadInput(in_Renamed, buf, 0, bufsize);
					if (z.avail_in == -1)
					{
						z.avail_in = 0;
						nomoreinput = true;
					}
				}
				num = ((!compress) ? z.inflate(flush) : z.deflate(flush));
				if (nomoreinput && num == -5)
				{
					return -1;
				}
				if (num != 0 && num != 1)
				{
					throw new ZStreamException((compress ? "de" : "in") + "flating: " + z.msg);
				}
				if (nomoreinput && z.avail_out == len)
				{
					return -1;
				}
			}
			while (z.avail_out == len && num == 0);
			return len - z.avail_out;
		}

		public long skip(long n)
		{
			int num = 512;
			if (n < num)
			{
				num = (int)n;
			}
			byte[] array = new byte[num];
			return SupportClass.ReadInput(BaseStream, array, 0, array.Length);
		}

		public override void Close()
		{
			in_Renamed.Close();
		}
	}
}
