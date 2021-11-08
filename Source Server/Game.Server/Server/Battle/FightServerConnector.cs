// Decompiled with JetBrains decompiler
// Type: Game.Server.Battle.FightServerConnector
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Base;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Object;
using Game.Server.Buffer;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.RingStation;
using Game.Server.Rooms;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Game.Server.Battle
{
  public class FightServerConnector : BaseConnector
  {
    private static readonly ILog ilog_2 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private BattleServer battleServer_0;
    private string string_0;

    protected override void OnDisconnect()
    {
      base.OnDisconnect();
    }

    public override void OnRecvPacket(GSPacketIn pkg)
    {
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.AsynProcessPacket), (object) pkg);
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
          case 32:
            this.HandleSendToPlayer(gsPacketIn);
            break;
          case 33:
            this.method_29(gsPacketIn);
            break;
          case 34:
            this.method_27(gsPacketIn);
            break;
          case 35:
            this.HandlePlayerOnGameOver(gsPacketIn);
            break;
          case 36:
            this.method_24(gsPacketIn);
            break;
          case 38:
            this.method_23(gsPacketIn);
            break;
          case 39:
            this.method_18(gsPacketIn);
            break;
          case 40:
            this.method_10(gsPacketIn);
            break;
          case 41:
            this.method_11(gsPacketIn);
            break;
          case 42:
            this.method_12(gsPacketIn);
            break;
          case 43:
            this.method_13(gsPacketIn);
            break;
          case 44:
            this.method_14(gsPacketIn);
            break;
          case 45:
            this.method_15(gsPacketIn);
            break;
          case 48:
            this.method_17(gsPacketIn);
            break;
          case 49:
            this.method_22(gsPacketIn);
            break;
          case 50:
            this.method_16(gsPacketIn);
            break;
          case 51:
            this.HandlePlayerAddOffer(gsPacketIn);
            break;
          case 52:
            this.HandPlayerAddRobRiches(gsPacketIn);
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
            this.HandleFindConsortiaAlly(gsPacketIn);
            break;
          case 70:
            this.method_19(gsPacketIn);
            break;
          case 71:
            this.method_21(gsPacketIn);
            break;
          case 73:
            this.method_28(gsPacketIn);
            break;
          case 88:
            this.HandleFightNPC(gsPacketIn);
            break;
          case 200:
            this.method_7(gsPacketIn);
            break;
          case 201:
            this.method_8(gsPacketIn);
            break;
          case 203:
            this.HandleFightNPC(gsPacketIn);
            break;
          case 204:
            this.method_3(gsPacketIn);
            break;
          case 205:
            this.method_4(gsPacketIn);
            break;
          case 206:
            this.method_5(gsPacketIn);
            break;
          case 666:
            this.HandleTakeCardTemp(gsPacketIn);
            break;
        }
      }
      catch (Exception ex)
      {
        GameServer.log.Error((object) nameof (AsynProcessPacket), ex);
      }
    }

    private void HandlePlayerAddOffer(GSPacketIn pkg)
    {
      WorldMgr.GetPlayerById(pkg.ClientID)?.AddOffer(pkg.Parameter1, false);
    }

    private void HandPlayerAddRobRiches(GSPacketIn pkg)
    {
      GamePlayer playerById = WorldMgr.GetPlayerById(pkg.ClientID);
      int num = pkg.ReadInt();
      if (playerById == null || num != pkg.Parameter1)
        return;
      playerById.AddRobRiches(pkg.Parameter1);
    }

    public void HandleTakeCardTemp(GSPacketIn pkg)
    {
      WorldMgr.GetPlayerById(pkg.ClientID)?.OnTakeCard(pkg.ReadInt(), pkg.ReadInt(), pkg.ReadInt(), pkg.ReadInt());
    }

    private void method_3(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.AddEliteScore(gspacketIn_0.Parameter1);
    }

    private void method_4(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.RemoveEliteScore(gspacketIn_0.Parameter1);
    }

    private void method_5(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.SendWinEliteChampion();
    }

    private void HandleFightNPC(GSPacketIn gspacketIn_0)
    {
      int roomtype = gspacketIn_0.ReadInt();
      int gametype = gspacketIn_0.ReadInt();
      int npcId = gspacketIn_0.ReadInt();
      GamePlayer playerById = WorldMgr.GetPlayerById(gspacketIn_0.Parameter1);
      if (playerById == null)
      {
        RingStationMgr.CreateAutoBot(roomtype, gametype, npcId);
        Console.WriteLine("Create autobot by default");
      }
      else
      {
        RingStationMgr.CreateAutoBot(playerById, roomtype, gametype, npcId);
        Console.WriteLine("Create autobot by " + playerById.PlayerCharacter.NickName);
      }
    }

    private void method_7(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.OnFightOneBloodIsWin((eRoomType) gspacketIn_0.Parameter1, true);
    }

    private void method_8(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.OnFightAddOffer(gspacketIn_0.Parameter1);
    }

    private void method_9(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.SendMessage(gspacketIn_0.ReadString());
    }

    public void HandleFindConsortiaAlly(GSPacketIn pkg)
    {
      this.SendFindConsortiaAlly(ConsortiaMgr.FindConsortiaAlly(pkg.ReadInt(), pkg.ReadInt()), pkg.ReadInt());
    }

    private void method_10(GSPacketIn gspacketIn_0)
    {
      GamePlayer playerById = WorldMgr.GetPlayerById(gspacketIn_0.ClientID);
      AbstractGame game = playerById.CurrentRoom.Game;
      playerById?.OnKillingLiving(game, gspacketIn_0.ReadInt(), gspacketIn_0.ClientID, gspacketIn_0.ReadBoolean(), gspacketIn_0.ReadInt());
    }

    private void method_11(GSPacketIn gspacketIn_0)
    {
      GamePlayer playerById = WorldMgr.GetPlayerById(gspacketIn_0.ClientID);
      AbstractGame game = playerById.CurrentRoom.Game;
      playerById?.OnMissionOver(game, gspacketIn_0.ReadBoolean(), gspacketIn_0.ReadInt(), gspacketIn_0.ReadInt());
    }

    private void method_12(GSPacketIn gspacketIn_0)
    {
      GamePlayer playerById1 = WorldMgr.GetPlayerById(gspacketIn_0.ClientID);
      Dictionary<int, Player> players = new Dictionary<int, Player>();
      int consortiaWin = gspacketIn_0.ReadInt();
      int consortiaLose = gspacketIn_0.ReadInt();
      int count = gspacketIn_0.ReadInt();
      for (int key = 0; key < count; ++key)
      {
        GamePlayer playerById2 = WorldMgr.GetPlayerById(gspacketIn_0.ReadInt());
        if (playerById2 != null)
        {
          Player player = new Player((IGamePlayer) playerById2, 0, (BaseGame) null, 0, playerById2.PlayerCharacter.hp);
          players.Add(key, player);
        }
      }
      eRoomType roomType = (eRoomType) gspacketIn_0.ReadByte();
      eGameType gameClass = (eGameType) gspacketIn_0.ReadByte();
      int totalKillHealth = gspacketIn_0.ReadInt();
      playerById1?.ConsortiaFight(consortiaWin, consortiaLose, players, roomType, gameClass, totalKillHealth, count);
    }

    private void method_13(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.SendConsortiaFight(gspacketIn_0.ReadInt(), gspacketIn_0.ReadInt(), gspacketIn_0.ReadString());
    }

    private void method_14(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.RemoveGold(gspacketIn_0.ReadInt());
    }

    private void method_15(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.RemoveMoney(gspacketIn_0.ReadInt());
    }

    private void method_16(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.RemoveOffer(gspacketIn_0.ReadInt());
    }

    private void method_17(GSPacketIn gspacketIn_0)
    {
      GamePlayer playerById = WorldMgr.GetPlayerById(gspacketIn_0.ClientID);
      if (playerById == null)
        return;
      ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(gspacketIn_0.ReadInt());
      eBageType bagType = (eBageType) gspacketIn_0.ReadByte();
      if (itemTemplate == null)
        return;
      int count = gspacketIn_0.ReadInt();
      SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate, count, 118);
      fromTemplate.Count = count;
      fromTemplate.ValidDate = gspacketIn_0.ReadInt();
      fromTemplate.IsBinds = gspacketIn_0.ReadBoolean();
      fromTemplate.IsUsed = gspacketIn_0.ReadBoolean();
      playerById.AddTemplate(fromTemplate, bagType, fromTemplate.Count, true);
    }

    private void method_18(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.AddGP(gspacketIn_0.Parameter1);
    }

    private void method_19(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.AddMoney(gspacketIn_0.Parameter1);
    }

    private void method_21(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.AddGiftToken(gspacketIn_0.Parameter1);
    }

    private void method_22(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.RemoveGP(gspacketIn_0.Parameter1);
    }

    private void method_23(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.AddGold(gspacketIn_0.Parameter1);
    }

    private void method_24(GSPacketIn gspacketIn_0)
    {
      GamePlayer playerById = WorldMgr.GetPlayerById(gspacketIn_0.ClientID);
      if (playerById == null)
        return;
      int num = gspacketIn_0.ReadInt();
      bool bool_4 = playerById.UsePropItem((AbstractGame) null, gspacketIn_0.Parameter1, gspacketIn_0.Parameter2, num, gspacketIn_0.ReadBoolean());
      this.method_25(playerById.CurrentRoom.Game.Id, playerById.GameId, num, bool_4);
    }

    private void method_25(int int_2, int int_3, int int_4, bool bool_4)
    {
      GSPacketIn pkg = new GSPacketIn((short) 36, int_2);
      pkg.Parameter1 = int_3;
      pkg.Parameter2 = int_4;
      pkg.WriteBoolean(bool_4);
      this.SendTCP(pkg);
    }

    public void SendPlayerDisconnet(int gameId, int playerId, int roomid)
    {
      this.SendTCP(new GSPacketIn((short) 83, gameId)
      {
        Parameter1 = playerId
      });
    }

    private void HandlePlayerOnGameOver(GSPacketIn pkg)
    {
      GamePlayer playerById = WorldMgr.GetPlayerById(pkg.ClientID);
      if (playerById == null || playerById.CurrentRoom == null || playerById.CurrentRoom.Game == null)
        return;
      playerById.OnGameOver(playerById.CurrentRoom.Game, pkg.ReadBoolean(), pkg.ReadInt(), pkg.ReadBoolean(), pkg.ReadBoolean(), pkg.ReadInt(), pkg.ReadInt());
    }

    private void method_27(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.Disconnect();
    }

    public void SendRSALogin(RSACryptoServiceProvider rsa, string key)
    {
      GSPacketIn pkg = new GSPacketIn((short) 1);
      pkg.Write(rsa.Encrypt(Encoding.UTF8.GetBytes(key), false));
      this.SendTCP(pkg);
    }

    protected void HandleRSAKey(GSPacketIn packet)
    {
      RSAParameters parameters = new RSAParameters();
      parameters.Modulus = packet.ReadBytes(128);
      parameters.Exponent = packet.ReadBytes();
      RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
      rsa.ImportParameters(parameters);
      this.SendRSALogin(rsa, this.string_0);
    }

    private void method_28(GSPacketIn gspacketIn_0)
    {
      WorldMgr.GetPlayerById(gspacketIn_0.ClientID)?.RemoveHealstone();
    }

    public FightServerConnector(BattleServer server, string ip, int port, string key)
      : base(ip, port, true, new byte[8192], new byte[8192])
    {
      this.battleServer_0 = server;
      this.string_0 = key;
      this.Strict = true;
    }

    public void SendAddRoom(BaseRoom room)
    {
      GSPacketIn pkg = new GSPacketIn((short) 64);
      pkg.WriteInt(room.RoomId);
      pkg.WriteInt((int) room.RoomType);
      pkg.WriteInt((int) room.GameType);
      pkg.WriteInt(room.GuildId);
      pkg.WriteInt(room.PickUpNpcId);
      pkg.WriteBoolean(room.StartWithNpc);
      pkg.WriteBoolean(false);
      pkg.WriteBoolean(room.isCrosszone);
      List<GamePlayer> players = room.GetPlayers();
      pkg.WriteInt(players.Count);
      foreach (GamePlayer gamePlayer in players)
      {
        pkg.WriteInt(gamePlayer.PlayerCharacter.ID);
        pkg.WriteInt(gamePlayer.ZoneId);
        pkg.WriteString(gamePlayer.ZoneName);
        pkg.WriteInt(gamePlayer.CurrentEnemyId);
        pkg.WriteString(gamePlayer.PlayerCharacter.NickName);
        pkg.WriteBoolean(gamePlayer.PlayerCharacter.Sex);
        pkg.WriteInt(gamePlayer.PlayerCharacter.Hide);
        pkg.WriteString(gamePlayer.PlayerCharacter.Style);
        pkg.WriteString(gamePlayer.PlayerCharacter.Colors);
        pkg.WriteString(gamePlayer.PlayerCharacter.Skin);
        pkg.WriteInt(gamePlayer.PlayerCharacter.Offer);
        pkg.WriteInt(gamePlayer.PlayerCharacter.GP);
        pkg.WriteInt(gamePlayer.PlayerCharacter.Grade);
        pkg.WriteInt(gamePlayer.PlayerCharacter.Repute);
        pkg.WriteInt(gamePlayer.PlayerCharacter.ConsortiaID);
        pkg.WriteString(gamePlayer.PlayerCharacter.ConsortiaName);
        pkg.WriteInt(gamePlayer.PlayerCharacter.ConsortiaLevel);
        pkg.WriteInt(gamePlayer.PlayerCharacter.ConsortiaRepute);
        pkg.WriteBoolean(gamePlayer.PlayerCharacter.IsShowConsortia);
        pkg.WriteInt(gamePlayer.PlayerCharacter.badgeID);
        pkg.WriteString(gamePlayer.PlayerCharacter.Honor);
        pkg.WriteInt(gamePlayer.PlayerCharacter.AchievementPoint);
        pkg.WriteString(gamePlayer.PlayerCharacter.WeaklessGuildProgressStr);
        pkg.WriteInt(gamePlayer.PlayerCharacter.MoneyPlus);
        pkg.WriteInt(gamePlayer.PlayerCharacter.FightPower);
        pkg.WriteInt(gamePlayer.PlayerCharacter.apprenticeshipState);
        pkg.WriteInt(gamePlayer.PlayerCharacter.masterID);
        pkg.WriteString(gamePlayer.PlayerCharacter.masterOrApprentices);
        pkg.WriteInt(gamePlayer.PlayerCharacter.Attack);
        pkg.WriteInt(gamePlayer.PlayerCharacter.Defence);
        pkg.WriteInt(gamePlayer.PlayerCharacter.Agility);
        pkg.WriteInt(gamePlayer.PlayerCharacter.Luck);
        pkg.WriteInt(gamePlayer.PlayerCharacter.hp);
        pkg.WriteDouble(gamePlayer.GetBaseAttack());
        pkg.WriteDouble(gamePlayer.GetBaseDefence());
        pkg.WriteDouble(gamePlayer.GetBaseAgility());
        pkg.WriteDouble(gamePlayer.GetBaseBlood());
        pkg.WriteInt(gamePlayer.MainWeapon.TemplateID);
        pkg.WriteInt(gamePlayer.MainWeapon.StrengthenLevel);
        if (gamePlayer.MainWeapon.GoldEquip == null)
        {
          pkg.WriteInt(0);
        }
        else
        {
          pkg.WriteInt(gamePlayer.MainWeapon.GoldEquip.TemplateID);
          pkg.WriteDateTime(gamePlayer.MainWeapon.goldBeginTime);
          pkg.WriteInt(gamePlayer.MainWeapon.goldValidDate);
        }
        pkg.WriteBoolean(gamePlayer.CanUseProp);
        if (gamePlayer.SecondWeapon != null)
        {
          pkg.WriteInt(gamePlayer.SecondWeapon.TemplateID);
          pkg.WriteInt(gamePlayer.SecondWeapon.StrengthenLevel);
        }
        else
        {
          pkg.WriteInt(0);
          pkg.WriteInt(0);
        }
        if (gamePlayer.Healstone != null)
        {
          pkg.WriteInt(gamePlayer.Healstone.TemplateID);
          pkg.WriteInt(gamePlayer.Healstone.Count);
        }
        else
        {
          pkg.WriteInt(0);
          pkg.WriteInt(0);
        }
        pkg.WriteDouble(gamePlayer.GPAddPlus == 0.0 ? 1.0 : gamePlayer.GPAddPlus);
        pkg.WriteDouble(gamePlayer.OfferAddPlus == 0.0 ? 1.0 : gamePlayer.OfferAddPlus);
        pkg.WriteDouble(gamePlayer.GPApprenticeOnline);
        pkg.WriteDouble(gamePlayer.GPApprenticeTeam);
        pkg.WriteDouble(gamePlayer.GPSpouseTeam);
        pkg.WriteInt(GameServer.Instance.Configuration.ServerID);
        List<AbstractBuffer> allBuffer = gamePlayer.BufferList.GetAllBuffer();
        pkg.WriteInt(allBuffer.Count);
        foreach (AbstractBuffer abstractBuffer in allBuffer)
        {
          BufferInfo info = abstractBuffer.Info;
          pkg.WriteInt(info.Type);
          pkg.WriteBoolean(info.IsExist);
          pkg.WriteDateTime(info.BeginDate);
          pkg.WriteInt(info.ValidDate);
          pkg.WriteInt(info.Value);
        }
        pkg.WriteInt(gamePlayer.EquipEffect.Count);
        foreach (int val in gamePlayer.EquipEffect)
          pkg.WriteInt(val);
        pkg.WriteInt(gamePlayer.FightBuffs.Count);
        foreach (BufferInfo fightBuff in gamePlayer.FightBuffs)
        {
          pkg.WriteInt(fightBuff.Type);
          pkg.WriteInt(fightBuff.Value);
        }
        pkg.WriteByte(gamePlayer.PlayerCharacter.typeVIP);
        pkg.WriteInt(gamePlayer.PlayerCharacter.VIPLevel);
        pkg.WriteDateTime(gamePlayer.PlayerCharacter.VIPExpireDay);
        pkg.WriteBoolean(gamePlayer.MatchInfo.DailyLeagueFirst);
        pkg.WriteInt(gamePlayer.MatchInfo.DailyLeagueLastScore);
        bool val1 = gamePlayer.Pet != null;
        pkg.WriteBoolean(val1);
        if (val1)
        {
          pkg.WriteInt(gamePlayer.Pet.Place);
          pkg.WriteInt(gamePlayer.Pet.TemplateID);
          pkg.WriteInt(gamePlayer.Pet.ID);
          pkg.WriteString(gamePlayer.Pet.Name);
          pkg.WriteInt(gamePlayer.Pet.UserID);
          pkg.WriteInt(gamePlayer.Pet.Level);
          pkg.WriteString(gamePlayer.Pet.Skill);
          pkg.WriteString(gamePlayer.Pet.SkillEquip);
        }
      }
      this.SendTCP(pkg);
    }

    public void SendRemoveRoom(BaseRoom room)
    {
      this.SendTCP(new GSPacketIn((short) 65)
      {
        Parameter1 = room.RoomId
      });
    }

    public void SendToGame(int gameId, GSPacketIn pkg)
    {
      GSPacketIn pkg1 = new GSPacketIn((short) 2, gameId);
      pkg1.WritePacket(pkg);
      this.SendTCP(pkg1);
    }

    protected void HandleRoomRemove(GSPacketIn packet)
    {
      this.battleServer_0.RemoveRoomImp(packet.ClientID);
    }

    protected void HandleStartGame(GSPacketIn pkg)
    {
      ProxyGame game = new ProxyGame(pkg.Parameter2, this, (eRoomType) pkg.ReadInt(), (eGameType) pkg.ReadInt(), pkg.ReadInt());
      this.battleServer_0.StartGame(pkg.Parameter1, game);
    }

    protected void HandleStopGame(GSPacketIn pkg)
    {
      this.battleServer_0.StopGame(pkg.Parameter1, pkg.Parameter2);
    }

    protected void HandleSendToRoom(GSPacketIn pkg)
    {
      this.battleServer_0.SendToRoom(pkg.ClientID, pkg.ReadPacket(), pkg.Parameter1, pkg.Parameter2);
    }

    protected void HandleSendToPlayer(GSPacketIn pkg)
    {
      int clientId = pkg.ClientID;
      try
      {
        GSPacketIn pkg1 = pkg.ReadPacket();
        this.battleServer_0.SendToUser(clientId, pkg1);
      }
      catch (Exception ex)
      {
        FightServerConnector.ilog_2.Error((object) string.Format("pkg len:{0}", (object) pkg.Length), ex);
        FightServerConnector.ilog_2.Error((object) Marshal.ToHexDump("pkg content:", pkg.Buffer, 0, pkg.Length));
      }
    }

    private void method_29(GSPacketIn gspacketIn_0)
    {
      this.battleServer_0.UpdatePlayerGameId(gspacketIn_0.Parameter1, gspacketIn_0.Parameter2);
    }

    public void SendChatMessage(string msg, GamePlayer player, bool team)
    {
      GSPacketIn pkg = new GSPacketIn((short) 19, player.CurrentRoom.Game.Id);
      pkg.WriteInt(player.GameId);
      pkg.WriteBoolean(team);
      pkg.WriteString(msg);
      this.SendTCP(pkg);
    }

    public void SendFightNotice(GamePlayer player, int GameId)
    {
      this.SendTCP(new GSPacketIn((short) 3, GameId)
      {
        Parameter1 = player.GameId
      });
    }

    public void SendFindConsortiaAlly(int state, int gameid)
    {
      GSPacketIn pkg = new GSPacketIn((short) 69, gameid);
      pkg.WriteInt(state);
      pkg.WriteInt((int) RateMgr.GetRate(eRateType.Riches_Rate));
      this.SendTCP(pkg);
    }
  }
}
