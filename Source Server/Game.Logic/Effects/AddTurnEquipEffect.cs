// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AddTurnEquipEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class AddTurnEquipEffect : BasePlayerEffect
  {
    private int int_0;
    private int int_1;

    public AddTurnEquipEffect(int count, int probability)
      : base(eEffectType.AddTurnEquipEffect)
    {
      this.int_0 = count;
      this.int_1 = probability;
    }

    public override bool Start(Living living)
    {
      AddTurnEquipEffect ofType = living.EffectList.GetOfType(eEffectType.AddTurnEquipEffect) as AddTurnEquipEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.int_1 = this.int_1 > ofType.int_1 ? this.int_1 : ofType.int_1;
      return true;
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerShoot += new PlayerEventHandle(this.method_0);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerShoot -= new PlayerEventHandle(this.method_0);
    }

    private void method_0(Player player_0)
    {
      if (player_0.CurrentBall.IsSpecial() || this.rand.Next(10000) >= this.int_1 * 10)
        return;
      player_0.Delay = player_0.DefaultDelay;
      this.IsTrigger = true;
      player_0.EffectTrigger = true;
      player_0.Game.AddAction((IAction) new LivingSayAction((Living) player_0, LanguageMgr.GetTranslation("AddTurnEquipEffect.msg"), 9, 0, 1000));
    }
  }
}
