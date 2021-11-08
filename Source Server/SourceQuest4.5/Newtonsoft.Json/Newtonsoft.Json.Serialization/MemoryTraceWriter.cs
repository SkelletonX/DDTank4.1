using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Newtonsoft.Json.Serialization
{
	public class MemoryTraceWriter : ITraceWriter
	{
		private readonly Queue<string> _traceMessages;

		public TraceLevel LevelFilter
		{
			get;
			set;
		}

		public MemoryTraceWriter()
		{
			LevelFilter = TraceLevel.Verbose;
			_traceMessages = new Queue<string>();
		}

		public void Trace(TraceLevel level, string message, Exception ex)
		{
			string item = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff", CultureInfo.InvariantCulture) + " " + level.ToString("g") + " " + message;
			if (_traceMessages.Count >= 1000)
			{
				_traceMessages.Dequeue();
			}
			_traceMessages.Enqueue(item);
		}

		public IEnumerable<string> GetTraceMessages()
		{
			return _traceMessages;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string traceMessage in _traceMessages)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(traceMessage);
			}
			return stringBuilder.ToString();
		}
	}
}
