using System;

namespace Ajax
{
	internal class AjaxConverterItem
	{
		internal Type ConverterType = null;

		internal AjaxConverterConfigurationAction Action = AjaxConverterConfigurationAction.Add;

		internal AjaxConverterItem(Type converterType, AjaxConverterConfigurationAction action)
		{
			ConverterType = converterType;
			Action = action;
		}
	}
}
