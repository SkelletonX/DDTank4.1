// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.ItemStrengthenCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
  public class ItemStrengthenCondition : BaseCondition
  {
    public ItemStrengthenCondition(BaseQuest quest, QuestConditionInfo info, int value)
      : base(quest, info, value)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.ItemStrengthen += new GamePlayer.PlayerItemStrengthenEventHandle(this.player_ItemStrengthen);
    }

    public override bool IsCompleted(GamePlayer player)
    {
      return true;
    }

    private void player_ItemStrengthen(int categoryID, int level)
    {
      if (this.m_info.Para1 != categoryID || this.m_info.Para2 > level)
        return;
      this.Value = 0;
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.ItemStrengthen -= new GamePlayer.PlayerItemStrengthenEventHandle(this.player_ItemStrengthen);
    }
  }
}
