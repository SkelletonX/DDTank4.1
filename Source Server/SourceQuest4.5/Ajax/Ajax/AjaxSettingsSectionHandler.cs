using System.Configuration;
using System.Xml;

namespace Ajax
{
	internal class AjaxSettingsSectionHandler : IConfigurationSectionHandler
	{
		internal AjaxSettingsSectionHandler()
		{
		}

		public object Create(object parent, object configContext, XmlNode section)
		{
			AjaxSettings ajaxSettings = new AjaxSettings();
			foreach (XmlNode childNode in section.ChildNodes)
			{
				if (childNode.Name == "commonAjax")
				{
					if (childNode.SelectSingleNode("@enabled") != null && childNode.SelectSingleNode("@enabled").InnerText == "true")
					{
						if (childNode.SelectSingleNode("@path") != null && childNode.SelectSingleNode("@path").InnerText != "")
						{
							ajaxSettings.CommonScript = childNode.SelectSingleNode("@path").InnerText;
						}
						if (childNode.SelectSingleNode("@language") != null && childNode.SelectSingleNode("@language").InnerText != "")
						{
							ajaxSettings.ScriptLanguage = childNode.SelectSingleNode("@language").InnerText;
						}
					}
				}
				else if (childNode.Name == "temporaryFiles")
				{
					string text = null;
					int num = -1;
					if (childNode.SelectSingleNode("@path") != null && childNode.SelectSingleNode("@path").InnerText != "")
					{
						text = childNode.SelectSingleNode("@path").InnerText;
					}
					if (childNode.SelectSingleNode("@deleteAfter") != null && childNode.SelectSingleNode("@deleteAfter").InnerText != "")
					{
						try
						{
							num = int.Parse(childNode.SelectSingleNode("@deleteAfter").InnerText);
						}
						catch
						{
						}
					}
					if (text != null || num >= 0)
					{
						if (text != null)
						{
							ajaxSettings.TemporaryFiles.Path = text;
						}
						if (num >= 0)
						{
							ajaxSettings.TemporaryFiles.DeleteAfter = num;
						}
					}
				}
				else if (childNode.Name == "urlNamespaceMappings")
				{
					foreach (XmlNode item in childNode.SelectNodes("add"))
					{
						XmlNode xmlNode3 = item.SelectSingleNode("@namespace");
						XmlNode xmlNode4 = item.SelectSingleNode("@path");
						if (xmlNode3 != null && !(xmlNode3.InnerText == "") && xmlNode4 != null && !(xmlNode4.InnerText == ""))
						{
							ajaxSettings.UrlNamespaceMappings.Add(xmlNode4.InnerText, xmlNode3.InnerText);
						}
					}
				}
			}
			return ajaxSettings;
		}
	}
}
