// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.QuestInventoryOld
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Quests
{
  public class QuestInventoryOld
  {
    private Dictionary<int, QuestDataInfo> _currentQuest;
    private object _lock;
    private GamePlayer _player;

    public QuestInventoryOld(GamePlayer player)
    {
      this._player = player;
      this._lock = new object();
      this._currentQuest = new Dictionary<int, QuestDataInfo>();
    }

    public bool AddQuest(int questID, out string msg)
    {
      QuestMgr.GetSingleQuest(questID);
      msg = "未开始";
      return false;
    }

    public bool ClearConsortiaQuest()
    {
      if ((uint) this._player.PlayerCharacter.ConsortiaID > 0U)
        return false;
      lock (this._lock)
      {
        foreach (QuestDataInfo questDataInfo in this._currentQuest.Values)
          ;
      }
      return true;
    }

    public bool ClearMarryQuest()
    {
      return true;
    }

    public void CheckClient(int questID, int count)
    {
    }

    public void CheckCompose(int itemID)
    {
    }

    public void CheckKillPlayer(
      int map,
      int fightMode,
      int timeMode,
      bool captain,
      int killLevel,
      int selfCount,
      int rivalCount,
      int relation,
      int roomType)
    {
    }

    public void CheckStrengthen(int strengthenLevel, int categoryID)
    {
    }

    public void CheckUseItem(int itemID)
    {
    }

    public void CheckWin(
      int map,
      int fightMode,
      int timeMode,
      bool captain,
      int selfCount,
      int rivalCount,
      bool isWin,
      bool isFightConsortia,
      int roomType,
      bool isMarry)
    {
    }

    public bool FinishQuest(int questID)
    {
      return true;
    }

    public QuestDataInfo[] GetALlQuest()
    {
      QuestDataInfo[] array = (QuestDataInfo[]) null;
      lock (this._lock)
        this._currentQuest.Values.CopyTo(array, 0);
      return array ?? new QuestDataInfo[0];
    }

    public QuestDataInfo GetCurrentQuest(int questID)
    {
      lock (this._lock)
      {
        if (this._currentQuest.ContainsKey(questID))
          return this._currentQuest[questID];
      }
      return (QuestDataInfo) null;
    }

    public QuestDataInfo GetCurrentQuest(int questID, bool isExist)
    {
      lock (this._lock)
      {
        if (this._currentQuest.ContainsKey(questID))
        {
          if (this._currentQuest[questID].IsExist == isExist)
            return this._currentQuest[questID];
        }
      }
      return (QuestDataInfo) null;
    }

    public int GetQuestCount()
    {
      int num = 0;
      lock (this._lock)
      {
        foreach (QuestDataInfo questDataInfo in this._currentQuest.Values)
        {
          if (!questDataInfo.IsComplete && questDataInfo.IsExist)
            ++num;
        }
      }
      return num;
    }

    public Dictionary<int, int> GetRequestItems()
    {
      return new Dictionary<int, int>();
    }

    public void LoadFromDatabase(int playerId)
    {
      lock (this._lock)
      {
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          foreach (QuestDataInfo questDataInfo in playerBussiness.GetUserQuest(playerId))
          {
            if (!this._currentQuest.ContainsKey(questDataInfo.QuestID))
              this._currentQuest.Add(questDataInfo.QuestID, questDataInfo);
          }
        }
      }
      this.ClearConsortiaQuest();
      this.ClearMarryQuest();
    }

    public bool RemoveQuest(int questID)
    {
      QuestDataInfo currentQuest = this.GetCurrentQuest(questID, true);
      if (currentQuest == null || currentQuest.IsComplete)
        return false;
      currentQuest.IsExist = false;
      return true;
    }

    public void SaveToDatabase()
    {
      lock (this._lock)
      {
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          foreach (QuestDataInfo info in this._currentQuest.Values)
          {
            if (info.IsDirty)
              playerBussiness.UpdateDbQuestDataInfo(info);
          }
        }
      }
    }
  }
}
