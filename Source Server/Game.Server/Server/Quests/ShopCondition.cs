// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.ShopCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
  public class ShopCondition : BaseCondition
  {
    public ShopCondition(BaseQuest quest, QuestConditionInfo info, int value)
      : base(quest, info, value)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.Paid += new GamePlayer.PlayerShopEventHandle(this.player_Shop);
    }

    public override bool IsCompleted(GamePlayer player)
    {
      return this.Value <= 0;
    }

    private void player_Shop(
      int money,
      int gold,
      int offer,
      int gifttoken,
      int medal,
      string payGoods)
    {
      if (this.m_info.Para1 == -1 && money > 0)
        this.Value -= money;
      if (this.m_info.Para1 == -2 && gold > 0)
        this.Value -= gold;
      if (this.m_info.Para1 == -3 && offer > 0)
        this.Value -= offer;
      if (this.m_info.Para1 == -4 && gifttoken > 0)
        this.Value -= gifttoken;
      string str1 = payGoods;
      char[] chArray = new char[1]{ ',' };
      foreach (string str2 in str1.Split(chArray))
      {
        int num = this.m_info.Para1;
        string str3 = num.ToString();
        if (str2 == str3)
          num = this.Value--;
      }
      if (this.Value >= 0)
        return;
      this.Value = 0;
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.Paid -= new GamePlayer.PlayerShopEventHandle(this.player_Shop);
    }
  }
}
