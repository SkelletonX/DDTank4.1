using log4net.Core;
using System;

namespace log4net.Filter
{
	/// <summary>
	/// This is a simple filter based on <see cref="T:log4net.Core.Level" /> matching.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The filter admits three options <see cref="P:log4net.Filter.LevelRangeFilter.LevelMin" /> and <see cref="P:log4net.Filter.LevelRangeFilter.LevelMax" />
	/// that determine the range of priorities that are matched, and
	/// <see cref="P:log4net.Filter.LevelRangeFilter.AcceptOnMatch" />. If there is a match between the range
	/// of priorities and the <see cref="T:log4net.Core.Level" /> of the <see cref="T:log4net.Core.LoggingEvent" />, then the 
	/// <see cref="M:log4net.Filter.LevelRangeFilter.Decide(log4net.Core.LoggingEvent)" /> method returns <see cref="F:log4net.Filter.FilterDecision.Accept" /> in case the <see cref="P:log4net.Filter.LevelRangeFilter.AcceptOnMatch" /> 
	/// option value is set to <c>true</c>, if it is <c>false</c>
	/// then <see cref="F:log4net.Filter.FilterDecision.Deny" /> is returned. If there is no match, <see cref="F:log4net.Filter.FilterDecision.Deny" /> is returned.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class LevelRangeFilter : FilterSkeleton
	{
		/// <summary>
		/// Flag to indicate the behavior when matching a <see cref="T:log4net.Core.Level" />
		/// </summary>
		private bool m_acceptOnMatch = true;

		/// <summary>
		/// the minimum <see cref="T:log4net.Core.Level" /> value to match
		/// </summary>
		private Level m_levelMin;

		/// <summary>
		/// the maximum <see cref="T:log4net.Core.Level" /> value to match
		/// </summary>
		private Level m_levelMax;

		/// <summary>
		/// <see cref="F:log4net.Filter.FilterDecision.Accept" /> when matching <see cref="P:log4net.Filter.LevelRangeFilter.LevelMin" /> and <see cref="P:log4net.Filter.LevelRangeFilter.LevelMax" />
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="P:log4net.Filter.LevelRangeFilter.AcceptOnMatch" /> property is a flag that determines
		/// the behavior when a matching <see cref="T:log4net.Core.Level" /> is found. If the
		/// flag is set to true then the filter will <see cref="F:log4net.Filter.FilterDecision.Accept" /> the 
		/// logging event, otherwise it will <see cref="F:log4net.Filter.FilterDecision.Neutral" /> the event.
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
		/// Set the minimum matched <see cref="T:log4net.Core.Level" />
		/// </summary>
		/// <remarks>
		/// <para>
		/// The minimum level that this filter will attempt to match against the 
		/// <see cref="T:log4net.Core.LoggingEvent" /> level. If a match is found then
		/// the result depends on the value of <see cref="P:log4net.Filter.LevelRangeFilter.AcceptOnMatch" />.
		/// </para>
		/// </remarks>
		public Level LevelMin
		{
			get
			{
				return m_levelMin;
			}
			set
			{
				m_levelMin = value;
			}
		}

		/// <summary>
		/// Sets the maximum matched <see cref="T:log4net.Core.Level" />
		/// </summary>
		/// <remarks>
		/// <para>
		/// The maximum level that this filter will attempt to match against the 
		/// <see cref="T:log4net.Core.LoggingEvent" /> level. If a match is found then
		/// the result depends on the value of <see cref="P:log4net.Filter.LevelRangeFilter.AcceptOnMatch" />.
		/// </para>
		/// </remarks>
		public Level LevelMax
		{
			get
			{
				return m_levelMax;
			}
			set
			{
				m_levelMax = value;
			}
		}

		/// <summary>
		/// Check if the event should be logged.
		/// </summary>
		/// <param name="loggingEvent">the logging event to check</param>
		/// <returns>see remarks</returns>
		/// <remarks>
		/// <para>
		/// If the <see cref="T:log4net.Core.Level" /> of the logging event is outside the range
		/// matched by this filter then <see cref="F:log4net.Filter.FilterDecision.Deny" />
		/// is returned. If the <see cref="T:log4net.Core.Level" /> is matched then the value of
		/// <see cref="P:log4net.Filter.LevelRangeFilter.AcceptOnMatch" /> is checked. If it is true then
		/// <see cref="F:log4net.Filter.FilterDecision.Accept" /> is returned, otherwise
		/// <see cref="F:log4net.Filter.FilterDecision.Neutral" /> is returned.
		/// </para>
		/// </remarks>
		public override FilterDecision Decide(LoggingEvent loggingEvent)
		{
			if (loggingEvent == null)
			{
				throw new ArgumentNullException("loggingEvent");
			}
			if (m_levelMin != null && loggingEvent.Level < m_levelMin)
			{
				return FilterDecision.Deny;
			}
			if (m_levelMax != null && loggingEvent.Level > m_levelMax)
			{
				return FilterDecision.Deny;
			}
			if (m_acceptOnMatch)
			{
				return FilterDecision.Accept;
			}
			return FilterDecision.Neutral;
		}
	}
}
