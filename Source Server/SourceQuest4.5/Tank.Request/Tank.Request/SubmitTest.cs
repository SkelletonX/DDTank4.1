using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Tank.Request
{
	public class SubmitTest : Page
	{
		protected HtmlForm form1;

		protected TextBox TextBox1;

		protected Button Button1;

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			base.Response.Redirect("/LoginTest.aspx?name=" + TextBox1.Text);
		}
	}
}
