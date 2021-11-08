using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class bombconfig : IHttpHandler
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
					BallConfigInfo[] allBallConfig = produceBussiness.GetAllBallConfig();
					foreach (BallConfigInfo b in allBallConfig)
					{
						xElement.Add(FlashUtils.CreateBallConfigInfo(b));
					}
				}
				flag = true;
				value = "Success!";
			}
			catch (Exception)
			{
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			return csFunction.CreateCompressXml(context, xElement, "bombconfig", isCompress: true);
		}
	}
}
