using Bussiness.Interface;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Tank.Request
{
	public class SentRewardTest : Page
	{
		protected HtmlForm form1;

		protected void Page_Load(object sender, EventArgs e)
		{
			string text = "大幅度是";
			string text2 = "大幅度是";
			string text3 = "watson";
			string text4 = "6666";
			string text5 = "99999";
			string text6 = "11020,4,0,0,0,0,0,0,1|7014,2,9,400,400,400,400,400,0";
			string text7 = text + "#" + text2 + "#" + text3 + "#" + text4 + "#" + text5 + "#" + text6 + "#";
			DateTime now = DateTime.Now;
			string text8 = "asdfgh";
			string text9 = BaseInterface.md5(text3 + text4 + text5 + text6 + BaseInterface.ConvertDateTimeInt(now) + text8);
			base.Response.Redirect("http://192.168.0.4:828/SentReward.ashx?content=" + base.Server.UrlEncode(text7 + BaseInterface.ConvertDateTimeInt(now) + "#" + text9));
		}
	}
}
