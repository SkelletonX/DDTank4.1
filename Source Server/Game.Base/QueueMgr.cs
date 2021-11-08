// Decompiled with JetBrains decompiler
// Type: Road.Base.QueueMgr`1
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using System;
using System.Collections.Generic;

namespace Road.Base
{
  public class QueueMgr<T>
  {
    private Queue<T> _queue;
    private bool _executing;
    private QueueMgr<T>.Exectue _exectue;
    private object _syncRoot;
    private QueueMgr<T>.AsynExecute _asynExecute;
    private AsyncCallback _asynCallBack;

    public QueueMgr(QueueMgr<T>.Exectue exec)
    {
      this._queue = new Queue<T>();
      this._executing = false;
      this._exectue = exec;
      this._syncRoot = new object();
      this._asynExecute = new QueueMgr<T>.AsynExecute(this.Exectuing);
      this._asynCallBack = new AsyncCallback(this.AsynCallBack);
    }

    public void ExecuteQueue(T info)
    {
      lock (this._syncRoot)
      {
        if (this._executing)
        {
          this._queue.Enqueue(info);
          return;
        }
        this._executing = true;
      }
      this._asynExecute.BeginInvoke(info, this._asynCallBack, (object) this._asynExecute);
    }

    private void AsynCallBack(IAsyncResult ar)
    {
      ((QueueMgr<T>.AsynExecute) ar.AsyncState).EndInvoke(ar);
    }

    private void Exectuing(T info)
    {
      this._exectue(info);
      T info1;
      lock (this._syncRoot)
      {
        if (this._queue.Count > 0)
        {
          info1 = this._queue.Peek();
          this._queue.Dequeue();
        }
        else
        {
          this._executing = false;
          return;
        }
      }
      this.Exectuing(info1);
    }

    public delegate void Exectue(T msg);

    public delegate void AsynExecute(T info);
  }
}
