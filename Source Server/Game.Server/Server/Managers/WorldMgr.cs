// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.WorldMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.GameUtils;
using Game.Server.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;

namespace Game.Server.Managers
{
  public sealed class WorldMgr
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ReaderWriterLock readerWriterLock_0 = new ReaderWriterLock();
    private static Dictionary<int, GamePlayer> dictionary_0 = new Dictionary<int, GamePlayer>();
    public static Dictionary<int, UsersExtraInfo> CaddyRank = new Dictionary<int, UsersExtraInfo>();
    private static Dictionary<int, AreaConfigInfo> dictionary_1 = new Dictionary<int, AreaConfigInfo>();
    private static Dictionary<int, EdictumInfo> dictionary_2 = new Dictionary<int, EdictumInfo>();
    private static Dictionary<int, ShopFreeCountInfo> dictionary_3 = new Dictionary<int, ShopFreeCountInfo>();
    private static Dictionary<int, UserExitRoomLogInfo> dictionary_4 = new Dictionary<int, UserExitRoomLogInfo>();
    public static DateTime LastTimeUpdateCaddyRank = DateTime.Now;
    private static string[] string_0 = new string[17]
    {
      "gunny",
      "gun",
      "gunn",
      "g u n n y",
      "g unny",
      "g u nny",
      "g u n ny",
      "g un",
      "g u n",
      "com",
      "c om",
      "c o m",
      "net",
      "n et",
      "n e t",
      "ᶰ",
      "¥"
    };
    private static AreaConfigInfo areaConfigInfo_0;
    public static Scene _marryScene;
    public static Scene _hotSpringScene;
    private static RSACryptoServiceProvider rsacryptoServiceProvider_0;

    public static Scene MarryScene
    {
      get
      {
        return WorldMgr._marryScene;
      }
    }

    public static Scene HotSpringScene
    {
      get
      {
        return WorldMgr._hotSpringScene;
      }
    }

    public static RSACryptoServiceProvider RsaCryptor
    {
      get
      {
        return WorldMgr.rsacryptoServiceProvider_0;
      }
    }

    public static bool Init()
    {
      bool flag = false;
      try
      {
        WorldMgr.rsacryptoServiceProvider_0 = new RSACryptoServiceProvider();
        WorldMgr.rsacryptoServiceProvider_0.FromXmlString(GameServer.Instance.Configuration.PrivateKey);
        WorldMgr.dictionary_0.Clear();
        using (ServiceBussiness serviceBussiness = new ServiceBussiness())
        {
          ServerInfo serviceSingle = serviceBussiness.GetServiceSingle(GameServer.Instance.Configuration.ServerID);
          if (serviceSingle != null)
          {
            WorldMgr._marryScene = new Scene(serviceSingle);
            WorldMgr._hotSpringScene = new Scene(serviceSingle);
            flag = true;
          }
        }
        Dictionary<int, EdictumInfo> dictionary = WorldMgr.smethod_0();
        if (dictionary.Values.Count > 0)
          Interlocked.Exchange<Dictionary<int, EdictumInfo>>(ref WorldMgr.dictionary_2, dictionary);
        WorldMgr.UpdateCaddyRank();
        WorldMgr.smethod_2();
      }
      catch (Exception ex)
      {
        WorldMgr.ilog_0.Error((object) "WordMgr Init", ex);
      }
      return flag;
    }

    public static bool ReloadEdictum()
    {
      bool flag = false;
      try
      {
        Dictionary<int, EdictumInfo> dictionary = WorldMgr.smethod_0();
        if (dictionary.Values.Count > 0)
          Interlocked.Exchange<Dictionary<int, EdictumInfo>>(ref WorldMgr.dictionary_2, dictionary);
        flag = true;
      }
      catch (Exception ex)
      {
        WorldMgr.ilog_0.Error((object) "WordMgr ReloadEdictum Init", ex);
      }
      return flag;
    }

