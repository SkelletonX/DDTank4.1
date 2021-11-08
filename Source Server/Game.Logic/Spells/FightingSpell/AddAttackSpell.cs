// Decompiled with JetBrains decompiler
// Type: Game.Logic.Spells.FightingSpell.AddAttackSpell
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace Game.Logic.Spells.FightingSpell
{
  [SpellAttibute(14)]
  public class AddAttackSpell : ISpellHandler
  {
    public void Execute(BaseGame game, Player player, ItemTemplateInfo item)
    {
      if (player.IsLiving)
      {
        if (player.CurrentBall.ID != 3 && player.CurrentBall.ID != 5 && player.CurrentBall.ID != 1 || item.TemplateID != 10001 && item.TemplateID != 10002)
        {
          player.ShootCount += item.Property2;
          if (item.Property2 == 2)
            player.CurrentShootMinus *= 0.6f;
          else
            player.CurrentShootMinus *= 0.9f;
        }
        else
          player.ShootCount = 1;
      }
      else
      {
        if (game.CurrentLiving == null || !(game.CurrentLiving is Player) || game.CurrentLiving.Team != player.Team)
          return;
        if (player.CurrentBall.ID != 3 && player.CurrentBall.ID != 5 && player.CurrentBall.ID != 1 || item.TemplateID != 10001 && item.TemplateID != 10002)
        {
          (game.CurrentLiving as Player).ShootCount += item.Property2;
          if (item.Property2 == 2)
            game.CurrentLiving.CurrentShootMinus *= 0.6f;
          else
            game.CurrentLiving.CurrentShootMinus *= 0.9f;
        }
        else
          (game.CurrentLiving as Player).ShootCount = 1;
      }
    }
  }
}
