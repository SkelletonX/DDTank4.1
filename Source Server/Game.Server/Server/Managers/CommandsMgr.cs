// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.CommandsMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using System.Collections.Generic;

namespace Game.Server.Managers
{
  public class CommandsMgr
  {
    private static Dictionary<int, List<string>> Commands;

    public static bool CheckAdmin(int UserID, string Command)
    {
      return CommandsMgr.Commands.ContainsKey(UserID);
    }

    public static bool Init()
    {
      CommandsMgr.Commands = new PlayerBussiness().LoadCommands();
      if (CommandsMgr.Commands != null)
        return CommandsMgr.Commands.Count > 0;
      return false;
    }
  }
}
