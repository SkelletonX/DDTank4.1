// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.BaseClass.ApplicationLog
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System.Diagnostics;

namespace SqlDataProvider.BaseClass
{
  public static class ApplicationLog
  {
    public static void WriteError(string message)
    {
      ApplicationLog.WriteLog(TraceLevel.Error, message);
    }

    private static void WriteLog(TraceLevel level, string messageText)
    {
      try
      {
        EventLogEntryType type = level != TraceLevel.Error ? EventLogEntryType.Error : EventLogEntryType.Error;
        string str = "Application";
        if (!EventLog.SourceExists(str))
          EventLog.CreateEventSource(str, "BIZ");
        new EventLog(str, ".", str).WriteEntry(messageText, type);
      }
      catch
      {
      }
    }
  }
}
