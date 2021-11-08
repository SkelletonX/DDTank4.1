// Decompiled with JetBrains decompiler
// Type: Game.Server.ConsortiaTask.BaseConsortiaTaskCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.ConsortiaTask.Condition;
using Game.Server.ConsortiaTask.Data;
using log4net;
using SqlDataProvider.Data;
using System.Reflection;

namespace Game.Server.ConsortiaTask
{
  public class BaseConsortiaTaskCondition
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected ConsortiaTaskInfo m_info;
    private int int_0;
    private BaseConsortiaTask baseConsortiaTask_0;
    private ConsortiaTaskUserDataInfo consortiaTaskUserDataInfo_0;

    public BaseConsortiaTaskCondition(
      ConsortiaTaskUserDataInfo player,
      BaseConsortiaTask consortiaTask,
      ConsortiaTaskInfo info,
      int value)
    {
      this.baseConsortiaTask_0 = consortiaTask;
      this.m_info = info;
      this.consortiaTaskUserDataInfo_0 = player;
      this.int_0 = value;
    }

    public ConsortiaTaskInfo Info
    {
      get
      {
        return this.m_info;
      }
    }

    public int Value
    {
      get
      {
        return this.int_0;
      }
      set
      {
        if (this.int_0 >= value)
          return;
        int valueAdd = value - this.int_0;
        this.baseConsortiaTask_0.RemakeValue(this.m_info.ID, ref valueAdd);
        if (valueAdd <= 0)
          return;
        this.int_0 += valueAdd;
        this.baseConsortiaTask_0.Update(this.consortiaTaskUserDataInfo_0);
      }
    }

    public virtual void AddTrigger(ConsortiaTaskUserDataInfo player)
    {
    }

    public virtual void RemoveTrigger(ConsortiaTaskUserDataInfo player)
    {
    }

    public static BaseConsortiaTaskCondition CreateCondition(
      ConsortiaTaskUserDataInfo player,
      BaseConsortiaTask quest,
      ConsortiaTaskInfo info,
      int value)
    {
      switch (info.CondictionType)
      {
        case 3:
          return (BaseConsortiaTaskCondition) new UsingItemCondition(player, quest, info, value);
        case 4:
          return (BaseConsortiaTaskCondition) new UseBigBugleCondition(player, quest, info, value);
        case 5:
          return (BaseConsortiaTaskCondition) new GameFightByRoomCondition(player, quest, info, value);
        case 6:
          return (BaseConsortiaTaskCondition) new GameOverByRoomCondition(player, quest, info, value);
        case 13:
          return (BaseConsortiaTaskCondition) new GameMonsterCondition(player, quest, info, value);
        case 21:
          return (BaseConsortiaTaskCondition) new GameMissionOverCondition(player, quest, info, value);
        case 22:
          return (BaseConsortiaTaskCondition) new GameKillByGameCondition(player, quest, info, value);
        case 23:
          return (BaseConsortiaTaskCondition) new GameFightByGameCondition(player, quest, info, value);
        case 34:
          return (BaseConsortiaTaskCondition) new GameFight2v2Condition(player, quest, info, value);
        case 38:
          return (BaseConsortiaTaskCondition) new RechargeMoneyCondition(player, quest, info, value);
        default:
          if (BaseConsortiaTaskCondition.ilog_0.IsErrorEnabled)
            BaseConsortiaTaskCondition.ilog_0.Error((object) string.Format("Can't find consortia task condition : {0}", (object) info.CondictionType));
          return (BaseConsortiaTaskCondition) null;
      }
    }
  }
}
