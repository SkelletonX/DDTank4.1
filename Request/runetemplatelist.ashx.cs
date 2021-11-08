using Bussiness;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
    /// <summary>
    /// Summary description for runetemplatelist
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class runetemplatelist : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                context.Response.Write(Bulid(context));
            }
            else
            {
                context.Response.Write("IP is not valid!");
            }
        }

        public static string Bulid(HttpContext context)
        {
            bool value = false;
            string message = "Fail!";
            XElement result = new XElement("Result");

            XElement group = new XElement("RuneTemplate");
            try
            {

                using (ProduceBussiness db = new ProduceBussiness())
                {
                    RuneTemplateInfo[] infos = db.GetAllRuneTemplate();
                    foreach (RuneTemplateInfo info in infos)
                    {
                        group.Add(Road.Flash.FlashUtils.CreateRuneTemplateInfo(info));
                    }
                }

                value = true;
                message = "Success!";
            }
            catch (Exception ex)
            {
                //log.Error("BallList", ex);
            }

            result.Add(new XAttribute("value", value));
            result.Add(new XAttribute("message", message));
            result.Add(group);

            //return result.ToString(false);
            csFunction.CreateCompressXml(context, result, "runetemplatelist_out", false);
            return csFunction.CreateCompressXml(context, result, "runetemplatelist", true);
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