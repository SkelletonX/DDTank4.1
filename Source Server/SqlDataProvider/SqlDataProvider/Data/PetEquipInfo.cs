// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.PetEquipInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class PetEquipInfo
  {
    private ItemTemplateInfo itemTemplateInfo_0;

    public PetEquipInfo(ItemTemplateInfo temp)
    {
      this.itemTemplateInfo_0 = temp;
    }

    public ItemTemplateInfo Template
    {
      get
      {
        return this.itemTemplateInfo_0;
      }
    }

    public int eqType { set; get; }

    public int eqTemplateID { set; get; }

    public DateTime startTime { set; get; }

    public int ValidDate { set; get; }

    public bool IsValidItem()
    {
      if ((uint) this.ValidDate > 0U)
        return DateTime.Compare(this.startTime.AddDays((double) this.ValidDate), DateTime.Now) > 0;
      return true;
    }
  }
}
