using Bussiness;
using Bussiness.CenterService;
using Bussiness.Interface;
using log4net;
using Road.Flash;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ServerList : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static string agentId => ConfigurationManager.AppSettings["ServerID"];

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string value = "Fail!";
			int num = 0;
			XElement xElement = new XElement("Result");
			try
			{
				if (BaseInterface.CheckRnd(context.Request["rnd"]))
				{
					using (CenterServiceClient centerServiceClient = new CenterServiceClient())
					{
						foreach (ServerData item in (IEnumerable<ServerData>)centerServiceClient.GetServerList())
						{
							if (item.State != -1)
							{
								num += item.Online;
								xElement.Add(FlashUtils.CreateServerInfo(item.Id, item.Name, item.Ip, item.Port, item.State, item.MustLevel, item.LowestLevel, item.Online));
							}
						}
					}
				}
				flag = true;
				value = "Success!";
			}
			catch (Exception exception)
			{
				log.Error("Load server list error:", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			xElement.Add(new XAttribute("total", num));
			xElement.Add(new XAttribute("agentId", agentId));
			xElement.Add(new XAttribute("AreaName", "a" + agentId));
			xElement.Add(new XAttribute("Info", agentId));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
