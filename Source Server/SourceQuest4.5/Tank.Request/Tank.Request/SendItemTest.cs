using System;
using System.Web;
using System.Web.UI;

namespace Tank.Request
{
	public class SendItemTest : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			HttpCookie httpCookie = base.Request.Cookies["userInfo"];
			string value = httpCookie.Value;
			_ = httpCookie.Values["bd_sig_user"];
			base.Response.Write(value);
		}
	}
}
