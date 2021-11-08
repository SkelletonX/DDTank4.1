namespace log4net.Filter
{
	/// <summary>
	/// Simple filter to match a string in the <see cref="T:log4net.NDC" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// Simple filter to match a string in the <see cref="T:log4net.NDC" />
	/// </para>
	/// <para>
	/// As the MDC has been replaced with named stacks stored in the
	/// properties collections the <see cref="T:log4net.Filter.PropertyFilter" /> should 
	/// be used instead.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class NdcFilter : PropertyFilter
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Sets the <see cref="P:log4net.Filter.PropertyFilter.Key" /> to <c>"NDC"</c>.
		/// </para>
		/// </remarks>
		public NdcFilter()
		{
			base.Key = "NDC";
		}
	}
}
