using log4net.Repository;
using log4net.Util;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Xml;

namespace log4net.Config
{
	/// <summary>
	/// Use this class to initialize the log4net environment using an Xml tree.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Configures a <see cref="T:log4net.Repository.ILoggerRepository" /> using an Xml tree.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public sealed class XmlConfigurator
	{
		/// <summary>
		/// Class used to watch config files.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Uses the <see cref="T:System.IO.FileSystemWatcher" /> to monitor
		/// changes to a specified file. Because multiple change notifications
		/// may be raised when the file is modified, a timer is used to
		/// compress the notifications into a single event. The timer
		/// waits for <see cref="F:log4net.Config.XmlConfigurator.ConfigureAndWatchHandler.TimeoutMillis" /> time before delivering
		/// the event notification. If any further <see cref="T:System.IO.FileSystemWatcher" />
		/// change notifications arrive while the timer is waiting it
		/// is reset and waits again for <see cref="F:log4net.Config.XmlConfigurator.ConfigureAndWatchHandler.TimeoutMillis" /> to
		/// elapse.
		/// </para>
		/// </remarks>
		private sealed class ConfigureAndWatchHandler
		{
			/// <summary>
			/// The default amount of time to wait after receiving notification
			/// before reloading the config file.
			/// </summary>
			private const int TimeoutMillis = 500;

			/// <summary>
			/// Holds the FileInfo used to configure the XmlConfigurator
			/// </summary>
			private FileInfo m_configFile;

			/// <summary>
			/// Holds the repository being configured.
			/// </summary>
			private ILoggerRepository m_repository;

			/// <summary>
			/// The timer used to compress the notification events.
			/// </summary>
			private Timer m_timer;

			/// <summary>
			/// Watch a specified config file used to configure a repository
			/// </summary>
			/// <param name="repository">The repository to configure.</param>
			/// <param name="configFile">The configuration file to watch.</param>
			/// <remarks>
			/// <para>
			/// Watch a specified config file used to configure a repository
			/// </para>
			/// </remarks>
			internal static void StartWatching(ILoggerRepository repository, FileInfo configFile)
			{
				new ConfigureAndWatchHandler(repository, configFile);
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="T:log4net.Config.XmlConfigurator.ConfigureAndWatchHandler" /> class.
			/// </summary>
			/// <param name="repository">The repository to configure.</param>
			/// <param name="configFile">The configuration file to watch.</param>
			/// <remarks>
			/// <para>
			/// Initializes a new instance of the <see cref="T:log4net.Config.XmlConfigurator.ConfigureAndWatchHandler" /> class.
			/// </para>
			/// </remarks>
			private ConfigureAndWatchHandler(ILoggerRepository repository, FileInfo configFile)
			{
				m_repository = repository;
				m_configFile = configFile;
				FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
				fileSystemWatcher.Path = m_configFile.DirectoryName;
				fileSystemWatcher.Filter = m_configFile.Name;
				fileSystemWatcher.NotifyFilter = (NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime);
				fileSystemWatcher.Changed += ConfigureAndWatchHandler_OnChanged;
				fileSystemWatcher.Created += ConfigureAndWatchHandler_OnChanged;
				fileSystemWatcher.Deleted += ConfigureAndWatchHandler_OnChanged;
				fileSystemWatcher.Renamed += ConfigureAndWatchHandler_OnRenamed;
				fileSystemWatcher.EnableRaisingEvents = true;
				m_timer = new Timer(OnWatchedFileChange, null, -1, -1);
			}

			/// <summary>
			/// Event handler used by <see cref="T:log4net.Config.XmlConfigurator.ConfigureAndWatchHandler" />.
			/// </summary>
			/// <param name="source">The <see cref="T:System.IO.FileSystemWatcher" /> firing the event.</param>
			/// <param name="e">The argument indicates the file that caused the event to be fired.</param>
			/// <remarks>
			/// <para>
			/// This handler reloads the configuration from the file when the event is fired.
			/// </para>
			/// </remarks>
			private void ConfigureAndWatchHandler_OnChanged(object source, FileSystemEventArgs e)
			{
				LogLog.Debug(string.Concat("ConfigureAndWatchHandler: ", e.ChangeType, " [", m_configFile.FullName, "]"));
				m_timer.Change(500, -1);
			}

			/// <summary>
			/// Event handler used by <see cref="T:log4net.Config.XmlConfigurator.ConfigureAndWatchHandler" />.
			/// </summary>
			/// <param name="source">The <see cref="T:System.IO.FileSystemWatcher" /> firing the event.</param>
			/// <param name="e">The argument indicates the file that caused the event to be fired.</param>
			/// <remarks>
			/// <para>
			/// This handler reloads the configuration from the file when the event is fired.
			/// </para>
			/// </remarks>
			private void ConfigureAndWatchHandler_OnRenamed(object source, RenamedEventArgs e)
			{
				LogLog.Debug(string.Concat("ConfigureAndWatchHandler: ", e.ChangeType, " [", m_configFile.FullName, "]"));
				m_timer.Change(500, -1);
			}

			/// <summary>
			/// Called by the timer when the configuration has been updated.
			/// </summary>
			/// <param name="state">null</param>
			private void OnWatchedFileChange(object state)
			{
				Configure(m_repository, m_configFile);
			}
		}

		/// <summary>
		/// Private constructor
		/// </summary>
		private XmlConfigurator()
		{
		}

		/// <summary>
		/// Automatically configures the log4net system based on the 
		/// application's configuration settings.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Each application has a configuration file. This has the
		/// same name as the application with '.config' appended.
		/// This file is XML and calling this function prompts the
		/// configurator to look in that file for a section called
		/// <c>log4net</c> that contains the configuration data.
		/// </para>
		/// <para>
		/// To use this method to configure log4net you must specify 
		/// the <see cref="T:log4net.Config.Log4NetConfigurationSectionHandler" /> section
		/// handler for the <c>log4net</c> configuration section. See the
		/// <see cref="T:log4net.Config.Log4NetConfigurationSectionHandler" /> for an example.
		/// </para>
		/// </remarks>
		/// <seealso cref="T:log4net.Config.Log4NetConfigurationSectionHandler" />
		public static void Configure()
		{
			Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()));
		}

