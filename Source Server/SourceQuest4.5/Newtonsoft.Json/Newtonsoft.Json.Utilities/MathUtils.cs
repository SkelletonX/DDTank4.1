using System;

namespace Newtonsoft.Json.Utilities
{
	internal static class MathUtils
	{
		public static int IntLength(ulong i)
		{
			if (i < 10000000000L)
			{
				if (i < 10)
				{
					return 1;
				}
				if (i < 100)
				{
					return 2;
				}
				if (i < 1000)
				{
					return 3;
				}
				if (i < 10000)
				{
					return 4;
				}
				if (i < 100000)
				{
					return 5;
				}
				if (i < 1000000)
				{
					return 6;
				}
				if (i < 10000000)
				{
					return 7;
				}
				if (i < 100000000)
				{
					return 8;
				}
				if (i < 1000000000)
				{
					return 9;
				}
				return 10;
			}
			if (i < 100000000000L)
			{
				return 11;
			}
			if (i < 1000000000000L)
			{
				return 12;
			}
			if (i < 10000000000000L)
			{
				return 13;
			}
			if (i < 100000000000000L)
			{
				return 14;
			}
			if (i < 1000000000000000L)
			{
				return 15;
			}
			if (i < 10000000000000000L)
			{
				return 16;
			}
			if (i < 100000000000000000L)
			{
				return 17;
			}
			if (i < 1000000000000000000L)
			{
				return 18;
			}
			if (i < 10000000000000000000uL)
			{
				return 19;
			}
			return 20;
		}

		public static char IntToHex(int n)
		{
			if (n <= 9)
			{
				return (char)(n + 48);
			}
			return (char)(n - 10 + 97);
		}

		public static int? Min(int? val1, int? val2)
		{
			if (!val1.HasValue)
			{
				return val2;
			}
			if (!val2.HasValue)
			{
				return val1;
			}
			return Math.Min(val1.Value, val2.Value);
		}

		public static int? Max(int? val1, int? val2)
		{
			if (!val1.HasValue)
			{
				return val2;
			}
			if (!val2.HasValue)
			{
				return val1;
			}
			return Math.Max(val1.Value, val2.Value);
		}

		public static double? Max(double? val1, double? val2)
		{
			if (!val1.HasValue)
			{
				return val2;
			}
			if (!val2.HasValue)
			{
				return val1;
			}
			return Math.Max(val1.Value, val2.Value);
		}

		public static bool ApproxEquals(double d1, double d2)
		{
			if (d1 == d2)
			{
				return true;
			}
			double num = (Math.Abs(d1) + Math.Abs(d2) + 10.0) * 2.2204460492503131E-16;
			double num2 = d1 - d2;
			if (0.0 - num < num2)
			{
				return num > num2;
			}
			return false;
		}
	}
}
