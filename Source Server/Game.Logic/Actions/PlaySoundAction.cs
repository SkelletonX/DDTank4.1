// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.PlaySoundAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic.Actions
{
  public class PlaySoundAction : BaseAction
  {
    private string m_action;

    public PlaySoundAction(string action, int delay)
      : base(delay, 1000)
    {
      this.m_action = action;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      ((PVEGame) game).SendPlaySound(this.m_action);
      this.Finish(tick);
    }
  }
}
