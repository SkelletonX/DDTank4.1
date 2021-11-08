using log4net.Core;
using System;

namespace log4net.Util
{
	/// <summary>
	/// A fixed size rolling buffer of logging events.
	/// </summary>
	/// <remarks>
	/// <para>
	/// An array backed fixed size leaky bucket.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class CyclicBuffer
	{
		private LoggingEvent[] m_events;

		private int m_first;

		private int m_last;

		private int m_numElems;

		private int m_maxSize;

		/// <summary>
		/// Gets the <paramref name="i" />th oldest event currently in the buffer.
		/// </summary>
		/// <value>The <paramref name="i" />th oldest event currently in the buffer.</value>
		/// <remarks>
		/// <para>
		/// If <paramref name="i" /> is outside the range 0 to the number of events
		/// currently in the buffer, then <c>null</c> is returned.
		/// </para>
		/// </remarks>
		public LoggingEvent this[int i]
		{
			get
			{
				lock (this)
				{
					if (i < 0 || i >= m_numElems)
					{
						return null;
					}
					return m_events[(m_first + i) % m_maxSize];
				}
			}
		}

		/// <summary>
		/// Gets the maximum size of the buffer.
		/// </summary>
		/// <value>The maximum size of the buffer.</value>
		/// <remarks>
		/// <para>
		/// Gets the maximum size of the buffer
		/// </para>
		/// </remarks>
		public int MaxSize
		{
			get
			{
				lock (this)
				{
					return m_maxSize;
				}
			}
		}

		/// <summary>
		/// Gets the number of logging events in the buffer.
		/// </summary>
		/// <value>The number of logging events in the buffer.</value>
		/// <remarks>
		/// <para>
		/// This number is guaranteed to be in the range 0 to <see cref="P:log4net.Util.CyclicBuffer.MaxSize" />
		/// (inclusive).
		/// </para>
		/// </remarks>
		public int Length
		{
			get
			{
				lock (this)
				{
					return m_numElems;
				}
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="maxSize">The maximum number of logging events in the buffer.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.CyclicBuffer" /> class with 
		/// the specified maximum number of buffered logging events.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="maxSize" /> argument is not a positive integer.</exception>
		public CyclicBuffer(int maxSize)
		{
			if (maxSize < 1)
			{
				throw SystemInfo.CreateArgumentOutOfRangeException("maxSize", maxSize, "Parameter: maxSize, Value: [" + maxSize + "] out of range. Non zero positive integer required");
			}
			m_maxSize = maxSize;
			m_events = new LoggingEvent[maxSize];
			m_first = 0;
			m_last = 0;
			m_numElems = 0;
		}

		/// <summary>
		/// Appends a <paramref name="loggingEvent" /> to the buffer.
		/// </summary>
		/// <param name="loggingEvent">The event to append to the buffer.</param>
		/// <returns>The event discarded from the buffer, if the buffer is full, otherwise <c>null</c>.</returns>
		/// <remarks>
		/// <para>
		/// Append an event to the buffer. If the buffer still contains free space then
		/// <c>null</c> is returned. If the buffer is full then an event will be dropped
		/// to make space for the new event, the event dropped is returned.
		/// </para>
		/// </remarks>
		public LoggingEvent Append(LoggingEvent loggingEvent)
		{
			if (loggingEvent == null)
			{
				throw new ArgumentNullException("loggingEvent");
			}
			lock (this)
			{
				LoggingEvent result = m_events[m_last];
				m_events[m_last] = loggingEvent;
				if (++m_last == m_maxSize)
				{
					m_last = 0;
				}
				if (m_numElems < m_maxSize)
				{
					m_numElems++;
				}
				else if (++m_first == m_maxSize)
				{
					m_first = 0;
				}
				if (m_numElems < m_maxSize)
				{
					return null;
				}
				return result;
			}
		}

		/// <summary>
		/// Get and remove the oldest event in the buffer.
		/// </summary>
		/// <returns>The oldest logging event in the buffer</returns>
		/// <remarks>
		/// <para>
		/// Gets the oldest (first) logging event in the buffer and removes it 
		/// from the buffer.
		/// </para>
		/// </remarks>
		public LoggingEvent PopOldest()
		{
			lock (this)
			{
				LoggingEvent result = null;
				if (m_numElems > 0)
				{
					m_numElems--;
					result = m_events[m_first];
					m_events[m_first] = null;
					if (++m_first == m_maxSize)
					{
						m_first = 0;
					}
				}
				return result;
			}
		}

		/// <summary>
		/// Pops all the logging events from the buffer into an array.
		/// </summary>
		/// <returns>An array of all the logging events in the buffer.</returns>
		/// <remarks>
		/// <para>
		/// Get all the events in the buffer and clear the buffer.
		/// </para>
		/// </remarks>
		public LoggingEvent[] PopAll()
		{
			lock (this)
			{
				LoggingEvent[] array = new LoggingEvent[m_numElems];
				if (m_numElems > 0)
				{
					if (m_first < m_last)
					{
						Array.Copy(m_events, m_first, array, 0, m_numElems);
					}
					else
					{
						Array.Copy(m_events, m_first, array, 0, m_maxSize - m_first);
						Array.Copy(m_events, 0, array, m_maxSize - m_first, m_last);
					}
				}
				Clear();
				return array;
			}
		}

		/// <summary>
		/// Clear the buffer
		/// </summary>
		/// <remarks>
		/// <para>
		/// Clear the buffer of all events. The events in the buffer are lost.
		/// </para>
		/// </remarks>
		public void Clear()
		{
			lock (this)
			{
				Array.Clear(m_events, 0, m_events.Length);
				m_first = 0;
				m_last = 0;
				m_numElems = 0;
			}
		}
	}
}
