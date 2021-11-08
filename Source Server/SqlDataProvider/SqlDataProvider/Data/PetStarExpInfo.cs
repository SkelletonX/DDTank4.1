// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.PetStarExpInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class PetStarExpInfo
  {
    private int _Exp;
    private int _NewID;
    private int _OldID;

    public int Exp
    {
      get
      {
        return this._Exp;
      }
      set
      {
        this._Exp = value;
      }
    }

    public int NewID
    {
      get
      {
        return this._NewID;
      }
      set
      {
        this._NewID = value;
      }
    }

    public int OldID
    {
      get
      {
        return this._OldID;
      }
      set
      {
        this._OldID = value;
      }
    }
  }
}
