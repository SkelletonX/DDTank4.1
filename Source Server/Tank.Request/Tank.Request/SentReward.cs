using Bussiness;
using Bussiness.Interface;
using log4net;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class SentReward : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static string GetSentRewardIP => ConfigurationManager.AppSettings["SentRewardIP"];

		public static string GetSentRewardKey => ConfigurationManager.AppSettings["SentRewardKey"];

		public bool IsReusable => false;

		public static bool ValidSentRewardIP(string ip)
		{
			string getSentRewardIP = GetSentRewardIP;
			if (!string.IsNullOrEmpty(getSentRewardIP) && !getSentRewardIP.Split('|').Contains(ip))
			{
				return false;
			}
			return true;
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			try
			{
				int result = 1;
				if (ValidSentRewardIP(context.Request.UserHostAddress))
				{
					string content = HttpUtility.UrlDecode(context.Request["content"]);
					string getSentRewardKey = GetSentRewardKey;
					string[] array = BaseInterface.CreateInterface().UnEncryptSentReward(content, ref result, getSentRewardKey);
					if (array.Length == 8 && result != 5 && result != 6 && result != 7)
					{
						_ = array[0];
						_ = array[1];
						_ = array[2];
						int.Parse(array[3]);
						int.Parse(array[4]);
						string param = array[5];
						if (checkParam(ref param))
						{
							new PlayerBussiness();
						}
						else
						{
							result = 4;
						}
					}
				}
				else
				{
					result = 3;
				}
				context.Response.Write(result);
			}
			catch (Exception exception)
			{
				log.Error("SentReward", exception);
			}
		}

		private bool checkParam(ref string param)
		{
			int num = 0;
			string text = "1";
			int num2 = 9;
			int num3 = 0;
			string text2 = "0";
			string b = "10";
			string b2 = "20";
			string b3 = "30";
			string b4 = "40";
			string text3 = "1";
			string b5 = "0";
			if (!string.IsNullOrEmpty(param))
			{
				string[] array = param.Split('|');
				int num4 = array.Length;
				if (num4 > 0)
				{
					param = "";
					int num5 = 0;
					string[] array2 = array;
					foreach (string text4 in array2)
					{
						char[] separator = new char[1]
						{
							','
						};
						string[] array3 = text4.Split(separator);
						if (array3.Length != 0)
						{
							array[num5] = "";
							array3[2] = ((int.Parse(array3[2]) < num || string.IsNullOrEmpty(array3[2].ToString())) ? text : array3[2]);
							array3[3] = ((int.Parse(array3[3].ToString()) < num3 || int.Parse(array3[3].ToString()) > num2 || string.IsNullOrEmpty(array3[3].ToString())) ? num3.ToString() : array3[3]);
							array3[4] = ((array3[4] == text2 || array3[4] == b || array3[4] == b2 || array3[4] == b3 || (array3[4] == b4 && !string.IsNullOrEmpty(array3[4].ToString()))) ? array3[4] : text2);
							array3[5] = ((array3[5] == text2 || array3[5] == b || array3[5] == b2 || array3[5] == b3 || (array3[5] == b4 && !string.IsNullOrEmpty(array3[5].ToString()))) ? array3[5] : text2);
							array3[6] = ((array3[6] == text2 || array3[6] == b || array3[6] == b2 || array3[6] == b3 || (array3[6] == b4 && !string.IsNullOrEmpty(array3[6].ToString()))) ? array3[6] : text2);
							array3[7] = ((array3[7] == text2 || array3[7] == b || array3[7] == b2 || array3[7] == b3 || (array3[7] == b4 && !string.IsNullOrEmpty(array3[7].ToString()))) ? array3[7] : text2);
							array3[8] = ((array3[8] == text3 || (array3[8] == b5 && !string.IsNullOrEmpty(array3[8]))) ? array3[8] : text3);
						}
						for (int j = 0; j < 9; j++)
						{
							array[num5] = array[num5] + array3[j] + ",";
						}
						array[num5] = array[num5].Remove(array[num5].Length - 1, 1);
						num5++;
					}
					for (int k = 0; k < num4; k++)
					{
						param = param + array[k] + "|";
					}
					param = param.Remove(param.Length - 1, 1);
					return true;
				}
			}
			return false;
		}
	}
}
