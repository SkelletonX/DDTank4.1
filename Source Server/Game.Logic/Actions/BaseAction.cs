// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.BaseAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic.Actions
{
  public class BaseAction : IAction
  {
    private long m_finishDelay;
    private long m_finishTick;
    private long m_tick;

    public BaseAction(int delay)
      : this(delay, 0)
    {
    }

    public BaseAction(int delay, int finishDelay)
    {
      this.m_tick = TickHelper.GetTickCount() + (long) delay;
      this.m_finishDelay = (long) finishDelay;
      this.m_finishTick = long.MaxValue;
    }

    public void Execute(BaseGame game, long tick)
    {
      if (this.m_tick > tick || this.m_finishTick != long.MaxValue)
        return;
      this.ExecuteImp(game, tick);
    }

    protected virtual void ExecuteImp(BaseGame game, long tick)
    {
      this.Finish(tick);
    }

    public void Finish(long tick)
    {
      this.m_finishTick = tick + this.m_finishDelay;
    }

    public bool IsFinished(long tick)
    {
      return this.m_finishTick <= tick;
    }
  }
}
