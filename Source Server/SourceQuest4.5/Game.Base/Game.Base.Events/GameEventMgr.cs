using log4net;
using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Threading;

namespace Game.Base.Events
{
	public sealed class GameEventMgr
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static readonly HybridDictionary m_GameObjectEventCollections = new HybridDictionary();

		private static readonly ReaderWriterLock m_lock = new ReaderWriterLock();

		private const int TIMEOUT = 3000;

		private static RoadEventHandlerCollection m_GlobalHandlerCollection = new RoadEventHandlerCollection();

		public static void RegisterGlobalEvents(Assembly asm, Type attribute, RoadEvent e)
		{
			if (asm == null)
			{
				throw new ArgumentNullException("asm", "No assembly given to search for global event handlers!");
			}
			if (attribute == null)
			{
				throw new ArgumentNullException("attribute", "No attribute given!");
			}
			if (e == null)
			{
				throw new ArgumentNullException("e", "No event type given!");
			}
			Type[] types = asm.GetTypes();
			foreach (Type type in types)
			{
				if (!type.IsClass)
				{
					continue;
				}
				MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
				foreach (MethodInfo methodInfo in methods)
				{
					if (methodInfo.GetCustomAttributes(attribute, inherit: false).Length != 0)
					{
						try
						{
							m_GlobalHandlerCollection.AddHandler(e, (RoadEventHandler)Delegate.CreateDelegate(typeof(RoadEventHandler), methodInfo));
						}
						catch (Exception exception)
						{
							if (log.IsErrorEnabled)
							{
								log.Error("Error registering global event. Method: " + type.FullName + "." + methodInfo.Name, exception);
							}
						}
					}
				}
			}
		}

		public static void AddHandler(RoadEvent e, RoadEventHandler del)
		{
			AddHandler(e, del, unique: false);
		}

		public static void AddHandlerUnique(RoadEvent e, RoadEventHandler del)
		{
			AddHandler(e, del, unique: true);
		}

