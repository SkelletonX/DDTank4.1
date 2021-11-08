// Decompiled with JetBrains decompiler
// Type: Game.Server.ConsortiaTask.ConsortiaTaskMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base;
using Game.Server.ConsortiaTask.Data;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.ConsortiaTask
{
  public class ConsortiaTaskMgr
  {
    private static object object_0 = new object();
    private static readonly int[] int_0 = GameProperties.MissionAwardRichesArr();
    private static readonly int[] int_1 = GameProperties.MissionAwardGPArr();
    private static readonly int[] int_2 = GameProperties.MissionAwardOfferArr();
    private static readonly int[] int_3 = GameProperties.MissionRichesArr();
    private static Dictionary<int, ConsortiaTaskInfo> dictionary_0;
    private static Dictionary<int, BaseConsortiaTask> dictionary_1;
    private static Dictionary<int, ConsortiaTaskUserTempInfo> dictionary_2;
    private static ThreadSafeRandom threadSafeRandom_0;

    public static bool Init()
    {
      ConsortiaTaskMgr.dictionary_0 = new Dictionary<int, ConsortiaTaskInfo>();
      ConsortiaTaskMgr.dictionary_1 = new Dictionary<int, BaseConsortiaTask>();
      ConsortiaTaskMgr.dictionary_2 = new Dictionary<int, ConsortiaTaskUserTempInfo>();
      ConsortiaTaskMgr.threadSafeRandom_0 = new ThreadSafeRandom();
      ConsortiaTaskMgr.smethod_0();
      ConsortiaTaskMgr.smethod_1();
      return true;
    }

    public static void ScanConsortiaTask()
    {
      lock (ConsortiaTaskMgr.object_0)
      {
        foreach (BaseConsortiaTask baseConsortiaTask in ConsortiaTaskMgr.GetAllConsortiaTaskData())
        {
          if (baseConsortiaTask.Info.CanRemove || baseConsortiaTask.Finish())
          {
            baseConsortiaTask.ClearAllUserData();
            ConsortiaTaskMgr.RemoveConsortiaTask(baseConsortiaTask.Info.ConsortiaID);
          }
        }
      }
    }

    public static bool AddConsortiaTask(int consortiaId, int level)
    {
      bool flag = false;
      BaseConsortiaTask baseConsortiaTask1 = ConsortiaTaskMgr.GetSingleConsortiaTask(consortiaId);
      if (baseConsortiaTask1 != null && baseConsortiaTask1.Info.CanRemove)
      {
        ConsortiaTaskMgr.RemoveConsortiaTask(consortiaId);
        baseConsortiaTask1 = (BaseConsortiaTask) null;
      }
      if (baseConsortiaTask1 == null)
      {
        BaseConsortiaTask baseConsortiaTask2 = new BaseConsortiaTask(new ConsortiaTaskDataInfo(consortiaId, ConsortiaTaskMgr.int_1[level - 1], ConsortiaTaskMgr.int_0[level - 1], ConsortiaTaskMgr.int_2[level - 1], ConsortiaTaskMgr.GetRandomConsortiaBuff(level).id, GameProperties.MissionMinute), ConsortiaTaskMgr.GetRandomTaskCondition(4, 3));
        lock (ConsortiaTaskMgr.dictionary_1)
          ConsortiaTaskMgr.dictionary_1.Add(consortiaId, baseConsortiaTask2);
        flag = true;
      }
      return flag;
    }

    public static void AddConsortiaTask(BaseConsortiaTask taskBase)
    {
      lock (ConsortiaTaskMgr.dictionary_1)
        ConsortiaTaskMgr.dictionary_1.Add(taskBase.Info.ConsortiaID, taskBase);
    }

    public static bool ActiveTask(int consortiaId)
    {
      BaseConsortiaTask singleConsortiaTask = ConsortiaTaskMgr.GetSingleConsortiaTask(consortiaId);
      if (singleConsortiaTask == null || singleConsortiaTask.Info.IsActive)
        return false;
      singleConsortiaTask.Info.IsActive = true;
      singleConsortiaTask.Info.StartTime = DateTime.Now;
      foreach (GamePlayer playersWithConsortium in WorldMgr.GetAllPlayersWithConsortia(consortiaId))
      {
        if (playersWithConsortium.IsActive)
          singleConsortiaTask.AddToPlayer(playersWithConsortium);
      }
      singleConsortiaTask.SendSystemConsortiaChat(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg4"));
      return true;
    }

    public static bool AddConsortiaTaskUserTemp(ConsortiaTaskUserTempInfo temp)
    {
      bool flag = false;
      lock (ConsortiaTaskMgr.dictionary_2)
      {
        if (!ConsortiaTaskMgr.dictionary_2.ContainsKey(temp.UserID))
        {
          ConsortiaTaskMgr.dictionary_2.Add(temp.UserID, temp);
          flag = true;
        }
      }
      return flag;
    }

    public static bool CheckConsortiaTaskUserTemp(GamePlayer player)
    {
      bool flag = false;
      ConsortiaTaskUserTempInfo taskUserTempInfo = (ConsortiaTaskUserTempInfo) null;
      lock (ConsortiaTaskMgr.dictionary_2)
      {
        if (ConsortiaTaskMgr.dictionary_2.ContainsKey(player.PlayerId))
          taskUserTempInfo = ConsortiaTaskMgr.dictionary_2[player.PlayerId];
      }
      if (taskUserTempInfo != null)
      {
        player.AddGP(taskUserTempInfo.Exp);
        player.AddOffer(taskUserTempInfo.Offer);
        player.SendMessage(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg3", (object) taskUserTempInfo.Total, (object) taskUserTempInfo.Exp, (object) taskUserTempInfo.Offer));
        flag = true;
        lock (ConsortiaTaskMgr.dictionary_2)
          ConsortiaTaskMgr.dictionary_2.Remove(taskUserTempInfo.UserID);
      }
      return flag;
    }

    public static List<ConsortiaTaskInfo> GetRandomTaskCondition(
      int level,
      int total)
    {
      List<ConsortiaTaskInfo> consortiaTaskInfoList = new List<ConsortiaTaskInfo>();
      List<ConsortiaTaskInfo> allConditionInfo = ConsortiaTaskMgr.GetAllConditionInfo(level);
      for (int index1 = 0; index1 < total; ++index1)
      {
        int index2 = ConsortiaTaskMgr.threadSafeRandom_0.Next(allConditionInfo.Count);
        if (index2 < allConditionInfo.Count)
        {
          consortiaTaskInfoList.Add(allConditionInfo[index2]);
          allConditionInfo.RemoveAt(index2);
        }
      }
      return consortiaTaskInfoList;
    }

    public static ConsortiaBuffTempInfo GetRandomConsortiaBuff(int level)
    {
      List<ConsortiaBuffTempInfo> allConsortiaBuff = ConsortiaExtraMgr.GetAllConsortiaBuff(level, 1);
      int index = ConsortiaTaskMgr.threadSafeRandom_0.Next(allConsortiaBuff.Count);
      return allConsortiaBuff[index];
    }

    public static List<ConsortiaTaskInfo> GetAllConditionInfo(int level)
    {
      List<ConsortiaTaskInfo> consortiaTaskInfoList = new List<ConsortiaTaskInfo>();
      lock (ConsortiaTaskMgr.dictionary_0)
      {
        foreach (ConsortiaTaskInfo consortiaTaskInfo in ConsortiaTaskMgr.dictionary_0.Values)
        {
          if (consortiaTaskInfo.Level == level)
            consortiaTaskInfoList.Add(consortiaTaskInfo);
        }
      }
      return consortiaTaskInfoList;
    }

    public static List<BaseConsortiaTask> GetAllConsortiaTaskData()
    {
      List<BaseConsortiaTask> baseConsortiaTaskList = new List<BaseConsortiaTask>();
      lock (ConsortiaTaskMgr.dictionary_1)
      {
        foreach (BaseConsortiaTask baseConsortiaTask in ConsortiaTaskMgr.dictionary_1.Values)
          baseConsortiaTaskList.Add(baseConsortiaTask);
      }
      return baseConsortiaTaskList;
    }

    public static BaseConsortiaTask GetSingleConsortiaTask(int consortiaId)
    {
      BaseConsortiaTask baseConsortiaTask = (BaseConsortiaTask) null;
      lock (ConsortiaTaskMgr.dictionary_1)
      {
        if (ConsortiaTaskMgr.dictionary_1.ContainsKey(consortiaId))
          baseConsortiaTask = ConsortiaTaskMgr.dictionary_1[consortiaId];
      }
      return baseConsortiaTask;
    }

    public static void RemoveConsortiaTask(int consortiaId)
    {
      lock (ConsortiaTaskMgr.dictionary_1)
      {
        if (!ConsortiaTaskMgr.dictionary_1.ContainsKey(consortiaId))
          return;
        ConsortiaTaskMgr.dictionary_1.Remove(consortiaId);
      }
    }

    public static void AddPlayer(GamePlayer player)
    {
      ConsortiaTaskMgr.GetSingleConsortiaTask(player.PlayerCharacter.ConsortiaID)?.AddToPlayer(player);
    }

    public static void RemovePlayer(GamePlayer player)
    {
      ConsortiaTaskMgr.GetSingleConsortiaTask(player.PlayerCharacter.ConsortiaID)?.RemoveToPlayer(player);
    }

    private static void smethod_0()
    {
      ConsortiaTaskMgr.dictionary_0.Clear();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (ConsortiaTaskInfo consortiaTaskInfo in produceBussiness.GetAllConsortiaTask())
        {
          if (!ConsortiaTaskMgr.dictionary_0.ContainsKey(consortiaTaskInfo.ID))
            ConsortiaTaskMgr.dictionary_0.Add(consortiaTaskInfo.ID, consortiaTaskInfo);
        }
      }
    }

    private static void smethod_1()
    {
      ConsortiaTaskMgrProtobuf consortiaTaskMgrProtobuf = Marshal.LoadDataFile<ConsortiaTaskMgrProtobuf>("consortiatask", true);
      if (consortiaTaskMgrProtobuf == null || consortiaTaskMgrProtobuf.tempUsers == null)
        return;
      foreach (ConsortiaTaskUserTempInfo tempUser in consortiaTaskMgrProtobuf.tempUsers)
        ConsortiaTaskMgr.AddConsortiaTaskUserTemp(tempUser);
    }

    private static void smethod_2()
    {
      Marshal.SaveDataFile<ConsortiaTaskMgrProtobuf>(new ConsortiaTaskMgrProtobuf()
      {
        tempUsers = ConsortiaTaskMgr.dictionary_2.Values.ToList<ConsortiaTaskUserTempInfo>()
      }, "consortiatask", true);
    }

    public static void Stop()
    {
      ConsortiaTaskMgr.ScanConsortiaTask();
      ConsortiaTaskMgr.smethod_2();
    }
  }
}
