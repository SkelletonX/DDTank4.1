// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.TimeHelper
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System;

namespace Game.Server.Quests
{
  public class TimeHelper
  {
    public static int GetDaysBetween(DateTime min, DateTime max)
    {
      int num = (int) Math.Floor((min - DateTime.MinValue).TotalDays);
      return (int) Math.Floor((max - DateTime.MinValue).TotalDays) - num;
    }
  }
}
