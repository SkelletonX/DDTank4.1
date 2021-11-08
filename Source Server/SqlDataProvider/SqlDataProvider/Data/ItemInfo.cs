// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ItemInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;
using System.Collections.Generic;

namespace SqlDataProvider.Data
{
    public class ItemInfo : DataObject
    {
        private int _agilityCompose;
        private int _attackCompose;
        private int _bagType;
        private DateTime _beginDate;
        private string _color;
        private int _count;
        private int _defendCompose;
        private DateTime _goldBeginTime;
        private ItemTemplateInfo _goldEquip;
        private int _goldValidDate;
        private int _hole1;
        private int _hole2;
        private int _hole3;
        private int _hole4;
        private int _hole5;
        private int _hole5Exp;
        private int _hole5Level;
        private int _hole6;
        private int _hole6Exp;
        private int _hole6Level;
        private bool _isBinds;
        private bool _isExist;
        private bool _isGold;
        private bool _isJudage;
        private bool _isLogs;
        private bool _isTips;
        private bool _isUsed;
        private int _itemID;
        private int _luckCompose;
        private int _place;
        private DateTime _removeDate;
        private int _removeType;
        private string _skin;
        private int _strengthenLevel;
        private int _strengthenExp;
        private int _strengthenTimes;
        private ItemTemplateInfo _template;
        private int _templateId;
        private int _userID;
        private int _validDate;
        private DateTime _advanceDate;

        public ItemInfo(ItemTemplateInfo temp)
        {
            this._template = temp;
        }

        public bool CanEquip()
        {
            if (this._template.CategoryID < 10)
                return true;
            if (this._template.CategoryID >= 13)
                return this._template.CategoryID <= 16;
            return false;
        }

        public bool CanLatentEnergy()
        {
            switch (this.Template.CategoryID)
            {
                case 2:
                case 3:
                case 4:
                case 6:
                case 13:
                case 15:
                    return true;
                case 5:
                    return false;
                case 14:
                    return false;
                default:
                    return false;
            }
        }

        public bool CanStackedTo(ItemInfo to)
        {
            if (this._templateId == to.TemplateID && this.Template.MaxCount > 1 && (this._isBinds == to.IsBinds && this._isUsed == to._isUsed))
            {
                if (this.ValidDate == 0 || this.BeginDate.Date == to.BeginDate.Date && this.ValidDate == this.ValidDate)
                    return true;
            }
            else if (this._templateId == to.TemplateID && Equip.isDress(this.Template) && (Equip.isDress(to.Template) && to.StrengthenLevel <= 0))
                return true;
            return false;
        }

        public ItemInfo Clone()
        {
            ItemInfo itemInfo = new ItemInfo(this._template);
            itemInfo._userID = this._userID;
            itemInfo._validDate = this._validDate;
            itemInfo._templateId = this._templateId;
            itemInfo._goldEquip = this._goldEquip;
            itemInfo._strengthenLevel = this._strengthenLevel;
            itemInfo._luckCompose = this._luckCompose;
            itemInfo._itemID = 0;
            itemInfo._isJudage = this._isJudage;
            itemInfo._isExist = this._isExist;
            itemInfo._isBinds = this._isBinds;
            itemInfo._isUsed = this._isUsed;
            itemInfo._defendCompose = this._defendCompose;
            itemInfo._count = this._count;
            itemInfo._color = this._color;
            itemInfo.Skin = this._skin;
            itemInfo._beginDate = this._beginDate;
            itemInfo._attackCompose = this._attackCompose;
            itemInfo._agilityCompose = this._agilityCompose;
            itemInfo._bagType = this._bagType;
            itemInfo._isDirty = true;
            itemInfo._removeDate = this._removeDate;
            itemInfo._removeType = this._removeType;
            itemInfo._hole1 = this._hole1;
            itemInfo._hole2 = this._hole2;
            itemInfo._hole3 = this._hole3;
            itemInfo._hole4 = this._hole4;
            itemInfo._hole5 = this._hole5;
            itemInfo._hole6 = this._hole6;
            itemInfo._hole5Exp = this._hole5Exp;
            itemInfo._hole5Level = this._hole5Level;
            itemInfo._hole6Exp = this._hole6Exp;
            itemInfo._hole6Level = this._hole6Level;
            itemInfo._isGold = this._isGold;
            itemInfo._goldBeginTime = this._goldBeginTime;
            itemInfo._goldValidDate = this._goldValidDate;
            return itemInfo;
        }

