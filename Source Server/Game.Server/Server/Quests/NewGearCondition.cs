// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.NewGearCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
  public class NewGearCondition : BaseCondition
  {
    public NewGearCondition(BaseQuest quest, QuestConditionInfo info, int value)
      : base(quest, info, value)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.NewGearEvent += new GamePlayer.PlayerNewGearEventHandle(this.player_NewGear);
    }

    public override bool IsCompleted(GamePlayer player)
    {
      return this.Value <= 0;
    }

    private void player_NewGear(ItemInfo item)
    {
            if (item.Template.CategoryID != this.m_info.Para1 && item.Template.CategoryID != this.m_info.Para2 || this.Value < this.m_info.Para2)
        return;
      --this.Value;
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.NewGearEvent -= new GamePlayer.PlayerNewGearEventHandle(this.player_NewGear);
    }
  }
}
