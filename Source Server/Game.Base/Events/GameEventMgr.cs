// Decompiled with JetBrains decompiler
// Type: Game.Base.Events.GameEventMgr
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using log4net;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Threading;

namespace Game.Base.Events
{
  public sealed class GameEventMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static readonly HybridDictionary m_GameObjectEventCollections = new HybridDictionary();
    private static RoadEventHandlerCollection m_GlobalHandlerCollection = new RoadEventHandlerCollection();
    private static readonly ReaderWriterLock m_lock = new ReaderWriterLock();
    private const int TIMEOUT = 3000;

    public static void AddHandler(RoadEvent e, RoadEventHandler del)
    {
      GameEventMgr.AddHandler(e, del, false);
    }

    private static void AddHandler(RoadEvent e, RoadEventHandler del, bool unique)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e), "No event type given!");
      if (del == null)
        throw new ArgumentNullException(nameof (del), "No event handler given!");
      if (unique)
        GameEventMgr.m_GlobalHandlerCollection.AddHandlerUnique(e, del);
      else
        GameEventMgr.m_GlobalHandlerCollection.AddHandler(e, del);
    }

    public static void AddHandler(object obj, RoadEvent e, RoadEventHandler del)
    {
      GameEventMgr.AddHandler(obj, e, del, false);
    }

    private static void AddHandler(object obj, RoadEvent e, RoadEventHandler del, bool unique)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj), "No object given!");
      if (e == null)
        throw new ArgumentNullException(nameof (e), "No event type given!");
      if (del == null)
        throw new ArgumentNullException(nameof (del), "No event handler given!");
      if (!e.IsValidFor(obj))
        throw new ArgumentException("Object is not valid for this event type", nameof (obj));
      try
      {
        GameEventMgr.m_lock.AcquireReaderLock(3000);
        try
        {
          RoadEventHandlerCollection handlerCollection = (RoadEventHandlerCollection) GameEventMgr.m_GameObjectEventCollections[obj];
          if (handlerCollection == null)
          {
            handlerCollection = new RoadEventHandlerCollection();
            LockCookie writerLock = GameEventMgr.m_lock.UpgradeToWriterLock(3000);
            try
            {
              GameEventMgr.m_GameObjectEventCollections[obj] = (object) handlerCollection;
            }
            finally
            {
              GameEventMgr.m_lock.DowngradeFromWriterLock(ref writerLock);
            }
          }
          if (unique)
            handlerCollection.AddHandlerUnique(e, del);
          else
            handlerCollection.AddHandler(e, del);
        }
        finally
        {
          GameEventMgr.m_lock.ReleaseReaderLock();
        }
      }
      catch (ApplicationException ex)
      {
        if (!GameEventMgr.log.IsErrorEnabled)
          return;
        GameEventMgr.log.Error((object) "Failed to add local event handler!", (Exception) ex);
      }
    }

    public static void AddHandlerUnique(RoadEvent e, RoadEventHandler del)
    {
      GameEventMgr.AddHandler(e, del, true);
    }

    public static void AddHandlerUnique(object obj, RoadEvent e, RoadEventHandler del)
    {
      GameEventMgr.AddHandler(obj, e, del, true);
    }

    public static void Notify(RoadEvent e)
    {
      GameEventMgr.Notify(e, (object) null, (EventArgs) null);
    }

    public static void Notify(RoadEvent e, EventArgs args)
    {
      GameEventMgr.Notify(e, (object) null, args);
    }

    public static void Notify(RoadEvent e, object sender)
    {
      GameEventMgr.Notify(e, sender, (EventArgs) null);
    }

    public static void Notify(RoadEvent e, object sender, EventArgs eArgs)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e), "No event type given!");
      if (sender != null)
      {
        try
        {
          RoadEventHandlerCollection handlerCollection = (RoadEventHandlerCollection) null;
          GameEventMgr.m_lock.AcquireReaderLock(3000);
          try
          {
            handlerCollection = (RoadEventHandlerCollection) GameEventMgr.m_GameObjectEventCollections[sender];
          }
          finally
          {
            GameEventMgr.m_lock.ReleaseReaderLock();
          }
          handlerCollection?.Notify(e, sender, eArgs);
        }
        catch (ApplicationException ex)
        {
          if (GameEventMgr.log.IsErrorEnabled)
            GameEventMgr.log.Error((object) "Failed to notify local event handler!", (Exception) ex);
        }
      }
      GameEventMgr.m_GlobalHandlerCollection.Notify(e, sender, eArgs);
    }

    public static void RegisterGlobalEvents(Assembly asm, Type attribute, RoadEvent e)
    {
      if (asm == (Assembly) null)
        throw new ArgumentNullException(nameof (asm), "No assembly given to search for global event handlers!");
      if (attribute == (Type) null)
        throw new ArgumentNullException(nameof (attribute), "No attribute given!");
      if (e == null)
        throw new ArgumentNullException(nameof (e), "No event type given!");
      foreach (Type type in asm.GetTypes())
      {
        if (type.IsClass)
        {
          foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
          {
            if ((uint) method.GetCustomAttributes(attribute, false).Length > 0U)
            {
              try
              {
                GameEventMgr.m_GlobalHandlerCollection.AddHandler(e, (RoadEventHandler) Delegate.CreateDelegate(typeof (RoadEventHandler), method));
              }
              catch (Exception ex)
              {
                if (GameEventMgr.log.IsErrorEnabled)
                  GameEventMgr.log.Error((object) ("Error registering global event. Method: " + type.FullName + "." + method.Name), ex);
              }
            }
          }
        }
      }
    }

    public static void RemoveAllHandlers(bool deep)
    {
      if (deep)
      {
        try
        {
          GameEventMgr.m_lock.AcquireWriterLock(3000);
          try
          {
            GameEventMgr.m_GameObjectEventCollections.Clear();
          }
          finally
          {
            GameEventMgr.m_lock.ReleaseWriterLock();
          }
        }
        catch (ApplicationException ex)
        {
          if (GameEventMgr.log.IsErrorEnabled)
            GameEventMgr.log.Error((object) "Failed to remove all local event handlers!", (Exception) ex);
        }
      }
      GameEventMgr.m_GlobalHandlerCollection.RemoveAllHandlers();
    }

    public static void RemoveAllHandlers(RoadEvent e, bool deep)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e), "No event type given!");
      if (deep)
      {
        try
        {
          GameEventMgr.m_lock.AcquireReaderLock(3000);
          try
          {
            foreach (RoadEventHandlerCollection handlerCollection in (IEnumerable) GameEventMgr.m_GameObjectEventCollections.Values)
              handlerCollection.RemoveAllHandlers(e);
          }
          finally
          {
            GameEventMgr.m_lock.ReleaseReaderLock();
          }
        }
        catch (ApplicationException ex)
        {
          if (GameEventMgr.log.IsErrorEnabled)
            GameEventMgr.log.Error((object) "Failed to add local event handlers!", (Exception) ex);
        }
      }
      GameEventMgr.m_GlobalHandlerCollection.RemoveAllHandlers(e);
    }

    public static void RemoveAllHandlers(object obj, RoadEvent e)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj), "No object given!");
      if (e == null)
        throw new ArgumentNullException(nameof (e), "No event type given!");
      try
      {
        GameEventMgr.m_lock.AcquireReaderLock(3000);
        try
        {
          ((RoadEventHandlerCollection) GameEventMgr.m_GameObjectEventCollections[obj])?.RemoveAllHandlers(e);
        }
        finally
        {
          GameEventMgr.m_lock.ReleaseReaderLock();
        }
      }
      catch (ApplicationException ex)
      {
        if (!GameEventMgr.log.IsErrorEnabled)
          return;
        GameEventMgr.log.Error((object) "Failed to remove local event handlers!", (Exception) ex);
      }
    }

    public static void RemoveHandler(RoadEvent e, RoadEventHandler del)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e), "No event type given!");
      if (del == null)
        throw new ArgumentNullException(nameof (del), "No event handler given!");
      GameEventMgr.m_GlobalHandlerCollection.RemoveHandler(e, del);
    }

    public static void RemoveHandler(object obj, RoadEvent e, RoadEventHandler del)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj), "No object given!");
      if (e == null)
        throw new ArgumentNullException(nameof (e), "No event type given!");
      if (del == null)
        throw new ArgumentNullException(nameof (del), "No event handler given!");
      try
      {
        GameEventMgr.m_lock.AcquireReaderLock(3000);
        try
        {
          ((RoadEventHandlerCollection) GameEventMgr.m_GameObjectEventCollections[obj])?.RemoveHandler(e, del);
        }
        finally
        {
          GameEventMgr.m_lock.ReleaseReaderLock();
        }
      }
      catch (ApplicationException ex)
      {
        if (!GameEventMgr.log.IsErrorEnabled)
          return;
        GameEventMgr.log.Error((object) "Failed to remove local event handler!", (Exception) ex);
      }
    }
  }
}
