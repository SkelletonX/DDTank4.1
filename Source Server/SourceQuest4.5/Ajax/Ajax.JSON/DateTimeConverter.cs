using System;
using System.Text;

namespace Ajax.JSON
{
	internal sealed class DateTimeConverter : IAjaxObjectConverter
	{
		public string ClientScriptIdentifier => "AjaxDateTime";

		public Type[] SupportedTypes => new Type[1]
		{
			typeof(DateTime)
		};

		public bool IncludeSubclasses => true;

		public void RenderClientScript(ref StringBuilder sb)
		{
			sb.Append("function digi(v, c){v = v + \"\";var n = \"0000\";if(v.length < c) return n.substr(0, c-v.length) + v;return v;}\r\nfunction DateTime(year,month,day,hours,minutes,seconds){if(year>9999||year<1970||month<1||month>12||day<0||day>31||hours<0||hours>23||minutes<0||minutes>59||seconds<0||seconds>59)throw(\"ArgumentException\");this.Year = year;this.Month = month;this.Day = day;this.Hours = hours;this.Minutes = minutes;this.Seconds = seconds;}\r\nDateTime.prototype.toString = function(){return digi(this.Year,4) + digi(this.Month,2) + digi(this.Day,2) + digi(this.Hours,2) + digi(this.Minutes,2) + digi(this.Seconds,2);}\r\n");
		}

		public object FromString(string s, Type t)
		{
			if (s == null || s == "")
			{
				return DateTime.Now;
			}
			if (s.Length == 8)
			{
				return new DateTime(int.Parse(s.Substring(0, 4)), int.Parse(s.Substring(4, 2)), int.Parse(s.Substring(6, 2)));
			}
			if (s.Length == 14)
			{
				return new DateTime(int.Parse(s.Substring(0, 4)), int.Parse(s.Substring(4, 2)), int.Parse(s.Substring(6, 2)), int.Parse(s.Substring(8, 2)), int.Parse(s.Substring(10, 2)), int.Parse(s.Substring(12, 2)));
			}
			throw new NotImplementedException();
		}

		public void ToJSON(ref StringBuilder sb, object o)
		{
			if ((object)o.GetType() == typeof(DateTime) || (object)o.GetType().BaseType == typeof(DateTime))
			{
				DateTime dateTime = (DateTime)o;
				sb.Append("new Date(" + dateTime.Year + "," + (dateTime.Month - 1) + "," + dateTime.Day + "," + dateTime.Hour + "," + dateTime.Minute + "," + dateTime.Second + ")");
			}
		}
	}
}