    private static Dictionary<int, EdictumInfo> smethod_0()
    {
      Dictionary<int, EdictumInfo> dictionary = new Dictionary<int, EdictumInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (EdictumInfo edictumInfo in produceBussiness.GetAllEdictum())
        {
          if (!dictionary.ContainsKey(edictumInfo.ID))
            dictionary.Add(edictumInfo.ID, edictumInfo);
        }
      }
      return dictionary;
    }

    public static bool AddPlayer(int playerId, GamePlayer player)
    {
      WorldMgr.readerWriterLock_0.AcquireWriterLock(-1);
      try
      {
        if (WorldMgr.dictionary_0.ContainsKey(playerId))
          return false;
        WorldMgr.dictionary_0.Add(playerId, player);
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseWriterLock();
      }
      return true;
    }

    public static bool RemovePlayer(int playerId)
    {
      WorldMgr.readerWriterLock_0.AcquireWriterLock(-1);
      GamePlayer gamePlayer = (GamePlayer) null;
      try
      {
        if (WorldMgr.dictionary_0.ContainsKey(playerId))
        {
          gamePlayer = WorldMgr.dictionary_0[playerId];
          WorldMgr.dictionary_0.Remove(playerId);
        }
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseWriterLock();
      }
      if (gamePlayer == null)
        return false;
      GameServer.Instance.LoginServer.SendUserOffline(playerId, gamePlayer.PlayerCharacter.ConsortiaID);
      return true;
    }

    public static GamePlayer GetPlayerById(int playerId)
    {
      GamePlayer gamePlayer = (GamePlayer) null;
      WorldMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        if (WorldMgr.dictionary_0.ContainsKey(playerId))
          gamePlayer = WorldMgr.dictionary_0[playerId];
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return gamePlayer;
    }

    public static GamePlayer GetClientByPlayerNickName(string nickName)
    {
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.NickName == nickName)
          return allPlayer;
      }
      return (GamePlayer) null;
    }

    public static GamePlayer GetClientByPlayerUserName(string userName)
    {
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.UserName == userName)
          return allPlayer;
      }
      return (GamePlayer) null;
    }

    public static GamePlayer[] GetAllPlayers()
    {
      List<GamePlayer> gamePlayerList = new List<GamePlayer>();
      WorldMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        foreach (GamePlayer gamePlayer in WorldMgr.dictionary_0.Values)
        {
          if (gamePlayer != null && gamePlayer.PlayerCharacter != null)
            gamePlayerList.Add(gamePlayer);
        }
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return gamePlayerList.ToArray();
    }

    public static string smethod_1(GamePlayer p)
    {
      return (p.Client.Socket.RemoteEndPoint as IPEndPoint)?.Address.ToString();
    }
    public static GamePlayer[] GetAllPlayerWithHWID(string hwid)
    {
      List<GamePlayer> gamePlayerList = new List<GamePlayer>();
      WorldMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        foreach (GamePlayer gamePlayer in WorldMgr.dictionary_0.Values)
        {
          if (gamePlayer != null && gamePlayer.PlayerCharacter != null && (gamePlayer.Client != null && gamePlayer.Client.IsConnected) && (gamePlayer.Client.HWID != null && gamePlayer.Client.HWID.Length > 0 && gamePlayer.Client.HWID == hwid))
            gamePlayerList.Add(gamePlayer);
        }
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return gamePlayerList.ToArray();
    }

    public static GamePlayer[] GetAllPlayerWithIP(string ip)
    {
      List<GamePlayer> gamePlayerList = new List<GamePlayer>();
      WorldMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        foreach (GamePlayer gamePlayer in WorldMgr.dictionary_0.Values)
        {
          if (gamePlayer != null && gamePlayer.PlayerCharacter != null && (gamePlayer.Client != null && gamePlayer.Client.Socket != null) && gamePlayer.Client.IsConnected)
          {
            IPEndPoint remoteEndPoint = gamePlayer.Client.Socket.RemoteEndPoint as IPEndPoint;
            if (remoteEndPoint != null && remoteEndPoint.Address.ToString().Contains(ip))
              gamePlayerList.Add(gamePlayer);
          }
        }
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return gamePlayerList.ToArray();
    }

    public static GamePlayer[] GetAllPlayersNoGame()
    {
      List<GamePlayer> gamePlayerList = new List<GamePlayer>();
      WorldMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
        {
          if (allPlayer.CurrentRoom == null)
            gamePlayerList.Add(allPlayer);
        }
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return gamePlayerList.ToArray();
    }

    public static GamePlayer GetSinglePlayerWithConsortia(int ConsortiaID)
    {
      GamePlayer gamePlayer1 = (GamePlayer) null;
      WorldMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        foreach (GamePlayer gamePlayer2 in WorldMgr.dictionary_0.Values)
        {
          if (gamePlayer2 != null && gamePlayer2.PlayerCharacter != null && gamePlayer2.PlayerCharacter.ConsortiaID == ConsortiaID)
            gamePlayer1 = gamePlayer2;
        }
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return gamePlayer1;
    }

    public static GamePlayer[] GetAllPlayersWithConsortia(int ConsortiaID)
    {
      List<GamePlayer> gamePlayerList = new List<GamePlayer>();
      WorldMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        foreach (GamePlayer gamePlayer in WorldMgr.dictionary_0.Values)
        {
          if (gamePlayer != null && gamePlayer.PlayerCharacter != null && gamePlayer.PlayerCharacter.ConsortiaID == ConsortiaID)
            gamePlayerList.Add(gamePlayer);
        }
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return gamePlayerList.ToArray();
    }

    public static string GetPlayerStringByPlayerNickName(string nickName)
    {
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.NickName == nickName)
          return allPlayer.ToString();
      }
      return nickName + " is not online!";
    }

    public static string DisconnectPlayerByName(string nickName)
    {
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.NickName == nickName)
        {
          allPlayer.Disconnect();
          return "OK";
        }
      }
      return nickName + " is not online!";
    }

    public static void OnPlayerOffline(int playerid, int consortiaID)
    {
      WorldMgr.ChangePlayerState(playerid, 0, consortiaID);
    }

    public static void OnPlayerOnline(int playerid, int consortiaID)
    {
      WorldMgr.ChangePlayerState(playerid, 1, consortiaID);
    }

    public static void ChangePlayerState(int playerID, int state, int consortiaID)
    {
      GSPacketIn pkg = (GSPacketIn) null;
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.Friends != null && allPlayer.Friends.ContainsKey(playerID) && allPlayer.Friends[playerID] == 0 || allPlayer.PlayerCharacter.ConsortiaID != 0 && allPlayer.PlayerCharacter.ConsortiaID == consortiaID)
        {
          if (pkg == null)
            pkg = allPlayer.Out.SendFriendState(playerID, state, allPlayer.PlayerCharacter.typeVIP, allPlayer.PlayerCharacter.VIPLevel);
          else
            allPlayer.SendTCP(pkg);
        }
      }
    }

    public static void UpdateExitGame(int userId)
    {
      lock (WorldMgr.dictionary_4)
      {
        if (WorldMgr.dictionary_4.ContainsKey(userId))
        {
          if (WorldMgr.dictionary_4[userId].TotalExitTime <= 3)
            ++WorldMgr.dictionary_4[userId].TotalExitTime;
          else
            WorldMgr.dictionary_4[userId].TimeBlock = DateTime.Now.AddMinutes(30.0);
          WorldMgr.dictionary_4[userId].LastLogout = DateTime.Now;
        }
        else
          WorldMgr.dictionary_4.Add(userId, new UserExitRoomLogInfo()
          {
            UserID = userId,
            TimeBlock = DateTime.MinValue,
            TotalExitTime = 1,
            LastLogout = DateTime.Now
          });
      }
    }

    public static DateTime CheckTimeEnterRoom(int userid)
    {
      lock (WorldMgr.dictionary_4)
      {
        if (WorldMgr.dictionary_4.ContainsKey(userid))
        {
          if (WorldMgr.dictionary_4[userid].TimeBlock > DateTime.Now)
            return WorldMgr.dictionary_4[userid].TimeBlock;
          if (WorldMgr.dictionary_4[userid].TotalExitTime > 3)
            WorldMgr.dictionary_4[userid].TotalExitTime = 0;
        }
        return DateTime.MinValue;
      }
    }

    public static bool UpdateShopFreeCount(int shopId, int total)
    {
      bool flag = false;
      lock (WorldMgr.dictionary_3)
      {
        if (WorldMgr.dictionary_3.ContainsKey(shopId))
        {
          if (WorldMgr.dictionary_3[shopId].Count > 0)
          {
            --WorldMgr.dictionary_3[shopId].Count;
            flag = true;
          }
        }
        else
        {
          WorldMgr.dictionary_3.Add(shopId, new ShopFreeCountInfo()
          {
            ShopID = shopId,
            Count = total - 1,
            CreateDate = DateTime.Now
          });
          flag = true;
        }
      }
      return flag;
    }

    private static void smethod_2()
    {
      WorldMgrDataInfo worldMgrDataInfo = Marshal.LoadDataFile<WorldMgrDataInfo>("shopfreecount", true);
      if (worldMgrDataInfo == null || worldMgrDataInfo.ShopFreeCount == null)
        return;
      WorldMgr.dictionary_3 = worldMgrDataInfo.ShopFreeCount;
    }

    private static void smethod_3()
    {
      Marshal.SaveDataFile<WorldMgrDataInfo>(new WorldMgrDataInfo()
      {
        ShopFreeCount = WorldMgr.dictionary_3
      }, "shopfreecount", true);
    }

    public static void ScanShopFreeVaildDate()
    {
      lock (WorldMgr.dictionary_3)
      {
        bool flag = false;
        foreach (ShopFreeCountInfo shopFreeCountInfo in WorldMgr.dictionary_3.Values)
        {
          DateTime dateTime = shopFreeCountInfo.CreateDate;
          DateTime date1 = dateTime.Date;
          dateTime = DateTime.Now;
          DateTime date2 = dateTime.Date;
          if (date1 != date2)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return;
        WorldMgr.dictionary_3.Clear();
      }
    }

    public static List<ShopFreeCountInfo> GetAllShopFreeCount()
    {
      List<ShopFreeCountInfo> shopFreeCountInfoList = new List<ShopFreeCountInfo>();
      lock (WorldMgr.dictionary_3)
      {
        foreach (ShopFreeCountInfo shopFreeCountInfo in WorldMgr.dictionary_3.Values)
          shopFreeCountInfoList.Add(shopFreeCountInfo);
      }
      return shopFreeCountInfoList;
    }

    public static GSPacketIn SendSysNotice(
      eMessageType type,
      string msg,
      int ItemID,
      int TemplateID,
      string key)
    {
      int val = msg.IndexOf(TemplateID.ToString(), StringComparison.Ordinal);
      GSPacketIn pkg = new GSPacketIn((short) 10);
      pkg.WriteInt((int) type);
      pkg.WriteString(msg.Replace(TemplateID.ToString(), ""));
      pkg.WriteByte((byte) 1);
      pkg.WriteInt(val);
      pkg.WriteInt(TemplateID);
      pkg.WriteInt(ItemID);
      if (!string.IsNullOrEmpty(key))
        pkg.WriteString(key);
      WorldMgr.SendToAll(pkg);
      return pkg;
    }

    public static GSPacketIn SendSysTipNotice(string msg)
    {
      GSPacketIn pkg = new GSPacketIn((short) 10);
      pkg.WriteInt(2);
      pkg.WriteString(msg);
      WorldMgr.SendToAll(pkg);
      return pkg;
    }

    public static GSPacketIn SendSysNotice(string msg)
    {
      GSPacketIn pkg = new GSPacketIn((short) 10);
      pkg.WriteInt(3);
      pkg.WriteString(msg);
      WorldMgr.SendToAll(pkg);
      return pkg;
    }

    public static void SendSysNotice(string msg, int consortiaId)
    {
      foreach (GamePlayer playersWithConsortium in WorldMgr.GetAllPlayersWithConsortia(consortiaId))
      {
        GSPacketIn gsPacketIn = new GSPacketIn((short) 10);
        gsPacketIn.WriteInt(3);
        gsPacketIn.WriteString(msg);
        GSPacketIn pkg = gsPacketIn;
        playersWithConsortium.SendTCP(pkg);
      }
    }

    public static GSPacketIn SendSysNotice(
      eMessageType type,
      string msg,
      List<SqlDataProvider.Data.ItemInfo> items,
      int zoneID)
    {
      List<int> intList = WorldMgr.smethod_4(msg, "@");
      GSPacketIn pkg = (GSPacketIn) null;
      if (intList.Count == items.Count)
      {
        pkg = new GSPacketIn((short) 10);
        pkg.WriteInt((int) type);
        pkg.WriteString(msg.Replace("@", ""));
        if (type == eMessageType.CROSS_NOTICE)
          pkg.WriteInt(zoneID);
        int index = 0;
        pkg.WriteByte((byte) intList.Count);
        foreach (int val in intList)
        {
          SqlDataProvider.Data.ItemInfo itemInfo = items[index];
          pkg.WriteInt(val);
          pkg.WriteInt(itemInfo.TemplateID);
          pkg.WriteInt(itemInfo.ItemID);
          pkg.WriteString("");
          ++index;
        }
        WorldMgr.SendToAll(pkg);
      }
      else
        WorldMgr.ilog_0.Error((object) ("wrong msg: " + msg + ": itemcount: " + (object) items.Count));
      return pkg;
    }

    private static List<int> smethod_4(string string_1, string string_2)
    {
      List<int> intList = new List<int>();
      int length = string_2.Length;
      int num = -length;
      while (true)
      {
        num = string_1.IndexOf(string_2, num + length);
        if (num != -1)
          intList.Add(num);
        else
          break;
      }
      return intList;
    }

    public static void UpdateCaddyRank()
    {
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        UsersExtraInfo[] rankCaddy = playerBussiness.GetRankCaddy();
        WorldMgr.CaddyRank = new Dictionary<int, UsersExtraInfo>();
        foreach (UsersExtraInfo usersExtraInfo in rankCaddy)
        {
          if (!WorldMgr.CaddyRank.ContainsKey(usersExtraInfo.UserID))
            WorldMgr.CaddyRank.Add(usersExtraInfo.UserID, usersExtraInfo);
        }
        WorldMgr.LastTimeUpdateCaddyRank = DateTime.Now;
      }
    }

    public static void AddAreaConfig(AreaConfigInfo[] Areas)
    {
      foreach (AreaConfigInfo area in Areas)
      {
        if (!WorldMgr.dictionary_1.ContainsKey(area.AreaID))
        {
          if (area.AreaID == GameServer.Instance.Configuration.ZoneId)
            WorldMgr.areaConfigInfo_0 = area;
          WorldMgr.dictionary_1.Add(area.AreaID, area);
        }
      }
    }

    public static AreaConfigInfo FindAreaConfig(int zoneId)
    {
      WorldMgr.readerWriterLock_0.AcquireWriterLock(-1);
      try
      {
        if (WorldMgr.dictionary_1.ContainsKey(zoneId))
          return WorldMgr.dictionary_1[zoneId];
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseWriterLock();
      }
      return (AreaConfigInfo) null;
    }

    public static AreaConfigInfo[] GetAllAreaConfig()
    {
      List<AreaConfigInfo> areaConfigInfoList = new List<AreaConfigInfo>();
      foreach (AreaConfigInfo areaConfigInfo in WorldMgr.dictionary_1.Values)
        areaConfigInfoList.Add(areaConfigInfo);
      return areaConfigInfoList.ToArray();
    }

    public static EdictumInfo[] GetAllEdictumVersion()
    {
      List<EdictumInfo> edictumInfoList = new List<EdictumInfo>();
      foreach (EdictumInfo edictumInfo in WorldMgr.dictionary_2.Values)
      {
        DateTime dateTime = edictumInfo.EndDate;
        DateTime date1 = dateTime.Date;
        dateTime = DateTime.Now;
        DateTime date2 = dateTime.Date;
        if (date1 > date2)
          edictumInfoList.Add(edictumInfo);
      }
      return edictumInfoList.ToArray();
    }

    public static void SendToAll(GSPacketIn pkg)
    {
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
        allPlayer.SendTCP(pkg);
    }

    public static bool CheckBadWord(string msg)
    {
      foreach (string str in WorldMgr.string_0)
      {
        if (msg.ToLower().Contains(str.ToLower()))
          return true;
      }
      return false;
    }

    public static void Stop()
    {
      WorldMgr.smethod_3();
    }
  }
}
