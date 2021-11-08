using log4net.Core;

namespace log4net.Repository.Hierarchy
{
	/// <summary>
	/// Default implementation of <see cref="T:log4net.Repository.Hierarchy.ILoggerFactory" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// This default implementation of the <see cref="T:log4net.Repository.Hierarchy.ILoggerFactory" />
	/// interface is used to create the default subclass
	/// of the <see cref="T:log4net.Repository.Hierarchy.Logger" /> object.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	internal class DefaultLoggerFactory : ILoggerFactory
	{
		/// <summary>
		/// Default internal subclass of <see cref="T:log4net.Repository.Hierarchy.Logger" />
		/// </summary>
		/// <remarks>
		/// <para>
		/// This subclass has no additional behavior over the
		/// <see cref="T:log4net.Repository.Hierarchy.Logger" /> class but does allow instances
		/// to be created.
		/// </para>
		/// </remarks>
		internal sealed class LoggerImpl : Logger
		{
			/// <summary>
			/// Construct a new Logger
			/// </summary>
			/// <param name="name">the name of the logger</param>
			/// <remarks>
			/// <para>
			/// Initializes a new instance of the <see cref="T:log4net.Repository.Hierarchy.DefaultLoggerFactory.LoggerImpl" /> class
			/// with the specified name. 
			/// </para>
			/// </remarks>
			internal LoggerImpl(string name)
				: base(name)
			{
			}
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Repository.Hierarchy.DefaultLoggerFactory" /> class. 
		/// </para>
		/// </remarks>
		internal DefaultLoggerFactory()
		{
		}

		/// <summary>
		/// Create a new <see cref="T:log4net.Repository.Hierarchy.Logger" /> instance
		/// </summary>
		/// <param name="name">The name of the <see cref="T:log4net.Repository.Hierarchy.Logger" />.</param>
		/// <returns>The <see cref="T:log4net.Repository.Hierarchy.Logger" /> instance for the specified name.</returns>
		/// <remarks>
		/// <para>
		/// Create a new <see cref="T:log4net.Repository.Hierarchy.Logger" /> instance with the 
		/// specified name.
		/// </para>
		/// <para>
		/// Called by the <see cref="T:log4net.Repository.Hierarchy.Hierarchy" /> to create
		/// new named <see cref="T:log4net.Repository.Hierarchy.Logger" /> instances.
		/// </para>
		/// <para>
		/// If the <paramref name="name" /> is <c>null</c> then the root logger
		/// must be returned.
		/// </para>
		/// </remarks>
		public Logger CreateLogger(string name)
		{
			if (name == null)
			{
				return new RootLogger(Level.Debug);
			}
			return new LoggerImpl(name);
		}
	}
}
