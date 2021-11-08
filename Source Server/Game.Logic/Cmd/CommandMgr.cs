// Decompiled with JetBrains decompiler
// Type: Game.Logic.Cmd.CommandMgr
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Events;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Logic.Cmd
{
  public class CommandMgr
  {
    private static Dictionary<int, ICommandHandler> handles = new Dictionary<int, ICommandHandler>();

    public static ICommandHandler LoadCommandHandler(int code)
    {
      return CommandMgr.handles[code];
    }

    [ScriptLoadedEvent]
    public static void OnScriptCompiled(RoadEvent ev, object sender, EventArgs args)
    {
      CommandMgr.handles.Clear();
      CommandMgr.SearchCommandHandlers(Assembly.GetAssembly(typeof (BaseGame)));
    }

    protected static void RegisterCommandHandler(int code, ICommandHandler handle)
    {
      CommandMgr.handles.Add(code, handle);
    }

    protected static int SearchCommandHandlers(Assembly assembly)
    {
      int num = 0;
      foreach (Type type in assembly.GetTypes())
      {
        if (type.IsClass && type.GetInterface("Game.Logic.Cmd.ICommandHandler") != (Type) null)
        {
          GameCommandAttribute[] customAttributes = (GameCommandAttribute[]) type.GetCustomAttributes(typeof (GameCommandAttribute), true);
          if ((uint) customAttributes.Length > 0U)
          {
            ++num;
            CommandMgr.RegisterCommandHandler(customAttributes[0].Code, Activator.CreateInstance(type) as ICommandHandler);
          }
        }
      }
      return num;
    }
  }
}
