using Bussiness;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;

namespace Tank.Request
{
	public class eventrewarditemlist : IHttpHandler
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
					Dictionary<int, Dictionary<int, EventRewardInfo>> dictionary = new Dictionary<int, Dictionary<int, EventRewardInfo>>();
					EventRewardInfo[] allEventRewardInfo = produceBussiness.GetAllEventRewardInfo();
					EventRewardGoodsInfo[] allEventRewardGoods = produceBussiness.GetAllEventRewardGoods();
					EventRewardInfo[] array = allEventRewardInfo;
					foreach (EventRewardInfo eventRewardInfo in array)
					{
						eventRewardInfo.AwardLists = new List<EventRewardGoodsInfo>();
						if (!dictionary.ContainsKey(eventRewardInfo.ActivityType))
						{
							Dictionary<int, EventRewardInfo> dictionary2 = new Dictionary<int, EventRewardInfo>();
							dictionary2.Add(eventRewardInfo.SubActivityType, eventRewardInfo);
							dictionary.Add(eventRewardInfo.ActivityType, dictionary2);
						}
						else if (!dictionary[eventRewardInfo.ActivityType].ContainsKey(eventRewardInfo.SubActivityType))
						{
							dictionary[eventRewardInfo.ActivityType].Add(eventRewardInfo.SubActivityType, eventRewardInfo);
						}
					}
					EventRewardGoodsInfo[] array2 = allEventRewardGoods;
					foreach (EventRewardGoodsInfo eventRewardGoodsInfo in array2)
					{
						if (dictionary.ContainsKey(eventRewardGoodsInfo.ActivityType) && dictionary[eventRewardGoodsInfo.ActivityType].ContainsKey(eventRewardGoodsInfo.SubActivityType))
						{
							dictionary[eventRewardGoodsInfo.ActivityType][eventRewardGoodsInfo.SubActivityType].AwardLists.Add(eventRewardGoodsInfo);
						}
					}
					XElement xElement2 = null;
					foreach (Dictionary<int, EventRewardInfo> value2 in dictionary.Values)
					{
						foreach (EventRewardInfo value3 in value2.Values)
						{
							if (xElement2 == null)
							{
								xElement2 = new XElement("ActivityType", new XAttribute("value", value3.ActivityType));
							}
							XElement xElement3 = new XElement("Items", new XAttribute("SubActivityType", value3.SubActivityType), new XAttribute("Condition", value3.Condition));
							foreach (EventRewardGoodsInfo awardList in value3.AwardLists)
							{
								XElement content = new XElement("Item", new XAttribute("TemplateId", awardList.TemplateId), new XAttribute("StrengthLevel", awardList.StrengthLevel), new XAttribute("AttackCompose", awardList.AttackCompose), new XAttribute("DefendCompose", awardList.DefendCompose), new XAttribute("LuckCompose", awardList.LuckCompose), new XAttribute("AgilityCompose", awardList.AgilityCompose), new XAttribute("IsBind", awardList.IsBind), new XAttribute("ValidDate", awardList.ValidDate), new XAttribute("Count", awardList.Count));
								xElement3.Add(content);
							}
							xElement2.Add(xElement3);
						}
						xElement.Add(xElement2);
						xElement2 = null;
					}
					flag = true;
					value = "Success!";
				}
			}
			catch
			{
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			return csFunction.CreateCompressXml(context, xElement, "eventrewarditemlist", isCompress: true);
		}
	}
}
