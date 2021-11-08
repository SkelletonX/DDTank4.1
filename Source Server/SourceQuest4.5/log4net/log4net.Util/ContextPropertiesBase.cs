namespace log4net.Util
{
	/// <summary>
	/// Base class for Context Properties implementations
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class defines a basic property get set accessor
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public abstract class ContextPropertiesBase
	{
		/// <summary>
		/// Gets or sets the value of a property
		/// </summary>
		/// <value>
		/// The value for the property with the specified key
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets or sets the value of a property
		/// </para>
		/// </remarks>
		public abstract object this[string key]
		{
			get;
			set;
		}
	}
}
