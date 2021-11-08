using System.Collections;

namespace Game.Base.Config
{
	public class ConfigElement
	{
		protected ConfigElement m_parent;

		protected Hashtable m_children = new Hashtable();

		protected string m_value;

		public ConfigElement this[string key]
		{
			get
			{
				lock (m_children)
				{
					if (!m_children.Contains(key))
					{
						m_children.Add(key, GetNewConfigElement(this));
					}
				}
				return (ConfigElement)m_children[key];
			}
			set
			{
				lock (m_children)
				{
					m_children[key] = value;
				}
			}
		}

		public ConfigElement Parent => m_parent;

		public bool HasChildren => m_children.Count > 0;

		public Hashtable Children => m_children;

		public ConfigElement(ConfigElement parent)
		{
			m_parent = parent;
		}

		protected virtual ConfigElement GetNewConfigElement(ConfigElement parent)
		{
			return new ConfigElement(parent);
		}

		public string GetString()
		{
			return m_value;
		}

		public string GetString(string defaultValue)
		{
			if (m_value == null)
			{
				return defaultValue;
			}
			return m_value;
		}

		public int GetInt()
		{
			return int.Parse(m_value);
		}

		public int GetInt(int defaultValue)
		{
			if (m_value == null)
			{
				return defaultValue;
			}
			return int.Parse(m_value);
		}

		public long GetLong()
		{
			return long.Parse(m_value);
		}

		public long GetLong(long defaultValue)
		{
			if (m_value == null)
			{
				return defaultValue;
			}
			return long.Parse(m_value);
		}

		public bool GetBoolean()
		{
			return bool.Parse(m_value);
		}

		public bool GetBoolean(bool defaultValue)
		{
			if (m_value == null)
			{
				return defaultValue;
			}
			return bool.Parse(m_value);
		}

		public void Set(object value)
		{
			m_value = value.ToString();
		}
	}
}
