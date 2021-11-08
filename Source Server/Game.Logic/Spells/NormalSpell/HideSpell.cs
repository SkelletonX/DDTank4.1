// Decompiled with JetBrains decompiler
// Type: Game.Logic.Spells.NormalSpell.HideSpell
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Logic.Spells.NormalSpell
{
  [SpellAttibute(3)]
  public class HideSpell : ISpellHandler
  {
    public void Execute(BaseGame game, Player player, ItemTemplateInfo item)
    {
      switch (item.Property2)
      {
        case 0:
          if (!player.IsLiving)
          {
            if (game.CurrentLiving == null || !(game.CurrentLiving is Player) || game.CurrentLiving.Team != player.Team)
              break;
            new HideEffect(item.Property3).Start((Living) game.CurrentLiving);
            break;
          }
          new HideEffect(item.Property3).Start((Living) player);
          break;
        case 1:
          using (List<Player>.Enumerator enumerator = player.Game.GetAllFightPlayers().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Player current = enumerator.Current;
              if (current.IsLiving && current.Team == player.Team)
                new HideEffect(item.Property3).Start((Living) current);
            }
            break;
          }
      }
    }
  }
}
