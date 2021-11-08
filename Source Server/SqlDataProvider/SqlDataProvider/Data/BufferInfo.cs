// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.BufferInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class BufferInfo : DataObject
  {
    private DateTime _beginDate;
    private string _data;
    private bool _isExist;
    private int _templateID;
    private int _type;
    private int _userID;
    private int _validCount;
    private int _validDate;
    private int _value;

    public DateTime GetEndDate()
    {
      return this._beginDate.AddMinutes((double) this._validDate);
    }

    public bool IsValid()
    {
      return this._beginDate.AddMinutes((double) this._validDate) > DateTime.Now;
    }

    public DateTime BeginDate
    {
      get
      {
        return this._beginDate;
      }
      set
      {
        this._beginDate = value;
        this._isDirty = true;
      }
    }

    public string Data
    {
      get
      {
        return this._data;
      }
      set
      {
        this._data = value;
        this._isDirty = true;
      }
    }

    public bool IsExist
    {
      get
      {
        return this._isExist;
      }
      set
      {
        this._isExist = value;
        this._isDirty = true;
      }
    }

    public int TemplateID
    {
      get
      {
        return this._templateID;
      }
      set
      {
        this._templateID = value;
        this._isDirty = true;
      }
    }

    public int Type
    {
      get
      {
        return this._type;
      }
      set
      {
        this._type = value;
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

    public int ValidCount
    {
      get
      {
        return this._validCount;
      }
      set
      {
        this._validCount = value;
        this._isDirty = true;
      }
    }

    public int ValidDate
    {
      get
      {
        return this._validDate;
      }
      set
      {
        this._validDate = value;
        this._isDirty = true;
      }
    }

    public int Value
    {
      get
      {
        return this._value;
      }
      set
      {
        this._value = value;
        this._isDirty = true;
      }
    }
  }
}
