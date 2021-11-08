using log4net;
using System.Reflection;
using System.Web;
using System.Web.Services;

namespace Tank.Request.CelebList
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByDayOfferList : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(Build(context));
		}

		public static string Build(HttpContext context)
		{
			if (!csFunction.ValidAdminIP(context.Request.UserHostAddress))
			{
				return "CelebByDayOfferList Fail!";
			}
			return Build();
		}

		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByDayOfferList", 4, "CelebByDayOfferList_Out");
		}
	}
}
