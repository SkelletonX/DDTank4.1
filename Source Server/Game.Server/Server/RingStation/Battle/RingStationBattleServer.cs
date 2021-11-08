// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.Battle.RingStationBattleServer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base;
using Game.Base.Packets;
using Game.Logic;
using System.Collections.Generic;

namespace Game.Server.RingStation.Battle
{
  public class RingStationBattleServer
  {
    private string m_ip;
    private int m_port;
    private Dictionary<int, BaseRoomRingStation> m_rooms;
    private RingStationFightConnector m_server;
    private int m_serverId;

    public RingStationBattleServer(int serverId, string ip, int port, string loginKey)
    {
      this.m_serverId = serverId;
      this.m_ip = ip;
      this.m_port = port;
      this.m_server = new RingStationFightConnector(this, ip, port, loginKey);
      this.m_rooms = new Dictionary<int, BaseRoomRingStation>();
      this.m_server.Disconnected += new ClientEventHandle(this.m_server_Disconnected);
      this.m_server.Connected += new ClientEventHandle(this.m_server_Connected);
    }

    public bool AddRoom(BaseRoomRingStation room)
    {
      bool flag = false;
      BaseRoomRingStation baseRoomRingStation = (BaseRoomRingStation) null;
      lock (this.m_rooms)
      {
        if (this.m_rooms.ContainsKey(room.RoomId))
        {
          baseRoomRingStation = this.m_rooms[room.RoomId];
          this.m_rooms.Remove(room.RoomId);
        }
      }
      if (baseRoomRingStation != null && baseRoomRingStation.Game != null)
        baseRoomRingStation.Game.Stop();
      lock (this.m_rooms)
      {
        if (!this.m_rooms.ContainsKey(room.RoomId))
        {
          this.m_rooms.Add(room.RoomId, room);
          flag = true;
        }
      }
      if (flag)
        this.m_server.SendAddRoom(room);
      room.BattleServer = this;
      return flag;
    }

    private BaseRoomRingStation FindRoom(int roomId)
    {
      BaseRoomRingStation baseRoomRingStation = (BaseRoomRingStation) null;
      lock (this.m_rooms)
      {
        if (this.m_rooms.ContainsKey(roomId))
          baseRoomRingStation = this.m_rooms[roomId];
      }
      return baseRoomRingStation;
    }

    private void m_server_Connected(BaseClient client)
    {
    }

    private void m_server_Disconnected(BaseClient client)
    {
    }

    public bool RemoveRoom(BaseRoomRingStation room)
    {
      bool flag = false;
      lock (this.m_rooms)
      {
        flag = this.m_rooms.ContainsKey(room.RoomId);
        if (flag)
          this.m_server.SendRemoveRoom(room);
      }
      return flag;
    }

    public void RemoveRoomImp(int roomId)
    {
    }

    public void SendToRoom(int roomId, GSPacketIn pkg, int exceptId, int exceptGameId)
    {
      BaseRoomRingStation room = this.FindRoom(roomId);
      if (room == null)
        return;
      if ((uint) exceptId > 0U)
      {
        RingStationGamePlayer playerById = RingStationMgr.GetPlayerById(exceptId);
        if (playerById == null)
          return;
        if (playerById.GamePlayerId == exceptGameId)
          room.SendToAll(pkg, playerById);
        else
          room.SendToAll(pkg);
      }
      else
        room.SendToAll(pkg);
    }

    public void SendToUser(int playerid, GSPacketIn pkg)
    {
    }

    public bool Start()
    {
      return this.m_server.Connect();
    }

    public void StartGame(int roomId, ProxyRingStationGame game)
    {
      this.FindRoom(roomId)?.StartGame((AbstractGame) game);
    }

    public void StopGame(int roomId, int gameId)
    {
      if (this.FindRoom(roomId) == null)
        return;
      lock (this.m_rooms)
        this.m_rooms.Remove(roomId);
    }

    public override string ToString()
    {
      return string.Format("ServerID:{0},Ip:{1},Port:{2},IsConnected:{3},RoomCount:", (object) this.m_serverId, (object) this.m_server.RemoteEP.Address, (object) this.m_server.RemoteEP.Port, (object) this.m_server.IsConnected);
    }

    public void UpdatePlayerGameId(int playerid, int gamePlayerId)
    {
      RingStationGamePlayer playerById = RingStationMgr.GetPlayerById(playerid);
      if (playerById == null)
        return;
      playerById.GamePlayerId = gamePlayerId;
    }

    public void UpdateRoomId(int roomId, int fightRoomId)
    {
    }

    public string Ip
    {
      get
      {
        return this.m_ip;
      }
    }

    public bool IsActive
    {
      get
      {
        return this.m_server.IsConnected;
      }
    }

    public int Port
    {
      get
      {
        return this.m_port;
      }
    }

    public RingStationFightConnector Server
    {
      get
      {
        return this.m_server;
      }
    }
  }
}
