// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.BaseRoomRingStation
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Logic;
using Game.Server.RingStation.Battle;
using System.Collections.Generic;
using System.Timers;

namespace Game.Server.RingStation
{
  public class BaseRoomRingStation
  {
    private Timer addrom = new Timer();
    public RingStationBattleServer BattleServer;
    public bool IsAutoBot;
    public bool IsFreedom;
    private AbstractGame m_game;
    private List<RingStationGamePlayer> m_places;
    public int PickUpNpcId;
    public int RoomId;

    public BaseRoomRingStation(int roomId)
    {
      this.RoomId = roomId;
      this.m_places = new List<RingStationGamePlayer>();
    }

    public bool AddPlayer(RingStationGamePlayer player)
    {
      lock (this.m_places)
      {
        player.CurRoom = this;
        this.m_places.Add(player);
      }
      return true;
    }

    internal List<RingStationGamePlayer> GetPlayers()
    {
      return this.m_places;
    }

    public void RemovePlayer(RingStationGamePlayer player)
    {
      if (this.BattleServer != null)
      {
        if (this.m_game != null)
        {
          this.BattleServer.Server.SendPlayerDisconnet(this.Game.Id, player.GamePlayerId, this.RoomId);
          this.BattleServer.Server.SendRemoveRoom(this);
        }
        this.IsPlaying = false;
      }
      if (this.Game == null)
        return;
      this.Game.Stop();
    }

    internal void SendTCP(GSPacketIn pkg)
    {
      if (this.m_game == null)
        return;
      this.BattleServer.Server.SendToGame(this.m_game.Id, pkg);
    }

    public void SendToAll(GSPacketIn pkg)
    {
      this.SendToAll(pkg, (RingStationGamePlayer) null);
    }

    public void SendToAll(GSPacketIn pkg, RingStationGamePlayer except)
    {
      lock (this.m_places)
      {
        foreach (RingStationGamePlayer place in this.m_places)
        {
          if (place != null && place != except)
            place.ProcessPacket(pkg);
        }
      }
    }

    public void StartGame(AbstractGame game)
    {
      this.m_game = game;
    }

    public AbstractGame Game
    {
      get
      {
        return this.m_game;
      }
    }

    public int GameType { get; set; }

    public int GuildId { get; set; }

    public bool IsPlaying { get; set; }

    public int RoomType { get; set; }
  }
}
