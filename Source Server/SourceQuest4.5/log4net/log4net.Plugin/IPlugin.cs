using log4net.Repository;

namespace log4net.Plugin
{
	/// <summary>
	/// Interface implemented by logger repository plugins.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Plugins define additional behavior that can be associated
	/// with a <see cref="T:log4net.Repository.ILoggerRepository" />.
	/// The <see cref="T:log4net.Plugin.PluginMap" /> held by the <see cref="P:log4net.Repository.ILoggerRepository.PluginMap" />
	/// property is used to store the plugins for a repository.
	/// </para>
	/// <para>
	/// The <c>log4net.Config.PluginAttribute</c> can be used to
	/// attach plugins to repositories created using configuration
	/// attributes.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public interface IPlugin
	{
		/// <summary>
		/// Gets the name of the plugin.
		/// </summary>
		/// <value>
		/// The name of the plugin.
		/// </value>
		/// <remarks>
		/// <para>
		/// Plugins are stored in the <see cref="T:log4net.Plugin.PluginMap" />
		/// keyed by name. Each plugin instance attached to a
		/// repository must be a unique name.
		/// </para>
		/// </remarks>
		string Name
		{
			get;
		}

		/// <summary>
		/// Attaches the plugin to the specified <see cref="T:log4net.Repository.ILoggerRepository" />.
		/// </summary>
		/// <param name="repository">The <see cref="T:log4net.Repository.ILoggerRepository" /> that this plugin should be attached to.</param>
		/// <remarks>
		/// <para>
		/// A plugin may only be attached to a single repository.
		/// </para>
		/// <para>
		/// This method is called when the plugin is attached to the repository.
		/// </para>
		/// </remarks>
		void Attach(ILoggerRepository repository);

		/// <summary>
		/// Is called when the plugin is to shutdown.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method is called to notify the plugin that 
		/// it should stop operating and should detach from
		/// the repository.
		/// </para>
		/// </remarks>
		void Shutdown();
	}
}
