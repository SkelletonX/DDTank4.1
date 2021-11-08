using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Reflection;
using System.Web;
using System.Xml.Linq;

namespace Tank.Request
{
	public class petskillelementinfo : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(Bulid(context));
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
					PetSkillElementInfo[] allPetSkillElementInfo = produceBussiness.GetAllPetSkillElementInfo();
					foreach (PetSkillElementInfo info in allPetSkillElementInfo)
					{
						xElement.Add(FlashUtils.CreatePetSkillElement(info));
					}
					flag = true;
					value = "Success!";
				}
			}
			catch (Exception exception)
			{
				log.Error("Load petskillelementinfo is fail!", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			return csFunction.CreateCompressXml(context, xElement, "petskillelementinfo", isCompress: false);
		}
	}
}
