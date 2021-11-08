// Decompiled with JetBrains decompiler
// Type: Game.Server.GameUtils.CardAbstractInventory
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.GameUtils
{
  public abstract class CardAbstractInventory
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected object m_lock;
    private int int_0;
    private int int_1;
    protected UsersCardInfo[] m_cards;
    protected UsersCardInfo temp_card;
    protected List<int> m_changedPlaces;
    private int int_2;

    public int BeginSlot
    {
      get
      {
        return this.int_1;
      }
    }

    public int Capalility
    {
      get
      {
        return this.int_0;
      }
      set
      {
        this.int_0 = value < 0 ? 0 : (value > this.m_cards.Length ? this.m_cards.Length : value);
      }
    }

    public bool IsEmpty(int slot)
    {
      if (slot >= 0 && slot < this.int_0)
        return this.m_cards[slot] == null;
      return true;
    }

    public CardAbstractInventory(int capability, int beginSlot)
    {
      this.m_lock = new object();
      this.m_changedPlaces = new List<int>();
      this.int_0 = capability;
      this.int_1 = beginSlot;
      this.m_cards = new UsersCardInfo[capability];
      this.temp_card = new UsersCardInfo();
    }

    public virtual void UpdateTempCard(UsersCardInfo card)
    {
      lock (this.m_lock)
        this.temp_card = card;
    }

    public virtual void UpdateCard(UsersCardInfo card)
    {
      this.OnPlaceChanged(card.Place);
    }

    public virtual void UpdateCard()
    {
      int place = this.temp_card.Place;
      int templateId = this.temp_card.TemplateID;
      if (place < 5)
      {
        this.ReplaceCardTo(this.temp_card, place);
        int placeByTamplateId = this.FindPlaceByTamplateId(5, templateId);
        this.MoveCard(place, placeByTamplateId);
      }
      else
      {
        this.ReplaceCardTo(this.temp_card, place);
        int placeByTamplateId = this.FindPlaceByTamplateId(0, 5, templateId);
        if (this.GetItemAt(placeByTamplateId) == null || this.GetItemAt(placeByTamplateId).TemplateID != templateId)
          return;
        this.MoveCard(place, placeByTamplateId);
      }
    }

    public bool AddCard(UsersCardInfo card)
    {
      return this.AddCard(card, this.int_1);
    }

    public bool AddCard(UsersCardInfo card, int minSlot)
    {
      if (card == null)
        return false;
      int firstEmptySlot = this.FindFirstEmptySlot(minSlot);
      return this.AddCardTo(card, firstEmptySlot);
    }

    public virtual bool AddCardTo(UsersCardInfo card, int place)
    {
      if (card == null || place >= this.int_0 || place < 0)
        return false;
      lock (this.m_lock)
      {
        if (this.m_cards[place] != null)
        {
          place = -1;
        }
        else
        {
          this.m_cards[place] = card;
          card.Place = place;
        }
      }
      if (place != -1)
        this.OnPlaceChanged(place);
      return place != -1;
    }

    public virtual bool RemoveCardAt(int place)
    {
      return this.RemoveCard(this.GetItemAt(place));
    }

    public virtual bool RemoveCard(UsersCardInfo item)
    {
      if (item == null)
        return false;
      int place = -1;
      lock (this.m_lock)
      {
        for (int index = 0; index < this.int_0; ++index)
        {
          if (this.m_cards[index] == item)
          {
            place = index;
            this.m_cards[index] = (UsersCardInfo) null;
            break;
          }
        }
      }
      if (place != -1)
      {
        this.OnPlaceChanged(place);
        item.Place = -1;
      }
      return place != -1;
    }

    public virtual bool ReplaceCardTo(UsersCardInfo card, int place)
    {
      if (card == null || place >= this.int_0 || place < 0)
        return false;
      lock (this.m_lock)
      {
        if (this.m_cards[place] != null)
          this.RemoveCard(this.m_cards[place]);
        this.m_cards[place] = card;
        card.Place = place;
        this.OnPlaceChanged(place);
      }
      return true;
    }

    public virtual bool MoveCard(int fromSlot, int toSlot)
    {
      if (fromSlot < 0 || toSlot < 0 || (fromSlot >= this.int_0 || toSlot >= this.int_0))
        return false;
      bool flag = false;
      lock (this.m_lock)
        flag = this.StackCards(fromSlot, toSlot) || this.ExchangeCards(fromSlot, toSlot);
      if (flag)
      {
        this.BeginChanges();
        try
        {
          this.OnPlaceChanged(fromSlot);
          this.OnPlaceChanged(toSlot);
        }
        finally
        {
          this.CommitChanges();
        }
      }
      return flag;
    }

    protected virtual bool StackCards(int fromSlot, int toSlot)
    {
      UsersCardInfo card1 = this.m_cards[fromSlot];
      UsersCardInfo card2 = this.m_cards[toSlot];
      if (card1 == null || card2 == null || card2.TemplateID != card1.TemplateID)
        return false;
      card2.Count += card1.Count;
      this.RemoveCard(card1);
      return true;
    }

    public bool IsSolt(int slot)
    {
      if (slot >= 0)
        return slot < this.int_0;
      return false;
    }

    protected virtual bool ExchangeCards(int fromSlot, int toSlot)
    {
      UsersCardInfo card1 = this.m_cards[toSlot];
      UsersCardInfo card2 = this.m_cards[fromSlot];
      this.m_cards[fromSlot] = card1;
      this.m_cards[toSlot] = card2;
      if (card1 != null)
        card1.Place = fromSlot;
      if (card2 != null)
        card2.Place = toSlot;
      return true;
    }

    public virtual bool ResetCardSoul()
    {
      lock (this.m_lock)
      {
        for (int index = 0; index < 5; ++index)
        {
          this.m_cards[index].Level = 0;
          this.m_cards[index].CardGP = 0;
        }
      }
      return true;
    }

    public virtual bool UpGraceSlot(int soulPoint, int lv, int place)
    {
      lock (this.m_lock)
      {
        this.m_cards[place].CardGP += soulPoint;
        this.m_cards[place].Level = lv;
      }
      return true;
    }

    public virtual UsersCardInfo GetItemAt(int slot)
    {
      if (slot >= 0 && slot < this.int_0)
        return this.m_cards[slot];
      return (UsersCardInfo) null;
    }

    public virtual List<UsersCardInfo> GetEquipCard()
    {
      List<UsersCardInfo> usersCardInfoList = new List<UsersCardInfo>();
      for (int index = 0; index < 5; ++index)
      {
        if (this.m_cards[index] != null)
          usersCardInfoList.Add(this.m_cards[index]);
      }
      return usersCardInfoList;
    }

    public int FindFirstEmptySlot()
    {
      return this.FindFirstEmptySlot(this.int_1);
    }

    public int FindFirstEmptySlot(int minSlot)
    {
      if (minSlot >= this.int_0)
        return -1;
      lock (this.m_lock)
      {
        for (int index = minSlot; index < this.int_0; ++index)
        {
          if (this.m_cards[index] == null)
            return index;
        }
        return -1;
      }
    }

    public int FindPlaceByTamplateId(int minSlot, int templateId)
    {
      if (minSlot >= this.int_0)
        return -1;
      lock (this.m_lock)
      {
        for (int index = minSlot; index < this.int_0; ++index)
        {
          if (this.m_cards[index] != null && this.m_cards[index].TemplateID == templateId)
            return this.m_cards[index].Place;
        }
        return -1;
      }
    }

    public bool FindEquipCard(int templateId)
    {
      lock (this.m_lock)
      {
        for (int index = 0; index < 5; ++index)
        {
          if (this.m_cards[index].TemplateID == templateId)
            return true;
        }
        return false;
      }
    }

    public int FindPlaceByTamplateId(int minSlot, int maxSlot, int templateId)
    {
      if (minSlot >= this.int_0)
        return -1;
      lock (this.m_lock)
      {
        for (int index = minSlot; index < maxSlot; ++index)
        {
          if (this.m_cards[index] != null && this.m_cards[index].TemplateID == templateId)
            return this.m_cards[index].Place;
        }
        return -1;
      }
    }

    public int FindLastEmptySlot()
    {
      lock (this.m_lock)
      {
        for (int index = this.int_0 - 1; index >= 0; --index)
        {
          if (this.m_cards[index] == null)
            return index;
        }
        return -1;
      }
    }

    public virtual void Clear()
    {
      lock (this.m_lock)
      {
        for (int index = 0; index < this.int_0; ++index)
          this.m_cards[index] = (UsersCardInfo) null;
      }
    }

    public virtual UsersCardInfo GetItemByTemplateID(int templateId)
    {
      return this.GetItemByTemplateID(this.int_1, templateId);
    }

    public virtual UsersCardInfo GetItemByTemplateID(int minSlot, int templateId)
    {
      lock (this.m_lock)
      {
        for (int index = minSlot; index < this.int_0; ++index)
        {
          if (this.m_cards[index] != null && this.m_cards[index].TemplateID == templateId)
            return this.m_cards[index];
        }
        return (UsersCardInfo) null;
      }
    }

    public virtual UsersCardInfo GetItemByPlace(int minSlot, int place)
    {
      lock (this.m_lock)
      {
        for (int index = minSlot; index < this.int_0; ++index)
        {
          if (this.m_cards[index] != null && this.m_cards[index].Place == place)
            return this.m_cards[index];
        }
        return (UsersCardInfo) null;
      }
    }

    public virtual List<UsersCardInfo> GetCards()
    {
      return this.GetCards(0, this.int_0 - 1);
    }

    public virtual List<UsersCardInfo> GetCards(int minSlot, int maxSlot)
    {
      List<UsersCardInfo> usersCardInfoList = new List<UsersCardInfo>();
      lock (this.m_lock)
      {
        for (int index = minSlot; index <= maxSlot; ++index)
        {
          if (this.m_cards[index] != null)
            usersCardInfoList.Add(this.m_cards[index]);
        }
      }
      return usersCardInfoList;
    }

    public UsersCardInfo GetCardEquip(int templateid)
    {
      foreach (UsersCardInfo card in this.GetCards(0, 4))
      {
        if (card.TemplateID == templateid)
          return card;
      }
      return (UsersCardInfo) null;
    }

    public int GetEmptyCount()
    {
      return this.GetEmptyCount(this.int_1);
    }

    public virtual int GetEmptyCount(int minSlot)
    {
      if (minSlot < 0 || minSlot > this.int_0 - 1)
        return 0;
      int num = 0;
      lock (this.m_lock)
      {
        for (int index = minSlot; index < this.int_0; ++index)
        {
          if (this.m_cards[index] == null)
            ++num;
        }
      }
      return num;
    }

    protected void OnPlaceChanged(int place)
    {
      if (!this.m_changedPlaces.Contains(place))
        this.m_changedPlaces.Add(place);
      if (this.int_2 > 0 || this.m_changedPlaces.Count <= 0)
        return;
      this.UpdateChangedPlaces();
    }

    public void BeginChanges()
    {
      Interlocked.Increment(ref this.int_2);
    }

    public void CommitChanges()
    {
      int num = Interlocked.Decrement(ref this.int_2);
      if (num < 0)
      {
        if (CardAbstractInventory.ilog_0.IsErrorEnabled)
          CardAbstractInventory.ilog_0.Error((object) ("Inventory changes counter is bellow zero (forgot to use BeginChanges?)!\n\n" + Environment.StackTrace));
        Thread.VolatileWrite(ref this.int_2, 0);
      }
      if (num > 0 || this.m_changedPlaces.Count <= 0)
        return;
      this.UpdateChangedPlaces();
    }

    public virtual void UpdateChangedPlaces()
    {
      this.m_changedPlaces.Clear();
    }

    public void ClearBag()
    {
      this.BeginChanges();
      lock (this.m_lock)
      {
        for (int index = 5; index < this.int_0; ++index)
        {
          if (this.m_cards[index] != null)
            this.RemoveCard(this.m_cards[index]);
        }
      }
      this.CommitChanges();
    }

    public UsersCardInfo[] GetRawSpaces()
    {
      lock (this.m_lock)
        return this.m_cards.Clone() as UsersCardInfo[];
    }
  }
}
