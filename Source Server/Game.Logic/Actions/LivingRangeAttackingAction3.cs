// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingRangeAttackingAction3
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Base.Packets;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace Game.Logic.Actions
{
  public class LivingRangeAttackingAction3 : BaseAction
  {
    private static ThreadSafeRandom random = new ThreadSafeRandom();
    private Living m_living;
    private List<Living> m_Livings;
    private int m_fx;
    private int m_tx;
    private string m_action;
    private int m_type;

    public LivingRangeAttackingAction3(
      Living living,
      int fx,
      int tx,
      string action,
      int delay,
      List<Living> livings,
      int type)
      : base(delay, 1500)
    {
      this.m_living = living;
      this.m_Livings = livings;
      this.m_fx = fx;
      this.m_tx = tx;
      this.m_action = action;
      this.m_type = type;
    }

    private int MakeDamage(Living p)
    {
      double baseDamage = this.m_living.BaseDamage;
      double num1 = p.BaseGuard;
      double num2 = p.Defence;
      double attack = this.m_living.Attack;
      if (this.m_living.IgnoreArmor)
      {
        num1 = 0.0;
        num2 = 0.0;
      }
      float currentDamagePlus = this.m_living.CurrentDamagePlus;
      float currentShootMinus = this.m_living.CurrentShootMinus;
      double num3 = 0.95 * (num1 - (double) (3 * this.m_living.Grade)) / (500.0 + num1 - (double) (3 * this.m_living.Grade));
      double num4 = num2 - this.m_living.Lucky >= 0.0 ? 0.95 * (num2 - this.m_living.Lucky) / (600.0 + num2 - this.m_living.Lucky) : 0.0;
      double num5 = 1.0 + attack * 0.001;
      double num6 = baseDamage * num5 * (1.0 - (num3 + num4 - num3 * num4)) * (double) currentDamagePlus * (double) currentShootMinus * (1.1 - (double) Math.Abs(p.GetDirectDemageRect().X - this.m_living.X) / (double) Math.Abs(this.m_tx - this.m_fx) / 5.0);
      return num6 >= 0.0 ? (int) num6 : 1;
    }

    private int MakeCriticalDamage(Living p, int baseDamage)
    {
      double lucky = this.m_living.Lucky;
      return lucky * 70.0 / (2000.0 + lucky) <= (double) LivingRangeAttackingAction3.random.Next(100) ? 0 : (int) ((0.5 + lucky * 0.0003) * (double) baseDamage);
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      GSPacketIn pkg = new GSPacketIn((short) 91);
      pkg.Parameter1 = this.m_living.Id;
      pkg.WriteByte((byte) 61);
      List<Living> living1 = game.Map.FindLiving(this.m_fx, this.m_tx, this.m_Livings);
      List<Living> livingList = new List<Living>();
      foreach (Living killLiving in living1)
      {
        if (((PVEGame) game).CanCampAttack(this.m_living, killLiving))
          livingList.Add(killLiving);
      }
      int count = livingList.Count;
      foreach (Living living2 in livingList)
      {
        if (this.m_living.IsFriendly(living2))
          --count;
      }
      pkg.WriteInt(count);
      this.m_living.SyncAtTime = false;
      try
      {
        foreach (Living living2 in livingList)
        {
          living2.SyncAtTime = false;
          if (!this.m_living.IsFriendly(living2))
          {
            int val1 = 0;
            living2.IsFrost = false;
            game.SendGameUpdateFrozenState(living2);
            int damageAmount = this.MakeDamage(living2);
            int criticalAmount = this.MakeCriticalDamage(living2, damageAmount);
            int val2 = 0;
            if (living2.TakeDamage(this.m_living, ref damageAmount, ref criticalAmount, "", 1000))
            {
              val2 = damageAmount + criticalAmount;
              if (living2 is Player)
                val1 = (living2 as Player).Dander;
            }
            pkg.WriteInt(living2.Id);
            pkg.WriteInt(val2);
            pkg.WriteInt(living2.Blood);
            pkg.WriteInt(val1);
            pkg.WriteInt(criticalAmount != 0 ? 2 : 1);
          }
        }
        game.SendToAll(pkg);
        this.Finish(tick);
      }
      finally
      {
        this.m_living.SyncAtTime = true;
        foreach (Living living2 in livingList)
          living2.SyncAtTime = true;
      }
    }
  }
}
