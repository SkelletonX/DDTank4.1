// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingRotateTurnAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingRotateTurnAction : BaseAction
  {
    private string m_endPlay;
    private Player m_player;
    private int m_rotation;
    private int m_speed;

    public LivingRotateTurnAction(
      Player player,
      int rotation,
      int speed,
      string endPlay,
      int delay)
      : base(0, delay)
    {
      this.m_player = player;
      this.m_rotation = rotation;
      this.m_speed = speed;
      this.m_endPlay = endPlay;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      game.SendLivingTurnRotation(this.m_player, this.m_rotation, this.m_speed, this.m_endPlay);
      this.Finish(tick);
    }
  }
}
