// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LockFocusAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic.Actions
{
  public class LockFocusAction : BaseAction
  {
    private bool m_isLock;

    public LockFocusAction(bool isLock, int delay, int finishTime)
      : base(delay, finishTime)
    {
      this.m_isLock = isLock;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      game.SendLockFocus(this.m_isLock);
      this.Finish(tick);
    }
  }
}
