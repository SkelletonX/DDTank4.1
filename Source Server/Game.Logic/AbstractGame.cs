// Decompiled with JetBrains decompiler
// Type: Game.Logic.AbstractGame
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Packets;
using Game.Logic.Phy.Object;
using System.Threading;

namespace Game.Logic
{
  public class AbstractGame
  {
    private int int_0;
    protected eRoomType m_roomType;
    protected eGameType m_gameType;
    protected eMapType m_mapType;
    protected int m_timeType;
    private int int_1;

    public int Id
    {
      get
      {
        return this.int_0;
      }
    }

    public eRoomType RoomType
    {
      get
      {
        return this.m_roomType;
      }
    }

    public bool IsPVP()
    {
      if (this.m_roomType != eRoomType.Match && this.m_roomType != eRoomType.Freedom && this.m_roomType != eRoomType.EliteGameScore)
        return this.m_roomType == eRoomType.EliteGameChampion;
      return true;
    }

    public eGameType GameType
    {
      get
      {
        return this.m_gameType;
      }
    }

    public int TimeType
    {
      get
      {
        return this.m_timeType;
      }
    }

    public AbstractGame(int id, eRoomType roomType, eGameType gameType, int timeType)
    {
      this.int_0 = id;
      this.m_roomType = roomType;
      this.m_gameType = gameType;
      this.m_timeType = timeType;
      eRoomType roomType1 = this.m_roomType;
      switch (roomType1)
      {
        case eRoomType.Match:
          this.m_mapType = eMapType.PairUp;
          break;
        case eRoomType.Freedom:
          this.m_mapType = eMapType.Normal;
          break;
        default:
          if ((uint) (roomType1 - 12) > 1U)
          {
            this.m_mapType = eMapType.Normal;
            break;
          }
          goto case eRoomType.Match;
      }
    }

    public virtual void Start()
    {
      this.OnGameStarted();
    }

    public virtual void Stop()
    {
      this.OnGameStopped();
    }

    public virtual bool CanAddPlayer()
    {
      return false;
    }

    public virtual void Pause(int time)
    {
    }

    public virtual void Resume()
    {
    }

    public virtual void MissionStart(IGamePlayer host)
    {
    }

    public virtual void ProcessData(GSPacketIn pkg)
    {
    }

    public virtual Player AddPlayer(IGamePlayer player)
    {
      return (Player) null;
    }

    public virtual Player RemovePlayer(IGamePlayer player, bool IsKick)
    {
      return (Player) null;
    }

    public void Dispose()
    {
      if ((uint) Interlocked.Exchange(ref this.int_1, 1) > 0U)
        return;
      this.Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public event GameEventHandle GameStarted;

    public event GameEventHandle GameStopped;

    protected void OnGameStarted()
    {
      if (this.GameStarted == null)
        return;
      this.GameStarted(this);
    }

    protected void OnGameStopped()
    {
      if (this.GameStopped == null)
        return;
      this.GameStopped(this);
    }
  }
}
