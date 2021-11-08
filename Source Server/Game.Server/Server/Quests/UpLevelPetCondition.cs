// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.UpLevelPetCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
  public class UpLevelPetCondition : BaseCondition
  {
    public UpLevelPetCondition(BaseQuest quest, QuestConditionInfo info, int value)
      : base(quest, info, value)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.UpLevelPetEvent += new GamePlayer.PlayerUpLevelPetEventHandle(this.player_MissionOver);
    }

    public override bool IsCompleted(GamePlayer player)
    {
      return this.Value >= this.m_info.Para2;
    }

    private void player_MissionOver()
    {
      this.Value = this.m_info.Para2;
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.UpLevelPetEvent -= new GamePlayer.PlayerUpLevelPetEventHandle(this.player_MissionOver);
    }
  }
}
