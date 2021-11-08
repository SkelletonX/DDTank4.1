using Bussiness;
using System.Web;
using System.Xml.Linq;

namespace Tank.Request
{
	public class shopcheapitemlist : IHttpHandler
	{
		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = true;
			string value = "Success!";
			XElement xElement = new XElement("Result");
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
