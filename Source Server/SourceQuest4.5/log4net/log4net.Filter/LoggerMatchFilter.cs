using log4net.Core;
using System;

namespace log4net.Filter
{
	/// <summary>
	/// Simple filter to match a string in the event's logger name.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The works very similar to the <see cref="T:log4net.Filter.LevelMatchFilter" />. It admits two 
	/// options <see cref="P:log4net.Filter.LoggerMatchFilter.LoggerToMatch" /> and <see cref="P:log4net.Filter.LoggerMatchFilter.AcceptOnMatch" />. If the 
	/// <see cref="P:log4net.Core.LoggingEvent.LoggerName" /> of the <see cref="T:log4net.Core.LoggingEvent" /> starts 
	/// with the value of the <see cref="P:log4net.Filter.LoggerMatchFilter.LoggerToMatch" /> option, then the 
	/// <see cref="M:log4net.Filter.LoggerMatchFilter.Decide(log4net.Core.LoggingEvent)" /> method returns <see cref="F:log4net.Filter.FilterDecision.Accept" /> in 
	/// case the <see cref="P:log4net.Filter.LoggerMatchFilter.AcceptOnMatch" /> option value is set to <c>true</c>, 
	/// if it is <c>false</c> then <see cref="F:log4net.Filter.FilterDecision.Deny" /> is returned.
	/// </para>
	/// </remarks>
	/// <author>Daniel Cazzulino</author>
	public class LoggerMatchFilter : FilterSkeleton
	{
		/// <summary>
		/// Flag to indicate the behavior when we have a match
		/// </summary>
		private bool m_acceptOnMatch = true;

		/// <summary>
		/// The logger name string to substring match against the event
		/// </summary>
		private string m_loggerToMatch;

		/// <summary>
		/// <see cref="F:log4net.Filter.FilterDecision.Accept" /> when matching <see cref="P:log4net.Filter.LoggerMatchFilter.LoggerToMatch" />
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="P:log4net.Filter.LoggerMatchFilter.AcceptOnMatch" /> property is a flag that determines
		/// the behavior when a matching <see cref="T:log4net.Core.Level" /> is found. If the
		/// flag is set to true then the filter will <see cref="F:log4net.Filter.FilterDecision.Accept" /> the 
		/// logging event, otherwise it will <see cref="F:log4net.Filter.FilterDecision.Deny" /> the event.
		/// </para>
		/// <para>
		/// The default is <c>true</c> i.e. to <see cref="F:log4net.Filter.FilterDecision.Accept" /> the event.
		/// </para>
		/// </remarks>
		public bool AcceptOnMatch
		{
			get
			{
				return m_acceptOnMatch;
			}
			set
			{
				m_acceptOnMatch = value;
			}
		}

		/// <summary>
		/// The <see cref="P:log4net.Core.LoggingEvent.LoggerName" /> that the filter will match
		/// </summary>
		/// <remarks>
		/// <para>
		/// This filter will attempt to match this value against logger name in
		/// the following way. The match will be done against the beginning of the
		/// logger name (using <see cref="M:System.String.StartsWith(System.String)" />). The match is
		/// case sensitive. If a match is found then
		/// the result depends on the value of <see cref="P:log4net.Filter.LoggerMatchFilter.AcceptOnMatch" />.
		/// </para>
		/// </remarks>
		public string LoggerToMatch
		{
			get
			{
				return m_loggerToMatch;
			}
			set
			{
				m_loggerToMatch = value;
			}
		}

		/// <summary>
		/// Check if this filter should allow the event to be logged
		/// </summary>
		/// <param name="loggingEvent">the event being logged</param>
		/// <returns>see remarks</returns>
		/// <remarks>
		/// <para>
		/// The rendered message is matched against the <see cref="P:log4net.Filter.LoggerMatchFilter.LoggerToMatch" />.
		/// If the <see cref="P:log4net.Filter.LoggerMatchFilter.LoggerToMatch" /> equals the beginning of 
		/// the incoming <see cref="P:log4net.Core.LoggingEvent.LoggerName" /> (<see cref="M:System.String.StartsWith(System.String)" />)
		/// then a match will have occurred. If no match occurs
		/// this function will return <see cref="F:log4net.Filter.FilterDecision.Neutral" />
		/// allowing other filters to check the event. If a match occurs then
		/// the value of <see cref="P:log4net.Filter.LoggerMatchFilter.AcceptOnMatch" /> is checked. If it is
		/// true then <see cref="F:log4net.Filter.FilterDecision.Accept" /> is returned otherwise
		/// <see cref="F:log4net.Filter.FilterDecision.Deny" /> is returned.
		/// </para>
		/// </remarks>
		public override FilterDecision Decide(LoggingEvent loggingEvent)
		{
			if (loggingEvent == null)
			{
				throw new ArgumentNullException("loggingEvent");
			}
			if (m_loggerToMatch != null && m_loggerToMatch.Length != 0 && loggingEvent.LoggerName.StartsWith(m_loggerToMatch))
			{
				if (m_acceptOnMatch)
				{
					return FilterDecision.Accept;
				}
				return FilterDecision.Deny;
			}
			return FilterDecision.Neutral;
		}
	}
}
