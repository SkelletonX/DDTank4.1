// Decompiled with JetBrains decompiler
// Type: Game.Base.BaseServer
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using log4net;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Game.Base
{
  public class BaseServer
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static readonly int SEND_BUFF_SIZE = 16384;
    protected readonly HybridDictionary _clients = new HybridDictionary();
    protected SocketAsyncEventArgs ac_event = new SocketAsyncEventArgs();
    protected Socket _linstener;

    public BaseServer()
    {
      this.ac_event.Completed += new EventHandler<SocketAsyncEventArgs>(this.AcceptAsyncCompleted);
    }

    private void AcceptAsync()
    {
      try
      {
        if (this._linstener == null)
          return;
        SocketAsyncEventArgs e = new SocketAsyncEventArgs();
        e.Completed += new EventHandler<SocketAsyncEventArgs>(this.AcceptAsyncCompleted);
        this._linstener.AcceptAsync(e);
      }
      catch (Exception ex)
      {
        BaseServer.log.Error((object) "AcceptAsync is error!", ex);
      }
    }

    private void AcceptAsyncCompleted(object sender, SocketAsyncEventArgs e)
    {
      Socket connectedSocket = (Socket) null;
      try
      {
        connectedSocket = e.AcceptSocket;
        connectedSocket.SendBufferSize = BaseServer.SEND_BUFF_SIZE;
        BaseClient newClient = this.GetNewClient();
        try
        {
          if (BaseServer.log.IsInfoEnabled)
            BaseServer.log.Info((object) ("Incoming connection from " + (connectedSocket.Connected ? connectedSocket.RemoteEndPoint.ToString() : "socket disconnected")));
          lock (this._clients.SyncRoot)
          {
            this._clients.Add((object) newClient, (object) newClient);
            newClient.Disconnected += new ClientEventHandle(this.client_Disconnected);
          }
          newClient.Connect(connectedSocket);
          newClient.ReceiveAsync();
        }
        catch (Exception ex)
        {
          BaseServer.log.ErrorFormat("create client failed:{0}", (object) ex);
          newClient.Disconnect();
        }
      }
      catch
      {
        if (connectedSocket == null)
          return;
        try
        {
          connectedSocket.Close();
        }
        catch
        {
        }
      }
      finally
      {
        e.Dispose();
        this.AcceptAsync();
      }
    }

    private void client_Disconnected(BaseClient client)
    {
      client.Disconnected -= new ClientEventHandle(this.client_Disconnected);
      this.RemoveClient(client);
    }

    public void Dispose()
    {
      this.ac_event.Dispose();
    }

    public BaseClient[] GetAllClients()
    {
      lock (this._clients.SyncRoot)
      {
        BaseClient[] baseClientArray = new BaseClient[this._clients.Count];
        this._clients.Keys.CopyTo((Array) baseClientArray, 0);
        return baseClientArray;
      }
    }

    protected virtual BaseClient GetNewClient()
    {
      return new BaseClient(new byte[8192], new byte[8192]);
    }

    public virtual bool InitSocket(IPAddress ip, int port)
    {
      try
      {
        this._linstener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        this._linstener.Bind((EndPoint) new IPEndPoint(ip, port));
      }
      catch (Exception ex)
      {
        BaseServer.log.Error((object) nameof (InitSocket), ex);
        return false;
      }
      return true;
    }

    public virtual void RemoveClient(BaseClient client)
    {
      lock (this._clients.SyncRoot)
        this._clients.Remove((object) client);
    }

    public virtual bool Start()
    {
      if (this._linstener == null)
        return false;
      try
      {
        this._linstener.Listen(100);
        this.AcceptAsync();
        if (BaseServer.log.IsDebugEnabled)
          BaseServer.log.Debug((object) "Server is now listening to incoming connections!");
      }
      catch (Exception ex)
      {
        if (BaseServer.log.IsErrorEnabled)
          BaseServer.log.Error((object) nameof (Start), ex);
        if (this._linstener != null)
          this._linstener.Close();
        return false;
      }
      return true;
    }

    public virtual void Stop()
    {
      BaseServer.log.Debug((object) "Stopping server! - Entering method");
      try
      {
        if (this._linstener != null)
        {
          Socket linstener = this._linstener;
          this._linstener = (Socket) null;
          linstener.Close();
          BaseServer.log.Debug((object) "Server is no longer listening for incoming connections!");
        }
      }
      catch (Exception ex)
      {
        BaseServer.log.Error((object) nameof (Stop), ex);
      }
      if (this._clients != null)
      {
        lock (this._clients.SyncRoot)
        {
          try
          {
            BaseClient[] baseClientArray = new BaseClient[this._clients.Keys.Count];
            this._clients.Keys.CopyTo((Array) baseClientArray, 0);
            foreach (BaseClient baseClient in baseClientArray)
              baseClient.Disconnect();
            BaseServer.log.Debug((object) "Stopping server! - Cleaning up client list!");
          }
          catch (Exception ex)
          {
            BaseServer.log.Error((object) nameof (Stop), ex);
          }
        }
      }
      BaseServer.log.Debug((object) "Stopping server! - End of method!");
    }

    public int ClientCount
    {
      get
      {
        return this._clients.Count;
      }
    }
  }
}
