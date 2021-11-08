// Decompiled with JetBrains decompiler
// Type: Game.Server.GameClient
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using log4net;
using System;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Game.Server
{
  public class GameClient : BaseClient
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static readonly byte[] POLICY = Encoding.UTF8.GetBytes("<?xml version=\"1.0\"?><!DOCTYPE cross-domain-policy SYSTEM \"http://www.adobe.com/xml/dtds/cross-domain-policy.dtd\"><cross-domain-policy><allow-access-from domain=\"*\" to-ports=\"*\" /></cross-domain-policy>\0");
    protected GameServer _srvr;
    public int Lottery;
    protected IPacketLib m_packetLib;
    protected PacketProcessor m_packetProcessor;
    protected long m_pingTime;
    protected GamePlayer m_player;
    public string tempData;
    public int Version;
    private string m_hwid;

    public GameClient(GameServer svr, byte[] read, byte[] send)
      : base(read, send)
    {
      this.m_pingTime = DateTime.Now.Ticks;
      this.Lottery = -1;
      this.tempData = string.Empty;
      this.m_pingTime = DateTime.Now.Ticks;
      this._srvr = svr;
      this.m_player = (GamePlayer) null;
      this.Encryted = true;
      this.AsyncPostSend = true;
    }

    public override void Disconnect()
    {
      base.Disconnect();
    }

    public override void DisplayMessage(string msg)
    {
      base.DisplayMessage(msg);
    }

    protected override void OnConnect()
    {
      base.OnConnect();
      this.m_pingTime = DateTime.Now.Ticks;
    }

    protected override void OnDisconnect()
    {
      try
      {
        GamePlayer gamePlayer = Interlocked.Exchange<GamePlayer>(ref this.m_player, (GamePlayer) null);
        if (gamePlayer != null)
        {
          gamePlayer.FightBag.ClearBag();
          LoginMgr.ClearLoginPlayer(gamePlayer.PlayerCharacter.ID, this);
          gamePlayer.Quit();
        }
        byte[] sendBuffer = this.m_sendBuffer;
        this.m_sendBuffer = (byte[]) null;
        this._srvr.ReleasePacketBuffer(sendBuffer);
        byte[] readBuffer = this.m_readBuffer;
        this.m_readBuffer = (byte[]) null;
        this._srvr.ReleasePacketBuffer(readBuffer);
        base.OnDisconnect();
      }
      catch (Exception ex)
      {
        if (!GameClient.log.IsErrorEnabled)
          return;
        GameClient.log.Error((object) nameof (OnDisconnect), ex);
      }
    }

    public override void OnRecv(int num_bytes)
    {
      if (this.m_packetProcessor != null)
        base.OnRecv(num_bytes);
      else if (this.m_readBuffer[0] == (byte) 60)
        this.m_sock.Send(GameClient.POLICY);
      else
        base.OnRecv(num_bytes);
      this.m_pingTime = DateTime.Now.Ticks;
    }

    public override void OnRecvPacket(GSPacketIn pkg)
    {
      if (this.m_packetProcessor == null)
      {
        this.m_packetLib = AbstractPacketLib.CreatePacketLibForVersion(1, this);
        this.m_packetProcessor = new PacketProcessor(this);
      }
      if (this.m_player != null)
      {
        pkg.ClientID = this.m_player.PlayerId;
        pkg.WriteHeader();
      }
      this.m_packetProcessor.HandlePacket(pkg);
    }

    public override void SendTCP(GSPacketIn pkg)
    {
      base.SendTCP(pkg);
    }

    public override string ToString()
    {
      return new StringBuilder(128).Append(" pakLib:").Append(this.Out == null ? "(null)" : this.Out.GetType().FullName).Append(" IP:").Append(this.TcpEndpoint).Append(" char:").Append(this.Player == null ? "null" : this.Player.PlayerCharacter.NickName).ToString();
    }

    public IPacketLib Out
    {
      get
      {
        return this.m_packetLib;
      }
      set
      {
        this.m_packetLib = value;
      }
    }

    public PacketProcessor PacketProcessor
    {
      get
      {
        return this.m_packetProcessor;
      }
    }

    public long PingTime
    {
      get
      {
        return this.m_pingTime;
      }
    }

    public string HWID
    {
      get
      {
        return this.m_hwid;
      }
      set
      {
        this.m_hwid = value;
      }
    }

    public GamePlayer Player
    {
      get
      {
        return this.m_player;
      }
      set
      {
        Interlocked.Exchange<GamePlayer>(ref this.m_player, value)?.Quit();
      }
    }

    public GameServer Server
    {
      get
      {
        return this._srvr;
      }
    }
  }
}
