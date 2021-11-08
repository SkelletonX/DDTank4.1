// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.TurnPropertyCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Logic;
using Game.Server.GameObjects;
using Game.Server.Statics;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Quests
{
  public class TurnPropertyCondition : BaseCondition
  {
    private GamePlayer m_player;
    private BaseQuest m_quest;

    public TurnPropertyCondition(BaseQuest quest, QuestConditionInfo info, int value)
      : base(quest, info, value)
    {
      this.m_quest = quest;
    }

    public override void AddTrigger(GamePlayer player)
    {
      this.m_player = player;
      player.GameKillDrop += new GamePlayer.GameKillDropEventHandel(this.QuestDropItem);
      base.AddTrigger(player);
    }

    public override bool CancelFinish(GamePlayer player)
    {
      ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(this.m_info.Para1);
      if (itemTemplate == null)
        return false;
      ItemInfo fromTemplate = ItemInfo.CreateFromTemplate(itemTemplate, this.m_info.Para2, 117);
      return player.AddTemplate(fromTemplate, eBageType.TempBag, this.m_info.Para2, eGameView.OtherTypeGet);
    }

    public override bool Finish(GamePlayer player)
    {
      return player.RemoveTemplate(this.m_info.Para1, this.m_info.Para2);
    }

    public override bool IsCompleted(GamePlayer player)
    {
      bool flag = false;
      if (player.GetItemCount(this.m_info.Para1) >= this.m_info.Para2)
      {
        this.Value = 0;
        flag = true;
      }
      return flag;
    }

    private void QuestDropItem(AbstractGame game, int copyId, int npcId, bool playResult)
    {
      if (this.m_player.GetItemCount(this.m_info.Para1) >= this.m_info.Para2)
        return;
      List<ItemInfo> info = (List<ItemInfo>) null;
      int gold = 0;
      int money = 0;
      int giftToken = 0;
      if (game is PVEGame)
        DropInventory.PvEQuestsDrop(npcId, ref info);
      if (game is PVPGame)
        DropInventory.PvPQuestsDrop(game.RoomType, playResult, ref info);
      if (info == null)
        return;
      foreach (ItemInfo itemInfo in info)
      {
        ItemInfo.FindSpecialItemInfo(itemInfo, ref gold, ref money, ref giftToken);
        if (itemInfo != null)
          this.m_player.TempBag.AddTemplate(itemInfo, itemInfo.Count);
      }
      this.m_player.AddGold(gold);
      this.m_player.AddGiftToken(giftToken);
      this.m_player.AddMoney(money);
      LogMgr.LogMoneyAdd(LogMoneyType.Award, LogMoneyType.Award_Drop, this.m_player.PlayerCharacter.ID, money, this.m_player.PlayerCharacter.Money, 0, 0, 0, "", "", "");
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.GameKillDrop -= new GamePlayer.GameKillDropEventHandel(this.QuestDropItem);
      base.RemoveTrigger(player);
    }
  }
}
