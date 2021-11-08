using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using log4net.Util;
using System.Reflection;

namespace log4net.Config
{
	/// <summary>
	/// Use this class to quickly configure a <see cref="T:log4net.Repository.Hierarchy.Hierarchy" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Allows very simple programmatic configuration of log4net.
	/// </para>
	/// <para>
	/// Only one appender can be configured using this configurator.
	/// The appender is set at the root of the hierarchy and all logging
	/// events will be delivered to that appender.
	/// </para>
	/// <para>
	/// Appenders can also implement the <see cref="T:log4net.Core.IOptionHandler" /> interface. Therefore
	/// they would require that the <see cref="M:log4net.Core.IOptionHandler.ActivateOptions" /> method
	/// be called after the appenders properties have been configured.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public sealed class BasicConfigurator
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Config.BasicConfigurator" /> class. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// Uses a private access modifier to prevent instantiation of this class.
		/// </para>
		/// </remarks>
		private BasicConfigurator()
		{
		}

		/// <summary>
		/// Initializes the log4net system with a default configuration.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes the log4net logging system using a <see cref="T:log4net.Appender.ConsoleAppender" />
		/// that will write to <c>Console.Out</c>. The log messages are
		/// formatted using the <see cref="T:log4net.Layout.PatternLayout" /> layout object
		/// with the <see cref="F:log4net.Layout.PatternLayout.DetailConversionPattern" />
		/// layout style.
		/// </para>
		/// </remarks>
		public static void Configure()
		{
			Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()));
		}

		/// <summary>
		/// Initializes the log4net system using the specified appender.
		/// </summary>
		/// <param name="appender">The appender to use to log all logging events.</param>
		/// <remarks>
		/// <para>
		/// Initializes the log4net system using the specified appender.
		/// </para>
		/// </remarks>
		public static void Configure(IAppender appender)
		{
			Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), appender);
		}

		/// <summary>
		/// Initializes the <see cref="T:log4net.Repository.ILoggerRepository" /> with a default configuration.
		/// </summary>
		/// <param name="repository">The repository to configure.</param>
		/// <remarks>
		/// <para>
		/// Initializes the specified repository using a <see cref="T:log4net.Appender.ConsoleAppender" />
		/// that will write to <c>Console.Out</c>. The log messages are
		/// formatted using the <see cref="T:log4net.Layout.PatternLayout" /> layout object
		/// with the <see cref="F:log4net.Layout.PatternLayout.DetailConversionPattern" />
		/// layout style.
		/// </para>
		/// </remarks>
		public static void Configure(ILoggerRepository repository)
		{
			PatternLayout patternLayout = new PatternLayout();
			patternLayout.ConversionPattern = "%timestamp [%thread] %level %logger %ndc - %message%newline";
			patternLayout.ActivateOptions();
			ConsoleAppender consoleAppender = new ConsoleAppender();
			consoleAppender.Layout = patternLayout;
			consoleAppender.ActivateOptions();
			Configure(repository, consoleAppender);
		}

		/// <summary>
		/// Initializes the <see cref="T:log4net.Repository.ILoggerRepository" /> using the specified appender.
		/// </summary>
		/// <param name="repository">The repository to configure.</param>
		/// <param name="appender">The appender to use to log all logging events.</param>
		/// <remarks>
		/// <para>
		/// Initializes the <see cref="T:log4net.Repository.ILoggerRepository" /> using the specified appender.
		/// </para>
		/// </remarks>
		public static void Configure(ILoggerRepository repository, IAppender appender)
		{
			IBasicRepositoryConfigurator basicRepositoryConfigurator = repository as IBasicRepositoryConfigurator;
			if (basicRepositoryConfigurator != null)
			{
				basicRepositoryConfigurator.Configure(appender);
			}
			else
			{
				LogLog.Warn(string.Concat("BasicConfigurator: Repository [", repository, "] does not support the BasicConfigurator"));
			}
		}
	}
}
