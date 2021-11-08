// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.MarryProp
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class MarryProp
  {
    private bool _isCreatedMarryRoom;
    private bool _isGotRing;
    private bool _isMarried;
    private int _selfMarryRoomID;
    private int _spouseID;
    private string _spouseName;

    public bool IsCreatedMarryRoom
    {
      get
      {
        return this._isCreatedMarryRoom;
      }
      set
      {
        this._isCreatedMarryRoom = value;
      }
    }

    public bool IsGotRing
    {
      get
      {
        return this._isGotRing;
      }
      set
      {
        this._isGotRing = value;
      }
    }

    public bool IsMarried
    {
      get
      {
        return this._isMarried;
      }
      set
      {
        this._isMarried = value;
      }
    }

    public int SelfMarryRoomID
    {
      get
      {
        return this._selfMarryRoomID;
      }
      set
      {
        this._selfMarryRoomID = value;
      }
    }

    public int SpouseID
    {
      get
      {
        return this._spouseID;
      }
      set
      {
        this._spouseID = value;
      }
    }

    public string SpouseName
    {
      get
      {
        return this._spouseName;
      }
      set
      {
        this._spouseName = value;
      }
    }
  }
}
