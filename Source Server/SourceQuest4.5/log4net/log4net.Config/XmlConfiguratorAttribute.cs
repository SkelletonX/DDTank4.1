using log4net.Repository;
using log4net.Util;
using System;
using System.IO;
using System.Reflection;

namespace log4net.Config
{
	/// <summary>
	/// Assembly level attribute to configure the <see cref="T:log4net.Config.XmlConfigurator" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This attribute may only be used at the assembly scope and can only
	/// be used once per assembly.
	/// </para>
	/// <para>
	/// Use this attribute to configure the <see cref="T:log4net.Config.XmlConfigurator" />
	/// without calling one of the <see cref="M:log4net.Config.XmlConfigurator.Configure" />
	/// methods.
	/// </para>
	/// <para>
	/// If neither of the <see cref="P:log4net.Config.XmlConfiguratorAttribute.ConfigFile" /> or <see cref="P:log4net.Config.XmlConfiguratorAttribute.ConfigFileExtension" />
	/// properties are set the configuration is loaded from the application's .config file.
	/// If set the <see cref="P:log4net.Config.XmlConfiguratorAttribute.ConfigFile" /> property takes priority over the
	/// <see cref="P:log4net.Config.XmlConfiguratorAttribute.ConfigFileExtension" /> property. The <see cref="P:log4net.Config.XmlConfiguratorAttribute.ConfigFile" /> property
	/// specifies a path to a file to load the config from. The path is relative to the
	/// application's base directory; <see cref="P:System.AppDomain.BaseDirectory" />.
	/// The <see cref="P:log4net.Config.XmlConfiguratorAttribute.ConfigFileExtension" /> property is used as a postfix to the assembly file name.
	/// The config file must be located in the  application's base directory; <see cref="P:System.AppDomain.BaseDirectory" />.
	/// For example in a console application setting the <see cref="P:log4net.Config.XmlConfiguratorAttribute.ConfigFileExtension" /> to
	/// <c>config</c> has the same effect as not specifying the <see cref="P:log4net.Config.XmlConfiguratorAttribute.ConfigFile" /> or 
	/// <see cref="P:log4net.Config.XmlConfiguratorAttribute.ConfigFileExtension" /> properties.
	/// </para>
	/// <para>
	/// The <see cref="P:log4net.Config.XmlConfiguratorAttribute.Watch" /> property can be set to cause the <see cref="T:log4net.Config.XmlConfigurator" />
	/// to watch the configuration file for changes.
	/// </para>
	/// <note>
	/// <para>
	/// Log4net will only look for assembly level configuration attributes once.
	/// When using the log4net assembly level attributes to control the configuration 
	/// of log4net you must ensure that the first call to any of the 
	/// <see cref="T:log4net.Core.LoggerManager" /> methods is made from the assembly with the configuration
	/// attributes. 
	/// </para>
	/// <para>
	/// If you cannot guarantee the order in which log4net calls will be made from 
	/// different assemblies you must use programmatic configuration instead, i.e.
	/// call the <see cref="M:log4net.Config.XmlConfigurator.Configure" /> method directly.
	/// </para>
	/// </note>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	[Serializable]
	[AttributeUsage(AttributeTargets.Assembly)]
	public class XmlConfiguratorAttribute : ConfiguratorAttribute
	{
		private string m_configFile = null;

		private string m_configFileExtension = null;

		private bool m_configureAndWatch = false;

		/// <summary>
		/// Gets or sets the filename of the configuration file.
		/// </summary>
		/// <value>
		/// The filename of the configuration file.
		/// </value>
		/// <remarks>
		/// <para>
		/// If specified, this is the name of the configuration file to use with
		/// the <see cref="T:log4net.Config.XmlConfigurator" />. This file path is relative to the
		/// <b>application base</b> directory (<see cref="P:System.AppDomain.BaseDirectory" />).
		/// </para>
		/// <para>
		/// The <see cref="P:log4net.Config.XmlConfiguratorAttribute.ConfigFile" /> takes priority over the <see cref="P:log4net.Config.XmlConfiguratorAttribute.ConfigFileExtension" />.
		/// </para>
		/// </remarks>
		public string ConfigFile
		{
			get
			{
				return m_configFile;
			}
			set
			{
				m_configFile = value;
			}
		}

