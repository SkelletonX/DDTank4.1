// Decompiled with JetBrains decompiler
// Type: Game.Logic.Cmd.MissionPrepareCommand
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
  [GameCommand(116, "关卡准备")]
  public class MissionPrepareCommand : ICommandHandler
  {
    public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
    {
      if (game.GameState != eGameState.SessionPrepared && game.GameState != eGameState.GameOver)
        return;
      bool flag = packet.ReadBoolean();
      if (player.Ready == flag)
        return;
      player.Ready = flag;
      game.SendSyncLifeTime();
      game.SendToAll(packet);
    }
  }
}
