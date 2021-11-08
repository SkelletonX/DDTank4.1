using log4net.Core;
using System;

namespace log4net.Filter
{
	/// <summary>
	/// This is a very simple filter based on <see cref="T:log4net.Core.Level" /> matching.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The filter admits two options <see cref="P:log4net.Filter.LevelMatchFilter.LevelToMatch" /> and
	/// <see cref="P:log4net.Filter.LevelMatchFilter.AcceptOnMatch" />. If there is an exact match between the value
	/// of the <see cref="P:log4net.Filter.LevelMatchFilter.LevelToMatch" /> option and the <see cref="T:log4net.Core.Level" /> of the 
	/// <see cref="T:log4net.Core.LoggingEvent" />, then the <see cref="M:log4net.Filter.LevelMatchFilter.Decide(log4net.Core.LoggingEvent)" /> method returns <see cref="F:log4net.Filter.FilterDecision.Accept" /> in 
	/// case the <see cref="P:log4net.Filter.LevelMatchFilter.AcceptOnMatch" /> option value is set
	/// to <c>true</c>, if it is <c>false</c> then 
	/// <see cref="F:log4net.Filter.FilterDecision.Deny" /> is returned. If the <see cref="T:log4net.Core.Level" /> does not match then
	/// the result will be <see cref="F:log4net.Filter.FilterDecision.Neutral" />.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class LevelMatchFilter : FilterSkeleton
	{
		/// <summary>
		/// flag to indicate if the filter should <see cref="F:log4net.Filter.FilterDecision.Accept" /> on a match
		/// </summary>
		private bool m_acceptOnMatch = true;

		/// <summary>
		/// the <see cref="T:log4net.Core.Level" /> to match against
		/// </summary>
		private Level m_levelToMatch;

		/// <summary>
		/// <see cref="F:log4net.Filter.FilterDecision.Accept" /> when matching <see cref="P:log4net.Filter.LevelMatchFilter.LevelToMatch" />
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="P:log4net.Filter.LevelMatchFilter.AcceptOnMatch" /> property is a flag that determines
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
		/// The <see cref="T:log4net.Core.Level" /> that the filter will match
		/// </summary>
		/// <remarks>
		/// <para>
		/// The level that this filter will attempt to match against the 
		/// <see cref="T:log4net.Core.LoggingEvent" /> level. If a match is found then
		/// the result depends on the value of <see cref="P:log4net.Filter.LevelMatchFilter.AcceptOnMatch" />.
		/// </para>
		/// </remarks>
		public Level LevelToMatch
		{
			get
			{
				return m_levelToMatch;
			}
			set
			{
				m_levelToMatch = value;
			}
		}

		/// <summary>
		/// Tests if the <see cref="T:log4net.Core.Level" /> of the logging event matches that of the filter
		/// </summary>
		/// <param name="loggingEvent">the event to filter</param>
		/// <returns>see remarks</returns>
		/// <remarks>
		/// <para>
		/// If the <see cref="T:log4net.Core.Level" /> of the event matches the level of the
		/// filter then the result of the function depends on the
		/// value of <see cref="P:log4net.Filter.LevelMatchFilter.AcceptOnMatch" />. If it is true then
		/// the function will return <see cref="F:log4net.Filter.FilterDecision.Accept" />, it it is false then it
		/// will return <see cref="F:log4net.Filter.FilterDecision.Deny" />. If the <see cref="T:log4net.Core.Level" /> does not match then
		/// the result will be <see cref="F:log4net.Filter.FilterDecision.Neutral" />.
		/// </para>
		/// </remarks>
		public override FilterDecision Decide(LoggingEvent loggingEvent)
		{
			if (loggingEvent == null)
			{
				throw new ArgumentNullException("loggingEvent");
			}
			if (m_levelToMatch != null && m_levelToMatch == loggingEvent.Level)
			{
				return m_acceptOnMatch ? FilterDecision.Accept : FilterDecision.Deny;
			}
			return FilterDecision.Neutral;
		}
	}
}
