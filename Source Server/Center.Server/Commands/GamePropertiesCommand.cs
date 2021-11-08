// Decompiled with JetBrains decompiler
// Type: Center.Server.Commands.GamePropertiesCommand
// Assembly: Center.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4391F35A-9CAD-44A9-AFE0-208E0547A0DD
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Center\Center.Server.dll

using Game.Base;

namespace Center.Server.Commands
{
  [Cmd("&gp", ePrivLevel.Admin, "Manage game properties at runtime.", new string[] {"   /gp <option> [para1][para2] ...", "eg:    /gp -view   :List all game properties.", "       /gp -load   :Load game properties from database.", "       /gp -save   :Save game properties into database."})]
  public class GamePropertiesCommand : AbstractCommandHandler, ICommandHandler
  {
    public bool OnCommand(BaseClient client, string[] args)
    {
      return true;
    }
  }
}
