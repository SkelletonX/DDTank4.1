// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.RuneTemplateInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class RuneTemplateInfo
  {
    public bool IsAttack()
    {
      switch (this.Type1 == 37 ? this.Type2 : this.Type1)
      {
        case 1:
        case 4:
        case 5:
        case 8:
        case 9:
        case 11:
        case 12:
        case 14:
        case 16:
        case 17:
        case 18:
        case 21:
        case 22:
        case 23:
        case 24:
        case 25:
          return true;
        default:
          return false;
      }
    }

    public bool IsDefend()
    {
      switch (this.Type1 == 39 ? this.Type2 : this.Type1)
      {
        case 2:
        case 6:
        case 10:
        case 13:
        case 15:
        case 19:
        case 26:
          return true;
        case 14:
          return false;
        default:
          return false;
      }
    }

    public bool IsProp()
    {
      return (uint) (this.Type1 - 31) <= 5U;
    }

    public string Attribute1 { get; set; }

    public string Attribute2 { get; set; }

    public string Attribute3 { get; set; }

    public int BaseLevel { get; set; }

    public int MaxLevel { get; set; }

    public string Name { get; set; }

    public int NextTemplateID { get; set; }

    public int Rate1 { get; set; }

    public int Rate2 { get; set; }

    public int Rate3 { get; set; }

    public int TemplateID { get; set; }

    public int Turn1 { get; set; }

    public int Turn2 { get; set; }

    public int Turn3 { get; set; }

    public int Type1 { get; set; }

    public int Type2 { get; set; }

    public int Type3 { get; set; }
  }
}
