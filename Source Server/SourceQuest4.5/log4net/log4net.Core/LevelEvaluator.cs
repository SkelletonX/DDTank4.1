using System;

namespace log4net.Core
{
	/// <summary>
	/// An evaluator that triggers at a threshold level
	/// </summary>
	/// <remarks>
	/// <para>
	/// This evaluator will trigger if the level of the event
	/// passed to <see cref="M:log4net.Core.LevelEvaluator.IsTriggeringEvent(log4net.Core.LoggingEvent)" />
	/// is equal to or greater than the <see cref="P:log4net.Core.LevelEvaluator.Threshold" />
	/// level.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public class LevelEvaluator : ITriggeringEventEvaluator
	{
		/// <summary>
		/// The threshold for triggering
		/// </summary>
		private Level m_threshold;

		/// <summary>
		/// the threshold to trigger at
		/// </summary>
		/// <value>
		/// The <see cref="T:log4net.Core.Level" /> that will cause this evaluator to trigger
		/// </value>
		/// <remarks>
		/// <para>
		/// This evaluator will trigger if the level of the event
		/// passed to <see cref="M:log4net.Core.LevelEvaluator.IsTriggeringEvent(log4net.Core.LoggingEvent)" />
		/// is equal to or greater than the <see cref="P:log4net.Core.LevelEvaluator.Threshold" />
		/// level.
		/// </para>
		/// </remarks>
		public Level Threshold
		{
			get
			{
				return m_threshold;
			}
			set
			{
				m_threshold = value;
			}
		}

		/// <summary>
		/// Create a new evaluator using the <see cref="F:log4net.Core.Level.Off" /> threshold.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Create a new evaluator using the <see cref="F:log4net.Core.Level.Off" /> threshold.
		/// </para>
		/// <para>
		/// This evaluator will trigger if the level of the event
		/// passed to <see cref="M:log4net.Core.LevelEvaluator.IsTriggeringEvent(log4net.Core.LoggingEvent)" />
		/// is equal to or greater than the <see cref="P:log4net.Core.LevelEvaluator.Threshold" />
		/// level.
		/// </para>
		/// </remarks>
		public LevelEvaluator()
			: this(Level.Off)
		{
		}

		/// <summary>
		/// Create a new evaluator using the specified <see cref="T:log4net.Core.Level" /> threshold.
		/// </summary>
		/// <param name="threshold">the threshold to trigger at</param>
		/// <remarks>
		/// <para>
		/// Create a new evaluator using the specified <see cref="T:log4net.Core.Level" /> threshold.
		/// </para>
		/// <para>
		/// This evaluator will trigger if the level of the event
		/// passed to <see cref="M:log4net.Core.LevelEvaluator.IsTriggeringEvent(log4net.Core.LoggingEvent)" />
		/// is equal to or greater than the <see cref="P:log4net.Core.LevelEvaluator.Threshold" />
		/// level.
		/// </para>
		/// </remarks>
		public LevelEvaluator(Level threshold)
		{
			if (threshold == null)
			{
				throw new ArgumentNullException("threshold");
			}
			m_threshold = threshold;
		}

		/// <summary>
		/// Is this <paramref name="loggingEvent" /> the triggering event?
		/// </summary>
		/// <param name="loggingEvent">The event to check</param>
		/// <returns>This method returns <c>true</c>, if the event level
		/// is equal or higher than the <see cref="P:log4net.Core.LevelEvaluator.Threshold" />. 
		/// Otherwise it returns <c>false</c></returns>
		/// <remarks>
		/// <para>
		/// This evaluator will trigger if the level of the event
		/// passed to <see cref="M:log4net.Core.LevelEvaluator.IsTriggeringEvent(log4net.Core.LoggingEvent)" />
		/// is equal to or greater than the <see cref="P:log4net.Core.LevelEvaluator.Threshold" />
		/// level.
		/// </para>
		/// </remarks>
		public bool IsTriggeringEvent(LoggingEvent loggingEvent)
		{
			if (loggingEvent == null)
			{
				throw new ArgumentNullException("loggingEvent");
			}
			return loggingEvent.Level >= m_threshold;
		}
	}
}
