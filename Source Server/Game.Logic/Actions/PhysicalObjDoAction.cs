// Decompiled with JetBrains decompiler
// Type: Game.Logic.Actions.PhysicalObjDoAction
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
  public class PhysicalObjDoAction : BaseAction
  {
    private PhysicalObj m_obj;
    private string m_action;

    public PhysicalObjDoAction(PhysicalObj obj, string action, int delay, int movieTime)
      : base(delay, movieTime)
    {
      this.m_obj = obj;
      this.m_action = action;
    }

    protected override void ExecuteImp(BaseGame game, long tick)
    {
      this.m_obj.CurrentAction = this.m_action;
      game.SendPhysicalObjPlayAction(this.m_obj);
      this.Finish(tick);
    }
  }
}
