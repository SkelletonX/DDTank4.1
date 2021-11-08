using Bussiness;
using System;
using System.Web;
using System.Xml.Linq;

namespace Tank.Request
{
	public class runetemplatelist
	{
		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
			{
				context.Response.Write(Bulid(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		public static string Bulid(HttpContext context)
		{
			bool flag = false;
			string value = "Fail!";
			XElement xElement = new XElement("Result");
			XElement content = new XElement("RuneTemplate");
			try
			{
				using (new ProduceBussiness())
				{
				}
				flag = true;
				value = "Success!";
			}
			catch (Exception)
			{
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			xElement.Add(content);
			csFunction.CreateCompressXml(context, xElement, "runetemplatelist_out", isCompress: false);
			return csFunction.CreateCompressXml(context, xElement, "runetemplatelist", isCompress: true);
		}
	}
}
