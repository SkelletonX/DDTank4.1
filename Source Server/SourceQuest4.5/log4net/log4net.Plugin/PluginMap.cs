using log4net.Repository;
using System;
using System.Collections;

namespace log4net.Plugin
{
	/// <summary>
	/// Map of repository plugins.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class is a name keyed map of the plugins that are
	/// attached to a repository.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public sealed class PluginMap
	{
		private readonly Hashtable m_mapName2Plugin = new Hashtable();

		private readonly ILoggerRepository m_repository;

		/// <summary>
		/// Gets a <see cref="T:log4net.Plugin.IPlugin" /> by name.
		/// </summary>
		/// <param name="name">The name of the <see cref="T:log4net.Plugin.IPlugin" /> to lookup.</param>
		/// <returns>
		/// The <see cref="T:log4net.Plugin.IPlugin" /> from the map with the name specified, or 
		/// <c>null</c> if no plugin is found.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Lookup a plugin by name. If the plugin is not found <c>null</c>
		/// will be returned.
		/// </para>
		/// </remarks>
		public IPlugin this[string name]
		{
			get
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				lock (this)
				{
					return (IPlugin)m_mapName2Plugin[name];
				}
			}
		}

		/// <summary>
		/// Gets all possible plugins as a list of <see cref="T:log4net.Plugin.IPlugin" /> objects.
		/// </summary>
		/// <value>All possible plugins as a list of <see cref="T:log4net.Plugin.IPlugin" /> objects.</value>
		/// <remarks>
		/// <para>
		/// Get a collection of all the plugins defined in this map.
		/// </para>
		/// </remarks>
		public PluginCollection AllPlugins
		{
			get
			{
				lock (this)
				{
					return new PluginCollection(m_mapName2Plugin.Values);
				}
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="repository">The repository that the plugins should be attached to.</param>
		/// <remarks>
		/// <para>
		/// Initialize a new instance of the <see cref="T:log4net.Plugin.PluginMap" /> class with a 
		/// repository that the plugins should be attached to.
		/// </para>
		/// </remarks>
		public PluginMap(ILoggerRepository repository)
		{
			m_repository = repository;
		}

		/// <summary>
		/// Adds a <see cref="T:log4net.Plugin.IPlugin" /> to the map.
		/// </summary>
		/// <param name="plugin">The <see cref="T:log4net.Plugin.IPlugin" /> to add to the map.</param>
		/// <remarks>
		/// <para>
		/// The <see cref="T:log4net.Plugin.IPlugin" /> will be attached to the repository when added.
		/// </para>
		/// <para>
		/// If there already exists a plugin with the same name 
		/// attached to the repository then the old plugin will
		/// be <see cref="M:log4net.Plugin.IPlugin.Shutdown" /> and replaced with
		/// the new plugin.
		/// </para>
		/// </remarks>
		public void Add(IPlugin plugin)
		{
			if (plugin == null)
			{
				throw new ArgumentNullException("plugin");
			}
			IPlugin plugin2 = null;
			lock (this)
			{
				plugin2 = (m_mapName2Plugin[plugin.Name] as IPlugin);
				m_mapName2Plugin[plugin.Name] = plugin;
			}
			plugin2?.Shutdown();
			plugin.Attach(m_repository);
		}

		/// <summary>
		/// Removes a <see cref="T:log4net.Plugin.IPlugin" /> from the map.
		/// </summary>
		/// <param name="plugin">The <see cref="T:log4net.Plugin.IPlugin" /> to remove from the map.</param>
		/// <remarks>
		/// <para>
		/// Remove a specific plugin from this map.
		/// </para>
		/// </remarks>
		public void Remove(IPlugin plugin)
		{
			if (plugin == null)
			{
				throw new ArgumentNullException("plugin");
			}
			lock (this)
			{
				m_mapName2Plugin.Remove(plugin.Name);
			}
		}
	}
}
