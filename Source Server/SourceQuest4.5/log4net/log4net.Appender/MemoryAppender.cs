using log4net.Core;
using System;
using System.Collections;

namespace log4net.Appender
{
	/// <summary>
	/// Stores logging events in an array.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The memory appender stores all the logging events
	/// that are appended in an in-memory array.
	/// </para>
	/// <para>
	/// Use the <see cref="M:log4net.Appender.MemoryAppender.GetEvents" /> method to get
	/// the current list of events that have been appended.
	/// </para>
	/// <para>
	/// Use the <see cref="M:log4net.Appender.MemoryAppender.Clear" /> method to clear the
	/// current list of events.
	/// </para>
	/// </remarks>
	/// <author>Julian Biddle</author>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class MemoryAppender : AppenderSkeleton
	{
		/// <summary>
		/// The list of events that have been appended.
		/// </summary>
		protected ArrayList m_eventsList;

		/// <summary>
		/// Value indicating which fields in the event should be fixed
		/// </summary>
		/// <remarks>
		/// By default all fields are fixed
		/// </remarks>
		protected FixFlags m_fixFlags = FixFlags.All;

		/// <summary>
		/// Gets or sets a value indicating whether only part of the logging event 
		/// data should be fixed.
		/// </summary>
		/// <value>
		/// <c>true</c> if the appender should only fix part of the logging event 
		/// data, otherwise <c>false</c>. The default is <c>false</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// Setting this property to <c>true</c> will cause only part of the event 
		/// data to be fixed and stored in the appender, hereby improving performance. 
		/// </para>
		/// <para>
		/// See <see cref="M:log4net.Core.LoggingEvent.FixVolatileData(System.Boolean)" /> for more information.
		/// </para>
		/// </remarks>
		[Obsolete("Use Fix property")]
		public virtual bool OnlyFixPartialEventData
		{
			get
			{
				return Fix == FixFlags.Partial;
			}
			set
			{
				if (value)
				{
					Fix = FixFlags.Partial;
				}
				else
				{
					Fix = FixFlags.All;
				}
			}
		}

		/// <summary>
		/// Gets or sets the fields that will be fixed in the event
		/// </summary>
		/// <remarks>
		/// <para>
		/// The logging event needs to have certain thread specific values 
		/// captured before it can be buffered. See <see cref="P:log4net.Core.LoggingEvent.Fix" />
		/// for details.
		/// </para>
		/// </remarks>
		public virtual FixFlags Fix
		{
			get
			{
				return m_fixFlags;
			}
			set
			{
				m_fixFlags = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Appender.MemoryAppender" /> class.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Default constructor.
		/// </para>
		/// </remarks>
		public MemoryAppender()
		{
			m_eventsList = new ArrayList();
		}

		/// <summary>
		/// Gets the events that have been logged.
		/// </summary>
		/// <returns>The events that have been logged</returns>
		/// <remarks>
		/// <para>
		/// Gets the events that have been logged.
		/// </para>
		/// </remarks>
		public virtual LoggingEvent[] GetEvents()
		{
			return (LoggingEvent[])m_eventsList.ToArray(typeof(LoggingEvent));
		}

		/// <summary>
		/// This method is called by the <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)" /> method. 
		/// </summary>
		/// <param name="loggingEvent">the event to log</param>
		/// <remarks>
		/// <para>Stores the <paramref name="loggingEvent" /> in the events list.</para>
		/// </remarks>
		protected override void Append(LoggingEvent loggingEvent)
		{
			loggingEvent.Fix = Fix;
			m_eventsList.Add(loggingEvent);
		}

		/// <summary>
		/// Clear the list of events
		/// </summary>
		/// <remarks>
		/// Clear the list of events
		/// </remarks>
		public virtual void Clear()
		{
			m_eventsList.Clear();
		}
	}
}
