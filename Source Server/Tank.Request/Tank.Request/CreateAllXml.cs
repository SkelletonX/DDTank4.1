using System.Text;
using System.Web;
using System.Web.Services;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CreateAllXml : IHttpHandler
	{
		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(ActiveList.Bulid(context));
				stringBuilder.Append(BallList.Bulid(context));
				stringBuilder.Append(eventrewarditemlist.Bulid(context));
				stringBuilder.Append(bombconfig.Bulid(context));
				stringBuilder.Append(LoadMapsItems.Bulid(context));
				stringBuilder.Append(LoadPVEItems.Build(context));
				stringBuilder.Append(QuestList.Bulid(context));
				stringBuilder.Append(TemplateAllList.Bulid(context));
				stringBuilder.Append(ShopItemList.Bulid(context));
				stringBuilder.Append(ShopGoodsShowList.Bulid(context));
				stringBuilder.Append(LoadItemsCategory.Bulid(context));
				stringBuilder.Append(MapServerList.Bulid(context));
				stringBuilder.Append(ConsortiaLevelList.Bulid(context));
				stringBuilder.Append(DailyAwardList.Bulid(context));
				stringBuilder.Append(NPCInfoList.Bulid(context));
				context.Response.ContentType = "text/plain";
				context.Response.Write(stringBuilder.ToString());
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}
	}
}
