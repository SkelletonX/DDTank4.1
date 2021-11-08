using log4net.Appender;

namespace log4net.Repository
{
	/// <summary>
	/// Basic Configurator interface for repositories
	/// </summary>
	/// <remarks>
	/// <para>
	/// Interface used by basic configurator to configure a <see cref="T:log4net.Repository.ILoggerRepository" />
	/// with a default <see cref="T:log4net.Appender.IAppender" />.
	/// </para>
	/// <para>
	/// A <see cref="T:log4net.Repository.ILoggerRepository" /> should implement this interface to support
	/// configuration by the <see cref="T:log4net.Config.BasicConfigurator" />.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public interface IBasicRepositoryConfigurator
	{
		/// <summary>
		/// Initialize the repository using the specified appender
		/// </summary>
		/// <param name="appender">the appender to use to log all logging events</param>
		/// <remarks>
		/// <para>
		/// Configure the repository to route all logging events to the
		/// specified appender.
		/// </para>
		/// </remarks>
		void Configure(IAppender appender);
	}
}
