using Bussiness;
using Bussiness.Interface;
using log4net;
using Road.Flash;
using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class RenameConsortiaName : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string translation = LanguageMgr.GetTranslation("Tank.Request.RenameConsortiaName.Fail1");
			XElement xElement = new XElement("Result");
			try
			{
				BaseInterface.CreateInterface();
				string text = context.Request["p"];
				if (context.Request["site"] != null)
				{
					HttpUtility.UrlDecode(context.Request["site"]);
				}
				_ = context.Request.UserHostAddress;
				if (!string.IsNullOrEmpty(text))
				{
					byte[] array = CryptoHelper.RsaDecryt2(StaticFunction.RsaCryptor, text);
					string[] array2 = Encoding.UTF8.GetString(array, 7, array.Length - 7).Split(',');
					if (array2.Length == 5)
					{
						string name = array2[0];
						string pass = array2[1];
						string pass2 = array2[2];
						_ = array2[3];
						_ = array2[4];
						if (PlayerManager.Login(name, pass))
						{
							using (new ConsortiaBussiness())
							{
								PlayerManager.Update(name, pass2);
								flag = true;
								translation = LanguageMgr.GetTranslation("Tank.Request.RenameConsortiaName.Success");
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				log.Error("RenameConsortiaName", exception);
				flag = false;
				translation = LanguageMgr.GetTranslation("Tank.Request.RenameConsortiaName.Fail2");
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", translation));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
