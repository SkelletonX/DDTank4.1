// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.CallFunctionAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class CallFunctionAction : BaseAction
  {
    private LivingCallBack m_func;

    public CallFunctionAction(LivingCallBack func, int delay)
      : base(delay)
    {
      this.m_func = func;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      try
      {
        this.m_func();
      }
      finally
      {
        this.Finish(tick);
      }
    }
  }
}
