using System.Collections;

namespace log4net.Repository.Hierarchy
{
	/// <summary>
	/// Provision nodes are used where no logger instance has been specified
	/// </summary>
	/// <remarks>
	/// <para>
	/// <see cref="T:log4net.Repository.Hierarchy.ProvisionNode" /> instances are used in the 
	/// <see cref="T:log4net.Repository.Hierarchy.Hierarchy" /> when there is no specified 
	/// <see cref="T:log4net.Repository.Hierarchy.Logger" /> for that node.
	/// </para>
	/// <para>
	/// A provision node holds a list of child loggers on behalf of
	/// a logger that does not exist.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	internal sealed class ProvisionNode : ArrayList
	{
		/// <summary>
		/// Create a new provision node with child node
		/// </summary>
		/// <param name="log">A child logger to add to this node.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Repository.Hierarchy.ProvisionNode" /> class 
		/// with the specified child logger.
		/// </para>
		/// </remarks>
		internal ProvisionNode(Logger log)
		{
			Add(log);
		}
	}
}
