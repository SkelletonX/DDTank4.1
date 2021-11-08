using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.GameUtils
{
    public abstract class AbstractInventory
    {
        private static readonly ILog ilog_0;

        public readonly static ILog log;

        protected object m_lock;

        private int int_0;

        private int int_1;

        private int int_2;

        private bool bool_0;

        protected ItemInfo[] m_items;

        protected List<int> m_changedPlaces;

        private int int_3;

        public int BeginSlot => int_2;

        public int Capalility
        {
            get
            {
                return int_1;
            }
            set
            {
                int_1 = ((value >= 0) ? ((value > m_items.Length) ? m_items.Length : value) : 0);
            }
        }

        public int BagType => int_0;

        public bool IsEmpty(int slot)
        {
            if (slot >= 0 && slot < int_1)
            {
                return m_items[slot] == null;
            }
            return true;
        }

        public AbstractInventory(int capability, int type, int beginSlot, bool autoStack)
        {

            m_lock = new object();
            m_changedPlaces = new List<int>();

            int_1 = capability;
            int_0 = type;
            int_2 = beginSlot;
            bool_0 = autoStack;
            m_items = new ItemInfo[capability];
        }

        public virtual bool AddItem(ItemInfo item)
        {
            return AddItem(item, int_2);
        }

        public virtual bool AddItem(ItemInfo item, int minSlot)
        {
            if (item == null)
            {
                return false;
            }
            int place = FindFirstEmptySlot(minSlot);
            return AddItemTo(item, place);
        }

        public virtual bool AddItem(ItemInfo item, int minSlot, int maxSlot)
        {
            if (item == null)
            {
                return false;
            }
            int place = FindFirstEmptySlot(minSlot, maxSlot);
            return AddItemTo(item, place);
        }

        public virtual bool AddItemTo(ItemInfo item, int place)
        {
            if (item != null && place < int_1 && place >= 0)
            {
                lock (m_lock)
                {
                    if (m_items[place] != null)
                    {
                        place = -1;
                    }
                    else
                    {
                        m_items[place] = item;
                        item.Place = place;
                        item.BagType = int_0;
                    }
                }
                if (place != -1)
                {
                    OnPlaceChanged(place);
                }
                return place != -1;
            }
            return false;
        }

        public virtual bool TakeOutItem(ItemInfo item)
        {
            if (item == null)
            {
                return false;
            }
            int num = -1;
            lock (m_lock)
            {
                for (int i = 0; i < int_1; i++)
                {
                    if (m_items[i] == item)
                    {
                        num = i;
                        m_items[i] = null;
                        break;
                    }
                }
            }
            if (num != -1)
            {
                OnPlaceChanged(num);
                if (item.BagType == BagType)
                {
                    item.Place = -1;
                    item.BagType = -1;
                }
            }
            return num != -1;
        }

        public bool TakeOutItemAt(int place)
        {
            return TakeOutItem(GetItemAt(place));
        }

        public void RemoveAllItem(List<ItemInfo> items)
        {
            BeginChanges();
            lock (m_lock)
            {
                foreach (ItemInfo item in items)
                {
                    if (item.Place >= m_items.Length)
                    {
                        ilog_0.Error((object)("ERROR PLACE OUT SIZE CAPALITITY: " + item.Place + " - tempid: " + item.TemplateID));
                    }
                    else if (m_items[item.Place] != null)
                    {
                        RemoveItem(m_items[item.Place]);
                    }
                }
            }
            CommitChanges();
        }

        public void RemoveAllItem(List<int> places)
        {
            BeginChanges();
            lock (m_lock)
            {
                for (int i = 0; i < places.Count; i++)
                {
                    int num = places[i];
                    if (m_items[num] != null)
                    {
                        RemoveItem(m_items[num]);
                    }
                }
            }
            CommitChanges();
        }

        public virtual bool RemoveItem(ItemInfo item)
        {
            if (item == null)
            {
                return false;
            }
            int num = -1;
            lock (m_lock)
            {
                for (int i = 0; i < int_1; i++)
                {
                    if (m_items[i] == item)
                    {
                        num = i;
                        m_items[i] = null;
                        break;
                    }
                }
            }
            if (num != -1)
            {
                OnPlaceChanged(num);
                if (item.BagType == BagType)
                {
                    item.Place = -1;
                    item.BagType = -1;
                }
            }
            return num != -1;
        }

        public bool RemoveItemAt(int place)
        {
            return RemoveItem(GetItemAt(place));
        }

        public virtual bool AddCountToStack(ItemInfo item, int count)
        {
            if (item == null)
            {
                return false;
            }
            if (count > 0 && item.BagType == int_0)
            {
                if (item.Count + count > item.Template.MaxCount)
                {
                    return false;
                }
                item.Count += count;
                OnPlaceChanged(item.Place);
                return true;
            }
            return false;
        }

        public virtual bool RemoveCountFromStack(ItemInfo item, int count)
        {
            if (item == null)
            {
                return false;
            }
            if (count > 0 && item.BagType == int_0)
            {
                if (item.Count < count)
                {
                    return false;
                }
                if (item.Count == count)
                {
                    return RemoveItem(item);
                }
                item.Count -= count;
                OnPlaceChanged(item.Place);
                return true;
            }
            return false;
        }

        public virtual bool AddTemplateAt(ItemInfo cloneItem, int count, int place)
        {
            return AddTemplate(cloneItem, count, place, int_1 - 1);
        }

        public virtual bool AddTemplate(ItemInfo cloneItem, int count)
        {
            return AddTemplate(cloneItem, count, int_2, int_1 - 1);
        }

        public virtual bool AddTemplate(ItemInfo cloneItem)
        {
            return AddTemplate(cloneItem, cloneItem.Count, int_2, int_1 - 1);
        }

        public virtual bool AddTemplate(ItemInfo cloneItem, int count, int minSlot, int maxSlot)
        {
            if (cloneItem == null)
            {
                return false;
            }
            ItemTemplateInfo template = cloneItem.Template;
            if (template == null)
            {
                return false;
            }
            if (count <= 0)
            {
                return false;
            }
            if (minSlot >= int_2 && minSlot <= int_1 - 1)
            {
                if (maxSlot >= int_2 && maxSlot <= int_1 - 1)
                {
                    if (minSlot > maxSlot)
                    {
                        return false;
                    }
                    lock (m_lock)
                    {
                        List<int> list = new List<int>();
                        int num = count;
                        for (int i = minSlot; i <= maxSlot; i++)
                        {
                            ItemInfo itemInfo = m_items[i];
                            if (itemInfo == null)
                            {
                                num -= template.MaxCount;
                                list.Add(i);
                            }
                            else if (bool_0 && cloneItem.CanStackedTo(itemInfo))
                            {
                                num -= template.MaxCount - itemInfo.Count;
                                list.Add(i);
                            }
                            if (num <= 0)
                            {
                                break;
                            }
                        }
                        if (num <= 0)
                        {
                            BeginChanges();
                            try
                            {
                                num = count;
                                foreach (int item in list)
                                {
                                    ItemInfo itemInfo2 = m_items[item];
                                    if (itemInfo2 == null)
                                    {
                                        itemInfo2 = cloneItem.Clone();
                                        itemInfo2.Count = ((num < template.MaxCount) ? num : template.MaxCount);
                                        num -= itemInfo2.Count;
                                        AddItemTo(itemInfo2, item);
                                    }
                                    else if (itemInfo2.TemplateID == template.TemplateID)
                                    {
                                        int num2 = (itemInfo2.Count + num < template.MaxCount) ? num : (template.MaxCount - itemInfo2.Count);
                                        itemInfo2.Count += num2;
                                        num -= num2;
                                        OnPlaceChanged(item);
                                    }
                                    else
                                    {
                                        ilog_0.Error((object)"Add template erro: select slot's TemplateId not equest templateId");
                                    }
                                }
                                if (num != 0)
                                {
                                    ilog_0.Error((object)"Add template error: last count not equal Zero.");
                                }
                            }
                            finally
                            {
                                CommitChanges();
                            }
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }
            return false;
        }

        public virtual bool RemoveTemplate(int templateId, int count)
        {
            return RemoveTemplate(templateId, count, 0, int_1 - 1);
        }

        public virtual bool RemoveTemplate(int templateId, int count, int minSlot, int maxSlot)
        {
            if (count <= 0)
            {
                return false;
            }
            if (minSlot >= 0 && minSlot <= int_1 - 1)
            {
                if (maxSlot > 0 && maxSlot <= int_1 - 1)
                {
                    if (minSlot > maxSlot)
                    {
                        return false;
                    }
                    lock (m_lock)
                    {
                        List<int> list = new List<int>();
                        int num = count;
                        for (int i = minSlot; i <= maxSlot; i++)
                        {
                            ItemInfo itemInfo = m_items[i];
                            if (itemInfo != null && itemInfo.TemplateID == templateId)
                            {
                                list.Add(i);
                                num -= itemInfo.Count;
                                if (num <= 0)
                                {
                                    break;
                                }
                            }
                        }
                        if (num <= 0)
                        {
                            BeginChanges();
                            num = count;
                            try
                            {
                                foreach (int item in list)
                                {
                                    ItemInfo itemInfo2 = m_items[item];
                                    if (itemInfo2 != null && itemInfo2.TemplateID == templateId)
                                    {
                                        if (itemInfo2.Count <= num)
                                        {
                                            RemoveItem(itemInfo2);
                                            num -= itemInfo2.Count;
                                        }
                                        else
                                        {
                                            int num2 = (itemInfo2.Count - num < itemInfo2.Count) ? num : 0;
                                            itemInfo2.Count -= num2;
                                            num -= num2;
                                            OnPlaceChanged(item);
                                        }
                                    }
                                }
                                if (num != 0)
                                {
                                    ilog_0.Error((object)"Remove templat error:last itemcoutj not equal Zero.");
                                }
                            }
                            finally
                            {
                                CommitChanges();
                            }
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }
            return false;
        }

         public virtual bool MoveItem(int fromSlot, int toSlot, int count)
        {
            if (fromSlot < 0 || toSlot < 0 || fromSlot >= int_1 || toSlot >= int_1) return false;

            bool result = false;
            lock (m_lock)
            {
                if (!CombineItems(fromSlot, toSlot) && !StackItems(fromSlot, toSlot, count))
                {
                    result = ExchangeItems(fromSlot, toSlot);
                }
                else
                {
                    result = true;
                }
            }

            if (result)
            {
                BeginChanges();
                try
                {
                    OnPlaceChanged(fromSlot);
                    OnPlaceChanged(toSlot);
                }
                finally
                {
                    CommitChanges();
                }
            }

            return result;
        }

        public bool IsSolt(int slot)
        {
            if (slot >= 0)
            {
                return slot < int_1;
            }
            return false;
        }

        public void ClearBag()
        {
            BeginChanges();
            lock (m_lock)
            {
                for (int i = int_2; i < int_1; i++)
                {
                    if (m_items[i] != null)
                    {
                        RemoveItem(m_items[i]);
                    }
                }
            }
            CommitChanges();
        }

        public void ClearBagWithoutPlace(int place)
        {
            BeginChanges();
            lock (m_lock)
            {
                for (int i = int_2; i < int_1; i++)
                {
                    if (m_items[i] != null && m_items[i].Place != place)
                    {
                        RemoveItem(m_items[i]);
                    }
                }
            }
            CommitChanges();
        }

        public bool StackItemToAnother(ItemInfo item)
        {
            lock (m_lock)
            {
                for (int num = int_1 - 1; num >= 0; num--)
                {
                    if (item != null && m_items[num] != null && m_items[num] != item && item.CanStackedTo(m_items[num]) && m_items[num].Count + item.Count <= item.Template.MaxCount)
                    {
                        m_items[num].Count += item.Count;
                        item.IsExist = false;
                        item.RemoveType = 26;
                        UpdateItem(m_items[num]);
                        return true;
                    }
                }
            }
            return false;
        }

        protected virtual bool CombineItems(int fromSlot, int toSlot)
        {
            return false;
        }

        protected virtual bool StackItems(int fromSlot, int toSlot, int itemCount)
        {
            ItemInfo fromItem = m_items[fromSlot] as ItemInfo;
            ItemInfo toItem = m_items[toSlot] as ItemInfo;

            if (itemCount == 0)
            {
                if (fromItem.Count > 0)
                    itemCount = fromItem.Count;
                else
                    itemCount = 1;
            }

            if (toItem != null && toItem.TemplateID == fromItem.TemplateID && toItem.CanStackedTo(fromItem))
            {
                if (fromItem.Count + toItem.Count > fromItem.Template.MaxCount)
                {
                    fromItem.Count -= (toItem.Template.MaxCount - toItem.Count);
                    toItem.Count = toItem.Template.MaxCount;
                }
                else
                {
                    toItem.Count += itemCount;
                    RemoveItem(fromItem);
                }

                return true;
            }
            else if (toItem == null && fromItem.Count > itemCount)
            {
                ItemInfo newItem = (ItemInfo)fromItem.Clone();
                newItem.Count = itemCount;
                if (AddItemTo(newItem, toSlot))
                {
                    fromItem.Count -= itemCount;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        protected virtual bool ExchangeItems(int fromSlot, int toSlot)
        {
            ItemInfo fromItem = m_items[toSlot];
            ItemInfo toItem = m_items[fromSlot];

            m_items[fromSlot] = fromItem;
            m_items[toSlot] = toItem;

            if (fromItem != null)
                fromItem.Place = fromSlot;

            if (toItem != null)
                toItem.Place = toSlot;

            return true;
        }

        public virtual ItemInfo GetItemAt(int slot)
        {
            if (slot >= 0 && slot < int_1)
            {
                return m_items[slot];
            }
            return null;
        }

        public int FindFirstEmptySlot()
        {
            return FindFirstEmptySlot(int_2);
        }

        public virtual int FindFirstEmptySlot(int minSlot)
        {
            if (minSlot >= int_1)
            {
                return -1;
            }
            lock (m_lock)
            {
                int num = minSlot;
                while (true)
                {
                    if (num >= int_1)
                    {
                        return -1;
                    }
                    if (m_items[num] == null)
                    {
                        break;
                    }
                    num++;
                }
                return num;
            }
        }

        public int CountTotalEmptySlot()
        {
            return CountTotalEmptySlot(int_2);
        }

        public int CountTotalEmptySlot(int minSlot)
        {
            if (minSlot >= int_1)
            {
                return -1;
            }
            lock (m_lock)
            {
                int num = 0;
                for (int i = minSlot; i < int_1; i++)
                {
                    if (m_items[i] == null)
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        public int CountTotalFullItemSlots()
        {
            int num = 0;
            for(int x=0;x< m_items.Length;x++)
            {
                if (m_items[x] != null)
                    num++;
            }
            return num;
        }

        public int FindFirstEmptySlot(int minSlot, int maxSlot)
        {
            if (minSlot >= maxSlot) return -1;

            lock (m_lock)
            {
                for (int i = minSlot; i < maxSlot; i++)
                {
                    if (m_items[i] == null)
                    {
                        return i;
                    }
                }

                return -1;
            }
        }

        public int FindLastEmptySlot()
        {
            lock (m_lock)
            {
                int num = int_1 - 1;
                while (true)
                {
                    if (num < 0)
                    {
                        return -1;
                    }
                    if (m_items[num] == null)
                    {
                        break;
                    }
                    num--;
                }
                return num;
            }
        }

        public int FindLastEmptySlot(int maxSlot)
        {
            lock (m_lock)
            {
                int num = maxSlot - 1;
                while (true)
                {
                    if (num < 0)
                    {
                        return -1;
                    }
                    if (m_items[num] == null)
                    {
                        break;
                    }
                    num--;
                }
                return num;
            }
        }

        public virtual void Clear()
        {
            BeginChanges();
            lock (m_lock)
            {
                for (int i = 0; i < int_1; i++)
                {
                    m_items[i] = null;
                    OnPlaceChanged(i);
                }
            }
            CommitChanges();
        }

        public virtual ItemInfo GetItemByCategoryID(int minSlot, int categoryID, int property)
        {
            lock (m_lock)
            {
                int num = minSlot;
                while (true)
                {
                    if (num >= int_1)
                    {
                        return null;
                    }
                    if (m_items[num] != null && m_items[num].Template.CategoryID == categoryID && (property == -1 || m_items[num].Template.Property1 == property))
                    {
                        break;
                    }
                    num++;
                }
                return m_items[num];
            }
        }

        public virtual ItemInfo GetItemByTemplateID(int minSlot, int templateId)
        {
            lock (m_lock)
            {
                int num = minSlot;
                while (true)
                {
                    if (num >= int_1)
                    {
                        return null;
                    }
                    if (m_items[num] != null && m_items[num].TemplateID == templateId)
                    {
                        break;
                    }
                    num++;
                }
                return m_items[num];
            }
        }

        public virtual List<ItemInfo> GetItemsByTemplateID(int minSlot, int templateid)
        {
            lock (m_lock)
            {
                List<ItemInfo> list = new List<ItemInfo>();
                for (int i = minSlot; i < int_1; i++)
                {
                    if (m_items[i] != null && m_items[i].TemplateID == templateid)
                    {
                        list.Add(m_items[i]);
                    }
                }
                return list;
            }
        }

        public virtual ItemInfo GetItemByItemID(int minSlot, int itemId)
        {
            lock (m_lock)
            {
                int num = minSlot;
                while (true)
                {
                    if (num >= int_1)
                    {
                        return null;
                    }
                    if (m_items[num] != null && m_items[num].ItemID == itemId)
                    {
                        break;
                    }
                    num++;
                }
                return m_items[num];
            }
        }

        public virtual int GetItemCount(int templateId)
        {
            return GetItemCount(int_2, templateId);
        }

        public int GetItemCount(int minSlot, int templateId)
        {
            int num = 0;
            lock (m_lock)
            {
                for (int i = minSlot; i < int_1; i++)
                {
                    if (m_items[i] != null && m_items[i].TemplateID == templateId)
                    {
                        num += m_items[i].Count;
                    }
                }
                return num;
            }
        }

        public virtual List<ItemInfo> GetItems()
        {
            return GetItems(0, int_1);
        }

        public virtual List<ItemInfo> GetItems(int minSlot, int maxSlot)
        {
            List<ItemInfo> list = new List<ItemInfo>();
            lock (m_lock)
            {
                for (int i = minSlot; i < maxSlot; i++)
                {
                    if (m_items[i] != null)
                    {
                        list.Add(m_items[i]);
                    }
                }
                return list;
            }
        }

        public int GetEmptyCount()
        {
            return GetEmptyCount(int_2);
        }

        public virtual int GetEmptyCount(int minSlot)
        {
            if (minSlot >= 0 && minSlot <= int_1 - 1)
            {
                int num = 0;
                lock (m_lock)
                {
                    for (int i = minSlot; i < int_1; i++)
                    {
                        if (m_items[i] == null)
                        {
                            num++;
                        }
                    }
                    return num;
                }
            }
            return 0;
        }

        public virtual void UseItem(ItemInfo item)
        {
            bool flag = false;
            if (!item.IsBinds && (item.Template.BindType == 2 || item.Template.BindType == 3))
            {
                item.IsBinds = true;
                flag = true;
            }
            if (!item.IsUsed)
            {
                item.IsUsed = true;
                item.BeginDate = DateTime.Now;
                flag = true;
            }
            if (flag)
            {
                OnPlaceChanged(item.Place);
            }
        }

        public virtual void UpdateItem(ItemInfo item)
        {
            if (item != null && item.BagType == int_0)
            {
                if (item.Count <= 0)
                {
                    RemoveItem(item);
                }
                else
                {
                    OnPlaceChanged(item.Place);
                }
            }
        }

        public virtual bool RemoveCountFromStack(ItemInfo item, int count, eItemRemoveType type)
        {
            if (item == null)
            {
                return false;
            }
            if (count > 0 && item.BagType == int_0)
            {
                if (item.Count < count)
                {
                    return false;
                }
                if (item.Count == count)
                {
                    return RemoveItem(item);
                }
                item.Count -= count;
                OnPlaceChanged(item.Place);
                return true;
            }
            return false;
        }

        public virtual bool RemoveItem(ItemInfo item, eItemRemoveType type)
        {
            if (item == null)
            {
                return false;
            }
            int num = -1;
            lock (m_lock)
            {
                for (int i = 0; i < int_1; i++)
                {
                    if (m_items[i] == item)
                    {
                        num = i;
                        m_items[i] = null;
                        break;
                    }
                }
            }
            if (num != -1)
            {
                OnPlaceChanged(num);
                if (item.BagType == BagType && item.Place == num)
                {
                    item.Place = -1;
                    item.BagType = -1;
                }
            }
            return num != -1;
        }

        protected void OnPlaceChanged(int place)
        {
            if (!m_changedPlaces.Contains(place))
            {
                m_changedPlaces.Add(place);
            }
            if (int_3 <= 0 && m_changedPlaces.Count > 0)
            {
                UpdateChangedPlaces();
            }
        }

        public void BeginChanges()
        {
            Interlocked.Increment(ref int_3);
        }

        public void CommitChanges()
        {
            int num = Interlocked.Decrement(ref int_3);
            if (num < 0)
            {
                if (ilog_0.IsErrorEnabled)
                {
                    ilog_0.Error((object)("Inventory changes counter is bellow zero (forgot to use BeginChanges?)!\n\n" + Environment.StackTrace));
                }
                Thread.VolatileWrite(ref int_3, 0);
            }
            if (num <= 0 && m_changedPlaces.Count > 0)
            {
                UpdateChangedPlaces();
            }
        }

        public virtual void UpdateChangedPlaces()
        {
            m_changedPlaces.Clear();
        }

        public ItemInfo[] GetRawSpaces()
        {
            lock (m_lock)
            {
                return m_items.Clone() as ItemInfo[];
            }
        }

        static AbstractInventory()
        {

            ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }
    }
}
