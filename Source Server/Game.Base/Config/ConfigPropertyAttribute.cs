// Decompiled with JetBrains decompiler
// Type: Game.Base.Config.ConfigPropertyAttribute
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using System;

namespace Game.Base.Config
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class ConfigPropertyAttribute : Attribute
  {
    private object m_defaultValue;
    private string m_description;
    private string m_key;

    public ConfigPropertyAttribute(string key, string description, object defaultValue)
    {
      this.m_key = key;
      this.m_description = description;
      this.m_defaultValue = defaultValue;
    }

    public object DefaultValue
    {
      get
      {
        return this.m_defaultValue;
      }
    }

    public string Description
    {
      get
      {
        return this.m_description;
      }
    }

    public string Key
    {
      get
      {
        return this.m_key;
      }
    }
  }
}
