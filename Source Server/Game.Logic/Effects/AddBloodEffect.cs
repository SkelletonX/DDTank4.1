// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.AddBloodEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class AddBloodEffect : BasePlayerEffect
  {
    private int m_count;
    private int m_probability;

    public AddBloodEffect(int count, int probability)
      : base(eEffectType.AddBloodEffect)
    {
      this.m_count = count;
      this.m_probability = probability;
    }

    public void ChangeProperty(Living living)
    {
      this.IsTrigger = false;
      if (this.rand.Next(100) >= this.m_probability)
        return;
      this.IsTrigger = true;
      living.EffectTrigger = true;
      living.Blood += this.m_count;
      living.Game.SendEquipEffect(living, LanguageMgr.GetTranslation("AddBloodEffect.Success", (object) this.m_count));
    }

    protected override void OnAttachedToPlayer(Player player)
    {
      player.PlayerShoot += new PlayerEventHandle(this.ChangeProperty);
      player.BeginAttacked += new LivingEventHandle(this.ChangeProperty);
    }

    protected override void OnRemovedFromPlayer(Player player)
    {
      player.PlayerShoot -= new PlayerEventHandle(this.ChangeProperty);
      player.BeginAttacked -= new LivingEventHandle(this.ChangeProperty);
    }

    public override bool Start(Living living)
    {
      AddBloodEffect ofType = living.EffectList.GetOfType(eEffectType.AddBloodEffect) as AddBloodEffect;
      if (ofType == null)
        return base.Start(living);
      this.m_probability = this.m_probability > ofType.m_probability ? this.m_probability : ofType.m_probability;
      return true;
    }
  }
}
