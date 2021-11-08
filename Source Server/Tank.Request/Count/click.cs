using Ajax;
using Bussiness;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Count
{
	public class click : Page
	{
		protected HtmlForm form1;

		protected void Page_Load(object sender, EventArgs e)
		{
			Utility.RegisterTypeForAjax(typeof(click));
		}

		[AjaxMethod]
		public string Logoff(string App_Id, string Direct_Url, string Referry_Url, string Begin_time, string ScreenW, string ScreenH, string Color, string Flash)
		{
			HttpContext current = HttpContext.Current;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			try
			{
				dictionary.Add("Application_Id", App_Id);
				string userHostAddress = current.Request.UserHostAddress;
				string text = (current.Request.UserAgent == null) ? "无" : current.Request.UserAgent;
				if (current.Request.ServerVariables["HTTP_UA_CPU"] == null)
				{
					dictionary.Add("CPU", "未知");
				}
				else
				{
					dictionary.Add("CPU", current.Request.ServerVariables["HTTP_UA_CPU"]);
				}
				dictionary.Add("OperSystem", GetOSNameByUserAgent(text));
				dictionary.Add("IP", userHostAddress);
				dictionary.Add("IPAddress", userHostAddress);
				if (current.Request.Browser.ClrVersion == null)
				{
					dictionary.Add(".NETCLR", "不支持");
				}
				else
				{
					dictionary.Add("NETCLR", current.Request.Browser.ClrVersion.ToString());
				}
				dictionary.Add("Browser", current.Request.Browser.Browser + current.Request.Browser.Version);
				dictionary.Add("ActiveX", current.Request.Browser.ActiveXControls ? "True" : "False");
				dictionary.Add("Cookies", current.Request.Browser.Cookies ? "True" : "False");
				dictionary.Add("CSS", current.Request.Browser.SupportsCss ? "True" : "False");
				dictionary.Add("Language", current.Request.UserLanguages[0]);
				string text2 = current.Request.ServerVariables["HTTP_ACCEPT"];
				if (text2 == null)
				{
					dictionary.Add("Computer", "False");
				}
				else if (text2.IndexOf("wap") > -1)
				{
					dictionary.Add("Computer", "False");
				}
				else
				{
					dictionary.Add("Computer", "True");
				}
				dictionary.Add("Platform", current.Request.Browser.Platform);
				dictionary.Add("Win16", current.Request.Browser.Win16 ? "True" : "False");
				dictionary.Add("Win32", current.Request.Browser.Win32 ? "True" : "False");
				if (current.Request.ServerVariables["HTTP_ACCEPT_ENCODING"] == null)
				{
					dictionary.Add("AcceptEncoding", "无");
				}
				else
				{
					dictionary.Add("AcceptEncoding", current.Request.ServerVariables["HTTP_ACCEPT_ENCODING"]);
				}
				dictionary.Add("UserAgent", text);
				dictionary.Add("Referry", Referry_Url);
				dictionary.Add("Redirect", Direct_Url);
				dictionary.Add("TimeSpan", Begin_time.ToString());
				dictionary.Add("ScreenWidth", ScreenW);
				dictionary.Add("ScreenHeight", ScreenH);
				dictionary.Add("Color", Color);
				dictionary.Add("Flash", Flash);
				CountBussiness.InsertContentCount(dictionary);
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}
			return "ok";
		}

		private static string GetOSNameByUserAgent(string userAgent)
		{
			string result = "未知";
			if (userAgent.Contains("NT 6.0"))
			{
				result = "Windows Vista/Server 2008";
			}
			else if (userAgent.Contains("NT 5.2"))
			{
				result = "Windows Server 2003";
			}
			else if (userAgent.Contains("NT 5.1"))
			{
				result = "Windows XP";
			}
			else if (userAgent.Contains("NT 5"))
			{
				result = "Windows 2000";
			}
			else if (userAgent.Contains("NT 4"))
			{
				result = "Windows NT4";
			}
			else if (userAgent.Contains("Me"))
			{
				result = "Windows Me";
			}
			else if (userAgent.Contains("98"))
			{
				result = "Windows 98";
			}
			else if (userAgent.Contains("95"))
			{
				result = "Windows 95";
			}
			else if (userAgent.Contains("Mac"))
			{
				result = "Mac";
			}
			else if (userAgent.Contains("Unix"))
			{
				result = "UNIX";
			}
			else if (userAgent.Contains("Linux"))
			{
				result = "Linux";
			}
			else if (userAgent.Contains("SunOS"))
			{
				result = "SunOS";
			}
			return result;
		}
	}
}
