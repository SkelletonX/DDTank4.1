// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.MissionInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class MissionInfo
  {
    private int m_delay;
    private string m_description;
    private string m_failure;
    private int m_id;
    private int m_incrementDelay;
    private string m_name;
    private int m_param1;
    private int m_param2;
    private int m_param3;
    private int m_param4;
    private string m_script;
    private string m_success;
    private string m_title;
    private int m_totalCount;
    private int m_totalTurn;

    public MissionInfo()
    {
      this.m_param1 = -1;
      this.m_param2 = -1;
      this.m_param3 = -1;
      this.m_param4 = -1;
    }

    public MissionInfo(
      int id,
      string name,
      string key,
      string description,
      int totalCount,
      int totalTurn,
      int initDelay,
      int delay,
      string title,
      int param1,
      int param2)
    {
      this.m_id = id;
      this.m_name = name;
      this.m_description = description;
      this.m_failure = key;
      this.m_totalCount = totalCount;
      this.m_totalTurn = totalTurn;
      this.m_incrementDelay = initDelay;
      this.m_delay = delay;
      this.m_title = title;
      this.m_param1 = param1;
      this.m_param2 = param2;
      this.m_param3 = -1;
      this.m_param4 = -1;
    }

    public int Delay
    {
      get
      {
        return this.m_delay;
      }
      set
      {
        this.m_delay = value;
      }
    }

    public string Description
    {
      get
      {
        return this.m_description;
      }
      set
      {
        this.m_description = value;
      }
    }

    public string Failure
    {
      get
      {
        return this.m_failure;
      }
      set
      {
        this.m_failure = value;
      }
    }

    public int Id
    {
      get
      {
        return this.m_id;
      }
      set
      {
        this.m_id = value;
      }
    }

    public int IncrementDelay
    {
      get
      {
        return this.m_incrementDelay;
      }
      set
      {
        this.m_incrementDelay = value;
      }
    }

    public string Name
    {
      get
      {
        return this.m_name;
      }
      set
      {
        this.m_name = value;
      }
    }

    public int Param1
    {
      get
      {
        return this.m_param1;
      }
      set
      {
        this.m_param1 = value;
      }
    }

    public int Param2
    {
      get
      {
        return this.m_param2;
      }
      set
      {
        this.m_param2 = value;
      }
    }

    public int Param3
    {
      get
      {
        return this.m_param3;
      }
      set
      {
        this.m_param3 = value;
      }
    }

    public int Param4
    {
      get
      {
        return this.m_param4;
      }
      set
      {
        this.m_param4 = value;
      }
    }

    public string Script
    {
      get
      {
        return this.m_script;
      }
      set
      {
        this.m_script = value;
      }
    }

    public string Success
    {
      get
      {
        return this.m_success;
      }
      set
      {
        this.m_success = value;
      }
    }

    public string Title
    {
      get
      {
        return this.m_title;
      }
      set
      {
        this.m_title = value;
      }
    }

    public int TotalCount
    {
      get
      {
        return this.m_totalCount;
      }
      set
      {
        this.m_totalCount = value;
      }
    }

    public int TotalTurn
    {
      get
      {
        return this.m_totalTurn;
      }
      set
      {
        this.m_totalTurn = value;
      }
    }
  }
}
