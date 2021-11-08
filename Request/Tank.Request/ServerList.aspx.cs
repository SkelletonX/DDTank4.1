using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections.Generic;
using log4net;
using System.Reflection;
using Bussiness.CenterService;
using Bussiness;

namespace Tank.Request
{
    public partial class ServerList1 : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static ServerData[] infos;
        private static DateTime date = DateTime.Now;
        private static string xml = string.Empty;
        private static int OnlineTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Request["id"] == null ? -1 : int.Parse(Request["id"]);
            if (infos == null || date.AddMinutes(5).CompareTo(DateTime.Now) < 0)
            {
                bool value = false;
                string message = "Fail!";
                int total = 0;

                XElement result = new XElement("Result");

                try
                {
                    using (CenterServiceClient temp = new CenterServiceClient())
                    {
                        infos = temp.GetServerList();
                        date = DateTime.Now;
                    }

                    foreach (ServerData s in infos)
                    {
                        if (s.State == -1)
                            continue;

                        total += s.Online;
                        result.Add(Road.Flash.FlashUtils.CreateServerInfo(s.Id, s.Name, s.Ip, s.Port, s.State, s.MustLevel, s.LowestLevel, s.Online));
                    }

                    value = true;
                    message = "Success!";
                }
                catch (Exception ex)
                {
                    log.Error("ServerList1 error:", ex);
                }

                OnlineTotal = total;
                result.Add(new XAttribute("value", value));
                result.Add(new XAttribute("message", message));
                result.Add(new XAttribute("total", total));
                xml = result.ToString(false);

            }

            string query = "0";
            if (id == 0)
            {
                query = OnlineTotal.ToString();
            }
            else if (id > 0)
            {
                foreach (ServerData info in infos)
                {
                    if (info.Id == id)
                    {
                        query = info.Online.ToString();
                        break;
                    }
                }
            }
            else
            {
                query = xml;
            }

            Response.Write(query);

            //Response.Write(result.ToString(false));
        }
    }
}
