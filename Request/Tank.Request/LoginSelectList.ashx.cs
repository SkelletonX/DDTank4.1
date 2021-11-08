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

namespace Tank.Request
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LoginSelectList : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            bool value = false;
            string message = "Fail!";

            XElement result = new XElement("Result");
            try
            {
                string username = HttpUtility.UrlDecode(context.Request["username"]);
                string password = HttpUtility.UrlDecode(context.Request["password"]);
                using (PlayerBussiness db = new PlayerBussiness())
                {
                    PlayerInfo[] infos = db.GetUserLoginList(username);

                    if (infos.Length > 0)
                    {
                        foreach (PlayerInfo info in infos)
                        {
                            if (string.IsNullOrEmpty(info.NickName) && info.Password != password)
                                continue;

                            result.Add(Road.Flash.FlashUtils.CreateUserLoginList(info));
                        }
                        value = true;
                        message = "Success!";
                    }
                }


            }
            catch (Exception ex)
            {
                log.Error("LoginSelectList", ex);
            }
            finally
            {
                result.Add(new XAttribute("value", value));
                result.Add(new XAttribute("message", message));
                context.Response.ContentType = "text/plain";
                context.Response.Write(result.ToString(false));
            }
            //context.Response.BinaryWrite(StaticFunction.Compress(result.ToString()));
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
