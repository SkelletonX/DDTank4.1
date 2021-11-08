using Game.Base.Config;
using System;
using System.IO;
using System.Net;

namespace Game.Base
{
	public class BaseServerConfiguration
	{
		protected ushort _port;

		protected IPAddress _ip;

		public ushort Port
		{
			get
			{
				return _port;
			}
			set
			{
				_port = value;
			}
		}

		public IPAddress Ip
		{
			get
			{
				return _ip;
			}
			set
			{
				_ip = value;
			}
		}

		protected virtual void LoadFromConfig(ConfigElement root)
		{
			string @string = root["Server"]["IP"].GetString("any");
			if (@string == "any")
			{
				_ip = IPAddress.Any;
			}
			else
			{
				_ip = IPAddress.Parse(@string);
			}
			_port = (ushort)root["Server"]["Port"].GetInt(_port);
		}

		public void LoadFromXMLFile(FileInfo configFile)
		{
			XMLConfigFile root = XMLConfigFile.ParseXMLFile(configFile);
			LoadFromConfig(root);
		}

		protected virtual void SaveToConfig(ConfigElement root)
		{
			root["Server"]["Port"].Set(_port);
			root["Server"]["IP"].Set(_ip);
		}

		public void SaveToXMLFile(FileInfo configFile)
		{
			if (configFile == null)
			{
				throw new ArgumentNullException("configFile");
			}
			XMLConfigFile xMLConfigFile = new XMLConfigFile();
			SaveToConfig(xMLConfigFile);
			xMLConfigFile.Save(configFile);
		}

		public BaseServerConfiguration()
		{
			_port = 7000;
			_ip = IPAddress.Any;
		}
	}
}
