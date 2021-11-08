﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.HotSpringRooms.TankHandle.RenewalFeeCmd
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.HotSpringRooms.TankHandle
{
  [HotSpringCommandAttbute(3)]
  public class RenewalFeeCmd : IHotSpringCommandHandler
  {
    public bool HandleCommand(
      TankHotSpringLogicProcessor process,
      GamePlayer player,
      GSPacketIn packet)
    {
      if (player.CurrentHotSpringRoom != null)
        packet.ReadInt();
      return false;
    }
  }
}
