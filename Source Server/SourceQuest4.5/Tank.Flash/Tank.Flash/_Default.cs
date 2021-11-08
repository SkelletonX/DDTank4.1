using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Tank.Flash
{
	public class _Default : Page
	{
		private string _content = "";

		private string autoParam = "";

		protected HtmlHead Head1;

		public string Content => _content;

		public string Edition => ConfigurationManager.AppSettings["Edition"].ToLower();

		public string LoginOnUrl => ConfigurationManager.AppSettings["LoginOnUrl"];

		public string SiteTitle
		{
			get
			{
				if (ConfigurationManager.AppSettings["SiteTitle"] != null)
				{
					return ConfigurationManager.AppSettings["SiteTitle"];
				}
				return "弹弹堂";
			}
		}

		public long Rand => DateTime.Now.Ticks;

		public string AutoParam => autoParam;

		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				string text = HttpUtility.UrlDecode(base.Request["user"]);
				string text2 = HttpUtility.UrlDecode(base.Request["key"]);
				string text3 = (base.Request["site"] == null) ? "" : HttpUtility.UrlDecode(base.Request["site"]).ToLower();
				string str = (base.Request["sitename"] == null) ? "" : HttpUtility.UrlDecode(base.Request["sitename"]);
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
				{
					if (!string.IsNullOrEmpty(text3) && ((ConfigurationManager.AppSettings["Site"] == null) ? "" : ConfigurationManager.AppSettings["Site"].ToString().ToLower()) == "true")
					{
						text = $"{text3}_{text}";
					}
					_content = "user=" + HttpUtility.UrlEncode(text) + "&key=" + HttpUtility.UrlEncode(text2);
					autoParam = "site=" + HttpUtility.UrlEncode(text3) + "&sitename=" + HttpUtility.UrlEncode(str);
				}
				else
				{
					base.Response.Redirect(LoginOnUrl, endResponse: false);
				}
			}
			catch
			{
				base.Response.Redirect(LoginOnUrl, endResponse: false);
			}
		}
	}
}
