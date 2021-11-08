// Decompiled with JetBrains decompiler
// Type: Fighting.Server.FightServerConfig
// Assembly: Fighting.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8F1EB855-F1B7-44B3-B212-6508DAF33CC5
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Fight\Fighting.Server.dll

using Game.Base.Config;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Fighting.Server
{
  public class FightServerConfig : BaseAppConfig
  {
    [ConfigProperty("IP", "频道的IP", "10.0.3.5")]
    public string LogConfigFile = "logconfig.xml";
    [ConfigProperty("Port", "频道开放端口", 9208)]
    public int Port;
    public string RootDirectory;
    public string ScriptAssemblies;
    public string ScriptCompilationTarget;
    public string ServerName;
    [ConfigProperty("ZoneId", "服务器编号", 4)]
    public int ZoneId;
    public string Ip;

    protected override void Load(Type type)
    {
      this.RootDirectory = !(Assembly.GetEntryAssembly() != (Assembly) null) ? new FileInfo(Assembly.GetAssembly(typeof (FightServer)).Location).DirectoryName : new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName;
      base.Load(type);
    }

    public void LoadConfiguration()
    {
      this.Load(typeof (FightServerConfig));
    }

    public void Load()
    {
      this.LogConfigFile = ConfigurationSettings.AppSettings["Logconfig"];
      this.Ip = ConfigurationSettings.AppSettings["Ip"];
      this.ServerName = ConfigurationSettings.AppSettings["ServerName"];
      this.Port = int.Parse(ConfigurationSettings.AppSettings["Port"]);
      this.ScriptAssemblies = ConfigurationSettings.AppSettings["ScriptAssemblies"];
      this.ScriptCompilationTarget = ConfigurationSettings.AppSettings["ScriptAssemblies"];
      this.ZoneId = int.Parse(ConfigurationSettings.AppSettings["ServerID"]);
      this.RootDirectory = new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName;
    }
  }
}
