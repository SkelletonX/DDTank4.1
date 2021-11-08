using Bussiness;
using Bussiness.CenterService;
using log4net;
using Road.Flash;
using System;
using System.Configuration;
using System.Reflection;
using System.Web.UI;
using System.Xml.Linq;

namespace Tank.Request
{
	public class ServerList1 : Page
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static DateTime date = DateTime.Now;

		private static string xml = string.Empty;

		private static int OnlineTotal = 0;

		private static ServerData[] infos;

		public static string agentId => ConfigurationManager.AppSettings["ServerID"];

		protected void Page_Load(object sender, EventArgs e)
		{
			int num = (base.Request["id"] == null) ? (-1) : int.Parse(base.Request["id"]);
			if (infos == null || date.AddMinutes(5.0).CompareTo(DateTime.Now) < 0)
			{
				bool flag = false;
				string value = "Fail!";
				int num2 = 0;
				XElement xElement = new XElement("Result");
				try
				{
					using (CenterServiceClient centerServiceClient = new CenterServiceClient())
					{
						infos = centerServiceClient.GetServerList();
						date = DateTime.Now;
					}
					ServerData[] array = infos;
					foreach (ServerData serverData in array)
					{
						if (serverData.State != -1)
						{
							num2 += serverData.Online;
							xElement.Add(FlashUtils.CreateServerInfo(serverData.Id, serverData.Name, serverData.Ip, serverData.Port, serverData.State, serverData.MustLevel, serverData.LowestLevel, serverData.Online));
						}
					}
					flag = true;
					value = "Success!";
				}
				catch (Exception exception)
				{
					log.Error("ServerList1 error:", exception);
				}
				OnlineTotal = num2;
				xElement.Add(new XAttribute("value", flag));
				xElement.Add(new XAttribute("message", value));
				xElement.Add(new XAttribute("total", num2));
				xElement.Add(new XAttribute("agentId", agentId));
				xElement.Add(new XAttribute("AreaName", "an" + agentId));
				xElement.Add(new XAttribute("Info", agentId));
				xml = xElement.ToString(check: false);
			}
			string s = "0";
			if (num == 0)
			{
				s = OnlineTotal.ToString();
			}
			else if (num > 0)
			{
				ServerData[] array = infos;
				foreach (ServerData serverData2 in array)
				{
					if (serverData2.Id == num)
					{
						s = serverData2.Online.ToString();
						break;
					}
				}
			}
			else
			{
				s = xml;
			}
			base.Response.Write(s);
		}
	}
}
