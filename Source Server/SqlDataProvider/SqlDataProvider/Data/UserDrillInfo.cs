// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserDrillInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class UserDrillInfo : DataObject
  {
    private int _beadPlace;
    private int _drillPlace;
    private int _holeExp;
    private int _holeLv;
    private int _userID;

    public int BeadPlace
    {
      get
      {
        return this._beadPlace;
      }
      set
      {
        this._beadPlace = value;
        this._isDirty = true;
      }
    }

    public int DrillPlace
    {
      get
      {
        return this._drillPlace;
      }
      set
      {
        this._drillPlace = value;
        this._isDirty = true;
      }
    }

    public int HoleExp
    {
      get
      {
        return this._holeExp;
      }
      set
      {
        this._holeExp = value;
        this._isDirty = true;
      }
    }

    public int HoleLv
    {
      get
      {
        return this._holeLv;
      }
      set
      {
        this._holeLv = value;
        this._isDirty = true;
      }
    }

    public int UserID
    {
      get
      {
        return this._userID;
      }
      set
      {
        this._userID = value;
        this._isDirty = true;
      }
    }
  }
}
