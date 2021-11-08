// Decompiled with JetBrains decompiler
// Type: Game.Base.BaseServerConfiguration
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using Game.Base.Config;
using System;
using System.IO;
using System.Net;

namespace Game.Base
{
  public class BaseServerConfiguration
  {
    protected IPAddress _ip = IPAddress.Any;
    protected ushort _port = 7000;

    protected virtual void LoadFromConfig(ConfigElement root)
    {
      string ipString = root["Server"]["IP"].GetString("any");
      this._ip = !(ipString == "any") ? IPAddress.Parse(ipString) : IPAddress.Any;
      this._port = (ushort) root["Server"]["Port"].GetInt((int) this._port);
    }

    public void LoadFromXMLFile(FileInfo configFile)
    {
      this.LoadFromConfig((ConfigElement) XMLConfigFile.ParseXMLFile(configFile));
    }

    protected virtual void SaveToConfig(ConfigElement root)
    {
      root["Server"]["Port"].Set((object) this._port);
      root["Server"]["IP"].Set((object) this._ip);
    }

    public void SaveToXMLFile(FileInfo configFile)
    {
      if (configFile == null)
        throw new ArgumentNullException(nameof (configFile));
      XMLConfigFile xmlConfigFile = new XMLConfigFile();
      this.SaveToConfig((ConfigElement) xmlConfigFile);
      xmlConfigFile.Save(configFile);
    }

    public IPAddress Ip
    {
      get
      {
        return this._ip;
      }
      set
      {
        this._ip = value;
      }
    }

    public ushort Port
    {
      get
      {
        return this._port;
      }
      set
      {
        this._port = value;
      }
    }
  }
}
