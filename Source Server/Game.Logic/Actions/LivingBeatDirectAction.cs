// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingBeatDirectAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Base.Packets;
using Game.Logic.Phy.Object;
using System;

namespace Game.Logic.Actions
{
  public class LivingBeatDirectAction : BaseAction
  {
    private int int_0;
    private int int_1;
    private Living living_0;
    private Living living_1;
    private string string_0;

    public LivingBeatDirectAction(
      Living living,
      Living target,
      string action,
      int delay,
      int livingCount,
      int attackEffect)
      : base(delay)
    {
      this.living_0 = living;
      this.living_1 = target;
      this.string_0 = action;
      this.int_0 = livingCount;
      this.int_1 = attackEffect;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      this.living_1.SyncAtTime = false;
      try
      {
        GSPacketIn pkg = new GSPacketIn((short) 91, this.living_0.Id)
        {
          Parameter1 = this.living_0.Id
        };
        pkg.WriteByte((byte) 58);
        pkg.WriteString(!string.IsNullOrEmpty(this.string_0) ? this.string_0 : "");
        pkg.WriteInt(this.int_0);
        for (int index = 1; index <= this.int_0; ++index)
        {
          int damageAmount = this.living_0.MakeDamage(this.living_1, false);
          int criticalAmount = this.MakeCriticalDamage(damageAmount);
          int val = 0;
          if (this.living_1 is Player)
            val = (this.living_1 as Player).Dander;
          if (this.living_1.IsFrost)
          {
            this.living_1.IsFrost = false;
            game.method_30(this.living_1);
          }
          if (!this.living_1.TakeDamage(this.living_0, ref damageAmount, ref criticalAmount, "小怪伤血", 0))
            Console.WriteLine("//error beat direct damage");
          pkg.WriteInt(this.living_1.Id);
          pkg.WriteInt(damageAmount + criticalAmount);
          pkg.WriteInt(this.living_1.Blood);
          pkg.WriteInt(val);
          pkg.WriteInt(this.int_1);
        }
        game.SendToAll(pkg);
        this.Finish(tick);
      }
      finally
      {
        this.living_1.SyncAtTime = true;
      }
    }

    protected int MakeCriticalDamage(int baseDamage)
    {
      double lucky = this.living_0.Lucky;
      if (lucky * 45.0 / (800.0 + lucky) <= (double) ThreadSafeRandom.NextStatic(100))
        return 0;
      int num1 = 0;
      int num2 = (int) ((0.5 + lucky * 0.00015) * (double) baseDamage) * (100 - num1) / 100;
      if (this.living_0.FightBuffers.ConsortionAddCritical > 0)
        num2 += this.living_0.FightBuffers.ConsortionAddCritical;
      return num2;
    }
  }
}
