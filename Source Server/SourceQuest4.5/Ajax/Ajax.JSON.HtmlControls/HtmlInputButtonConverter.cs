using System;
using System.Web.UI.HtmlControls;

namespace Ajax.JSON.HtmlControls
{
	internal sealed class HtmlInputButtonConverter : HtmlControlConverter
	{
		public override Type[] SupportedTypes => new Type[1]
		{
			typeof(HtmlInputButton)
		};
	}
}
