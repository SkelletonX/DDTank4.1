using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Newtonsoft.Json.Utilities
{
	internal static class DateTimeUtils
	{
		private const int DaysPer100Years = 36524;

		private const int DaysPer400Years = 146097;

		private const int DaysPer4Years = 1461;

		private const int DaysPerYear = 365;

		private const long TicksPerDay = 864000000000L;

		internal static readonly long InitialJavaScriptDateTicks;

		private static readonly int[] DaysToMonth365;

		private static readonly int[] DaysToMonth366;

		static DateTimeUtils()
		{
			InitialJavaScriptDateTicks = 621355968000000000L;
			DaysToMonth365 = new int[13]
			{
				0,
				31,
				59,
				90,
				120,
				151,
				181,
				212,
				243,
				273,
				304,
				334,
				365
			};
			DaysToMonth366 = new int[13]
			{
				0,
				31,
				60,
				91,
				121,
				152,
				182,
				213,
				244,
				274,
				305,
				335,
				366
			};
		}

		public static TimeSpan GetUtcOffset(this DateTime d)
		{
			return TimeZoneInfo.Local.GetUtcOffset(d);
		}

		public static XmlDateTimeSerializationMode ToSerializationMode(DateTimeKind kind)
		{
			switch (kind)
			{
			case DateTimeKind.Local:
				return XmlDateTimeSerializationMode.Local;
			case DateTimeKind.Unspecified:
				return XmlDateTimeSerializationMode.Unspecified;
			case DateTimeKind.Utc:
				return XmlDateTimeSerializationMode.Utc;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("kind", kind, "Unexpected DateTimeKind value.");
			}
		}

		internal static DateTime EnsureDateTime(DateTime value, DateTimeZoneHandling timeZone)
		{
			switch (timeZone)
			{
			case DateTimeZoneHandling.Local:
				value = SwitchToLocalTime(value);
				break;
			case DateTimeZoneHandling.Utc:
				value = SwitchToUtcTime(value);
				break;
			case DateTimeZoneHandling.Unspecified:
				value = new DateTime(value.Ticks, DateTimeKind.Unspecified);
				break;
			default:
				throw new ArgumentException("Invalid date time handling value.");
			case DateTimeZoneHandling.RoundtripKind:
				break;
			}
			return value;
		}

		private static DateTime SwitchToLocalTime(DateTime value)
		{
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
				return new DateTime(value.Ticks, DateTimeKind.Local);
			case DateTimeKind.Utc:
				return value.ToLocalTime();
			case DateTimeKind.Local:
				return value;
			default:
				return value;
			}
		}

		private static DateTime SwitchToUtcTime(DateTime value)
		{
			switch (value.Kind)
			{
			case DateTimeKind.Unspecified:
				return new DateTime(value.Ticks, DateTimeKind.Utc);
			case DateTimeKind.Utc:
				return value;
			case DateTimeKind.Local:
				return value.ToUniversalTime();
			default:
				return value;
			}
		}

		private static long ToUniversalTicks(DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Utc)
			{
				return dateTime.Ticks;
			}
			return ToUniversalTicks(dateTime, dateTime.GetUtcOffset());
		}

		private static long ToUniversalTicks(DateTime dateTime, TimeSpan offset)
		{
			if (dateTime.Kind == DateTimeKind.Utc || dateTime == DateTime.MaxValue || dateTime == DateTime.MinValue)
			{
				return dateTime.Ticks;
			}
			long num = dateTime.Ticks - offset.Ticks;
			if (num > 3155378975999999999L)
			{
				return 3155378975999999999L;
			}
			if (num < 0)
			{
				return 0L;
			}
			return num;
		}

		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, TimeSpan offset)
		{
			long universialTicks = ToUniversalTicks(dateTime, offset);
			return UniversialTicksToJavaScriptTicks(universialTicks);
		}

		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime)
		{
			return ConvertDateTimeToJavaScriptTicks(dateTime, convertToUtc: true);
		}

		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, bool convertToUtc)
		{
			long universialTicks = convertToUtc ? ToUniversalTicks(dateTime) : dateTime.Ticks;
			return UniversialTicksToJavaScriptTicks(universialTicks);
		}

		private static long UniversialTicksToJavaScriptTicks(long universialTicks)
		{
			return (universialTicks - InitialJavaScriptDateTicks) / 10000;
		}

		internal static DateTime ConvertJavaScriptTicksToDateTime(long javaScriptTicks)
		{
			return new DateTime(javaScriptTicks * 10000 + InitialJavaScriptDateTicks, DateTimeKind.Utc);
		}

		internal static bool TryParseDateIso(string text, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, out object dt)
		{
			DateTimeParser dateTimeParser = default(DateTimeParser);
			if (!dateTimeParser.Parse(text))
			{
				dt = null;
				return false;
			}
			DateTime dateTime = new DateTime(dateTimeParser.Year, dateTimeParser.Month, dateTimeParser.Day, dateTimeParser.Hour, dateTimeParser.Minute, dateTimeParser.Second).AddTicks(dateTimeParser.Fraction);
			if (dateParseHandling == DateParseHandling.DateTimeOffset)
			{
				TimeSpan offset;
				switch (dateTimeParser.Zone)
				{
				case ParserTimeZone.Utc:
					offset = new TimeSpan(0L);
					break;
				case ParserTimeZone.LocalWestOfUtc:
					offset = new TimeSpan(-dateTimeParser.ZoneHour, -dateTimeParser.ZoneMinute, 0);
					break;
				case ParserTimeZone.LocalEastOfUtc:
					offset = new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0);
					break;
				default:
					offset = TimeZoneInfo.Local.GetUtcOffset(dateTime);
					break;
				}
				long num = dateTime.Ticks - offset.Ticks;
				if (num < 0 || num > 3155378975999999999L)
				{
					dt = null;
					return false;
				}
				dt = new DateTimeOffset(dateTime, offset);
				return true;
			}
			switch (dateTimeParser.Zone)
			{
			case ParserTimeZone.Utc:
				dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
				break;
			case ParserTimeZone.LocalWestOfUtc:
			{
				TimeSpan timeSpan2 = new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0);
				long num2 = dateTime.Ticks + timeSpan2.Ticks;
				if (num2 <= DateTime.MaxValue.Ticks)
				{
					dateTime = new DateTime(num2, DateTimeKind.Utc).ToLocalTime();
					break;
				}
				num2 += dateTime.GetUtcOffset().Ticks;
				if (num2 > DateTime.MaxValue.Ticks)
				{
					num2 = DateTime.MaxValue.Ticks;
				}
				dateTime = new DateTime(num2, DateTimeKind.Local);
				break;
			}
			case ParserTimeZone.LocalEastOfUtc:
			{
				TimeSpan timeSpan = new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0);
				long num2 = dateTime.Ticks - timeSpan.Ticks;
				if (num2 >= DateTime.MinValue.Ticks)
				{
					dateTime = new DateTime(num2, DateTimeKind.Utc).ToLocalTime();
					break;
				}
				num2 += dateTime.GetUtcOffset().Ticks;
				if (num2 < DateTime.MinValue.Ticks)
				{
					num2 = DateTime.MinValue.Ticks;
				}
				dateTime = new DateTime(num2, DateTimeKind.Local);
				break;
			}
			}
			dt = EnsureDateTime(dateTime, dateTimeZoneHandling);
			return true;
		}

		internal static bool TryParseDateTime(string s, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, string dateFormatString, CultureInfo culture, out object dt)
		{
			if (s.Length > 0)
			{
				if (s[0] == '/')
				{
					if (s.StartsWith("/Date(", StringComparison.Ordinal) && s.EndsWith(")/", StringComparison.Ordinal) && TryParseDateMicrosoft(s, dateParseHandling, dateTimeZoneHandling, out dt))
					{
						return true;
					}
				}
				else if (s.Length >= 19 && s.Length <= 40 && char.IsDigit(s[0]) && s[10] == 'T' && TryParseDateIso(s, dateParseHandling, dateTimeZoneHandling, out dt))
				{
					return true;
				}
				if (!string.IsNullOrEmpty(dateFormatString) && TryParseDateExact(s, dateParseHandling, dateTimeZoneHandling, dateFormatString, culture, out dt))
				{
					return true;
				}
			}
			dt = null;
			return false;
		}

		private static bool TryParseDateMicrosoft(string text, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, out object dt)
		{
			string text2 = text.Substring(6, text.Length - 8);
			DateTimeKind dateTimeKind = DateTimeKind.Utc;
			int num = text2.IndexOf('+', 1);
			if (num == -1)
			{
				num = text2.IndexOf('-', 1);
			}
			TimeSpan timeSpan = TimeSpan.Zero;
			if (num != -1)
			{
				dateTimeKind = DateTimeKind.Local;
				timeSpan = ReadOffset(text2.Substring(num));
				text2 = text2.Substring(0, num);
			}
			if (!long.TryParse(text2, NumberStyles.Integer, CultureInfo.InvariantCulture, out long result))
			{
				dt = null;
				return false;
			}
			DateTime dateTime = ConvertJavaScriptTicksToDateTime(result);
			if (dateParseHandling == DateParseHandling.DateTimeOffset)
			{
				dt = new DateTimeOffset(dateTime.Add(timeSpan).Ticks, timeSpan);
				return true;
			}
			DateTime value;
			switch (dateTimeKind)
			{
			case DateTimeKind.Unspecified:
				value = DateTime.SpecifyKind(dateTime.ToLocalTime(), DateTimeKind.Unspecified);
				break;
			case DateTimeKind.Local:
				value = dateTime.ToLocalTime();
				break;
			default:
				value = dateTime;
				break;
			}
			dt = EnsureDateTime(value, dateTimeZoneHandling);
			return true;
		}

		private static bool TryParseDateExact(string text, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, string dateFormatString, CultureInfo culture, out object dt)
		{
			DateTime result2;
			if (dateParseHandling == DateParseHandling.DateTimeOffset)
			{
				if (DateTimeOffset.TryParseExact(text, dateFormatString, culture, DateTimeStyles.RoundtripKind, out DateTimeOffset result))
				{
					dt = result;
					return true;
				}
			}
			else if (DateTime.TryParseExact(text, dateFormatString, culture, DateTimeStyles.RoundtripKind, out result2))
			{
				result2 = EnsureDateTime(result2, dateTimeZoneHandling);
				dt = result2;
				return true;
			}
			dt = null;
			return false;
		}

		private static TimeSpan ReadOffset(string offsetText)
		{
			bool flag = offsetText[0] == '-';
			int num = int.Parse(offsetText.Substring(1, 2), NumberStyles.Integer, CultureInfo.InvariantCulture);
			int num2 = 0;
			if (offsetText.Length >= 5)
			{
				num2 = int.Parse(offsetText.Substring(3, 2), NumberStyles.Integer, CultureInfo.InvariantCulture);
			}
			TimeSpan result = TimeSpan.FromHours(num) + TimeSpan.FromMinutes(num2);
			if (flag)
			{
				result = result.Negate();
			}
			return result;
		}

		internal static void WriteDateTimeString(TextWriter writer, DateTime value, DateFormatHandling format, string formatString, CultureInfo culture)
		{
			if (string.IsNullOrEmpty(formatString))
			{
				char[] array = new char[64];
				int count = WriteDateTimeString(array, 0, value, null, value.Kind, format);
				writer.Write(array, 0, count);
			}
			else
			{
				writer.Write(value.ToString(formatString, culture));
			}
		}

		internal static int WriteDateTimeString(char[] chars, int start, DateTime value, TimeSpan? offset, DateTimeKind kind, DateFormatHandling format)
		{
			int num = start;
			if (format == DateFormatHandling.MicrosoftDateFormat)
			{
				TimeSpan offset2 = offset ?? value.GetUtcOffset();
				long num2 = ConvertDateTimeToJavaScriptTicks(value, offset2);
				"\\/Date(".CopyTo(0, chars, num, 7);
				num += 7;
				string text = num2.ToString(CultureInfo.InvariantCulture);
				text.CopyTo(0, chars, num, text.Length);
				num += text.Length;
				switch (kind)
				{
				case DateTimeKind.Unspecified:
					if (value != DateTime.MaxValue && value != DateTime.MinValue)
					{
						num = WriteDateTimeOffset(chars, num, offset2, format);
					}
					break;
				case DateTimeKind.Local:
					num = WriteDateTimeOffset(chars, num, offset2, format);
					break;
				}
				")\\/".CopyTo(0, chars, num, 3);
				num += 3;
			}
			else
			{
				num = WriteDefaultIsoDate(chars, num, value);
				switch (kind)
				{
				case DateTimeKind.Local:
					num = WriteDateTimeOffset(chars, num, offset ?? value.GetUtcOffset(), format);
					break;
				case DateTimeKind.Utc:
					chars[num++] = 'Z';
					break;
				}
			}
			return num;
		}

		internal static int WriteDefaultIsoDate(char[] chars, int start, DateTime dt)
		{
			int num = 19;
			GetDateValues(dt, out int year, out int month, out int day);
			CopyIntToCharArray(chars, start, year, 4);
			chars[start + 4] = '-';
			CopyIntToCharArray(chars, start + 5, month, 2);
			chars[start + 7] = '-';
			CopyIntToCharArray(chars, start + 8, day, 2);
			chars[start + 10] = 'T';
			CopyIntToCharArray(chars, start + 11, dt.Hour, 2);
			chars[start + 13] = ':';
			CopyIntToCharArray(chars, start + 14, dt.Minute, 2);
			chars[start + 16] = ':';
			CopyIntToCharArray(chars, start + 17, dt.Second, 2);
			int num2 = (int)(dt.Ticks % 10000000);
			if (num2 != 0)
			{
				int num3 = 7;
				while (num2 % 10 == 0)
				{
					num3--;
					num2 /= 10;
				}
				chars[start + 19] = '.';
				CopyIntToCharArray(chars, start + 20, num2, num3);
				num += num3 + 1;
			}
			return start + num;
		}

		private static void CopyIntToCharArray(char[] chars, int start, int value, int digits)
		{
			while (digits-- != 0)
			{
				chars[start + digits] = (char)(value % 10 + 48);
				value /= 10;
			}
		}

		internal static int WriteDateTimeOffset(char[] chars, int start, TimeSpan offset, DateFormatHandling format)
		{
			chars[start++] = ((offset.Ticks >= 0) ? '+' : '-');
			int value = Math.Abs(offset.Hours);
			CopyIntToCharArray(chars, start, value, 2);
			start += 2;
			if (format == DateFormatHandling.IsoDateFormat)
			{
				chars[start++] = ':';
			}
			int value2 = Math.Abs(offset.Minutes);
			CopyIntToCharArray(chars, start, value2, 2);
			start += 2;
			return start;
		}

		internal static void WriteDateTimeOffsetString(TextWriter writer, DateTimeOffset value, DateFormatHandling format, string formatString, CultureInfo culture)
		{
			if (string.IsNullOrEmpty(formatString))
			{
				char[] array = new char[64];
				int count = WriteDateTimeString(array, 0, (format == DateFormatHandling.IsoDateFormat) ? value.DateTime : value.UtcDateTime, value.Offset, DateTimeKind.Local, format);
				writer.Write(array, 0, count);
			}
			else
			{
				writer.Write(value.ToString(formatString, culture));
			}
		}

		private static void GetDateValues(DateTime td, out int year, out int month, out int day)
		{
			long ticks = td.Ticks;
			int num = (int)(ticks / 864000000000L);
			int num2 = num / 146097;
			num -= num2 * 146097;
			int num3 = num / 36524;
			if (num3 == 4)
			{
				num3 = 3;
			}
			num -= num3 * 36524;
			int num4 = num / 1461;
			num -= num4 * 1461;
			int num5 = num / 365;
			if (num5 == 4)
			{
				num5 = 3;
			}
			year = num2 * 400 + num3 * 100 + num4 * 4 + num5 + 1;
			num -= num5 * 365;
			int[] array = (num5 == 3 && (num4 != 24 || num3 == 3)) ? DaysToMonth366 : DaysToMonth365;
			int i;
			for (i = num >> 6; num >= array[i]; i++)
			{
			}
			month = i;
			day = num - array[i - 1] + 1;
		}
	}
}
