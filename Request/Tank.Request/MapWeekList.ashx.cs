using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Bussiness;
using SqlDataProvider.Data;
using Road.Flash;
using log4net;
using System.Reflection;

namespace Tank.Request
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class MapWeekList : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            bool value = false;
            string message = "获取失败!";
            XElement result = new XElement("Result");

            try
            {
                using (MapBussiness db = new MapBussiness())
                {
                    MapWeekInfo[] infos = db.GetAllMapWeek();
                    foreach (MapWeekInfo info in infos)
                    {
                        result.Add(FlashUtils.CreateMapWeek(info));
                    }

                    value = true;
                    message = "获取成功!";
                }
            }
            catch (Exception ex)
            {
                log.Error("加载地图周期失败", ex);
            }

            result.Add(new XAttribute("value", value));
            result.Add(new XAttribute("message", message));

            context.Response.ContentType = "text/plain";
            context.Response.Write(result.ToString(false));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