        public static ItemInfo CloneFromTemplate(ItemTemplateInfo goods, ItemInfo item)
        {
            if (goods == null)
                return (ItemInfo)null;
            ItemInfo itemInfo1 = new ItemInfo(goods);
            itemInfo1.GoldEquip = item.GoldEquip;
            itemInfo1.AgilityCompose = item.AgilityCompose;
            itemInfo1.AttackCompose = item.AttackCompose;
            itemInfo1.BeginDate = item.BeginDate;
            itemInfo1.Color = item.Color;
            itemInfo1.Skin = item.Skin;
            itemInfo1.DefendCompose = item.DefendCompose;
            itemInfo1.IsBinds = item.IsBinds;
            itemInfo1.Place = item.Place;
            itemInfo1.BagType = item.BagType;
            itemInfo1.IsUsed = item.IsUsed;
            itemInfo1.IsDirty = item.IsDirty;
            itemInfo1.IsExist = item.IsExist;
            itemInfo1.IsJudge = item.IsJudge;
            itemInfo1.LuckCompose = item.LuckCompose;
            itemInfo1.StrengthenLevel = item.StrengthenLevel;
            itemInfo1.TemplateID = goods.TemplateID;
            itemInfo1.ValidDate = item.ValidDate;
            itemInfo1._template = goods;
            itemInfo1.Count = item.Count;
            itemInfo1._removeDate = item._removeDate;
            itemInfo1._removeType = item._removeType;
            itemInfo1.Hole1 = item.Hole1;
            itemInfo1.Hole2 = item.Hole2;
            itemInfo1.Hole3 = item.Hole3;
            itemInfo1.Hole4 = item.Hole4;
            itemInfo1.Hole5 = item.Hole5;
            itemInfo1.Hole6 = item.Hole6;
            itemInfo1.Hole5Level = item.Hole5Level;
            itemInfo1.Hole5Exp = item.Hole5Exp;
            itemInfo1.Hole6Level = item.Hole6Level;
            itemInfo1.Hole6Exp = item.Hole6Exp;
            itemInfo1.goldBeginTime = item.goldBeginTime;
            itemInfo1.goldValidDate = item.goldValidDate;
            itemInfo1.StrengthenExp = item.StrengthenExp;
            ItemInfo itemInfo2 = itemInfo1;
            ItemInfo.OpenHole(ref itemInfo2);
            return itemInfo2;
        }

        public void Copy(ItemInfo item)
        {
            this._userID = item.UserID;
            this._validDate = item.ValidDate;
            this._templateId = item.TemplateID;
            this._strengthenLevel = item.StrengthenLevel;
            this._luckCompose = item.LuckCompose;
            this._itemID = 0;
            this._isJudage = item.IsJudge;
            this._isExist = item.IsExist;
            this._isBinds = item.IsBinds;
            this._isUsed = item.IsUsed;
            this._defendCompose = item.DefendCompose;
            this._count = item.Count;
            this._color = item.Color;
            this._skin = item.Skin;
            this._beginDate = item.BeginDate;
            this._attackCompose = item.AttackCompose;
            this._agilityCompose = item.AgilityCompose;
            this._bagType = item.BagType;
            this._isDirty = item.IsDirty;
            this._removeDate = item.RemoveDate;
            this._removeType = item.RemoveType;
            this._hole1 = item.Hole1;
            this._hole2 = item.Hole2;
            this._hole3 = item.Hole3;
            this._hole4 = item.Hole4;
            this._hole5 = item.Hole5;
            this._hole6 = item.Hole6;
            this._hole5Exp = item.Hole5Exp;
            this._hole5Level = item.Hole5Level;
            this._hole6Exp = item.Hole6Exp;
            this._hole6Level = item.Hole6Level;
            this._isGold = item.IsGold;
            this._goldBeginTime = item.goldBeginTime;
            this._goldValidDate = item.goldValidDate;
        }

