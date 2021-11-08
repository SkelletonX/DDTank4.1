// Decompiled with JetBrains decompiler
// Type: Game.Base.Packets.GSPacketIn
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using log4net;
using System.Reflection;

namespace Game.Base.Packets
{
  public class GSPacketIn : PacketIn
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    public const ushort HDR_SIZE = 20;
    public const short HEADER = 29099;
    protected int m_cliendId;
    protected short m_code;
    protected int m_parameter1;
    protected int m_parameter2;

    public GSPacketIn(short code)
      : this(code, 0, 8192)
    {
    }

    public GSPacketIn(byte[] buf, int size)
      : base(buf, size)
    {
    }

    public GSPacketIn(short code, int clientId)
      : this(code, clientId, 8192)
    {
    }

    public GSPacketIn(short code, int clientId, int size)
      : base(new byte[size], 20)
    {
      this.m_code = code;
      this.m_cliendId = clientId;
      this.m_offset = 20;
    }

    public void ClearContext()
    {
      this.m_offset = 20;
      this.m_length = 20;
    }

    public void ClearOffset()
    {
      this.m_offset = 20;
    }

    public GSPacketIn Clone()
    {
      GSPacketIn gsPacketIn = new GSPacketIn(this.m_buffer, this.m_length);
      gsPacketIn.ReadHeader();
      gsPacketIn.Offset = this.m_length;
      return gsPacketIn;
    }

    public void Compress()
    {
      byte[] src = Marshal.Compress(this.m_buffer, 20, this.Length - 20);
      this.m_offset = 20;
      this.Write(src);
      this.m_length = src.Length + 20;
    }

    public short checkSum()
    {
      short num1 = 119;
      int num2 = 6;
      while (num2 < this.m_length)
      {
        try
        {
          num1 += (short) this.m_buffer[num2++];
        }
        catch
        {
        }
      }
      return (short) ((int) num1 & 32639);
    }

    public void ReadHeader()
    {
      int num1 = (int) this.ReadShort();
      this.m_length = (int) this.ReadShort();
      int num2 = (int) this.ReadShort();
      this.m_code = this.ReadShort();
      this.m_cliendId = this.ReadInt();
      this.m_parameter1 = this.ReadInt();
      this.m_parameter2 = this.ReadInt();
    }

    public GSPacketIn ReadPacket()
    {
      byte[] buf = this.ReadBytes();
      GSPacketIn gsPacketIn = new GSPacketIn(buf, buf.Length);
      gsPacketIn.ReadHeader();
      return gsPacketIn;
    }

    public void UnCompress()
    {
    }

    public void WriteHeader()
    {
      lock (this)
      {
        int offset = this.m_offset;
        this.m_offset = 0;
        this.WriteShort((short) 29099);
        this.WriteShort((short) this.m_length);
        this.WriteShort(this.checkSum());
        this.WriteShort(this.m_code);
        this.WriteInt(this.m_cliendId);
        this.WriteInt(this.m_parameter1);
        this.WriteInt(this.m_parameter2);
        this.m_offset = offset;
      }
      lock (this)
      {
        int offset = this.m_offset;
        this.m_offset = 0;
        this.WriteShort((short) 29099);
        this.WriteShort((short) this.m_length);
        this.WriteShort(this.checkSum());
        this.WriteShort(this.m_code);
        this.WriteInt(this.m_cliendId);
        this.WriteInt(this.m_parameter1);
        this.WriteInt(this.m_parameter2);
        this.m_offset = offset;
      }
    }

    public void WriteHeader3()
    {
      lock (this)
      {
        int offset = this.m_offset;
        this.m_offset = 0;
        this.WriteShort((short) 29099);
        this.WriteShort((short) this.m_length);
        this.m_offset = 6;
        this.WriteShort(this.m_code);
        this.WriteInt(this.m_cliendId);
        this.WriteInt(this.m_parameter1);
        this.WriteInt(this.m_parameter2);
        this.m_offset = 4;
        this.WriteShort(this.checkSum());
        this.m_offset = offset;
      }
    }

    public void WritePacket(GSPacketIn pkg)
    {
      pkg.WriteHeader();
      this.Write(pkg.Buffer, 0, pkg.Length);
    }

    public int ClientID
    {
      get
      {
        return this.m_cliendId;
      }
      set
      {
        this.m_cliendId = value;
      }
    }

    public short Code
    {
      get
      {
        return this.m_code;
      }
      set
      {
        this.m_code = value;
      }
    }

    public int Parameter1
    {
      get
      {
        return this.m_parameter1;
      }
      set
      {
        this.m_parameter1 = value;
      }
    }

    public int Parameter2
    {
      get
      {
        return this.m_parameter2;
      }
      set
      {
        this.m_parameter2 = value;
      }
    }
  }
}
