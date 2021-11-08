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
using log4net;
using System.Reflection;
using Road.Flash;

namespace Tank.Request
{
    /// <summary>
    /// Summary description for UserRankDate
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UserRankDate : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void ProcessRequest(HttpContext context)
        {
            //userrankdate.ashx?userID=18&ConsortiaID=8
            bool value = false;
            string message = "Fail!";

            XElement result = new XElement("Result");
            try
            {
                string userID = HttpUtility.UrlDecode(context.Request["userID"]);
                string ConsortiaID = HttpUtility.UrlDecode(context.Request["ConsortiaID"]);
                using (PlayerBussiness db = new PlayerBussiness())
                {
                    UserRankDateInfo info = db.GetUserRankDateByID(int.Parse(userID));
                    if (info != null)
                    {
                        result.Add(FlashUtils.CreateUserRankDateItems(info));
                        value = true;
                        message = "Success!";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("UserRankDate", ex);
            }
            finally
            {
                if (value == true)
                {
                    result.Add(new XAttribute("value", value));
                    result.Add(new XAttribute("message", message));
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(result.ToString(false));
                }
            }
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