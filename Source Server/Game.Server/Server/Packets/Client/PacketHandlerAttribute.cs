// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.PacketHandlerAttribute
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System;

namespace Game.Server.Packets.Client
{
  [AttributeUsage(AttributeTargets.Class)]
  public class PacketHandlerAttribute : Attribute
  {
    protected int m_code;
    protected string m_desc;

    public PacketHandlerAttribute(int code, string desc)
    {
      this.m_code = code;
      this.m_desc = desc;
    }

    public int Code
    {
      get
      {
        return this.m_code;
      }
    }

    public string Description
    {
      get
      {
        return this.m_desc;
      }
    }
  }
}
