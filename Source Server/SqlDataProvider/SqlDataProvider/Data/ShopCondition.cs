// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ShopCondition
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class ShopCondition
  {
    public static bool isDDTMoney(int type)
    {
      return type == 2;
    }

    public static bool isFree(int type)
    {
        return type == 20;
    }

    public static bool isLabyrinth(int type)
    {
      return type == 94;
    }

    public static bool isLeague(int type)
    {
      return type == 93;
    }

    public static bool isMoney(int type)
    {
      return type == 1;
    }

    public static bool isOffer(int type)
    {
      return (uint) (type - 11) <= 4U;
    }

    public static bool isGiftToken(int type)
    {
      return type == 3;
    }
    public static bool isMedal(int type)
    {
      return type == 4;
    }
    public static bool isPetScrore(int type)
    {
      return type == 92;
    }

    public static bool isSearchGoods(int type)
    {
      return type == 99;
    }

    public static bool isWorldBoss(int type)
    {
      return type == 91;
    }
  }
}