		/// <summary>
		/// Gets or sets the extension of the configuration file.
		/// </summary>
		/// <value>
		/// The extension of the configuration file.
		/// </value>
		/// <remarks>
		/// <para>
		/// If specified this is the extension for the configuration file.
		/// The path to the config file is built by using the <b>application 
		/// base</b> directory (<see cref="P:System.AppDomain.BaseDirectory" />),
		/// the <b>assembly file name</b> and the config file extension.
		/// </para>
		/// <para>
		/// If the <see cref="P:log4net.Config.XmlConfiguratorAttribute.ConfigFileExtension" /> is set to <c>MyExt</c> then
		/// possible config file names would be: <c>MyConsoleApp.exe.MyExt</c> or
		/// <c>MyClassLibrary.dll.MyExt</c>.
		/// </para>
		/// <para>
		/// The <see cref="P:log4net.Config.XmlConfiguratorAttribute.ConfigFile" /> takes priority over the <see cref="P:log4net.Config.XmlConfiguratorAttribute.ConfigFileExtension" />.
		/// </para>
		/// </remarks>
		public string ConfigFileExtension
		{
			get
			{
				return m_configFileExtension;
			}
			set
			{
				m_configFileExtension = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to watch the configuration file.
		/// </summary>
		/// <value>
		/// <c>true</c> if the configuration should be watched, <c>false</c> otherwise.
		/// </value>
		/// <remarks>
		/// <para>
		/// If this flag is specified and set to <c>true</c> then the framework
		/// will watch the configuration file and will reload the config each time 
		/// the file is modified.
		/// </para>
		/// <para>
		/// The config file can only be watched if it is loaded from local disk.
		/// In a No-Touch (Smart Client) deployment where the application is downloaded
		/// from a web server the config file may not reside on the local disk
		/// and therefore it may not be able to watch it.
		/// </para>
		/// <note>
		/// Watching configuration is not supported on the SSCLI.
		/// </note>
		/// </remarks>
		public bool Watch
		{
			get
			{
				return m_configureAndWatch;
			}
			set
			{
				m_configureAndWatch = value;
			}
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Default constructor
		/// </para>
		/// </remarks>
		public XmlConfiguratorAttribute()
			: base(0)
		{
		}

		/// <summary>
		/// Configures the <see cref="T:log4net.Repository.ILoggerRepository" /> for the specified assembly.
		/// </summary>
		/// <param name="sourceAssembly">The assembly that this attribute was defined on.</param>
		/// <param name="targetRepository">The repository to configure.</param>
		/// <remarks>
		/// <para>
		/// Configure the repository using the <see cref="T:log4net.Config.XmlConfigurator" />.
		/// The <paramref name="targetRepository" /> specified must extend the <see cref="T:log4net.Repository.Hierarchy.Hierarchy" />
		/// class otherwise the <see cref="T:log4net.Config.XmlConfigurator" /> will not be able to
		/// configure it.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="repository" /> does not extend <see cref="T:log4net.Repository.Hierarchy.Hierarchy" />.</exception>
		public override void Configure(Assembly sourceAssembly, ILoggerRepository targetRepository)
		{
			string text = null;
			try
			{
				text = SystemInfo.ApplicationBaseDirectory;
			}
			catch
			{
			}
			if (text == null || new Uri(text).IsFile)
			{
				ConfigureFromFile(sourceAssembly, targetRepository);
			}
			else
			{
				ConfigureFromUri(sourceAssembly, targetRepository);
			}
		}

		/// <summary>
		/// Attempt to load configuration from the local file system
		/// </summary>
		/// <param name="sourceAssembly">The assembly that this attribute was defined on.</param>
		/// <param name="targetRepository">The repository to configure.</param>
		private void ConfigureFromFile(Assembly sourceAssembly, ILoggerRepository targetRepository)
		{
			string text = null;
			if (m_configFile == null || m_configFile.Length == 0)
			{
				if (m_configFileExtension == null || m_configFileExtension.Length == 0)
				{
					try
					{
						text = SystemInfo.ConfigurationFileLocation;
					}
					catch (Exception exception)
					{
						LogLog.Error("XmlConfiguratorAttribute: Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when ConfigFile and ConfigFileExtension properties are not set.", exception);
					}
				}
				else
				{
					if (m_configFileExtension[0] != '.')
					{
						m_configFileExtension = "." + m_configFileExtension;
					}
					string text2 = null;
					try
					{
						text2 = SystemInfo.ApplicationBaseDirectory;
					}
					catch (Exception exception)
					{
						LogLog.Error("XmlConfiguratorAttribute: Exception getting ApplicationBaseDirectory. Must be able to resolve ApplicationBaseDirectory and AssemblyFileName when ConfigFileExtension property is set.", exception);
					}
					if (text2 != null)
					{
						text = Path.Combine(text2, SystemInfo.AssemblyFileName(sourceAssembly) + m_configFileExtension);
					}
				}
			}
			else
			{
				string text2 = null;
				try
				{
					text2 = SystemInfo.ApplicationBaseDirectory;
				}
				catch (Exception exception)
				{
					LogLog.Warn("XmlConfiguratorAttribute: Exception getting ApplicationBaseDirectory. ConfigFile property path [" + m_configFile + "] will be treated as an absolute path.", exception);
				}
				text = ((text2 == null) ? m_configFile : Path.Combine(text2, m_configFile));
			}
			if (text != null)
			{
				ConfigureFromFile(targetRepository, new FileInfo(text));
			}
		}

		/// <summary>
		/// Configure the specified repository using a <see cref="T:System.IO.FileInfo" />
		/// </summary>
		/// <param name="targetRepository">The repository to configure.</param>
		/// <param name="configFile">the FileInfo pointing to the config file</param>
		private void ConfigureFromFile(ILoggerRepository targetRepository, FileInfo configFile)
		{
			if (m_configureAndWatch)
			{
				XmlConfigurator.ConfigureAndWatch(targetRepository, configFile);
			}
			else
			{
				XmlConfigurator.Configure(targetRepository, configFile);
			}
		}

		/// <summary>
		/// Attempt to load configuration from a URI
		/// </summary>
		/// <param name="sourceAssembly">The assembly that this attribute was defined on.</param>
		/// <param name="targetRepository">The repository to configure.</param>
		private void ConfigureFromUri(Assembly sourceAssembly, ILoggerRepository targetRepository)
		{
			Uri uri = null;
			if (m_configFile == null || m_configFile.Length == 0)
			{
				if (m_configFileExtension == null || m_configFileExtension.Length == 0)
				{
					string text = null;
					try
					{
						text = SystemInfo.ConfigurationFileLocation;
					}
					catch (Exception exception)
					{
						LogLog.Error("XmlConfiguratorAttribute: Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when ConfigFile and ConfigFileExtension properties are not set.", exception);
					}
					if (text != null)
					{
						Uri uri2 = new Uri(text);
						uri = uri2;
					}
				}
				else
				{
					if (m_configFileExtension[0] != '.')
					{
						m_configFileExtension = "." + m_configFileExtension;
					}
					string text = null;
					try
					{
						text = SystemInfo.ConfigurationFileLocation;
					}
					catch (Exception exception)
					{
						LogLog.Error("XmlConfiguratorAttribute: Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when the ConfigFile property are not set.", exception);
					}
					if (text != null)
					{
						UriBuilder uriBuilder = new UriBuilder(new Uri(text));
						string text2 = uriBuilder.Path;
						int num = text2.LastIndexOf(".");
						if (num >= 0)
						{
							text2 = text2.Substring(0, num);
						}
						text2 = (uriBuilder.Path = text2 + m_configFileExtension);
						uri = uriBuilder.Uri;
					}
				}
			}
			else
			{
				string text4 = null;
				try
				{
					text4 = SystemInfo.ApplicationBaseDirectory;
				}
				catch (Exception exception)
				{
					LogLog.Warn("XmlConfiguratorAttribute: Exception getting ApplicationBaseDirectory. ConfigFile property path [" + m_configFile + "] will be treated as an absolute URI.", exception);
				}
				uri = ((text4 == null) ? new Uri(m_configFile) : new Uri(new Uri(text4), m_configFile));
			}
			if (!(uri != null))
			{
				return;
			}
			if (uri.IsFile)
			{
				ConfigureFromFile(targetRepository, new FileInfo(uri.LocalPath));
				return;
			}
			if (m_configureAndWatch)
			{
				LogLog.Warn("XmlConfiguratorAttribute: Unable to watch config file loaded from a URI");
			}
			XmlConfigurator.Configure(targetRepository, uri);
		}
	}
}
