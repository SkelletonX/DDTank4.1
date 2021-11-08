// Decompiled with JetBrains decompiler
// Type: Game.Logic.TickHelper
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using System.Diagnostics;

namespace Game.Logic
{
  public static class TickHelper
  {
    private static long StopwatchFrequencyMilliseconds = Stopwatch.Frequency / 1000L;

    public static long GetTickCount()
    {
      return Stopwatch.GetTimestamp() / TickHelper.StopwatchFrequencyMilliseconds;
    }
  }
}
