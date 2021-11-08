using System;
using System.Web.UI;

namespace Tank.Flash
{
	public class index : Page
	{
		private string _content = "sdf";

		public string Content => _content;

		protected void Page_Load(object sender, EventArgs e)
		{
			_content = "kenken|123456";
		}
	}
}
