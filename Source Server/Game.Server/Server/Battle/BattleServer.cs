// Decompiled with JetBrains decompiler
// Type: Game.Server.Battle.BattleServer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Rooms;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Server.Battle
{
  public class BattleServer
  {
    private int int_0;
    private int int_1;
    private DateTime dateTime_0;
    private readonly ILog ilog_0;
    private FightServerConnector fightServerConnector_0;
    private Dictionary<int, BaseRoom> dictionary_0;
    private string string_0;
    private int int_2;
    private string string_1;

    public int RetryCount
    {
      get
      {
        return this.int_1;
      }
      set
      {
        this.int_1 = value;
      }
    }

    public DateTime LastRetryTime
    {
      get
      {
        return this.dateTime_0;
      }
      set
      {
        this.dateTime_0 = value;
      }
    }

    public FightServerConnector Server
    {
      get
      {
        return this.fightServerConnector_0;
      }
    }

    public BattleServer(int serverId, string ip, int port, string loginKey)
    {
      this.ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      this.int_0 = serverId;
      this.string_0 = ip;
      this.int_2 = port;
      this.string_1 = loginKey;
      this.int_1 = 0;
      this.dateTime_0 = DateTime.Now;
      this.fightServerConnector_0 = new FightServerConnector(this, ip, port, loginKey);
      this.dictionary_0 = new Dictionary<int, BaseRoom>();
      this.fightServerConnector_0.Disconnected += new ClientEventHandle(this.method_2);
      this.fightServerConnector_0.Connected += new ClientEventHandle(this.method_1);
    }

    public BattleServer Clone()
    {
      return new BattleServer(this.int_0, this.string_0, this.int_2, this.string_1);
    }

    public string LoginKey
    {
      get
      {
        return this.string_1;
      }
    }

    public int ServerId
    {
      get
      {
        return this.int_0;
      }
    }

    public bool IsActive
    {
      get
      {
        return this.fightServerConnector_0.IsConnected;
      }
    }

    public string Ip
    {
      get
      {
        return this.string_0;
      }
    }

    public int Port
    {
      get
      {
        return this.int_2;
      }
    }

    public void Start()
    {
      if (this.fightServerConnector_0.Connect())
        return;
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.method_0));
    }

    private void method_0(object object_0)
    {
      this.method_2((BaseClient) this.fightServerConnector_0);
    }

    public event EventHandler Disconnected;

    private void method_1(BaseClient baseClient_0)
    {
    }

    private void method_2(BaseClient baseClient_0)
    {
      this.RemoveAllRoom();
      if (this.Disconnected == null)
        return;
      this.Disconnected((object) this, (EventArgs) null);
    }

    public void RemoveAllRoom()
    {
      BaseRoom[] baseRoomArray = (BaseRoom[]) null;
      lock (this.dictionary_0)
      {
        baseRoomArray = this.dictionary_0.Values.ToArray<BaseRoom>();
        this.dictionary_0.Clear();
      }
      foreach (BaseRoom room in baseRoomArray)
      {
        if (room != null)
        {
          room.RemoveAllPlayer();
          RoomMgr.StopProxyGame(room);
        }
      }
    }

    public BaseRoom FindRoom(int roomId)
    {
      BaseRoom baseRoom = (BaseRoom) null;
      lock (this.dictionary_0)
      {
        if (this.dictionary_0.ContainsKey(roomId))
          baseRoom = this.dictionary_0[roomId];
      }
      return baseRoom;
    }

    public bool AddRoom(BaseRoom room)
    {
      bool flag = false;
      lock (this.dictionary_0)
      {
        if (!this.dictionary_0.ContainsKey(room.RoomId))
        {
          this.dictionary_0.Add(room.RoomId, room);
          flag = true;
        }
      }
      if (flag)
        this.fightServerConnector_0.SendAddRoom(room);
      return flag;
    }

    public bool RemoveRoom(BaseRoom room)
    {
      bool flag = false;
      lock (this.dictionary_0)
        flag = this.dictionary_0.ContainsKey(room.RoomId);
      if (flag)
        this.fightServerConnector_0.SendRemoveRoom(room);
      return flag;
    }

    public void RemoveRoomImp(int roomId)
    {
      BaseRoom room = (BaseRoom) null;
      lock (this.dictionary_0)
      {
        if (this.dictionary_0.ContainsKey(roomId))
        {
          room = this.dictionary_0[roomId];
          this.dictionary_0.Remove(roomId);
        }
      }
      if (room == null)
        return;
      if (room.IsPlaying && room.Game == null)
        RoomMgr.CancelPickup(this, room);
      else
        RoomMgr.StopProxyGame(room);
    }

    public void StartGame(int roomId, ProxyGame game)
    {
      BaseRoom room = this.FindRoom(roomId);
      if (room == null)
        return;
      RoomMgr.StartProxyGame(room, game);
    }

    public void StopGame(int roomId, int gameId)
    {
      BaseRoom room = this.FindRoom(roomId);
      if (room == null)
        return;
      RoomMgr.StopProxyGame(room);
      lock (this.dictionary_0)
        this.dictionary_0.Remove(roomId);
    }

    public void SendToRoom(int roomId, GSPacketIn pkg, int exceptId, int exceptGameId)
    {
      BaseRoom room = this.FindRoom(roomId);
      if (room == null)
        return;
      if ((uint) exceptId > 0U)
      {
        GamePlayer playerById = WorldMgr.GetPlayerById(exceptId);
        if (playerById == null)
          return;
        if (playerById.GameId == exceptGameId)
          room.SendToAll(pkg, playerById);
        else
          room.SendToAll(pkg);
      }
      else
        room.SendToAll(pkg);
    }

    public void SendToUser(int playerid, GSPacketIn pkg)
    {
      WorldMgr.GetPlayerById(playerid)?.SendTCP(pkg);
    }

    public void UpdatePlayerGameId(int playerid, int gamePlayerId)
    {
      GamePlayer playerById = WorldMgr.GetPlayerById(playerid);
      if (playerById != null)
        playerById.GameId = gamePlayerId;
      else
        this.ilog_0.Error((object) ("//UPDATE GAMEID ERROR: " + (object) playerid + "|" + (object) gamePlayerId));
    }

    public override string ToString()
    {
      return string.Format("ServerID:{0},Ip:{1},Port:{2},IsConnected:{3},RoomCount:{4}", (object) this.int_0, (object) this.fightServerConnector_0.RemoteEP.Address, (object) this.fightServerConnector_0.RemoteEP.Port, (object) this.fightServerConnector_0.IsConnected, (object) this.dictionary_0.Count);
    }
  }
}
