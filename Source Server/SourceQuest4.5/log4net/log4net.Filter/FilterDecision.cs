namespace log4net.Filter
{
	/// <summary>
	/// The return result from <see cref="M:log4net.Filter.IFilter.Decide(log4net.Core.LoggingEvent)" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// The return result from <see cref="M:log4net.Filter.IFilter.Decide(log4net.Core.LoggingEvent)" />
	/// </para>
	/// </remarks>
	public enum FilterDecision
	{
		/// <summary>
		/// The log event must be dropped immediately without 
		/// consulting with the remaining filters, if any, in the chain.
		/// </summary>
		Deny = -1,
		/// <summary>
		/// This filter is neutral with respect to the log event. 
		/// The remaining filters, if any, should be consulted for a final decision.
		/// </summary>
		Neutral,
		/// <summary>
		/// The log event must be logged immediately without 
		/// consulting with the remaining filters, if any, in the chain.
		/// </summary>
		Accept
	}
}
