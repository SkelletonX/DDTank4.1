// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.QuestInventory
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Server.Buffer;
using Game.Server.GameObjects;
using Game.Server.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Game.Server.Quests
{
  public class QuestInventory
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private UnicodeEncoding m_converter = new UnicodeEncoding();
    protected List<BaseQuest> m_changedQuests = new List<BaseQuest>();
    protected ArrayList m_clearList;
    private int m_changeCount;
    protected List<QuestDataInfo> m_datas;
    protected List<BaseQuest> m_list;
    private object m_lock;
    private GamePlayer m_player;
    private byte[] m_states;

    public QuestInventory(GamePlayer player)
    {
      this.m_player = player;
      this.m_lock = new object();
      this.m_list = new List<BaseQuest>();
      this.m_clearList = new ArrayList();
      this.m_datas = new List<QuestDataInfo>();
    }

    private bool AddQuest(BaseQuest quest)
    {
      lock (this.m_list)
        this.m_list.Add(quest);
      this.OnQuestsChanged(quest);
      quest.AddToPlayer(this.m_player);
      return true;
    }

    public bool AddQuest(QuestInfo info, out string msg)
    {
      msg = "";
      try
      {
        if (info == null)
        {
          msg = "Game.Server.Quests.NoQuest";
          return false;
        }
        DateTime now;
        if (info.TimeMode)
        {
          now = DateTime.Now;
          if (now.CompareTo(info.StartDate) < 0)
            msg = "Game.Server.Quests.NoTime";
        }
        if (info.TimeMode)
        {
          now = DateTime.Now;
          if (now.CompareTo(info.EndDate) > 0)
            msg = "Game.Server.Quests.TimeOver";
        }
        if (this.m_player.PlayerCharacter.Grade < info.NeedMinLevel)
          msg = "Game.Server.Quests.LevelLow";
        if (this.m_player.PlayerCharacter.Grade > info.NeedMaxLevel)
          msg = "Game.Server.Quests.LevelTop";
        if (info.PreQuestID != "0,")
        {
          string[] strArray = info.PreQuestID.Split(',');
          for (int index = 0; index < strArray.Length - 1; ++index)
          {
            if (!this.IsQuestFinish(Convert.ToInt32(strArray[index])))
              msg = "Game.Server.Quests.NoFinish";
          }
        }
      }
      catch (Exception ex)
      {
        QuestInventory.log.Info((object) ex.InnerException);
      }
      if (info.IsOther == 1 && !this.m_player.PlayerCharacter.IsConsortia)
        msg = "Game.Server.Quest.QuestInventory.HaveMarry";
      if (info.IsOther == 2 && !this.m_player.PlayerCharacter.IsMarried)
        msg = "Game.Server.Quest.QuestInventory.HaveMarry";
      BaseQuest quest1 = this.FindQuest(info.ID);
      if (quest1 != null && quest1.Data.IsComplete)
        msg = "Game.Server.Quests.Have";
      if (quest1 != null && !quest1.Info.CanRepeat)
        msg = "Game.Server.Quests.NoRepeat";
      if (quest1 != null && (DateTime.Now.CompareTo(quest1.Data.CompletedDate.Date.AddDays((double) quest1.Info.RepeatInterval)) < 0 && quest1.Data.RepeatFinish < 1))
        msg = "Game.Server.Quests.Rest";
      if (this.m_player.QuestInventory.FindQuest(info.ID) != null)
        msg = "Game.Server.Quests.Have";
      if (msg == "")
      {
        QuestMgr.GetQuestCondiction(info);
        int rand = 1;
        if ((Decimal) ThreadSafeRandom.NextStatic(1000000) <= info.Rands)
          rand = info.RandDouble;
        this.BeginChanges();
        if (quest1 == null)
        {
          BaseQuest quest2 = new BaseQuest(info, new QuestDataInfo());
          this.AddQuest(quest2);
          quest2.Reset(this.m_player, rand);
        }
        else
        {
          quest1.Reset(this.m_player, rand);
          quest1.AddToPlayer(this.m_player);
          this.OnQuestsChanged(quest1);
        }
        this.CommitChanges();
        return true;
      }
      msg = LanguageMgr.GetTranslation(msg);
      return false;
    }

    private bool AddQuestData(QuestDataInfo data)
    {
      lock (this.m_list)
        this.m_datas.Add(data);
      return true;
    }

    private void BeginChanges()
    {
      Interlocked.Increment(ref this.m_changeCount);
    }

    public bool ClearConsortiaQuest()
    {
      return true;
    }

    public bool ClearMarryQuest()
    {
      return true;
    }

    private void CommitChanges()
    {
      int num = Interlocked.Decrement(ref this.m_changeCount);
      if (num < 0)
      {
        if (QuestInventory.log.IsErrorEnabled)
          QuestInventory.log.Error((object) ("Inventory changes counter is bellow zero (forgot to use BeginChanges?)!\n\n" + Environment.StackTrace));
        Thread.VolatileWrite(ref this.m_changeCount, 0);
      }
      if (num > 0 || this.m_changedQuests.Count <= 0)
        return;
      this.UpdateChangedQuests();
    }

    public bool FindFinishQuestData(int ID, int UserID)
    {
      bool flag = false;
      lock (this.m_datas)
      {
        foreach (QuestDataInfo data in this.m_datas)
        {
          if (data.QuestID == ID && data.UserID == UserID)
            flag = data.IsComplete;
        }
      }
      return flag;
    }

    public BaseQuest FindQuest(int id)
    {
      foreach (BaseQuest baseQuest in this.m_list)
      {
        if (baseQuest.Info.ID == id)
          return baseQuest;
      }
      return (BaseQuest) null;
    }

    public bool Finish(BaseQuest baseQuest, int selectedItem)
    {
      string str = "";
      QuestInfo info = baseQuest.Info;
      QuestDataInfo data = baseQuest.Data;
      this.m_player.BeginAllChanges();
      try
      {
        if (baseQuest.Finish(this.m_player))
        {
          List<QuestAwardInfo> questGoods = QuestMgr.GetQuestGoods(info);
          List<SqlDataProvider.Data.ItemInfo> itemInfoList1 = new List<SqlDataProvider.Data.ItemInfo>();
          List<SqlDataProvider.Data.ItemInfo> itemInfoList2 = new List<SqlDataProvider.Data.ItemInfo>();
          List<SqlDataProvider.Data.ItemInfo> itemInfoList3 = new List<SqlDataProvider.Data.ItemInfo>();
          List<SqlDataProvider.Data.ItemInfo> items = new List<SqlDataProvider.Data.ItemInfo>();
          foreach (QuestAwardInfo questAwardInfo in questGoods)
          {
            if (!questAwardInfo.IsSelect || questAwardInfo.RewardItemID == selectedItem)
            {
              ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(questAwardInfo.RewardItemID);
              if (itemTemplate != null)
              {
                str = str + LanguageMgr.GetTranslation("Game.Server.Quests.FinishQuest.RewardProp", (object) itemTemplate.Name, (object) questAwardInfo.RewardItemCount) + " ";
                int num = this.m_player.PlayerCharacter.Sex ? 1 : 2;
                if (itemTemplate.NeedSex == 0 || itemTemplate.NeedSex == num)
                {
                  int rewardItemCount = questAwardInfo.RewardItemCount;
                  for (int index = 0; index < rewardItemCount; index += itemTemplate.MaxCount)
                  {
                    int count = index + itemTemplate.MaxCount > questAwardInfo.RewardItemCount ? questAwardInfo.RewardItemCount - index : itemTemplate.MaxCount;
                    SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate, count, 106);
                    if (fromTemplate != null)
                    {
                      fromTemplate.ValidDate = questAwardInfo.RewardItemValid;
                      fromTemplate.IsBinds = true;
                      fromTemplate.StrengthenLevel = questAwardInfo.StrengthenLevel;
                      fromTemplate.AttackCompose = questAwardInfo.AttackCompose;
                      fromTemplate.DefendCompose = questAwardInfo.DefendCompose;
                      fromTemplate.AgilityCompose = questAwardInfo.AgilityCompose;
                      fromTemplate.LuckCompose = questAwardInfo.LuckCompose;
                      if (itemTemplate.BagType == eBageType.PropBag)
                        itemInfoList2.Add(fromTemplate);
                      else
                        itemInfoList1.Add(fromTemplate);
                      if (itemTemplate.TemplateID == 11408)
                      {
                        this.m_player.LoadMedals();
                        this.m_player.OnPlayerAddItem("Medal", count);
                      }
                    }
                  }
                }
              }
            }
          }
          foreach (SqlDataProvider.Data.ItemInfo itemInfo in itemInfoList1)
          {
            if (!this.m_player.EquipBag.StackItemToAnother(itemInfo) && !this.m_player.EquipBag.AddItem(itemInfo))
              items.Add(itemInfo);
          }
          foreach (SqlDataProvider.Data.ItemInfo itemInfo in itemInfoList2)
          {
            if (itemInfo.TemplateID == 11408)
              itemInfo.Count *= data.RandDobule;
            if (itemInfo.Template.CategoryID != 10)
            {
              if (!this.m_player.PropBag.StackItemToAnother(itemInfo) && !this.m_player.PropBag.AddItem(itemInfo))
                items.Add(itemInfo);
            }
            else
            {
              switch (itemInfo.TemplateID)
              {
                case 10001:
                  this.m_player.PlayerCharacter.openFunction(Step.PICK_TWO_TWENTY);
                  continue;
                case 10003:
                  this.m_player.PlayerCharacter.openFunction(Step.POP_WIN);
                  continue;
                case 10004:
                  this.m_player.PlayerCharacter.openFunction(Step.FIFTY_OPEN);
                  this.m_player.AddGift(eGiftType.MONEY);
                  this.m_player.AddGift(eGiftType.BIG_EXP);
                  this.m_player.AddGift(eGiftType.PET_EXP);
                  continue;
                case 10005:
                  this.m_player.PlayerCharacter.openFunction(Step.FORTY_OPEN);
                  continue;
                case 10006:
                  this.m_player.PlayerCharacter.openFunction(Step.THIRTY_OPEN);
                  continue;
                case 10007:
                  this.m_player.PlayerCharacter.openFunction(Step.POP_TWO_TWENTY);
                  this.m_player.AddGift(eGiftType.SMALL_EXP);
                  continue;
                case 10008:
                  this.m_player.PlayerCharacter.openFunction(Step.GAIN_TEN_PERSENT);
                  continue;
                case 10024:
                  this.m_player.PlayerCharacter.openFunction(Step.PICK_ONE);
                  continue;
                case 10025:
                  this.m_player.PlayerCharacter.openFunction(Step.PLANE_OPEN);
                  continue;
                default:
                  continue;
              }
            }
          }
          foreach (SqlDataProvider.Data.ItemInfo itemInfo in itemInfoList3)
          {
            if (!this.m_player.FarmBag.StackItemToAnother(itemInfo) && !this.m_player.FarmBag.AddItem(itemInfo))
              items.Add(itemInfo);
          }
          if (items.Count > 0)
          {
            this.m_player.SendItemsToMail(items, "Mochila cheia", "Recompensas da tarefa", eMailType.ItemOverdue);
            this.m_player.Out.SendMailResponse(this.m_player.PlayerCharacter.ID, eMailRespose.Receiver);
          }
          string message = LanguageMgr.GetTranslation("Game.Server.Quests.FinishQuest.Reward") + str;
          if (info.RewardBuffID > 0 && info.RewardBuffDate > 0)
          {
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(info.RewardBuffID);
            if (itemTemplate != null)
            {
              int ValidHour = info.RewardBuffDate * data.RandDobule;
              BufferList.CreateBufferHour(itemTemplate, ValidHour).Start(this.m_player);
              message = message + LanguageMgr.GetTranslation("Game.Server.Quests.FinishQuest.RewardBuff", (object) itemTemplate.Name, (object) ValidHour) + " ";
            }
          }
          if ((uint) info.RewardGold > 0U)
          {
            int num = info.RewardGold * data.RandDobule;
            this.m_player.AddGold(num);
            message = message + LanguageMgr.GetTranslation("Game.Server.Quests.FinishQuest.RewardGold", (object) num) + " ";
          }
          if ((uint) info.RewardMoney > 0U)
          {
            int num = info.RewardMoney * data.RandDobule;
            this.m_player.AddMoney(info.RewardMoney * data.RandDobule);
            message = message + LanguageMgr.GetTranslation("Game.Server.Quests.FinishQuest.RewardMoney", (object) num) + " ";
          }
          if ((uint) info.RewardGP > 0U)
          {
            int gp = info.RewardGP * data.RandDobule;
            this.m_player.AddGP(gp);
            message = message + LanguageMgr.GetTranslation("Game.Server.Quests.FinishQuest.RewardGB1", (object) gp) + " ";
          }
          if (info.RewardRiches != 0 && (uint) this.m_player.PlayerCharacter.ConsortiaID > 0U)
          {
            int riches = info.RewardRiches * data.RandDobule;
            this.m_player.AddRichesOffer(riches);
            using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
              consortiaBussiness.ConsortiaRichAdd(this.m_player.PlayerCharacter.ConsortiaID, ref riches);
            message = message + LanguageMgr.GetTranslation("Game.Server.Quests.FinishQuest.RewardRiches", (object) riches) + " ";
          }
          if ((uint) info.RewardOffer > 0U)
          {
            int num = info.RewardOffer * data.RandDobule;
            this.m_player.AddOffer(num, false);
            message = message + LanguageMgr.GetTranslation("Game.Server.Quests.FinishQuest.RewardOffer", (object) num) + " ";
          }
          if ((uint) info.RewardGiftToken > 0U)
          {
            int num = info.RewardGiftToken * data.RandDobule;
            this.m_player.AddGiftToken(num);
            message += LanguageMgr.GetTranslation("Game.Server.Quests.FinishQuest.RewardGiftToken", (object) (num.ToString() + " "));
          }
          this.m_player.Out.SendMessage(eMessageType.GM_NOTICE, message);
          this.RemoveQuest(baseQuest);
          this.SetQuestFinish(baseQuest.Info.ID);
          this.m_player.PlayerCharacter.QuestSite = this.m_states;
        }
        this.OnQuestsChanged(baseQuest);
      }
      catch (Exception ex)
      {
        if (QuestInventory.log.IsErrorEnabled)
          QuestInventory.log.Error((object) ("Quest Finish：" + (object) ex));
        return false;
      }
      finally
      {
        this.m_player.CommitAllChanges();
      }
      return true;
    }

    private byte[] InitQuest()
    {
      byte[] numArray = new byte[200];
      for (int index = 0; index < 200; ++index)
        numArray[index] = (byte) 0;
      return numArray;
    }

    private bool IsQuestFinish(int questId)
    {
      if (questId > this.m_states.Length * 8 || questId < 1)
        return false;
      --questId;
      return ((uint) this.m_states[questId / 8] & (uint) (1 << questId % 8)) > 0U;
    }

    public void LoadFromDatabase(int playerId)
    {
      lock (this.m_lock)
      {
        this.m_states = this.m_player.PlayerCharacter.QuestSite.Length == 0 ? this.InitQuest() : this.m_player.PlayerCharacter.QuestSite;
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          QuestDataInfo[] userQuest = playerBussiness.GetUserQuest(playerId);
          this.BeginChanges();
          foreach (QuestDataInfo data in userQuest)
          {
            QuestInfo singleQuest = QuestMgr.GetSingleQuest(data.QuestID);
            if (singleQuest != null)
              this.AddQuest(new BaseQuest(singleQuest, data));
            this.AddQuestData(data);
          }
          this.CommitChanges();
        }
        List<BaseQuest> list = this.m_list;
      }
    }

    public List<QuestDataInfo> GetAllQuestData()
    {
      return this.m_datas;
    }

    protected void OnQuestsChanged(BaseQuest quest)
    {
      if (!this.m_changedQuests.Contains(quest))
        this.m_changedQuests.Add(quest);
      if (this.m_changeCount > 0 || this.m_changedQuests.Count <= 0)
        return;
      this.UpdateChangedQuests();
    }

    public bool RemoveQuest(BaseQuest quest)
    {
      int rand = 1;
      bool flag1;
      if (!quest.Info.CanRepeat)
      {
        bool flag2 = false;
        lock (this.m_list)
        {
          if (this.m_list.Remove(quest))
          {
            this.m_clearList.Add((object) quest);
            flag2 = true;
          }
        }
        if (flag2)
        {
          quest.RemoveFromPlayer(this.m_player);
          this.OnQuestsChanged(quest);
        }
        flag1 = flag2;
      }
      else
      {
        if ((Decimal) ThreadSafeRandom.NextStatic(1000000) <= quest.Info.Rands)
          rand = quest.Info.RandDouble;
        quest.Reset(this.m_player, rand);
        QuestDataInfo data = quest.Data;
        --data.RepeatFinish;
        if (data.RepeatFinish <= 0)
          data.IsComplete = true;
        quest.SaveData();
        this.OnQuestsChanged(quest);
        flag1 = true;
      }
      return flag1;
    }

    public void SaveToDatabase()
    {
      lock (this.m_lock)
      {
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          foreach (BaseQuest baseQuest in this.m_list)
          {
            baseQuest.SaveData();
            if (baseQuest.Data.IsDirty)
              playerBussiness.UpdateDbQuestDataInfo(baseQuest.Data);
          }
          foreach (BaseQuest clear in this.m_clearList)
          {
            clear.SaveData();
            playerBussiness.UpdateDbQuestDataInfo(clear.Data);
          }
          this.m_clearList.Clear();
        }
      }
    }

    private bool SetQuestFinish(int questId)
    {
      if (questId > this.m_states.Length * 8 || questId < 1)
        return false;
      --questId;
      int index = questId / 8;
      int num = questId % 8;
      this.m_states[index] = (byte) ((uint) this.m_states[index] | (uint) (1 << num));
      return true;
    }

    public void Update(BaseQuest quest)
    {
      this.OnQuestsChanged(quest);
    }

    public void UpdateChangedQuests()
    {
      this.m_player.Out.SendUpdateQuests(this.m_player, this.m_states, this.m_changedQuests.ToArray());
      this.m_changedQuests.Clear();
    }

    public bool Restart()
    {
      bool flag = false;
      foreach (QuestDataInfo questDataInfo in this.GetAllQuestData())
      {
        BaseQuest quest = this.FindQuest(questDataInfo.QuestID);
        if (quest != null && quest.Info.CanRepeat && quest.Data.IsComplete)
        {
          List<QuestConditionInfo> questCondiction = QuestMgr.GetQuestCondiction(quest.Info);
          if (questCondiction.Count > 0)
            quest.Data.Condition1 = questCondiction[0].Para2;
          if (questCondiction.Count > 1)
            quest.Data.Condition2 = questCondiction[1].Para2;
          if (questCondiction.Count > 2)
            quest.Data.Condition3 = questCondiction[2].Para2;
          if (questCondiction.Count > 3)
            quest.Data.Condition4 = questCondiction[3].Para2;
          --quest.Data.RepeatFinish;
          quest.Data.IsComplete = false;
          quest.Reset(this.m_player);
          quest.Update();
          this.SaveToDatabase();
          flag = true;
        }
      }
      return flag;
    }
  }
}
