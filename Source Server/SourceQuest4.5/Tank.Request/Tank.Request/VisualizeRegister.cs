using Bussiness;
using log4net;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Tank.Request.Illegalcharacters;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class VisualizeRegister : IHttpHandler
	{
		private static FileSystem fileIllegal = new FileSystem(HttpContext.Current.Server.MapPath(IllegalCharacters), HttpContext.Current.Server.MapPath(IllegalDirectory), "*.txt");

		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static string IllegalCharacters => ConfigurationManager.AppSettings["IllegalCharacters"];

		public static string IllegalDirectory => ConfigurationManager.AppSettings["IllegalDirectory"];

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string msg = LanguageMgr.GetTranslation("Tank.Request.VisualizeRegister.Fail1");
			XElement xElement = new XElement("Result");
			try
			{
				NameValueCollection @params = context.Request.Params;
				string text = @params["Name"];
				string text2 = @params["Pass"];
				string text3 = @params["NickName"].Trim().Replace(",", "");
				_ = @params["Arm"];
				_ = @params["Hair"];
				_ = @params["Face"];
				_ = @params["Cloth"];
				_ = @params["Cloth"];
				_ = @params["ArmID"];
				_ = @params["HairID"];
				_ = @params["FaceID"];
				_ = @params["ClothID"];
				_ = @params["ClothID"];
				int num = -1;
				if (bool.Parse(ConfigurationManager.AppSettings["MustSex"]))
				{
					num = (bool.Parse(@params["Sex"]) ? 1 : 0);
				}
				if (Encoding.Default.GetByteCount(text3) <= 14)
				{
					if (!fileIllegal.checkIllegalChar(text3))
					{
						if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
						{
							string[] array = (num == 1) ? ConfigurationManager.AppSettings["BoyVisualizeItem"].Split(';') : ConfigurationManager.AppSettings["GrilVisualizeItem"].Split(';');
							string[] array2 = array;
							char[] separator = new char[1]
							{
								','
							};
							string text4 = array2[0].Split(separator)[0];
							char[] separator2 = new char[1]
							{
								','
							};
							string text5 = array2[0].Split(separator2)[1];
							char[] separator3 = new char[1]
							{
								','
							};
							string text6 = array2[0].Split(separator3)[2];
							char[] separator4 = new char[1]
							{
								','
							};
							string text7 = array2[0].Split(separator4)[3];
							char[] separator5 = new char[1]
							{
								','
							};
							string text8 = array2[0].Split(separator5)[4];
							string armColor = "";
							string hairColor = "";
							string faceColor = "";
							string clothColor = "";
							string hatColor = "";
							using (PlayerBussiness playerBussiness = new PlayerBussiness())
							{
								string text9 = text4 + "," + text5 + "," + text6 + "," + text7 + "," + text8;
								if (playerBussiness.RegisterPlayer(text, text2, text3, text9, text9, armColor, hairColor, faceColor, clothColor, hatColor, num, ref msg, int.Parse(ConfigurationManager.AppSettings["ValidDate"])))
								{
									flag = true;
									msg = LanguageMgr.GetTranslation("Tank.Request.VisualizeRegister.Success");
								}
							}
						}
						else
						{
							msg = LanguageMgr.GetTranslation("!string.IsNullOrEmpty(name) && !");
						}
					}
					else
					{
						msg = LanguageMgr.GetTranslation("Tank.Request.VisualizeRegister.Illegalcharacters");
					}
				}
				else
				{
					msg = LanguageMgr.GetTranslation("Tank.Request.VisualizeRegister.Long");
				}
			}
			catch (Exception exception)
			{
				log.Error("VisualizeRegister", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", msg));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
