// Decompiled with JetBrains decompiler
// Type: Game.Server.ConsortiaTask.Data.ConsortiaTaskDataInfo
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System;

namespace Game.Server.ConsortiaTask.Data
{
  public class ConsortiaTaskDataInfo
  {
    public ConsortiaTaskDataInfo(
      int consortiaId,
      int totalExp,
      int totalRiches,
      int totalOffer,
      int buffId,
      int vaild)
    {
      this.ConsortiaID = consortiaId;
      this.TotalExp = totalExp;
      this.TotalRiches = totalRiches;
      this.TotalOffer = totalOffer;
      this.BuffID = buffId;
      this.VaildDate = vaild;
      this.StartTime = DateTime.Now;
      this.IsActive = false;
      this.CanRemove = false;
    }

    public int ConsortiaID { get; set; }

    public bool IsActive { get; set; }

    public bool CanRemove { get; set; }

    public int TotalExp { get; set; }

    public int TotalRiches { get; set; }

    public int TotalOffer { get; set; }

    public int BuffID { get; set; }

    public int Condition1 { get; set; }

    public int Condition2 { get; set; }

    public int Condition3 { get; set; }

    public bool IsComplete { get; set; }

    public int VaildDate { get; set; }

    public DateTime StartTime { get; set; }

    public int GetTotalConditionCompleted()
    {
      return this.Condition1 + this.Condition2 + this.Condition3;
    }

    public bool IsVaildDate()
    {
      return this.StartTime.AddMinutes((double) this.VaildDate) > DateTime.Now;
    }
  }
}
