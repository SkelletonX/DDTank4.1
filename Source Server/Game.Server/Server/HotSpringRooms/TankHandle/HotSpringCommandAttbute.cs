// Decompiled with JetBrains decompiler
// Type: Game.Server.HotSpringRooms.TankHandle.HotSpringCommandAttbute
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System;

namespace Game.Server.HotSpringRooms.TankHandle
{
  public class HotSpringCommandAttbute : Attribute
  {
    public HotSpringCommandAttbute(byte code)
    {
      this.Code = code;
    }

    public byte Code { get; private set; }
  }
}