		private static void AddHandler(RoadEvent e, RoadEventHandler del, bool unique)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e", "No event type given!");
			}
			if (del == null)
			{
				throw new ArgumentNullException("del", "No event handler given!");
			}
			if (unique)
			{
				m_GlobalHandlerCollection.AddHandlerUnique(e, del);
			}
			else
			{
				m_GlobalHandlerCollection.AddHandler(e, del);
			}
		}

		public static void AddHandler(object obj, RoadEvent e, RoadEventHandler del)
		{
			AddHandler(obj, e, del, unique: false);
		}

		public static void AddHandlerUnique(object obj, RoadEvent e, RoadEventHandler del)
		{
			AddHandler(obj, e, del, unique: true);
		}

		private static void AddHandler(object obj, RoadEvent e, RoadEventHandler del, bool unique)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", "No object given!");
			}
			if (e == null)
			{
				throw new ArgumentNullException("e", "No event type given!");
			}
			if (del == null)
			{
				throw new ArgumentNullException("del", "No event handler given!");
			}
			if (!e.IsValidFor(obj))
			{
				throw new ArgumentException("Object is not valid for this event type", "obj");
			}
			try
			{
				m_lock.AcquireReaderLock(-1);
				try
				{
					RoadEventHandlerCollection roadEventHandlerCollection = (RoadEventHandlerCollection)m_GameObjectEventCollections[obj];
					if (roadEventHandlerCollection == null)
					{
						roadEventHandlerCollection = new RoadEventHandlerCollection();
						LockCookie lockCookie = m_lock.UpgradeToWriterLock(-1);
						try
						{
							m_GameObjectEventCollections[obj] = roadEventHandlerCollection;
						}
						finally
						{
							m_lock.DowngradeFromWriterLock(ref lockCookie);
						}
					}
					if (unique)
					{
						roadEventHandlerCollection.AddHandlerUnique(e, del);
					}
					else
					{
						roadEventHandlerCollection.AddHandler(e, del);
					}
				}
				finally
				{
					m_lock.ReleaseReaderLock();
				}
			}
			catch (ApplicationException exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Failed to add local event handler!", exception);
				}
			}
		}

		public static void RemoveHandler(RoadEvent e, RoadEventHandler del)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e", "No event type given!");
			}
			if (del == null)
			{
				throw new ArgumentNullException("del", "No event handler given!");
			}
			m_GlobalHandlerCollection.RemoveHandler(e, del);
		}

		public static void RemoveHandler(object obj, RoadEvent e, RoadEventHandler del)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", "No object given!");
			}
			if (e == null)
			{
				throw new ArgumentNullException("e", "No event type given!");
			}
			if (del == null)
			{
				throw new ArgumentNullException("del", "No event handler given!");
			}
			try
			{
				m_lock.AcquireReaderLock(-1);
				try
				{
					((RoadEventHandlerCollection)m_GameObjectEventCollections[obj])?.RemoveHandler(e, del);
				}
				finally
				{
					m_lock.ReleaseReaderLock();
				}
			}
			catch (ApplicationException exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Failed to remove local event handler!", exception);
				}
			}
		}

		public static void RemoveAllHandlers(RoadEvent e, bool deep)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e", "No event type given!");
			}
			if (deep)
			{
				try
				{
					m_lock.AcquireReaderLock(-1);
					try
					{
						foreach (RoadEventHandlerCollection value in m_GameObjectEventCollections.Values)
						{
							value.RemoveAllHandlers(e);
						}
					}
					finally
					{
						m_lock.ReleaseReaderLock();
					}
				}
				catch (ApplicationException exception)
				{
					if (log.IsErrorEnabled)
					{
						log.Error("Failed to add local event handlers!", exception);
					}
				}
			}
			m_GlobalHandlerCollection.RemoveAllHandlers(e);
		}

		public static void RemoveAllHandlers(object obj, RoadEvent e)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", "No object given!");
			}
			if (e == null)
			{
				throw new ArgumentNullException("e", "No event type given!");
			}
			try
			{
				m_lock.AcquireReaderLock(-1);
				try
				{
					((RoadEventHandlerCollection)m_GameObjectEventCollections[obj])?.RemoveAllHandlers(e);
				}
				finally
				{
					m_lock.ReleaseReaderLock();
				}
			}
			catch (ApplicationException exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Failed to remove local event handlers!", exception);
				}
			}
		}

		public static void RemoveAllHandlers(bool deep)
		{
			if (deep)
			{
				try
				{
					m_lock.AcquireWriterLock(-1);
					try
					{
						m_GameObjectEventCollections.Clear();
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
						log.Error("Failed to remove all local event handlers!", exception);
					}
				}
			}
			m_GlobalHandlerCollection.RemoveAllHandlers();
		}

		public static void Notify(RoadEvent e)
		{
			Notify(e, null, null);
		}

		public static void Notify(RoadEvent e, object sender)
		{
			Notify(e, sender, null);
		}

		public static void Notify(RoadEvent e, EventArgs args)
		{
			Notify(e, null, args);
		}

		public static void Notify(RoadEvent e, object sender, EventArgs eArgs)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e", "No event type given!");
			}
			if (sender != null)
			{
				try
				{
					RoadEventHandlerCollection roadEventHandlerCollection = null;
					m_lock.AcquireReaderLock(-1);
					try
					{
						roadEventHandlerCollection = (RoadEventHandlerCollection)m_GameObjectEventCollections[sender];
					}
					finally
					{
						m_lock.ReleaseReaderLock();
					}
					roadEventHandlerCollection?.Notify(e, sender, eArgs);
				}
				catch (ApplicationException exception)
				{
					if (log.IsErrorEnabled)
					{
						log.Error("Failed to notify local event handler!", exception);
					}
				}
			}
			m_GlobalHandlerCollection.Notify(e, sender, eArgs);
		}
	}
}
