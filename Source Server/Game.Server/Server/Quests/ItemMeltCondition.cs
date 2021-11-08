// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.ItemMeltCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
  public class ItemMeltCondition : BaseCondition
  {
    public ItemMeltCondition(BaseQuest quest, QuestConditionInfo info, int value)
      : base(quest, info, value)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.ItemMelt += new GamePlayer.PlayerItemMeltEventHandle(this.player_ItemMelt);
    }

    public override bool IsCompleted(GamePlayer player)
    {
      return this.Value <= 0;
    }

    private void player_ItemMelt(int categoryID)
    {
      if (categoryID != this.m_info.Para1)
        return;
      this.Value = 0;
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.ItemMelt -= new GamePlayer.PlayerItemMeltEventHandle(this.player_ItemMelt);
    }
  }
}
