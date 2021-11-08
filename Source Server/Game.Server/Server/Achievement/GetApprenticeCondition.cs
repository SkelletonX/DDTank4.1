﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Achievement.GetApprenticeCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;

namespace Game.Server.Achievement
{
  public class GetApprenticeCondition : BaseUserRecord
  {
    public GetApprenticeCondition(GamePlayer player, int type)
      : base(player, type)
    {
      this.AddTrigger(player);
    }

    public override void AddTrigger(GamePlayer player)
    {
      player.AcademyEvent += new GamePlayer.PlayerAcademyEventHandle(this.academyEvent);
    }

    private void academyEvent(GamePlayer friendly, int type)
    {
      if (type != 1)
        return;
      this.m_player.AchievementInventory.UpdateUserAchievement(this.m_type, 1, 1);
    }

    public override void RemoveTrigger(GamePlayer player)
    {
      player.AcademyEvent -= new GamePlayer.PlayerAcademyEventHandle(this.academyEvent);
    }
  }
}
