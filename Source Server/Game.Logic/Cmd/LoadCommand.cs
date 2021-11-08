// Decompiled with JetBrains decompiler
// Type: Game.Logic.Cmd.LoadCommand
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
  [GameCommand(16, "游戏加载进度")]
  public class LoadCommand : ICommandHandler
  {
    public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
    {
      if (game.GameState != eGameState.Loading)
        return;
      player.LoadingProcess = packet.ReadInt();
      if (player.LoadingProcess >= 100)
        game.CheckState(0);
      int loadingProcess = player.LoadingProcess;
      GSPacketIn pkg = new GSPacketIn((short) 91);
      pkg.WriteByte((byte) 16);
      pkg.WriteInt(player.LoadingProcess);
      pkg.WriteInt(player.PlayerDetail.ZoneId);
      pkg.WriteInt(player.PlayerDetail.PlayerCharacter.ID);
      game.SendToAll(pkg);
    }
  }
}
