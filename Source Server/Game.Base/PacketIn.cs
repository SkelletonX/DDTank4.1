// Decompiled with JetBrains decompiler
// Type: Game.Base.PacketIn
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using System;
using System.Text;
using System.Threading;

namespace Game.Base
{
  public class PacketIn
  {
    public volatile bool isSended = true;
    private byte lastbits;
    protected byte[] m_buffer;
    protected byte[] m_buffer2;
    protected int m_length;
    public volatile int m_loop;
    protected int m_offset;
    public volatile int m_sended;
    public volatile int packetNum;

    public PacketIn(byte[] buf, int len)
    {
      this.m_buffer = buf;
      this.m_length = len;
      this.m_offset = 0;
    }

    public virtual int CopyFrom(byte[] src, int srcOffset, int offset, int count)
    {
      if (count >= this.m_buffer.Length || count - srcOffset >= src.Length)
        return -1;
      System.Buffer.BlockCopy((Array) src, srcOffset, (Array) this.m_buffer, offset, count);
      return count;
    }

    public virtual int CopyFrom(byte[] src, int srcOffset, int offset, int count, int key)
    {
      if (count >= this.m_buffer.Length || count - srcOffset >= src.Length)
        return -1;
      key = (key & 16711680) >> 16;
      for (int index = 0; index < count; ++index)
        this.m_buffer[offset + index] = (byte) ((uint) src[srcOffset + index] ^ (uint) key);
      return count;
    }

    public virtual int[] CopyFrom3(byte[] src, int srcOffset, int offset, int count, byte[] key)
    {
      int[] numArray = new int[count];
      for (int index = 0; index < count; ++index)
        this.m_buffer[index] = src[index];
      if (count < this.m_buffer.Length && count - srcOffset < src.Length)
      {
        this.m_buffer[0] = (byte) ((uint) src[srcOffset] ^ (uint) key[0]);
        for (int index = 1; index < count; ++index)
        {
          key[index % 8] = (byte) ((int) key[index % 8] + (int) src[srcOffset + index - 1] ^ index);
          this.m_buffer[index] = (byte) ((uint) src[srcOffset + index] - (uint) src[srcOffset + index - 1] ^ (uint) key[index % 8]);
        }
      }
      return numArray;
    }

    public virtual int CopyTo(byte[] dst, int dstOffset, int offset)
    {
      int count = this.m_length - offset < dst.Length - dstOffset ? this.m_length - offset : dst.Length - dstOffset;
      if (count > 0)
        System.Buffer.BlockCopy((Array) this.m_buffer, offset, (Array) dst, dstOffset, count);
      return count;
    }

    public virtual int CopyTo(byte[] dst, int dstOffset, int offset, byte[] key)
    {
      int num = this.m_length - offset < dst.Length - dstOffset ? this.m_length - offset : dst.Length - dstOffset;
      if (num > 0)
      {
        for (int index = 0; index < num; ++index)
        {
          if (offset + index == 0)
            dst[dstOffset] = (byte) ((uint) this.m_buffer[offset + index] ^ (uint) key[(offset + index) % 8]);
          else if (index == 0 && dstOffset == 0)
          {
            key[(offset + index) % 8] = (byte) ((int) key[(offset + index) % 8] + (int) this.lastbits ^ offset + index);
            dst[dstOffset + index] = (byte) (((uint) this.m_buffer[offset + index] ^ (uint) key[(offset + index) % 8]) + (uint) this.lastbits);
          }
          else
          {
            key[(offset + index) % 8] = (byte) ((int) key[(offset + index) % 8] + (int) dst[dstOffset + index - 1] ^ offset + index);
            dst[dstOffset + index] = (byte) (((uint) this.m_buffer[offset + index] ^ (uint) key[(offset + index) % 8]) + (uint) dst[dstOffset + index - 1]);
            if (index == num - 1)
              this.lastbits = dst[dstOffset + index];
          }
        }
      }
      return num;
    }

