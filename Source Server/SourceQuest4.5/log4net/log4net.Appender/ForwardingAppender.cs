using log4net.Core;
using log4net.Util;
using System;

namespace log4net.Appender
{
	/// <summary>
	/// This appender forwards logging events to attached appenders.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The forwarding appender can be used to specify different thresholds
	/// and filters for the same appender at different locations within the hierarchy.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class ForwardingAppender : AppenderSkeleton, IAppenderAttachable
	{
		/// <summary>
		/// Implementation of the <see cref="T:log4net.Core.IAppenderAttachable" /> interface
		/// </summary>
		private AppenderAttachedImpl m_appenderAttachedImpl;

		/// <summary>
		/// Gets the appenders contained in this appender as an 
		/// <see cref="T:System.Collections.ICollection" />.
		/// </summary>
		/// <remarks>
		/// If no appenders can be found, then an <see cref="T:log4net.Util.EmptyCollection" /> 
		/// is returned.
		/// </remarks>
		/// <returns>
		/// A collection of the appenders in this appender.
		/// </returns>
		public virtual AppenderCollection Appenders
		{
			get
			{
				lock (this)
				{
					if (m_appenderAttachedImpl == null)
					{
						return AppenderCollection.EmptyCollection;
					}
					return m_appenderAttachedImpl.Appenders;
				}
			}
		}

		/// <summary>
		/// Closes the appender and releases resources.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Releases any resources allocated within the appender such as file handles, 
		/// network connections, etc.
		/// </para>
		/// <para>
		/// It is a programming error to append to a closed appender.
		/// </para>
		/// </remarks>
		protected override void OnClose()
		{
			lock (this)
			{
				if (m_appenderAttachedImpl != null)
				{
					m_appenderAttachedImpl.RemoveAllAppenders();
				}
			}
		}

		/// <summary>
		/// Forward the logging event to the attached appenders 
		/// </summary>
		/// <param name="loggingEvent">The event to log.</param>
		/// <remarks>
		/// <para>
		/// Delivers the logging event to all the attached appenders.
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent loggingEvent)
		{
			if (m_appenderAttachedImpl != null)
			{
				m_appenderAttachedImpl.AppendLoopOnAppenders(loggingEvent);
			}
		}

		/// <summary>
		/// Forward the logging events to the attached appenders 
		/// </summary>
		/// <param name="loggingEvents">The array of events to log.</param>
		/// <remarks>
		/// <para>
		/// Delivers the logging events to all the attached appenders.
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent[] loggingEvents)
		{
			if (m_appenderAttachedImpl != null)
			{
				m_appenderAttachedImpl.AppendLoopOnAppenders(loggingEvents);
			}
		}

		/// <summary>
		/// Adds an <see cref="T:log4net.Appender.IAppender" /> to the list of appenders of this
		/// instance.
		/// </summary>
		/// <param name="newAppender">The <see cref="T:log4net.Appender.IAppender" /> to add to this appender.</param>
		/// <remarks>
		/// <para>
		/// If the specified <see cref="T:log4net.Appender.IAppender" /> is already in the list of
		/// appenders, then it won't be added again.
		/// </para>
		/// </remarks>
		public virtual void AddAppender(IAppender newAppender)
		{
			if (newAppender == null)
			{
				throw new ArgumentNullException("newAppender");
			}
			lock (this)
			{
				if (m_appenderAttachedImpl == null)
				{
					m_appenderAttachedImpl = new AppenderAttachedImpl();
				}
				m_appenderAttachedImpl.AddAppender(newAppender);
			}
		}

		/// <summary>
		/// Looks for the appender with the specified name.
		/// </summary>
		/// <param name="name">The name of the appender to lookup.</param>
		/// <returns>
		/// The appender with the specified name, or <c>null</c>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Get the named appender attached to this appender.
		/// </para>
		/// </remarks>
		public virtual IAppender GetAppender(string name)
		{
			lock (this)
			{
				if (m_appenderAttachedImpl == null || name == null)
				{
					return null;
				}
				return m_appenderAttachedImpl.GetAppender(name);
			}
		}

		/// <summary>
		/// Removes all previously added appenders from this appender.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is useful when re-reading configuration information.
		/// </para>
		/// </remarks>
		public virtual void RemoveAllAppenders()
		{
			lock (this)
			{
				if (m_appenderAttachedImpl != null)
				{
					m_appenderAttachedImpl.RemoveAllAppenders();
					m_appenderAttachedImpl = null;
				}
			}
		}

		/// <summary>
		/// Removes the specified appender from the list of appenders.
		/// </summary>
		/// <param name="appender">The appender to remove.</param>
		/// <returns>The appender removed from the list</returns>
		/// <remarks>
		/// The appender removed is not closed.
		/// If you are discarding the appender you must call
		/// <see cref="M:log4net.Appender.IAppender.Close" /> on the appender removed.
		/// </remarks>
		public virtual IAppender RemoveAppender(IAppender appender)
		{
			lock (this)
			{
				if (appender != null && m_appenderAttachedImpl != null)
				{
					return m_appenderAttachedImpl.RemoveAppender(appender);
				}
			}
			return null;
		}

		/// <summary>
		/// Removes the appender with the specified name from the list of appenders.
		/// </summary>
		/// <param name="name">The name of the appender to remove.</param>
		/// <returns>The appender removed from the list</returns>
		/// <remarks>
		/// The appender removed is not closed.
		/// If you are discarding the appender you must call
		/// <see cref="M:log4net.Appender.IAppender.Close" /> on the appender removed.
		/// </remarks>
		public virtual IAppender RemoveAppender(string name)
		{
			lock (this)
			{
				if (name != null && m_appenderAttachedImpl != null)
				{
					return m_appenderAttachedImpl.RemoveAppender(name);
				}
			}
			return null;
		}
	}
}
