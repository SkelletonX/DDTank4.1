// Decompiled with JetBrains decompiler
// Type: Game.Service.actions.ServiceRun
// Assembly: Road.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 39736CC6-5447-4D8B-81FB-29999A4887F8
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Road.Service.exe

using System.Collections;
using System.ServiceProcess;

namespace Game.Service.actions
{
  public class ServiceRun : IAction
  {
    public string Name
    {
      get
      {
        return "--SERVICERUN";
      }
    }

    public string Syntax
    {
      get
      {
        return (string) null;
      }
    }

    public string Description
    {
      get
      {
        return (string) null;
      }
    }

    public void OnAction(Hashtable parameters)
    {
      ServiceBase.Run((ServiceBase) new GameServerService());
    }
  }
}
