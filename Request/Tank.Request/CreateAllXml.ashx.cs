using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Text;

namespace Tank.Request
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CreateAllXml : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                StringBuilder bulid = new StringBuilder();
                bulid.Append(ActiveList.Bulid(context));
                bulid.Append(BallList.Bulid(context));
                bulid.Append(LoadMapsItems.Bulid(context));
                bulid.Append(LoadPVEItems.Build(context));
                bulid.Append(QuestList.Bulid(context));
                bulid.Append(TemplateAllList.Bulid(context));
                bulid.Append(ShopItemList.Bulid(context));

                bulid.Append(LoadItemsCategory.Bulid(context));
                bulid.Append(ItemStrengthenList.Bulid(context));
                bulid.Append(MapServerList.Bulid(context));
                bulid.Append(ConsortiaLevelList.Bulid(context));


                bulid.Append(DailyAwardList.Bulid(context));
                bulid.Append(NPCInfoList.Bulid(context));


                context.Response.ContentType = "text/plain";
                context.Response.Write(bulid.ToString());

            }
            else
            {
                context.Response.Write("IP is not valid!");
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
