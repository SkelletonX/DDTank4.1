// Decompiled with JetBrains decompiler
// Type: Game.Logic.Cmd.FireTagCommand
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
  [GameCommand(96, "准备开炮")]
  public class FireTagCommand : ICommandHandler
  {
    public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
    {
      if (!player.IsAttacking)
        return;
      packet.Parameter1 = player.Id;
      game.SendToAll(packet);
      game.SendSyncLifeTime();
      if (!packet.ReadBoolean())
        return;
      byte speedTime = packet.ReadByte();
      player.PrepareShoot(speedTime);
    }
  }
}
