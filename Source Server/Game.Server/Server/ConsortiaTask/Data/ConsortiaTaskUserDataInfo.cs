// Decompiled with JetBrains decompiler
// Type: Game.Server.ConsortiaTask.Data.ConsortiaTaskUserDataInfo
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.GameObjects;
using System;
using System.Collections.Generic;

namespace Game.Server.ConsortiaTask.Data
{
  public class ConsortiaTaskUserDataInfo
  {
    public int UserID { get; set; }

    public int Condition1 { get; set; }

    public int Condition2 { get; set; }

    public int Condition3 { get; set; }

    public GamePlayer Player { get; set; }

    public List<BaseConsortiaTaskCondition> ConditionList { get; set; }

    public int GetTotalConditionCompleted()
    {
      return this.Condition1 + this.Condition2 + this.Condition3;
    }

    public int GetConditionValue(int index)
    {
      switch (index)
      {
        case 0:
          return this.Condition1;
        case 1:
          return this.Condition2;
        case 2:
          return this.Condition3;
        default:
          throw new Exception("Consortia Task condition index out of range.");
      }
    }

    public void SaveConditionValue(int index, int value)
    {
      switch (index)
      {
        case 0:
          this.Condition1 = value;
          break;
        case 1:
          this.Condition2 = value;
          break;
        case 2:
          this.Condition3 = value;
          break;
        default:
          throw new Exception("Consortia Task condition index out of range.");
      }
    }
  }
}
