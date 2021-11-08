// Decompiled with JetBrains decompiler
// Type: Game.Logic.Spells.SpellAttibute
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using System;

namespace Game.Logic.Spells
{
  public class SpellAttibute : Attribute
  {
    public SpellAttibute(int type)
    {
      this.Type = type;
    }

    public int Type { get; private set; }
  }
}
