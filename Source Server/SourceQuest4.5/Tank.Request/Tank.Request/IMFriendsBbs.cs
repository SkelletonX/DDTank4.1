using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class IMFriendsBbs : IHttpHandler
	{
		public interface IAgentFriends
		{
			string FriendsString(string uid);
		}

		public class Normal : IAgentFriends
		{
			private string Url;

			public static string FriendInterface => ConfigurationManager.AppSettings["FriendInterface"];

			public string FriendsString(string uid)
			{
				try
				{
					if (FriendInterface == "")
					{
						return string.Empty;
					}
					string err = "";
					Url = string.Format(CultureInfo.InvariantCulture, FriendInterface, new object[1]
					{
						uid
					});
					string page = WebsResponse.GetPage(Url, "", "utf-8", out err);
					if (!(err == ""))
					{
						throw new Exception(err);
					}
					return page;
				}
				catch (Exception exception)
				{
					if (log.IsErrorEnabled)
					{
						log.Error("Normal：", exception);
					}
				}
				return string.Empty;
			}
		}

		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public bool IsReusable => false;

		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string value = "Fail!";
			XElement xElement = new XElement("Result");
			Normal normal = new Normal();
			StringBuilder stringBuilder = new StringBuilder();
			string uid = HttpContext.Current.Request.Params["Uid"];
			string text = normal.FriendsString(uid);
			DataSet dataSet = new DataSet();
			if (text != "")
			{
				try
				{
					dataSet.ReadXml(new StringReader(text));
					for (int i = 0; i < dataSet.Tables["item"].DefaultView.Count; i++)
					{
						stringBuilder.Append(dataSet.Tables["item"].DefaultView[i]["UserName"].ToString() + ",");
					}
				}
				catch (Exception exception)
				{
					if (log.IsErrorEnabled)
					{
						log.Error("Get Table Item ", exception);
					}
				}
			}
			if (stringBuilder.Length <= 1 || text == "")
			{
				xElement.Add(new XAttribute("value", flag));
				xElement.Add(new XAttribute("message", value));
				context.Response.ContentType = "text/plain";
				context.Response.Write(xElement.ToString(check: false));
				return;
			}
			string[] array = stringBuilder.ToString().Split(',');
			ArrayList arrayList = new ArrayList();
			StringBuilder stringBuilder2 = new StringBuilder(4000);
			for (int j = 0; j < array.Count() && !(array[j] == ""); j++)
			{
				if (stringBuilder2.Length + array[j].Length < 4000)
				{
					stringBuilder2.Append(array[j] + ",");
					continue;
				}
				arrayList.Add(stringBuilder2.ToString());
				stringBuilder2.Remove(0, stringBuilder2.Length);
			}
			arrayList.Add(stringBuilder2.ToString());
			try
			{
				for (int k = 0; k < arrayList.Count; k++)
				{
					string condictArray = arrayList[k].ToString();
					using (PlayerBussiness playerBussiness = new PlayerBussiness())
					{
						FriendInfo[] friendsBbs = playerBussiness.GetFriendsBbs(condictArray);
						for (int l = 0; l < friendsBbs.Count(); l++)
						{
							DataRow[] array2 = dataSet.Tables["item"].Select("UserName='" + friendsBbs[l].UserName + "'");
							XElement content = new XElement("Item", new XAttribute("NickName", friendsBbs[l].NickName), new XAttribute("UserName", friendsBbs[l].UserName), new XAttribute("UserId", friendsBbs[l].UserID), new XAttribute("Photo", (array2[0]["Photo"] == null) ? "" : array2[0]["Photo"].ToString()), new XAttribute("PersonWeb", (array2[0]["PersonWeb"] == null) ? "" : array2[0]["PersonWeb"].ToString()), new XAttribute("IsExist", friendsBbs[l].IsExist), new XAttribute("OtherName", (array2[0]["OtherName"] == null) ? "" : array2[0]["OtherName"].ToString()));
							xElement.Add(content);
						}
					}
				}
				flag = true;
				value = "Success!";
			}
			catch (Exception exception2)
			{
				log.Error("IMFriendsGood", exception2);
			}
			xElement.Add(new XAttribute("value", flag));
			xElement.Add(new XAttribute("message", value));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xElement.ToString(check: false));
		}
	}
}
