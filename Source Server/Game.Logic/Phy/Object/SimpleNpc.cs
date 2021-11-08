using Game.Logic.AI;
using Game.Logic.AI.Npc;
using Game.Server.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Logic.Phy.Object
{
  public class SimpleNpc : Living
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private NpcInfo m_npcInfo;
    private ABrain m_ai;
    private int m_rank;

    public NpcInfo NpcInfo
    {
      get
      {
        return this.m_npcInfo;
      }
    }

    public int Rank
    {
      get
      {
        return this.m_rank;
      }
    }

    public SimpleNpc(int id, BaseGame game, NpcInfo npcInfo, int type, int direction)
      : base(id, game, npcInfo.Camp, npcInfo.Name, npcInfo.ModelID, npcInfo.Blood, npcInfo.Immunity, direction)
    {
      switch (type)
      {
        case 0:
          this.Type = eLivingType.SimpleNpc;
          break;
        case 1:
          this.Type = eLivingType.ClearEnemy;
          break;
        default:
          this.Type = eLivingType.SimpleNpc;
          break;
      }
      this.m_npcInfo = npcInfo;
      this.m_ai = ScriptMgr.CreateInstance(npcInfo.Script) as ABrain;
      if (this.m_ai == null)
      {
        SimpleNpc.log.ErrorFormat("Can't create abrain :{0}", (object) npcInfo.Script);
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
        SimpleNpc.log.ErrorFormat("SimpleNpc Created error:{1}", (object) ex);
      }
    }

    public SimpleNpc(
      int id,
      BaseGame game,
      NpcInfo npcInfo,
      int type,
      int direction,
      string action)
      : base(id, game, npcInfo.Camp, npcInfo.Name, npcInfo.ModelID, npcInfo.Blood, npcInfo.Immunity, direction)
    {
      if (type == 0)
        this.Type = eLivingType.SimpleNpc;
      else
        this.Type = eLivingType.SimpleNpc1;
      this.m_npcInfo = npcInfo;
      this.ActionStr = action;
      this.m_ai = ScriptMgr.CreateInstance(npcInfo.Script) as ABrain;
      if (this.m_ai == null)
      {
        SimpleNpc.log.ErrorFormat("Can't create abrain :{0}", (object) npcInfo.Script);
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
        SimpleNpc.log.ErrorFormat("SimpleNpc Created error:{1}", (object) ex);
      }
    }

    public SimpleNpc(int id, BaseGame game, NpcInfo npcInfo, int type, int direction, int rank)
      : base(id, game, npcInfo.Camp, npcInfo.Name, npcInfo.ModelID, npcInfo.Blood, npcInfo.Immunity, direction)
    {
      if (type == 0)
        this.Type = eLivingType.SimpleNpc;
      else
        this.Type = eLivingType.SimpleNpc1;
      this.m_npcInfo = npcInfo;
      this.ActionStr = "";
      this.m_ai = ScriptMgr.CreateInstance(npcInfo.Script) as ABrain;
      if (this.m_ai == null)
      {
        SimpleNpc.log.ErrorFormat("Can't create abrain :{0}", (object) npcInfo.Script);
        this.m_ai = (ABrain) SimpleBrain.Simple;
      }
      this.m_ai.Game = this.m_game;
      this.m_ai.Body = (Living) this;
      this.m_rank = rank;
      try
      {
        this.m_ai.OnCreated();
      }
      catch (Exception ex)
      {
        SimpleNpc.log.ErrorFormat("SimpleNpc Created error:{1}", (object) ex);
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
        SimpleNpc.log.ErrorFormat("SimpleNpc BeforeTakeDamage error:{0}", (object) ex);
      }
      base.BeforeTakedDamage(source, ref damageAmount, ref criticalAmount);
    }

    public override void Reset()
    {
      this.Agility = (double) this.m_npcInfo.Agility;
      this.Attack = (double) this.m_npcInfo.Attack;
      this.BaseDamage = (double) this.m_npcInfo.BaseDamage;
      this.BaseGuard = (double) this.m_npcInfo.BaseGuard;
      this.Lucky = (double) this.m_npcInfo.Lucky;
      this.Grade = this.m_npcInfo.Level;
      this.Experience = this.m_npcInfo.Experience;
      this.SetRect(this.m_npcInfo.X, this.m_npcInfo.Y, this.m_npcInfo.Width, this.m_npcInfo.Height);
      this.SetRelateDemagemRect(this.m_npcInfo.X, this.m_npcInfo.Y, this.m_npcInfo.Width, this.m_npcInfo.Height);
      base.Reset();
    }

    public void GetDropItemInfo()
    {
      if (!(this.m_game.CurrentLiving is Player))
        return;
      Player currentLiving = this.m_game.CurrentLiving as Player;
      List<SqlDataProvider.Data.ItemInfo> info1 = (List<SqlDataProvider.Data.ItemInfo>) null;
      int gold = 0;
      int money = 0;
      int giftToken = 0;
      DropInventory.NPCDrop(this.m_npcInfo.DropId, ref info1);
      if (info1 == null)
        return;
      foreach (SqlDataProvider.Data.ItemInfo info2 in info1)
      {
        SqlDataProvider.Data.ItemInfo specialItemInfo = SqlDataProvider.Data.ItemInfo.FindSpecialItemInfo(info2, ref gold, ref money, ref giftToken);
        if (specialItemInfo != null)
        {
          if (specialItemInfo.Template.CategoryID == 10)
            currentLiving.PlayerDetail.AddTemplate(specialItemInfo, eBageType.FightBag, info2.Count, eGameView.dungeonTypeGet);
          else
            currentLiving.PlayerDetail.AddTemplate(specialItemInfo, eBageType.TempBag, info2.Count, eGameView.dungeonTypeGet);
        }
      }
      currentLiving.PlayerDetail.AddGold(gold);
      currentLiving.PlayerDetail.AddMoney(money);
      currentLiving.PlayerDetail.AddGiftToken(giftToken);
    }

    public override void Die()
    {
      this.GetDropItemInfo();
      base.Die();
    }

    public override void Die(int delay)
    {
      this.GetDropItemInfo();
      base.Die(delay);
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
        SimpleNpc.log.ErrorFormat("SimpleNpc BeginNewTurn error:{0}", (object) ex);
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
        SimpleNpc.log.ErrorFormat("SimpleNpc StartAttacking error:{0}", (object) ex);
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
        SimpleNpc.log.ErrorFormat("SimpleNpc Dispose error:{0}", (object) ex);
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
        SimpleNpc.log.ErrorFormat("SimpleNpc DiedSay error {0}", (object) ex);
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
        SimpleNpc.log.ErrorFormat("SimpleNpc DiedEvent error {0}", (object) ex);
      }
    }

    public void OnDie()
    {
      try
      {
        this.m_ai.OnDie();
      }
      catch (Exception ex)
      {
        SimpleNpc.log.ErrorFormat("SimpleNpc OnDie error {0}", (object) ex);
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
        SimpleNpc.log.ErrorFormat("SimpleBoss OnAfterTakedBomb error:{1}", (object) ex);
      }
    }

    public override void OnAfterTakedFrozen()
    {
      try
      {
        this.m_ai.OnAfterTakedFrozen();
      }
      catch (Exception ex)
      {
        SimpleNpc.log.ErrorFormat("SimpleBoss OnAfterTakedFrozen error:{1}", (object) ex);
      }
    }
  }
}
