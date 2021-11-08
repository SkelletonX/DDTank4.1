namespace log4net.Filter
{
	/// <summary>
	/// Simple filter to match a keyed string in the <see cref="T:log4net.MDC" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// Simple filter to match a keyed string in the <see cref="T:log4net.MDC" />
	/// </para>
	/// <para>
	/// As the MDC has been replaced with layered properties the
	/// <see cref="T:log4net.Filter.PropertyFilter" /> should be used instead.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class MdcFilter : PropertyFilter
	{
	}
}
