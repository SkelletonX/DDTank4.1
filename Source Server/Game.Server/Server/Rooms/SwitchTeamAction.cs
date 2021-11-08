﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.SwitchTeamAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;

namespace Game.Server.Rooms
{
  public class SwitchTeamAction : IAction
  {
    private GamePlayer m_player;

    public SwitchTeamAction(GamePlayer player)
    {
      this.m_player = player;
    }

    public void Execute()
    {
      this.m_player.CurrentRoom?.SwitchTeamUnsafe(this.m_player);
    }
  }
}
