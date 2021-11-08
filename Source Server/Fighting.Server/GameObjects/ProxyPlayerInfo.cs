// Decompiled with JetBrains decompiler
// Type: Fighting.Server.GameObjects.ProxyPlayerInfo
// Assembly: Fighting.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8F1EB855-F1B7-44B3-B212-6508DAF33CC5
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Fight\Fighting.Server.dll

using Bussiness.Managers;
using System;

namespace Fighting.Server.GameObjects
{
  public class ProxyPlayerInfo
  {
    public SqlDataProvider.Data.ItemInfo GetHealstone()
    {
      SqlDataProvider.Data.ItemInfo itemInfo = (SqlDataProvider.Data.ItemInfo) null;
      if (this.Healstone != 0)
      {
        itemInfo = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(this.Healstone), 1, 1);
        itemInfo.Count = this.HealstoneCount;
      }
      return itemInfo;
    }

    public SqlDataProvider.Data.ItemInfo GetItemInfo()
    {
      SqlDataProvider.Data.ItemInfo itemInfo = (SqlDataProvider.Data.ItemInfo) null;
      if (this.SecondWeapon != 0)
      {
        itemInfo = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(this.SecondWeapon), 1, 1);
        itemInfo.StrengthenLevel = this.StrengthLevel;
      }
      return itemInfo;
    }

    public int WeaponStrengthLevel { get; set; }

    public SqlDataProvider.Data.ItemInfo GetItemTemplateInfo()
    {
      SqlDataProvider.Data.ItemInfo itemInfo = (SqlDataProvider.Data.ItemInfo) null;
      if (this.TemplateId != 0)
      {
        itemInfo = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(this.TemplateId), 1, 1);
        itemInfo.StrengthenLevel = this.WeaponStrengthLevel;
      }
      if (this.GoldTemplateId != 0)
      {
        itemInfo.GoldEquip = ItemMgr.FindItemTemplate(this.GoldTemplateId);
        itemInfo.goldBeginTime = this.goldBeginTime;
        itemInfo.goldValidDate = this.goldValidDate;
      }
      return itemInfo;
    }

    public double AntiAddictionRate { get; set; }

    public float AuncherExperienceRate { get; set; }

    public float AuncherOfferRate { get; set; }

    public float AuncherRichesRate { get; set; }

    public double BaseAgility { get; set; }

    public double BaseAttack { get; set; }

    public double BaseBlood { get; set; }

    public double BaseDefence { get; set; }

    public bool CanUserProp { get; set; }

    public bool CanX2Exp { get; set; }

    public bool CanX3Exp { get; set; }

    public string FightFootballStyle { get; set; }

    public float GMExperienceRate { get; set; }

    public float GMOfferRate { get; set; }

    public float GMRichesRate { get; set; }

    public double GPAddPlus { get; set; }

    public int Healstone { get; set; }

    public int HealstoneCount { get; set; }

    public double OfferAddPlus { get; set; }

    public int SecondWeapon { get; set; }

    public int ServerId { get; set; }

    public int StrengthLevel { get; set; }

    public int TemplateId { get; set; }

    public int ZoneId { get; set; }

    public string ZoneName { get; set; }

    public int GoldTemplateId { get; set; }

    public DateTime goldBeginTime { get; set; }

    public int goldValidDate { get; set; }
  }
}
