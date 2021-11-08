using System.Configuration;
using System.Xml;

namespace log4net.Config
{
	/// <summary>
	/// Class to register for the log4net section of the configuration file
	/// </summary>
	/// <remarks>
	/// The log4net section of the configuration file needs to have a section
	/// handler registered. This is the section handler used. It simply returns
	/// the XML element that is the root of the section.
	/// </remarks>
	/// <example>
	/// Example of registering the log4net section handler :
	/// <code lang="XML" escaped="true">
	/// <configuration>
	/// 	<configSections>
	/// 		<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
	/// 	</configSections>
	/// 	<log4net>
	/// 		log4net configuration XML goes here
	/// 	</log4net>
	/// </configuration>
	/// </code>
	/// </example>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class Log4NetConfigurationSectionHandler : IConfigurationSectionHandler
	{
		/// <summary>
		/// Parses the configuration section.
		/// </summary>
		/// <param name="parent">The configuration settings in a corresponding parent configuration section.</param>
		/// <param name="configContext">The configuration context when called from the ASP.NET configuration system. Otherwise, this parameter is reserved and is a null reference.</param>
		/// <param name="section">The <see cref="T:System.Xml.XmlNode" /> for the log4net section.</param>
		/// <returns>The <see cref="T:System.Xml.XmlNode" /> for the log4net section.</returns>
		/// <remarks>
		/// <para>
		/// Returns the <see cref="T:System.Xml.XmlNode" /> containing the configuration data,
		/// </para>
		/// </remarks>
		public object Create(object parent, object configContext, XmlNode section)
		{
			return section;
		}
	}
}
