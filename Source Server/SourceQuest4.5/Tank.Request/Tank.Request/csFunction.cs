// Tank.Request.csFunction
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Tank.Request;

public class csFunction
{
	private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

	private static string[] al = ";|and|1=1|exec|insert|select|delete|update|like|count|chr|mid|master|or|truncate|char|declare|join".Split('|');

	public static string GetAdminIP => ConfigurationManager.AppSettings["AdminIP"];

	public static bool ValidAdminIP(string ip)
	{
		string getAdminIP = GetAdminIP;
		return string.IsNullOrEmpty(getAdminIP) || getAdminIP.Split('|').Contains(ip);
	}

	public static string ConvertSql(string inputString)
	{
		inputString = inputString.Trim().ToLower();
		inputString = inputString.Replace("'", "''");
		inputString = inputString.Replace(";--", "");
		inputString = inputString.Replace("=", "");
		inputString = inputString.Replace(" or", "");
		inputString = inputString.Replace(" or ", "");
		inputString = inputString.Replace(" and", "");
		inputString = inputString.Replace("and ", "");
		if (!SqlChar(inputString))
		{
			inputString = "";
		}
		return inputString;
	}

	public static bool SqlChar(string v)
	{
		if (v.Trim() != "")
		{
			string[] array = al;
			foreach (string text in array)
			{
				if (v.IndexOf(text + " ") > -1 || v.IndexOf(" " + text) > -1)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static string CreateCompressXml(HttpContext context, XElement result, string file, bool isCompress)
	{
		return CreateCompressXml(context.Server.MapPath("~"), result, file, isCompress);
	}

	public static string CreateCompressXml(XElement result, string file, bool isCompress)
	{
		return CreateCompressXml(StaticsMgr.CurrentPath, result, file, isCompress);
	}

	public static string CreateCompressXml(string path, XElement result, string file, bool isCompress)
	{
		try
		{
			file += ".xml";
			path = Path.Combine(path, file);
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				if (isCompress)
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
					{
						binaryWriter.Write(Tank.Request.StaticFunction.Compress(result.ToString(check: false)));
					}
				}
				else
				{
					using (StreamWriter streamWriter = new StreamWriter(fileStream))
					{
						streamWriter.Write(result.ToString(check: false));
					}
				}
			}
			return "Build:" + file + ",Success!";
		}
		catch (Exception exception)
		{
			log.Error("CreateCompressXml " + file + " is fail!", exception);
			return "Build:" + file + ",Fail!";
		}
	}

	public static string BuildCelebConsortia(string file, int order)
	{
		return BuildCelebConsortia(file, order, "");
	}

	public static string BuildCelebConsortia(string file, int order, string fileNotCompress)
	{
		bool flag = false;
		string value = "Fail!";
		XElement xElement = new XElement("Result");
		int total = 0;
		try
		{
			int page = 1;
			int size = 50;
			int consortiaID = -1;
			string name = "";
			int level = -1;
			using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
			{
				ConsortiaInfo[] consortiaPage = consortiaBussiness.GetConsortiaPage(page, size, ref total, order, name, consortiaID, level, -1);
				foreach (ConsortiaInfo consortiaInfo in consortiaPage)
				{
					XElement xElement2 = FlashUtils.CreateConsortiaInfo(consortiaInfo);
					if (consortiaInfo.ChairmanID != 0)
					{
						using (PlayerBussiness playerBussiness = new PlayerBussiness())
						{
							PlayerInfo userSingleByUserID = playerBussiness.GetUserSingleByUserID(consortiaInfo.ChairmanID);
							if (userSingleByUserID != null)
							{
								xElement2.Add(FlashUtils.CreateCelebInfo(userSingleByUserID));
							}
						}
					}
					xElement.Add(xElement2);
				}
				flag = true;
				value = "Success!";
			}
		}
		catch (Exception exception)
		{
			log.Error(file + " is fail!", exception);
		}
		xElement.Add(new XAttribute("total", total));
		xElement.Add(new XAttribute("value", flag));
		xElement.Add(new XAttribute("message", value));
		xElement.Add(new XAttribute("date", DateTime.Today.ToString("yyyy-MM-dd")));
		if (!string.IsNullOrEmpty(fileNotCompress))
		{
			CreateCompressXml(xElement, fileNotCompress, isCompress: false);
		}
		return CreateCompressXml(xElement, file, isCompress: true);
	}

	public static string BuildCelebUsers(string file, int order)
	{
		return BuildCelebUsers(file, order, "");
	}

	public static string BuildEliteMatchPlayerList(string file)
	{
		bool flag = false;
		string value = "Fail!";
		XElement xElement = new XElement("Result");
		try
		{
			int page = 1;
			int size = 50;
			int userID = -1;
			int total = 0;
			bool resultValue = false;
			using (PlayerBussiness playerBussiness = new PlayerBussiness())
			{
				PlayerInfo[] playerPage = playerBussiness.GetPlayerPage(page, size, ref total, 7, userID, ref resultValue);
				if (resultValue)
				{
					int num = 1;
					int num2 = 1;
					XElement xElement2 = new XElement("ItemSet", new XAttribute("value", 1));
					XElement xElement3 = new XElement("ItemSet", new XAttribute("value", 2));
					PlayerInfo[] array = playerPage;
					foreach (PlayerInfo playerInfo in array)
					{
						if (playerInfo.Grade <= 40)
						{
							xElement2.Add(FlashUtils.CreateEliteMatchPlayersList(playerInfo, num));
							num++;
						}
						else
						{
							xElement3.Add(FlashUtils.CreateEliteMatchPlayersList(playerInfo, num2));
							num2++;
						}
					}
					xElement.Add(xElement2);
					xElement.Add(xElement3);
					flag = true;
					value = "Success!";
				}
			}
		}
		catch (Exception exception)
		{
			log.Error(file + " is fail!", exception);
		}
		xElement.Add(new XAttribute("value", flag));
		xElement.Add(new XAttribute("message", value));
		xElement.Add(new XAttribute("lastUpdateTime", DateTime.Now.ToString()));
		return CreateCompressXml(xElement, file, isCompress: true);
	}

	public static string BuildCelebUsers(string file, int order, string fileNotCompress)
	{
		bool flag = false;
		string value = "Fail!";
		XElement xElement = new XElement("Result");
		try
		{
			int page = 1;
			int size = 50;
			int userID = -1;
			int total = 0;
			bool resultValue = false;
			using (PlayerBussiness playerBussiness = new PlayerBussiness())
			{
				if (order == 6)
				{
					playerBussiness.UpdateUserReputeFightPower();
				}
				PlayerInfo[] playerPage = playerBussiness.GetPlayerPage(page, size, ref total, order, userID, ref resultValue);
				if (resultValue)
				{
					PlayerInfo[] array = playerPage;
					foreach (PlayerInfo info in array)
					{
						xElement.Add(FlashUtils.CreateCelebInfo(info));
					}
					flag = true;
					value = "Success!";
				}
			}
		}
		catch (Exception exception)
		{
			log.Error(file + " is fail!", exception);
		}
		xElement.Add(new XAttribute("value", flag));
		xElement.Add(new XAttribute("message", value));
		xElement.Add(new XAttribute("date", DateTime.Today.ToString("yyyy-MM-dd")));
		if (!string.IsNullOrEmpty(fileNotCompress))
		{
			CreateCompressXml(xElement, fileNotCompress, isCompress: false);
		}
		return CreateCompressXml(xElement, file, isCompress: true);
	}

	public static string BuildCelebUsersMath(string file, string fileNotCompress)
	{
		bool flag = false;
		string value = "Fail!";
		XElement xElement = new XElement("Result");
		try
		{
			int page = 1;
			int size = 50;
			int total = 0;
			bool resultValue = false;
			using (PlayerBussiness playerBussiness = new PlayerBussiness())
			{
				PlayerInfo[] playerMathPage = playerBussiness.GetPlayerMathPage(page, size, ref total, ref resultValue);
				if (resultValue)
				{
					PlayerInfo[] array = playerMathPage;
					foreach (PlayerInfo info in array)
					{
						xElement.Add(FlashUtils.CreateCelebInfo(info));
					}
					flag = true;
					value = "Success!";
				}
			}
		}
		catch (Exception exception)
		{
			log.Error(file + " is fail!", exception);
		}
		xElement.Add(new XAttribute("value", flag));
		xElement.Add(new XAttribute("message", value));
		xElement.Add(new XAttribute("date", DateTime.Today.ToString("yyyy-MM-dd")));
		if (!string.IsNullOrEmpty(fileNotCompress))
		{
			CreateCompressXml(xElement, fileNotCompress, isCompress: false);
		}
		return CreateCompressXml(xElement, file, isCompress: true);
	}

	public static string BuildCelebConsortiaFightPower(string file, string fileNotCompress)
	{
		bool flag = false;
		string value = "Fail!";
		XElement xElement = new XElement("Result");
		int num = 0;
		try
		{
			using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
			{
				ConsortiaInfo[] array = consortiaBussiness.UpdateConsortiaFightPower();
				num = array.Length;
				ConsortiaInfo[] array2 = array;
				foreach (ConsortiaInfo consortiaInfo in array2)
				{
					XElement xElement2 = FlashUtils.CreateConsortiaInfo(consortiaInfo);
					if (consortiaInfo.ChairmanID != 0)
					{
						using (PlayerBussiness playerBussiness = new PlayerBussiness())
						{
							PlayerInfo userSingleByUserID = playerBussiness.GetUserSingleByUserID(consortiaInfo.ChairmanID);
							if (userSingleByUserID != null)
							{
								xElement2.Add(FlashUtils.CreateCelebInfo(userSingleByUserID));
							}
						}
					}
					xElement.Add(xElement2);
				}
				flag = true;
				value = "Success!";
			}
		}
		catch (Exception exception)
		{
			log.Error(file + " is fail!", exception);
		}
		xElement.Add(new XAttribute("total", num));
		xElement.Add(new XAttribute("value", flag));
		xElement.Add(new XAttribute("message", value));
		xElement.Add(new XAttribute("date", DateTime.Today.ToString("yyyy-MM-dd")));
		if (!string.IsNullOrEmpty(fileNotCompress))
		{
			CreateCompressXml(xElement, fileNotCompress, isCompress: false);
		}
		return CreateCompressXml(xElement, file, isCompress: true);
	}
}
