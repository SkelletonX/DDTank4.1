using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Bussiness.Interface;
using Bussiness;
using log4net;
using System.Reflection;
using Bussiness.CenterService;
using SqlDataProvider.Data;

namespace Tank.Request
{
    public partial class ChargeMoney : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string GetChargeIP
        {
            get
            {
                return ConfigurationSettings.AppSettings["ChargeIP"];
            }
        }

        public static bool ValidLoginIP(string ip)
        {
            string ips = GetChargeIP;
            if (string.IsNullOrEmpty(ips) || ips.Split('|').Contains(ip))
                return true;
            return false;
        } 

        protected void Page_Load(object sender, EventArgs e)
        {
            int result = 1;
            try
            {
                string IP = Context.Request.UserHostAddress;
                if (ValidLoginIP(IP))
                {
                    string content = HttpUtility.UrlDecode(Request["content"]);
                    string site = Request["site"] == null ? "" : HttpUtility.UrlDecode(Request["site"]).ToLower();
                    string nickname = Request["nickname"] == null ? "" : HttpUtility.UrlDecode(Request["nickname"]);

                    BaseInterface inter = BaseInterface.CreateInterface();
                    string[] str = inter.UnEncryptCharge(content, ref result, site);
                    if (str.Length > 5)
                    {
                        string chargeID = str[0];
                        string userName = str[1].Trim();
                        int money = int.Parse(str[2]);
                        string payWay = str[3];
                        decimal needMoney = decimal.Parse(str[4]);

                        if (!string.IsNullOrEmpty(userName))
                        {
                            userName= BaseInterface.GetNameBySite(userName, site);
                            if (money > 0)
                            {
                                using (PlayerBussiness db = new PlayerBussiness())
                                {
                                    int userID;
                                    DateTime date = DateTime.Now;
                                    if (db.AddChargeMoney(chargeID, userName, money, payWay, needMoney, out userID, ref result, date, IP, nickname))
                                    {
                                        result = 0;
                                        using (CenterServiceClient temp = new CenterServiceClient())
                                        {
                                            temp.ChargeMoney(userID, chargeID);
                                            using (PlayerBussiness pb = new PlayerBussiness())
                                            {
                                                PlayerInfo player = pb.GetUserSingleByUserID(userID);
                                                if (player != null)
                                                    StaticsMgr.Log(date, userName, player.Sex, money, payWay,needMoney);
                                                else
                                                {
                                                    StaticsMgr.Log(date, userName, true, money, payWay, needMoney);
                                                    log.Error("ChargeMoney_StaticsMgr:Player is null!");
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            else
                            {
                                result = 3;
                            }
                        }
                        else 
                        {
                            result = 2;
                        }
                    }
                }
                else
                {
                    result = 5;
                }
            }
            catch (Exception ex)
            {
                log.Error("ChargeMoney:", ex);
            }
            Response.Write(result);
        }
    }
}
