// Decompiled with JetBrains decompiler
// Type: Game.Logic.Spells.FightingSpell.ShootStraightSpell
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace Game.Logic.Spells.FightingSpell
{
  [SpellAttibute(12)]
  public class ShootStraightSpell : ISpellHandler
  {
    public void Execute(BaseGame game, Player player, ItemTemplateInfo item)
    {
      if (player.IsLiving)
      {
        player.ControlBall = true;
        player.CurrentShootMinus *= 0.5f;
      }
      else
      {
        if (game.CurrentLiving == null || !(game.CurrentLiving is Player) || game.CurrentLiving.Team != player.Team)
          return;
        game.CurrentLiving.ControlBall = true;
        game.CurrentLiving.CurrentShootMinus *= 0.5f;
      }
    }
  }
}
