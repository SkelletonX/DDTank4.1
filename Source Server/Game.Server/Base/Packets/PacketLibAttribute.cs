// Decompiled with JetBrains decompiler
// Type: Game.Base.Packets.PacketLibAttribute
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System;

namespace Game.Base.Packets
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
  public class PacketLibAttribute : Attribute
  {
    private int m_rawVersion;

    public PacketLibAttribute(int rawVersion)
    {
      this.m_rawVersion = rawVersion;
    }

    public int RawVersion
    {
      get
      {
        return this.m_rawVersion;
      }
    }
  }
}