		/// <summary>
		/// Automatically configures the <see cref="T:log4net.Repository.ILoggerRepository" /> using settings
		/// stored in the application's configuration file.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Each application has a configuration file. This has the
		/// same name as the application with '.config' appended.
		/// This file is XML and calling this function prompts the
		/// configurator to look in that file for a section called
		/// <c>log4net</c> that contains the configuration data.
		/// </para>
		/// <para>
		/// To use this method to configure log4net you must specify 
		/// the <see cref="T:log4net.Config.Log4NetConfigurationSectionHandler" /> section
		/// handler for the <c>log4net</c> configuration section. See the
		/// <see cref="T:log4net.Config.Log4NetConfigurationSectionHandler" /> for an example.
		/// </para>
		/// </remarks>
		/// <param name="repository">The repository to configure.</param>
		public static void Configure(ILoggerRepository repository)
		{
			LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using .config file section");
			try
			{
				LogLog.Debug("XmlConfigurator: Application config file is [" + SystemInfo.ConfigurationFileLocation + "]");
			}
			catch
			{
				LogLog.Debug("XmlConfigurator: Application config file location unknown");
			}
			try
			{
				XmlElement xmlElement = null;
				xmlElement = (ConfigurationManager.GetSection("log4net") as XmlElement);
				if (xmlElement == null)
				{
					LogLog.Error("XmlConfigurator: Failed to find configuration section 'log4net' in the application's .config file. Check your .config file for the <log4net> and <configSections> elements. The configuration section should look like: <section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler,log4net\" />");
				}
				else
				{
					ConfigureFromXml(repository, xmlElement);
				}
			}
			catch (ConfigurationException ex)
			{
				if (ex.BareMessage.IndexOf("Unrecognized element") >= 0)
				{
					LogLog.Error("XmlConfigurator: Failed to parse config file. Check your .config file is well formed XML.", ex);
				}
				else
				{
					string str = "<section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler," + Assembly.GetExecutingAssembly().FullName + "\" />";
					LogLog.Error("XmlConfigurator: Failed to parse config file. Is the <configSections> specified as: " + str, ex);
				}
			}
		}

