namespace log4net.Plugin
{
	/// <summary>
	/// Interface used to create plugins.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Interface used to create  a plugin.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public interface IPluginFactory
	{
		/// <summary>
		/// Creates the plugin object.
		/// </summary>
		/// <returns>the new plugin instance</returns>
		/// <remarks>
		/// <para>
		/// Create and return a new plugin instance.
		/// </para>
		/// </remarks>
		IPlugin CreatePlugin();
	}
}
