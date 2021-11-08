using log4net.Core;
using log4net.Util;
using System;
using System.Collections;

namespace log4net.Appender
{
	/// <summary>
	/// Abstract base class implementation of <see cref="T:log4net.Appender.IAppender" /> that 
	/// buffers events in a fixed size buffer.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This base class should be used by appenders that need to buffer a 
	/// number of events before logging them. For example the <see cref="T:log4net.Appender.AdoNetAppender" /> 
	/// buffers events and then submits the entire contents of the buffer to 
	/// the underlying Game.Service in one go.
	/// </para>
	/// <para>
	/// Subclasses should override the <see cref="M:log4net.Appender.BufferingAppenderSkeleton.SendBuffer(log4net.Core.LoggingEvent[])" />
	/// method to deliver the buffered events.
	/// </para>
	/// <para>The BufferingAppenderSkeleton maintains a fixed size cyclic 
	/// buffer of events. The size of the buffer is set using 
	/// the <see cref="P:log4net.Appender.BufferingAppenderSkeleton.BufferSize" /> property.
	/// </para>
	/// <para>A <see cref="T:log4net.Core.ITriggeringEventEvaluator" /> is used to inspect 
	/// each event as it arrives in the appender. If the <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Evaluator" /> 
	/// triggers, then the current buffer is sent immediately 
	/// (see <see cref="M:log4net.Appender.BufferingAppenderSkeleton.SendBuffer(log4net.Core.LoggingEvent[])" />). Otherwise the event 
	/// is stored in the buffer. For example, an evaluator can be used to 
	/// deliver the events immediately when an ERROR event arrives.
	/// </para>
	/// <para>
	/// The buffering appender can be configured in a <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Lossy" /> mode. 
	/// By default the appender is NOT lossy. When the buffer is full all 
	/// the buffered events are sent with <see cref="M:log4net.Appender.BufferingAppenderSkeleton.SendBuffer(log4net.Core.LoggingEvent[])" />.
	/// If the <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Lossy" /> property is set to <c>true</c> then the 
	/// buffer will not be sent when it is full, and new events arriving 
	/// in the appender will overwrite the oldest event in the buffer. 
	/// In lossy mode the buffer will only be sent when the <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Evaluator" />
	/// triggers. This can be useful behavior when you need to know about 
	/// ERROR events but not about events with a lower level, configure an 
	/// evaluator that will trigger when an ERROR event arrives, the whole 
	/// buffer will be sent which gives a history of events leading up to
	/// the ERROR event.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public abstract class BufferingAppenderSkeleton : AppenderSkeleton
	{
		/// <summary>
		/// The default buffer size.
		/// </summary>
		/// <remarks>
		/// The default size of the cyclic buffer used to store events.
		/// This is set to 512 by default.
		/// </remarks>
		private const int DEFAULT_BUFFER_SIZE = 512;

		/// <summary>
		/// The size of the cyclic buffer used to hold the logging events.
		/// </summary>
		/// <remarks>
		/// Set to <see cref="F:log4net.Appender.BufferingAppenderSkeleton.DEFAULT_BUFFER_SIZE" /> by default.
		/// </remarks>
		private int m_bufferSize = 512;

		/// <summary>
		/// The cyclic buffer used to store the logging events.
		/// </summary>
		private CyclicBuffer m_cb;

		/// <summary>
		/// The triggering event evaluator that causes the buffer to be sent immediately.
		/// </summary>
		/// <remarks>
		/// The object that is used to determine if an event causes the entire
		/// buffer to be sent immediately. This field can be <c>null</c>, which 
		/// indicates that event triggering is not to be done. The evaluator
		/// can be set using the <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Evaluator" /> property. If this appender
		/// has the <see cref="F:log4net.Appender.BufferingAppenderSkeleton.m_lossy" /> (<see cref="P:log4net.Appender.BufferingAppenderSkeleton.Lossy" /> property) set to 
		/// <c>true</c> then an <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Evaluator" /> must be set.
		/// </remarks>
		private ITriggeringEventEvaluator m_evaluator;

		/// <summary>
		/// Indicates if the appender should overwrite events in the cyclic buffer 
		/// when it becomes full, or if the buffer should be flushed when the 
		/// buffer is full.
		/// </summary>
		/// <remarks>
		/// If this field is set to <c>true</c> then an <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Evaluator" /> must 
		/// be set.
		/// </remarks>
		private bool m_lossy = false;

		/// <summary>
		/// The triggering event evaluator filters discarded events.
		/// </summary>
		/// <remarks>
		/// The object that is used to determine if an event that is discarded should
		/// really be discarded or if it should be sent to the appenders. 
		/// This field can be <c>null</c>, which indicates that all discarded events will
		/// be discarded. 
		/// </remarks>
		private ITriggeringEventEvaluator m_lossyEvaluator;

		/// <summary>
		/// Value indicating which fields in the event should be fixed
		/// </summary>
		/// <remarks>
		/// By default all fields are fixed
		/// </remarks>
		private FixFlags m_fixFlags = FixFlags.All;

		/// <summary>
		/// The events delivered to the subclass must be fixed.
		/// </summary>
		private readonly bool m_eventMustBeFixed;

		/// <summary>
		/// Gets or sets a value that indicates whether the appender is lossy.
		/// </summary>
		/// <value>
		/// <c>true</c> if the appender is lossy, otherwise <c>false</c>. The default is <c>false</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// This appender uses a buffer to store logging events before 
		/// delivering them. A triggering event causes the whole buffer
		/// to be send to the remote sink. If the buffer overruns before
		/// a triggering event then logging events could be lost. Set
		/// <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Lossy" /> to <c>false</c> to prevent logging events 
		/// from being lost.
		/// </para>
		/// <para>If <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Lossy" /> is set to <c>true</c> then an
		/// <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Evaluator" /> must be specified.</para>
		/// </remarks>
		public bool Lossy
		{
			get
			{
				return m_lossy;
			}
			set
			{
				m_lossy = value;
			}
		}

		/// <summary>
		/// Gets or sets the size of the cyclic buffer used to hold the 
		/// logging events.
		/// </summary>
		/// <value>
		/// The size of the cyclic buffer used to hold the logging events.
		/// </value>
		/// <remarks>
		/// <para>
		/// The <see cref="P:log4net.Appender.BufferingAppenderSkeleton.BufferSize" /> option takes a positive integer
		/// representing the maximum number of logging events to collect in 
		/// a cyclic buffer. When the <see cref="P:log4net.Appender.BufferingAppenderSkeleton.BufferSize" /> is reached,
		/// oldest events are deleted as new events are added to the
		/// buffer. By default the size of the cyclic buffer is 512 events.
		/// </para>
		/// <para>
		/// If the <see cref="P:log4net.Appender.BufferingAppenderSkeleton.BufferSize" /> is set to a value less than
		/// or equal to 1 then no buffering will occur. The logging event
		/// will be delivered synchronously (depending on the <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Lossy" />
		/// and <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Evaluator" /> properties). Otherwise the event will
		/// be buffered.
		/// </para>
		/// </remarks>
		public int BufferSize
		{
			get
			{
				return m_bufferSize;
			}
			set
			{
				m_bufferSize = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="T:log4net.Core.ITriggeringEventEvaluator" /> that causes the 
		/// buffer to be sent immediately.
		/// </summary>
		/// <value>
		/// The <see cref="T:log4net.Core.ITriggeringEventEvaluator" /> that causes the buffer to be
		/// sent immediately.
		/// </value>
		/// <remarks>
		/// <para>
		/// The evaluator will be called for each event that is appended to this 
		/// appender. If the evaluator triggers then the current buffer will 
		/// immediately be sent (see <see cref="M:log4net.Appender.BufferingAppenderSkeleton.SendBuffer(log4net.Core.LoggingEvent[])" />).
		/// </para>
		/// <para>If <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Lossy" /> is set to <c>true</c> then an
		/// <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Evaluator" /> must be specified.</para>
		/// </remarks>
		public ITriggeringEventEvaluator Evaluator
		{
			get
			{
				return m_evaluator;
			}
			set
			{
				m_evaluator = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of the <see cref="T:log4net.Core.ITriggeringEventEvaluator" /> to use.
		/// </summary>
		/// <value>
		/// The value of the <see cref="T:log4net.Core.ITriggeringEventEvaluator" /> to use.
		/// </value>
		/// <remarks>
		/// <para>
		/// The evaluator will be called for each event that is discarded from this 
		/// appender. If the evaluator triggers then the current buffer will immediately 
		/// be sent (see <see cref="M:log4net.Appender.BufferingAppenderSkeleton.SendBuffer(log4net.Core.LoggingEvent[])" />).
		/// </para>
		/// </remarks>
		public ITriggeringEventEvaluator LossyEvaluator
		{
			get
			{
				return m_lossyEvaluator;
			}
			set
			{
				m_lossyEvaluator = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating if only part of the logging event data
		/// should be fixed.
		/// </summary>
		/// <value>
		/// <c>true</c> if the appender should only fix part of the logging event 
		/// data, otherwise <c>false</c>. The default is <c>false</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// Setting this property to <c>true</c> will cause only part of the
		/// event data to be fixed and serialized. This will improve performance.
		/// </para>
		/// <para>
		/// See <see cref="M:log4net.Core.LoggingEvent.FixVolatileData(log4net.Core.FixFlags)" /> for more information.
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
		/// Gets or sets a the fields that will be fixed in the event
		/// </summary>
		/// <value>
		/// The event fields that will be fixed before the event is buffered
		/// </value>
		/// <remarks>
		/// <para>
		/// The logging event needs to have certain thread specific values 
		/// captured before it can be buffered. See <see cref="P:log4net.Core.LoggingEvent.Fix" />
		/// for details.
		/// </para>
		/// </remarks>
		/// <seealso cref="P:log4net.Core.LoggingEvent.Fix" />
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
		/// Initializes a new instance of the <see cref="T:log4net.Appender.BufferingAppenderSkeleton" /> class.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Protected default constructor to allow subclassing.
		/// </para>
		/// </remarks>
		protected BufferingAppenderSkeleton()
			: this(eventMustBeFixed: true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Appender.BufferingAppenderSkeleton" /> class.
		/// </summary>
		/// <param name="eventMustBeFixed">the events passed through this appender must be
		/// fixed by the time that they arrive in the derived class' <c>SendBuffer</c> method.</param>
		/// <remarks>
		/// <para>
		/// Protected constructor to allow subclassing.
		/// </para>
		/// <para>
		/// The <paramref name="eventMustBeFixed" /> should be set if the subclass
		/// expects the events delivered to be fixed even if the 
		/// <see cref="P:log4net.Appender.BufferingAppenderSkeleton.BufferSize" /> is set to zero, i.e. when no buffering occurs.
		/// </para>
		/// </remarks>
		protected BufferingAppenderSkeleton(bool eventMustBeFixed)
		{
			m_eventMustBeFixed = eventMustBeFixed;
		}

		/// <summary>
		/// Flush the currently buffered events
		/// </summary>
		/// <remarks>
		/// <para>
		/// Flushes any events that have been buffered.
		/// </para>
		/// <para>
		/// If the appender is buffering in <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Lossy" /> mode then the contents
		/// of the buffer will NOT be flushed to the appender.
		/// </para>
		/// </remarks>
		public virtual void Flush()
		{
			Flush(flushLossyBuffer: false);
		}

		/// <summary>
		/// Flush the currently buffered events
		/// </summary>
		/// <param name="flushLossyBuffer">set to <c>true</c> to flush the buffer of lossy events</param>
		/// <remarks>
		/// <para>
		/// Flushes events that have been buffered. If <paramref name="flushLossyBuffer" /> is
		/// <c>false</c> then events will only be flushed if this buffer is non-lossy mode.
		/// </para>
		/// <para>
		/// If the appender is buffering in <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Lossy" /> mode then the contents
		/// of the buffer will only be flushed if <paramref name="flushLossyBuffer" /> is <c>true</c>.
		/// In this case the contents of the buffer will be tested against the 
		/// <see cref="P:log4net.Appender.BufferingAppenderSkeleton.LossyEvaluator" /> and if triggering will be output. All other buffered
		/// events will be discarded.
		/// </para>
		/// <para>
		/// If <paramref name="flushLossyBuffer" /> is <c>true</c> then the buffer will always
		/// be emptied by calling this method.
		/// </para>
		/// </remarks>
		public virtual void Flush(bool flushLossyBuffer)
		{
			lock (this)
			{
				if (m_cb != null && m_cb.Length > 0)
				{
					if (m_lossy)
					{
						if (flushLossyBuffer)
						{
							if (m_lossyEvaluator != null)
							{
								LoggingEvent[] array = m_cb.PopAll();
								ArrayList arrayList = new ArrayList(array.Length);
								LoggingEvent[] array2 = array;
								foreach (LoggingEvent loggingEvent in array2)
								{
									if (m_lossyEvaluator.IsTriggeringEvent(loggingEvent))
									{
										arrayList.Add(loggingEvent);
									}
								}
								if (arrayList.Count > 0)
								{
									SendBuffer((LoggingEvent[])arrayList.ToArray(typeof(LoggingEvent)));
								}
							}
							else
							{
								m_cb.Clear();
							}
						}
					}
					else
					{
						SendFromBuffer(null, m_cb);
					}
				}
			}
		}

		/// <summary>
		/// Initialize the appender based on the options set
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Appender.BufferingAppenderSkeleton.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Appender.BufferingAppenderSkeleton.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Appender.BufferingAppenderSkeleton.ActivateOptions" /> must be called again.
		/// </para>
		/// </remarks>
		public override void ActivateOptions()
		{
			base.ActivateOptions();
			if (m_lossy && m_evaluator == null)
			{
				ErrorHandler.Error("Appender [" + base.Name + "] is Lossy but has no Evaluator. The buffer will never be sent!");
			}
			if (m_bufferSize > 1)
			{
				m_cb = new CyclicBuffer(m_bufferSize);
			}
			else
			{
				m_cb = null;
			}
		}

		/// <summary>
		/// Close this appender instance.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Close this appender instance. If this appender is marked
		/// as not <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Lossy" /> then the remaining events in 
		/// the buffer must be sent when the appender is closed.
		/// </para>
		/// </remarks>
		protected override void OnClose()
		{
			Flush(flushLossyBuffer: true);
		}

		/// <summary>
		/// This method is called by the <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)" /> method. 
		/// </summary>
		/// <param name="loggingEvent">the event to log</param>
		/// <remarks>
		/// <para>
		/// Stores the <paramref name="loggingEvent" /> in the cyclic buffer.
		/// </para>
		/// <para>
		/// The buffer will be sent (i.e. passed to the <see cref="M:log4net.Appender.BufferingAppenderSkeleton.SendBuffer(log4net.Core.LoggingEvent[])" /> 
		/// method) if one of the following conditions is met:
		/// </para>
		/// <list type="bullet">
		/// 	<item>
		/// 		<description>The cyclic buffer is full and this appender is
		/// 		marked as not lossy (see <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Lossy" />)</description>
		/// 	</item>
		/// 	<item>
		/// 		<description>An <see cref="P:log4net.Appender.BufferingAppenderSkeleton.Evaluator" /> is set and
		/// 		it is triggered for the <paramref name="loggingEvent" />
		/// 		specified.</description>
		/// 	</item>
		/// </list>
		/// <para>
		/// Before the event is stored in the buffer it is fixed
		/// (see <see cref="M:log4net.Core.LoggingEvent.FixVolatileData(log4net.Core.FixFlags)" />) to ensure that
		/// any data referenced by the event will be valid when the buffer
		/// is processed.
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent loggingEvent)
		{
			if (m_cb == null || m_bufferSize <= 1)
			{
				if (!m_lossy || (m_evaluator != null && m_evaluator.IsTriggeringEvent(loggingEvent)) || (m_lossyEvaluator != null && m_lossyEvaluator.IsTriggeringEvent(loggingEvent)))
				{
					if (m_eventMustBeFixed)
					{
						loggingEvent.Fix = Fix;
					}
					SendBuffer(new LoggingEvent[1]
					{
						loggingEvent
					});
				}
				return;
			}
			loggingEvent.Fix = Fix;
			LoggingEvent loggingEvent2 = m_cb.Append(loggingEvent);
			if (loggingEvent2 != null)
			{
				if (!m_lossy)
				{
					SendFromBuffer(loggingEvent2, m_cb);
					return;
				}
				if (m_lossyEvaluator == null || !m_lossyEvaluator.IsTriggeringEvent(loggingEvent2))
				{
					loggingEvent2 = null;
				}
				if (m_evaluator != null && m_evaluator.IsTriggeringEvent(loggingEvent))
				{
					SendFromBuffer(loggingEvent2, m_cb);
				}
				else if (loggingEvent2 != null)
				{
					SendBuffer(new LoggingEvent[1]
					{
						loggingEvent2
					});
				}
			}
			else if (m_evaluator != null && m_evaluator.IsTriggeringEvent(loggingEvent))
			{
				SendFromBuffer(null, m_cb);
			}
		}

		/// <summary>
		/// Sends the contents of the buffer.
		/// </summary>
		/// <param name="firstLoggingEvent">The first logging event.</param>
		/// <param name="buffer">The buffer containing the events that need to be send.</param>
		/// <remarks>
		/// <para>
		/// The subclass must override <see cref="M:log4net.Appender.BufferingAppenderSkeleton.SendBuffer(log4net.Core.LoggingEvent[])" />.
		/// </para>
		/// </remarks>
		protected virtual void SendFromBuffer(LoggingEvent firstLoggingEvent, CyclicBuffer buffer)
		{
			LoggingEvent[] array = buffer.PopAll();
			if (firstLoggingEvent == null)
			{
				SendBuffer(array);
				return;
			}
			if (array.Length == 0)
			{
				SendBuffer(new LoggingEvent[1]
				{
					firstLoggingEvent
				});
				return;
			}
			LoggingEvent[] array2 = new LoggingEvent[array.Length + 1];
			Array.Copy(array, 0, array2, 1, array.Length);
			array2[0] = firstLoggingEvent;
			SendBuffer(array2);
		}

		/// <summary>
		/// Sends the events.
		/// </summary>
		/// <param name="events">The events that need to be send.</param>
		/// <remarks>
		/// <para>
		/// The subclass must override this method to process the buffered events.
		/// </para>
		/// </remarks>
		protected abstract void SendBuffer(LoggingEvent[] events);
	}
}
