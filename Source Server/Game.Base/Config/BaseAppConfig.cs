// Decompiled with JetBrains decompiler
// Type: Game.Base.Config.BaseAppConfig
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using log4net;
using System;
using System.Configuration;
using System.Reflection;

namespace Game.Base.Config
{
  public abstract class BaseAppConfig
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    protected virtual void Load(Type type)
    {
      ConfigurationManager.RefreshSection("appSettings");
      foreach (FieldInfo field in type.GetFields())
      {
        object[] customAttributes = field.GetCustomAttributes(typeof (ConfigPropertyAttribute), false);
        if ((uint) customAttributes.Length > 0U)
        {
          ConfigPropertyAttribute attrib = (ConfigPropertyAttribute) customAttributes[0];
          field.SetValue((object) this, this.LoadConfigProperty(attrib));
        }
      }
    }

    private object LoadConfigProperty(ConfigPropertyAttribute attrib)
    {
      string key = attrib.Key;
      string appSetting = ConfigurationManager.AppSettings[key];
      if (appSetting == null)
      {
        appSetting = attrib.DefaultValue.ToString();
        BaseAppConfig.log.Warn((object) ("Loading " + key + " value is null,using default vaule:" + appSetting));
      }
      else
        BaseAppConfig.log.Debug((object) ("Loading " + key + " Value is " + appSetting));
      try
      {
        return Convert.ChangeType((object) appSetting, attrib.DefaultValue.GetType());
      }
      catch (Exception ex)
      {
        BaseAppConfig.log.Error((object) "Exception in ServerProperties Load: ", ex);
        return (object) null;
      }
    }
  }
}
