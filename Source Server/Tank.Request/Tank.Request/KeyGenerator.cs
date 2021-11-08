using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class KeyGenerator : IHttpHandler
	{
		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			new CspParameters().Flags = CspProviderFlags.UseMachineKeyStore;
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(2048);
			RSAParameters rSAParameters = rSACryptoServiceProvider.ExportParameters(includePrivateParameters: true);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < rSAParameters.Modulus.Length; i++)
			{
				stringBuilder.Append(rSAParameters.Modulus[i].ToString("X2"));
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int j = 0; j < rSAParameters.Exponent.Length; j++)
			{
				stringBuilder2.Append(rSAParameters.Exponent[j].ToString("X2"));
			}
			XElement xElement = new XElement("list");
			XElement content = new XElement("private", new XAttribute("key", rSACryptoServiceProvider.ToXmlString(includePrivateParameters: true)));
			XElement content2 = new XElement("public", new XAttribute("model", stringBuilder.ToString()), new XAttribute("exponent", stringBuilder2.ToString()));
			xElement.Add(content);
			xElement.Add(content2);
			context.Response.Write(xElement.ToString());
		}
	}
}
