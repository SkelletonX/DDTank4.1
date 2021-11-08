using log4net;
using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Threading;

namespace Game.Base.Events
{
	public class RoadEventHandlerCollection
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected const int TIMEOUT = 3000;

		protected readonly ReaderWriterLock m_lock;

		protected readonly HybridDictionary m_events;

		public RoadEventHandlerCollection()
		{
			m_lock = new ReaderWriterLock();
			m_events = new HybridDictionary();
		}

		public void AddHandler(RoadEvent e, RoadEventHandler del)
		{
			try
			{
				m_lock.AcquireWriterLock(-1);
				try
				{
					WeakMulticastDelegate weakMulticastDelegate = (WeakMulticastDelegate)m_events[e];
					if (weakMulticastDelegate == null)
					{
						m_events[e] = new WeakMulticastDelegate(del);
					}
					else
					{
						m_events[e] = WeakMulticastDelegate.Combine(weakMulticastDelegate, del);
					}
				}
				finally
				{
					m_lock.ReleaseWriterLock();
				}
			}
			catch (ApplicationException exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Failed to add event handler!", exception);
				}
			}
		}

		public void AddHandlerUnique(RoadEvent e, RoadEventHandler del)
		{
			try
			{
				m_lock.AcquireWriterLock(-1);
				try
				{
					WeakMulticastDelegate weakMulticastDelegate = (WeakMulticastDelegate)m_events[e];
					if (weakMulticastDelegate == null)
					{
						m_events[e] = new WeakMulticastDelegate(del);
					}
					else
					{
						m_events[e] = WeakMulticastDelegate.CombineUnique(weakMulticastDelegate, del);
					}
				}
				finally
				{
					m_lock.ReleaseWriterLock();
				}
			}
			catch (ApplicationException exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Failed to add event handler!", exception);
				}
			}
		}

		public void RemoveHandler(RoadEvent e, RoadEventHandler del)
		{
			try
			{
				m_lock.AcquireWriterLock(-1);
				try
				{
					WeakMulticastDelegate weakMulticastDelegate = (WeakMulticastDelegate)m_events[e];
					if (weakMulticastDelegate != null)
					{
						weakMulticastDelegate = WeakMulticastDelegate.Remove(weakMulticastDelegate, del);
						if (weakMulticastDelegate == null)
						{
							m_events.Remove(e);
						}
						else
						{
							m_events[e] = weakMulticastDelegate;
						}
					}
				}
				finally
				{
					m_lock.ReleaseWriterLock();
				}
			}
			catch (ApplicationException exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Failed to remove event handler!", exception);
				}
			}
		}

		public void RemoveAllHandlers(RoadEvent e)
		{
			try
			{
				m_lock.AcquireWriterLock(-1);
				try
				{
					m_events.Remove(e);
				}
				finally
				{
					m_lock.ReleaseWriterLock();
				}
			}
			catch (ApplicationException exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Failed to remove event handlers!", exception);
				}
			}
		}

		public void RemoveAllHandlers()
		{
			try
			{
				m_lock.AcquireWriterLock(-1);
				try
				{
					m_events.Clear();
				}
				finally
				{
					m_lock.ReleaseWriterLock();
				}
			}
			catch (ApplicationException exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Failed to remove all event handlers!", exception);
				}
			}
		}

		public void Notify(RoadEvent e)
		{
			Notify(e, null, null);
		}

		public void Notify(RoadEvent e, object sender)
		{
			Notify(e, sender, null);
		}

		public void Notify(RoadEvent e, EventArgs args)
		{
			Notify(e, null, args);
		}

		public void Notify(RoadEvent e, object sender, EventArgs eArgs)
		{
			try
			{
				m_lock.AcquireReaderLock(-1);
				WeakMulticastDelegate weakMulticastDelegate;
				try
				{
					weakMulticastDelegate = (WeakMulticastDelegate)m_events[e];
				}
				finally
				{
					m_lock.ReleaseReaderLock();
				}
				weakMulticastDelegate?.InvokeSafe(new object[3]
				{
					e,
					sender,
					eArgs
				});
			}
			catch (ApplicationException exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Failed to notify event handler!", exception);
				}
			}
		}
	}
}
