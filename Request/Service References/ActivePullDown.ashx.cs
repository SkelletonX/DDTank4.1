using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using log4net;
using System.Reflection;
using Bussiness;
using Bussiness.CenterService;
using SqlDataProvider.Data;
using System.Security.Cryptography;
using System.Text;
using Road.Flash;
using System.Configuration;

namespace Tank.Request
{
    /// <summary>
    /// Summary description for ActivePullDown
    /// </summary>
    public class ActivePullDown : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            LanguageMgr.Setup(ConfigurationManager.AppSettings["ReqPath"]);
            //    activepulldown.ashx?
            int selfid = Convert.ToInt32(context.Request["selfid"]);//    selfid=19&
            int activeID = Convert.ToInt32(context.Request["activeID"]);//    activeID=21709&
            string key = context.Request["key"];//    key=db5e742130d3eec405df4408ff982fa8&
            string activeKey = context.Request["activeKey"];//    activeKey=KYbdx04Pv5JktjPqKbTlNcGQS5zhKg9o2xEkjcq4Vsde09L1oMKYkzM84WsfSTaJEho7CtbtUJtwouJeD4YRDr5AXJj3bPEHdsimIj8SCmAqhej1EyLVCtZ2NP0E5UdxruePXev46CsuV0bRnVUIjICb%2BHmVEL2rZOvL5smr5b0%3            
            bool value = false;
            string message = "ActivePullDownHandler.Fail";
            string awardID = "";
            XElement result = new XElement("Result");
            if (activeKey != "")
            {
                byte[] src = CryptoHelper.RsaDecryt2(StaticFunction.RsaCryptor, activeKey);
                awardID = Encoding.UTF8.GetString(src, 0, src.Length);
            }
            try
            {

                using (ActiveBussiness activeBussiness = new ActiveBussiness())
                {
                    if (activeBussiness.PullDown(activeID, awardID, selfid, ref message) == 0)
                    {
                        using (CenterServiceClient client = new CenterServiceClient())
                        {
                            client.MailNotice(selfid);
                        }
                    }
                }

                value = true;
                message = LanguageMgr.GetTranslation(message); //"Success!";
            }
            catch (Exception ex)
            {
                log.Error("ActivePullDown", ex);
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