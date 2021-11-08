// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.BasePlayerEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class BasePlayerEffect : AbstractEffect
  {
    public BasePlayerEffect(eEffectType type)
      : base(type)
    {
    }

    public override sealed void OnAttached(Living living)
    {
      if (!(living is Player))
        return;
      this.OnAttachedToPlayer(living as Player);
    }

    protected virtual void OnAttachedToPlayer(Player player)
    {
    }

    public override sealed void OnRemoved(Living living)
    {
      if (!(living is Player))
        return;
      this.OnRemovedFromPlayer(living as Player);
    }

    protected virtual void OnRemovedFromPlayer(Player player)
    {
    }

    public override bool Start(Living living)
    {
      if (living is Player)
        return base.Start(living);
      return false;
    }
  }
}
