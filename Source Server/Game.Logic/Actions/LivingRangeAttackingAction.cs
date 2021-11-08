// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingRangeAttackingAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Packets;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.Actions
{
  public class LivingRangeAttackingAction : BaseAction
  {
    private string m_action;
    private int m_fx;
    private Living m_living;
    private List<Player> m_players;
    private int m_tx;

    public LivingRangeAttackingAction(
      Living living,
      int fx,
      int tx,
      string action,
      int delay,
      List<Player> players)
      : base(delay, 1000)
    {
      this.m_living = living;
      this.m_players = players;
      this.m_fx = fx;
      this.m_tx = tx;
      this.m_action = action;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      GSPacketIn pkg = new GSPacketIn((short) 91, this.m_living.Id)
      {
        Parameter1 = this.m_living.Id
      };
      pkg.WriteByte((byte) 61);
      List<Living> players = game.Map.FindPlayers(this.m_fx, this.m_tx, this.m_players);
      int count = players.Count;
      foreach (Living living in players)
      {
        if (this.m_living.IsFriendly(living) || (living is SimpleBoss || living is SimpleNpc) && !living.Config.CanTakeDamage)
          --count;
      }
      pkg.WriteInt(count);
      this.m_living.SyncAtTime = false;
      try
      {
        foreach (Living living in players)
        {
          living.SyncAtTime = false;
          if (!this.m_living.IsFriendly(living) && (!(living is SimpleBoss) && !(living is SimpleNpc) || living.Config.CanTakeDamage))
          {
            int val1 = 0;
            living.IsFrost = false;
            game.SendGameUpdateFrozenState(living);
            int damageAmount = this.MakeDamage(living);
            int criticalAmount = this.MakeCriticalDamage(living, damageAmount);
            int val2 = 0;
            if (living.TakeDamage(this.m_living, ref damageAmount, ref criticalAmount, "范围攻击", 0))
            {
              val2 = damageAmount + criticalAmount;
              if (living is Player)
                val1 = (living as Player).Dander;
            }
            pkg.WriteInt(living.Id);
            pkg.WriteInt(val2);
            pkg.WriteInt(living.Blood);
            pkg.WriteInt(val1);
            pkg.WriteInt(1);
          }
        }
        game.SendToAll(pkg);
        this.Finish(tick);
      }
      finally
      {
        this.m_living.SyncAtTime = true;
        foreach (Living living in players)
          living.SyncAtTime = true;
      }
    }

    private int MakeCriticalDamage(Living p, int baseDamage)
    {
      double lucky = this.m_living.Lucky;
      Random random = new Random();
      if (75000.0 * lucky / (lucky + 800.0) <= (double) random.Next(100000))
        return 0;
      int num = 0;
      return (int) ((0.5 + lucky * 0.0003) * (double) baseDamage) * (100 - num) / 100;
    }

    private int MakeDamage(Living p)
    {
      double baseDamage = this.m_living.BaseDamage;
      double num1 = p.BaseGuard;
      double num2 = p.Defence;
      double attack = this.m_living.Attack;
      if (p.AddArmor && (p as Player).DeputyWeapon != null)
      {
        int num3 = (p as Player).DeputyWeapon.Template.Property7 + (int) Math.Pow(1.1, (double) (p as Player).DeputyWeapon.StrengthenLevel);
        num1 += (double) num3;
        num2 += (double) num3;
      }
      if (this.m_living.IgnoreArmor)
      {
        num1 = 0.0;
        num2 = 0.0;
      }
      float currentDamagePlus = this.m_living.CurrentDamagePlus;
      float currentShootMinus = this.m_living.CurrentShootMinus;
      double num4 = 0.95 * (num1 - (double) (3 * this.m_living.Grade)) / (500.0 + num1 - (double) (3 * this.m_living.Grade));
      double num5 = num2 - this.m_living.Lucky >= 0.0 ? 0.95 * (num2 - this.m_living.Lucky) / (600.0 + num2 - this.m_living.Lucky) : 0.0;
      double num6 = 1.0 + attack * 0.001;
      double num7 = baseDamage * num6 * (1.0 - (num4 + num5 - num4 * num5)) * (double) currentDamagePlus * (double) currentShootMinus;
      Rectangle directDemageRect = p.GetDirectDemageRect();
      double num8 = Math.Sqrt((double) ((directDemageRect.X - this.m_living.X) * (directDemageRect.X - this.m_living.X) + (directDemageRect.Y - this.m_living.Y) * (directDemageRect.Y - this.m_living.Y)));
      double num9 = num7 * (1.0 - num8 / (double) Math.Abs(this.m_tx - this.m_fx) / 4.0);
      if (num9 < 0.0)
        return 1;
      return (int) num9;
    }
  }
}