    public virtual int CopyTo3(
      byte[] dst,
      int dstOffset,
      int offset,
      byte[] key,
      ref int packetArrangeSend)
    {
      int num1 = this.m_length - offset < dst.Length - dstOffset ? this.m_length - offset : dst.Length - dstOffset;
      lock (this)
      {
        if (num1 > 0)
        {
          int num2;
          if (this.isSended)
          {
            this.packetNum = Interlocked.Increment(ref packetArrangeSend);
            packetArrangeSend = this.packetNum;
            this.m_sended = 0;
            this.isSended = false;
            num2 = this.m_sended + dstOffset;
          }
          else
            num2 = 8192;
          if (this.packetNum != packetArrangeSend)
            return 0;
          for (int index1 = 0; index1 < num1; ++index1)
          {
            int index2 = offset + index1;
            while (num2 > 8192)
              num2 -= 8192;
            if (this.m_sended == 0)
            {
              dst[dstOffset] = (byte) ((uint) this.m_buffer[index2] ^ (uint) key[this.m_sended % 8]);
            }
            else
            {
              if (num2 == 0)
                return 0;
              key[this.m_sended % 8] = (byte) ((int) key[this.m_sended % 8] + (int) dst[num2 - 1] ^ this.m_sended);
              dst[dstOffset + index1] = (byte) (((uint) this.m_buffer[index2] ^ (uint) key[this.m_sended % 8]) + (uint) dst[num2 - 1]);
            }
            ++this.m_sended;
            ++num2;
          }
        }
        return num1;
      }
    }

    public virtual void Fill(byte val, int num)
    {
      for (int index = 0; index < num; ++index)
        this.WriteByte(val);
    }

    public virtual bool ReadBoolean()
    {
      return this.m_buffer[this.m_offset++] > (byte) 0;
    }

    public virtual byte ReadByte()
    {
      return this.m_buffer[this.m_offset++];
    }

    public virtual byte[] ReadBytes()
    {
      return this.ReadBytes(this.m_length - this.m_offset);
    }

    public virtual byte[] ReadBytes(int maxLen)
    {
      byte[] numArray = new byte[maxLen];
      Array.Copy((Array) this.m_buffer, this.m_offset, (Array) numArray, 0, maxLen);
      this.m_offset += maxLen;
      return numArray;
    }

    public DateTime ReadDateTime()
    {
      return new DateTime((int) this.ReadShort(), (int) this.ReadByte(), (int) this.ReadByte(), (int) this.ReadByte(), (int) this.ReadByte(), (int) this.ReadByte());
    }

    public virtual double ReadDouble()
    {
      byte[] numArray = new byte[8];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = this.ReadByte();
      return BitConverter.ToDouble(numArray, 0);
    }

    public virtual float ReadFloat()
    {
      byte[] numArray = new byte[4];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = this.ReadByte();
      return BitConverter.ToSingle(numArray, 0);
    }

    public virtual int ReadInt()
    {
      int num1 = (int) this.ReadByte();
      byte num2 = this.ReadByte();
      byte num3 = this.ReadByte();
      byte num4 = this.ReadByte();
      int num5 = (int) num2;
      int num6 = (int) num3;
      int num7 = (int) num4;
      return Marshal.ConvertToInt32((byte) num1, (byte) num5, (byte) num6, (byte) num7);
    }

    public virtual long ReadLong()
    {
      int num1 = this.ReadInt();
      long num2 = this.readUnsignedInt();
      int num3 = 1;
      if (num1 < 0)
        num3 = -1;
      return (long) ((double) num3 * (Math.Abs((double) num1 * Math.Pow(2.0, 32.0)) + (double) num2));
    }

    public virtual short ReadShort()
    {
      return Marshal.ConvertToInt16(this.ReadByte(), this.ReadByte());
    }

    public virtual short ReadShortLowEndian()
    {
      return Marshal.ConvertToInt16(this.ReadByte(), this.ReadByte());
    }

    public virtual string ReadString()
    {
      short num = this.ReadShort();
      string str = Encoding.UTF8.GetString(this.m_buffer, this.m_offset, (int) num);
      this.m_offset += (int) num;
      return str.Replace("\0", "");
    }

    public virtual uint ReadUInt()
    {
      int num1 = (int) this.ReadByte();
      byte num2 = this.ReadByte();
      byte num3 = this.ReadByte();
      byte num4 = this.ReadByte();
      int num5 = (int) num2;
      int num6 = (int) num3;
      int num7 = (int) num4;
      return Marshal.ConvertToUInt32((byte) num1, (byte) num5, (byte) num6, (byte) num7);
    }

    public virtual long readUnsignedInt()
    {
      return (long) this.ReadInt() & (long) uint.MaxValue;
    }

    public void Skip(int num)
    {
      this.m_offset += num;
    }

    public virtual void vmethod_0(uint val)
    {
      this.WriteByte((byte) (val >> 24));
      this.WriteByte((byte) (val >> 16 & (uint) byte.MaxValue));
      this.WriteByte((byte) ((val & (uint) ushort.MaxValue) >> 8));
      this.WriteByte((byte) ((int) val & (int) ushort.MaxValue & (int) byte.MaxValue));
    }

    public virtual void Write(byte[] src)
    {
      this.Write(src, 0, src.Length);
    }

