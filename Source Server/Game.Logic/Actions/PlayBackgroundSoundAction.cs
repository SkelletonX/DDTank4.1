// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.PlayBackgroundSoundAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic.Actions
{
  public class PlayBackgroundSoundAction : BaseAction
  {
    private bool m_isPlay;

    public PlayBackgroundSoundAction(bool isPlay, int delay)
      : base(delay, 1000)
    {
      this.m_isPlay = isPlay;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      ((PVEGame) game).SendPlayBackgroundSound(this.m_isPlay);
      this.Finish(tick);
    }
  }
}
