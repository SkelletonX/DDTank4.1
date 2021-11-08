using System;
using System.Diagnostics;

namespace Newtonsoft.Json.Serialization
{
	public class DiagnosticsTraceWriter : ITraceWriter
	{
		public TraceLevel LevelFilter
		{
			get;
			set;
		}

		private TraceEventType GetTraceEventType(TraceLevel level)
		{
			switch (level)
			{
			case TraceLevel.Error:
				return TraceEventType.Error;
			case TraceLevel.Warning:
				return TraceEventType.Warning;
			case TraceLevel.Info:
				return TraceEventType.Information;
			case TraceLevel.Verbose:
				return TraceEventType.Verbose;
			default:
				throw new ArgumentOutOfRangeException("level");
			}
		}

		public void Trace(TraceLevel level, string message, Exception ex)
		{
			if (level != 0)
			{
				TraceEventCache eventCache = new TraceEventCache();
				TraceEventType traceEventType = GetTraceEventType(level);
				foreach (TraceListener listener in System.Diagnostics.Trace.Listeners)
				{
					if (!listener.IsThreadSafe)
					{
						lock (listener)
						{
							listener.TraceEvent(eventCache, "Newtonsoft.Json", traceEventType, 0, message);
						}
					}
					else
					{
						listener.TraceEvent(eventCache, "Newtonsoft.Json", traceEventType, 0, message);
					}
					if (System.Diagnostics.Trace.AutoFlush)
					{
						listener.Flush();
					}
				}
			}
		}
	}
}
