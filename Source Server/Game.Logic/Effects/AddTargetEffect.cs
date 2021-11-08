// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AddTargetEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class AddTargetEffect : BasePlayerEffect
  {
    public AddTargetEffect()
      : base(eEffectType.AddTargetEffect)
    {
    }

    public override bool Start(Living living)
    {
      if (living.EffectList.GetOfType(eEffectType.AddTargetEffect) is AddTargetEffect)
        return true;
      return base.Start(living);
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.Game.SendPlayerPicture((Living) player, 7, true);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.Game.SendPlayerPicture((Living) player, 7, false);
    }
  }
}
