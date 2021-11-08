// Decompiled with JetBrains decompiler
// Type: Game.Logic.Cmd.PassDramaCommand
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
  [GameCommand(133, "跳过剧情动画")]
  public class PassDramaCommand : ICommandHandler
  {
    public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
    {
      if (!(game is PVEGame) || game.GameState == eGameState.Playing)
        return;
      PVEGame pveGame = game as PVEGame;
      pveGame.IsPassDrama = packet.ReadBoolean();
      pveGame.CheckState(0);
    }
  }
}
