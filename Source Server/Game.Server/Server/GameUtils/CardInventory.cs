// Decompiled with JetBrains decompiler
// Type: Game.Server.GameUtils.CardInventory
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.GameUtils
{
  public class CardInventory : CardAbstractInventory
  {
    protected GamePlayer m_player;
    private bool bool_0;
    private List<UsersCardInfo> list_0;

    public GamePlayer Player
    {
      get
      {
        return this.m_player;
      }
    }

    public CardInventory(GamePlayer player, bool saveTodb, int capibility, int beginSlot)
      : base(capibility, beginSlot)
    {
      this.list_0 = new List<UsersCardInfo>();
      this.m_player = player;
      this.bool_0 = saveTodb;
    }

    public virtual void LoadFromDatabase()
    {
      if (!this.bool_0)
        return;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        UsersCardInfo[] singleUserCard = playerBussiness.GetSingleUserCard(this.m_player.PlayerCharacter.ID);
        this.BeginChanges();
        try
        {
          foreach (UsersCardInfo card in singleUserCard)
            this.AddCardTo(card, card.Place);
        }
        finally
        {
          this.CommitChanges();
        }
      }
    }

    public virtual void SaveToDatabase()
    {
      if (!this.bool_0)
        return;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        lock (this.m_lock)
        {
          for (int index = 0; index < this.m_cards.Length; ++index)
          {
            UsersCardInfo card = this.m_cards[index];
            if (card != null && card.IsDirty)
            {
              if (card.CardID > 0)
                playerBussiness.UpdateCards(card);
              else if (card.CardID == 0 && card.Place != -1)
                playerBussiness.AddCards(card);
            }
          }
        }
        lock (this.list_0)
        {
          foreach (UsersCardInfo usersCardInfo in this.list_0)
          {
            if (usersCardInfo.CardID > 0)
              playerBussiness.UpdateCards(usersCardInfo);
          }
          this.list_0.Clear();
        }
      }
    }

    public virtual bool AddCard(int templateId, int count)
    {
      UsersCardInfo itemByTemplateId = this.GetItemByTemplateID(templateId);
      if (itemByTemplateId == null)
        return this.AddCard(new UsersCardInfo(this.m_player.PlayerCharacter.ID, templateId, count));
      if(itemByTemplateId.Count >= 1000)
                return false;
      itemByTemplateId.Count += count;
      this.UpdateCard(itemByTemplateId);
      return true;
    }

    public override bool AddCardTo(UsersCardInfo item, int place)
    {
      if (!base.AddCardTo(item, place))
        return false;
      item.UserID = this.m_player.PlayerCharacter.ID;
      return true;
    }

    public override bool RemoveCardAt(int place)
    {
      UsersCardInfo itemAt = this.GetItemAt(place);
      if (itemAt == null)
        return false;
      this.list_0.Add(itemAt);
      base.RemoveCardAt(place);
      return true;
    }

    public override bool RemoveCard(UsersCardInfo item)
    {
      if (item == null)
        return false;
      this.list_0.Add(item);
      base.RemoveCard(item);
      return true;
    }

    public override void UpdateChangedPlaces()
    {
      this.m_player.Out.SendUpdateCardData(this, this.m_changedPlaces.ToArray());
      base.UpdateChangedPlaces();
    }

    public bool IsCardEquip(int templateid)
    {
      foreach (UsersCardInfo usersCardInfo in this.GetEquipCard())
      {
        if (usersCardInfo.TemplateID == templateid)
          return true;
      }
      return false;
    }
  }
}
