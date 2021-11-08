// Decompiled with JetBrains decompiler
// Type: Center.Server.ServerClient
// Assembly: Center.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4391F35A-9CAD-44A9-AFE0-208E0547A0DD
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Center\Center.Server.dll

using Bussiness;
using Bussiness.Protocol;
using Center.Server.Managers;
using Game.Base;
using Game.Base.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Center.Server
{
  public class ServerClient : BaseClient
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private RSACryptoServiceProvider _rsa;
    private CenterServer _svr;
    public bool NeedSyncMacroDrop;

    public ServerClient(CenterServer svr)
      : base(new byte[8192], new byte[8192])
    {
      this._svr = svr;
    }

    public void HandkeItemStrengthen(GSPacketIn pkg)
    {
      this._svr.SendToALL(pkg, this);
    }

    public void HandleBigBugle(GSPacketIn pkg)
    {
      this._svr.SendToALL(pkg, this);
    }

    public void HandleBuyBadge(GSPacketIn pkg)
    {
      pkg.ReadInt();
      this._svr.SendToALL(pkg, (ServerClient) null);
    }

    public void HandleConsortiaCreate(GSPacketIn pkg)
    {
      pkg.ReadInt();
      pkg.ReadInt();
      this._svr.SendToALL(pkg, (ServerClient) null);
    }

    public void HandleConsortiaFight(GSPacketIn pkg)
    {
      this._svr.SendToALL(pkg);
    }

    public void HandleConsortiaOffer(GSPacketIn pkg)
    {
      pkg.ReadInt();
      pkg.ReadInt();
      pkg.ReadInt();
    }

    public void HandleConsortiaResponse(GSPacketIn pkg)
    {
      int num = (int) pkg.ReadByte();
      this._svr.SendToALL(pkg, (ServerClient) null);
    }

    public void HandleConsortiaUpGrade(GSPacketIn pkg)
    {
      this._svr.SendToALL(pkg, this);
    }

    public void HandleChatConsortia(GSPacketIn pkg)
    {
      this._svr.SendToALL(pkg, this);
    }

    public void HandleChatPersonal(GSPacketIn pkg)
    {
      this._svr.SendToALL(pkg);
    }

    public void HandleChatScene(GSPacketIn pkg)
    {
      if (pkg.ReadByte() != (byte) 3)
        return;
      this.HandleChatConsortia(pkg);
    }

    public void HandleFirendResponse(GSPacketIn pkg)
    {
      this._svr.SendToALL(pkg, this);
    }

    public void HandleFriend(GSPacketIn pkg)
    {
      this._svr.SendToALL(pkg, this);
    }

    public void HandleFriendState(GSPacketIn pkg)
    {
      this._svr.SendToALL(pkg, this);
    }

    public void HandleIPAndPort(GSPacketIn pkg)
    {
    }

    public void HandleLogin(GSPacketIn pkg)
    {
      string[] strArray = Encoding.UTF8.GetString(this._rsa.Decrypt(pkg.ReadBytes(), false)).Split(',');
      if (strArray.Length != 2)
      {
        ServerClient.log.ErrorFormat("Error Login Packet from {0}", (object) this.TcpEndpoint);
        this.Disconnect();
      }
      else
      {
        this._rsa = (RSACryptoServiceProvider) null;
        int id = int.Parse(strArray[0]);
        this.Info = ServerMgr.GetServerInfo(id);
        if (this.Info == null || this.Info.State != 1)
        {
          ServerClient.log.ErrorFormat("Error Login Packet from {0} want to login serverid:{1}", (object) this.TcpEndpoint, (object) id);
          this.Disconnect();
        }
        else
        {
          this.Strict = false;
          CenterServer.Instance.SendConfigState();
          CenterServer.Instance.SendUpdateWorldEvent();
          this.Info.Online = 0;
          this.Info.State = 2;
        }
      }
    }

    public void HandleMacroDrop(GSPacketIn pkg)
    {
      Dictionary<int, int> temp = new Dictionary<int, int>();
      int num1 = pkg.ReadInt();
      for (int index = 0; index < num1; ++index)
      {
        int key = pkg.ReadInt();
        int num2 = pkg.ReadInt();
        temp.Add(key, num2);
      }
      MacroDropMgr.DropNotice(temp);
      this.NeedSyncMacroDrop = true;
    }

    public void HandleMailResponse(GSPacketIn pkg)
    {
      int playerid = pkg.ReadInt();
      this.HandleUserPrivateMsg(pkg, playerid);
    }

    public void HandleMarryRoomInfoToPlayer(GSPacketIn pkg)
    {
      Player player = LoginMgr.GetPlayer(pkg.ReadInt());
      if (player == null || player.CurrentServer == null)
        return;
      player.CurrentServer.SendTCP(pkg);
    }

    public void HandlePing(GSPacketIn pkg)
    {
      this.Info.Online = pkg.ReadInt();
      this.Info.State = ServerMgr.GetState(this.Info.Online, this.Info.Total);
    }

    public void HandleQuestUserState(GSPacketIn pkg)
    {
      int num = pkg.ReadInt();
      if (LoginMgr.GetServerClient(num) == null)
        this.SendUserState(num, false);
      else
        this.SendUserState(num, true);
    }

    public void HandleRecvConsortiaBossAdd(GSPacketIn pkg)
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
        LastOpenBoss = pkg.ReadDateTime()
      };
      if (!ConsortiaBossMgr.AddConsortia(consortia.ConsortiaID, consortia))
        consortia = ConsortiaBossMgr.GetConsortiaById(consortia.ConsortiaID);
      this.HandleSendConsortiaBossInfo(consortia, (byte) 180);
    }

    public void HandleRecvConsortiaBossCreate(GSPacketIn pkg)
    {
      int consortiaId = pkg.ReadInt();
      byte num = pkg.ReadByte();
      DateTime endTime = pkg.ReadDateTime();
      DateTime LastOpenBoss = pkg.ReadDateTime();
      long MaxBlood = (long) pkg.ReadInt();
      if (!ConsortiaBossMgr.UpdateConsortia(consortiaId, (int) num, endTime, LastOpenBoss, MaxBlood))
        return;
      ConsortiaInfo consortiaById = ConsortiaBossMgr.GetConsortiaById(consortiaId);
      if (consortiaById != null)
        this.HandleSendConsortiaBossInfo(consortiaById, (byte) 183);
    }

    public void HandleRecvConsortiaBossExtendAvailable(GSPacketIn pkg)
    {
      int consortiaId = pkg.ReadInt();
      int riches = pkg.ReadInt();
      if (ConsortiaBossMgr.ExtendAvailable(consortiaId, riches))
      {
        ConsortiaInfo consortiaById = ConsortiaBossMgr.GetConsortiaById(consortiaId);
        if (consortiaById == null)
          return;
        this.HandleSendConsortiaBossInfo(consortiaById, (byte) 182);
      }
      else
      {
        ConsortiaInfo consortiaById = ConsortiaBossMgr.GetConsortiaById(consortiaId);
        if (consortiaById != null)
          this.HandleSendConsortiaBossInfo(consortiaById, (byte) 184);
      }
    }

    public void HandleRecvConsortiaBossReload(GSPacketIn pkg)
    {
      ConsortiaInfo consortiaById = ConsortiaBossMgr.GetConsortiaById(pkg.ReadInt());
      if (consortiaById == null)
        return;
      if (consortiaById.bossState == 2 && consortiaById.SendToClient)
      {
        if (consortiaById.IsBossDie)
          this.HandleSendConsortiaBossInfo(consortiaById, (byte) 188);
        else
          this.HandleSendConsortiaBossInfo(consortiaById, (byte) 187);
        ConsortiaBossMgr.UpdateSendToClient(consortiaById.ConsortiaID);
      }
      else
        this.HandleSendConsortiaBossInfo(consortiaById, (byte) 184);
    }

    public void HandleRecvConsortiaBossUpdateBlood(GSPacketIn pkg)
    {
      ConsortiaBossMgr.UpdateBlood(pkg.ReadInt(), pkg.ReadInt());
    }

    public void HandleRecvConsortiaBossUpdateRank(GSPacketIn pkg)
    {
      ConsortiaBossMgr.UpdateRank(pkg.ReadInt(), pkg.ReadInt(), pkg.ReadInt(), pkg.ReadInt(), pkg.ReadString(), pkg.ReadInt());
    }

    public void HandleReload(GSPacketIn pkg)
    {
      eReloadType eReloadType = (eReloadType) pkg.ReadInt();
      int num = pkg.ReadInt();
      bool flag = pkg.ReadBoolean();
      Console.WriteLine(num.ToString() + " " + eReloadType.ToString() + " is reload " + (flag ? (object) "succeed!" : (object) "fail"));
    }

    public void HandleSendConsortiaBossInfo(ConsortiaInfo consortia, byte code)
    {
      GSPacketIn pkg = new GSPacketIn((short) 180);
      pkg.WriteInt(consortia.ConsortiaID);
      pkg.WriteInt(consortia.ChairmanID);
      pkg.WriteByte((byte) consortia.bossState);
      pkg.WriteDateTime(consortia.endTime);
      pkg.WriteInt(consortia.extendAvailableNum);
      pkg.WriteInt(consortia.callBossLevel);
      pkg.WriteInt(consortia.Level);
      pkg.WriteInt(consortia.SmithLevel);
      pkg.WriteInt(consortia.StoreLevel);
      pkg.WriteInt(consortia.SkillLevel);
      pkg.WriteInt(consortia.Riches);
      pkg.WriteDateTime(consortia.LastOpenBoss);
      pkg.WriteLong(consortia.MaxBlood);
      pkg.WriteLong(consortia.TotalAllMemberDame);
      pkg.WriteBoolean(consortia.IsBossDie);
      List<RankingPersonInfo> rankingPersonInfoList = ConsortiaBossMgr.SelectRank(consortia.ConsortiaID);
      pkg.WriteInt(rankingPersonInfoList.Count);
      int val = 1;
      foreach (RankingPersonInfo rankingPersonInfo in rankingPersonInfoList)
      {
        pkg.WriteString(rankingPersonInfo.Name);
        pkg.WriteInt(val);
        pkg.WriteInt(rankingPersonInfo.TotalDamage);
        pkg.WriteInt(rankingPersonInfo.Honor);
        pkg.WriteInt(rankingPersonInfo.Damage);
        ++val;
      }
      pkg.WriteByte(code);
      this._svr.SendToALL(pkg);
    }

    public void HandleShutdown(GSPacketIn pkg)
    {
      int num = pkg.ReadInt();
      if (pkg.ReadBoolean())
        Console.WriteLine(num.ToString() + "  begin stoping !");
      else
        Console.WriteLine(num.ToString() + "  is stoped !");
    }

    public void HandleUpdatePlayerState(GSPacketIn pkg)
    {
      Player player = LoginMgr.GetPlayer(pkg.ReadInt());
      if (player == null || player.CurrentServer == null)
        return;
      player.CurrentServer.SendTCP(pkg);
    }

    private void HandleUserLogin(GSPacketIn pkg)
    {
      int num = pkg.ReadInt();
      if (LoginMgr.TryLoginPlayer(num, this))
        this.SendAllowUserLogin(num, true);
      else
        this.SendAllowUserLogin(num, false);
    }

    private void HandleUserOffline(GSPacketIn pkg)
    {
      List<int> intList = new List<int>();
      int num = pkg.ReadInt();
      for (int index = 0; index < num; ++index)
      {
        int id = pkg.ReadInt();
        pkg.ReadInt();
        LoginMgr.PlayerLoginOut(id, this);
      }
      this._svr.SendToALL(pkg);
    }

    private void HandleUserOnline(GSPacketIn pkg)
    {
      int num = pkg.ReadInt();
      for (int index = 0; index < num; ++index)
      {
        int id = pkg.ReadInt();
        pkg.ReadInt();
        LoginMgr.PlayerLogined(id, this);
      }
      this._svr.SendToALL(pkg, this);
    }

    private void HandleUserPrivateMsg(GSPacketIn pkg, int playerid)
    {
      ServerClient serverClient = LoginMgr.GetServerClient(playerid);
      if (serverClient == null)
        return;
      serverClient.SendTCP(pkg);
    }

    public void HandleUserPublicMsg(GSPacketIn pkg)
    {
      this._svr.SendToALL(pkg, this);
    }

    public void HandleWorldBossFightOver(GSPacketIn pkg)
    {
      WorldMgr.WorldBossFightOver();
      this._svr.SendWorldBossFightOver();
    }

    public void HandleWorldBossPrivateInfo(GSPacketIn pkg)
    {
      this._svr.SendPrivateInfo(pkg.ReadString());
    }

    public void HandleWorldBossRank(GSPacketIn pkg, bool update)
    {
      if (update)
        WorldMgr.UpdateRank(pkg.ReadInt(), pkg.ReadInt(), pkg.ReadString());
      this._svr.SendUpdateRank(false);
    }

    public void HandleWorldBossRoomClose(GSPacketIn pkg)
    {
      WorldMgr.WorldBossRoomClose();
      this._svr.SendRoomClose((byte) 0);
    }

    public void HandleWorldBossUpdateBlood(GSPacketIn pkg)
    {
      int num = pkg.ReadInt();
      if (num > 0)
        WorldMgr.ReduceBlood(num);
      this._svr.SendUpdateWorldBlood();
    }

    protected override void OnConnect()
    {
      base.OnConnect();
      this._rsa = new RSACryptoServiceProvider();
      RSAParameters rsaParameters = this._rsa.ExportParameters(false);
      this.SendRSAKey(rsaParameters.Modulus, rsaParameters.Exponent);
    }

    protected override void OnDisconnect()
    {
      base.OnDisconnect();
      this._rsa = (RSACryptoServiceProvider) null;
      List<Player> serverPlayers = LoginMgr.GetServerPlayers(this);
      LoginMgr.RemovePlayer(serverPlayers);
      this.SendUserOffline(serverPlayers);
      if (this.Info == null)
        return;
      this.Info.State = 1;
      this.Info.Online = 0;
      this.Info = (ServerInfo) null;
    }

    public override void OnRecvPacket(GSPacketIn pkg)
    {
      short code = pkg.Code;
      if (code <= (short) 91)
      {
        if (code <= (short) 37)
        {
          switch ((int) code - 1)
          {
            case 0:
              this.HandleLogin(pkg);
              break;
            case 1:
              break;
            case 2:
              this.HandleUserLogin(pkg);
              break;
            case 3:
              this.HandleUserOffline(pkg);
              break;
            case 4:
              this.HandleUserOnline(pkg);
              break;
            case 5:
              this.HandleQuestUserState(pkg);
              break;
            case 6:
              break;
            case 7:
              break;
            case 8:
              break;
            case 9:
              this.HandkeItemStrengthen(pkg);
              break;
            case 10:
              this.HandleReload(pkg);
              break;
            case 11:
              this.HandlePing(pkg);
              break;
            case 12:
              this.HandleUpdatePlayerState(pkg);
              break;
            case 13:
              this.HandleMarryRoomInfoToPlayer(pkg);
              break;
            case 14:
              this.HandleShutdown(pkg);
              break;
            case 15:
              break;
            case 16:
              break;
            case 17:
              break;
            case 18:
              this.HandleChatScene(pkg);
              break;
            default:
              if (code != (short) 37)
                break;
              this.HandleChatPersonal(pkg);
              break;
          }
        }
        else
        {
          switch (code)
          {
            case 72:
              this.HandleBigBugle(pkg);
              break;
            case 81:
              this.HandleWorldBossRank(pkg, true);
              break;
            case 82:
              this.HandleWorldBossFightOver(pkg);
              break;
            case 83:
              this.HandleWorldBossRoomClose(pkg);
              break;
            case 84:
              this.HandleWorldBossUpdateBlood(pkg);
              break;
            case 85:
              this.HandleWorldBossPrivateInfo(pkg);
              break;
            case 86:
              this.HandleWorldBossRank(pkg, false);
              break;
          }
        }
      }
      else if (code <= (short) 130)
      {
        switch (code)
        {
          case 117:
            this.HandleMailResponse(pkg);
            break;
          case 128:
            this.HandleConsortiaResponse(pkg);
            break;
          case 130:
            this.HandleConsortiaCreate(pkg);
            break;
        }
      }
      else
      {
        switch (code)
        {
          case 156:
            this.HandleConsortiaOffer(pkg);
            break;
          case 157:
            break;
          case 158:
            this.HandleConsortiaFight(pkg);
            break;
          case 159:
            break;
          case 160:
            this.HandleFriend(pkg);
            break;
          case 178:
            this.HandleMacroDrop(pkg);
            break;
          case 179:
            break;
          case 180:
            this.HandleRecvConsortiaBossAdd(pkg);
            break;
          case 181:
            this.HandleRecvConsortiaBossUpdateRank(pkg);
            break;
          case 182:
            this.HandleRecvConsortiaBossExtendAvailable(pkg);
            break;
          case 183:
            this.HandleRecvConsortiaBossCreate(pkg);
            break;
          case 184:
            this.HandleRecvConsortiaBossReload(pkg);
            break;
          case 185:
            break;
          case 186:
            this.HandleRecvConsortiaBossUpdateBlood(pkg);
            break;
          default:
            if (code == (short) 240)
              this.HandleIPAndPort(pkg);
            break;
        }
      }
    }

    public void SendAllowUserLogin(int playerid, bool allow)
    {
      GSPacketIn pkg = new GSPacketIn((short) 3);
      pkg.WriteInt(playerid);
      pkg.WriteBoolean(allow);
      this.SendTCP(pkg);
    }

    public void SendASS(bool state)
    {
      GSPacketIn pkg = new GSPacketIn((short) 7);
      pkg.WriteBoolean(state);
      this.SendTCP(pkg);
    }

    public void SendChargeMoney(int player, string chargeID)
    {
      GSPacketIn pkg = new GSPacketIn((short) 9, player);
      pkg.WriteString(chargeID);
            Console.WriteLine("Mandando Recarga para {0} Id:{1}", player, chargeID);
      this.SendTCP(pkg);
    }

    public void SendKitoffUser(int playerid)
    {
      this.SendKitoffUser(playerid, LanguageMgr.GetTranslation("Center.Server.SendKitoffUser"));
    }

    public void SendKitoffUser(int playerid, string msg)
    {
      GSPacketIn pkg = new GSPacketIn((short) 2);
      pkg.WriteInt(playerid);
      pkg.WriteString(msg);
      this.SendTCP(pkg);
    }

    public void SendRSAKey(byte[] m, byte[] e)
    {
      GSPacketIn pkg = new GSPacketIn((short) 0);
      pkg.Write(m);
      pkg.Write(e);
      this.SendTCP(pkg);
    }

    public void SendUserOffline(List<Player> users)
    {
      for (int index1 = 0; index1 < users.Count; index1 += 100)
      {
        int val = index1 + 100 > users.Count ? users.Count - index1 : 100;
        GSPacketIn pkg = new GSPacketIn((short) 4);
        pkg.WriteInt(val);
        for (int index2 = index1; index2 < index1 + val; ++index2)
        {
          pkg.WriteInt(users[index2].Id);
          pkg.WriteInt(0);
        }
        this.SendTCP(pkg);
        this._svr.SendToALL(pkg, this);
      }
    }

    public void SendUserState(int player, bool state)
    {
      GSPacketIn pkg = new GSPacketIn((short) 6, player);
      pkg.WriteBoolean(state);
      this.SendTCP(pkg);
    }

    public ServerInfo Info { get; set; }
  }
}
