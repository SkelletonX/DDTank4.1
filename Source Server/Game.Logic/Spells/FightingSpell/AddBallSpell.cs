// Decompiled with JetBrains decompiler
// Type: Game.Logic.Spells.FightingSpell.AddBallSpell
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace Game.Logic.Spells.FightingSpell
{
  [SpellAttibute(15)]
  public class AddBallSpell : ISpellHandler
  {
    public void Execute(BaseGame game, Player player, ItemTemplateInfo item)
    {
      if (player.IsSpecialSkill)
        return;
      if ((player.CurrentBall.ID == 3 || player.CurrentBall.ID == 5 || player.CurrentBall.ID == 1) && item.TemplateID == 10003)
      {
        player.BallCount = 1;
      }
      else
      {
        player.CurrentDamagePlus *= 0.5f;
        player.BallCount = item.Property2;
      }
    }
  }
}
