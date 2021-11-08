using log4net.Core;
using System;

namespace log4net.Filter
{
	/// <summary>
	/// Simple filter to match a string an event property
	/// </summary>
	/// <remarks>
	/// <para>
	/// Simple filter to match a string in the value for a
	/// specific event property
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public class PropertyFilter : StringMatchFilter
	{
		/// <summary>
		/// The key to use to lookup the string from the event properties
		/// </summary>
		private string m_key;

		/// <summary>
		/// The key to lookup in the event properties and then match against.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The key name to use to lookup in the properties map of the
		/// <see cref="T:log4net.Core.LoggingEvent" />. The match will be performed against 
		/// the value of this property if it exists.
		/// </para>
		/// </remarks>
		public string Key
		{
			get
			{
				return m_key;
			}
			set
			{
				m_key = value;
			}
		}

		/// <summary>
		/// Check if this filter should allow the event to be logged
		/// </summary>
		/// <param name="loggingEvent">the event being logged</param>
		/// <returns>see remarks</returns>
		/// <remarks>
		/// <para>
		/// The event property for the <see cref="P:log4net.Filter.PropertyFilter.Key" /> is matched against 
		/// the <see cref="P:log4net.Filter.StringMatchFilter.StringToMatch" />.
		/// If the <see cref="P:log4net.Filter.StringMatchFilter.StringToMatch" /> occurs as a substring within
		/// the property value then a match will have occurred. If no match occurs
		/// this function will return <see cref="F:log4net.Filter.FilterDecision.Neutral" />
		/// allowing other filters to check the event. If a match occurs then
		/// the value of <see cref="P:log4net.Filter.StringMatchFilter.AcceptOnMatch" /> is checked. If it is
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
			if (m_key == null)
			{
				return FilterDecision.Neutral;
			}
			object obj = loggingEvent.LookupProperty(m_key);
			string text = loggingEvent.Repository.RendererMap.FindAndRender(obj);
			if (text == null || (m_stringToMatch == null && m_regexToMatch == null))
			{
				return FilterDecision.Neutral;
			}
			if (m_regexToMatch != null)
			{
				if (!m_regexToMatch.Match(text).Success)
				{
					return FilterDecision.Neutral;
				}
				if (m_acceptOnMatch)
				{
					return FilterDecision.Accept;
				}
				return FilterDecision.Deny;
			}
			if (m_stringToMatch != null)
			{
				if (text.IndexOf(m_stringToMatch) == -1)
				{
					return FilterDecision.Neutral;
				}
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
