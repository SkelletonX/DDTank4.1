// Decompiled with JetBrains decompiler
// Type: Game.Base.Commands.ScriptManagerCommand
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using Game.Server.Managers;
using System;
using System.IO;
using System.Reflection;

namespace Game.Base.Commands
{
  [Cmd("&sm", ePrivLevel.Player, "Script Manager console commands.", new string[] {"   /sm  <option>  [para1][para2]...", "eg: /sm -list              : List all assemblies in scripts array.", "    /sm -add <assembly>    : Add assembly into the scripts array.", "    /sm -remove <assembly> : Remove assembly from the scripts array."})]
  public class ScriptManagerCommand : AbstractCommandHandler, ICommandHandler
  {
    public bool OnCommand(BaseClient client, string[] args)
    {
      if (args.Length > 1)
      {
        string str = args[1];
        if (!(str == "-list"))
        {
          if (!(str == "-add"))
          {
            if (str == "-remove")
            {
              if (args.Length > 2 && args[2] != null)
              {
                if (File.Exists(args[2]))
                {
                  try
                  {
                    if (ScriptMgr.RemoveAssembly(Assembly.LoadFile(args[2])))
                    {
                      this.DisplayMessage(client, "Remove assembly success!");
                      return true;
                    }
                    this.DisplayMessage(client, "Assembly didn't exist in the scripts array!");
                    return false;
                  }
                  catch (Exception ex)
                  {
                    this.DisplayMessage(client, "Remove assembly error:", (object) ex.Message);
                    return false;
                  }
                }
              }
              this.DisplayMessage(client, "Can't find remove assembly!");
              return false;
            }
            this.DisplayMessage(client, "Can't fine option:{0}", (object) args[1]);
            return true;
          }
          if (args.Length > 2 && args[2] != null)
          {
            if (File.Exists(args[2]))
            {
              try
              {
                if (ScriptMgr.InsertAssembly(Assembly.LoadFile(args[2])))
                {
                  this.DisplayMessage(client, "Add assembly success!");
                  return true;
                }
                this.DisplayMessage(client, "Assembly already exists in the scripts array!");
                return false;
              }
              catch (Exception ex)
              {
                this.DisplayMessage(client, "Add assembly error:", (object) ex.Message);
                return false;
              }
            }
          }
          this.DisplayMessage(client, "Can't find add assembly!");
          return false;
        }
        foreach (Assembly script in ScriptMgr.Scripts)
          this.DisplayMessage(client, script.FullName);
        return true;
      }
      this.DisplaySyntax(client);
      return true;
    }
  }
}
