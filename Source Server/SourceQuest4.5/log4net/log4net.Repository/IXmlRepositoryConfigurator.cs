using System.Xml;

namespace log4net.Repository
{
	/// <summary>
	/// Configure repository using XML
	/// </summary>
	/// <remarks>
	/// <para>
	/// Interface used by Xml configurator to configure a <see cref="T:log4net.Repository.ILoggerRepository" />.
	/// </para>
	/// <para>
	/// A <see cref="T:log4net.Repository.ILoggerRepository" /> should implement this interface to support
	/// configuration by the <see cref="T:log4net.Config.XmlConfigurator" />.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public interface IXmlRepositoryConfigurator
	{
		/// <summary>
		/// Initialize the repository using the specified config
		/// </summary>
		/// <param name="element">the element containing the root of the config</param>
		/// <remarks>
		/// <para>
		/// The schema for the XML configuration data is defined by
		/// the implementation.
		/// </para>
		/// </remarks>
		void Configure(XmlElement element);
	}
}
