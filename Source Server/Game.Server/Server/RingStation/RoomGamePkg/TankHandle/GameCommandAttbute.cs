// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.RoomGamePkg.TankHandle.GameCommandAttbute
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System;

namespace Game.Server.RingStation.RoomGamePkg.TankHandle
{
  public class GameCommandAttbute : Attribute
  {
    public GameCommandAttbute(byte code)
    {
      this.Code = code;
    }

    public byte Code { get; private set; }
  }
}
