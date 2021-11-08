using Bussiness.Interface;
using System;
using System.Configuration;
using System.Web;
using System.Web.UI;

namespace Tank.Flash
{
	public class LoginGame : Page
	{
		public static string FlashUrl => ConfigurationManager.AppSettings["FlashUrl"];

		protected void Page_Load(object sender, EventArgs e)
		{
			string text = "";
			try
			{
				string text2 = base.Request.Form["txtSite"];
				string text3 = base.Request.Form["txtUserName"];
				string text4 = base.Request.Form["txtPassword"];
				string text5 = "1236319807954";
				string text6 = string.Empty;
				if (!string.IsNullOrEmpty(text2))
				{
					text6 = ConfigurationManager.AppSettings[$"LoginKey_{text2}"];
				}
				if (string.IsNullOrEmpty(text6))
				{
					text6 = BaseInterface.GetLoginKey;
				}
				string text7 = BaseInterface.md5(text3 + text4 + text5.ToString() + text6);
				text = BaseInterface.RequestContent(string.Concat(BaseInterface.LoginUrl + "?content=" + HttpUtility.UrlEncode(text3 + "|" + text4 + "|" + text5.ToString() + "|" + text7), "&site=", text2));
				if (text == "0")
				{
					string url = FlashUrl + "?user=" + HttpUtility.UrlEncode(text3) + "&key=" + HttpUtility.UrlEncode(text4) + "&site=" + text2 + "&sitename=" + text2;
					base.Response.Redirect(url, endResponse: false);
				}
				else
				{
					base.Response.Write(text);
				}
			}
			catch (Exception ex)
			{
				base.Response.Write(ex.ToString());
			}
		}
	}
}
