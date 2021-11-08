namespace log4net.Repository.Hierarchy
{
	/// <summary>
	/// Interface abstracts creation of <see cref="T:log4net.Repository.Hierarchy.Logger" /> instances
	/// </summary>
	/// <remarks>
	/// <para>
	/// This interface is used by the <see cref="T:log4net.Repository.Hierarchy.Hierarchy" /> to 
	/// create new <see cref="T:log4net.Repository.Hierarchy.Logger" /> objects.
	/// </para>
	/// <para>
	/// The <see cref="M:log4net.Repository.Hierarchy.ILoggerFactory.CreateLogger(System.String)" /> method is called
	/// to create a named <see cref="T:log4net.Repository.Hierarchy.Logger" />.
	/// </para>
	/// <para>
	/// Implement this interface to create new subclasses of <see cref="T:log4net.Repository.Hierarchy.Logger" />.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public interface ILoggerFactory
	{
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
		Logger CreateLogger(string name);
	}
}
