using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class QuestList : IHttpHandler
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
					QuestInfo[] aLlQuest = produceBussiness.GetALlQuest();
					QuestAwardInfo[] allQuestGoods = produceBussiness.GetAllQuestGoods();
					QuestConditionInfo[] allQuestCondiction = produceBussiness.GetAllQuestCondiction();
					QuestInfo[] array = aLlQuest;
					foreach (QuestInfo questInfo in array)
					{
						QuestInfo quest = questInfo;
						XElement xElement2 = FlashUtils.CreateQuestInfo(quest);
						foreach (QuestConditionInfo item in (IEnumerable)allQuestCondiction.Where((QuestConditionInfo s) => s.QuestID == quest.ID))
						{
							xElement2.Add(FlashUtils.CreateQuestCondiction(item));
						}
						foreach (QuestAwardInfo item2 in (IEnumerable)allQuestGoods.Where((QuestAwardInfo s) => s.QuestID == quest.ID))
						{
							xElement2.Add(FlashUtils.CreateQuestGoods(item2));
						}
						xElement.Add(xElement2);
					}
					flag = true;
					value = "Success!";
				}
			}
			catch (Exception exception)
			{
				log.Error("QuestList", exception);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			return csFunction.CreateCompressXml(context, xElement, "QuestList", isCompress: true);
		}

		private static void AppendAttribute(XmlDocument doc, XmlNode node, string attr, string value)
		{
			XmlAttribute xmlAttribute = doc.CreateAttribute(attr);
			xmlAttribute.Value = value;
			node.Attributes.Append(xmlAttribute);
		}
	}
}
