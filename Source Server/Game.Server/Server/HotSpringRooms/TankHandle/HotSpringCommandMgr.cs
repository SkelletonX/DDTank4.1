// Decompiled with JetBrains decompiler
// Type: Game.Server.HotSpringRooms.TankHandle.HotSpringCommandMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.HotSpringRooms.TankHandle
{
  public class HotSpringCommandMgr
  {
    private Dictionary<int, IHotSpringCommandHandler> dictionary_0 = new Dictionary<int, IHotSpringCommandHandler>();

    public HotSpringCommandMgr()
    {
      this.dictionary_0.Clear();
      this.SearchCommandHandlers(Assembly.GetAssembly(typeof (GameServer)));
    }

    public IHotSpringCommandHandler LoadCommandHandler(int code)
    {
      return this.dictionary_0[code];
    }

    protected void RegisterCommandHandler(int code, IHotSpringCommandHandler handle)
    {
      this.dictionary_0.Add(code, handle);
    }

    protected int SearchCommandHandlers(Assembly assembly)
    {
      int num = 0;
      foreach (Type type in assembly.GetTypes())
      {
        if (type.IsClass && type.GetInterface("Game.Server.HotSpringRooms.TankHandle.IHotSpringCommandHandler") != (Type) null)
        {
          HotSpringCommandAttbute[] customAttributes = (HotSpringCommandAttbute[]) type.GetCustomAttributes(typeof (HotSpringCommandAttbute), true);
          if ((uint) customAttributes.Length > 0U)
          {
            ++num;
            this.RegisterCommandHandler((int) customAttributes[0].Code, Activator.CreateInstance(type) as IHotSpringCommandHandler);
          }
        }
      }
      return num;
    }
  }
}
