// Decompiled with JetBrains decompiler
// Type: Game.Logic.Cmd.TryAgainCommand
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
  [GameCommand(119, "关卡失败再试一次")]
  public class TryAgainCommand : ICommandHandler
  {
    public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
    {
      if (!(game is PVEGame))
        return;
      PVEGame pveGame = game as PVEGame;
      bool flag = packet.ReadBoolean();
      if (!packet.ReadBoolean())
        return;
      if (flag)
      {
        if (player.PlayerDetail.RemoveMoney(100) > 0)
        {
          pveGame.WantTryAgain = 1;
          game.SendToAll(packet);
          player.PlayerDetail.LogAddMoney(AddMoneyType.Game, AddMoneyType.Game_TryAgain, player.PlayerDetail.PlayerCharacter.ID, 100, player.PlayerDetail.PlayerCharacter.Money);
        }
        else
          player.PlayerDetail.SendInsufficientMoney(2);
      }
      else
      {
        pveGame.WantTryAgain = 0;
        game.SendToAll(packet);
      }
      pveGame.CheckState(0);
    }
  }
}
