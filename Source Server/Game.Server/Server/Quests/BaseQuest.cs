// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.BaseQuest
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Quests
{
  public class BaseQuest
  {
    private QuestDataInfo m_data;
    private QuestInfo m_info;
    private List<BaseCondition> m_list;
    private DateTime m_oldFinishDate;
    private GamePlayer m_player;

    public BaseQuest(QuestInfo info, QuestDataInfo data)
    {
      this.m_info = info;
      this.m_data = data;
      this.m_data.QuestID = this.m_info.ID;
      if(info.RepeatMax >1)
      {
        this.m_data.RepeatFinish = this.m_info.RepeatMax;
      }
      this.m_list = new List<BaseCondition>();
      List<QuestConditionInfo> questCondiction = QuestMgr.GetQuestCondiction(info);
      int num = 0;
      foreach (QuestConditionInfo info1 in questCondiction)
      {
        BaseCondition condition = BaseCondition.CreateCondition(this, info1, data.GetConditionValue(num++));
        if (condition != null)
          this.m_list.Add(condition);
      }
    }

    public void AddToPlayer(GamePlayer player)
    {
      this.m_player = player;
      if (this.m_data.IsComplete)
        return;
      this.AddTrigger(player);
    }

    private void AddTrigger(GamePlayer player)
    {
      foreach (BaseCondition baseCondition in this.m_list)
        baseCondition.AddTrigger(player);
    }

    public bool CancelFinish(GamePlayer player)
    {
      this.m_data.IsComplete = false;
      this.m_data.CompletedDate = this.m_oldFinishDate;
      foreach (BaseCondition baseCondition in this.m_list)
        baseCondition.CancelFinish(player);
      return true;
    }

    public bool CanCompleted(GamePlayer player)
    {
      if (this.m_data.IsComplete)
        return false;
      int notMustCount = this.m_info.NotMustCount;
      foreach (BaseCondition baseCondition in this.m_list)
      {
        if (!baseCondition.IsCompleted(player) && this.m_data.QuestID != 70)
        {
          if (!baseCondition.Info.isOpitional)
            return false;
        }
        else
          --notMustCount;
      }
      return notMustCount <= 0;
    }

    public bool Finish(GamePlayer player)
    {
      if (!this.CanCompleted(player))
        return false;
      foreach (BaseCondition baseCondition in this.m_list)
      {
        if (!baseCondition.Finish(player))
          return false;
      }
      if (!this.Info.CanRepeat)
      {
        this.m_data.IsComplete = true;
        this.RemveTrigger(player);
      }
      this.m_oldFinishDate = this.m_data.CompletedDate;
      this.m_data.CompletedDate = DateTime.Now;
      return true;
    }

    public BaseCondition GetConditionById(int id)
    {
      foreach (BaseCondition baseCondition in this.m_list)
      {
        if (baseCondition.Info.CondictionID == id)
          return baseCondition;
      }
      return (BaseCondition) null;
    }

    public void RemoveFromPlayer(GamePlayer player)
    {
      if (!this.m_data.IsComplete)
        this.RemveTrigger(player);
      this.m_player = (GamePlayer) null;
    }

    private void RemveTrigger(GamePlayer player)
    {
      foreach (BaseCondition baseCondition in this.m_list)
        baseCondition.RemoveTrigger(player);
    }

    public void Reset(GamePlayer player)
    {
      foreach (BaseCondition baseCondition in this.m_list)
        baseCondition.Reset(player);
    }

    public void Reset(GamePlayer player, int rand)
    {
      BaseQuest quest = player.QuestInventory.FindQuest(this.m_info.ID);
      this.m_data.QuestID = this.m_info.ID;
      this.m_data.UserID = player.PlayerId;
      this.m_data.IsComplete = false;
      this.m_data.IsExist = true;
      if (this.m_data.CompletedDate == DateTime.MinValue)
        this.m_data.CompletedDate = DateTime.Now;
      if ((DateTime.Now - this.m_data.CompletedDate).TotalDays >= (double) this.m_info.RepeatInterval && (quest.m_oldFinishDate - DateTime.Now).TotalDays >= 1)
        this.m_data.RepeatFinish = this.m_info.RepeatMax;
      if (!this.m_info.CanRepeat || this.m_info.RepeatMax > 1)
        --this.m_data.RepeatFinish;
      else if(this.m_info.RepeatMax <= 1)
        this.m_data.RepeatFinish = this.m_info.RepeatMax;
      this.m_data.RandDobule = rand;
      if (!this.m_info.CanRepeat || quest.Data!= null)
      {
        foreach (BaseCondition baseCondition in this.m_list)
          baseCondition.Reset(player);
      }
      this.SaveData();
    }

    public void SaveData()
    {
      int num = 0;
      foreach (BaseCondition baseCondition in this.m_list)
        this.m_data.SaveConditionValue(num++, baseCondition.Value);
    }

    public void Update()
    {
      this.SaveData();
      if (!this.m_data.IsDirty || this.m_player == null)
        return;
      this.m_player.QuestInventory.Update(this);
    }

    public QuestDataInfo Data
    {
      get
      {
        return this.m_data;
      }
    }

    public QuestInfo Info
    {
      get
      {
        return this.m_info;
      }
    }
  }
}
