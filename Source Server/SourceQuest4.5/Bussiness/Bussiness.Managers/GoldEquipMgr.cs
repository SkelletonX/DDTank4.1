using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bussiness.Managers
{
	public class GoldEquipMgr
	{
		private static readonly ILog log;

		private static Dictionary<int, GoldEquipTemplateInfo> dictionary_0;

		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, GoldEquipTemplateInfo> infos = new Dictionary<int, GoldEquipTemplateInfo>();
				if (LoadItem(infos))
				{
					try
					{
						dictionary_0 = infos;
						return true;
					}
					catch
					{
					}
				}
			}
			catch (Exception ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("ReLoad", ex);
				}
			}
			return false;
		}

		public static bool Init()
		{
			try
			{
				dictionary_0 = new Dictionary<int, GoldEquipTemplateInfo>();
				return LoadItem(dictionary_0);
			}
			catch (Exception ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Init", ex);
				}
				return false;
			}
		}

		public static bool LoadItem(Dictionary<int, GoldEquipTemplateInfo> infos)
		{
			using (ProduceBussiness produceBussiness = new ProduceBussiness())
			{
				GoldEquipTemplateInfo[] allGoldEquipTemplateLoad = produceBussiness.GetAllGoldEquipTemplateLoad();
				foreach (GoldEquipTemplateInfo equipTemplateInfo in allGoldEquipTemplateLoad)
				{
					if (!infos.Keys.Contains(equipTemplateInfo.ID))
					{
						infos.Add(equipTemplateInfo.ID, equipTemplateInfo);
					}
				}
			}
			return true;
		}

		public static GoldEquipTemplateInfo FindGoldEquipByTemplate(int templateId)
		{
			if (dictionary_0 == null)
			{
				Init();
			}
			try
			{
				foreach (GoldEquipTemplateInfo equipTemplateInfo in dictionary_0.Values)
				{
					if (equipTemplateInfo.OldTemplateId == templateId)
					{
						return equipTemplateInfo;
					}
				}
			}
			catch
			{
			}
			return null;
		}

		public static GoldEquipTemplateInfo FindGoldEquipOldTemplate(int TemplateId)
		{
			if (dictionary_0 == null)
			{
				Init();
			}
			try
			{
				foreach (GoldEquipTemplateInfo equipTemplateInfo in dictionary_0.Values)
				{
					string str = equipTemplateInfo.OldTemplateId.ToString();
					if (equipTemplateInfo.NewTemplateId == TemplateId && str.Substring(6) != "6")
					{
						return equipTemplateInfo;
					}
				}
			}
			catch
			{
			}
			return null;
		}

		public static GoldEquipTemplateInfo FindGoldEquipByTemplate(int templateId, int categoryId)
		{
			GoldEquipTemplateInfo equipTemplateInfo1 = null;
			if (dictionary_0 == null)
			{
				Init();
			}
			try
			{
				foreach (GoldEquipTemplateInfo equipTemplateInfo2 in dictionary_0.Values)
				{
					if (equipTemplateInfo2.OldTemplateId == templateId || (equipTemplateInfo2.OldTemplateId == -1 && equipTemplateInfo2.CategoryID == categoryId))
					{
						equipTemplateInfo1 = equipTemplateInfo2;
						return equipTemplateInfo1;
					}
				}
				return equipTemplateInfo1;
			}
			catch
			{
				return equipTemplateInfo1;
			}
		}

		static GoldEquipMgr()
		{
			log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}
	}
}