        public static ItemInfo CreateFromTemplate(ItemTemplateInfo goods, int count, int type)
        {
            if (goods == null)
                return (ItemInfo)null;
            ItemInfo itemInfo = new ItemInfo(goods);
            itemInfo.AgilityCompose = 0;
            itemInfo.AttackCompose = 0;
            itemInfo.BeginDate = DateTime.Now;
            itemInfo.Color = "";
            itemInfo.Skin = "";
            itemInfo.DefendCompose = 0;
            itemInfo.IsUsed = false;
            itemInfo.IsDirty = false;
            itemInfo.IsExist = true;
            itemInfo.IsJudge = true;
            itemInfo.LuckCompose = 0;
            itemInfo.StrengthenLevel = 0;
            itemInfo.TemplateID = goods.TemplateID;
            itemInfo.ValidDate = 0;
            itemInfo.Count = count;
            itemInfo.IsBinds = goods.BindType == 1;
            itemInfo._removeDate = DateTime.Now;
            itemInfo._removeType = type;
            itemInfo.Hole1 = -1;
            itemInfo.Hole2 = -1;
            itemInfo.Hole3 = -1;
            itemInfo.Hole4 = -1;
            itemInfo.Hole5 = -1;
            itemInfo.Hole6 = -1;
            itemInfo.Hole5Exp = 0;
            itemInfo.Hole5Level = 0;
            itemInfo.Hole6Exp = 0;
            itemInfo.Hole6Level = 0;
            itemInfo.goldValidDate = 0;
            itemInfo.goldBeginTime = DateTime.Now;
            return itemInfo;
        }

        public int eqType()
        {
            switch (this._template.CategoryID)
            {
                case 51:
                    return 1;
                case 52:
                    return 2;
                default:
                    return 0;
            }
        }

        public static ItemInfo FindSpecialItemInfo(
          ItemInfo info,
          ref int gold,
          ref int money,
          ref int giftToken)
        {
            int gp = 0;
            return ItemInfo.FindSpecialItemInfo(info, ref gold, ref money, ref giftToken, ref gp);
        }

        public static ItemInfo FindSpecialItemInfo(
          ItemInfo info,
          ref int gold,
          ref int money,
          ref int giftToken,
          ref int gp)
        {
            switch (info.TemplateID)
            {
                case -300:
                    giftToken += info.Count;
                    info = (ItemInfo)null;
                    break;
                case -200:
                    money += info.Count;
                    info = (ItemInfo)null;
                    break;
                case -100:
                    gold += info.Count;
                    info = (ItemInfo)null;
                    break;
                case 11107:
                    gp += info.Count;
                    info = (ItemInfo)null;
                    break;
            }
            return info;
        }

        public string GetBagName()
        {
            int categoryId = this._template.CategoryID;
            if (categoryId - 10 <= 1)
                return "Game.Server.GameObjects.Prop";
            return categoryId != 12 ? "Game.Server.GameObjects.Equip" : "Game.Server.GameObjects.Task";
        }

        public static void GetItemPrice(
          int Prices,
          int Values,
          Decimal beat,
          ref int gold,
          ref int money,
          ref int offer,
          ref int gifttoken,
          ref int iTemplateID,
          ref int iCount)
        {
            iTemplateID = 0;
            iCount = 0;
            switch (Prices - -4)
            {
                case 0:
                    gifttoken += (int)((Decimal)Values * beat);
                    break;
                case 1:
                    offer += (int)((Decimal)Values * beat);
                    break;
                case 2:
                    gold += (int)((Decimal)Values * beat);
                    break;
                case 3:
                    money += (int)((Decimal)Values * beat);
                    break;
                default:
                    if (Prices <= 0)
                        break;
                    iTemplateID = Prices;
                    iCount = Values;
                    break;
            }
        }

