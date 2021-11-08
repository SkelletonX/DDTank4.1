// Decompiled with JetBrains decompiler
// Type: Game.Server.LoginCrosszoneServerConnector
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Bussiness.Protocol;
using Game.Base;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Rooms;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Game.Server
{
  public class LoginCrosszoneServerConnector : BaseConnector
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private string m_loginKey;
    private int m_serverId;

    public LoginCrosszoneServerConnector(
      string ip,
      int port,
      int serverid,
      string name,
      byte[] readBuffer,
      byte[] sendBuffer)
      : base(ip, port, true, readBuffer, sendBuffer)
    {
      this.m_serverId = serverid;
      this.m_loginKey = string.Format("{0},{1}", (object) serverid, (object) name);
      this.Strict = true;
    }

    protected void AsynProcessPacket(object state)
    {
      try
      {
        GSPacketIn gsPacketIn = state as GSPacketIn;
        int code = (int) gsPacketIn.Code;
        if (code <= 117)
        {
          if (code <= 38)
          {
            switch (code)
            {
              case 0:
                this.HandleRSAKey(gsPacketIn);
                break;
              case 2:
                //this.HandleKitoffPlayer((object) gsPacketIn);
                break;
              case 3:
                this.HandleAllowUserLogin((object) gsPacketIn);
                break;
              case 4:
                this.HandleUserOffline(gsPacketIn);
                break;
              case 5:
                this.HandleUserOnline(gsPacketIn);
                break;
              case 7:
                this.HandleASSState(gsPacketIn);
                break;
              case 8:
                this.HandleConfigState(gsPacketIn);
                break;
              case 9:
                //this.HandleChargeMoney(gsPacketIn);
                break;
              case 10:
                //this.HandleSystemNotice(gsPacketIn);
                break;
              case 13:
                this.HandleUpdatePlayerMarriedState(gsPacketIn);
                break;
              case 14:
                this.HandleMarryRoomInfoToPlayer(gsPacketIn);
                break;
              case 15:
                this.HandleShutdown(gsPacketIn);
                break;
              case 19:
                this.HandleChatConsortia(gsPacketIn);
                break;
              case 37:
                this.HandleChatPersonal(gsPacketIn);
                break;
              case 38:
                //this.HandleSysMess(gsPacketIn);
                break;
            }
          }
          else
          {
            switch (code - 72)
            {
              case 0:
                this.HandleBigBugle(gsPacketIn);
                goto case 1;
              case 1:
              case 2:
              case 3:
              case 4:
              case 5:
              case 6:
              case 12:
              case 14:
                break;
              default:
                if (code == 117)
                {
                  this.HandleMailResponse(gsPacketIn);
                  goto case 1;
                }
                else
                  goto case 1;
            }
          }
        }
        else if (code <= 160)
        {
          switch (code - 128)
          {
            case 0:
              this.HandleConsortiaResponse(gsPacketIn);
              break;
            case 1:
              break;
            case 2:
              this.HandleConsortiaCreate(gsPacketIn);
              break;
            default:
              switch (code - 158)
              {
                case 0:
                  this.HandleConsortiaFight(gsPacketIn);
                  return;
                case 1:
                  return;
                case 2:
                  this.HandleFriend(gsPacketIn);
                  return;
                default:
                  return;
              }
          }
        }
        else
        {
          switch (code - 177)
          {
            case 0:
              this.HandleRate(gsPacketIn);
              goto case 2;
            case 1:
              this.HandleMacroDrop(gsPacketIn);
              goto case 2;
            case 2:
              break;
            case 3:
              this.HandleConsortiaBossInfo(gsPacketIn);
              goto case 2;
            default:
              switch (code - 903)
              {
                case 0:
                  this.HandlerEliteGameKickOut(gsPacketIn);
                  break;
                case 1:
                  this.HandlerEliteGameStatusUpdate(gsPacketIn);
                  break;
                case 3:
                  this.HandlerEliteGameUpdateRank(gsPacketIn);
                  break;
                case 4:
                  this.HandleEliteGameRequestStart(gsPacketIn);
                  break;
                case 6:
                  this.HandlerEliteGameRoundAdd(gsPacketIn);
                  break;
                case 8:
                  this.HandlerEliteGameSynPlayers(gsPacketIn);
                  break;
                case 9:
                  this.HandleEliteGameReload(gsPacketIn);
                  break;
              }
              if (code == 185)
              {
                this.HandleConsortiaBossSendAward(gsPacketIn);
                goto case 2;
              }
              else
                goto case 2;
          }
        }
      }
      catch (Exception ex)
      {
        GameServer.log.Error((object) nameof (AsynProcessPacket), ex);
      }
    }

    protected void HandleAllowUserLogin(object stateInfo)
    {
      try
      {
        GSPacketIn gsPacketIn = (GSPacketIn) stateInfo;
        int num = gsPacketIn.ReadInt();
        if (!gsPacketIn.ReadBoolean())
          return;
        GamePlayer gamePlayer = LoginMgr.LoginClient(num);
        if (gamePlayer != null)
        {
          if (gamePlayer.Login())
          {
            this.SendUserOnline(num, gamePlayer.PlayerCharacter.ConsortiaID);
            WorldMgr.OnPlayerOnline(num, gamePlayer.PlayerCharacter.ConsortiaID);
          }
          else
          {
            gamePlayer.Client.Disconnect();
            this.SendUserOffline(num, 0);
          }
        }
        else
          this.SendUserOffline(num, 0);
      }
      catch (Exception ex)
      {
        GameServer.log.Error((object) nameof (HandleAllowUserLogin), ex);
      }
    }

    public void HandleASSState(GSPacketIn packet)
    {
      bool flag = packet.ReadBoolean();
      AntiAddictionMgr.SetASSState(flag);
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
        allPlayer.Out.SendAASControl(flag, allPlayer.IsAASInfo, allPlayer.IsMinor);
    }

    protected void HandleBigBugle(GSPacketIn packet)
    {
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
        allPlayer.Out.SendTCP(packet);
    }

    public void HandleConfigState(GSPacketIn packet)
    {
      bool flag = packet.ReadBoolean();
      AwardMgr.DailyAwardState = packet.ReadBoolean();
      AntiAddictionMgr.SetASSState(flag);
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
        allPlayer.Out.SendAASControl(flag, allPlayer.IsAASInfo, allPlayer.IsMinor);
    }

    public void HandleConsortiaAlly(GSPacketIn packet)
    {
      int cosortiaID1 = packet.ReadInt();
      int consortiaID2 = packet.ReadInt();
      int state = packet.ReadInt();
      ConsortiaMgr.UpdateConsortiaAlly(cosortiaID1, consortiaID2, state);
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ConsortiaID == cosortiaID1 || allPlayer.PlayerCharacter.ConsortiaID == consortiaID2)
          allPlayer.Out.SendTCP(packet);
      }
    }

    public void HandleConsortiaBanChat(GSPacketIn packet)
    {
      bool flag = packet.ReadBoolean();
      int num = packet.ReadInt();
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ID == num)
        {
          allPlayer.PlayerCharacter.IsBanChat = flag;
          allPlayer.Out.SendTCP(packet);
          break;
        }
      }
    }

    public void HandleConsortiaBossClose(ConsortiaInfo consortia)
    {
      this.SendToAllConsortiaMember(consortia, 1);
    }

    public void HandleConsortiaBossCreateBoss(ConsortiaInfo consortia)
    {
      this.SendToAllConsortiaMember(consortia, 0);
    }

    public void HandleConsortiaBossDie(ConsortiaInfo consortia)
    {
      this.SendToAllConsortiaMember(consortia, 2);
    }

    public void HandleConsortiaBossExtendAvailable(ConsortiaInfo consortia)
    {
      this.SendToAllConsortiaMember(consortia, 3);
    }

    public void HandleConsortiaBossInfo(GSPacketIn pkg)
    {
      ConsortiaInfo consortia = new ConsortiaInfo()
      {
        ConsortiaID = pkg.ReadInt(),
        ChairmanID = pkg.ReadInt(),
        bossState = (int) pkg.ReadByte(),
        endTime = pkg.ReadDateTime(),
        extendAvailableNum = pkg.ReadInt(),
        callBossLevel = pkg.ReadInt(),
        Level = pkg.ReadInt(),
        SmithLevel = pkg.ReadInt(),
        StoreLevel = pkg.ReadInt(),
        SkillLevel = pkg.ReadInt(),
        Riches = pkg.ReadInt(),
        LastOpenBoss = pkg.ReadDateTime(),
        MaxBlood = pkg.ReadLong(),
        TotalAllMemberDame = pkg.ReadLong(),
        IsBossDie = pkg.ReadBoolean(),
        RankList = new Dictionary<string, RankingPersonInfo>()
      };
      int num = pkg.ReadInt();
      for (int index = 0; index < num; ++index)
      {
        RankingPersonInfo rankingPersonInfo = new RankingPersonInfo()
        {
          Name = pkg.ReadString(),
          ID = pkg.ReadInt(),
          TotalDamage = pkg.ReadInt(),
          Honor = pkg.ReadInt(),
          Damage = pkg.ReadInt()
        };
        consortia.RankList.Add(rankingPersonInfo.Name, rankingPersonInfo);
      }
      switch (pkg.ReadByte())
      {
        case 180:
          this.SendToAllConsortiaMember(consortia, -1);
          break;
        case 182:
          this.HandleConsortiaBossExtendAvailable(consortia);
          break;
        case 183:
          this.HandleConsortiaBossCreateBoss(consortia);
          break;
        case 184:
          this.HandleConsortiaBossReload(consortia);
          break;
        case 187:
          this.HandleConsortiaBossClose(consortia);
          break;
        case 188:
          this.HandleConsortiaBossDie(consortia);
          break;
      }
    }

    public void HandleConsortiaBossReload(ConsortiaInfo consortia)
    {
      this.SendToAllConsortiaMember(consortia, -1);
    }

    public void HandleConsortiaBossSendAward(GSPacketIn pkg)
    {
      int num = pkg.ReadInt();
      for (int index = 0; index < num; ++index)
        ConsortiaBossMgr.SendConsortiaAward(pkg.ReadInt());
    }

    public void HandleConsortiaCreate(GSPacketIn packet)
    {
      int consortiaID = packet.ReadInt();
      packet.ReadInt();
      ConsortiaMgr.AddConsortia(consortiaID);
    }

    public void HandleConsortiaDelete(GSPacketIn packet)
    {
      int num = packet.ReadInt();
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ConsortiaID == num)
        {
          allPlayer.ClearConsortia(true);
          allPlayer.AddRobRiches(-allPlayer.PlayerCharacter.RichesRob);
          allPlayer.Out.SendTCP(packet);
        }
      }
    }

    public void HandleConsortiaDuty(GSPacketIn packet)
    {
      int num1 = (int) packet.ReadByte();
      int num2 = packet.ReadInt();
      int num3 = packet.ReadInt();
      packet.ReadString();
      int num4 = packet.ReadInt();
      string str = packet.ReadString();
      int num5 = packet.ReadInt();
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ConsortiaID == num2)
        {
          if (num1 == 2 && allPlayer.PlayerCharacter.DutyLevel == num4)
            allPlayer.PlayerCharacter.DutyName = str;
          else if (allPlayer.PlayerCharacter.ID == num3 && (num1 == 5 || num1 == 6 || (num1 == 7 || num1 == 8) || num1 == 9))
          {
            allPlayer.PlayerCharacter.DutyLevel = num4;
            allPlayer.PlayerCharacter.DutyName = str;
            allPlayer.PlayerCharacter.Right = num5;
          }
          allPlayer.Out.SendTCP(packet);
        }
      }
    }

    public void HandleConsortiaFight(GSPacketIn packet)
    {
      int num = packet.ReadInt();
      packet.ReadInt();
      string message = packet.ReadString();
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ConsortiaID == num)
          allPlayer.Out.SendMessage(eMessageType.ChatNormal, message);
      }
    }

    protected void HandleConsortiaResponse(GSPacketIn packet)
    {
      switch (packet.ReadByte())
      {
        case 1:
          this.HandleConsortiaUserPass(packet);
          break;
        case 2:
          this.HandleConsortiaDelete(packet);
          break;
        case 3:
          this.HandleConsortiaUserDelete(packet);
          break;
        case 4:
          this.HandleConsortiaUserInvite(packet);
          break;
        case 5:
          this.HandleConsortiaBanChat(packet);
          break;
        case 6:
          this.HandleConsortiaUpGrade(packet);
          break;
        case 7:
          this.HandleConsortiaAlly(packet);
          break;
        case 8:
          this.HandleConsortiaDuty(packet);
          break;
        case 9:
          this.HandleConsortiaRichesOffer(packet);
          break;
        case 10:
          this.HandleConsortiaShopUpGrade(packet);
          break;
        case 11:
          this.HandleConsortiaSmithUpGrade(packet);
          break;
        case 12:
          this.HandleConsortiaStoreUpGrade(packet);
          break;
      }
    }

    public void HandleConsortiaRichesOffer(GSPacketIn packet)
    {
      int num = packet.ReadInt();
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ConsortiaID == num)
          allPlayer.Out.SendTCP(packet);
      }
    }

    public void HandleConsortiaShopUpGrade(GSPacketIn packet)
    {
      int consortiaID = packet.ReadInt();
      packet.ReadString();
      int shopLevel = packet.ReadInt();
      ConsortiaMgr.ConsortiaShopUpGrade(consortiaID, shopLevel);
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ConsortiaID == consortiaID)
        {
          allPlayer.PlayerCharacter.ShopLevel = shopLevel;
          allPlayer.Out.SendTCP(packet);
        }
      }
    }

    public void HandleConsortiaSmithUpGrade(GSPacketIn packet)
    {
      int consortiaID = packet.ReadInt();
      packet.ReadString();
      int smithLevel = packet.ReadInt();
      ConsortiaMgr.ConsortiaSmithUpGrade(consortiaID, smithLevel);
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ConsortiaID == consortiaID)
        {
          allPlayer.PlayerCharacter.SmithLevel = smithLevel;
          allPlayer.Out.SendTCP(packet);
        }
      }
    }

    public void HandleConsortiaStoreUpGrade(GSPacketIn packet)
    {
      int consortiaID = packet.ReadInt();
      packet.ReadString();
      int storeLevel = packet.ReadInt();
      ConsortiaMgr.ConsortiaStoreUpGrade(consortiaID, storeLevel);
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ConsortiaID == consortiaID)
        {
          allPlayer.PlayerCharacter.StoreLevel = storeLevel;
          allPlayer.Out.SendTCP(packet);
        }
      }
    }

    public void HandleConsortiaUpGrade(GSPacketIn packet)
    {
      int consortiaID = packet.ReadInt();
      packet.ReadString();
      int consortiaLevel = packet.ReadInt();
      ConsortiaMgr.ConsortiaUpGrade(consortiaID, consortiaLevel);
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ConsortiaID == consortiaID)
        {
          allPlayer.PlayerCharacter.ConsortiaLevel = consortiaLevel;
          allPlayer.Out.SendTCP(packet);
        }
      }
    }

    public void HandleConsortiaUserDelete(GSPacketIn packet)
    {
      int num1 = packet.ReadInt();
      int num2 = packet.ReadInt();
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ConsortiaID == num2 || allPlayer.PlayerCharacter.ID == num1)
        {
          if (allPlayer.PlayerCharacter.ID == num1)
            allPlayer.ClearConsortia(true);
          allPlayer.Out.SendTCP(packet);
        }
      }
    }

    public void HandleConsortiaUserInvite(GSPacketIn packet)
    {
      packet.ReadInt();
      int num = packet.ReadInt();
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ID == num)
        {
          allPlayer.Out.SendTCP(packet);
          break;
        }
      }
    }

    public void HandleConsortiaUserPass(GSPacketIn packet)
    {
      packet.ReadInt();
      packet.ReadBoolean();
      int consortiaID = packet.ReadInt();
      string str1 = packet.ReadString();
      int num1 = packet.ReadInt();
      packet.ReadString();
      packet.ReadInt();
      packet.ReadString();
      packet.ReadInt();
      string str2 = packet.ReadString();
      packet.ReadInt();
      packet.ReadInt();
      packet.ReadInt();
      packet.ReadDateTime();
      packet.ReadInt();
      int num2 = packet.ReadInt();
      packet.ReadInt();
      packet.ReadBoolean();
      int num3 = packet.ReadInt();
      packet.ReadInt();
      packet.ReadInt();
      packet.ReadInt();
      int num4 = packet.ReadInt();
      packet.ReadString();
      packet.ReadInt();
      packet.ReadInt();
      packet.ReadString();
      packet.ReadInt();
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ID == num1)
        {
          allPlayer.BeginChanges();
          allPlayer.PlayerCharacter.ConsortiaID = consortiaID;
          allPlayer.PlayerCharacter.ConsortiaName = str1;
          allPlayer.PlayerCharacter.DutyName = str2;
          allPlayer.PlayerCharacter.DutyLevel = num2;
          allPlayer.PlayerCharacter.Right = num3;
          allPlayer.PlayerCharacter.ConsortiaRepute = num4;
          ConsortiaInfo consortiaInfo = ConsortiaMgr.FindConsortiaInfo(consortiaID);
          if (consortiaInfo != null)
            allPlayer.PlayerCharacter.ConsortiaLevel = consortiaInfo.Level;
          allPlayer.CommitChanges();
        }
        if (allPlayer.PlayerCharacter.ConsortiaID == consortiaID)
          allPlayer.Out.SendTCP(packet);
      }
    }

    public void HandleChargeMoney(GSPacketIn packet)
    {
      WorldMgr.GetPlayerById(packet.ClientID)?.ChargeToUser();
    }

    protected void HandleChatConsortia(GSPacketIn packet)
    {
      int num1 = (int) packet.ReadByte();
      packet.ReadBoolean();
      packet.ReadString();
      packet.ReadString();
      int num2 = packet.ReadInt();
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ConsortiaID == num2)
          allPlayer.Out.SendTCP(packet);
      }
    }

    protected void HandleChatPersonal(GSPacketIn packet)
    {
      packet.ReadInt();
      string str1 = packet.ReadString();
      string str2 = packet.ReadString();
      string msg = packet.ReadString();
      bool isAutoReply = packet.ReadBoolean();
      int playerID = 0;
      GamePlayer byPlayerNickName1 = WorldMgr.GetClientByPlayerNickName(str1);
      GamePlayer byPlayerNickName2 = WorldMgr.GetClientByPlayerNickName(str2);
      if (byPlayerNickName2 != null)
        playerID = byPlayerNickName2.PlayerCharacter.ID;
      if (byPlayerNickName1 == null || byPlayerNickName1.IsBlackFriend(playerID))
        return;
      int id = byPlayerNickName1.PlayerCharacter.ID;
      byPlayerNickName1.SendPrivateChat(id, str1, str2, msg, isAutoReply);
    }

    public void HandleFirendResponse(GSPacketIn packet)
    {
      WorldMgr.GetPlayerById(packet.ReadInt())?.Out.SendTCP(packet);
    }

    public void HandleFriend(GSPacketIn pkg)
    {
      switch (pkg.ReadByte())
      {
        case 165:
          this.HandleFriendState(pkg);
          break;
        case 166:
          this.HandleFirendResponse(pkg);
          break;
      }
    }

    public void HandleFriendState(GSPacketIn pkg)
    {
      WorldMgr.ChangePlayerState(pkg.ClientID, pkg.ReadInt(), pkg.ReadInt());
    }

    protected void HandleKitoffPlayer(object stateInfo)
    {
      try
      {
        GSPacketIn gsPacketIn = (GSPacketIn) stateInfo;
        int num = gsPacketIn.ReadInt();
        GamePlayer playerById = WorldMgr.GetPlayerById(num);
        if (playerById != null)
        {
          string msg = gsPacketIn.ReadString();
          playerById.Out.SendKitoff(msg);
          playerById.Client.Disconnect();
        }
        else
          this.SendUserOffline(num, 0);
      }
      catch (Exception ex)
      {
        GameServer.log.Error((object) nameof (HandleKitoffPlayer), ex);
      }
    }

    public void HandleMacroDrop(GSPacketIn pkg)
    {
      Dictionary<int, MacroDropInfo> temp = new Dictionary<int, MacroDropInfo>();
      int num = pkg.ReadInt();
      for (int index = 0; index < num; ++index)
      {
        int key = pkg.ReadInt();
        MacroDropInfo macroDropInfo = new MacroDropInfo(pkg.ReadInt(), pkg.ReadInt());
        temp.Add(key, macroDropInfo);
      }
      MacroDropMgr.UpdateDropInfo(temp);
    }

    public void HandleMailResponse(GSPacketIn packet)
    {
      WorldMgr.GetPlayerById(packet.ReadInt())?.Out.SendTCP(packet);
    }

    public void HandleMarryRoomInfoToPlayer(GSPacketIn packet)
    {
      int playerId = packet.ReadInt();
      GamePlayer playerById = WorldMgr.GetPlayerById(playerId);
      if (playerById == null)
        return;
      packet.Code = (short) 252;
      packet.ClientID = playerId;
      playerById.Out.SendTCP(packet);
    }

    public void HandleRate(GSPacketIn packet)
    {
      RateMgr.ReLoad();
    }

    public void HandleReload(GSPacketIn packet)
    {
      eReloadType eReloadType = (eReloadType) packet.ReadInt();
      bool val = false;
      switch (eReloadType)
      {
        case eReloadType.ball:
          val = BallMgr.ReLoad();
          break;
        case eReloadType.map:
          val = MapMgr.ReLoadMap();
          break;
        case eReloadType.mapserver:
          val = MapMgr.ReLoadMapServer();
          break;
        case eReloadType.item:
          val = ItemMgr.ReLoad();
          break;
        case eReloadType.quest:
          val = QuestMgr.ReLoad();
          break;
        case eReloadType.fusion:
          val = FusionMgr.ReLoad();
          break;
        case eReloadType.server:
          GameServer.Instance.Configuration.Refresh();
          break;
        case eReloadType.rate:
          val = RateMgr.ReLoad();
          break;
        case eReloadType.consortia:
          val = ConsortiaMgr.ReLoad();
          break;
        case eReloadType.shop:
          val = ShopMgr.ReLoad();
          break;
        case eReloadType.fight:
          val = FightRateMgr.ReLoad();
          break;
        case eReloadType.dailyaward:
          val = AwardMgr.ReLoad();
          break;
        case eReloadType.language:
          val = LanguageMgr.Reload("");
          break;
      }
      packet.WriteInt(GameServer.Instance.Configuration.ServerID);
      packet.WriteBoolean(val);
      this.SendTCP(packet);
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
      this.SendRSALogin(rsa, this.m_loginKey);
      this.SendListenIPPort(IPAddress.Parse(GameServer.Instance.Configuration.Ip), GameServer.Instance.Configuration.Port);
    }

    public void HandleShutdown(GSPacketIn pkg)
    {
      GameServer.Instance.Shutdown();
    }

    public void HandleSysMess(GSPacketIn packet)
    {
      if (packet.ReadInt() != 1)
        return;
      WorldMgr.GetPlayerById(packet.ReadInt())?.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("LoginServerConnector.HandleSysMess.Msg1", (object) packet.ReadString()));
    }

    public void HandleSystemNotice(GSPacketIn packet)
    {
      //foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
       // allPlayer.Out.SendTCP(packet);
    }

    public void HandleUpdatePlayerMarriedState(GSPacketIn packet)
    {
      GamePlayer playerById = WorldMgr.GetPlayerById(packet.ReadInt());
      if (playerById == null)
        return;
      playerById.LoadMarryProp();
      playerById.LoadMarryMessage();
      playerById.QuestInventory.ClearMarryQuest();
    }

    protected void HandleUserOffline(GSPacketIn packet)
    {
      int num1 = packet.ReadInt();
      for (int index = 0; index < num1; ++index)
      {
        int num2 = packet.ReadInt();
        int consortiaID = packet.ReadInt();
        if (LoginMgr.ContainsUser(num2))
          this.SendAllowUserLogin(num2);
        WorldMgr.OnPlayerOffline(num2, consortiaID);
      }
    }

    protected void HandleUserOnline(GSPacketIn packet)
    {
      int num1 = packet.ReadInt();
      for (int index = 0; index < num1; ++index)
      {
        int num2 = packet.ReadInt();
        int consortiaID = packet.ReadInt();
        LoginMgr.ClearLoginPlayer(num2);
        GamePlayer playerById = WorldMgr.GetPlayerById(num2);
        if (playerById != null)
        {
          GameServer.log.Error((object) "Player hang in server!!!");
          playerById.Out.SendKitoff(LanguageMgr.GetTranslation("Game.Server.LoginNext"));
          playerById.Client.Disconnect();
        }
        WorldMgr.OnPlayerOnline(num2, consortiaID);
      }
    }

    protected override void OnDisconnect()
    {
      base.OnDisconnect();
    }

    public override void OnRecvPacket(GSPacketIn pkg)
    {
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.AsynProcessPacket), (object) pkg);
    }

    public void SendAllowUserLogin(int playerid)
    {
      GSPacketIn pkg = new GSPacketIn((short) 3);
      pkg.WriteInt(playerid);
      this.SendTCP(pkg);
    }

    public void SendConsortiaAlly(int consortiaID1, int consortiaID2, int state)
    {
      GSPacketIn pkg = new GSPacketIn((short) 128);
      pkg.WriteByte((byte) 7);
      pkg.WriteInt(consortiaID1);
      pkg.WriteInt(consortiaID2);
      pkg.WriteInt(state);
      this.SendTCP(pkg);
      ConsortiaMgr.UpdateConsortiaAlly(consortiaID1, consortiaID2, state);
    }

    public void SendConsortiaBanChat(
      int playerid,
      string playerName,
      int handleID,
      string handleName,
      bool isBan)
    {
      GSPacketIn pkg = new GSPacketIn((short) 128);
      pkg.WriteByte((byte) 5);
      pkg.WriteBoolean(isBan);
      pkg.WriteInt(playerid);
      pkg.WriteString(playerName);
      pkg.WriteInt(handleID);
      pkg.WriteString(handleName);
      this.SendTCP(pkg);
    }

    public void SendConsortiaCreate(int consortiaID, int offer, string consotiaName)
    {
      GSPacketIn pkg = new GSPacketIn((short) 130);
      pkg.WriteInt(consortiaID);
      pkg.WriteInt(offer);
      pkg.WriteString(consotiaName);
      this.SendTCP(pkg);
    }

    public void SendConsortiaDelete(int consortiaID)
    {
      GSPacketIn pkg = new GSPacketIn((short) 128);
      pkg.WriteByte((byte) 2);
      pkg.WriteInt(consortiaID);
      this.SendTCP(pkg);
    }

    public void SendConsortiaDuty(ConsortiaDutyInfo info, int updateType, int consortiaID)
    {
      this.SendConsortiaDuty(info, updateType, consortiaID, 0, "", 0, "");
    }

    public void SendConsortiaDuty(
      ConsortiaDutyInfo info,
      int updateType,
      int consortiaID,
      int playerID,
      string playerName,
      int handleID,
      string handleName)
    {
      GSPacketIn pkg = new GSPacketIn((short) 128);
      pkg.WriteByte((byte) 8);
      pkg.WriteByte((byte) updateType);
      pkg.WriteInt(consortiaID);
      pkg.WriteInt(playerID);
      pkg.WriteString(playerName);
      pkg.WriteInt(info.Level);
      pkg.WriteString(info.DutyName);
      pkg.WriteInt(info.Right);
      pkg.WriteInt(handleID);
      pkg.WriteString(handleName);
      this.SendTCP(pkg);
    }

    public void SendConsortiaFight(int consortiaID, int riches, string msg)
    {
      GSPacketIn pkg = new GSPacketIn((short) 158);
      pkg.WriteInt(consortiaID);
      pkg.WriteInt(riches);
      pkg.WriteString(msg);
      this.SendTCP(pkg);
    }

    public void SendConsortiaInvite(
      int ID,
      int playerid,
      string playerName,
      int inviteID,
      string intviteName,
      string consortiaName,
      int consortiaID)
    {
      GSPacketIn pkg = new GSPacketIn((short) 128);
      pkg.WriteByte((byte) 4);
      pkg.WriteInt(ID);
      pkg.WriteInt(playerid);
      pkg.WriteString(playerName);
      pkg.WriteInt(inviteID);
      pkg.WriteString(intviteName);
      pkg.WriteInt(consortiaID);
      pkg.WriteString(consortiaName);
      this.SendTCP(pkg);
    }

    public void SendConsortiaKillUpGrade(ConsortiaInfo info)
    {
      GSPacketIn pkg = new GSPacketIn((short) 128);
      pkg.WriteByte((byte) 13);
      pkg.WriteInt(info.ConsortiaID);
      pkg.WriteString(info.ConsortiaName);
      pkg.WriteInt(info.SkillLevel);
      this.SendTCP(pkg);
    }

    public void SendConsortiaOffer(int consortiaID, int offer, int riches)
    {
      GSPacketIn pkg = new GSPacketIn((short) 156);
      pkg.WriteInt(consortiaID);
      pkg.WriteInt(offer);
      pkg.WriteInt(riches);
      this.SendTCP(pkg);
    }

    public void SendConsortiaRichesOffer(
      int consortiaID,
      int playerID,
      string playerName,
      int riches)
    {
      GSPacketIn pkg = new GSPacketIn((short) 128);
      pkg.WriteByte((byte) 9);
      pkg.WriteInt(consortiaID);
      pkg.WriteInt(playerID);
      pkg.WriteString(playerName);
      pkg.WriteInt(riches);
      this.SendTCP(pkg);
    }

    public void SendConsortiaShopUpGrade(ConsortiaInfo info)
    {
      GSPacketIn pkg = new GSPacketIn((short) 128);
      pkg.WriteByte((byte) 10);
      pkg.WriteInt(info.ConsortiaID);
      pkg.WriteString(info.ConsortiaName);
      pkg.WriteInt(info.ShopLevel);
      this.SendTCP(pkg);
    }

    public void SendConsortiaSmithUpGrade(ConsortiaInfo info)
    {
      GSPacketIn pkg = new GSPacketIn((short) 128);
      pkg.WriteByte((byte) 11);
      pkg.WriteInt(info.ConsortiaID);
      pkg.WriteString(info.ConsortiaName);
      pkg.WriteInt(info.SmithLevel);
      this.SendTCP(pkg);
    }

    public void SendConsortiaStoreUpGrade(ConsortiaInfo info)
    {
      GSPacketIn pkg = new GSPacketIn((short) 128);
      pkg.WriteByte((byte) 12);
      pkg.WriteInt(info.ConsortiaID);
      pkg.WriteString(info.ConsortiaName);
      pkg.WriteInt(info.StoreLevel);
      this.SendTCP(pkg);
    }

    public void SendConsortiaUpGrade(ConsortiaInfo info)
    {
      GSPacketIn pkg = new GSPacketIn((short) 128);
      pkg.WriteByte((byte) 6);
      pkg.WriteInt(info.ConsortiaID);
      pkg.WriteString(info.ConsortiaName);
      pkg.WriteInt(info.Level);
      this.SendTCP(pkg);
    }

    public void SendConsortiaUserDelete(
      int playerid,
      int consortiaID,
      bool isKick,
      string nickName,
      string kickName)
    {
      GSPacketIn pkg = new GSPacketIn((short) 128);
      pkg.WriteByte((byte) 3);
      pkg.WriteInt(playerid);
      pkg.WriteInt(consortiaID);
      pkg.WriteBoolean(isKick);
      pkg.WriteString(nickName);
      pkg.WriteString(kickName);
      this.SendTCP(pkg);
    }

    public void SendConsortiaUserPass(
      int playerid,
      string playerName,
      ConsortiaUserInfo info,
      bool isInvite,
      int consortiaRepute)
    {
      GSPacketIn pkg = new GSPacketIn((short) 128, playerid);
      pkg.WriteByte((byte) 1);
      pkg.WriteInt(info.ID);
      pkg.WriteBoolean(isInvite);
      pkg.WriteInt(info.ConsortiaID);
      pkg.WriteString(info.ConsortiaName);
      pkg.WriteInt(info.UserID);
      pkg.WriteString(info.UserName);
      pkg.WriteInt(playerid);
      pkg.WriteString(playerName);
      pkg.WriteInt(info.DutyID);
      pkg.WriteString(info.DutyName);
      pkg.WriteInt(info.Offer);
      pkg.WriteInt(info.RichesOffer);
      pkg.WriteInt(info.RichesRob);
      pkg.WriteDateTime(info.LastDate);
      pkg.WriteInt(info.Grade);
      pkg.WriteInt(info.Level);
      pkg.WriteInt(info.State);
      pkg.WriteBoolean(info.Sex);
      pkg.WriteInt(info.Right);
      pkg.WriteInt(info.Win);
      pkg.WriteInt(info.Total);
      pkg.WriteInt(info.Escape);
      pkg.WriteInt(consortiaRepute);
      pkg.WriteString(info.LoginName);
      pkg.WriteInt(info.FightPower);
      pkg.WriteInt(info.AchievementPoint);
      pkg.WriteString(info.honor);
      pkg.WriteInt(info.UseOffer);
      this.SendTCP(pkg);
    }

    public void SendListenIPPort(IPAddress ip, int port)
    {
      GSPacketIn pkg = new GSPacketIn((short) 240);
      pkg.Write(ip.GetAddressBytes());
      pkg.WriteInt(port);
      this.SendTCP(pkg);
    }

    public void SendMailResponse(int playerid)
    {
      GSPacketIn pkg = new GSPacketIn((short) 117);
      pkg.WriteInt(playerid);
      this.SendTCP(pkg);
    }

    public void SendMarryRoomDisposeToPlayer(int roomId)
    {
      GSPacketIn pkg = new GSPacketIn((short) 241);
      pkg.WriteInt(roomId);
      this.SendTCP(pkg);
    }

    public void SendMarryRoomInfoToPlayer(int playerId, bool state, MarryRoomInfo info)
    {
      GSPacketIn pkg = new GSPacketIn((short) 14);
      pkg.WriteInt(playerId);
      pkg.WriteBoolean(state);
      if (state)
      {
        pkg.WriteInt(info.ID);
        pkg.WriteString(info.Name);
        pkg.WriteInt(info.MapIndex);
        pkg.WriteInt(info.AvailTime);
        pkg.WriteInt(info.PlayerID);
        pkg.WriteInt(info.GroomID);
        pkg.WriteInt(info.BrideID);
        pkg.WriteDateTime(info.BeginTime);
        pkg.WriteBoolean(info.IsGunsaluteUsed);
      }
      this.SendTCP(pkg);
    }

    public void SendPacket(GSPacketIn packet)
    {
      this.SendTCP(packet);
    }

    public void SendPingCenter()
    {
      GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
      int val = allPlayers == null ? 0 : allPlayers.Length;
      GSPacketIn pkg = new GSPacketIn((short) 12);
      pkg.WriteInt(val);
      this.SendTCP(pkg);
    }

    public void SendRSALogin(RSACryptoServiceProvider rsa, string key)
    {
      GSPacketIn pkg = new GSPacketIn((short) 1);
      pkg.Write(rsa.Encrypt(Encoding.UTF8.GetBytes(key), false));
      this.SendTCP(pkg);
    }

    public void SendShutdown(bool isStoping)
    {
      GSPacketIn pkg = new GSPacketIn((short) 15);
      pkg.WriteInt(this.m_serverId);
      pkg.WriteBoolean(isStoping);
      this.SendTCP(pkg);
    }

    public void SendToAllConsortiaMember(ConsortiaInfo consortia, int type)
    {
      if (!ConsortiaBossMgr.AddConsortia(consortia.ConsortiaID, consortia))
        ConsortiaBossMgr.UpdateConsortia(consortia);
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ConsortiaID == consortia.ConsortiaID)
        {
          allPlayer.SendConsortiaBossInfo(consortia);
          switch (type)
          {
            case 0:
              allPlayer.SendConsortiaBossOpenClose(0);
              continue;
            case 1:
              allPlayer.SendConsortiaBossOpenClose(1);
              continue;
            case 2:
              allPlayer.SendConsortiaBossOpenClose(2);
              continue;
            case 3:
              allPlayer.SendConsortiaBossOpenClose(3);
              continue;
            default:
              continue;
          }
        }
      }
    }

    public void SendUpdatePlayerMarriedStates(int playerId)
    {
      GSPacketIn pkg = new GSPacketIn((short) 13);
      pkg.WriteInt(playerId);
      this.SendTCP(pkg);
    }

    public GSPacketIn SendUserOffline(int playerid, int consortiaID)
    {
      GSPacketIn pkg = new GSPacketIn((short) 4);
      pkg.WriteInt(1);
      pkg.WriteInt(playerid);
      pkg.WriteInt(consortiaID);
      this.SendTCP(pkg);
      return pkg;
    }

    public GSPacketIn SendUserOnline(Dictionary<int, int> users)
    {
      GSPacketIn pkg = new GSPacketIn((short) 5);
      pkg.WriteInt(users.Count);
      foreach (KeyValuePair<int, int> user in users)
      {
        pkg.WriteInt(user.Key);
        pkg.WriteInt(user.Value);
      }
      this.SendTCP(pkg);
      return pkg;
    }

    public GSPacketIn SendUserOnline(int playerid, int consortiaID)
    {
      GSPacketIn pkg = new GSPacketIn((short) 5);
      pkg.WriteInt(1);
      pkg.WriteInt(playerid);
      pkg.WriteInt(consortiaID);
      this.SendTCP(pkg);
      return pkg;
    }

    public void SendEliteChampionBattleStatus(int userId, bool isReady)
    {
      GSPacketIn pkg = new GSPacketIn((short) 910);
      pkg.WriteInt(userId);
      pkg.WriteBoolean(isReady);
      this.SendTCP(pkg);
    }

    public void SendEliteScoreUpdate(int playerId, string NickName, int type, int score)
    {
      GSPacketIn pkg = new GSPacketIn((short) 905);
      pkg.WriteInt(playerId);
      pkg.WriteString(NickName);
      pkg.WriteInt(type);
      pkg.WriteInt(score);
      this.SendTCP(pkg);
    }

    public void SendEliteChampionRoundUpdate(EliteGameRoundInfo round)
    {
      GSPacketIn pkg = new GSPacketIn((short) 908);
      pkg.WriteInt(round.RoundID);
      pkg.WriteInt(round.RoundType);
      pkg.WriteInt(round.PlayerWin.UserID);
      this.SendTCP(pkg);
    }

    public void HandleEliteGameReload(GSPacketIn pkg)
    {
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        allPlayer.PlayerCharacter.EliteScore = 1000;
        allPlayer.PlayerCharacter.EliteRank = 0;
      }
      ExerciseMgr.ResetEliteGame();
    }

    public void HandleEliteGameRequestStart(GSPacketIn pkg)
    {
      int num = pkg.ReadInt();
      for (int index = 0; index < num; ++index)
        WorldMgr.GetPlayerById(pkg.ReadInt())?.Out.SendEliteGameStartRoom();
    }

    public void HandlerEliteGameSynPlayers(GSPacketIn pkg)
    {
      int num = pkg.ReadInt();
      for (int index = 0; index < num; ++index)
        ExerciseMgr.UpdateEliteGameChapionPlayerList(new PlayerEliteGameInfo()
        {
          UserID = pkg.ReadInt(),
          NickName = pkg.ReadString(),
          GameType = pkg.ReadInt(),
          Status = pkg.ReadInt(),
          Winer = pkg.ReadInt(),
          Rank = pkg.ReadInt(),
          CurrentPoint = pkg.ReadInt()
        });
    }

    public void HandlerEliteGameStatusUpdate(GSPacketIn pkg)
    {
      ExerciseMgr.EliteStatus = pkg.ReadInt();
    }

    public void HandlerEliteGameRoundAdd(GSPacketIn pkg)
    {
      ExerciseMgr.AddEliteRound(new EliteGameRoundInfo()
      {
        RoundID = pkg.ReadInt(),
        RoundType = pkg.ReadInt(),
        PlayerOne = new PlayerEliteGameInfo()
        {
          UserID = pkg.ReadInt()
        },
        PlayerTwo = new PlayerEliteGameInfo()
        {
          UserID = pkg.ReadInt()
        }
      });
    }

    public void HandlerEliteGameUpdateRank(GSPacketIn pkg)
    {
      pkg.UnCompress();
      int num = pkg.ReadInt();
      for (int index = 0; index < num; ++index)
      {
        GamePlayer playerById = WorldMgr.GetPlayerById(pkg.ReadInt());
        if (playerById != null)
          playerById.PlayerCharacter.EliteRank = pkg.ReadInt();
        else
          pkg.ReadInt();
      }
    }

    public void HandlerEliteGameKickOut(GSPacketIn pkg)
    {
      RoomMgr.KickUsingRoom(pkg.ReadInt());
    }
  }
}
