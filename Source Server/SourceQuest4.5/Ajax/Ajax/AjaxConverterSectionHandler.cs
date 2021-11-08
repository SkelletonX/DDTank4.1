using System;
using System.Configuration;
using System.Xml;

namespace Ajax
{
	internal class AjaxConverterSectionHandler : IConfigurationSectionHandler
	{
		internal AjaxConverterSectionHandler()
		{
		}

		public object Create(object parent, object configContext, XmlNode section)
		{
			AjaxConverterConfiguration ajaxConverterConfiguration = new AjaxConverterConfiguration();
			foreach (XmlNode childNode in section.ChildNodes)
			{
				if (childNode.SelectSingleNode("@type") != null)
				{
					Type type = Type.GetType(childNode.SelectSingleNode("@type").InnerText);
					if ((object)type != null)
					{
						if (childNode.Name == "add")
						{
							ajaxConverterConfiguration.Add(new AjaxConverterItem(type, AjaxConverterConfigurationAction.Add));
						}
						else if (childNode.Name == "remove")
						{
							ajaxConverterConfiguration.Add(new AjaxConverterItem(type, AjaxConverterConfigurationAction.Remove));
						}
					}
				}
			}
			return ajaxConverterConfiguration;
		}
	}
}
