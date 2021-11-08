// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.UserToemGemstoneCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
  public class UserToemGemstoneCondition : BaseCondition
  {
    public UserToemGemstoneCondition(BaseQuest quest, QuestConditionInfo info, int value)
      : base(quest, info, value)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.UserToemGemstonetEvent += new GamePlayer.PlayerUserToemGemstoneEventHandle(this.player_UserToemGemstonet);
    }

    public override bool IsCompleted(GamePlayer player)
    {
      return this.Value <= 0;
    }

    private void player_UserToemGemstonet()
    {
      if (this.Value <= 0)
        return;
      --this.Value;
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.UserToemGemstonetEvent -= new GamePlayer.PlayerUserToemGemstoneEventHandle(this.player_UserToemGemstonet);
    }
  }
}
