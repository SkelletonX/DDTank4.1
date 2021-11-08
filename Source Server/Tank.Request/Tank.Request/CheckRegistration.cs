using log4net;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Xml.Linq;
using zlib;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CheckRegistration : IHttpHandler, IRequiresSessionState
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = true;
			string value = "Registered!";
			XElement xElement = new XElement("Result");
			int num = 1;
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			xElement.Add(new XAttribute("status", num));
			context.Response.ContentType = "text/plain";
			context.Response.BinaryWrite(StaticFunction.Compress(xElement.ToString()));
		}

		public static byte[] Compress(byte[] data)
		{
			MemoryStream memoryStream = new MemoryStream();
			ZOutputStream zOutputStream = new ZOutputStream(memoryStream, 3);
			zOutputStream.Write(data, 0, data.Length);
			zOutputStream.Flush();
			zOutputStream.Close();
			return memoryStream.ToArray();
		}
	}
}