    public virtual void Write(byte[] src, int offset, int len)
    {
      if (this.m_offset + len >= this.m_buffer.Length)
      {
        byte[] buffer = this.m_buffer;
        this.m_buffer = new byte[this.m_buffer.Length * 2];
        Array.Copy((Array) buffer, (Array) this.m_buffer, buffer.Length);
        this.Write(src, offset, len);
      }
      else
      {
        Array.Copy((Array) src, offset, (Array) this.m_buffer, this.m_offset, len);
        this.m_offset += len;
        this.m_length = this.m_offset > this.m_length ? this.m_offset : this.m_length;
      }
    }

    public virtual void Write3(byte[] src, int offset, int len)
    {
      Array.Copy((Array) src, offset, (Array) this.m_buffer, this.m_offset, len);
      this.m_offset += len;
      this.m_length = this.m_offset > this.m_length ? this.m_offset : this.m_length;
    }

    public virtual void WriteBoolean(bool val)
    {
      if (this.m_offset == this.m_buffer.Length)
      {
        byte[] buffer = this.m_buffer;
        this.m_buffer = new byte[this.m_buffer.Length * 2];
        Array.Copy((Array) buffer, (Array) this.m_buffer, buffer.Length);
      }
      this.m_buffer[this.m_offset++] = val ? (byte) 1 : (byte) 0;
      this.m_length = this.m_offset > this.m_length ? this.m_offset : this.m_length;
    }

    public virtual void WriteByte(byte val)
    {
      if (this.m_offset == this.m_buffer.Length)
      {
        byte[] buffer = this.m_buffer;
        this.m_buffer = new byte[this.m_buffer.Length * 2];
        Array.Copy((Array) buffer, (Array) this.m_buffer, buffer.Length);
      }
      this.m_buffer[this.m_offset++] = val;
      this.m_length = this.m_offset > this.m_length ? this.m_offset : this.m_length;
    }

    public void WriteDateTime(DateTime date)
    {
      this.WriteShort((short) date.Year);
      this.WriteByte((byte) date.Month);
      this.WriteByte((byte) date.Day);
      this.WriteByte((byte) date.Hour);
      this.WriteByte((byte) date.Minute);
      this.WriteByte((byte) date.Second);
    }

    public virtual void WriteDouble(double val)
    {
      this.Write(BitConverter.GetBytes(val));
    }

    public virtual void WriteFloat(float val)
    {
      this.Write(BitConverter.GetBytes(val));
    }

    public virtual void WriteInt(int val)
    {
      this.WriteByte((byte) (val >> 24));
      this.WriteByte((byte) (val >> 16 & (int) byte.MaxValue));
      this.WriteByte((byte) ((val & (int) ushort.MaxValue) >> 8));
      this.WriteByte((byte) (val & (int) ushort.MaxValue & (int) byte.MaxValue));
    }

    public virtual void WriteLong(long val)
    {
      int val1;
      string str1 = Convert.ToString((long) (val1 = (int) val), 2);
      string str2 = str1.Length <= 32 ? "" : str1.Substring(0, str1.Length - 32);
      int val2 = 0;
      for (int index = 0; index < str2.Length; ++index)
      {
        string str3 = str2.Substring(str2.Length - (index + 1));
        if (str3 != "0")
        {
          if (str3 == "1")
            val2 += 1 << index;
          else
            break;
        }
      }
      this.WriteInt(val2);
      this.WriteInt(val1);
    }

    public virtual void WriteShort(short val)
    {
      this.WriteByte((byte) ((uint) val >> 8));
      this.WriteByte((byte) ((uint) val & (uint) byte.MaxValue));
    }

    public virtual void WriteShortLowEndian(short val)
    {
      this.WriteByte((byte) ((uint) val & (uint) byte.MaxValue));
      this.WriteByte((byte) ((uint) val >> 8));
    }

    public virtual void WriteString(string str)
    {
      if (!string.IsNullOrEmpty(str))
      {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        this.WriteShort((short) (bytes.Length + 1));
        this.Write(bytes, 0, bytes.Length);
        this.WriteByte((byte) 0);
      }
      else
      {
        this.WriteShort((short) 1);
        this.WriteByte((byte) 0);
      }
    }

    public virtual void WriteString(string str, int maxlen)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(str);
      int len = bytes.Length < maxlen ? bytes.Length : maxlen;
      this.WriteShort((short) len);
      this.Write(bytes, 0, len);
    }

    public byte[] Buffer
    {
      get
      {
        return this.m_buffer;
      }
    }

    public int DataLeft
    {
      get
      {
        return this.m_length - this.m_offset;
      }
    }

    public int Length
    {
      get
      {
        return this.m_length;
      }
    }

    public int Offset
    {
      get
      {
        return this.m_offset;
      }
      set
      {
        this.m_offset = value;
      }
    }
  }
}
