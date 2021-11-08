// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaHandleMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.Consortia.Handle
{
  public class ConsortiaHandleMgr
  {
    private Dictionary<int, IConsortiaCommandHadler> dictionary_0;

    public IConsortiaCommandHadler LoadCommandHandler(int code)
    {
      return this.dictionary_0[code];
    }

    public ConsortiaHandleMgr()
    {
      this.dictionary_0 = new Dictionary<int, IConsortiaCommandHadler>();
      this.dictionary_0.Clear();
      this.SearchCommandHandlers(Assembly.GetAssembly(typeof (GameServer)));
    }

    protected int SearchCommandHandlers(Assembly assembly)
    {
      int num = 0;
      foreach (Type type in assembly.GetTypes())
      {
        if (type.IsClass && !(type.GetInterface("Game.Server.Consortia.Handle.IConsortiaCommandHadler") == (Type) null))
        {
          global::Consortia[] customAttributes = (global::Consortia[]) type.GetCustomAttributes(typeof (global::Consortia), true);
          if ((uint) customAttributes.Length > 0U)
          {
            ++num;
            this.RegisterCommandHandler((int) customAttributes[0].method_0(), Activator.CreateInstance(type) as IConsortiaCommandHadler);
          }
        }
      }
      return num;
    }

    protected void RegisterCommandHandler(int code, IConsortiaCommandHadler handle)
    {
      this.dictionary_0.Add(code, handle);
    }
  }
}
