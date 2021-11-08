namespace Game.Base
{
	public class eAllowIP
	{
		private static string BB = "203.162.121.30";

		private static string LL = "123.30.150.32|123.30.150.33|123.30.150.34|123.30.150.35|42.112.20.161|42.112.20.163|42.112.20.164|123.30.150.17|123.30.150.18";

		public static string IP6 = $"127.0.0.1|{BB}|{LL}";

		public static bool IPBlocker(string ip)
		{
			string[] array = IP6.Split('|');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == CheckIp(ip))
				{
					return true;
				}
			}
			return true;
		}

		public static string CheckIp(string str)
		{
			return str;
		}
	}
}