		/// <summary>
		/// Configures log4net using a <c>log4net</c> element
		/// </summary>
		/// <remarks>
		/// <para>
		/// Loads the log4net configuration from the XML element
		/// supplied as <paramref name="element" />.
		/// </para>
		/// </remarks>
		/// <param name="element">The element to parse.</param>
		public static void Configure(XmlElement element)
		{
			ConfigureFromXml(LogManager.GetRepository(Assembly.GetCallingAssembly()), element);
		}

		/// <summary>
		/// Configures the <see cref="T:log4net.Repository.ILoggerRepository" /> using the specified XML 
		/// element.
		/// </summary>
		/// <remarks>
		/// Loads the log4net configuration from the XML element
		/// supplied as <paramref name="element" />.
		/// </remarks>
		/// <param name="repository">The repository to configure.</param>
		/// <param name="element">The element to parse.</param>
		public static void Configure(ILoggerRepository repository, XmlElement element)
		{
			LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using XML element");
			ConfigureFromXml(repository, element);
		}

		/// <summary>
		/// Configures log4net using the specified configuration file.
		/// </summary>
		/// <param name="configFile">The XML file to load the configuration from.</param>
		/// <remarks>
		/// <para>
		/// The configuration file must be valid XML. It must contain
		/// at least one element called <c>log4net</c> that holds
		/// the log4net configuration data.
		/// </para>
		/// <para>
		/// The log4net configuration file can possible be specified in the application's
		/// configuration file (either <c>MyAppName.exe.config</c> for a
		/// normal application on <c>Web.config</c> for an ASP.NET application).
		/// </para>
		/// <para>
		/// The first element matching <c>&lt;configuration&gt;</c> will be read as the 
		/// configuration. If this file is also a .NET .config file then you must specify 
		/// a configuration section for the <c>log4net</c> element otherwise .NET will 
		/// complain. Set the type for the section handler to <see cref="T:System.Configuration.IgnoreSectionHandler" />, for example:
		/// <code lang="XML" escaped="true">
		/// <configSections>
		/// 	<section name="log4net" type="System.Configuration.IgnoreSectionHandler" />
		/// </configSections>
		/// </code>
		/// </para>
		/// <example>
		/// The following example configures log4net using a configuration file, of which the 
		/// location is stored in the application's configuration file :
		/// </example>
		/// <code lang="C#">
		/// using log4net.Config;
		/// using System.IO;
		/// using System.Configuration;
		///
		/// ...
		///
		/// XmlConfigurator.Configure(new FileInfo(ConfigurationSettings.AppSettings["log4net-config-file"]));
		/// </code>
		/// <para>
		/// In the <c>.config</c> file, the path to the log4net can be specified like this :
		/// </para>
		/// <code lang="XML" escaped="true">
		/// <configuration>
		/// 	<appSettings>
		/// 		<add key="log4net-config-file" value="log.config" />
		/// 	</appSettings>
		/// </configuration>
		/// </code>
		/// </remarks>
		public static void Configure(FileInfo configFile)
		{
			Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), configFile);
		}

		/// <summary>
		/// Configures log4net using the specified configuration URI.
		/// </summary>
		/// <param name="configUri">A URI to load the XML configuration from.</param>
		/// <remarks>
		/// <para>
		/// The configuration data must be valid XML. It must contain
		/// at least one element called <c>log4net</c> that holds
		/// the log4net configuration data.
		/// </para>
		/// <para>
		/// The <see cref="T:System.Net.WebRequest" /> must support the URI scheme specified.
		/// </para>
		/// </remarks>
		public static void Configure(Uri configUri)
		{
			Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), configUri);
		}

		/// <summary>
		/// Configures log4net using the specified configuration data stream.
		/// </summary>
		/// <param name="configStream">A stream to load the XML configuration from.</param>
		/// <remarks>
		/// <para>
		/// The configuration data must be valid XML. It must contain
		/// at least one element called <c>log4net</c> that holds
		/// the log4net configuration data.
		/// </para>
		/// <para>
		/// Note that this method will NOT close the stream parameter.
		/// </para>
		/// </remarks>
		public static void Configure(Stream configStream)
		{
			Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), configStream);
		}

		/// <summary>
		/// Configures the <see cref="T:log4net.Repository.ILoggerRepository" /> using the specified configuration 
		/// file.
		/// </summary>
		/// <param name="repository">The repository to configure.</param>
		/// <param name="configFile">The XML file to load the configuration from.</param>
		/// <remarks>
		/// <para>
		/// The configuration file must be valid XML. It must contain
		/// at least one element called <c>log4net</c> that holds
		/// the configuration data.
		/// </para>
		/// <para>
		/// The log4net configuration file can possible be specified in the application's
		/// configuration file (either <c>MyAppName.exe.config</c> for a
		/// normal application on <c>Web.config</c> for an ASP.NET application).
		/// </para>
		/// <para>
		/// The first element matching <c>&lt;configuration&gt;</c> will be read as the 
		/// configuration. If this file is also a .NET .config file then you must specify 
		/// a configuration section for the <c>log4net</c> element otherwise .NET will 
		/// complain. Set the type for the section handler to <see cref="T:System.Configuration.IgnoreSectionHandler" />, for example:
		/// <code lang="XML" escaped="true">
		/// <configSections>
		/// 	<section name="log4net" type="System.Configuration.IgnoreSectionHandler" />
		/// </configSections>
		/// </code>
		/// </para>
		/// <example>
		/// The following example configures log4net using a configuration file, of which the 
		/// location is stored in the application's configuration file :
		/// </example>
		/// <code lang="C#">
		/// using log4net.Config;
		/// using System.IO;
		/// using System.Configuration;
		///
		/// ...
		///
		/// XmlConfigurator.Configure(new FileInfo(ConfigurationSettings.AppSettings["log4net-config-file"]));
		/// </code>
		/// <para>
		/// In the <c>.config</c> file, the path to the log4net can be specified like this :
		/// </para>
		/// <code lang="XML" escaped="true">
		/// <configuration>
		/// 	<appSettings>
		/// 		<add key="log4net-config-file" value="log.config" />
		/// 	</appSettings>
		/// </configuration>
		/// </code>
		/// </remarks>
		public static void Configure(ILoggerRepository repository, FileInfo configFile)
		{
			LogLog.Debug(string.Concat("XmlConfigurator: configuring repository [", repository.Name, "] using file [", configFile, "]"));
			if (configFile == null)
			{
				LogLog.Error("XmlConfigurator: Configure called with null 'configFile' parameter");
			}
			else if (File.Exists(configFile.FullName))
			{
				FileStream fileStream = null;
				int num = 5;
				while (--num >= 0)
				{
					try
					{
						fileStream = configFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
					}
					catch (IOException exception)
					{
						if (num == 0)
						{
							LogLog.Error("XmlConfigurator: Failed to open XML config file [" + configFile.Name + "]", exception);
							fileStream = null;
						}
						Thread.Sleep(250);
						continue;
					}
					break;
				}
				if (fileStream != null)
				{
					try
					{
						Configure(repository, fileStream);
					}
					finally
					{
						fileStream.Close();
					}
				}
			}
			else
			{
				LogLog.Debug("XmlConfigurator: config file [" + configFile.FullName + "] not found. Configuration unchanged.");
			}
		}

		/// <summary>
		/// Configures the <see cref="T:log4net.Repository.ILoggerRepository" /> using the specified configuration 
		/// URI.
		/// </summary>
		/// <param name="repository">The repository to configure.</param>
		/// <param name="configUri">A URI to load the XML configuration from.</param>
		/// <remarks>
		/// <para>
		/// The configuration data must be valid XML. It must contain
		/// at least one element called <c>log4net</c> that holds
		/// the configuration data.
		/// </para>
		/// <para>
		/// The <see cref="T:System.Net.WebRequest" /> must support the URI scheme specified.
		/// </para>
		/// </remarks>
		public static void Configure(ILoggerRepository repository, Uri configUri)
		{
			LogLog.Debug(string.Concat("XmlConfigurator: configuring repository [", repository.Name, "] using URI [", configUri, "]"));
			if (configUri == null)
			{
				LogLog.Error("XmlConfigurator: Configure called with null 'configUri' parameter");
				return;
			}
			if (configUri.IsFile)
			{
				Configure(repository, new FileInfo(configUri.LocalPath));
				return;
			}
			WebRequest webRequest = null;
			try
			{
				webRequest = WebRequest.Create(configUri);
			}
			catch (Exception exception)
			{
				LogLog.Error(string.Concat("XmlConfigurator: Failed to create WebRequest for URI [", configUri, "]"), exception);
			}
			if (webRequest != null)
			{
				try
				{
					webRequest.Credentials = CredentialCache.DefaultCredentials;
				}
				catch
				{
				}
				try
				{
					WebResponse response = webRequest.GetResponse();
					if (response != null)
					{
						try
						{
							using (Stream configStream = response.GetResponseStream())
							{
								Configure(repository, configStream);
							}
						}
						finally
						{
							response.Close();
						}
					}
				}
				catch (Exception exception)
				{
					LogLog.Error(string.Concat("XmlConfigurator: Failed to request config from URI [", configUri, "]"), exception);
				}
			}
		}

		/// <summary>
		/// Configures the <see cref="T:log4net.Repository.ILoggerRepository" /> using the specified configuration 
		/// file.
		/// </summary>
		/// <param name="repository">The repository to configure.</param>
		/// <param name="configStream">The stream to load the XML configuration from.</param>
		/// <remarks>
		/// <para>
		/// The configuration data must be valid XML. It must contain
		/// at least one element called <c>log4net</c> that holds
		/// the configuration data.
		/// </para>
		/// <para>
		/// Note that this method will NOT close the stream parameter.
		/// </para>
		/// </remarks>
		public static void Configure(ILoggerRepository repository, Stream configStream)
		{
			LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using stream");
			if (configStream == null)
			{
				LogLog.Error("XmlConfigurator: Configure called with null 'configStream' parameter");
				return;
			}
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				xmlReaderSettings.ProhibitDtd = false;
				XmlReader reader = XmlReader.Create(configStream, xmlReaderSettings);
				xmlDocument.Load(reader);
			}
			catch (Exception exception)
			{
				LogLog.Error("XmlConfigurator: Error while loading XML configuration", exception);
				xmlDocument = null;
			}
			if (xmlDocument != null)
			{
				LogLog.Debug("XmlConfigurator: loading XML configuration");
				XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("log4net");
				if (elementsByTagName.Count == 0)
				{
					LogLog.Debug("XmlConfigurator: XML configuration does not contain a <log4net> element. Configuration Aborted.");
				}
				else if (elementsByTagName.Count > 1)
				{
					LogLog.Error("XmlConfigurator: XML configuration contains [" + elementsByTagName.Count + "] <log4net> elements. Only one is allowed. Configuration Aborted.");
				}
				else
				{
					ConfigureFromXml(repository, elementsByTagName[0] as XmlElement);
				}
			}
		}

		/// <summary>
		/// Configures log4net using the file specified, monitors the file for changes 
		/// and reloads the configuration if a change is detected.
		/// </summary>
		/// <param name="configFile">The XML file to load the configuration from.</param>
		/// <remarks>
		/// <para>
		/// The configuration file must be valid XML. It must contain
		/// at least one element called <c>log4net</c> that holds
		/// the configuration data.
		/// </para>
		/// <para>
		/// The configuration file will be monitored using a <see cref="T:System.IO.FileSystemWatcher" />
		/// and depends on the behavior of that class.
		/// </para>
		/// <para>
		/// For more information on how to configure log4net using
		/// a separate configuration file, see <see cref="M:log4net.Config.XmlConfigurator.Configure(System.IO.FileInfo)" />.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.Config.XmlConfigurator.Configure(System.IO.FileInfo)" />
		public static void ConfigureAndWatch(FileInfo configFile)
		{
			ConfigureAndWatch(LogManager.GetRepository(Assembly.GetCallingAssembly()), configFile);
		}

		/// <summary>
		/// Configures the <see cref="T:log4net.Repository.ILoggerRepository" /> using the file specified, 
		/// monitors the file for changes and reloads the configuration if a change 
		/// is detected.
		/// </summary>
		/// <param name="repository">The repository to configure.</param>
		/// <param name="configFile">The XML file to load the configuration from.</param>
		/// <remarks>
		/// <para>
		/// The configuration file must be valid XML. It must contain
		/// at least one element called <c>log4net</c> that holds
		/// the configuration data.
		/// </para>
		/// <para>
		/// The configuration file will be monitored using a <see cref="T:System.IO.FileSystemWatcher" />
		/// and depends on the behavior of that class.
		/// </para>
		/// <para>
		/// For more information on how to configure log4net using
		/// a separate configuration file, see <see cref="M:log4net.Config.XmlConfigurator.Configure(System.IO.FileInfo)" />.
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.Config.XmlConfigurator.Configure(System.IO.FileInfo)" />
		public static void ConfigureAndWatch(ILoggerRepository repository, FileInfo configFile)
		{
			LogLog.Debug(string.Concat("XmlConfigurator: configuring repository [", repository.Name, "] using file [", configFile, "] watching for file updates"));
			if (configFile == null)
			{
				LogLog.Error("XmlConfigurator: ConfigureAndWatch called with null 'configFile' parameter");
				return;
			}
			Configure(repository, configFile);
			try
			{
				ConfigureAndWatchHandler.StartWatching(repository, configFile);
			}
			catch (Exception exception)
			{
				LogLog.Error("XmlConfigurator: Failed to initialize configuration file watcher for file [" + configFile.FullName + "]", exception);
			}
		}

		/// <summary>
		/// Configures the specified repository using a <c>log4net</c> element.
		/// </summary>
		/// <param name="repository">The hierarchy to configure.</param>
		/// <param name="element">The element to parse.</param>
		/// <remarks>
		/// <para>
		/// Loads the log4net configuration from the XML element
		/// supplied as <paramref name="element" />.
		/// </para>
		/// <para>
		/// This method is ultimately called by one of the Configure methods 
		/// to load the configuration from an <see cref="T:System.Xml.XmlElement" />.
		/// </para>
		/// </remarks>
		private static void ConfigureFromXml(ILoggerRepository repository, XmlElement element)
		{
			if (element == null)
			{
				LogLog.Error("XmlConfigurator: ConfigureFromXml called with null 'element' parameter");
				return;
			}
			if (repository == null)
			{
				LogLog.Error("XmlConfigurator: ConfigureFromXml called with null 'repository' parameter");
				return;
			}
			LogLog.Debug("XmlConfigurator: Configuring Repository [" + repository.Name + "]");
			IXmlRepositoryConfigurator xmlRepositoryConfigurator = repository as IXmlRepositoryConfigurator;
			if (xmlRepositoryConfigurator == null)
			{
				LogLog.Warn(string.Concat("XmlConfigurator: Repository [", repository, "] does not support the XmlConfigurator"));
				return;
			}
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement element2 = (XmlElement)xmlDocument.AppendChild(xmlDocument.ImportNode(element, deep: true));
			xmlRepositoryConfigurator.Configure(element2);
		}
	}
}
