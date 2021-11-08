// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.CommConfigInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class CommConfigInfo
  {
    private int _ID;
    private DateTime _ParaDate1;
    private DateTime _ParaDate2;
    private DateTime _ParaDate3;
    private int _ParaNumber1;
    private int _ParaNumber2;
    private int _ParaNumber3;
    private string _ParaString1;
    private string _ParaString2;
    private string _ParaString3;

    public CommConfigInfo()
    {
    }

    public CommConfigInfo(
      int ID,
      string ParaString1,
      string ParaString2,
      string ParaString3,
      DateTime ParaDate1,
      DateTime ParaDate2,
      DateTime ParaDate3,
      int ParaNumber1,
      int ParaNumber2,
      int ParaNumber3)
    {
      this._ID = ID;
      this._ParaString1 = ParaString1;
      this._ParaString2 = ParaString2;
      this._ParaString3 = ParaString3;
      this._ParaDate1 = ParaDate1;
      this._ParaDate2 = ParaDate2;
      this._ParaDate3 = ParaDate3;
      this._ParaNumber1 = ParaNumber1;
      this._ParaNumber2 = ParaNumber2;
      this._ParaNumber3 = ParaNumber3;
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
      }
    }

    public DateTime ParaDate1
    {
      get
      {
        return this._ParaDate1;
      }
      set
      {
        this._ParaDate1 = value;
      }
    }

    public DateTime ParaDate2
    {
      get
      {
        return this._ParaDate2;
      }
      set
      {
        this._ParaDate2 = value;
      }
    }

    public DateTime ParaDate3
    {
      get
      {
        return this._ParaDate3;
      }
      set
      {
        this._ParaDate3 = value;
      }
    }

    public int ParaNumber1
    {
      get
      {
        return this._ParaNumber1;
      }
      set
      {
        this._ParaNumber1 = value;
      }
    }

    public int ParaNumber2
    {
      get
      {
        return this._ParaNumber2;
      }
      set
      {
        this._ParaNumber2 = value;
      }
    }

    public int ParaNumber3
    {
      get
      {
        return this._ParaNumber3;
      }
      set
      {
        this._ParaNumber3 = value;
      }
    }

    public string ParaString1
    {
      get
      {
        return this._ParaString1;
      }
      set
      {
        this._ParaString1 = value;
      }
    }

    public string ParaString2
    {
      get
      {
        return this._ParaString2;
      }
      set
      {
        this._ParaString2 = value;
      }
    }

    public string ParaString3
    {
      get
      {
        return this._ParaString3;
      }
      set
      {
        this._ParaString3 = value;
      }
    }
  }
}
