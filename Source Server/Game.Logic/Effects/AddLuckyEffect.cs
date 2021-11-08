// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AddLuckyEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Actions;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class AddLuckyEffect : BasePlayerEffect
  {
    private int m_added;
    private int m_count;
    private int m_probability;

    public AddLuckyEffect(int count, int probability)
      : base(eEffectType.AddLuckyEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
      this.m_added = 0;
    }

    private void ChangeProperty(Player player)
    {
      if (player.CurrentBall.IsSpecial())
        return;
      player.Lucky -= (double) this.m_added;
      this.m_added = 0;
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probability)
        return;
      player.FlyingPartical = 65;
      this.IsTrigger = true;
      player.Lucky += (double) this.m_count;
      player.EffectTrigger = true;
      this.m_added = this.m_count;
      player.Game.AddAction((IAction) new LivingSayAction((Living) player, LanguageMgr.GetTranslation("AddLuckyEffect.msg"), 9, 0, 1000));
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.BeforePlayerShoot += new PlayerEventHandle(this.ChangeProperty);
      player.AfterPlayerShooted += new PlayerEventHandle(this.player_AfterPlayerShooted);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.BeforePlayerShoot -= new PlayerEventHandle(this.ChangeProperty);
      player.AfterPlayerShooted -= new PlayerEventHandle(this.player_AfterPlayerShooted);
    }

    private void player_AfterPlayerShooted(Player player)
    {
      player.FlyingPartical = 0;
    }

    public override bool Start(Living living)
    {
      AddLuckyEffect ofType = living.EffectList.GetOfType(eEffectType.AddLuckyEffect) as AddLuckyEffect;
      if (ofType == null)
        return base.Start(living);
      ofType.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
