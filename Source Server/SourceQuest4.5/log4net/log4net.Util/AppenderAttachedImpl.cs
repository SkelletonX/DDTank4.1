using log4net.Appender;
using log4net.Core;
using System;

namespace log4net.Util
{
	/// <summary>
	/// A straightforward implementation of the <see cref="T:log4net.Core.IAppenderAttachable" /> interface.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is the default implementation of the <see cref="T:log4net.Core.IAppenderAttachable" />
	/// interface. Implementors of the <see cref="T:log4net.Core.IAppenderAttachable" /> interface
	/// should aggregate an instance of this type.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class AppenderAttachedImpl : IAppenderAttachable
	{
		/// <summary>
		/// List of appenders
		/// </summary>
		private AppenderCollection m_appenderList;

		/// <summary>
		/// Array of appenders, used to cache the m_appenderList
		/// </summary>
		private IAppender[] m_appenderArray;

		/// <summary>
		/// Gets all attached appenders.
		/// </summary>
		/// <returns>
		/// A collection of attached appenders, or <c>null</c> if there
		/// are no attached appenders.
		/// </returns>
		/// <remarks>
		/// <para>
		/// The read only collection of all currently attached appenders.
		/// </para>
		/// </remarks>
		public AppenderCollection Appenders
		{
			get
			{
				if (m_appenderList == null)
				{
					return AppenderCollection.EmptyCollection;
				}
				return AppenderCollection.ReadOnly(m_appenderList);
			}
		}

		/// <summary>
		/// Append on on all attached appenders.
		/// </summary>
		/// <param name="loggingEvent">The event being logged.</param>
		/// <returns>The number of appenders called.</returns>
		/// <remarks>
		/// <para>
		/// Calls the <see cref="M:log4net.Appender.IAppender.DoAppend(log4net.Core.LoggingEvent)" /> method on all 
		/// attached appenders.
		/// </para>
		/// </remarks>
		public int AppendLoopOnAppenders(LoggingEvent loggingEvent)
		{
			if (loggingEvent == null)
			{
				throw new ArgumentNullException("loggingEvent");
			}
			if (m_appenderList == null)
			{
				return 0;
			}
			if (m_appenderArray == null)
			{
				m_appenderArray = m_appenderList.ToArray();
			}
			IAppender[] appenderArray = m_appenderArray;
			foreach (IAppender appender in appenderArray)
			{
				try
				{
					appender.DoAppend(loggingEvent);
				}
				catch (Exception exception)
				{
					LogLog.Error("AppenderAttachedImpl: Failed to append to appender [" + appender.Name + "]", exception);
				}
			}
			return m_appenderList.Count;
		}

		/// <summary>
		/// Append on on all attached appenders.
		/// </summary>
		/// <param name="loggingEvents">The array of events being logged.</param>
		/// <returns>The number of appenders called.</returns>
		/// <remarks>
		/// <para>
		/// Calls the <see cref="M:log4net.Appender.IAppender.DoAppend(log4net.Core.LoggingEvent)" /> method on all 
		/// attached appenders.
		/// </para>
		/// </remarks>
		public int AppendLoopOnAppenders(LoggingEvent[] loggingEvents)
		{
			if (loggingEvents == null)
			{
				throw new ArgumentNullException("loggingEvents");
			}
			if (loggingEvents.Length == 0)
			{
				throw new ArgumentException("loggingEvents array must not be empty", "loggingEvents");
			}
			if (loggingEvents.Length == 1)
			{
				return AppendLoopOnAppenders(loggingEvents[0]);
			}
			if (m_appenderList == null)
			{
				return 0;
			}
			if (m_appenderArray == null)
			{
				m_appenderArray = m_appenderList.ToArray();
			}
			IAppender[] appenderArray = m_appenderArray;
			foreach (IAppender appender in appenderArray)
			{
				try
				{
					CallAppend(appender, loggingEvents);
				}
				catch (Exception exception)
				{
					LogLog.Error("AppenderAttachedImpl: Failed to append to appender [" + appender.Name + "]", exception);
				}
			}
			return m_appenderList.Count;
		}

		/// <summary>
		/// Calls the DoAppende method on the <see cref="T:log4net.Appender.IAppender" /> with 
		/// the <see cref="T:log4net.Core.LoggingEvent" /> objects supplied.
		/// </summary>
		/// <param name="appender">The appender</param>
		/// <param name="loggingEvents">The events</param>
		/// <remarks>
		/// <para>
		/// If the <paramref name="appender" /> supports the <see cref="T:log4net.Appender.IBulkAppender" />
		/// interface then the <paramref name="loggingEvents" /> will be passed 
		/// through using that interface. Otherwise the <see cref="T:log4net.Core.LoggingEvent" />
		/// objects in the array will be passed one at a time.
		/// </para>
		/// </remarks>
		private static void CallAppend(IAppender appender, LoggingEvent[] loggingEvents)
		{
			IBulkAppender bulkAppender = appender as IBulkAppender;
			if (bulkAppender != null)
			{
				bulkAppender.DoAppend(loggingEvents);
				return;
			}
			foreach (LoggingEvent loggingEvent in loggingEvents)
			{
				appender.DoAppend(loggingEvent);
			}
		}

		/// <summary>
		/// Attaches an appender.
		/// </summary>
		/// <param name="newAppender">The appender to add.</param>
		/// <remarks>
		/// <para>
		/// If the appender is already in the list it won't be added again.
		/// </para>
		/// </remarks>
		public void AddAppender(IAppender newAppender)
		{
			if (newAppender == null)
			{
				throw new ArgumentNullException("newAppender");
			}
			m_appenderArray = null;
			if (m_appenderList == null)
			{
				m_appenderList = new AppenderCollection(1);
			}
			if (!m_appenderList.Contains(newAppender))
			{
				m_appenderList.Add(newAppender);
			}
		}

		/// <summary>
		/// Gets an attached appender with the specified name.
		/// </summary>
		/// <param name="name">The name of the appender to get.</param>
		/// <returns>
		/// The appender with the name specified, or <c>null</c> if no appender with the
		/// specified name is found.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Lookup an attached appender by name.
		/// </para>
		/// </remarks>
		public IAppender GetAppender(string name)
		{
			if (m_appenderList != null && name != null)
			{
				AppenderCollection.IAppenderCollectionEnumerator enumerator = m_appenderList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						IAppender current = enumerator.Current;
						if (name == current.Name)
						{
							return current;
						}
					}
				}
				finally
				{
					(enumerator as IDisposable)?.Dispose();
				}
			}
			return null;
		}

		/// <summary>
		/// Removes all attached appenders.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Removes and closes all attached appenders
		/// </para>
		/// </remarks>
		public void RemoveAllAppenders()
		{
			if (m_appenderList != null)
			{
				AppenderCollection.IAppenderCollectionEnumerator enumerator = m_appenderList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						IAppender current = enumerator.Current;
						try
						{
							current.Close();
						}
						catch (Exception exception)
						{
							LogLog.Error("AppenderAttachedImpl: Failed to Close appender [" + current.Name + "]", exception);
						}
					}
				}
				finally
				{
					(enumerator as IDisposable)?.Dispose();
				}
				m_appenderList = null;
				m_appenderArray = null;
			}
		}

		/// <summary>
		/// Removes the specified appender from the list of attached appenders.
		/// </summary>
		/// <param name="appender">The appender to remove.</param>
		/// <returns>The appender removed from the list</returns>
		/// <remarks>
		/// <para>
		/// The appender removed is not closed.
		/// If you are discarding the appender you must call
		/// <see cref="M:log4net.Appender.IAppender.Close" /> on the appender removed.
		/// </para>
		/// </remarks>
		public IAppender RemoveAppender(IAppender appender)
		{
			if (appender != null && m_appenderList != null)
			{
				m_appenderList.Remove(appender);
				if (m_appenderList.Count == 0)
				{
					m_appenderList = null;
				}
				m_appenderArray = null;
			}
			return appender;
		}

		/// <summary>
		/// Removes the appender with the specified name from the list of appenders.
		/// </summary>
		/// <param name="name">The name of the appender to remove.</param>
		/// <returns>The appender removed from the list</returns>
		/// <remarks>
		/// <para>
		/// The appender removed is not closed.
		/// If you are discarding the appender you must call
		/// <see cref="M:log4net.Appender.IAppender.Close" /> on the appender removed.
		/// </para>
		/// </remarks>
		public IAppender RemoveAppender(string name)
		{
			return RemoveAppender(GetAppender(name));
		}
	}
}