        public int GetRemainDate()
        {
            if (this.ValidDate == 0)
                return int.MaxValue;
            if (!this._isUsed)
                return this.ValidDate;
            int num = DateTime.Compare(this._beginDate.AddDays((double)this._validDate), DateTime.Now);
            if (num >= 0)
                return num;
            return 0;
        }

        public bool IsBead()
        {
            if (this._template.Property1 == 31)
                return this._template.CategoryID == 11;
            return false;
        }

        public bool IsCard()
        {
            int categoryId = this._template.CategoryID;
            if (categoryId != 11)
                return categoryId == 18;
            if (this._template.TemplateID != 112108)
                return this._template.TemplateID == 112150;
            return true;
        }

        public bool isDress()
        {
            switch (this._template.CategoryID)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 13:
                case 15:
                    return true;
                default:
                    return false;
            }
        }

        public bool isDrill(int holelv)
        {
            switch (this._template.TemplateID)
            {
                case 11026:
                    return holelv == 2;
                case 11027:
                    return holelv == 3;
                case 11034:
                    return holelv == 4;
                case 11035:
                    return holelv == 0;
                case 11036:
                    return holelv == 1;
                default:
                    return false;
            }
        }

        public bool IsEquipPet()
        {
            if (this._template.CategoryID != 50 && this._template.CategoryID != 51)
                return this._template.CategoryID == 52;
            return true;
        }

