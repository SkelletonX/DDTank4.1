namespace log4net.Core
{
	/// <summary>
	/// Interface used to delay activate a configured object.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This allows an object to defer activation of its options until all
	/// options have been set. This is required for components which have
	/// related options that remain ambiguous until all are set.
	/// </para>
	/// <para>
	/// If a component implements this interface then the <see cref="M:log4net.Core.IOptionHandler.ActivateOptions" /> method 
	/// must be called by the container after its all the configured properties have been set 
	/// and before the component can be used.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public interface IOptionHandler
	{
		/// <summary>
		/// Activate the options that were previously set with calls to properties.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This allows an object to defer activation of its options until all
		/// options have been set. This is required for components which have
		/// related options that remain ambiguous until all are set.
		/// </para>
		/// <para>
		/// If a component implements this interface then this method must be called
		/// after its properties have been set before the component can be used.
		/// </para>
		/// </remarks>
		void ActivateOptions();
	}
}
