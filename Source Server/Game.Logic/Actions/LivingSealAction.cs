// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.LivingSealAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Effects;
using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class LivingSealAction : BaseAction
  {
    private Living m_Living;
    private Living m_Target;
    private int m_Type;

    public LivingSealAction(Living Living, Living target, int type, int delay)
      : base(delay, 2000)
    {
      this.m_Living = Living;
      this.m_Target = target;
      this.m_Type = type;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      this.m_Target.AddEffect((AbstractEffect) new SealEffect(2), 0);
      this.Finish(tick);
    }
  }
}
