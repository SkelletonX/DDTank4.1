using log4net;
using System;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Game.Base
{
	public class PacketIn
	{
		protected byte[] m_buffer;

		protected byte[] m_buffer2;

		protected int m_length;

		protected int m_offset;

		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private byte lastbits;

		public volatile bool isSended = true;

		public volatile int m_sended;

		public volatile int packetNum;

		public volatile int m_loop;

		public byte[] Buffer => m_buffer;

		public int Length => m_length;

		public int Offset
		{
			get
			{
				return m_offset;
			}
			set
			{
				m_offset = value;
			}
		}

		public int DataLeft => m_length - m_offset;

		public PacketIn(byte[] buf, int len)
		{
			m_buffer = buf;
			m_length = len;
			m_offset = 0;
		}

		public void Skip(int num)
		{
			m_offset += num;
		}

		public virtual bool ReadBoolean()
		{
			return m_buffer[m_offset++] != 0;
		}

		public virtual byte ReadByte()
		{
			return m_buffer[m_offset++];
		}

		public virtual short ReadShort()
		{
			byte v = ReadByte();
			byte v2 = ReadByte();
			return Marshal.ConvertToInt16(v, v2);
		}

		public virtual short ReadShortLowEndian()
		{
			byte v = ReadByte();
			return Marshal.ConvertToInt16(ReadByte(), v);
		}

		public virtual int ReadInt()
		{
			byte v = ReadByte();
			byte v2 = ReadByte();
			byte v3 = ReadByte();
			byte v4 = ReadByte();
			return Marshal.ConvertToInt32(v, v2, v3, v4);
		}

		public virtual uint ReadUInt()
		{
			byte v = ReadByte();
			byte v2 = ReadByte();
			byte v3 = ReadByte();
			byte v4 = ReadByte();
			return Marshal.ConvertToUInt32(v, v2, v3, v4);
		}

		public virtual long ReadLong()
		{
			int num = ReadInt();
			long num2 = readUnsignedInt();
			int num3 = 1;
			if (num < 0)
			{
				num3 = -1;
			}
			return (long)((double)num3 * (Math.Abs((double)num * Math.Pow(2.0, 32.0)) + (double)num2));
		}

		public virtual long readUnsignedInt()
		{
			return ReadInt() & uint.MaxValue;
		}

		public virtual float ReadFloat()
		{
			byte[] array = new byte[4];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ReadByte();
			}
			return BitConverter.ToSingle(array, 0);
		}

		public virtual double ReadDouble()
		{
			byte[] array = new byte[8];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ReadByte();
			}
			return BitConverter.ToDouble(array, 0);
		}

		public virtual string ReadString()
		{
			short num = ReadShort();
			string @string = Encoding.UTF8.GetString(m_buffer, m_offset, num);
			m_offset += num;
			return @string.Replace("\0", "");
		}

		public virtual byte[] ReadBytes(int maxLen)
		{
			byte[] array = new byte[maxLen];
			Array.Copy(m_buffer, m_offset, array, 0, maxLen);
			m_offset += maxLen;
			return array;
		}

		public virtual byte[] ReadBytes()
		{
			return ReadBytes(m_length - m_offset);
		}

		public DateTime ReadDateTime()
		{
			return new DateTime(ReadShort(), ReadByte(), ReadByte(), ReadByte(), ReadByte(), ReadByte());
		}

		public virtual int CopyTo(byte[] dst, int dstOffset, int offset)
		{
			int num = (m_length - offset < dst.Length - dstOffset) ? (m_length - offset) : (dst.Length - dstOffset);
			if (num > 0)
			{
				System.Buffer.BlockCopy(m_buffer, offset, dst, dstOffset, num);
			}
			return num;
		}

		public virtual int CopyTo(byte[] dst, int dstOffset, int offset, byte[] key)
		{
			lock (this)
			{
				int num = (m_length - offset < dst.Length - dstOffset) ? (m_length - offset) : (dst.Length - dstOffset);
				if (num > 0)
				{
					for (int i = 0; i < num; i++)
					{
						if (offset + i == 0)
						{
							dst[dstOffset] = (byte)(m_buffer[offset + i] ^ key[(offset + i) % 8]);
						}
						else if (i == 0 && dstOffset == 0)
						{
							key[(offset + i) % 8] = (byte)((key[(offset + i) % 8] + lastbits) ^ (offset + i));
							dst[dstOffset + i] = (byte)((m_buffer[offset + i] ^ key[(offset + i) % 8]) + lastbits);
						}
						else
						{
							key[(offset + i) % 8] = (byte)((key[(offset + i) % 8] + dst[dstOffset + i - 1]) ^ (offset + i));
							dst[dstOffset + i] = (byte)((m_buffer[offset + i] ^ key[(offset + i) % 8]) + dst[dstOffset + i - 1]);
							if (i == num - 1)
							{
								lastbits = dst[dstOffset + i];
							}
						}
					}
				}
				return num;
			}
		}

		public virtual int CopyTo3(byte[] dst, int dstOffset, int offset, byte[] key, ref int packetArrangeSend)
		{
			lock (this)
			{
				int num = (m_length - offset < dst.Length - dstOffset) ? (m_length - offset) : (dst.Length - dstOffset);
				if (num > 0)
				{
					int num2;
					if (isSended)
					{
						packetNum = Interlocked.Increment(ref packetArrangeSend);
						m_sended = 0;
						isSended = false;
						num2 = m_sended + dstOffset;
					}
					else
					{
						num2 = 8192;
					}
					if (packetNum != packetArrangeSend)
					{
						return 0;
					}
					for (int i = 0; i < num; i++)
					{
						int num3 = offset + i;
						while (num2 > 8192)
						{
							num2 -= 8192;
						}
						if (m_sended == 0)
						{
							dst[dstOffset] = (byte)(m_buffer[num3] ^ key[m_sended]);
						}
						else
						{
							if (num2 == 0)
							{
								log.Error("indexBuffex == 0: " + num3 + " - " + dstOffset);
								return 0;
							}
							key[m_sended % 8] = (byte)((key[m_sended % 8] + dst[num2 - 1]) ^ m_sended);
							dst[dstOffset + i] = (byte)((m_buffer[num3] ^ key[m_sended % 8]) + dst[num2 - 1]);
						}
						m_sended++;
						num2++;
					}
				}
				return num;
			}
		}

		public virtual int CopyFrom(byte[] src, int srcOffset, int offset, int count)
		{
			if (count < m_buffer.Length && count - srcOffset < src.Length)
			{
				System.Buffer.BlockCopy(src, srcOffset, m_buffer, offset, count);
				return count;
			}
			return -1;
		}

		public virtual int CopyFrom(byte[] src, int srcOffset, int offset, int count, int key)
		{
			if (count < m_buffer.Length && count - srcOffset < src.Length)
			{
				key = (key & 0xFF0000) >> 16;
				for (int i = 0; i < count; i++)
				{
					m_buffer[offset + i] = (byte)(src[srcOffset + i] ^ key);
				}
				return count;
			}
			return -1;
		}

		public virtual int[] CopyFrom3(byte[] src, int srcOffset, int offset, int count, byte[] key)
		{
			int[] result = new int[count];
			for (int i = 0; i < count; i++)
			{
				m_buffer[i] = src[i];
			}
			if (count < m_buffer.Length && count - srcOffset < src.Length)
			{
				m_buffer[0] = (byte)(src[srcOffset] ^ key[0]);
				for (int j = 1; j < count; j++)
				{
					key[j % 8] = (byte)((key[j % 8] + src[srcOffset + j - 1]) ^ j);
					m_buffer[j] = (byte)((src[srcOffset + j] - src[srcOffset + j - 1]) ^ key[j % 8]);
				}
			}
			return result;
		}

		public virtual void WriteBoolean(bool val)
		{
			if (m_offset == m_buffer.Length)
			{
				byte[] buffer = m_buffer;
				m_buffer = new byte[m_buffer.Length * 2];
				Array.Copy(buffer, m_buffer, buffer.Length);
			}
			m_buffer[m_offset++] = (byte)(val ? 1 : 0);
			m_length = ((m_offset > m_length) ? m_offset : m_length);
		}

		public virtual void WriteByte(byte val)
		{
			if (m_offset == m_buffer.Length)
			{
				byte[] buffer = m_buffer;
				m_buffer = new byte[m_buffer.Length * 2];
				Array.Copy(buffer, m_buffer, buffer.Length);
			}
			m_buffer[m_offset++] = val;
			m_length = ((m_offset > m_length) ? m_offset : m_length);
		}

		public virtual void Write(byte[] src)
		{
			Write(src, 0, src.Length);
		}

		public virtual void Write3(byte[] src, int offset, int len)
		{
			Array.Copy(src, offset, m_buffer, m_offset, len);
			m_offset += len;
			m_length = ((m_offset > m_length) ? m_offset : m_length);
		}

		public virtual void Write(byte[] src, int offset, int len)
		{
			if (m_offset + len >= m_buffer.Length)
			{
				byte[] buffer = m_buffer;
				m_buffer = new byte[m_buffer.Length * 2];
				Array.Copy(buffer, m_buffer, buffer.Length);
				Write(src, offset, len);
			}
			else
			{
				Array.Copy(src, offset, m_buffer, m_offset, len);
				m_offset += len;
				m_length = ((m_offset > m_length) ? m_offset : m_length);
			}
		}

		public virtual void WriteShort(short val)
		{
			WriteByte((byte)(val >> 8));
			WriteByte((byte)(val & 0xFF));
		}

		public virtual void WriteShortLowEndian(short val)
		{
			WriteByte((byte)(val & 0xFF));
			WriteByte((byte)(val >> 8));
		}

		public virtual void WriteInt(int val)
		{
			WriteByte((byte)(val >> 24));
			WriteByte((byte)((val >> 16) & 0xFF));
			WriteByte((byte)((val & 0xFFFF) >> 8));
			WriteByte((byte)(val & 0xFFFF & 0xFF));
		}

		public virtual void WriteUInt(uint val)
		{
			WriteByte((byte)(val >> 24));
			WriteByte((byte)((val >> 16) & 0xFF));
			WriteByte((byte)((val & 0xFFFF) >> 8));
			WriteByte((byte)(val & 0xFFFF & 0xFF));
		}

		public virtual void WriteLong(long val)
		{
			int val2 = (int)val;
			string text = Convert.ToString(val, 2);
			string text2 = (text.Length <= 32) ? "" : text.Substring(0, text.Length - 32);
			int num = 0;
			for (int i = 0; i < text2.Length; i++)
			{
				string a = text2.Substring(text2.Length - (i + 1));
				if (!(a == "0"))
				{
					if (!(a == "1"))
					{
						break;
					}
					num += 1 << i;
				}
			}
			WriteInt(num);
			WriteInt(val2);
		}

		public virtual void WriteFloat(float val)
		{
			byte[] bytes = BitConverter.GetBytes(val);
			Write(bytes);
		}

		public virtual void WriteDouble(double val)
		{
			byte[] bytes = BitConverter.GetBytes(val);
			Write(bytes);
		}

		public virtual void Fill(byte val, int num)
		{
			for (int i = 0; i < num; i++)
			{
				WriteByte(val);
			}
		}

		public virtual void WriteString(string str)
		{
			if (!string.IsNullOrEmpty(str))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(str);
				WriteShort((short)(bytes.Length + 1));
				Write(bytes, 0, bytes.Length);
				WriteByte(0);
			}
			else
			{
				WriteShort(1);
				WriteByte(0);
			}
		}

		public virtual void WriteString(string str, int maxlen)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			int num = (bytes.Length < maxlen) ? bytes.Length : maxlen;
			WriteShort((short)num);
			Write(bytes, 0, num);
		}

		public void WriteDateTime(DateTime date)
		{
			WriteShort((short)date.Year);
			WriteByte((byte)date.Month);
			WriteByte((byte)date.Day);
			WriteByte((byte)date.Hour);
			WriteByte((byte)date.Minute);
			WriteByte((byte)date.Second);
		}
	}
}
