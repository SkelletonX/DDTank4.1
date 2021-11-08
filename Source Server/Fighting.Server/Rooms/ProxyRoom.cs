// Decompiled with JetBrains decompiler
// Type: Fighting.Server.Rooms.ProxyRoom
// Assembly: Fighting.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8F1EB855-F1B7-44B3-B212-6508DAF33CC5
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Fight\Fighting.Server.dll

using Fighting.Server.GameObjects;
using Game.Base.Packets;
using Game.Logic;
using log4net;
using System.Collections.Generic;
using System.Reflection;

namespace Fighting.Server.Rooms
{
  public class ProxyRoom
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private List<IGamePlayer> list_0;
    private int int_0;
    private int int_1;
    private ServerClient serverClient_0;
    public bool bool_0;
    public int PickUpNPCTotal;
    public int PickUpCount;
    public int selfId;
    public bool startWithNpc;
    private int int_4;
    private bool bool_2;
    private bool bool_3;
    private int rYyHcnovuT;
    private int int_5;
    public bool IsPlaying;
    public eGameType GameType;
    public eRoomType RoomType;
    public bool IsCrossZone;
    public int GuildId;
    public string GuildName;
    public int AvgLevel;
    public int FightPower;
    private BaseGame baseGame_0;

    public bool HaveNewbie { get; set; }

    public int PickUpRate { get; set; }

    public int PickUpRateLevel { get; set; }

    public int RoomId
    {
      get
      {
        return this.int_0;
      }
    }

    public ServerClient Client
    {
      get
      {
        return this.serverClient_0;
      }
    }

    public int NpcId
    {
      get
      {
        return this.int_4;
      }
      set
      {
        this.int_4 = value;
      }
    }

    public bool isAutoBot
    {
      get
      {
        return this.bool_2;
      }
    }

    public bool isBotSnape
    {
      get
      {
        return this.bool_3;
      }
      set
      {
        this.bool_3 = value;
      }
    }

    public int EliteGameType
    {
      get
      {
        return this.rYyHcnovuT;
      }
    }

    public int ZoneId
    {
      get
      {
        return this.int_5;
      }
    }

    public ProxyRoom(
      int roomId,
      int orientRoomId,
      int zoneID,
      IGamePlayer[] players,
      ServerClient client,
      int npcId,
      bool pickUpWithNPC,
      bool isBot,
      bool isSmartBot)
    {
      this.int_4 = npcId;
      this.int_0 = roomId;
      this.int_1 = orientRoomId;
      this.list_0 = new List<IGamePlayer>();
      this.list_0.AddRange((IEnumerable<IGamePlayer>) players);
      this.serverClient_0 = client;
      this.bool_0 = pickUpWithNPC;
      this.bool_2 = isBot;
      this.bool_3 = isSmartBot;
      this.PickUpCount = 0;
      this.HaveNewbie = false;
      if (this.GameType == eGameType.EliteGameScore)
        this.method_0();
      this.PickUpRate = 5;
      this.PickUpRateLevel = 1;
      this.PickUpNPCTotal = 0;
      this.int_5 = zoneID;
    }

    private void method_0()
    {
      if (this.list_0[0].PlayerCharacter.Grade <= 40)
        this.rYyHcnovuT = 1;
      else
        this.rYyHcnovuT = 2;
    }

    public void SendToAll(GSPacketIn pkg)
    {
      this.SendToAll(pkg, (IGamePlayer) null);
    }

    public void SendToAll(GSPacketIn pkg, IGamePlayer except)
    {
      this.serverClient_0.SendToRoom(this.int_1, pkg, except);
    }

    public int PlayerCount
    {
      get
      {
        return this.list_0.Count;
      }
    }

    public List<IGamePlayer> GetPlayers()
    {
      List<IGamePlayer> gamePlayerList = new List<IGamePlayer>();
      lock (this.list_0)
        gamePlayerList.AddRange((IEnumerable<IGamePlayer>) this.list_0);
      return gamePlayerList;
    }

    public List<IGamePlayer> GetNewbie(int grade)
    {
      List<IGamePlayer> gamePlayerList = new List<IGamePlayer>();
      lock (this.list_0)
      {
        foreach (IGamePlayer gamePlayer in this.list_0)
        {
          if (gamePlayer.PlayerCharacter.Grade <= grade)
          {
            gamePlayerList.Add(gamePlayer);
            this.HaveNewbie = true;
          }
        }
      }
      return gamePlayerList;
    }

    public void SetDefaultDamageAll()
    {
      lock (this.list_0)
      {
        foreach (ProxyPlayer proxyPlayer in this.list_0)
        {
          if (proxyPlayer != null)
          {
            proxyPlayer.PlayerCharacter.Attack = 1700;
            proxyPlayer.PlayerCharacter.Defence = 1500;
            proxyPlayer.PlayerCharacter.Agility = 1600;
            proxyPlayer.PlayerCharacter.Luck = 1500;
            proxyPlayer.PlayerCharacter.hp = 25000;
          }
        }
      }
    }

    public bool RemovePlayer(IGamePlayer player)
    {
      bool flag = false;
      lock (this.list_0)
      {
        if (this.list_0.Remove(player))
          flag = true;
      }
      if (this.PlayerCount == 0)
        ProxyRoomMgr.RemoveRoom(this);
      return flag;
    }

    public BaseGame Game
    {
      get
      {
        return this.baseGame_0;
      }
    }

    public void StartGame(BaseGame game)
    {
      this.IsPlaying = true;
      this.baseGame_0 = game;
      game.GameStopped += new GameEventHandle(this.method_1);
      this.serverClient_0.SendStartGame(this.int_1, (AbstractGame) game);
    }

    private void method_1(AbstractGame abstractGame_0)
    {
      this.baseGame_0.GameStopped -= new GameEventHandle(this.method_1);
      this.IsPlaying = false;
      this.serverClient_0.SendStopGame(this.int_1, this.baseGame_0.Id);
    }

    public void Dispose()
    {
      this.serverClient_0.RemoveRoom(this.int_1, this);
    }

    public override string ToString()
    {
      return string.Format("RoomId:{0} OriendId:{1} PlayerCount:{2},IsPlaying:{3},GuildId:{4},GuildName:{5}", (object) this.int_0, (object) this.int_1, (object) this.list_0.Count, (object) this.IsPlaying, (object) this.GuildId, (object) this.GuildName);
    }
  }
}
