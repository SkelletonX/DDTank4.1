// Decompiled with JetBrains decompiler
// Type: Game.Base.Events.RoadEventHandlerCollection
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

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
    protected readonly HybridDictionary m_events = new HybridDictionary();
    protected readonly ReaderWriterLock m_lock = new ReaderWriterLock();
    protected const int TIMEOUT = 3000;

    public void AddHandler(RoadEvent e, RoadEventHandler del)
    {
      try
      {
        this.m_lock.AcquireWriterLock(3000);
        try
        {
          WeakMulticastDelegate weakDelegate = (WeakMulticastDelegate) this.m_events[(object) e];
          if (weakDelegate == null)
            this.m_events[(object) e] = (object) new WeakMulticastDelegate((Delegate) del);
          else
            this.m_events[(object) e] = (object) WeakMulticastDelegate.Combine(weakDelegate, (Delegate) del);
        }
        finally
        {
          this.m_lock.ReleaseWriterLock();
        }
      }
      catch (ApplicationException ex)
      {
        if (!RoadEventHandlerCollection.log.IsErrorEnabled)
          return;
        RoadEventHandlerCollection.log.Error((object) "Failed to add event handler!", (Exception) ex);
      }
    }

    public void AddHandlerUnique(RoadEvent e, RoadEventHandler del)
    {
      try
      {
        this.m_lock.AcquireWriterLock(3000);
        try
        {
          WeakMulticastDelegate weakDelegate = (WeakMulticastDelegate) this.m_events[(object) e];
          if (weakDelegate == null)
            this.m_events[(object) e] = (object) new WeakMulticastDelegate((Delegate) del);
          else
            this.m_events[(object) e] = (object) WeakMulticastDelegate.CombineUnique(weakDelegate, (Delegate) del);
        }
        finally
        {
          this.m_lock.ReleaseWriterLock();
        }
      }
      catch (ApplicationException ex)
      {
        if (!RoadEventHandlerCollection.log.IsErrorEnabled)
          return;
        RoadEventHandlerCollection.log.Error((object) "Failed to add event handler!", (Exception) ex);
      }
    }

    public void Notify(RoadEvent e)
    {
      this.Notify(e, (object) null, (EventArgs) null);
    }

    public void Notify(RoadEvent e, EventArgs args)
    {
      this.Notify(e, (object) null, args);
    }

    public void Notify(RoadEvent e, object sender)
    {
      this.Notify(e, sender, (EventArgs) null);
    }

    public void Notify(RoadEvent e, object sender, EventArgs eArgs)
    {
      try
      {
        this.m_lock.AcquireReaderLock(3000);
        WeakMulticastDelegate multicastDelegate;
        try
        {
          multicastDelegate = (WeakMulticastDelegate) this.m_events[(object) e];
        }
        finally
        {
          this.m_lock.ReleaseReaderLock();
        }
        multicastDelegate?.InvokeSafe(new object[3]
        {
          (object) e,
          sender,
          (object) eArgs
        });
      }
      catch (ApplicationException ex)
      {
        if (!RoadEventHandlerCollection.log.IsErrorEnabled)
          return;
        RoadEventHandlerCollection.log.Error((object) "Failed to notify event handler!", (Exception) ex);
      }
    }

    public void RemoveAllHandlers()
    {
      try
      {
        this.m_lock.AcquireWriterLock(3000);
        try
        {
          this.m_events.Clear();
        }
        finally
        {
          this.m_lock.ReleaseWriterLock();
        }
      }
      catch (ApplicationException ex)
      {
        if (!RoadEventHandlerCollection.log.IsErrorEnabled)
          return;
        RoadEventHandlerCollection.log.Error((object) "Failed to remove all event handlers!", (Exception) ex);
      }
    }

    public void RemoveAllHandlers(RoadEvent e)
    {
      try
      {
        this.m_lock.AcquireWriterLock(3000);
        try
        {
          this.m_events.Remove((object) e);
        }
        finally
        {
          this.m_lock.ReleaseWriterLock();
        }
      }
      catch (ApplicationException ex)
      {
        if (!RoadEventHandlerCollection.log.IsErrorEnabled)
          return;
        RoadEventHandlerCollection.log.Error((object) "Failed to remove event handlers!", (Exception) ex);
      }
    }

    public void RemoveHandler(RoadEvent e, RoadEventHandler del)
    {
      try
      {
        this.m_lock.AcquireWriterLock(3000);
        try
        {
          WeakMulticastDelegate weakDelegate = (WeakMulticastDelegate) this.m_events[(object) e];
          if (weakDelegate == null)
            return;
          WeakMulticastDelegate multicastDelegate = WeakMulticastDelegate.Remove(weakDelegate, (Delegate) del);
          if (multicastDelegate == null)
            this.m_events.Remove((object) e);
          else
            this.m_events[(object) e] = (object) multicastDelegate;
        }
        finally
        {
          this.m_lock.ReleaseWriterLock();
        }
      }
      catch (ApplicationException ex)
      {
        if (!RoadEventHandlerCollection.log.IsErrorEnabled)
          return;
        RoadEventHandlerCollection.log.Error((object) "Failed to remove event handler!", (Exception) ex);
      }
    }
  }
}
