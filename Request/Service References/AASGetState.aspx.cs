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
using log4net;
using System.Reflection;
using Bussiness.CenterService;

namespace Tank.Request
{
    public partial class AASGetState : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string GetAdminIP
        {
            get
            {
                return ConfigurationSettings.AppSettings["AdminIP"];
            }
        }

        public static bool ValidLoginIP(string ip)
        {
            string ips = GetAdminIP;
            if (string.IsNullOrEmpty(ips) || ips.Split('|').Contains(ip))
                return true;
            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int result = 2;
            try
            {
                if (ValidLoginIP(Context.Request.UserHostAddress))
                {
                    using (CenterServiceClient temp = new CenterServiceClient())
                    {
                        result = temp.AASGetState();
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error("ASSGetState:", ex);
            }
            Response.Write(result);
        }
    }
}
