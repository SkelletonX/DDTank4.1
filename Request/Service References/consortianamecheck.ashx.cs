using Bussiness;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
    /// <summary>
    /// Summary description for consortianamecheck
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class consortianamecheck : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void ProcessRequest(HttpContext context)
        {
            LanguageMgr.Setup(ConfigurationManager.AppSettings["ReqPath"]);
            bool value = false;
            //string message = "Name is Exist!";
            string message = "Tên hội đã có người sử dụng.";
            XElement result = new XElement("Result");

            try
            {
                string nickName = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["NickName"]));
                if (System.Text.Encoding.Default.GetByteCount(nickName) <= 20)
                {
                    if (!string.IsNullOrEmpty(nickName))
                    {
                        using (ConsortiaBussiness db = new ConsortiaBussiness())
                        {
                            if (db.GetConsortiaByName(nickName) == null)
                            {
                                value = true;
                                message = "Chúc mừng! Tên hội đã có thể sử dụng.";
                            }
                        }
                    }
                }
                else
                {
                    message = "Tên hội quá dài";
                }
            }
            catch (Exception ex)
            {
                log.Error("NickNameCheck", ex);
                value = false;
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