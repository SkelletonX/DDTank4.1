// Decompiled with JetBrains decompiler
// Type: Game.Server.ConsortiaTask.Condition.UsingItemCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.ConsortiaTask.Data;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.ConsortiaTask.Condition
{
  public class UsingItemCondition : BaseConsortiaTaskCondition
  {
    public UsingItemCondition(
      ConsortiaTaskUserDataInfo player,
      BaseConsortiaTask quest,
      ConsortiaTaskInfo info,
      int value)
      : base(player, quest, info, value)
    {
    }

    public override void AddTrigger(ConsortiaTaskUserDataInfo player)
    {
      player.Player.AfterUsingItem += new GamePlayer.PlayerItemPropertyEventHandle(this.method_0);
    }

    public override void RemoveTrigger(ConsortiaTaskUserDataInfo player)
    {
      player.Player.AfterUsingItem -= new GamePlayer.PlayerItemPropertyEventHandle(this.method_0);
    }

    private void method_0(int int_1, int int_2)
    {
      if (int_1 != this.m_info.Para1 || this.Value >= this.m_info.Para2)
        return;
      this.Value += int_2;
    }
  }
}
