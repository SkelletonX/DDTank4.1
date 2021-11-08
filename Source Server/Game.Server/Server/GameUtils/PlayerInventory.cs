// Decompiled with JetBrains decompiler
// Type: Game.Server.GameUtils.PlayerInventory
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Server.GameObjects;
using Game.Server.Packets;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Server.GameUtils
{
  public class PlayerInventory : AbstractInventory
  {
    protected GamePlayer m_player;
    private bool bool_1;
    private List<SqlDataProvider.Data.ItemInfo> list_0;

    public GamePlayer Player
    {
      get
      {
        return this.m_player;
      }
    }

    public PlayerInventory(
      GamePlayer player,
      bool saveTodb,
      int capibility,
      int type,
      int beginSlot,
      bool autoStack)
      : base(capibility, type, beginSlot, autoStack)
    {
      this.list_0 = new List<SqlDataProvider.Data.ItemInfo>();
      this.m_player = player;
      this.bool_1 = saveTodb;
    }

    public virtual void LoadFromDatabase()
    {
      if (!this.bool_1)
        return;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        SqlDataProvider.Data.ItemInfo[] userBagByType = playerBussiness.GetUserBagByType(this.m_player.PlayerCharacter.ID, this.BagType);
        this.BeginChanges();
        try
        {
          foreach (SqlDataProvider.Data.ItemInfo cloneItem in userBagByType)
          {
            if (this.IsWrongPlace(cloneItem) && cloneItem.Place < 31 && this.BagType == 0)
            {
              int firstEmptySlot = this.FindFirstEmptySlot(31);
              if (firstEmptySlot != -1)
                this.MoveItem(cloneItem.Place, firstEmptySlot, cloneItem.Count);
              else
                this.m_player.AddTemplate(cloneItem);
            }
            else
              this.AddItemTo(cloneItem, cloneItem.Place);
          }
        }
        finally
        {
          this.CommitChanges();
        }
      }
    }

    public bool IsWrongPlace(SqlDataProvider.Data.ItemInfo item)
    {
      if (item == null || item.Template == null)
        return false;
      if (item.Template.CategoryID == 7 && item.Place != 6 || item.Template.CategoryID == 27 && item.Place != 6 || item.Template.CategoryID == 17 && item.Place != 15)
        return true;
      if (item.Template.CategoryID == 31)
        return item.Place != 15;
      return false;
    }

    public bool IsEquipSlot(int slot)
    {
      if (slot >= 0)
        return slot < this.BeginSlot;
      return false;
    }

    public virtual void SaveToDatabase()
    {
      if (!this.bool_1)
        return;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        lock (this.m_lock)
        {
          for (int index = 0; index < this.m_items.Length; ++index)
          {
            SqlDataProvider.Data.ItemInfo itemInfo = this.m_items[index];
            if (itemInfo != null && itemInfo.IsDirty)
            {
              if (itemInfo.ItemID > 0)
                playerBussiness.UpdateGoods(itemInfo);
              else
                playerBussiness.AddGoods(itemInfo);
            }
          }
        }
        lock (this.list_0)
        {
          foreach (SqlDataProvider.Data.ItemInfo itemInfo in this.list_0)
          {
            if (itemInfo.ItemID > 0)
              playerBussiness.UpdateGoods(itemInfo);
          }
          this.list_0.Clear();
        }
      }
    }

    public virtual void SaveNewsItemIntoDatabas()
    {
      if (!this.bool_1)
        return;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        lock (this.m_lock)
        {
          for (int index = 0; index < this.m_items.Length; ++index)
          {
            SqlDataProvider.Data.ItemInfo itemInfo = this.m_items[index];
            if (itemInfo != null && itemInfo.IsDirty && itemInfo.ItemID == 0)
              playerBussiness.AddGoods(itemInfo);
          }
        }
      }
    }

    public override bool AddItemTo(SqlDataProvider.Data.ItemInfo item, int place)
    {
      if (!base.AddItemTo(item, place))
        return false;
      item.UserID = this.m_player.PlayerCharacter.ID;
      item.IsExist = true;
      return true;
    }

    public override bool TakeOutItem(SqlDataProvider.Data.ItemInfo item)
    {
      if (!base.TakeOutItem(item))
        return false;
      if (this.bool_1)
      {
        lock (this.list_0)
          this.list_0.Add(item);
      }
      return true;
    }

    public override bool RemoveItem(SqlDataProvider.Data.ItemInfo item)
    {
      if (!base.RemoveItem(item))
        return false;
      item.IsExist = false;
      if (this.bool_1)
      {
        lock (this.list_0)
          this.list_0.Add(item);
      }
      return true;
    }

    public override void UpdateChangedPlaces()
    {
      this.m_player.Out.SendUpdateInventorySlot(this, this.m_changedPlaces.ToArray());
      this.m_player.UpdateProperties();
      base.UpdateChangedPlaces();
    }

    public bool SendAllItemsToMail(string sender, string title, eMailType type)
    {
      if (this.bool_1)
      {
        this.BeginChanges();
        try
        {
          using (PlayerBussiness pb = new PlayerBussiness())
          {
            lock (this.m_lock)
            {
              List<SqlDataProvider.Data.ItemInfo> items1 = this.GetItems();
              int count = items1.Count;
              for (int index1 = 0; index1 < count; index1 += 5)
              {
                MailInfo mail = new MailInfo();
                mail.SenderID = 0;
                mail.Sender = sender;
                mail.ReceiverID = this.m_player.PlayerCharacter.ID;
                mail.Receiver = this.m_player.PlayerCharacter.NickName;
                mail.Title = title;
                mail.Type = (int) type;
                mail.Content = "";
                List<SqlDataProvider.Data.ItemInfo> items2 = new List<SqlDataProvider.Data.ItemInfo>();
                for (int index2 = 0; index2 < 5; ++index2)
                {
                  int index3 = index1 * 5 + index2;
                  if (index3 < items1.Count)
                    items2.Add(items1[index3]);
                }
                if (!this.SendItemsToMail(items2, mail, pb))
                  return false;
              }
            }
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("Send Items Mail Error:" + (object) ex);
        }
        finally
        {
          this.SaveToDatabase();
          this.CommitChanges();
        }
        this.m_player.Out.SendMailResponse(this.m_player.PlayerCharacter.ID, eMailRespose.Receiver);
      }
      return true;
    }

    public bool SendItemsToMail(List<SqlDataProvider.Data.ItemInfo> items, MailInfo mail, PlayerBussiness pb)
    {
      if (mail == null || items.Count > 5 || !this.bool_1)
        return false;
      List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(LanguageMgr.GetTranslation("Game.Server.GameUtils.CommonBag.AnnexRemark"));
      int itemId;
      if (items.Count > 0 && this.TakeOutItem(items[0]))
      {
        SqlDataProvider.Data.ItemInfo itemInfo = items[0];
        MailInfo mailInfo = mail;
        itemId = itemInfo.ItemID;
        string str = itemId.ToString();
        mailInfo.Annex1 = str;
        mail.Annex1Name = itemInfo.Template.Name;
        stringBuilder.Append("1、" + mail.Annex1Name + "x" + (object) itemInfo.Count + ";");
        itemInfoList.Add(itemInfo);
      }
      if (items.Count > 1 && this.TakeOutItem(items[1]))
      {
        SqlDataProvider.Data.ItemInfo itemInfo = items[1];
        MailInfo mailInfo = mail;
        itemId = itemInfo.ItemID;
        string str = itemId.ToString();
        mailInfo.Annex2 = str;
        mail.Annex2Name = itemInfo.Template.Name;
        stringBuilder.Append("2、" + mail.Annex2Name + "x" + (object) itemInfo.Count + ";");
        itemInfoList.Add(itemInfo);
      }
      if (items.Count > 2 && this.TakeOutItem(items[2]))
      {
        SqlDataProvider.Data.ItemInfo itemInfo = items[2];
        MailInfo mailInfo = mail;
        itemId = itemInfo.ItemID;
        string str = itemId.ToString();
        mailInfo.Annex3 = str;
        mail.Annex3Name = itemInfo.Template.Name;
        stringBuilder.Append("3、" + mail.Annex3Name + "x" + (object) itemInfo.Count + ";");
        itemInfoList.Add(itemInfo);
      }
      if (items.Count > 3 && this.TakeOutItem(items[3]))
      {
        SqlDataProvider.Data.ItemInfo itemInfo = items[3];
        MailInfo mailInfo = mail;
        itemId = itemInfo.ItemID;
        string str = itemId.ToString();
        mailInfo.Annex4 = str;
        mail.Annex4Name = itemInfo.Template.Name;
        stringBuilder.Append("4、" + mail.Annex4Name + "x" + (object) itemInfo.Count + ";");
        itemInfoList.Add(itemInfo);
      }
      if (items.Count > 4 && this.TakeOutItem(items[4]))
      {
        SqlDataProvider.Data.ItemInfo itemInfo = items[4];
        MailInfo mailInfo = mail;
        itemId = itemInfo.ItemID;
        string str = itemId.ToString();
        mailInfo.Annex5 = str;
        mail.Annex5Name = itemInfo.Template.Name;
        stringBuilder.Append("5、" + mail.Annex5Name + "x" + (object) itemInfo.Count + ";");
        itemInfoList.Add(itemInfo);
      }
      mail.AnnexRemark = stringBuilder.ToString();
      if (pb.SendMail(mail))
        return true;
      foreach (SqlDataProvider.Data.ItemInfo itemInfo in itemInfoList)
        this.AddItem(itemInfo);
      return false;
    }

    public bool SendItemToMail(SqlDataProvider.Data.ItemInfo item)
    {
      if (!this.bool_1)
        return false;
      using (PlayerBussiness pb = new PlayerBussiness())
        return this.SendItemToMail(item, pb, (MailInfo) null);
    }

    public bool SendItemToMail(SqlDataProvider.Data.ItemInfo item, PlayerBussiness pb, MailInfo mail)
    {
      if (!this.bool_1 || item.BagType != this.BagType)
        return false;
      if (mail == null)
      {
        mail = new MailInfo();
        mail.Annex1 = item.ItemID.ToString();
        mail.Content = LanguageMgr.GetTranslation("Game.Server.GameUtils.Title");
        mail.Gold = 0;
        mail.IsExist = true;
        mail.Money = 0;
        mail.Receiver = this.m_player.PlayerCharacter.NickName;
        mail.ReceiverID = item.UserID;
        mail.Sender = this.m_player.PlayerCharacter.NickName;
        mail.SenderID = item.UserID;
        mail.Title = LanguageMgr.GetTranslation("Game.Server.GameUtils.Title");
        mail.Type = 9;
      }
      if (!pb.SendMail(mail))
        return false;
      this.RemoveItem(item);
      item.IsExist = true;
      return true;
    }
  }
}
