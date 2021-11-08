using System;
using System.Text;

namespace Ajax.JSON
{
	internal sealed class TimeSpanConverter : IAjaxObjectConverter
	{
		public string ClientScriptIdentifier => "AjaxTimeSpan";

		public Type[] SupportedTypes => new Type[1]
		{
			typeof(TimeSpan)
		};

		public bool IncludeSubclasses => true;

		public void RenderClientScript(ref StringBuilder sb)
		{
			sb.Append("function TimeSpan(){this.Days=0;this.Hours=0;this.Minutes=0;this.Seconds=0;this.Milliseconds=0;}\r\n");
			sb.Append("TimeSpan.prototype.toString = function(){return this.Days+'.'+this.Hours+':'+this.Minutes+':'+this.Seconds+'.'+this.Milliseconds;}\r\n");
		}

		public object FromString(string s, Type t)
		{
			TimeSpan timeSpan = TimeSpan.Parse(s);
			return timeSpan;
		}

		public void ToJSON(ref StringBuilder sb, object o)
		{
			if ((object)o.GetType() == typeof(TimeSpan) || (object)o.GetType().BaseType == typeof(TimeSpan))
			{
				TimeSpan timeSpan = (TimeSpan)o;
				sb.Append("{");
				sb.Append("'TotalDays':" + DefaultConverter.ToJSON(timeSpan.TotalDays) + ",");
				sb.Append("'TotalHours':" + DefaultConverter.ToJSON(timeSpan.TotalHours) + ",");
				sb.Append("'TotalMinutes':" + DefaultConverter.ToJSON(timeSpan.TotalMinutes) + ",");
				sb.Append("'TotalSeconds':" + DefaultConverter.ToJSON(timeSpan.TotalSeconds) + ",");
				sb.Append("'TotalMilliseconds':" + DefaultConverter.ToJSON(timeSpan.TotalMilliseconds) + ",");
				sb.Append("'Ticks':" + timeSpan.Ticks + ",");
				sb.Append("'Days':" + timeSpan.Days + ",");
				sb.Append("'Hours':" + timeSpan.Hours + ",");
				sb.Append("'Minutes':" + timeSpan.Minutes + ",");
				sb.Append("'Seconds':" + timeSpan.Seconds + ",");
				sb.Append("'Milliseconds':" + timeSpan.Milliseconds + "");
				sb.Append("}");
			}
		}
	}
}
