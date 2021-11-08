using log4net.Repository;

namespace log4net.Plugin
{
	/// <summary>
	/// Base implementation of <see cref="T:log4net.Plugin.IPlugin" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// Default abstract implementation of the <see cref="T:log4net.Plugin.IPlugin" />
	/// interface. This base class can be used by implementors
	/// of the <see cref="T:log4net.Plugin.IPlugin" /> interface.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public abstract class PluginSkeleton : IPlugin
	{
		/// <summary>
		/// The name of this plugin.
		/// </summary>
		private string m_name;

		/// <summary>
		/// The repository this plugin is attached to.
		/// </summary>
		private ILoggerRepository m_repository;

		/// <summary>
		/// Gets or sets the name of the plugin.
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
		/// <para>
		/// The name of the plugin must not change one the 
		/// plugin has been attached to a repository.
		/// </para>
		/// </remarks>
		public virtual string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		/// <summary>
		/// The repository for this plugin
		/// </summary>
		/// <value>
		/// The <see cref="T:log4net.Repository.ILoggerRepository" /> that this plugin is attached to.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets or sets the <see cref="T:log4net.Repository.ILoggerRepository" /> that this plugin is 
		/// attached to.
		/// </para>
		/// </remarks>
		protected virtual ILoggerRepository LoggerRepository
		{
			get
			{
				return m_repository;
			}
			set
			{
				m_repository = value;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">the name of the plugin</param>
		/// <remarks>
		/// Initializes a new Plugin with the specified name.
		/// </remarks>
		protected PluginSkeleton(string name)
		{
			m_name = name;
		}

		/// <summary>
		/// Attaches this plugin to a <see cref="T:log4net.Repository.ILoggerRepository" />.
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
		public virtual void Attach(ILoggerRepository repository)
		{
			m_repository = repository;
		}

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
		public virtual void Shutdown()
		{
		}
	}
}
