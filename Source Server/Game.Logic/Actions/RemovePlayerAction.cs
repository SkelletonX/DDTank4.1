// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.RemovePlayerAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class RemovePlayerAction : IAction
  {
    private bool m_isFinished;
    private Player m_player;

    public RemovePlayerAction(Player player)
    {
      this.m_player = player;
      this.m_isFinished = false;
    }

    public void Execute(BaseGame game, long tick)
    {
      this.m_player.DeadLink();
      this.m_isFinished = true;
    }

    public bool IsFinished(long tick)
    {
      return this.m_isFinished;
    }
  }
}
