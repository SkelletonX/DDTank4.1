using log4net.Core;
using System;
using System.Collections;
using System.Threading;

namespace log4net.Appender
{
	/// <summary>
	/// Delivers logging events to a remote logging sink. 
	/// </summary>
	/// <remarks>
	/// <para>
	/// This Appender is designed to deliver events to a remote sink. 
	/// That is any object that implements the <see cref="T:log4net.Appender.RemotingAppender.IRemoteLoggingSink" />
	/// interface. It delivers the events using .NET remoting. The
	/// object to deliver events to is specified by setting the
	/// appenders <see cref="P:log4net.Appender.RemotingAppender.Sink" /> property.</para>
	/// <para>
	/// The RemotingAppender buffers events before sending them. This allows it to 
	/// make more efficient use of the remoting infrastructure.</para>
	/// <para>
	/// Once the buffer is full the events are still not sent immediately. 
	/// They are scheduled to be sent using a pool thread. The effect is that 
	/// the send occurs asynchronously. This is very important for a 
	/// number of non obvious reasons. The remoting infrastructure will 
	/// flow thread local variables (stored in the <see cref="T:System.Runtime.Remoting.Messaging.CallContext" />),
	/// if they are marked as <see cref="T:System.Runtime.Remoting.Messaging.ILogicalThreadAffinative" />, across the 
	/// remoting boundary. If the server is not contactable then
	/// the remoting infrastructure will clear the <see cref="T:System.Runtime.Remoting.Messaging.ILogicalThreadAffinative" />
	/// objects from the <see cref="T:System.Runtime.Remoting.Messaging.CallContext" />. To prevent a logging failure from
	/// having side effects on the calling application the remoting call must be made
	/// from a separate thread to the one used by the application. A <see cref="T:System.Threading.ThreadPool" />
	/// thread is used for this. If no <see cref="T:System.Threading.ThreadPool" /> thread is available then
	/// the events will block in the thread pool manager until a thread is available.</para>
	/// <para>
	/// Because the events are sent asynchronously using pool threads it is possible to close 
	/// this appender before all the queued events have been sent.
	/// When closing the appender attempts to wait until all the queued events have been sent, but 
	/// this will timeout after 30 seconds regardless.</para>
	/// <para>
	/// If this appender is being closed because the <see cref="E:System.AppDomain.ProcessExit" />
	/// event has fired it may not be possible to send all the queued events. During process
	/// exit the runtime limits the time that a <see cref="E:System.AppDomain.ProcessExit" />
	/// event handler is allowed to run for. If the runtime terminates the threads before
	/// the queued events have been sent then they will be lost. To ensure that all events
	/// are sent the appender must be closed before the application exits. See 
	/// <see cref="M:log4net.Core.LoggerManager.Shutdown" /> for details on how to shutdown
	/// log4net programmatically.</para>
	/// </remarks>
	/// <seealso cref="T:log4net.Appender.RemotingAppender.IRemoteLoggingSink" />
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	/// <author>Daniel Cazzulino</author>
	public class RemotingAppender : BufferingAppenderSkeleton
	{
		/// <summary>
		/// Interface used to deliver <see cref="T:log4net.Core.LoggingEvent" /> objects to a remote sink.
		/// </summary>
		/// <remarks>
		/// This interface must be implemented by a remoting sink
		/// if the <see cref="T:log4net.Appender.RemotingAppender" /> is to be used
		/// to deliver logging events to the sink.
		/// </remarks>
		public interface IRemoteLoggingSink
		{
			/// <summary>
			/// Delivers logging events to the remote sink
			/// </summary>
			/// <param name="events">Array of events to log.</param>
			/// <remarks>
			/// <para>
			/// Delivers logging events to the remote sink
			/// </para>
			/// </remarks>
			void LogEvents(LoggingEvent[] events);
		}

		/// <summary>
		/// The URL of the remote sink.
		/// </summary>
		private string m_sinkUrl;

		/// <summary>
		/// The local proxy (.NET remoting) for the remote logging sink.
		/// </summary>
		private IRemoteLoggingSink m_sinkObj;

		/// <summary>
		/// The number of queued callbacks currently waiting or executing
		/// </summary>
		private int m_queuedCallbackCount = 0;

		/// <summary>
		/// Event used to signal when there are no queued work items
		/// </summary>
		/// <remarks>
		/// This event is set when there are no queued work items. In this
		/// state it is safe to close the appender.
		/// </remarks>
		private ManualResetEvent m_workQueueEmptyEvent = new ManualResetEvent(initialState: true);

		/// <summary>
		/// Gets or sets the URL of the well-known object that will accept 
		/// the logging events.
		/// </summary>
		/// <value>
		/// The well-known URL of the remote sink.
		/// </value>
		/// <remarks>
		/// <para>
		/// The URL of the remoting sink that will accept logging events.
		/// The sink must implement the <see cref="T:log4net.Appender.RemotingAppender.IRemoteLoggingSink" />
		/// interface.
		/// </para>
		/// </remarks>
		public string Sink
		{
			get
			{
				return m_sinkUrl;
			}
			set
			{
				m_sinkUrl = value;
			}
		}

