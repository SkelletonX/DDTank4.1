using System;
using System.Web.UI.HtmlControls;

namespace Ajax.JSON.HtmlControls
{
	internal sealed class HtmlTableConverter : HtmlControlConverter
	{
		public override Type[] SupportedTypes => new Type[1]
		{
			typeof(HtmlTable)
		};
	}
}
