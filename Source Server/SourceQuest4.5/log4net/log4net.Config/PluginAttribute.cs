using log4net.Core;
using log4net.Plugin;
using log4net.Util;
using System;

namespace log4net.Config
{
	/// <summary>
	/// Assembly level attribute that specifies a plugin to attach to 
	/// the repository.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Specifies the type of a plugin to create and attach to the
	/// assembly's repository. The plugin type must implement the
	/// <see cref="T:log4net.Plugin.IPlugin" /> interface.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	[Serializable]
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class PluginAttribute : Attribute, IPluginFactory
	{
		private string m_typeName = null;

		private Type m_type = null;

		/// <summary>
		/// Gets or sets the type for the plugin.
		/// </summary>
		/// <value>
		/// The type for the plugin.
		/// </value>
		/// <remarks>
		/// <para>
		/// The type for the plugin.
		/// </para>
		/// </remarks>
		public Type Type
		{
			get
			{
				return m_type;
			}
			set
			{
				m_type = value;
			}
		}

		/// <summary>
		/// Gets or sets the type name for the plugin.
		/// </summary>
		/// <value>
		/// The type name for the plugin.
		/// </value>
		/// <remarks>
		/// <para>
		/// The type name for the plugin.
		/// </para>
		/// <para>
		/// Where possible use the <see cref="P:log4net.Config.PluginAttribute.Type" /> property instead.
		/// </para>
		/// </remarks>
		public string TypeName
		{
			get
			{
				return m_typeName;
			}
			set
			{
				m_typeName = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Config.PluginAttribute" /> class
		/// with the specified type.
		/// </summary>
		/// <param name="typeName">The type name of plugin to create.</param>
		/// <remarks>
		/// <para>
		/// Create the attribute with the plugin type specified.
		/// </para>
		/// <para>
		/// Where possible use the constructor that takes a <see cref="T:System.Type" />.
		/// </para>
		/// </remarks>
		public PluginAttribute(string typeName)
		{
			m_typeName = typeName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Config.PluginAttribute" /> class
		/// with the specified type.
		/// </summary>
		/// <param name="type">The type of plugin to create.</param>
		/// <remarks>
		/// <para>
		/// Create the attribute with the plugin type specified.
		/// </para>
		/// </remarks>
		public PluginAttribute(Type type)
		{
			m_type = type;
		}

		/// <summary>
		/// Creates the plugin object defined by this attribute.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Creates the instance of the <see cref="T:log4net.Plugin.IPlugin" /> object as 
		/// specified by this attribute.
		/// </para>
		/// </remarks>
		/// <returns>The plugin object.</returns>
		public IPlugin CreatePlugin()
		{
			Type type = m_type;
			if ((object)m_type == null)
			{
				type = SystemInfo.GetTypeFromString(m_typeName, throwOnError: true, ignoreCase: true);
			}
			if (!typeof(IPlugin).IsAssignableFrom(type))
			{
				throw new LogException("Plugin type [" + type.FullName + "] does not implement the log4net.IPlugin interface");
			}
			return (IPlugin)Activator.CreateInstance(type);
		}

		/// <summary>
		/// Returns a representation of the properties of this object.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Overrides base class <see cref="M:System.Object.ToString" /> method to 
		/// return a representation of the properties of this object.
		/// </para>
		/// </remarks>
		/// <returns>A representation of the properties of this object</returns>
		public override string ToString()
		{
			if ((object)m_type != null)
			{
				return "PluginAttribute[Type=" + m_type.FullName + "]";
			}
			return "PluginAttribute[Type=" + m_typeName + "]";
		}
	}
}
