// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.AuctionInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class AuctionInfo
  {
    public int AuctioneerID { get; set; }

    public string AuctioneerName { get; set; }

    public int AuctionID { get; set; }

    public DateTime BeginDate { get; set; }

    public int BuyerID { get; set; }

    public string BuyerName { get; set; }

    public int Category { get; set; }

    public int goodsCount { get; set; }

    public bool IsExist { get; set; }

    public int ItemID { get; set; }

    public int Mouthful { get; set; }

    public string Name { get; set; }

    public int PayType { get; set; }

    public int Price { get; set; }

    public int Random { get; set; }

    public int Rise { get; set; }

    public int TemplateID { get; set; }

    public int ValidDate { get; set; }
  }
}
