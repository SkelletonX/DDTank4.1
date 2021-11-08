// Decompiled with JetBrains decompiler
// Type: Fighting.Server.Games.GameMgr
// Assembly: Fighting.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8F1EB855-F1B7-44B3-B212-6508DAF33CC5
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Fight\Fighting.Server.dll

using Bussiness;
using Fighting.Server.GameObjects;
using Fighting.Server.Rooms;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Maps;
using Game.Logic.Phy.Object;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Fighting.Server.Games
{
  public class GameMgr
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    public static readonly long THREAD_INTERVAL = 40;
    private static readonly int int_3 = 60000;
    private static Dictionary<int, BaseGame> dictionary_0;
    private static Thread thread_0;
    private static bool bool_0;
    private static int int_0;
    private static int int_1;
    private static int int_2;
    private static long long_0;

    public static bool Setup(int serverId, int boxBroadcastLevel)
    {
      GameMgr.thread_0 = new Thread(new ThreadStart(GameMgr.smethod_0));
      GameMgr.dictionary_0 = new Dictionary<int, BaseGame>();
      GameMgr.int_0 = serverId;
      GameMgr.int_1 = boxBroadcastLevel;
      GameMgr.int_2 = 0;
      return true;
    }

    public static int BoxBroadcastLevel
    {
      get
      {
        return GameMgr.int_1;
      }
    }

    public static void Start()
    {
      if (GameMgr.bool_0)
        return;
      GameMgr.bool_0 = true;
      GameMgr.thread_0.Start();
    }

    public static void Stop()
    {
      if (!GameMgr.bool_0)
        return;
      GameMgr.bool_0 = false;
      GameMgr.thread_0.Join();
    }

    private static void smethod_0()
    {
      long num = 0;
      GameMgr.long_0 = TickHelper.GetTickCount();
      while (GameMgr.bool_0)
      {
        long tickCount1 = TickHelper.GetTickCount();
        try
        {
          GameMgr.smethod_1(tickCount1);
          GameMgr.smethod_2(tickCount1);
        }
        catch (Exception ex)
        {
          GameMgr.ilog_0.Error((object) "Room Mgr Thread Error:", ex);
        }
        long tickCount2 = TickHelper.GetTickCount();
        num += GameMgr.THREAD_INTERVAL - (tickCount2 - tickCount1);
        if (num > 0L)
        {
          Thread.Sleep((int) num);
          num = 0L;
        }
        else if (num < -1000L)
        {
          GameMgr.ilog_0.WarnFormat("Room Mgr is delay {0} ms!", (object) num);
          num += 1000L;
        }
      }
    }

    private static void smethod_1(long long_1)
    {
      IList games = (IList) GameMgr.GetGames();
      if (games == null)
        return;
      foreach (BaseGame baseGame in (IEnumerable) games)
      {
        try
        {
          baseGame.Update(long_1);
        }
        catch (Exception ex)
        {
          GameMgr.ilog_0.Error((object) "Game  updated error:", ex);
        }
      }
    }

    private static void smethod_2(long long_1)
    {
      if (GameMgr.long_0 > long_1)
        return;
      GameMgr.long_0 += (long) GameMgr.int_3;
      ArrayList arrayList = new ArrayList();
      lock (GameMgr.dictionary_0)
      {
        foreach (BaseGame baseGame in GameMgr.dictionary_0.Values)
        {
          if (baseGame.GameState == eGameState.Stopped)
            arrayList.Add((object) baseGame);
        }
        foreach (BaseGame baseGame in arrayList)
        {
          GameMgr.dictionary_0.Remove(baseGame.Id);
          try
          {
            baseGame.Dispose();
          }
          catch (Exception ex)
          {
            GameMgr.ilog_0.Error((object) "game dispose error:", ex);
          }
        }
      }
    }

    public static List<BaseGame> GetGames()
    {
      List<BaseGame> baseGameList = new List<BaseGame>();
      lock (GameMgr.dictionary_0)
        baseGameList.AddRange((IEnumerable<BaseGame>) GameMgr.dictionary_0.Values);
      return baseGameList;
    }

    public static BaseGame FindGame(int id)
    {
      lock (GameMgr.dictionary_0)
      {
        if (GameMgr.dictionary_0.ContainsKey(id))
          return GameMgr.dictionary_0[id];
      }
      return (BaseGame) null;
    }

    public static BaseGame StartPVPGame(
      List<IGamePlayer> red,
      List<IGamePlayer> blue,
      int mapIndex,
      eRoomType roomType,
      eGameType gameType,
      int timeType)
    {
      try
      {
        Map map = MapMgr.CloneMap(MapMgr.GetMapIndex(mapIndex, (byte) roomType, GameMgr.int_0));
        if (map == null)
          return (BaseGame) null;
        PVPGame pvpGame = new PVPGame(GameMgr.int_2++, 0, red, blue, map, roomType, gameType, timeType);
        lock (GameMgr.dictionary_0)
          GameMgr.dictionary_0.Add(pvpGame.Id, (BaseGame) pvpGame);
        pvpGame.Prepare();
        return (BaseGame) pvpGame;
      }
      catch (Exception ex)
      {
        GameMgr.ilog_0.Error((object) "Create game error:", ex);
        return (BaseGame) null;
      }
    }

    public static BattleGame StartBattleGame(
      List<IGamePlayer> red,
      ProxyRoom roomRed,
      List<IGamePlayer> blue,
      ProxyRoom roomBlue,
      int mapIndex,
      eRoomType roomType,
      eGameType gameType,
      int timeType)
    {
      try
      {
        Map map = MapMgr.CloneMap(MapMgr.GetMapIndex(mapIndex, (byte) roomType, GameMgr.int_0));
        if (map == null)
          return (BattleGame) null;
        BattleGame game = new BattleGame(GameMgr.int_2++, red, roomRed, blue, roomBlue, map, roomType, gameType, timeType);
        lock (GameMgr.dictionary_0)
          GameMgr.dictionary_0.Add(game.Id, (BaseGame) game);
        game.Prepare();
        GameMgr.SendStartMessage(game);
        return game;
      }
      catch (Exception ex)
      {
        GameMgr.ilog_0.Error((object) "Create battle game error:", ex);
        return (BattleGame) null;
      }
    }

    public static void SendStartMessage(BattleGame game)
    {
      GSPacketIn pkg1 = new GSPacketIn((short) 3);
      pkg1.WriteInt(2);
      if (game.GameType != eGameType.EliteGameScore && game.GameType != eGameType.EliteGameChampion)
      {
        if (game.GameType == eGameType.Free)
        {
          foreach (Player allFightPlayer in game.GetAllFightPlayers())
          {
            (allFightPlayer.PlayerDetail as ProxyPlayer).m_antiAddictionRate = 1.0;
            GSPacketIn pkg2 = GameMgr.SendBufferList(allFightPlayer, (allFightPlayer.PlayerDetail as ProxyPlayer).Buffers);
            game.SendToAll(pkg2);
          }
          GSPacketIn pkg3 = GameMgr.smethod_3(DateTime.Now);
          game.SendToAll(pkg3);
          pkg1.WriteString(LanguageMgr.GetTranslation("FightServer.MergeRoom.Match.Success"));
        }
        else
          pkg1.WriteString(LanguageMgr.GetTranslation("FightServer.MergeRoom.Guild.Success"));
      }
      else
        pkg1.WriteString(LanguageMgr.GetTranslation("FightServer.MergeRoom.EliteGame.Success"));
      game.SendToAll(pkg1, (IGamePlayer) null);
    }

    public static GSPacketIn SendBufferList(Player player, List<BufferInfo> infos)
    {
      GSPacketIn gsPacketIn = new GSPacketIn((short) 186, player.Id);
      gsPacketIn.WriteInt(infos.Count);
      foreach (BufferInfo info in infos)
      {
        gsPacketIn.WriteInt(info.Type);
        gsPacketIn.WriteBoolean(info.IsExist);
        gsPacketIn.WriteDateTime(info.BeginDate);
        gsPacketIn.WriteInt(info.ValidDate);
        gsPacketIn.WriteInt(info.Value);
      }
      return gsPacketIn;
    }

    private static GSPacketIn smethod_3(DateTime dateTime_0)
    {
      GSPacketIn gsPacketIn = new GSPacketIn((short) 999);
      gsPacketIn.WriteDateTime(dateTime_0);
      return gsPacketIn;
    }
  }
}
