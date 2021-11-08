// Decompiled with JetBrains decompiler
// Type: Game.Base.Commands.BuildScriptCommand
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using Game.Server.Managers;

namespace Game.Base.Commands
{
  [Cmd("&cs", ePrivLevel.Player, "Compile the C# scripts.", new string[] {"/cs  <source file> <target> <importlib>", "eg: /cs ./scripts temp.dll game.base.dll,game.logic.dll"})]
  public class BuildScriptCommand : AbstractCommandHandler, ICommandHandler
  {
    public bool OnCommand(BaseClient client, string[] args)
    {
      if (args.Length >= 4)
        ScriptMgr.CompileScripts(false, args[1], args[2], args[3].Split(','));
      else
        this.DisplaySyntax(client);
      return true;
    }
  }
}
