// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.TankHandle.MarryCommandMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.SceneMarryRooms.TankHandle
{
  public class MarryCommandMgr
  {
    private Dictionary<int, IMarryCommandHandler> handles = new Dictionary<int, IMarryCommandHandler>();

    public MarryCommandMgr()
    {
      this.handles.Clear();
      this.SearchCommandHandlers(Assembly.GetAssembly(typeof (GameServer)));
    }

    public IMarryCommandHandler LoadCommandHandler(int code)
    {
      return this.handles[code];
    }

    protected void RegisterCommandHandler(int code, IMarryCommandHandler handle)
    {
      this.handles.Add(code, handle);
    }

    protected int SearchCommandHandlers(Assembly assembly)
    {
      int num = 0;
      foreach (Type type in assembly.GetTypes())
      {
        if (type.IsClass && type.GetInterface("Game.Server.SceneMarryRooms.TankHandle.IMarryCommandHandler") != (Type) null)
        {
          MarryCommandAttbute[] customAttributes = (MarryCommandAttbute[]) type.GetCustomAttributes(typeof (MarryCommandAttbute), true);
          if ((uint) customAttributes.Length > 0U)
          {
            ++num;
            this.RegisterCommandHandler((int) customAttributes[0].Code, Activator.CreateInstance(type) as IMarryCommandHandler);
          }
        }
      }
      return num;
    }
  }
}