		/// <summary>
		/// Initialize the appender based on the options set
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Appender.RemotingAppender.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Appender.RemotingAppender.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Appender.RemotingAppender.ActivateOptions" /> must be called again.
		/// </para>
		/// </remarks>
		public override void ActivateOptions()
		{
			base.ActivateOptions();
			IDictionary dictionary = new Hashtable();
			dictionary["typeFilterLevel"] = "Full";
			m_sinkObj = (IRemoteLoggingSink)Activator.GetObject(typeof(IRemoteLoggingSink), m_sinkUrl, dictionary);
		}

		/// <summary>
		/// Send the contents of the buffer to the remote sink.
		/// </summary>
		/// <remarks>
		/// The events are not sent immediately. They are scheduled to be sent
		/// using a pool thread. The effect is that the send occurs asynchronously.
		/// This is very important for a number of non obvious reasons. The remoting
		/// infrastructure will flow thread local variables (stored in the <see cref="T:System.Runtime.Remoting.Messaging.CallContext" />),
		/// if they are marked as <see cref="T:System.Runtime.Remoting.Messaging.ILogicalThreadAffinative" />, across the 
		/// remoting boundary. If the server is not contactable then
		/// the remoting infrastructure will clear the <see cref="T:System.Runtime.Remoting.Messaging.ILogicalThreadAffinative" />
		/// objects from the <see cref="T:System.Runtime.Remoting.Messaging.CallContext" />. To prevent a logging failure from
		/// having side effects on the calling application the remoting call must be made
		/// from a separate thread to the one used by the application. A <see cref="T:System.Threading.ThreadPool" />
		/// thread is used for this. If no <see cref="T:System.Threading.ThreadPool" /> thread is available then
		/// the events will block in the thread pool manager until a thread is available.
		/// </remarks>
		/// <param name="events">The events to send.</param>
		protected override void SendBuffer(LoggingEvent[] events)
		{
			BeginAsyncSend();
			if (!ThreadPool.QueueUserWorkItem(SendBufferCallback, events))
			{
				EndAsyncSend();
				ErrorHandler.Error("RemotingAppender [" + base.Name + "] failed to ThreadPool.QueueUserWorkItem logging events in SendBuffer.");
			}
		}

		/// <summary>
		/// Override base class close.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method waits while there are queued work items. The events are
		/// sent asynchronously using <see cref="T:System.Threading.ThreadPool" /> work items. These items
		/// will be sent once a thread pool thread is available to send them, therefore
		/// it is possible to close the appender before all the queued events have been
		/// sent.</para>
		/// <para>
		/// This method attempts to wait until all the queued events have been sent, but this 
		/// method will timeout after 30 seconds regardless.</para>
		/// <para>
		/// If the appender is being closed because the <see cref="E:System.AppDomain.ProcessExit" />
		/// event has fired it may not be possible to send all the queued events. During process
		/// exit the runtime limits the time that a <see cref="E:System.AppDomain.ProcessExit" />
		/// event handler is allowed to run for.</para>
		/// </remarks>
		protected override void OnClose()
		{
			base.OnClose();
			if (!m_workQueueEmptyEvent.WaitOne(30000, exitContext: false))
			{
				ErrorHandler.Error("RemotingAppender [" + base.Name + "] failed to send all queued events before close, in OnClose.");
			}
		}

		/// <summary>
		/// A work item is being queued into the thread pool
		/// </summary>
		private void BeginAsyncSend()
		{
			m_workQueueEmptyEvent.Reset();
			Interlocked.Increment(ref m_queuedCallbackCount);
		}

		/// <summary>
		/// A work item from the thread pool has completed
		/// </summary>
		private void EndAsyncSend()
		{
			if (Interlocked.Decrement(ref m_queuedCallbackCount) <= 0)
			{
				m_workQueueEmptyEvent.Set();
			}
		}

		/// <summary>
		/// Send the contents of the buffer to the remote sink.
		/// </summary>
		/// <remarks>
		/// This method is designed to be used with the <see cref="T:System.Threading.ThreadPool" />.
		/// This method expects to be passed an array of <see cref="T:log4net.Core.LoggingEvent" />
		/// objects in the state param.
		/// </remarks>
		/// <param name="state">the logging events to send</param>
		private void SendBufferCallback(object state)
		{
			try
			{
				LoggingEvent[] events = (LoggingEvent[])state;
				m_sinkObj.LogEvents(events);
			}
			catch (Exception e)
			{
				ErrorHandler.Error("Failed in SendBufferCallback", e);
			}
			finally
			{
				EndAsyncSend();
			}
		}
	}
}
