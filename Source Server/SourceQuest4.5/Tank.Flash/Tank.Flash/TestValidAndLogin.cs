using Bussiness.Interface;
using System;
using System.Configuration;
using System.Web;
using System.Web.UI;

namespace Tank.Flash
{
	public class TestValidAndLogin : Page
	{
		public static string FlashUrl => ConfigurationManager.AppSettings["FlashUrl"];

		protected void Page_Load(object sender, EventArgs e)
		{
			string text = "";
			try
			{
				string text2 = "";
				if (base.Request["id"] != null)
				{
					_ = base.Request["id"];
				}
				string text3 = base.Request["username"];
				string text4 = (base.Request["password"] == null) ? "" : base.Request["password"];
				text4 = Guid.NewGuid().ToString();
				string text5 = "1236319807954";
				string text6 = string.Empty;
				if (string.IsNullOrEmpty(text6))
				{
					text6 = BaseInterface.GetLoginKey;
				}
				string text7 = BaseInterface.md5(text3 + text4 + text5.ToString() + text6);
				text = BaseInterface.RequestContent(string.Concat(BaseInterface.LoginUrl + "?content=" + HttpUtility.UrlEncode(text3 + "|" + text4 + "|" + text5.ToString() + "|" + text7), "&site=", text2));
				if (text == "0")
				{
					string s = FlashUrl + "?user=" + HttpUtility.UrlEncode(text3) + "&key=" + HttpUtility.UrlEncode(text4) + "&site=" + text2 + "&sitename=" + text2;
					base.Response.Write(s);
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
