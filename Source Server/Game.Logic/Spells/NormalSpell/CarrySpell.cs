﻿// Decompiled with JetBrains decompiler
// Type: Game.Logic.Spells.NormalSpell.CarrySpell
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace Game.Logic.Spells.NormalSpell
{
  [SpellAttibute(5)]
  public class CarrySpell : ISpellHandler
  {
    public void Execute(BaseGame game, Player player, ItemTemplateInfo item)
    {
      if (player.IsLiving)
      {
        player.SetBall(3);
      }
      else
      {
        if (game.CurrentLiving == null || !(game.CurrentLiving is Player) || game.CurrentLiving.Team != player.Team)
          return;
        (game.CurrentLiving as Player).SetBall(3);
      }
    }
  }
}
