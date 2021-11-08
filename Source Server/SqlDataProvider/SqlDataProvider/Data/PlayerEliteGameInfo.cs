// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.PlayerEliteGameInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class PlayerEliteGameInfo
  {
    private string yfgFrdHamEK;

    public int UserID { get; set; }

    public string NickName
    {
      get
      {
        return this.yfgFrdHamEK;
      }
      set
      {
        this.yfgFrdHamEK = value;
      }
    }

    public int GameType { get; set; }

    public int Rank { get; set; }

    public int CurrentPoint { get; set; }

    public int Status { get; set; }

    public int Winer { get; set; }

    public bool ReadyStatus { get; set; }
  }
}
