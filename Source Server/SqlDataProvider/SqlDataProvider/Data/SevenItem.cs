// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.SevenItem
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class SevenItem
  {
    private int _Index;
    private int _PosX;
    private int _Tag;
    private int _Type;

    public SevenItem()
    {
    }

    public SevenItem(int index, int type, int posx, int tag)
    {
      this._Index = index;
      this._Type = type;
      this._PosX = posx;
      this._Tag = tag;
    }

    public int Index
    {
      get
      {
        return this._Index;
      }
      set
      {
        this._Index = value;
      }
    }

    public int PosX
    {
      get
      {
        return this._PosX;
      }
      set
      {
        this._PosX = value;
      }
    }

    public int Tag
    {
      get
      {
        return this._Tag;
      }
      set
      {
        this._Tag = value;
      }
    }

    public int Type
    {
      get
      {
        return this._Type;
      }
      set
      {
        this._Type = value;
      }
    }
  }
}
