using log4net.Core;
using log4net.Util;
using System;
using System.IO;

namespace log4net.Layout.Pattern
{
	/// <summary>
	/// Abstract class that provides the formatting functionality that 
	/// derived classes need.
	/// </summary>
	/// <remarks>
	/// Conversion specifiers in a conversion patterns are parsed to
	/// individual PatternConverters. Each of which is responsible for
	/// converting a logging event in a converter specific manner.
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public abstract class PatternLayoutConverter : PatternConverter
	{
		/// <summary>
		/// Flag indicating if this converter handles exceptions
		/// </summary>
		/// <remarks>
		/// <c>false</c> if this converter handles exceptions
		/// </remarks>
		private bool m_ignoresException = true;

		/// <summary>
		/// Flag indicating if this converter handles the logging event exception
		/// </summary>
		/// <value><c>false</c> if this converter handles the logging event exception</value>
		/// <remarks>
		/// <para>
		/// If this converter handles the exception object contained within
		/// <see cref="T:log4net.Core.LoggingEvent" />, then this property should be set to
		/// <c>false</c>. Otherwise, if the layout ignores the exception
		/// object, then the property should be set to <c>true</c>.
		/// </para>
		/// <para>
		/// Set this value to override a this default setting. The default
		/// value is <c>true</c>, this converter does not handle the exception.
		/// </para>
		/// </remarks>
		public virtual bool IgnoresException
		{
			get
			{
				return m_ignoresException;
			}
			set
			{
				m_ignoresException = value;
			}
		}

		/// <summary>
		/// Derived pattern converters must override this method in order to
		/// convert conversion specifiers in the correct way.
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">The <see cref="T:log4net.Core.LoggingEvent" /> on which the pattern converter should be executed.</param>
		protected abstract void Convert(TextWriter writer, LoggingEvent loggingEvent);

		/// <summary>
		/// Derived pattern converters must override this method in order to
		/// convert conversion specifiers in the correct way.
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="state">The state object on which the pattern converter should be executed.</param>
		protected override void Convert(TextWriter writer, object state)
		{
			LoggingEvent loggingEvent = state as LoggingEvent;
			if (loggingEvent != null)
			{
				Convert(writer, loggingEvent);
				return;
			}
			throw new ArgumentException("state must be of type [" + typeof(LoggingEvent).FullName + "]", "state");
		}
	}
}
