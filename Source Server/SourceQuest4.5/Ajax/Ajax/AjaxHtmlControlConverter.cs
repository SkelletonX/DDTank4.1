using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Ajax
{
	internal class AjaxHtmlControlConverter
	{
		internal static string ControlToString(Control control)
		{
			StringWriter stringWriter = new StringWriter(new StringBuilder());
			control.RenderControl(new HtmlTextWriter(stringWriter));
			return stringWriter.ToString();
		}

		internal static HtmlControl HtmlToHtmlControl(string html, Type htmlControlType)
		{
			object obj = Activator.CreateInstance(htmlControlType);
			if (obj is HtmlControl)
			{
				html = AddRunAtServer(html, (obj as HtmlControl).TagName);
				TemplateControl templateControl = new UserControl();
				Control control = templateControl.ParseControl(html);
				HtmlControl result = null;
				if ((object)control.GetType() == htmlControlType)
				{
					return control as HtmlControl;
				}
				{
					foreach (Control control2 in control.Controls)
					{
						if ((object)control2.GetType() == htmlControlType)
						{
							return control2 as HtmlControl;
						}
					}
					return result;
				}
			}
			throw new InvalidCastException("The target-type is not a HtmlControlType");
		}

		internal static string AddRunAtServer(string input, string tagName)
		{
			string pattern = "<" + Regex.Escape(tagName) + "[^>]*?(?<InsertPos>\\s*)>";
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			Match match = regex.Match(input);
			if (match.Success)
			{
				Group group = match.Groups["InsertPos"];
				return input.Insert(group.Index + group.Length, " runat=\"server\"");
			}
			return input;
		}
	}
}
