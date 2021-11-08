using log4net;
using System;
using System.Configuration;
using System.Reflection;

namespace Game.Base.Config
{
	public abstract class BaseAppConfig
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public BaseAppConfig()
		{
		}

		protected virtual void Load(Type type)
		{
			ConfigurationManager.RefreshSection("appSettings");
			FieldInfo[] fields = type.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(ConfigPropertyAttribute), inherit: false);
				if (customAttributes.Length != 0)
				{
					ConfigPropertyAttribute attrib = (ConfigPropertyAttribute)customAttributes[0];
					fieldInfo.SetValue(this, LoadConfigProperty(attrib));
				}
			}
		}

		private object LoadConfigProperty(ConfigPropertyAttribute attrib)
		{
			string key = attrib.Key;
			string text = ConfigurationManager.AppSettings[key];
			if (text == null)
			{
				text = attrib.DefaultValue.ToString();
				log.Warn("Loading " + key + " value is null,using default vaule:" + text);
			}
			else
			{
				log.Debug("Loading " + key + " Value is " + text);
			}
			try
			{
				return Convert.ChangeType(text, attrib.DefaultValue.GetType());
			}
			catch (Exception exception)
			{
				log.Error("Exception in ServerProperties Load: ", exception);
				return null;
			}
		}
	}
}
