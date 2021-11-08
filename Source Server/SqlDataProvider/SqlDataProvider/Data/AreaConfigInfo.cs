// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.AreaConfigInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class AreaConfigInfo
  {
    public int AreaID { get; set; }

    public string AreaServer { get; set; }

    public string AreaName { get; set; }

    public string DataSource { get; set; }

    public string Catalog { get; set; }

    public string UserID { get; set; }

    public string Password { get; set; }

    public string RequestUrl { get; set; }

    public bool CrossChatAllow { get; set; }

    public bool CrossPrivateChat { get; set; }

    public string Version { get; set; }
  }
}
