// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.EliteGameRoundInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class EliteGameRoundInfo
  {
    private int int_0;
    private int int_1;
    private PlayerEliteGameInfo playerEliteGameInfo_0;
    private PlayerEliteGameInfo playerEliteGameInfo_1;
    private PlayerEliteGameInfo playerEliteGameInfo_2;

    public int RoundID
    {
      get
      {
        return this.int_0;
      }
      set
      {
        this.int_0 = value;
      }
    }

    public int RoundType
    {
      get
      {
        return this.int_1;
      }
      set
      {
        this.int_1 = value;
      }
    }

    public PlayerEliteGameInfo PlayerOne
    {
      get
      {
        return this.playerEliteGameInfo_0;
      }
      set
      {
        this.playerEliteGameInfo_0 = value;
      }
    }

    public PlayerEliteGameInfo PlayerTwo
    {
      get
      {
        return this.playerEliteGameInfo_1;
      }
      set
      {
        this.playerEliteGameInfo_1 = value;
      }
    }

    public PlayerEliteGameInfo PlayerWin
    {
      get
      {
        return this.playerEliteGameInfo_2;
      }
      set
      {
        this.playerEliteGameInfo_2 = value;
      }
    }
  }
}
