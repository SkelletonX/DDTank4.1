using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
    public class ItemBoxMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static ThreadSafeRandom random = new ThreadSafeRandom();
        private static ItemBoxInfo[] m_itemBox;
        private static Dictionary<int, List<ItemBoxInfo>> m_itemBoxs;

        public static bool ReLoad()
        {
            try
            {
                ItemBoxInfo[] itemBoxs = ItemBoxMgr.LoadItemBoxDb();
                Dictionary<int, List<ItemBoxInfo>> dictionary = ItemBoxMgr.LoadItemBoxs(itemBoxs);
                if (itemBoxs != null)
                {
                    Interlocked.Exchange<ItemBoxInfo[]>(ref ItemBoxMgr.m_itemBox, itemBoxs);
                    Interlocked.Exchange<Dictionary<int, List<ItemBoxInfo>>>(ref ItemBoxMgr.m_itemBoxs, dictionary);
                }
            }
            catch (Exception ex)
            {
                if (ItemBoxMgr.log.IsErrorEnabled)
                    ItemBoxMgr.log.Error((object)nameof(ReLoad), ex);
                return false;
            }
            return true;
        }

        public static bool Init()
        {
            return ItemBoxMgr.ReLoad();
        }

        public static ItemBoxInfo[] LoadItemBoxDb()
        {
            using (ProduceBussiness produceBussiness = new ProduceBussiness())
                return produceBussiness.GetItemBoxInfos();
        }

        public static Dictionary<int, List<ItemBoxInfo>> LoadItemBoxs(
          ItemBoxInfo[] itemBoxs)
        {
            Dictionary<int, List<ItemBoxInfo>> dictionary = new Dictionary<int, List<ItemBoxInfo>>();
            for (int index = 0; index < itemBoxs.Length; ++index)
            {
                ItemBoxInfo info = itemBoxs[index];
                if (!dictionary.Keys.Contains<int>(info.ID))
                {
                    IEnumerable<ItemBoxInfo> source = ((IEnumerable<ItemBoxInfo>)itemBoxs).Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => s.ID == info.ID));
                    dictionary.Add(info.ID, source.ToList<ItemBoxInfo>());
                }
            }
            return dictionary;
        }

        public static List<ItemBoxInfo> FindItemBox(int DataId)
        {
            if (ItemBoxMgr.m_itemBoxs.ContainsKey(DataId))
                return ItemBoxMgr.m_itemBoxs[DataId];
            return (List<ItemBoxInfo>)null;
        }

        public static List<SqlDataProvider.Data.ItemInfo> GetAllItemBoxAward(int DataId)
        {
            List<ItemBoxInfo> itemBox = ItemBoxMgr.FindItemBox(DataId);
            List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
            foreach (ItemBoxInfo itemBoxInfo in itemBox)
            {
                SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemBoxInfo.TemplateId), itemBoxInfo.ItemCount, 105);
                fromTemplate.IsBinds = itemBoxInfo.IsBind;
                fromTemplate.ValidDate = itemBoxInfo.ItemValid;
                itemInfoList.Add(fromTemplate);
            }
            return itemInfoList;
        }

        public static ItemBoxInfo FindSpecialItemBox(int DataId)
        {
            ItemBoxInfo itemBoxInfo = new ItemBoxInfo();
            switch (DataId)
            {
                case -1100:
                    itemBoxInfo.TemplateId = 11213;
                    itemBoxInfo.ItemCount = 1;
                    break;
                case -300:
                    itemBoxInfo.TemplateId = 11420;
                    itemBoxInfo.ItemCount = 1;
                    break;
                case -200:
                    itemBoxInfo.TemplateId = 112244;
                    itemBoxInfo.ItemCount = 1;
                    break;
                case -100:
                    itemBoxInfo.TemplateId = 11233;
                    itemBoxInfo.ItemCount = 1;
                    break;
                case -800:
                    itemBoxInfo.TemplateId = 11917;
                    itemBoxInfo.ItemCount = 1;
                    break;
                case 11408:
                    itemBoxInfo.TemplateId = 11420;
                    itemBoxInfo.ItemCount = 1;
                    break;
            }
            return itemBoxInfo;
        }

        public static bool CreateItemBox(
          int DateId,
          List<SqlDataProvider.Data.ItemInfo> itemInfos,
          SpecialItemDataInfo specialInfo)
        {
            return ItemBoxMgr.CreateItemBox(DateId, (List<ItemBoxInfo>)null, itemInfos, specialInfo);
        }

        public static bool CreateItemBox(
          int DateId,
          List<ItemBoxInfo> tempBox,
          List<SqlDataProvider.Data.ItemInfo> itemInfos,
          SpecialItemDataInfo specialInfo)
        {
            List<ItemBoxInfo> itemBoxInfoList1 = new List<ItemBoxInfo>();
            List<ItemBoxInfo> source = ItemBoxMgr.FindItemBox(DateId);
            if (tempBox != null && tempBox.Count > 0)
                source = tempBox;
            if (source == null)
                return false;
            List<ItemBoxInfo> itemBoxInfoList2 = source.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => s.IsSelect)).ToList<ItemBoxInfo>();
            int num1 = 1;
            int maxRound = 0;
            if (itemBoxInfoList2.Count < source.Count)
            {
                maxRound = ThreadSafeRandom.NextStatic(source.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => !s.IsSelect)).Select<ItemBoxInfo, int>((Func<ItemBoxInfo, int>)(s => s.Random)).Max());
                if (maxRound <= 0)
                {
                    ItemBoxMgr.log.Error((object)("ItemBoxMgr Random Error: " + (object)maxRound + " | " + (object)DateId));
                    maxRound = source.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => !s.IsSelect)).Select<ItemBoxInfo, int>((Func<ItemBoxInfo, int>)(s => s.Random)).Max();
                }
            }
            List<ItemBoxInfo> list = source.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s =>
           {
               if (!s.IsSelect)
                   return s.Random >= maxRound;
               return false;
           })).ToList<ItemBoxInfo>();
            int num2 = list.Count<ItemBoxInfo>();
            if (num2 > 0)
            {
                int count = num1 > num2 ? num2 : num1;
                foreach (int randomUnrepeat in ItemBoxMgr.GetRandomUnrepeatArray(0, num2 - 1, count))
                {
                    ItemBoxInfo itemBoxInfo = list[randomUnrepeat];
                    if (itemBoxInfoList2 == null)
                        itemBoxInfoList2 = new List<ItemBoxInfo>();
                    itemBoxInfoList2.Add(itemBoxInfo);
                }
            }
            foreach (ItemBoxInfo itemBoxInfo in itemBoxInfoList2)
            {
                if (itemBoxInfo == null)
                    return false;
                switch (itemBoxInfo.TemplateId)
                {
                    case -300:
                        specialInfo.GiftToken += itemBoxInfo.ItemCount;
                        continue;
                    case -200:
                        specialInfo.Money += itemBoxInfo.ItemCount;
                        continue;
                    case -100:
                        specialInfo.Gold += itemBoxInfo.ItemCount;
                        continue;
                    case -800:
                        specialInfo.myHonor += itemBoxInfo.ItemCount;
                        continue;
                    case 11107:
                        specialInfo.GP += itemBoxInfo.ItemCount;
                        continue;
                    default:
                        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemBoxInfo.TemplateId), itemBoxInfo.ItemCount, 101);
                        if (fromTemplate != null)
                        {
                            fromTemplate.IsBinds = itemBoxInfo.IsBind;
                            fromTemplate.ValidDate = itemBoxInfo.ItemValid;
                            fromTemplate.StrengthenLevel = itemBoxInfo.StrengthenLevel;
                            fromTemplate.AttackCompose = itemBoxInfo.AttackCompose;
                            fromTemplate.DefendCompose = itemBoxInfo.DefendCompose;
                            fromTemplate.AgilityCompose = itemBoxInfo.AgilityCompose;
                            fromTemplate.LuckCompose = itemBoxInfo.LuckCompose;
                            if (itemInfos == null)
                                itemInfos = new List<SqlDataProvider.Data.ItemInfo>();
                            itemInfos.Add(fromTemplate);
                            continue;
                        }
                        continue;
                }
            }
            return true;
        }

        public static bool CreateItemBox(
          int DateId,
          List<SqlDataProvider.Data.ItemInfo> itemInfos,
          ref int gold,
          ref int point,
          ref int giftToken,
          ref int medal,
          ref int exp,
          ref int myHonor)
        {
            List<ItemBoxInfo> itemBoxInfoList1 = new List<ItemBoxInfo>();
            List<ItemBoxInfo> itemBox = ItemBoxMgr.FindItemBox(DateId);
            if (itemBox == null)
                return false;
            List<ItemBoxInfo> itemBoxInfoList2 = itemBox.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => s.IsSelect)).ToList<ItemBoxInfo>();
            int num1 = 1;
            int maxRound = 0;
            if (itemBoxInfoList2.Count < itemBox.Count)
                maxRound = ThreadSafeRandom.NextStatic(itemBox.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => !s.IsSelect)).Select<ItemBoxInfo, int>((Func<ItemBoxInfo, int>)(s => s.Random)).Max());
            List<ItemBoxInfo> list = itemBox.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s =>
           {
               if (!s.IsSelect)
                   return s.Random >= maxRound;
               return false;
           })).ToList<ItemBoxInfo>();
            int num2 = list.Count<ItemBoxInfo>();
            if (num2 > 0)
            {
                int count = num1 > num2 ? num2 : num1;
                foreach (int randomUnrepeat in ItemBoxMgr.GetRandomUnrepeatArray(0, num2 - 1, count))
                {
                    ItemBoxInfo itemBoxInfo = list[randomUnrepeat];
                    if (itemBoxInfoList2 == null)
                        itemBoxInfoList2 = new List<ItemBoxInfo>();
                    itemBoxInfoList2.Add(itemBoxInfo);
                }
            }
            foreach (ItemBoxInfo itemBoxInfo in itemBoxInfoList2)
            {
                if (itemBoxInfo == null)
                    return false;
                switch (itemBoxInfo.TemplateId)
                {
                    case -1100:
                        giftToken += itemBoxInfo.ItemCount;
                        continue;
                    case -300:
                        medal += itemBoxInfo.ItemCount;
                        continue;
                    case -200:
                        point += itemBoxInfo.ItemCount;
                        continue;
                    case -100:
                        gold += itemBoxInfo.ItemCount;
                        continue;
                    case -800:
                        myHonor += itemBoxInfo.ItemCount;
                        continue;
                    case 11107:
                        exp += itemBoxInfo.ItemCount;
                        continue;
                    default:
                        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemBoxInfo.TemplateId), itemBoxInfo.ItemCount, 101);
                        if (fromTemplate != null)
                        {
                            fromTemplate.Count = itemBoxInfo.ItemCount;
                            fromTemplate.IsBinds = itemBoxInfo.IsBind;
                            fromTemplate.ValidDate = itemBoxInfo.ItemValid;
                            fromTemplate.StrengthenLevel = itemBoxInfo.StrengthenLevel;
                            fromTemplate.AttackCompose = itemBoxInfo.AttackCompose;
                            fromTemplate.DefendCompose = itemBoxInfo.DefendCompose;
                            fromTemplate.AgilityCompose = itemBoxInfo.AgilityCompose;
                            fromTemplate.LuckCompose = itemBoxInfo.LuckCompose;
                            fromTemplate.IsTips = (uint)itemBoxInfo.IsTips > 0U;
                            fromTemplate.IsLogs = itemBoxInfo.IsLogs;
                            if (itemInfos == null)
                                itemInfos = new List<SqlDataProvider.Data.ItemInfo>();
                            itemInfos.Add(fromTemplate);
                            continue;
                        }
                        continue;
                }
            }
            return true;
        }

        public static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
        {
            int[] numArray = new int[count];
            for (int index1 = 0; index1 < count; ++index1)
            {
                int num1 = ItemBoxMgr.random.Next(minValue, maxValue + 1);
                int num2 = 0;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    if (numArray[index2] == num1)
                        ++num2;
                }
                if (num2 == 0)
                    numArray[index1] = num1;
                else
                    --index1;
            }
            return numArray;
        }

        public static bool CreateItemBox(
          int DateId,
          List<SqlDataProvider.Data.ItemInfo> itemInfos,
          ref int gold,
          ref int point,
          ref int giftToken,
          ref int exp)
        {
            return ItemBoxMgr.CreateItemBox(DateId, (List<ItemBoxInfo>)null, itemInfos, ref gold, ref point, ref giftToken, ref exp);
        }

        public static bool CreateItemBox(
          int DateId,
          List<ItemBoxInfo> tempBox,
          List<SqlDataProvider.Data.ItemInfo> itemInfos,
          ref int gold,
          ref int point,
          ref int giftToken,
          ref int exp)
        {
            List<ItemBoxInfo> itemBoxInfoList1 = new List<ItemBoxInfo>();
            List<ItemBoxInfo> source = ItemBoxMgr.FindItemBox(DateId);
            if (tempBox != null && tempBox.Count > 0)
                source = tempBox;
            if (source == null)
                return false;
            List<ItemBoxInfo> itemBoxInfoList2 = source.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => s.IsSelect)).ToList<ItemBoxInfo>();
            int num1 = 1;
            int maxRound = 0;
            if (itemBoxInfoList2.Count < source.Count)
                maxRound = ThreadSafeRandom.NextStatic(source.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s => !s.IsSelect)).Select<ItemBoxInfo, int>((Func<ItemBoxInfo, int>)(s => s.Random)).Max());
            List<ItemBoxInfo> list = source.Where<ItemBoxInfo>((Func<ItemBoxInfo, bool>)(s =>
           {
               if (!s.IsSelect)
                   return s.Random >= maxRound;
               return false;
           })).ToList<ItemBoxInfo>();
            int num2 = list.Count<ItemBoxInfo>();
            if (num2 > 0)
            {
                int count = num1 > num2 ? num2 : num1;
                foreach (int randomUnrepeat in ItemBoxMgr.GetRandomUnrepeatArray(0, num2 - 1, count))
                {
                    ItemBoxInfo itemBoxInfo = list[randomUnrepeat];
                    if (itemBoxInfoList2 == null)
                        itemBoxInfoList2 = new List<ItemBoxInfo>();
                    itemBoxInfoList2.Add(itemBoxInfo);
                }
            }
            foreach (ItemBoxInfo itemBoxInfo in itemBoxInfoList2)
            {
                if (itemBoxInfo == null)
                    return false;
                switch (itemBoxInfo.TemplateId)
                {
                    case -300:
                        giftToken += itemBoxInfo.ItemCount;
                        continue;
                    case -200:
                        point += itemBoxInfo.ItemCount;
                        continue;
                    case -100:
                        gold += itemBoxInfo.ItemCount;
                        continue;
                    case 11107:
                        exp += itemBoxInfo.ItemCount;
                        continue;
                    default:
                        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemBoxInfo.TemplateId), itemBoxInfo.ItemCount, 101);
                        if (fromTemplate != null)
                        {
                            fromTemplate.IsBinds = itemBoxInfo.IsBind;
                            fromTemplate.ValidDate = itemBoxInfo.ItemValid;
                            fromTemplate.StrengthenLevel = itemBoxInfo.StrengthenLevel;
                            fromTemplate.AttackCompose = itemBoxInfo.AttackCompose;
                            fromTemplate.DefendCompose = itemBoxInfo.DefendCompose;
                            fromTemplate.AgilityCompose = itemBoxInfo.AgilityCompose;
                            fromTemplate.LuckCompose = itemBoxInfo.LuckCompose;
                            if (itemInfos == null)
                                itemInfos = new List<SqlDataProvider.Data.ItemInfo>();
                            itemInfos.Add(fromTemplate);
                            continue;
                        }
                        continue;
                }
            }
            return true;
        }

        public static List<ItemBoxInfo> FindLotteryItemBoxByRand(
          int DateId,
          int countSelect)
        {
            List<ItemBoxInfo> lotteryItemBox = ItemBoxMgr.FindLotteryItemBox(DateId);
            List<ItemBoxInfo> itemBoxInfoList = new List<ItemBoxInfo>();
            for (int index1 = 0; index1 < countSelect; ++index1)
            {
                int index2 = ThreadSafeRandom.NextStatic(0, lotteryItemBox.Count);
                if (index2 < lotteryItemBox.Count)
                {
                    itemBoxInfoList.Add(lotteryItemBox[index2]);
                    lotteryItemBox.Remove(lotteryItemBox[index2]);
                }
            }
            return itemBoxInfoList;
        }

        public static List<ItemBoxInfo> FindLotteryItemBox(int DataId)
        {
            if (!ItemBoxMgr.m_itemBoxs.ContainsKey(DataId))
                return (List<ItemBoxInfo>)null;
            List<ItemBoxInfo> itemBoxInfoList = new List<ItemBoxInfo>();
            using (List<ItemBoxInfo>.Enumerator enumerator = ItemBoxMgr.m_itemBoxs[DataId].GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ItemBoxInfo current = enumerator.Current;
                    bool flag = true;
                    foreach (ItemBoxInfo itemBoxInfo in itemBoxInfoList)
                    {
                        if (itemBoxInfo.TemplateId == current.TemplateId && itemBoxInfo.ItemCount == current.ItemCount)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                        itemBoxInfoList.Add(current);
                }
                return itemBoxInfoList;
            }
        }
    }
}
