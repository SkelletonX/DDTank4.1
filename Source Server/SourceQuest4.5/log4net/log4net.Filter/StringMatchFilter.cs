using log4net.Core;
using System;
using System.Text.RegularExpressions;

namespace log4net.Filter
{
	/// <summary>
	/// Simple filter to match a string in the rendered message
	/// </summary>
	/// <remarks>
	/// <para>
	/// Simple filter to match a string in the rendered message
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class StringMatchFilter : FilterSkeleton
	{
		/// <summary>
		/// Flag to indicate the behavior when we have a match
		/// </summary>
		protected bool m_acceptOnMatch = true;

		/// <summary>
		/// The string to substring match against the message
		/// </summary>
		protected string m_stringToMatch;

		/// <summary>
		/// A string regex to match
		/// </summary>
		protected string m_stringRegexToMatch;

		/// <summary>
		/// A regex object to match (generated from m_stringRegexToMatch)
		/// </summary>
		protected Regex m_regexToMatch;

		/// <summary>
		/// <see cref="F:log4net.Filter.FilterDecision.Accept" /> when matching <see cref="P:log4net.Filter.StringMatchFilter.StringToMatch" /> or <see cref="P:log4net.Filter.StringMatchFilter.RegexToMatch" />
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="P:log4net.Filter.StringMatchFilter.AcceptOnMatch" /> property is a flag that determines
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
		/// Sets the static string to match
		/// </summary>
		/// <remarks>
		/// <para>
		/// The string that will be substring matched against
		/// the rendered message. If the message contains this
		/// string then the filter will match. If a match is found then
		/// the result depends on the value of <see cref="P:log4net.Filter.StringMatchFilter.AcceptOnMatch" />.
		/// </para>
		/// <para>
		/// One of <see cref="P:log4net.Filter.StringMatchFilter.StringToMatch" /> or <see cref="P:log4net.Filter.StringMatchFilter.RegexToMatch" />
		/// must be specified.
		/// </para>
		/// </remarks>
		public string StringToMatch
		{
			get
			{
				return m_stringToMatch;
			}
			set
			{
				m_stringToMatch = value;
			}
		}

		/// <summary>
		/// Sets the regular expression to match
		/// </summary>
		/// <remarks>
		/// <para>
		/// The regular expression pattern that will be matched against
		/// the rendered message. If the message matches this
		/// pattern then the filter will match. If a match is found then
		/// the result depends on the value of <see cref="P:log4net.Filter.StringMatchFilter.AcceptOnMatch" />.
		/// </para>
		/// <para>
		/// One of <see cref="P:log4net.Filter.StringMatchFilter.StringToMatch" /> or <see cref="P:log4net.Filter.StringMatchFilter.RegexToMatch" />
		/// must be specified.
		/// </para>
		/// </remarks>
		public string RegexToMatch
		{
			get
			{
				return m_stringRegexToMatch;
			}
			set
			{
				m_stringRegexToMatch = value;
			}
		}

		/// <summary>
		/// Initialize and precompile the Regex if required
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Filter.StringMatchFilter.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Filter.StringMatchFilter.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Filter.StringMatchFilter.ActivateOptions" /> must be called again.
		/// </para>
		/// </remarks>
		public override void ActivateOptions()
		{
			if (m_stringRegexToMatch != null)
			{
				m_regexToMatch = new Regex(m_stringRegexToMatch, RegexOptions.Compiled);
			}
		}

		/// <summary>
		/// Check if this filter should allow the event to be logged
		/// </summary>
		/// <param name="loggingEvent">the event being logged</param>
		/// <returns>see remarks</returns>
		/// <remarks>
		/// <para>
		/// The rendered message is matched against the <see cref="P:log4net.Filter.StringMatchFilter.StringToMatch" />.
		/// If the <see cref="P:log4net.Filter.StringMatchFilter.StringToMatch" /> occurs as a substring within
		/// the message then a match will have occurred. If no match occurs
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
			string renderedMessage = loggingEvent.RenderedMessage;
			if (renderedMessage == null || (m_stringToMatch == null && m_regexToMatch == null))
			{
				return FilterDecision.Neutral;
			}
			if (m_regexToMatch != null)
			{
				if (!m_regexToMatch.Match(renderedMessage).Success)
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
				if (renderedMessage.IndexOf(m_stringToMatch) == -1)
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
