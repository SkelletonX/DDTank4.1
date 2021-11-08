// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.TreasureDataInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class TreasureDataInfo : DataObject
  {
    private DateTime _BeginDate;
    private int _Count;
    private int _ID;
    private bool _IsExit;
    private int _pos;
    private int _TemplateID;
    private int _UserID;
    private int _ValidDate;

    public DateTime BeginDate
    {
      get
      {
        return this._BeginDate;
      }
      set
      {
        this._BeginDate = value;
        this._isDirty = true;
      }
    }

    public int Count
    {
      get
      {
        return this._Count;
      }
      set
      {
        this._Count = value;
        this._isDirty = true;
      }
    }

    public int ID
    {
      get
      {
        return this._ID;
      }
      set
      {
        this._ID = value;
        this._isDirty = true;
      }
    }

    public bool IsExit
    {
      get
      {
        return this._IsExit;
      }
      set
      {
        this._IsExit = value;
        this._isDirty = true;
      }
    }

    public int pos
    {
      get
      {
        return this._pos;
      }
      set
      {
        this._pos = value;
        this._isDirty = true;
      }
    }

    public int TemplateID
    {
      get
      {
        return this._TemplateID;
      }
      set
      {
        this._TemplateID = value;
        this._isDirty = true;
      }
    }

    public int UserID
    {
      get
      {
        return this._UserID;
      }
      set
      {
        this._UserID = value;
        this._isDirty = true;
      }
    }

    public int ValidDate
    {
      get
      {
        return this._ValidDate;
      }
      set
      {
        this._ValidDate = value;
        this._isDirty = true;
      }
    }
  }
}
