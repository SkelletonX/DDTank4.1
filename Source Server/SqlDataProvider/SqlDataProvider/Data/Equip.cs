// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.Equip
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class Equip
  {
    public static bool isAvatar(ItemTemplateInfo info)
    {
      switch (info.TemplateID)
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

    public static bool isDress(ItemTemplateInfo info)
    {
      return false;
    }

    public static bool isMagicStone(ItemTemplateInfo info)
    {
      bool flag = false;
      if (info.CategoryID == 61)
        flag = true;
      return flag;
    }

    public static bool isShowImp(ItemTemplateInfo info)
    {
      switch (info.CategoryID)
      {
        case 1:
          return true;
        case 5:
        case 7:
          return true;
        default:
          return false;
      }
    }

    public static bool isWeddingRing(ItemTemplateInfo info)
    {
      switch (info.TemplateID)
      {
        case 9022:
        case 9122:
        case 9222:
        case 9322:
        case 9422:
        case 9522:
        case 9622:
        case 9722:
        case 9822:
        case 9922:
          return true;
        default:
          return false;
      }
    }
  }
}
