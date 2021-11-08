// Decompiled with JetBrains decompiler
// Type: Game.Logic.Spells.FightingSpell.AddWoudSpell
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace Game.Logic.Spells.FightingSpell
{
  [SpellAttibute(13)]
  public class AddWoudSpell : ISpellHandler
  {
    public void Execute(BaseGame game, Player player, ItemTemplateInfo item)
    {
      player.CurrentDamagePlus += (float) item.Property2 / 100f;
    }
  }
}
