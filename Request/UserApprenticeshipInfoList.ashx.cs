using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections.Specialized;
using log4net;
using System.Reflection;
using Bussiness;

namespace Tank.Request
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UserApprenticeshipInfoList : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            bool value = true;
            string message = "true!";
            bool isPlayerRegeisted=false;
            bool isSelfPublishEquip=false;
            XElement result = new XElement("Result");
            result.Add(new XAttribute("total", 0   ));
            result.Add(new XAttribute("value", value));
            result.Add(new XAttribute("message", message));
             result.Add(new XAttribute("isPlayerRegeisted", isPlayerRegeisted));
             result.Add(new XAttribute("isSelfPublishEquip", isSelfPublishEquip));
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
