// Decompiled with JetBrains decompiler
// Type: Game.Service.GameServerServiceInstaller
// Assembly: Road.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 39736CC6-5447-4D8B-81FB-29999A4887F8
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Road.Service.exe

using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Game.Service
{
  [RunInstaller(true)]
  public class GameServerServiceInstaller : Installer
  {
    private ServiceInstaller m_gameServerServiceInstaller;
    private ServiceProcessInstaller m_gameServerServiceProcessInstaller;

    public GameServerServiceInstaller()
    {
      this.m_gameServerServiceProcessInstaller = new ServiceProcessInstaller();
      this.m_gameServerServiceProcessInstaller.Account = ServiceAccount.LocalSystem;
      this.m_gameServerServiceInstaller = new ServiceInstaller();
      this.m_gameServerServiceInstaller.StartType = ServiceStartMode.Manual;
      this.m_gameServerServiceInstaller.ServiceName = "ROAD";
      this.Installers.Add((Installer) this.m_gameServerServiceProcessInstaller);
      this.Installers.Add((Installer) this.m_gameServerServiceInstaller);
    }

    public override void Install(IDictionary stateSaver)
    {
      StringDictionary parameters = this.Context.Parameters;
      parameters["assemblyPath"] = parameters["assemblyPath"] + " --SERVICERUN " + this.Context.Parameters["commandline"];
      base.Install(stateSaver);
    }
  }
}
