using System;

namespace Game.Base.Config
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class ConfigPropertyAttribute : Attribute
	{
		private string m_key;

		private string m_description;

		private object m_defaultValue;

		public string Key => m_key;

		public string Description => m_description;

		public object DefaultValue => m_defaultValue;

		public ConfigPropertyAttribute(string key, string description, object defaultValue)
		{
			m_key = key;
			m_description = description;
			m_defaultValue = defaultValue;
		}
	}
}
