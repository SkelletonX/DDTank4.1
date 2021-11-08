using System.Diagnostics;

namespace SqlDataProvider.BaseClass
{
	public static class ApplicationLog
	{
		public static void WriteError(string message)
		{
			WriteLog(TraceLevel.Error, message);
		}

		private static void WriteLog(TraceLevel level, string messageText)
		{
			try
			{
				EventLogEntryType LogEntryType = (level != TraceLevel.Error) ? EventLogEntryType.Error : EventLogEntryType.Error;
				string LogName = "Application";
				if (!EventLog.SourceExists(LogName))
				{
					EventLog.CreateEventSource(LogName, "BIZ");
				}
				new EventLog(LogName, ".", LogName).WriteEntry(messageText, LogEntryType);
			}
			catch
			{
			}
		}
	}
}