        public bool isGemStone()
        {
            if (this._template == null)
            {
                return false;
            }
            int templateID = this._template.TemplateID;
            if (templateID != 100100)
            {
                if (templateID != 201264)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsProp()
        {
            int categoryId = this._template.CategoryID;
            if (categoryId <= 18)
                return categoryId == 11 || categoryId == 18;
            switch (categoryId - 32)
            {
                case 0:
                case 2:
                case 3:
                case 8:
                    return true;
                case 1:
                    return false;
                default:
                    return false;
            }
        }

        public bool isTexp()
        {
            return this._template.CategoryID == 20;
        }

        public bool IsValidGoldItem()
        {
            if (this._goldValidDate > 0)
                return DateTime.Compare(this._goldBeginTime.AddDays((double)this._goldValidDate), DateTime.Now) > 0;
            return false;
        }

        public bool IsValidItem()
        {
            if (this._validDate != 0 && this._isUsed)
                return DateTime.Compare(this._beginDate.AddDays((double)this._validDate), DateTime.Now) > 0;
            return true;
        }

        public static void OpenHole(ref ItemInfo item)
        {
            string[] strArray1 = item.Template.Hole.Split('|');
            for (int index = 0; index < strArray1.Length; ++index)
            {
                string[] strArray2 = strArray1[index].Split(',');
                if (item.StrengthenLevel >= Convert.ToInt32(strArray2[0]) && Convert.ToInt32(strArray2[1]) != -1)
                {
                    switch (index)
                    {
                        case 0:
                            if (item.Hole1 < 0)
                            {
                                item.Hole1 = 0;
                                continue;
                            }
                            continue;
                        case 1:
                            if (item.Hole2 < 0)
                            {
                                item.Hole2 = 0;
                                continue;
                            }
                            continue;
                        case 2:
                            if (item.Hole3 < 0)
                            {
                                item.Hole3 = 0;
                                continue;
                            }
                            continue;
                        case 3:
                            if (item.Hole4 < 0)
                            {
                                item.Hole4 = 0;
                                continue;
                            }
                            continue;
                        case 4:
                            if (item.Hole5 < 0)
                            {
                                item.Hole5 = 0;
                                continue;
                            }
                            continue;
                        case 5:
                            if (item.Hole6 < 0)
                            {
                                item.Hole6 = 0;
                                continue;
                            }
                            continue;
                        default:
                            continue;
                    }
                }
            }
        }

        public static List<int> SetItemType(
          ShopItemInfo shop,
          int type,
          ref int gold,
          ref int money,
          ref int offer,
          ref int gifttoken)
        {
            int iTemplateID = 0;
            int iCount = 0;
            List<int> intList = new List<int>();
            if (type == 1)
            {
                ItemInfo.GetItemPrice(shop.APrice1, shop.AValue1, shop.Beat, ref gold, ref money, ref offer, ref gifttoken, ref iTemplateID, ref iCount);
                if (iTemplateID > 0)
                {
                    intList.Add(iTemplateID);
                    intList.Add(iCount);
                }
                ItemInfo.GetItemPrice(shop.APrice2, shop.AValue2, shop.Beat, ref gold, ref money, ref offer, ref gifttoken, ref iTemplateID, ref iCount);
                if (iTemplateID > 0)
                {
                    intList.Add(iTemplateID);
                    intList.Add(iCount);
                }
                ItemInfo.GetItemPrice(shop.APrice3, shop.AValue3, shop.Beat, ref gold, ref money, ref offer, ref gifttoken, ref iTemplateID, ref iCount);
                if (iTemplateID > 0)
                {
                    intList.Add(iTemplateID);
                    intList.Add(iCount);
                }
            }
            if (type == 2)
            {
                ItemInfo.GetItemPrice(shop.BPrice1, shop.BValue1, shop.Beat, ref gold, ref money, ref offer, ref gifttoken, ref iTemplateID, ref iCount);
                if (iTemplateID > 0)
                {
                    intList.Add(iTemplateID);
                    intList.Add(iCount);
                }
                ItemInfo.GetItemPrice(shop.BPrice2, shop.BValue2, shop.Beat, ref gold, ref money, ref offer, ref gifttoken, ref iTemplateID, ref iCount);
                if (iTemplateID > 0)
                {
                    intList.Add(iTemplateID);
                    intList.Add(iCount);
                }
                ItemInfo.GetItemPrice(shop.BPrice3, shop.BValue3, shop.Beat, ref gold, ref money, ref offer, ref gifttoken, ref iTemplateID, ref iCount);
                if (iTemplateID > 0)
                {
                    intList.Add(iTemplateID);
                    intList.Add(iCount);
                }
            }
            if (type == 3)
            {
                ItemInfo.GetItemPrice(shop.CPrice1, shop.CValue1, shop.Beat, ref gold, ref money, ref offer, ref gifttoken, ref iTemplateID, ref iCount);
                if (iTemplateID > 0)
                {
                    intList.Add(iTemplateID);
                    intList.Add(iCount);
                }
                ItemInfo.GetItemPrice(shop.CPrice2, shop.CValue2, shop.Beat, ref gold, ref money, ref offer, ref gifttoken, ref iTemplateID, ref iCount);
                if (iTemplateID > 0)
                {
                    intList.Add(iTemplateID);
                    intList.Add(iCount);
                }
                ItemInfo.GetItemPrice(shop.CPrice3, shop.CValue3, shop.Beat, ref gold, ref money, ref offer, ref gifttoken, ref iTemplateID, ref iCount);
                if (iTemplateID > 0)
                {
                    intList.Add(iTemplateID);
                    intList.Add(iCount);
                }
            }
            return intList;
        }

        public int Agility
        {
            get
            {
                int agility = this._template.Agility;
                if (this.IsGold && this.GoldEquip != null)
                    agility = this.GoldEquip.Agility;
                return this._agilityCompose + agility;
            }
        }

        public int AgilityCompose
        {
            get
            {
                return this._agilityCompose;
            }
            set
            {
                this._agilityCompose = value;
                this._isDirty = true;
            }
        }

        public int Attack
        {
            get
            {
                int attack = this._template.Attack;
                if (this.IsGold && this.GoldEquip != null)
                    attack = this.GoldEquip.Attack;
                return this._attackCompose + attack;
            }
        }

        public int AttackCompose
        {
            get
            {
                return this._attackCompose;
            }
            set
            {
                this._attackCompose = value;
                this._isDirty = true;
            }
        }

        public int BagType
        {
            get
            {
                return this._bagType;
            }
            set
            {
                this._bagType = value;
                this._isDirty = true;
            }
        }

        public DateTime BeginDate
        {
            get
            {
                return this._beginDate;
            }
            set
            {
                this._beginDate = value;
                this._isDirty = true;
            }
        }

        public string Color
        {
            get
            {
                return this._color;
            }
            set
            {
                this._color = value;
                this._isDirty = true;
            }
        }

        public int Count
        {
            get
            {
                return this._count;
            }
            set
            {
                this._count = value;
                this._isDirty = true;
            }
        }

        public int Defence
        {
            get
            {
                int defence = this._template.Defence;
                if (this.IsGold && this.GoldEquip != null)
                    defence = this.GoldEquip.Defence;
                return this._defendCompose + defence;
            }
        }

        public int DefendCompose
        {
            get
            {
                return this._defendCompose;
            }
            set
            {
                this._defendCompose = value;
                this._isDirty = true;
            }
        }

        public int GetBagType
        {
            get
            {
                return (int)this._template.BagType;
            }
        }

        public int GetBagTypee()
        {
            int categoryId = this._template.CategoryID;
            if (categoryId - 10 <= 1)
                return 1;
            return categoryId != 12 ? 0 : 2;
        }

        public DateTime goldBeginTime
        {
            get
            {
                return this._goldBeginTime;
            }
            set
            {
                this._goldBeginTime = value;
                this._isDirty = true;
            }
        }

        public ItemTemplateInfo GoldEquip
        {
            get
            {
                return this._goldEquip;
            }
            set
            {
                this._goldEquip = value;
                this._isDirty = true;
            }
        }

        public int goldValidDate
        {
            get
            {
                return this._goldValidDate;
            }
            set
            {
                this._goldValidDate = value;
                this._isDirty = true;
            }
        }

        public int Hole1
        {
            get
            {
                return this._hole1;
            }
            set
            {
                this._hole1 = value;
                this._isDirty = true;
            }
        }

        public int Hole2
        {
            get
            {
                return this._hole2;
            }
            set
            {
                this._hole2 = value;
                this._isDirty = true;
            }
        }

        public int Hole3
        {
            get
            {
                return this._hole3;
            }
            set
            {
                this._hole3 = value;
                this._isDirty = true;
            }
        }

        public int Hole4
        {
            get
            {
                return this._hole4;
            }
            set
            {
                this._hole4 = value;
                this._isDirty = true;
            }
        }

        public int Hole5
        {
            get
            {
                return this._hole5;
            }
            set
            {
                this._hole5 = value;
                this._isDirty = true;
            }
        }

        public int Hole5Exp
        {
            get
            {
                return this._hole5Exp;
            }
            set
            {
                this._hole5Exp = value;
                this._isDirty = true;
            }
        }

        public int Hole5Level
        {
            get
            {
                return this._hole5Level;
            }
            set
            {
                this._hole5Level = value;
                this._isDirty = true;
            }
        }

        public int Hole6
        {
            get
            {
                return this._hole6;
            }
            set
            {
                this._hole6 = value;
                this._isDirty = true;
            }
        }

        public int Hole6Exp
        {
            get
            {
                return this._hole6Exp;
            }
            set
            {
                this._hole6Exp = value;
                this._isDirty = true;
            }
        }

        public int Hole6Level
        {
            get
            {
                return this._hole6Level;
            }
            set
            {
                this._hole6Level = value;
                this._isDirty = true;
            }
        }

        public bool IsBinds
        {
            get
            {
                return this._isBinds;
            }
            set
            {
                this._isBinds = value;
                this._isDirty = true;
            }
        }

        public bool IsExist
        {
            get
            {
                return this._isExist;
            }
            set
            {
                this._isExist = value;
                this._isDirty = true;
            }
        }

        public bool IsGold
        {
            get
            {
                return this.IsValidGoldItem();
            }
        }

        public bool IsJudge
        {
            get
            {
                return this._isJudage;
            }
            set
            {
                this._isJudage = value;
                this._isDirty = true;
            }
        }

        public bool IsLogs
        {
            get
            {
                return this._isLogs;
            }
            set
            {
                this._isLogs = value;
            }
        }

        public bool IsTips
        {
            get
            {
                return this._isTips;
            }
            set
            {
                this._isTips = value;
            }
        }

        public bool IsUsed
        {
            get
            {
                return this._isUsed;
            }
            set
            {
                if (this._isUsed == value)
                    return;
                this._isUsed = value;
                this._isDirty = true;
            }
        }

        public int ItemID
        {
            get
            {
                return this._itemID;
            }
            set
            {
                this._itemID = value;
                this._isDirty = true;
            }
        }

        public int Luck
        {
            get
            {
                int luck = this._template.Luck;
                if (this.IsGold && this.GoldEquip != null)
                    luck = this.GoldEquip.Luck;
                return this._luckCompose + luck;
            }
        }

        public int LuckCompose
        {
            get
            {
                return this._luckCompose;
            }
            set
            {
                this._luckCompose = value;
                this._isDirty = true;
            }
        }

        public string Pic
        {
            get
            {
                if (this.IsGold && this.GoldEquip != null)
                    return this.GoldEquip.Pic;
                return this._template.Pic;
            }
        }

        public int Place
        {
            get
            {
                return this._place;
            }
            set
            {
                this._place = value;
                this._isDirty = true;
            }
        }

        public int RefineryLevel
        {
            get
            {
                if (this.IsGold && this.GoldEquip != null)
                    return this.GoldEquip.RefineryLevel;
                return this._template.RefineryLevel;
            }
        }

        public DateTime RemoveDate
        {
            get
            {
                return this._removeDate;
            }
            set
            {
                this._removeDate = value;
                this._isDirty = true;
            }
        }

        public int RemoveType
        {
            get
            {
                return this._removeType;
            }
            set
            {
                this._removeType = value;
                this._removeDate = DateTime.Now;
                this._isDirty = true;
            }
        }

        public string Skin
        {
            get
            {
                return this._skin;
            }
            set
            {
                this._skin = value;
                this._isDirty = true;
            }
        }

        public int StrengthenLevel
        {
            get
            {
                return this._strengthenLevel;
            }
            set
            {
                this._strengthenLevel = value;
                this._isDirty = true;
            }
        }

        public int StrengthenExp
        {
            get
            {
                return this._strengthenExp;
            }
            set
            {
                this._strengthenExp = value;
                this._isDirty = true;
            }
        }

        public int StrengthenTimes
        {
            get
            {
                return this._strengthenTimes;
            }
            set
            {
                this._strengthenTimes = value;
                this._isDirty = true;
            }
        }

        public ItemTemplateInfo Template
        {
            get
            {
                return this._template;
            }
        }

        public int TemplateID
        {
            get
            {
                if (this.IsGold && this.GoldEquip != null)
                    return this.GoldEquip.TemplateID;
                return this._templateId;
            }
            set
            {
                this._templateId = value;
                this._isDirty = true;
            }
        }

        public int UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
                this._isDirty = true;
            }
        }

        public int ValidDate
        {
            get
            {
                return this._validDate;
            }
            set
            {
                this._validDate = value > 999 ? 365 : value;
                this._isDirty = true;
            }
        }

        public bool isGold
        {
            get
            {
                return this.IsValidGoldItem();
            }
        }

        public bool IsAdvanceDate()
        {
            return this._advanceDate.Date < DateTime.Now.Date;
        }

        public DateTime AdvanceDate
        {
            get
            {
                return this._advanceDate;
            }
            set
            {
                this._advanceDate = value;
                this._isDirty = true;
            }
        }
    }
}
