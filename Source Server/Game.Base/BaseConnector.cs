// Decompiled with JetBrains decompiler
// Type: Game.Base.BaseConnector
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using log4net;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace Game.Base
{
    public class BaseConnector : BaseClient
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly int RECONNECT_INTERVAL = 10000;
        private bool _autoReconnect;
        private IPEndPoint _remoteEP;
        private SocketAsyncEventArgs e;
        private Timer timer;

        public BaseConnector(
          string ip,
          int port,
          bool autoReconnect,
          byte[] readBuffer,
          byte[] sendBuffer)
          : base(readBuffer, sendBuffer)
        {
            this._remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);
            this._autoReconnect = autoReconnect;
            this.e = new SocketAsyncEventArgs();
        }

        public bool Connect()
        {
            try
            {
                this.m_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.m_readBufEnd = 0;
                this.m_sock.Connect((EndPoint)this._remoteEP);
                BaseConnector.log.InfoFormat("Connected to {0}", (object)this._remoteEP);
            }
            catch
            {
                BaseConnector.log.ErrorFormat("Connect {0} failed!", (object)this._remoteEP);
                this.m_sock = (Socket)null;
                return false;
            }
            this.OnConnect();
            this.ReceiveAsync();
            return true;
        }

        private static void RetryTimerCallBack(object target)
        {
            BaseConnector baseConnector = target as BaseConnector;
            if (baseConnector != null)
                baseConnector.TryReconnect();
            else
                BaseConnector.log.Error((object)"BaseConnector retryconnect timer return NULL!");
        }

        private void TryReconnect()
        {
            if (this.Connect())
            {
                if (this.timer != null)
                {
                    this.timer.Dispose();
                    this.timer = (Timer)null;
                }
                this.ReceiveAsync();
            }
            else
            {
                BaseConnector.log.ErrorFormat("Reconnect {0} failed:", (object)this._remoteEP);
                BaseConnector.log.ErrorFormat("Retry after {0} ms!", (object)BaseConnector.RECONNECT_INTERVAL);
                if (this.timer == null)
                    this.timer = new Timer(new TimerCallback(BaseConnector.RetryTimerCallBack), (object)this, -1, -1);
                this.timer.Change(BaseConnector.RECONNECT_INTERVAL, -1);
            }
        }

        public IPEndPoint RemoteEP
        {
            get
            {
                return this._remoteEP;
            }
        }
    }
}
