using Bussiness.Interface;
using System.Configuration;
using System.Web;
using System.Web.Services;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ChargeTest : IHttpHandler
	{
		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			try
			{
				string text = context.Request["chargeID"];
				string text2 = context.Request["userName"];
				int num = int.Parse(context.Request["money"]);
				string text3 = context.Request["payWay"];
				decimal num2 = decimal.Parse(context.Request["needMoney"]);
				string str = (context.Request["nickname"] == null) ? "" : HttpUtility.UrlDecode(context.Request["nickname"]);
				string text4 = "";
				new QYInterface();
				_ = string.Empty;
				string text5 = string.IsNullOrEmpty(text4) ? BaseInterface.GetChargeKey : ConfigurationManager.AppSettings[$"ChargeKey_{text4}"];
				string text6 = BaseInterface.md5(text + text2 + num + text3 + num2 + text5);
				string url = "http://192.168.0.4:828/ChargeMoney.aspx?content=" + text + "|" + text2 + "|" + num + "|" + text3 + "|" + num2 + "|" + text6 + "&site=" + text4 + "&nickname=" + HttpUtility.UrlEncode(str);
				context.Response.Write(BaseInterface.RequestContent(url));
			}
			catch
			{
				context.Response.Write("false");
			}
		}
	}
}
