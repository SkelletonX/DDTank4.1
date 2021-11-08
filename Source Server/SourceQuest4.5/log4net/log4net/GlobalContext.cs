using log4net.Util;

namespace log4net
{
	/// <summary>
	/// The log4net Global Context.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <c>GlobalContext</c> provides a location for global debugging 
	/// information to be stored.
	/// </para>
	/// <para>
	/// The global context has a properties map and these properties can 
	/// be included in the output of log messages. The <see cref="T:log4net.Layout.PatternLayout" />
	/// supports selecting and outputing these properties.
	/// </para>
	/// <para>
	/// By default the <c>log4net:HostName</c> property is set to the name of 
	/// the current machine.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code lang="C#">
	/// GlobalContext.Properties["hostname"] = Environment.MachineName;
	/// </code>
	/// </example>
	/// <threadsafety static="true" instance="true" />
	/// <author>Nicko Cadell</author>
	public sealed class GlobalContext
	{
		/// <summary>
		/// The global context properties instance
		/// </summary>
		private static readonly GlobalContextProperties s_properties;

		/// <summary>
		/// The global properties map.
		/// </summary>
		/// <value>
		/// The global properties map.
		/// </value>
		/// <remarks>
		/// <para>
		/// The global properties map.
		/// </para>
		/// </remarks>
		public static GlobalContextProperties Properties => s_properties;

		/// <summary>
		/// Private Constructor. 
		/// </summary>
		/// <remarks>
		/// Uses a private access modifier to prevent instantiation of this class.
		/// </remarks>
		private GlobalContext()
		{
		}

		static GlobalContext()
		{
			s_properties = new GlobalContextProperties();
			Properties["log4net:HostName"] = SystemInfo.HostName;
		}
	}
}
