// Decompiled with JetBrains decompiler
// Type: Bussiness.StaticFunction
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using log4net;
using System;
using System.Configuration;
using System.Reflection;

namespace Bussiness
{
  public class StaticFunction
  {
    protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static bool UpdateConfig(string fileName, string name, string value)
    {
      try
      {
        System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap()
        {
          ExeConfigFilename = fileName
        }, ConfigurationUserLevel.None);
        configuration.AppSettings.Settings[name].Value = value;
        configuration.Save();
        ConfigurationManager.RefreshSection("appSettings");
        return true;
      }
      catch (Exception ex)
      {
        if (StaticFunction.log.IsErrorEnabled)
          StaticFunction.log.Error((object) nameof (UpdateConfig), ex);
      }
      return false;
    }
  }
}
