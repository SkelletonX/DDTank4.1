// Decompiled with JetBrains decompiler
// Type: Center.Server.CenterServerConfig
// Assembly: Center.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4391F35A-9CAD-44A9-AFE0-208E0547A0DD
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Center\Center.Server.dll

using Game.Base.Config;
using log4net;
using System;
using System.IO;
using System.Reflection;

namespace Center.Server
{
  public class CenterServerConfig : BaseAppConfig
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    [ConfigProperty("IP", "中心服务器监听IP", "10.0.3.5")]
    public string Ip;
    [ConfigProperty("LogConfigFile", "日志配置文件", "logconfig.xml")]
    public string LogConfigFile;
    [ConfigProperty("LoginLapseInterval", "登陆超时时间,分钟为单位", 1)]
    public int LoginLapseInterval;
    [ConfigProperty("Port", "中心服务器监听端口", 9202)]
    public int Port;
    public string RootDirectory;
    [ConfigProperty("SaveInterval", "数据保存周期,分钟为单位", 1)]
    public int SaveIntervalInterval;
    [ConfigProperty("SaveRecordInterval", "日志保存周期,分钟为单位", 1)]
    public int SaveRecordInterval;
    [ConfigProperty("ScanAuctionInterval", "排名行扫描周期,分钟为单位", 60)]
    public int ScanAuctionInterval;
    [ConfigProperty("ScanConsortiaInterval", "工会扫描周期,以分钟为单位", 60)]
    public int ScanConsortiaInterval;
    [ConfigProperty("ScanMailInterval", "邮件扫描周期,分钟为单位", 60)]
    public int ScanMailInterval;
    [ConfigProperty("ScriptAssemblies", "脚本编译引用库", "")]
    public string ScriptAssemblies;
    [ConfigProperty("ScriptCompilationTarget", "脚本编译目标名称", "")]
    public string ScriptCompilationTarget;
    [ConfigProperty("SystemNoticeInterval", "登陆超时时间,分钟为单位", 2)]
    public int SystemNoticeInterval;

    public CenterServerConfig()
    {
      this.Load(typeof (CenterServerConfig));
    }

    protected override void Load(Type type)
    {
      this.RootDirectory = !(Assembly.GetEntryAssembly() != (Assembly) null) ? new FileInfo(Assembly.GetAssembly(type).Location).DirectoryName : new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName;
      base.Load(type);
    }

    public void Refresh()
    {
      this.Load(typeof (CenterServerConfig));
    }
  }
}
