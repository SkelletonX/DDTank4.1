using System;
using System.Web.UI;

namespace Tank.Flash
{
	public class BeginLoad : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["Loading"] == null)
			{
				LoadingManager.LoadingCount++;
				Session["Loading"] = false;
			}
		}
	}
}
