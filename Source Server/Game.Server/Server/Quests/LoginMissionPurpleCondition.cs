using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
  public class LoginMissionPurpleCondition : BaseCondition
  {
    public LoginMissionPurpleCondition(BaseQuest quest, QuestConditionInfo info, int value)
      : base(quest, info, value)
    {
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.PlayerLogin += new GamePlayer.PlayerLoginEventHandle(this.player_Login);
    }

    public override bool IsCompleted(GamePlayer player)
    {
      return player.PlayerCharacter.typeVIP == (byte) 2;
    }

    private void player_Login()
    {
      --this.Value;
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.PlayerLogin -= new GamePlayer.PlayerLoginEventHandle(this.player_Login);
    }
  }
}
