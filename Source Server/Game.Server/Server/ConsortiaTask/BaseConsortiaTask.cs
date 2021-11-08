// Decompiled with JetBrains decompiler
// Type: Game.Server.ConsortiaTask.BaseConsortiaTask
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.ConsortiaTask.Data;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.ConsortiaTask
{
  public class BaseConsortiaTask
  {
    private ConsortiaTaskDataInfo consortiaTaskDataInfo_0;
    private Dictionary<int, ConsortiaTaskUserDataInfo> dictionary_0;
    private Dictionary<int, ConsortiaTaskInfo> dictionary_1;
    private object object_0;

    public Dictionary<int, ConsortiaTaskInfo> ConditionList
    {
      get
      {
        return this.dictionary_1;
      }
    }

    public ConsortiaTaskDataInfo Info
    {
      get
      {
        return this.consortiaTaskDataInfo_0;
      }
    }

    public Dictionary<int, ConsortiaTaskUserDataInfo> ListUsers
    {
      get
      {
        return this.dictionary_0;
      }
    }

    public BaseConsortiaTask(ConsortiaTaskDataInfo info, List<ConsortiaTaskInfo> listCondition)
    {
      this.object_0 = new object();
      this.dictionary_0 = new Dictionary<int, ConsortiaTaskUserDataInfo>();
      this.dictionary_1 = new Dictionary<int, ConsortiaTaskInfo>();
      this.consortiaTaskDataInfo_0 = info;
      int key = 0;
      foreach (ConsortiaTaskInfo consortiaTaskInfo in listCondition)
      {
        this.dictionary_1.Add(key, consortiaTaskInfo);
        ++key;
      }
    }

    private void method_0(
      ConsortiaTaskUserDataInfo consortiaTaskUserDataInfo_0)
    {
      foreach (KeyValuePair<int, ConsortiaTaskInfo> keyValuePair in this.dictionary_1)
      {
        BaseConsortiaTaskCondition condition = BaseConsortiaTaskCondition.CreateCondition(consortiaTaskUserDataInfo_0, this, keyValuePair.Value, consortiaTaskUserDataInfo_0.GetConditionValue(keyValuePair.Key));
        if (condition != null)
        {
          condition.AddTrigger(consortiaTaskUserDataInfo_0);
          consortiaTaskUserDataInfo_0.ConditionList.Add(condition);
        }
      }
    }

    private void method_1(
      ConsortiaTaskUserDataInfo consortiaTaskUserDataInfo_0)
    {
      foreach (BaseConsortiaTaskCondition condition in consortiaTaskUserDataInfo_0.ConditionList)
        condition.RemoveTrigger(consortiaTaskUserDataInfo_0);
    }

    public ConsortiaTaskInfo GetPlaceCondtion(int conditionId, ref int place)
    {
      ConsortiaTaskInfo consortiaTaskInfo = (ConsortiaTaskInfo) null;
      foreach (KeyValuePair<int, ConsortiaTaskInfo> keyValuePair in this.dictionary_1)
      {
        if (keyValuePair.Value.ID == conditionId)
        {
          place = keyValuePair.Key;
          consortiaTaskInfo = keyValuePair.Value;
          break;
        }
      }
      return consortiaTaskInfo;
    }

    public int GetTotalValueByConditionPlace(int place)
    {
      switch (place)
      {
        case 0:
          return this.consortiaTaskDataInfo_0.Condition1;
        case 1:
          return this.consortiaTaskDataInfo_0.Condition2;
        case 2:
          return this.consortiaTaskDataInfo_0.Condition3;
        default:
          return 0;
      }
    }

    public int GetValueByConditionPlace(int userid, int place)
    {
      ConsortiaTaskUserDataInfo singlePlayer = this.GetSinglePlayer(userid);
      if (singlePlayer == null)
        return 0;
      switch (place)
      {
        case 0:
          return singlePlayer.Condition1;
        case 1:
          return singlePlayer.Condition2;
        case 2:
          return singlePlayer.Condition3;
        default:
          return 0;
      }
    }

    public void SaveData(ConsortiaTaskUserDataInfo player)
    {
      int index = 0;
      foreach (BaseConsortiaTaskCondition condition in player.ConditionList)
      {
        player.SaveConditionValue(index, condition.Value);
        ++index;
      }
    }

    public void RemakeValue(int conditionId, ref int valueAdd)
    {
      int place = -1;
      ConsortiaTaskInfo placeCondtion = this.GetPlaceCondtion(conditionId, ref place);
      lock (this.object_0)
      {
        if (placeCondtion != null && this.consortiaTaskDataInfo_0.IsVaildDate() && !this.consortiaTaskDataInfo_0.IsComplete)
        {
          switch (place)
          {
            case 0:
              if (this.consortiaTaskDataInfo_0.Condition1 + valueAdd <= placeCondtion.Para2)
                break;
              valueAdd = placeCondtion.Para2 - this.consortiaTaskDataInfo_0.Condition1;
              break;
            case 1:
              if (this.consortiaTaskDataInfo_0.Condition2 + valueAdd <= placeCondtion.Para2)
                break;
              valueAdd = placeCondtion.Para2 - this.consortiaTaskDataInfo_0.Condition2;
              break;
            case 2:
              if (this.consortiaTaskDataInfo_0.Condition3 + valueAdd <= placeCondtion.Para2)
                break;
              valueAdd = placeCondtion.Para2 - this.consortiaTaskDataInfo_0.Condition3;
              break;
          }
        }
        else
          valueAdd = 0;
      }
    }

    public void Update(ConsortiaTaskUserDataInfo player)
    {
      this.SaveData(player);
      this.UpdateTotalCondition();
      this.Finish();
    }

    public void UpdateTotalCondition()
    {
      List<ConsortiaTaskUserDataInfo> allPlayers = this.GetAllPlayers();
      lock (this.object_0)
      {
        this.consortiaTaskDataInfo_0.Condition1 = 0;
        this.consortiaTaskDataInfo_0.Condition2 = 0;
        this.consortiaTaskDataInfo_0.Condition3 = 0;
        foreach (ConsortiaTaskUserDataInfo taskUserDataInfo in allPlayers)
        {
          this.consortiaTaskDataInfo_0.Condition1 += taskUserDataInfo.Condition1;
          this.consortiaTaskDataInfo_0.Condition2 += taskUserDataInfo.Condition2;
          this.consortiaTaskDataInfo_0.Condition3 += taskUserDataInfo.Condition3;
        }
      }
    }

    public bool CanCompleted()
    {
      if (!this.consortiaTaskDataInfo_0.IsActive)
        return false;
      lock (this.object_0)
      {
        foreach (KeyValuePair<int, ConsortiaTaskInfo> keyValuePair in this.dictionary_1)
        {
          switch (keyValuePair.Key)
          {
            case 0:
              if (keyValuePair.Value.Para2 > this.consortiaTaskDataInfo_0.Condition1)
                return false;
              continue;
            case 1:
              if (keyValuePair.Value.Para2 > this.consortiaTaskDataInfo_0.Condition2)
                return false;
              continue;
            case 2:
              if (keyValuePair.Value.Para2 > this.consortiaTaskDataInfo_0.Condition3)
                return false;
              continue;
            default:
              continue;
          }
        }
        return true;
      }
    }

    public bool Finish()
    {
      if (this.consortiaTaskDataInfo_0.IsVaildDate() && this.CanCompleted())
      {
        this.DisableTriggle();
        this.consortiaTaskDataInfo_0.IsComplete = true;
        List<ConsortiaTaskUserDataInfo> taskUserDataInfoList = new List<ConsortiaTaskUserDataInfo>();
        lock (this.dictionary_0)
          taskUserDataInfoList = this.dictionary_0.Values.OrderByDescending<ConsortiaTaskUserDataInfo, int>((Func<ConsortiaTaskUserDataInfo, int>) (a => a.GetTotalConditionCompleted())).ToList<ConsortiaTaskUserDataInfo>();
        int conditionCompleted = this.consortiaTaskDataInfo_0.GetTotalConditionCompleted();
        if (conditionCompleted > 0)
        {
          using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
          {
            foreach (ConsortiaTaskUserDataInfo taskUserDataInfo in taskUserDataInfoList)
            {
              float num1 = (float) ((double) taskUserDataInfo.GetTotalConditionCompleted() / (double) conditionCompleted * 100.0);
              if ((double) num1 > 0.0)
              {
                int gp = (int) Math.Floor((double) (this.consortiaTaskDataInfo_0.TotalExp / 100) * (double) num1);
                int num2 = (int) Math.Floor((double) (this.consortiaTaskDataInfo_0.TotalOffer / 100) * (double) num1);
                int riches = (int) Math.Floor((double) (this.consortiaTaskDataInfo_0.TotalRiches / 100) * (double) num1);
                if (taskUserDataInfo.Player != null && taskUserDataInfo.Player.IsActive)
                {
                  taskUserDataInfo.Player.AddGP(gp);
                  taskUserDataInfo.Player.AddOffer(num2);
                  consortiaBussiness.ConsortiaRichAdd(taskUserDataInfo.Player.PlayerCharacter.ConsortiaID, ref riches);
                  taskUserDataInfo.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg3", (object) num1, (object) gp, (object) num2));
                }
                else
                  ConsortiaTaskMgr.AddConsortiaTaskUserTemp(new ConsortiaTaskUserTempInfo()
                  {
                    Total = (int) num1,
                    Exp = gp,
                    Offer = num2
                  });
              }
            }
            if (this.consortiaTaskDataInfo_0.BuffID > 0)
            {
              ConsortiaBuffTempInfo consortiaBuffInfo = ConsortiaExtraMgr.FindConsortiaBuffInfo(this.consortiaTaskDataInfo_0.BuffID);
              if (consortiaBuffInfo != null)
                ConsortiaMgr.AddBuffConsortia((GamePlayer) null, consortiaBuffInfo, this.consortiaTaskDataInfo_0.ConsortiaID, this.consortiaTaskDataInfo_0.BuffID, 1440);
            }
          }
        }
        this.SendSystemConsortiaChat(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg2"));
        this.consortiaTaskDataInfo_0.CanRemove = true;
        return true;
      }
      if (this.consortiaTaskDataInfo_0.IsVaildDate() || !this.consortiaTaskDataInfo_0.IsActive)
        return false;
      this.SendSystemConsortiaChat(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg1"));
      this.DisableTriggle();
      this.consortiaTaskDataInfo_0.IsComplete = true;
      this.consortiaTaskDataInfo_0.CanRemove = true;
      return true;
    }

    public void AddToPlayer(GamePlayer player)
    {
      if (!this.CheckCanAddPlayer(player.PlayerId))
        return;
      ConsortiaTaskUserDataInfo consortiaTaskUserDataInfo_0 = this.GetSinglePlayer(player.PlayerId);
      if (consortiaTaskUserDataInfo_0 == null)
      {
        consortiaTaskUserDataInfo_0 = new ConsortiaTaskUserDataInfo();
        consortiaTaskUserDataInfo_0.UserID = player.PlayerId;
        lock (this.dictionary_0)
          this.dictionary_0.Add(player.PlayerId, consortiaTaskUserDataInfo_0);
      }
      if (consortiaTaskUserDataInfo_0.Player == null)
        consortiaTaskUserDataInfo_0.Player = player;
      consortiaTaskUserDataInfo_0.ConditionList = new List<BaseConsortiaTaskCondition>();
      this.method_0(consortiaTaskUserDataInfo_0);
    }

    public void RemoveToPlayer(GamePlayer player)
    {
      ConsortiaTaskUserDataInfo singlePlayer = this.GetSinglePlayer(player.PlayerId);
      if (singlePlayer == null || singlePlayer.ConditionList == null)
        return;
      this.method_1(singlePlayer);
      singlePlayer.Player = (GamePlayer) null;
      singlePlayer.ConditionList = (List<BaseConsortiaTaskCondition>) null;
    }

    public bool CheckCanAddPlayer(int userid)
    {
      bool flag = false;
      if (this.consortiaTaskDataInfo_0.IsActive && !this.consortiaTaskDataInfo_0.IsComplete && this.consortiaTaskDataInfo_0.IsVaildDate())
      {
        lock (this.dictionary_0)
        {
          if (this.dictionary_0.ContainsKey(userid))
          {
            if (this.dictionary_0[userid].Player != null)
              goto label_8;
          }
          flag = true;
        }
      }
label_8:
      return flag;
    }

    public ConsortiaTaskUserDataInfo GetSinglePlayer(int userId)
    {
      ConsortiaTaskUserDataInfo taskUserDataInfo = (ConsortiaTaskUserDataInfo) null;
      lock (this.dictionary_0)
      {
        if (this.dictionary_0.ContainsKey(userId))
          taskUserDataInfo = this.dictionary_0[userId];
      }
      return taskUserDataInfo;
    }

    public List<ConsortiaTaskUserDataInfo> GetAllPlayers()
    {
      List<ConsortiaTaskUserDataInfo> taskUserDataInfoList = new List<ConsortiaTaskUserDataInfo>();
      lock (this.dictionary_0)
      {
        foreach (ConsortiaTaskUserDataInfo taskUserDataInfo in this.dictionary_0.Values)
          taskUserDataInfoList.Add(taskUserDataInfo);
      }
      return taskUserDataInfoList;
    }

    public void DisableTriggle()
    {
      lock (this.dictionary_0)
      {
        foreach (ConsortiaTaskUserDataInfo consortiaTaskUserDataInfo_0 in this.dictionary_0.Values)
        {
          if (consortiaTaskUserDataInfo_0.Player != null && consortiaTaskUserDataInfo_0.ConditionList != null)
            this.method_1(consortiaTaskUserDataInfo_0);
        }
      }
    }

    public void ClearAllUserData()
    {
      this.DisableTriggle();
      lock (this.dictionary_0)
        this.dictionary_0.Clear();
    }

    public void SendSystemConsortiaChat(string content)
    {
      foreach (GamePlayer playersWithConsortium in WorldMgr.GetAllPlayersWithConsortia(this.consortiaTaskDataInfo_0.ConsortiaID))
        playersWithConsortium.Out.SendSystemConsortiaChat(content, true);
    }

    public void SendToAll(GSPacketIn pkg, GamePlayer ext)
    {
      foreach (GamePlayer playersWithConsortium in WorldMgr.GetAllPlayersWithConsortia(this.consortiaTaskDataInfo_0.ConsortiaID))
      {
        if (playersWithConsortium.IsActive && playersWithConsortium != ext)
          playersWithConsortium.SendTCP(pkg);
      }
    }
  }
}
