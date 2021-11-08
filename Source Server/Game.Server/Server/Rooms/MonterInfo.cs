// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.MonterInfo
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System.Drawing;

namespace Game.Server.Rooms
{
  public class MonterInfo
  {
    public int ID { get; set; }

    public Point MonsterNewPos { get; set; }

    public Point MonsterPos { get; set; }

    public int PlayerID { get; set; }

    public int state { get; set; }

    public int type { get; set; }
  }
}
