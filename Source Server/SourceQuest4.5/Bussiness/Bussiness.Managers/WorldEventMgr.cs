using log4net;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Bussiness.Managers
{
	public class WorldEventMgr
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static ReaderWriterLock m_lock;

		private static ThreadSafeRandom random = new ThreadSafeRandom();

		public static bool SendItemsToMail(List<ItemInfo> infos, int PlayerId, string Nickname, string title, string content = null)
		{
			bool flag = false;
			using (PlayerBussiness bussiness = new PlayerBussiness())
			{
				List<ItemInfo> list = new List<ItemInfo>();
				foreach (ItemInfo info in infos)
				{
					if (info.Template.MaxCount == 1)
					{
						for (int j = 0; j < info.Count; j++)
						{
							ItemInfo item = ItemInfo.CloneFromTemplate(info.Template, info);
							item.Count = 1;
							list.Add(item);
						}
					}
					else
					{
						list.Add(info);
					}
				}
				for (int i = 0; i < list.Count; i += 5)
				{
					MailInfo mail = new MailInfo
					{
						Title = title,
						Content = content,
						Gold = 0,
						IsExist = true,
						Money = 0,
						Receiver = Nickname,
						ReceiverID = PlayerId,
						Sender = "Administrador do Sistema",
						SenderID = 0,
						Type = 9,
						GiftToken = 0
					};
					StringBuilder builder = new StringBuilder();
					StringBuilder builder2 = new StringBuilder();
					builder.Append(LanguageMgr.GetTranslation("Game.Server.GameUtils.CommonBag.AnnexRemark"));
					int num7 = i;
					if (list.Count > num7)
					{
						ItemInfo info6 = list[num7];
						if (info6.ItemID == 0)
						{
							bussiness.AddGoods(info6);
						}
						mail.Annex1 = info6.ItemID.ToString();
						mail.Annex1Name = info6.Template.Name;
						builder.Append("1、" + mail.Annex1Name + "x" + info6.Count + ";");
						builder2.Append("1、" + mail.Annex1Name + "x" + info6.Count + ";");
					}
					num7 = i + 1;
					if (list.Count > num7)
					{
						ItemInfo info6 = list[num7];
						if (info6.ItemID == 0)
						{
							bussiness.AddGoods(info6);
						}
						mail.Annex2 = info6.ItemID.ToString();
						mail.Annex2Name = info6.Template.Name;
						builder.Append("2、" + mail.Annex2Name + "x" + info6.Count + ";");
						builder2.Append("2、" + mail.Annex2Name + "x" + info6.Count + ";");
					}
					num7 = i + 2;
					if (list.Count > num7)
					{
						ItemInfo info6 = list[num7];
						if (info6.ItemID == 0)
						{
							bussiness.AddGoods(info6);
						}
						mail.Annex3 = info6.ItemID.ToString();
						mail.Annex3Name = info6.Template.Name;
						builder.Append("3、" + mail.Annex3Name + "x" + info6.Count + ";");
						builder2.Append("3、" + mail.Annex3Name + "x" + info6.Count + ";");
					}
					num7 = i + 3;
					if (list.Count > num7)
					{
						ItemInfo info6 = list[num7];
						if (info6.ItemID == 0)
						{
							bussiness.AddGoods(info6);
						}
						mail.Annex4 = info6.ItemID.ToString();
						mail.Annex4Name = info6.Template.Name;
						builder.Append("4、" + mail.Annex4Name + "x" + info6.Count + ";");
						builder2.Append("4、" + mail.Annex4Name + "x" + info6.Count + ";");
					}
					num7 = i + 4;
					if (list.Count > num7)
					{
						ItemInfo info6 = list[num7];
						if (info6.ItemID == 0)
						{
							bussiness.AddGoods(info6);
						}
						mail.Annex5 = info6.ItemID.ToString();
						mail.Annex5Name = info6.Template.Name;
						builder.Append("5、" + mail.Annex5Name + "x" + info6.Count + ";");
						builder2.Append("5、" + mail.Annex5Name + "x" + info6.Count + ";");
					}
					mail.AnnexRemark = builder.ToString();
					mail.Content = (mail.Content ?? builder2.ToString());
					flag = bussiness.SendMail(mail);
				}
				return flag;
			}
		}

		public static bool SendItemToMail(ItemInfo info, int PlayerId, string Nickname, int zoneId, AreaConfigInfo areaConfig, string title)
		{
			return SendItemsToMail(new List<ItemInfo>
			{
				info
			}, PlayerId, Nickname, title);
		}

		public static bool SendItemsToMails(List<ItemInfo> infos, int PlayerId, string Nickname, int zoneId, AreaConfigInfo areaConfig, string title)
		{
			return SendItemsToMail(infos, PlayerId, Nickname, title);
		}

		public static bool SendItemsToMails(List<ItemInfo> infos, int PlayerId, string Nickname, int zoneId, AreaConfigInfo areaConfig, string title, string content)
		{
			return SendItemsToMail(infos, PlayerId, Nickname, title);
		}

		public static bool SendItemToMail(ItemInfo info, int PlayerId, string Nickname, int zoneId, AreaConfigInfo areaConfig, string title, string sender)
		{
			return SendItemsToMail(new List<ItemInfo>
			{
				info
			}, PlayerId, Nickname, title);
		}

		public static bool SendItemsToMail(List<ItemInfo> infos, int PlayerId, string Nickname, int zoneId, AreaConfigInfo areaConfig, string title, int type, string sender)
		{
			return SendItemsToMail(infos, PlayerId, Nickname, title);
		}

		public static bool SendItemsToMail(List<ItemInfo> infos, int PlayerId, string Nickname, int zoneId, AreaConfigInfo areaConfig, string title, string content)
		{
			return SendItemsToMail(infos, PlayerId, Nickname, title);
		}
	}
}
