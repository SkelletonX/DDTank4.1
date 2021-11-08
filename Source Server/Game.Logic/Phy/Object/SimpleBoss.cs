// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Object.SimpleBoss
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.AI;
using Game.Logic.AI.Npc;
using Game.Server.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Game.Logic.Phy.Object
{
  public class SimpleBoss : TurnedLiving
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private List<SimpleNpc> m_child = new List<SimpleNpc>();
    private List<SimpleBoss> m_childB = new List<SimpleBoss>();
    protected NpcInfo m_npcInfo;
    private ABrain m_ai;
    private Dictionary<Player, int> m_mostHateful;
    private List<SimpleNpc> m_fire;
    private List<SimpleBoss> m_boss;

    public NpcInfo NpcInfo
    {
      get
      {
        return this.m_npcInfo;
      }
    }

    public List<SimpleNpc> Child
    {
      get
      {
        return this.m_child;
      }
    }

    public List<SimpleBoss> ChildB
    {
      get
      {
        return this.m_childB;
      }
    }

    public int CurrentLivingNpcNum
    {
      get
      {
        int num = 0;
        foreach (Physics physics in this.Child)
        {
          if (physics.IsLiving)
            ++num;
        }
        foreach (Physics physics in this.ChildB)
        {
          if (physics.IsLiving)
            ++num;
        }
        return num;
      }
    }

    public SimpleBoss(
      int id,
      BaseGame game,
      NpcInfo npcInfo,
      int direction,
      int type,
      string actions)
      : base(id, game, npcInfo.Camp, npcInfo.Name, npcInfo.ModelID, npcInfo.Blood, npcInfo.Immunity, direction)
    {
      this.m_child = new List<SimpleNpc>();
      this.m_boss = new List<SimpleBoss>();
      this.m_fire = new List<SimpleNpc>();
      switch (type)
      {
        case 0:
          this.Type = eLivingType.SimpleBoss;
          break;
        case 1:
          this.Type = eLivingType.ClearEnemy;
          break;
        default:
          this.Type = (eLivingType) type;
          break;
      }
      this.ActionStr = actions;
      this.m_mostHateful = new Dictionary<Player, int>();
      this.m_npcInfo = npcInfo;
      this.m_ai = ScriptMgr.CreateInstance(npcInfo.Script) as ABrain;
      if (this.m_ai == null)
      {
        SimpleBoss.log.ErrorFormat("Can't create abrain :{0}", (object) npcInfo.Script);
        this.m_ai = (ABrain) SimpleBrain.Simple;
      }
      this.m_ai.Game = this.m_game;
      this.m_ai.Body = (Living) this;
      try
      {
        this.m_ai.OnCreated();
      }
      catch (Exception ex)
      {
        SimpleBoss.log.ErrorFormat("SimpleBoss Created error:{1}", (object) ex);
      }
    }

    public override void Reset()
    {
      this.m_maxBlood = this.m_npcInfo.Blood;
      this.BaseDamage = (double) this.m_npcInfo.BaseDamage;
      this.BaseGuard = (double) this.m_npcInfo.BaseGuard;
      this.Attack = (double) this.m_npcInfo.Attack;
      this.Defence = (double) this.m_npcInfo.Defence;
      this.Agility = (double) this.m_npcInfo.Agility;
      this.Lucky = (double) this.m_npcInfo.Lucky;
      this.Grade = this.m_npcInfo.Level;
      this.Experience = this.m_npcInfo.Experience;
      this.m_delay = this.m_npcInfo.Agility;
      this.SetRect(this.m_npcInfo.X, this.m_npcInfo.Y, this.m_npcInfo.Width, this.m_npcInfo.Height);
      this.SetRelateDemagemRect(this.m_npcInfo.X, this.m_npcInfo.Y, this.m_npcInfo.Width, this.m_npcInfo.Height);
      base.Reset();
    }

    public override void Die()
    {
      base.Die();
    }

    public override void Die(int delay)
    {
      base.Die(delay);
    }

    public override bool TakeDamage(
      Living source,
      ref int damageAmount,
      ref int criticalAmount,
      string msg,
      int delay)
    {
      bool damage = base.TakeDamage(source, ref damageAmount, ref criticalAmount, msg, delay);
      if (source is Player)
      {
        Player key = source as Player;
        int num = damageAmount + criticalAmount;
        if (this.m_mostHateful.ContainsKey(key))
        {
          this.m_mostHateful[key] += num;
          return damage;
        }
        this.m_mostHateful.Add(key, num);
      }
      if (damage)
        this.ShootedSay(delay);
      return damage;
    }

    public void RandomSay(string[] msg, int type, int delay, int finishTime)
    {
      int index = this.Game.Random.Next(0, msg.Length);
      this.Say(msg[index], type, delay, finishTime);
    }

    public Player FindMostHatefulPlayer()
    {
      Player player;
      if (this.m_mostHateful.Count > 0)
      {
        KeyValuePair<Player, int> keyValuePair1 = this.m_mostHateful.ElementAt<KeyValuePair<Player, int>>(0);
        foreach (KeyValuePair<Player, int> keyValuePair2 in this.m_mostHateful)
        {
          if (keyValuePair1.Value < keyValuePair2.Value)
            keyValuePair1 = keyValuePair2;
        }
        player = keyValuePair1.Key;
      }
      else
        player = (Player) null;
      return player;
    }

    public void CreateChild(int id, int x, int y, int disToSecond, int maxCount, int direction)
    {
      if (this.CurrentLivingNpcNum >= maxCount)
        return;
      if (maxCount - this.CurrentLivingNpcNum >= 2)
      {
        this.Child.Add(((PVEGame) this.Game).CreateNpc(id, x + disToSecond, y, 1, direction));
        this.Child.Add(((PVEGame) this.Game).CreateNpc(id, x, y, 1, direction));
      }
      else
      {
        if (maxCount - this.CurrentLivingNpcNum != 1)
          return;
        this.Child.Add(((PVEGame) this.Game).CreateNpc(id, x, y, 1, direction));
      }
    }

    public void CreateChild(
      int id,
      Point[] brithPoint,
      int maxCount,
      int maxCountForOnce,
      int type)
    {
      this.CreateChild(id, brithPoint, maxCount, maxCountForOnce, type, 0);
    }

    public void CreateChild(
      int id,
      Point[] brithPoint,
      int maxCount,
      int maxCountForOnce,
      int type,
      int objtype)
    {
      Point[] pointArray = new Point[brithPoint.Length];
      if (this.CurrentLivingNpcNum >= maxCount)
        return;
      int num = maxCount - this.CurrentLivingNpcNum < maxCountForOnce ? maxCount - this.CurrentLivingNpcNum : maxCountForOnce;
      for (int index1 = 0; index1 < num; ++index1)
      {
        int index2 = 0;
        if (num <= brithPoint.Length)
        {
          for (int index3 = 0; index3 < brithPoint.Length; index3 = index3 - 1 + 1)
          {
            index2 = this.Game.Random.Next(0, brithPoint.Length);
            bool flag = false;
            for (int index4 = 0; index4 < pointArray.Length; ++index4)
            {
              if (brithPoint[index2] == pointArray[index4])
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              pointArray[index2] = brithPoint[index2];
              break;
            }
          }
        }
        if (objtype == 0)
          this.Child.Add(((PVEGame) this.Game).CreateNpc(id, brithPoint[index2].X, brithPoint[index2].Y, type));
        else
          this.ChildB.Add(((PVEGame) this.Game).CreateBoss(id, brithPoint[index2].X, brithPoint[index2].Y, -1, type));
      }
    }

    public SimpleNpc CreateChild(
      int id,
      int x,
      int y,
      bool showBlood,
      LivingConfig config)
    {
      return this.CreateChild(id, x, y, 0, -1, showBlood, config);
    }

    public SimpleNpc CreateChild(
      int id,
      int x,
      int y,
      int dir,
      bool showBlood,
      LivingConfig config)
    {
      return this.CreateChild(id, x, y, 0, dir, showBlood, config);
    }

    public SimpleNpc CreateChild(
      int id,
      int x,
      int y,
      int type,
      int dir,
      bool showBlood,
      LivingConfig config)
    {
       SimpleNpc npc;
      npc = ((PVEGame) this.Game).CreateNpc(id, x, y, type, dir, config);
      this.Child.Add(npc);
      if (!showBlood)
        this.Game.PedSuikAov((Living) npc, 0);
      return npc;
    }

    public SimpleNpc CreateChild(
      int id,
      int x,
      int y,
      int type,
      int dir,
      bool showBlood)
    {
       SimpleNpc npc;
      npc = ((PVEGame) this.Game).CreateNpc(id, x, y, type, dir);
      this.Child.Add(npc);
      if (!showBlood)
        this.Game.PedSuikAov((Living) npc, 0);
      return npc;
    }

        public void RandomConSay(string[] msg, int type, int delay, int finishTime)
    {
      if (this.Game.Random.Next(0, 2) != 1)
        return;
      int index = this.Game.Random.Next(0, ((IEnumerable<string>) msg).Count<string>());
      this.Say(msg[index], type, delay, finishTime);
    }

    public void TowardsToPlayer(int playerX, int delay)
    {
      if (playerX > this.X)
        this.ChangeDirection(1, delay);
      else
        this.ChangeDirection(-1, delay);
    }

    public List<SimpleBoss> Boss
    {
      get
      {
        return this.m_boss;
      }
    }

    public List<SimpleNpc> Fire
    {
      get
      {
        return this.m_fire;
      }
    }

    public void RemoveAllChild()
    {
      foreach (SimpleNpc simpleNpc in this.Child)
      {
        if (simpleNpc.IsLiving)
          simpleNpc.Die();
      }
      this.m_child = new List<SimpleNpc>();
    }

    public override void PrepareNewTurn()
    {
      base.PrepareNewTurn();
      try
      {
        this.m_ai.OnBeginNewTurn();
      }
      catch (Exception ex)
      {
        SimpleBoss.log.ErrorFormat("SimpleBoss BeginNewTurn error:{0}", (object) ex);
      }
    }

    public override void BeforeTakedDamage(
      Living source,
      ref int damageAmount,
      ref int criticalAmount)
    {
      try
      {
        this.m_ai.OnBeforeTakedDamage(source, ref damageAmount, ref criticalAmount);
      }
      catch (Exception ex)
      {
        SimpleBoss.log.ErrorFormat("SimpleBoss BeforeTakeDamage error:{0}", (object) ex);
      }
      base.BeforeTakedDamage(source, ref damageAmount, ref criticalAmount);
    }

    public override void PrepareSelfTurn()
    {
      base.PrepareSelfTurn();
      this.DefaultDelay = this.m_delay;
      this.AddDelay(this.m_npcInfo.Delay);
      try
      {
        this.m_ai.OnBeginSelfTurn();
      }
      catch (Exception ex)
      {
        SimpleBoss.log.ErrorFormat("SimpleBoss BeginSelfTurn error:{0}", (object) ex);
      }
    }

    public override void StartAttacking()
    {
      base.StartAttacking();
      try
      {
        this.m_ai.OnStartAttacking();
      }
      catch (Exception ex)
      {
        SimpleBoss.log.ErrorFormat("SimpleBoss StartAttacking error:{0}", (object) ex);
      }
      if (!this.IsAttacking)
        return;
      this.StopAttacking();
    }

    public override void StopAttacking()
    {
      base.StopAttacking();
      try
      {
        this.m_ai.OnStopAttacking();
      }
      catch (Exception ex)
      {
        SimpleBoss.log.ErrorFormat("SimpleBoss StopAttacking error:{0}", (object) ex);
      }
    }

    public override void Dispose()
    {
      base.Dispose();
      try
      {
        this.m_ai.Dispose();
      }
      catch (Exception ex)
      {
        SimpleBoss.log.ErrorFormat("SimpleBoss Dispose error:{0}", (object) ex);
      }
    }

    public void KillPlayerSay()
    {
      try
      {
        this.m_ai.OnKillPlayerSay();
      }
      catch (Exception ex)
      {
        SimpleBoss.log.ErrorFormat("SimpleBoss Say error:{0}", (object) ex);
      }
    }

    public void DiedSay()
    {
      try
      {
        this.m_ai.OnDiedSay();
      }
      catch (Exception ex)
      {
        SimpleBoss.log.ErrorFormat("SimpleBoss DiedSay error {0}", (object) ex);
      }
    }

    public void DiedEvent()
    {
      try
      {
        this.m_ai.OnDiedEvent();
      }
      catch (Exception ex)
      {
        SimpleBoss.log.ErrorFormat("SimpleBoss DiedEvent error {0}", (object) ex);
      }
    }

    public override void OnAfterTakedBomb()
    {
      try
      {
        this.m_ai.OnAfterTakedBomb();
      }
      catch (Exception ex)
      {
        SimpleBoss.log.ErrorFormat("SimpleBoss OnAfterTakedBomb error:{1}", (object) ex);
      }
    }

    public void ShootedSay(int delay)
    {
      try
      {
        this.m_ai.OnShootedSay(delay);
      }
      catch (Exception ex)
      {
        SimpleBoss.log.ErrorFormat("SimpleBoss ShootedSay error {0}", (object) ex);
      }
    }
  }
}
