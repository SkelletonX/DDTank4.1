// Decompiled with JetBrains decompiler
// Type: Game.Logic.Cmd.FireCommand
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
  [GameCommand(2, "用户开炮")]
  public class FireCommand : ICommandHandler
  {
    public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
    {
      if (!player.IsAttacking)
        return;
      int x = packet.ReadInt();
      int y = packet.ReadInt();
      if (!player.CheckShootPoint(x, y))
        return;
      int force = packet.ReadInt();
      int angle = packet.ReadInt();
      player.Shoot(x, y, force, angle);
    }
  }
}
