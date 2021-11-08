// Decompiled with JetBrains decompiler
// Type: Game.Logic.Cmd.BossTakeCardCommand
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
  [GameCommand(130, "战胜关卡中Boss翻牌")]
  public class BossTakeCardCommand : ICommandHandler
  {
    public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
    {
      if (player.FinishTakeCard || player.CanTakeOut <= 0)
        return;
      int index = (int) packet.ReadByte();
      if (index < 0 || index > game.Cards.Length)
        game.TakeCard(player);
      else
        game.TakeCard(player, index);
    }
  }
}
