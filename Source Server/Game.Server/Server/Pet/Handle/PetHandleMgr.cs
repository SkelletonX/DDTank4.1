// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.PetHandleMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.Pet.Handle
{
  public class PetHandleMgr
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private Dictionary<int, IPetCommandHadler> dictionary_0;

    public IPetCommandHadler LoadCommandHandler(int code)
    {
      if (this.dictionary_0.ContainsKey(code))
        return this.dictionary_0[code];
      PetHandleMgr.ilog_0.Error((object) ("LoadCommandHandler code024: " + code.ToString()));
      return (IPetCommandHadler) null;
    }

    public PetHandleMgr()
    {
      this.dictionary_0 = new Dictionary<int, IPetCommandHadler>();
      this.dictionary_0.Clear();
      this.SearchCommandHandlers(Assembly.GetAssembly(typeof (GameServer)));
    }

    protected int SearchCommandHandlers(Assembly assembly)
    {
      int num = 0;
      foreach (Type type in assembly.GetTypes())
      {
        if (type.IsClass && !(type.GetInterface("Game.Server.Pet.Handle.IPetCommandHadler") == (Type) null))
        {
          global::Pet[] customAttributes = (global::Pet[]) type.GetCustomAttributes(typeof (global::Pet), true);
          if ((uint) customAttributes.Length > 0U)
          {
            ++num;
            this.RegisterCommandHandler((int) customAttributes[0].method_0(), Activator.CreateInstance(type) as IPetCommandHadler);
          }
        }
      }
      return num;
    }

    protected void RegisterCommandHandler(int code, IPetCommandHadler handle)
    {
      this.dictionary_0.Add(code, handle);
    }
  }
}
