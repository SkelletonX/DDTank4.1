// Decompiled with JetBrains decompiler
// Type: Game.Logic.Cmd.MoveStartCommand
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
  [GameCommand(9, "开始移动")]
  public class MoveStartCommand : ICommandHandler
  {
    public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
    {
      if (!player.IsAttacking)
        return;
      packet.ReadBoolean();
      byte num1 = packet.ReadByte();
      int x = packet.ReadInt();
      int y = packet.ReadInt();
      byte dir = packet.ReadByte();
      packet.ReadBoolean();
      int num2 = (int) packet.ReadShort();
      if (num1 > (byte) 1)
        return;
      player.SetXY(x, y);
      player.StartMoving();
      game.SendPlayerMove(player, (int) num1, player.X, player.Y, dir, player.IsLiving, true);
    }
  }
}
