// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingRangeAttackingAction2
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
  public class LivingRangeAttackingAction2 : BaseAction
  {
    private bool bool_0;
    private bool bool_1;
    private int int_x;
    private int int_y;
    private List<Player> list_player;
    private Living living_0;
    private string string_0;

    public LivingRangeAttackingAction2(
      Living living,
      int fx,
      int tx,
      string action,
      int delay,
      bool removeFrost,
      bool directDamage,
      List<Player> players)
      : base(delay, 1000)
    {
      this.living_0 = living;
      this.list_player = players;
      this.int_x = fx;
      this.int_y = tx;
      this.string_0 = action;
      this.bool_0 = removeFrost;
      this.bool_1 = directDamage;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      GSPacketIn pkg = new GSPacketIn((short) 91, this.living_0.Id)
      {
        Parameter1 = this.living_0.Id
      };
      pkg.WriteByte((byte) 61);
      List<Living> players = game.Map.FindPlayers(this.int_x, this.int_y, this.list_player);
      int count = players.Count;
      foreach (Living living in players)
      {
        if (this.living_0.IsFriendly(living) || (living is SimpleBoss || living is SimpleNpc) && !living.Config.CanTakeDamage)
          --count;
      }
      pkg.WriteInt(count);
      this.living_0.SyncAtTime = false;
      try
      {
        foreach (Living living in players)
        {
          living.SyncAtTime = false;
          if (!this.living_0.IsFriendly(living) && (!(living is SimpleBoss) && !(living is SimpleNpc) || living.Config.CanTakeDamage) || living.Config.IsHelper)
          {
            int val1 = 0;
            if (living.IsFrost)
            {
              living.IsFrost = false;
              game.method_30(living);
              this.Finish(tick);
              return;
            }
            int damageAmount = this.method_0(living);
            int criticalAmount = this.method_1(living, damageAmount);
            int val2 = 0;
            if (living.TakeDamage(this.living_0, ref damageAmount, ref criticalAmount, "范围攻击", 0))
            {
              val2 = damageAmount + criticalAmount;
              if (living is Player)
                val1 = (living as Player).Dander;
            }
            pkg.WriteInt(living.Id);
            pkg.WriteInt(val2);
            pkg.WriteInt(living.Blood);
            pkg.WriteInt(val1);
            pkg.WriteInt(living.IsLiving ? 1 : 6);
          }
        }
        game.SendToAll(pkg);
        this.Finish(tick);
      }
      finally
      {
        this.living_0.SyncAtTime = true;
        foreach (Living living in players)
          living.SyncAtTime = true;
      }
    }

    private int method_0(Living living_1)
    {
      double baseGuard = living_1.BaseGuard;
      double defence = living_1.Defence;
      double attack = this.living_0.Attack;
      double num1;
      double num2;
      if (living_1.AddArmor && (living_1 as Player).DeputyWeapon != null)
      {
        int num3 = (living_1 as Player).DeputyWeapon.Template.Property7 + (int) Math.Pow(1.1, (double) (living_1 as Player).DeputyWeapon.StrengthenLevel);
        num1 = baseGuard + (double) num3;
        num2 = defence + (double) num3;
      }
      if (this.living_0.IgnoreArmor)
      {
        num1 = 0.0;
        num2 = 0.0;
      }
      float currentDamagePlus = this.living_0.CurrentDamagePlus;
      float currentShootMinus = this.living_0.CurrentShootMinus;
      double num4 = 0.95 * (living_1.BaseGuard - (double) (3 * this.living_0.Grade)) / (500.0 + living_1.BaseGuard - (double) (3 * this.living_0.Grade));
      double num5 = living_1.Defence - this.living_0.Lucky >= 0.0 ? 0.95 * (living_1.Defence - this.living_0.Lucky) / (600.0 + living_1.Defence - this.living_0.Lucky) : 0.0;
      double num6 = this.living_0.BaseDamage * (1.0 + attack * 0.001) * (1.0 - (num4 + num5 - num4 * num5)) * (double) currentDamagePlus * (double) currentShootMinus;
      if (!this.bool_1)
      {
        Rectangle directDemageRect = living_1.GetDirectDemageRect();
        double num3 = Math.Sqrt((double) ((directDemageRect.X - this.living_0.X) * (directDemageRect.X - this.living_0.X) + (directDemageRect.Y - this.living_0.Y) * (directDemageRect.Y - this.living_0.Y)));
        num6 *= 1.0 - num3 / (double) Math.Abs(this.int_y - this.int_x) / 4.0;
      }
      if (num6 <= 0.0)
        return 1;
      return (int) num6;
    }

    private int method_1(Living living_1, int int_2)
    {
      double lucky = this.living_0.Lucky;
      Random random = new Random();
      if (75000.0 * lucky / (lucky + 800.0) <= (double) random.Next(100000))
        return 0;
      int num = 0;
      return (int) ((0.5 + lucky * 0.0003) * (double) int_2) * (100 - num) / 100;
    }
  }
}
