// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.Battle.RingStationFightConnector
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.GameObjects;
using Game.Server.Managers;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Game.Server.RingStation.Battle
{
  public class RingStationFightConnector : BaseConnector
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private string m_key;
    private RingStationBattleServer m_server;

    public RingStationFightConnector(
      RingStationBattleServer server,
      string ip,
      int port,
      string key)
      : base(ip, port, true, new byte[8192], new byte[8192])
    {
      this.m_server = server;
      this.m_key = key;
      this.Strict = true;
    }

    protected void AsynProcessPacket(object state)
    {
      try
      {
        GSPacketIn gsPacketIn = state as GSPacketIn;
        switch (gsPacketIn.Code)
        {
          case 0:
            this.HandleRSAKey(gsPacketIn);
            break;
          case 19:
            this.HandlePlayerChatSend(gsPacketIn);
            break;
          case 32:
            this.HandleSendToPlayer(gsPacketIn);
            break;
          case 33:
            this.HandleUpdatePlayerGameId(gsPacketIn);
            break;
          case 34:
            this.HandleDisconnectPlayer(gsPacketIn);
            break;
          case 35:
            this.HandlePlayerOnGameOver(gsPacketIn);
            break;
          case 36:
            this.HandlePlayerOnUsingItem(gsPacketIn);
            break;
          case 38:
            this.HandlePlayerAddGold(gsPacketIn);
            break;
          case 39:
            this.HandlePlayerAddGP(gsPacketIn);
            break;
          case 40:
            this.HandlePlayerOnKillingLiving(gsPacketIn);
            break;
          case 41:
            this.HandlePlayerOnMissionOver(gsPacketIn);
            break;
          case 42:
            this.HandlePlayerConsortiaFight(gsPacketIn);
            break;
          case 43:
            this.HandlePlayerSendConsortiaFight(gsPacketIn);
            break;
          case 44:
            this.HandlePlayerRemoveGold(gsPacketIn);
            break;
          case 45:
            this.HandlePlayerRemoveMoney(gsPacketIn);
            break;
          case 48:
            this.HandlePlayerAddTemplate1(gsPacketIn);
            break;
          case 49:
            this.HandlePlayerRemoveGP(gsPacketIn);
            break;
          case 50:
            this.HandlePlayerRemoveOffer(gsPacketIn);
            break;
          case 51:
            this.HandlePlayerAddOffer(gsPacketIn);
            break;
          case 52:
            this.HandPlayerAddRobRiches(gsPacketIn);
            break;
          case 53:
            this.HandleClearBag(gsPacketIn);
            break;
          case 65:
            this.HandleRoomRemove(gsPacketIn);
            break;
          case 66:
            this.HandleStartGame(gsPacketIn);
            break;
          case 67:
            this.HandleSendToRoom(gsPacketIn);
            break;
          case 68:
            this.HandleStopGame(gsPacketIn);
            break;
          case 69:
            this.HandleUpdateRoomId(gsPacketIn);
            break;
          case 70:
            this.HandlePlayerRemove(gsPacketIn);
            break;
          case 73:
            this.HandlePlayerHealstone(gsPacketIn);
            break;
          case 74:
            this.HandlePlayerAddMoney(gsPacketIn);
            break;
          case 75:
            this.HandlePlayerAddGiftToken(gsPacketIn);
            break;
          case 76:
            this.HandlePlayerAddMedal(gsPacketIn);
            break;
          case 77:
            this.HandleFindConsortiaAlly(gsPacketIn);
            break;
          case 84:
            this.HandlePlayerAddLeagueMoney(gsPacketIn);
            break;
          case 85:
            this.HandlePlayerAddPrestige(gsPacketIn);
            break;
          case 86:
            this.HandlePlayerUpdateRestCount(gsPacketIn);
            break;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
      }
    }

    private void HandleClearBag(GSPacketIn pkg)
    {
    }

    private void HandleDisconnectPlayer(GSPacketIn pkg)
    {
    }

    public void HandleFindConsortiaAlly(GSPacketIn pkg)
    {
    }

    private void HandlePlayerAddGold(GSPacketIn pkg)
    {
    }

    private void HandlePlayerAddGP(GSPacketIn pkg)
    {
    }

    private void HandlePlayerAddGiftToken(GSPacketIn pkg)
    {
    }

    private void HandlePlayerAddLeagueMoney(GSPacketIn pkg)
    {
    }

    private void HandlePlayerAddMedal(GSPacketIn pkg)
    {
    }

    private void HandlePlayerAddMoney(GSPacketIn pkg)
    {
    }

    private void HandlePlayerAddPrestige(GSPacketIn pkg)
    {
    }

    private void HandlePlayerAddTemplate1(GSPacketIn pkg)
    {
    }

    private void HandlePlayerConsortiaFight(GSPacketIn pkg)
    {
    }

    private void HandlePlayerChatSend(GSPacketIn pkg)
    {
    }

    private void HandlePlayerHealstone(GSPacketIn pkg)
    {
    }

    private void HandlePlayerOnGameOver(GSPacketIn pkg)
    {
    }

    private void HandlePlayerOnKillingLiving(GSPacketIn pkg)
    {
    }

    private void HandlePlayerOnMissionOver(GSPacketIn pkg)
    {
    }

    private void HandlePlayerOnUsingItem(GSPacketIn pkg)
    {
    }

    private void HandlePlayerRemove(GSPacketIn pkg)
    {
    }

    private void HandlePlayerRemoveGold(GSPacketIn pkg)
    {
    }

    private void HandlePlayerRemoveGP(GSPacketIn pkg)
    {
    }

    private void HandlePlayerRemoveMoney(GSPacketIn pkg)
    {
    }

    private void HandlePlayerRemoveOffer(GSPacketIn pkg)
    {
    }

    private void HandlePlayerSendConsortiaFight(GSPacketIn pkg)
    {
    }

    private void HandlePlayerUpdateRestCount(GSPacketIn pkg)
    {
    }

    protected void HandleRoomRemove(GSPacketIn packet)
    {
      this.m_server.RemoveRoomImp(packet.ClientID);
    }

    protected void HandleRSAKey(GSPacketIn packet)
    {
      RSAParameters parameters = new RSAParameters()
      {
        Modulus = packet.ReadBytes(128),
        Exponent = packet.ReadBytes()
      };
      RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
      rsa.ImportParameters(parameters);
      this.SendRSALogin(rsa, this.m_key);
    }

    protected void HandleSendToPlayer(GSPacketIn pkg)
    {
    }

    protected void HandleSendToRoom(GSPacketIn pkg)
    {
      this.m_server.SendToRoom(pkg.ClientID, pkg.ReadPacket(), pkg.Parameter1, pkg.Parameter2);
    }

    private void HandlePlayerAddOffer(GSPacketIn pkg)
    {
      GamePlayer playerById = WorldMgr.GetPlayerById(pkg.ClientID);
      Console.WriteLine("eoqringstation: " + (object) pkg.Parameter1);
      playerById?.AddOffer(pkg.Parameter1, false);
    }

    protected void HandleStartGame(GSPacketIn pkg)
    {
      ProxyRingStationGame game = new ProxyRingStationGame(pkg.Parameter2, this, (eRoomType) pkg.ReadInt(), (eGameType) pkg.ReadInt(), pkg.ReadInt());
      this.m_server.StartGame(pkg.Parameter1, game);
    }

    protected void HandleStopGame(GSPacketIn pkg)
    {
      this.m_server.StopGame(pkg.Parameter1, pkg.Parameter2);
    }

    private void HandleUpdatePlayerGameId(GSPacketIn pkg)
    {
      this.m_server.UpdatePlayerGameId(pkg.Parameter1, pkg.Parameter2);
    }

    private void HandleUpdateRoomId(GSPacketIn pkg)
    {
    }

    private void HandPlayerAddRobRiches(GSPacketIn pkg)
    {
      GamePlayer playerById = WorldMgr.GetPlayerById(pkg.ClientID);
      int num = pkg.ReadInt();
      if (playerById == null || num != pkg.Parameter1)
        return;
      playerById.AddRobRiches(pkg.Parameter1);
    }

    protected override void OnDisconnect()
    {
      base.OnDisconnect();
    }

    public override void OnRecvPacket(GSPacketIn pkg)
    {
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.AsynProcessPacket), (object) pkg);
    }

    public void SendAddRoom(BaseRoomRingStation room)
    {
      GSPacketIn pkg = new GSPacketIn((short) 64);
      pkg.WriteInt(room.RoomId);
      pkg.WriteInt(room.RoomType);
      pkg.WriteInt(room.GameType);
      pkg.WriteInt(room.GuildId);
      pkg.WriteInt(room.PickUpNpcId);
      pkg.WriteBoolean(false);
      pkg.WriteBoolean(room.IsAutoBot);
      pkg.WriteBoolean(false);
      List<RingStationGamePlayer> players = room.GetPlayers();
      pkg.WriteInt(players.Count);
      foreach (RingStationGamePlayer stationGamePlayer in players)
      {
        pkg.WriteInt(stationGamePlayer.ID);
        pkg.WriteInt(GameServer.Instance.Configuration.ZoneId);
        pkg.WriteString(GameServer.Instance.Configuration.ZoneName);
        pkg.WriteInt(0);
        pkg.WriteString(stationGamePlayer.NickName);
        pkg.WriteBoolean(stationGamePlayer.Sex);
        pkg.WriteInt(stationGamePlayer.Hide);
        pkg.WriteString(stationGamePlayer.Style);
        pkg.WriteString(stationGamePlayer.Colors);
        pkg.WriteString(stationGamePlayer.Skin);
        pkg.WriteInt(stationGamePlayer.Offer);
        pkg.WriteInt(stationGamePlayer.GP);
        pkg.WriteInt(stationGamePlayer.Grade);
        pkg.WriteInt(stationGamePlayer.Repute);
        pkg.WriteInt(stationGamePlayer.ConsortiaID);
        pkg.WriteString(stationGamePlayer.ConsortiaName);
        pkg.WriteInt(stationGamePlayer.ConsortiaLevel);
        pkg.WriteInt(stationGamePlayer.ConsortiaRepute);
        pkg.WriteBoolean(false);
        pkg.WriteInt(stationGamePlayer.badgeID);
        pkg.WriteString(stationGamePlayer.Honor);
        pkg.WriteInt(0);
        pkg.WriteString(stationGamePlayer.WeaklessGuildProgressStr);
        pkg.WriteInt(0);
        pkg.WriteInt(stationGamePlayer.FightPower);
        pkg.WriteInt(0);
        pkg.WriteInt(0);
        pkg.WriteString("");
        pkg.WriteInt(stationGamePlayer.Attack);
        pkg.WriteInt(stationGamePlayer.Defence);
        pkg.WriteInt(stationGamePlayer.Agility);
        pkg.WriteInt(stationGamePlayer.Luck);
        pkg.WriteInt(stationGamePlayer.hp);
        pkg.WriteDouble(stationGamePlayer.BaseAttack);
        pkg.WriteDouble(stationGamePlayer.BaseDefence);
        pkg.WriteDouble(stationGamePlayer.BaseAgility);
        pkg.WriteDouble(stationGamePlayer.BaseBlood);
        pkg.WriteInt(stationGamePlayer.TemplateID);
        pkg.WriteInt(stationGamePlayer.StrengthLevel);
        pkg.WriteInt(0);
        pkg.WriteBoolean(stationGamePlayer.CanUserProp);
        pkg.WriteInt(stationGamePlayer.SecondWeapon);
        pkg.WriteInt(stationGamePlayer.StrengthLevel);
        pkg.WriteInt(0);
        pkg.WriteInt(0);
        pkg.WriteDouble(stationGamePlayer.GPAddPlus);
        pkg.WriteDouble((double) stationGamePlayer.GMExperienceRate);
        pkg.WriteDouble(0.0);
        pkg.WriteDouble(0.0);
        pkg.WriteDouble(0.0);
        pkg.WriteInt(RingStationConfiguration.ServerID);
        pkg.WriteInt(0);
        pkg.WriteInt(0);
        pkg.WriteInt(0);
        pkg.WriteByte(stationGamePlayer.typeVIP);
        pkg.WriteInt(stationGamePlayer.VIPLevel);
        pkg.WriteDateTime(DateTime.Now);
        pkg.WriteBoolean(false);
        pkg.WriteInt(0);
        pkg.WriteBoolean(false);
      }
      this.SendTCP(pkg);
    }

    public void SendChangeGameType()
    {
    }

    public void SendChatMessage()
    {
    }

    public void SendFightNotice()
    {
    }

    public void SendFindConsortiaAlly(int state, int gameid)
    {
    }

    public void SendKitOffPlayer(int playerid)
    {
    }

    public void SendPlayerDisconnet(int gameId, int playerId, int roomid)
    {
    }

    public void SendRemoveRoom(BaseRoomRingStation room)
    {
      this.SendTCP(new GSPacketIn((short) 65)
      {
        Parameter1 = room.RoomId
      });
    }

    public void SendRSALogin(RSACryptoServiceProvider rsa, string key)
    {
      GSPacketIn pkg = new GSPacketIn((short) 1);
      pkg.Write(rsa.Encrypt(Encoding.UTF8.GetBytes(key), false));
      this.SendTCP(pkg);
    }

    public void SendToGame(int gameId, GSPacketIn pkg)
    {
      GSPacketIn pkg1 = new GSPacketIn((short) 2, gameId);
      pkg1.WritePacket(pkg);
      this.SendTCP(pkg1);
    }

    private void SendUsingPropInGame(int gameId, int playerId, int templateId, bool result)
    {
    }
  }
}
