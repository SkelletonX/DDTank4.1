using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Bussiness.Managers;
using Bussiness;
using Bussiness.Interface;
using System.Configuration;

namespace Tank.Request
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ChargeTest : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string chargeID = context.Request["chargeID"];
                string userName = context.Request["userName"];
                int money = int.Parse(context.Request["money"]);
                string payWay = context.Request["payWay"];
                decimal needMoney = decimal.Parse(context.Request["needMoney"]);
                string nickname = context.Request["nickname"] == null ? "" : HttpUtility.UrlDecode(context.Request["nickname"]);
                string site = "";

                QYInterface qy = new QYInterface();

                string key = string.Empty;
                if (!string.IsNullOrEmpty(site))
                {
                    key = ConfigurationSettings.AppSettings[string.Format("ChargeKey_{0}", site)];
                }
                else
                {
                    key = BaseInterface.GetChargeKey;
                }

                string v = BaseInterface.md5(chargeID + userName + money + payWay + needMoney + key);
                string Url = "http://192.168.0.4:828/ChargeMoney.aspx?content=" + chargeID + "|" + userName + "|" + money + "|" + payWay + "|" + needMoney + "|" + v;
                Url += "&site=" + site;
                Url += "&nickname=" + HttpUtility.UrlEncode(nickname);
                context.Response.Write(BaseInterface.RequestContent(Url));

                //int userid = 0;
                //int isResult = 0;
                //using (PlayerBussiness db = new PlayerBussiness())
                //{
                //    db.AddChargeMoney(chargeID, "dandan", 1000, "sdf", 200, out userid, ref isResult);
                //}
                //context.Response.Write(isResult);

            }
            catch 
            {
                context.Response.Write("false");
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
