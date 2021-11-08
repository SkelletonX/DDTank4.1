using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Reflection;
using System.Web;
using System.Xml.Linq;

namespace Tank.Request
{
	public class gmtipallbyids : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string value = "Fail!";
			XElement xElement = new XElement("Result");
			try
			{
				string text = context.Request["ids"];
				string[] array = null;
				if (!string.IsNullOrEmpty(text))
				{
					array = text.Split(',');
				}
				if (array != null)
				{
					using (ProduceBussiness produceBussiness = new ProduceBussiness())
					{
						EdictumInfo[] allEdictum = produceBussiness.GetAllEdictum();
						foreach (EdictumInfo edictumInfo in allEdictum)
						{
							edictumInfo.ID = int.Parse(array[0]);
							DateTime date = edictumInfo.EndDate.Date;
							DateTime date2 = DateTime.Now.Date;
							if (date > date2)
							{
								xElement.Add(FlashUtils.CreateEdictum(edictumInfo));
							}
						}
						flag = true;
						value = "Success!";
					}
				}
			}
			catch (Exception ex)
			{
				value = ex.ToString();
			}
			finally
			{
				xElement.Add(new XAttribute("value", flag));
				xElement.Add(new XAttribute("message", value));
				context.Response.ContentType = "text/plain";
				context.Response.Write(xElement.ToString(check: false));
			}
		}
	}
}
