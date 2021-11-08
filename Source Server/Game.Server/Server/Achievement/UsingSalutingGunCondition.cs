// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.UsingSalutingGunCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  public class UsingSalutingGunCondition : BaseUserRecord
  {
    public UsingSalutingGunCondition(GamePlayer player, int type)
      : base(player, type)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.AfterUsingItem += new GamePlayer.PlayerItemPropertyEventHandle(this.player_AfterUsingItem);
    }

    private void player_AfterUsingItem(int templateID, int count)
    {
      int num1;
      int num2;
      switch (templateID)
      {
        case 11463:
        case 11528:
        case 11539:
        case 11540:
        case 11542:
        case 11549:
        case 21002:
        case 21006:
          num1 = 0;
          goto label_7;
        case 170145:
          num2 = 0;
          break;
        case 200337:
          num1 = 0;
          goto label_7;
        default:
          num2 = 1;
          break;
      }
      num1 = num2;
label_7:
      if ((uint) num1 > 0U)
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.AfterUsingItem -= new GamePlayer.PlayerItemPropertyEventHandle(this.player_AfterUsingItem);
    }
  }
}
