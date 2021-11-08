// Decompiled with JetBrains decompiler
// Type: Game.Logic.Effects.Phongan
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Effects
{
  public class Phongan : AbstractEffect
  {
    private int m_count;

    public Phongan(int count)
      : base(eEffectType.PhongAn)
    {
      this.m_count = count;
    }

    public override void OnAttached(Living living)
    {
      living.BeginSelfTurn += new LivingEventHandle(this.player_BeginFitting);
      living.Game.SendPlayerPicture(living, 2, true);
    }

    public override void OnRemoved(Living living)
    {
      living.BeginSelfTurn -= new LivingEventHandle(this.player_BeginFitting);
      living.Game.SendPlayerPicture(living, 2, false);
    }

    private void player_BeginFitting(Living living)
    {
      Player player = (Player) null;
      --this.m_count;
      if (living is Player)
      {
        player = living as Player;
        player.capnhatstate("silencedSpecial", "true");
        player.IconPicture(eMirariType.Lockstate, true);
        player.State = 9;
      }
      if (this.m_count >= 0)
        return;
      if (player != null)
      {
        player.capnhatstate("silencedSpecial", "false");
        player.IconPicture(eMirariType.ReversePlayer, true);
        player.State = 0;
      }
      this.Stop();
    }

    public override bool Start(Living living)
    {
      Phongan ofType = living.EffectList.GetOfType(eEffectType.PhongAn) as Phongan;
      if (ofType == null)
        return base.Start(living);
      ofType.m_count = this.m_count;
      return true;
    }
  }
}
