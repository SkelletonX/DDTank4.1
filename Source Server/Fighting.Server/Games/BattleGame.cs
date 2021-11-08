// Decompiled with JetBrains decompiler
// Type: Fighting.Server.Games.BattleGame
// Assembly: Fighting.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8F1EB855-F1B7-44B3-B212-6508DAF33CC5
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Fight\Fighting.Server.dll

using Fighting.Server.Rooms;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Maps;
using System.Collections.Generic;
using System.Text;

namespace Fighting.Server.Games
{
  public class BattleGame : PVPGame
  {
    private ProxyRoom m_roomBlue;
    private ProxyRoom m_roomRed;

    public BattleGame(
      int id,
      List<IGamePlayer> red,
      ProxyRoom roomRed,
      List<IGamePlayer> blue,
      ProxyRoom roomBlue,
      Map map,
      eRoomType roomType,
      eGameType gameType,
      int timeType)
      : base(id, roomBlue.RoomId, red, blue, map, roomType, gameType, timeType)
    {
      this.m_roomRed = roomRed;
      this.m_roomBlue = roomBlue;
    }

    public override void SendToAll(GSPacketIn pkg, IGamePlayer except)
    {
      if (this.m_roomRed != null)
        this.m_roomRed.SendToAll(pkg, except);
      if (this.m_roomBlue == null)
        return;
      this.m_roomBlue.SendToAll(pkg, except);
    }

    public override void SendToTeam(GSPacketIn pkg, int team, IGamePlayer except)
    {
      if (team == 1)
        this.m_roomRed.SendToAll(pkg, except);
      else
        this.m_roomBlue.SendToAll(pkg, except);
    }

    public override string ToString()
    {
      return new StringBuilder(base.ToString()).Append(",class=BattleGame").ToString();
    }

    public ProxyRoom Blue
    {
      get
      {
        return this.m_roomBlue;
      }
    }

    public ProxyRoom Red
    {
      get
      {
        return this.m_roomRed;
      }
    }
  }
}
