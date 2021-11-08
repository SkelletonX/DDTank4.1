using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;
using System.Web;
using System.Xml.Linq;

namespace Tank.Request
{
	public class ShopGoodsShowList : IHttpHandler
	{
		public bool IsReusable => false;

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
			bool flag = false;
			string value = "Fail!";
			XElement xElement = new XElement("Result");
			try
			{
				using (ProduceBussiness produceBussiness = new ProduceBussiness())
				{
					XElement xElement2 = new XElement("Store");
					ShopGoodsShowListInfo[] allShopGoodsShowList = produceBussiness.GetAllShopGoodsShowList();
					foreach (ShopGoodsShowListInfo shop in allShopGoodsShowList)
					{
						xElement2.Add(FlashUtils.CreateShopShowInfo(shop));
					}
					xElement.Add(xElement2);
					flag = true;
					value = "Success!";
				}
			}
			catch
			{
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			return csFunction.CreateCompressXml(context, xElement, "ShopGoodsShowList", isCompress: true);
		}
	}
}
