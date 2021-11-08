// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.BaseRoom
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Object;
using Game.Server.Battle;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.Rooms
{
  public class BaseRoom
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private GamePlayer[] gamePlayer_0;
    private int[] int_0;
    private byte[] byte_0;
    private int int_1;
    private int int_2;
    private bool bool_0;
    private GamePlayer gamePlayer_1;
    public bool IsPlaying;
    public int RoomId;
    public int GameStyle;
    public string Name;
    public string Password;
    public eRoomType RoomType;
    public eGameType GameType;
    public eHardLevel HardLevel;
    public bool isCrosszone;
    public int currentFloor;
    public int LevelLimits;
    public byte TimeMode;
    public int MapId;
    public string m_roundName;
    public int maxViewerCnt;
    private int int_3;
    public int PickUpNpcId;
    public DateTime TimeStartGame;
    public bool IsDebug;
    public int ZoneId;
    private bool bool_1;
    private string string_0;
    private int int_4;
    private int int_5;
    private AbstractGame abstractGame_0;
    public BattleServer BattleServer;

    public int viewerCnt
    {
      get
      {
        return this.int_3;
      }
    }

    public GamePlayer Host
    {
      get
      {
        return this.gamePlayer_1;
      }
    }

    public byte[] PlayerState
    {
      get
      {
        return this.byte_0;
      }
      set
      {
        this.byte_0 = value;
      }
    }

    public int PlayerCount
    {
      get
      {
        return this.int_1;
      }
    }

    public int PlacesCount
    {
      get
      {
        return this.int_2;
      }
    }

    public int GuildId
    {
      get
      {
        return this.gamePlayer_1.PlayerCharacter.ConsortiaID;
      }
    }

    public bool IsUsing
    {
      get
      {
        return this.bool_0;
      }
    }

    public string RoundName
    {
      get
      {
        return this.m_roundName;
      }
      set
      {
        this.m_roundName = value;
      }
    }

    public bool StartWithNpc
    {
      get
      {
        return this.bool_1;
      }
      set
      {
        this.bool_1 = value;
      }
    }

    public string Pic
    {
      get
      {
        return this.string_0;
      }
      set
      {
        this.string_0 = value;
      }
    }

    public int barrierNum
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

    public BaseRoom(int roomId)
    {
      this.int_2 = 8;
      this.maxViewerCnt = 2;
      this.TimeStartGame = DateTime.MinValue;
      this.RoomId = roomId;
      this.gamePlayer_0 = new GamePlayer[10];
      this.int_0 = new int[10];
      this.byte_0 = new byte[10];
      this.string_0 = "";
      this.int_4 = 0;
      this.method_0();
    }

    public void Start()
    {
      if (this.bool_0)
        return;
      this.bool_0 = true;
      this.method_0();
    }

    public void Stop()
    {
      if (!this.bool_0)
        return;
      this.bool_0 = false;
      if (this.abstractGame_0 != null)
      {
        this.abstractGame_0.GameStopped -= new GameEventHandle(this.method_2);
        this.abstractGame_0 = (AbstractGame) null;
        this.IsPlaying = false;
      }
      RoomMgr.WaitingRoom.SendUpdateCurrentRoom(this);
    }

    private void method_0()
    {
      for (int index = 0; index < 10; ++index)
      {
        this.gamePlayer_0[index] = (GamePlayer) null;
        this.int_0[index] = -1;
        this.byte_0[index] = (byte) 0;
      }
      this.gamePlayer_1 = (GamePlayer) null;
      this.IsPlaying = false;
      this.int_2 = 8;
      this.int_1 = 0;
      this.HardLevel = eHardLevel.Simple;
      this.PickUpNpcId = -1;
      this.bool_1 = false;
    }

    public bool CanStart()
    {
      if (this.RoomType == eRoomType.Freedom)
      {
        int num1 = 0;
        int num2 = 0;
        for (int index = 0; index < 8; ++index)
        {
          if (index % 2 == 0)
          {
            if (this.byte_0[index] > (byte) 0)
              ++num1;
          }
          else if (this.byte_0[index] > (byte) 0)
            ++num2;
        }
        if ((uint) num1 > 0U)
          return (uint) num2 > 0U;
        return false;
      }
      int num = 0;
      for (int index = 0; index < 8; ++index)
      {
        if (this.byte_0[index] > (byte) 0)
          ++num;
      }
      return num == this.int_1;
    }

    public bool NeedPassword
    {
      get
      {
        return !string.IsNullOrEmpty(this.Password);
      }
    }

    public bool CanAddPlayer()
    {
      return this.int_1 < this.int_2;
    }

    public bool RoomCanEnter()
    {
      switch (this.RoomType)
      {
        case eRoomType.Freshman:
        case eRoomType.Academy:
        case eRoomType.CoupleBoss:
          return false;
        default:
          return true;
      }
    }

    public bool IsEmpty
    {
      get
      {
        return this.int_1 == 0;
      }
    }

    public List<GamePlayer> GetPlayers()
    {
      List<GamePlayer> gamePlayerList = new List<GamePlayer>();
      lock (this.gamePlayer_0)
      {
        for (int index = 0; index < 8; ++index)
        {
          if (this.gamePlayer_0[index] != null)
            gamePlayerList.Add(this.gamePlayer_0[index]);
        }
      }
      return gamePlayerList;
    }

    public List<GamePlayer> GetAllPlayers()
    {
      List<GamePlayer> gamePlayerList = new List<GamePlayer>();
      lock (this.gamePlayer_0)
      {
        for (int index = 0; index < 8; ++index)
        {
          if (this.gamePlayer_0[index] != null)
            gamePlayerList.Add(this.gamePlayer_0[index]);
        }
      }
      return gamePlayerList;
    }

    public void SetHost(GamePlayer player)
    {
      if (this.gamePlayer_1 == player)
        return;
      if (this.gamePlayer_1 != null)
        this.UpdatePlayerState(player, (byte) 0, false);
      this.gamePlayer_1 = player;
      this.UpdatePlayerState(player, (byte) 2, true);
    }

    public void UpdateRoom(string name, string pwd, eRoomType roomType, byte timeMode, int mapId)
    {
      this.Name = name;
      this.Password = pwd;
      this.RoomType = roomType;
      this.TimeMode = timeMode;
      this.MapId = mapId;
      this.UpdateRoomGameType();
      this.int_2 = roomType != eRoomType.Freedom ? 4 : 8;
      this.method_1();
    }

    private void method_1()
    {
      for (int pos = 0; pos < 8; ++pos)
      {
        if (pos >= this.int_2)
          this.UpdatePosUnsafe(pos, false, false);
      }
    }

    public void UpdateRoomGameType()
    {
      switch (this.RoomType)
      {
        case eRoomType.Match:
          this.GameType = eGameType.Free;
          break;
        case eRoomType.Freedom:
          this.GameType = eGameType.Free;
          break;
        case eRoomType.Exploration:
          this.GameType = eGameType.Exploration;
          break;
        case eRoomType.Boss:
          this.GameType = eGameType.Boss;
          break;
        case eRoomType.Dungeon:
        case eRoomType.Academy:
        case eRoomType.Labyrinth:
          this.GameType = eGameType.Dungeon;
          break;
        case eRoomType.FightLab:
          this.GameType = eGameType.FightLab;
          break;
        case eRoomType.Freshman:
          this.GameType = eGameType.Freshman;
          break;
        case eRoomType.EliteGameScore:
          this.GameType = eGameType.EliteGameScore;
          break;
        case eRoomType.EliteGameChampion:
          this.GameType = eGameType.EliteGameChampion;
          break;
        default:
          this.GameType = eGameType.ALL;
          break;
      }
    }

    public bool IsPVE()
    {
      switch (this.RoomType)
      {
        case eRoomType.Dungeon:
        case eRoomType.FightLab:
        case eRoomType.Freshman:
        case eRoomType.Academy:
        case eRoomType.Labyrinth:
        case eRoomType.CoupleBoss:
          return true;
        default:
          return false;
      }
    }

    public void UpdatePlayerState(GamePlayer player, byte state, bool sendToClient)
    {
      this.byte_0[player.CurrentRoomIndex] = state;
      if (!sendToClient)
        return;
      this.SendPlayerState();
    }

    public int AvgLevel
    {
      get
      {
        return this.int_5;
      }
    }

    public void UpdateAvgLevel()
    {
      int num = 0;
      for (int index = 0; index < 8; ++index)
      {
        if (this.gamePlayer_0[index] != null)
          num += this.gamePlayer_0[index].PlayerCharacter.Grade;
      }
      this.int_5 = num / this.int_1;
    }

    public void SendRoomSetupChange(BaseRoom room)
    {
      if (this.gamePlayer_1 == null)
        return;
      this.SendToAll(this.gamePlayer_1.Out.SendGameRoomSetupChange(room));
    }

    public void SendLastMissionWarriorArena(bool isLast)
    {
      if (this.gamePlayer_1 == null)
        return;
      GSPacketIn packet = new GSPacketIn((short) 94);
      packet.WriteByte((byte) 33);
      packet.WriteBoolean(isLast);
      this.gamePlayer_1.Out.SendTCP(packet);
    }

    public void SendNoTicketWarriorArena()
    {
      if (this.gamePlayer_1 == null)
        return;
      GSPacketIn packet = new GSPacketIn((short) 94);
      packet.WriteByte((byte) 35);
      packet.WriteString("Sem ingressos para a arena");
      this.gamePlayer_1.Out.SendTCP(packet);
    }

    public void SendWarriorArenaPass10()
    {
      if (this.gamePlayer_1 == null)
        return;
      GSPacketIn packet = new GSPacketIn((short) 94);
      packet.WriteByte((byte) 34);
      this.gamePlayer_1.Out.SendTCP(packet);
    }

    public void SendToAll(GSPacketIn pkg)
    {
      this.SendToAll(pkg, (GamePlayer) null);
    }

    public void SendToAll(GSPacketIn pkg, GamePlayer except)
    {
      GamePlayer[] gamePlayerArray = (GamePlayer[]) null;
      lock (this.gamePlayer_0)
        gamePlayerArray = (GamePlayer[]) this.gamePlayer_0.Clone();
      if (gamePlayerArray == null)
        return;
      for (int index = 0; index < gamePlayerArray.Length; ++index)
      {
        if (gamePlayerArray[index] != null && gamePlayerArray[index] != except)
          gamePlayerArray[index].Out.SendTCP(pkg);
      }
    }

    public void SendToTeam(GSPacketIn pkg, int team)
    {
      this.SendToTeam(pkg, team, (GamePlayer) null);
    }

    public void SendToTeam(GSPacketIn pkg, int team, GamePlayer except)
    {
      GamePlayer[] gamePlayerArray = (GamePlayer[]) null;
      lock (this.gamePlayer_0)
        gamePlayerArray = (GamePlayer[]) this.gamePlayer_0.Clone();
      for (int index = 0; index < gamePlayerArray.Length; ++index)
      {
        if (gamePlayerArray[index] != null && gamePlayerArray[index].CurrentRoomTeam == team && gamePlayerArray[index] != except)
          gamePlayerArray[index].Out.SendTCP(pkg);
      }
    }

    public void SendToHost(GSPacketIn pkg)
    {
      GamePlayer[] gamePlayerArray = (GamePlayer[]) null;
      lock (this.gamePlayer_0)
        gamePlayerArray = (GamePlayer[]) this.gamePlayer_0.Clone();
      for (int index = 0; index < gamePlayerArray.Length; ++index)
      {
        if (gamePlayerArray[index] != null && gamePlayerArray[index] == this.Host)
          gamePlayerArray[index].Out.SendTCP(pkg);
      }
    }

    public void SendPlayerState()
    {
      this.SendToAll(this.gamePlayer_1.Out.SendRoomUpdatePlayerStates(this.byte_0), this.gamePlayer_1);
    }

    public void SendPlaceState()
    {
      if (this.gamePlayer_1 == null)
        return;
      this.SendToAll(this.gamePlayer_1.Out.SendRoomUpdatePlacesStates(this.int_0), this.gamePlayer_1);
    }

    public void SendCancelPickUp()
    {
      if (this.gamePlayer_1 == null)
        return;
      this.SendToAll(this.gamePlayer_1.Out.SendRoomPairUpCancel(this), this.gamePlayer_1);
    }

    public void SendStartPickUp()
    {
      if (this.gamePlayer_1 == null)
        return;
      this.SendToAll(this.gamePlayer_1.Out.SendRoomPairUpStart(this), this.gamePlayer_1);
    }

    public void SendMessage(eMessageType type, string msg)
    {
      if (this.gamePlayer_1 == null)
        return;
      this.SendToAll(this.gamePlayer_1.Out.SendMessage(type, msg), this.gamePlayer_1);
    }

    public bool UpdatePosUnsafe(int pos, bool isOpened)
    {
      return this.UpdatePosUnsafe(pos, isOpened, true);
    }

    public bool UpdatePosUnsafe(int pos, bool isOpened, bool removePlaceCount)
    {
      if ((uint) pos > 7U)
        return false;
      int num = isOpened ? -1 : 0;
      if (this.int_0[pos] == num)
        return false;
      if (this.gamePlayer_0[pos] != null)
        this.RemovePlayerUnsafe(this.gamePlayer_0[pos]);
      this.int_0[pos] = num;
      this.SendPlaceState();
      if (removePlaceCount)
      {
        if (isOpened)
          ++this.int_2;
        else
          --this.int_2;
      }
      return true;
    }

    public bool IsAllSameGuild()
    {
      int guildId = this.GuildId;
      if (guildId == 0)
        return false;
      List<GamePlayer> players = this.GetPlayers();
      if (players.Count < 2)
        return false;
      foreach (GamePlayer gamePlayer in players)
      {
        if (gamePlayer.PlayerCharacter.ConsortiaID != guildId)
          return false;
      }
      return true;
    }

    public void UpdateGameStyle()
    {
      if (this.gamePlayer_1 == null || (uint) this.RoomType > 0U)
        return;
      if (this.GameType == eGameType.Guild && this.IsAllSameGuild())
      {
        this.GameStyle = 1;
        this.GameType = eGameType.Guild;
      }
      else
      {
        this.GameStyle = 0;
        this.GameType = eGameType.Free;
      }
      this.SendToAll(this.gamePlayer_1.Out.SendRoomType(this.gamePlayer_1, this));
    }

    public bool AddPlayerUnsafe(GamePlayer player)
    {
      int num = -1;
      lock (this.gamePlayer_0)
      {
        for (int index = 0; index < 8; ++index)
        {
          if (this.gamePlayer_0[index] == null && this.int_0[index] == -1)
          {
            this.gamePlayer_0[index] = player;
            this.int_0[index] = player.PlayerId;
            ++this.int_1;
            num = index;
            break;
          }
        }
      }
      if (num != -1)
      {
        player.CurrentRoom = this;
        player.CurrentRoomIndex = num;
        player.CurrentRoomTeam = this.RoomType != eRoomType.Freedom ? 1 : num % 2 + 1;
        this.SendToAll(player.Out.SendRoomPlayerAdd(player), player);
        this.SendToAll(player.Out.SendBufferList(player, player.BufferList.GetAllBuffer()), player);
        foreach (GamePlayer allPlayer in this.GetAllPlayers())
        {
          if (allPlayer != player)
          {
            player.Out.SendRoomPlayerAdd(allPlayer);
            player.Out.SendBufferList(allPlayer, allPlayer.BufferList.GetAllBuffer());
          }
        }
        if (this.gamePlayer_1 == null)
        {
          this.gamePlayer_1 = player;
          this.UpdatePlayerState(player, (byte) 2, true);
        }
        else
          this.UpdatePlayerState(player, (byte) 0, true);
        this.SendPlaceState();
      }
      return num != -1;
    }

    public bool RemovePlayerUnsafe(GamePlayer player)
    {
      return this.RemovePlayerUnsafe(player, false);
    }

    public bool RemovePlayerUnsafe(GamePlayer player, bool isKick)
    {
      int pos = -1;
      lock (this.gamePlayer_0)
      {
        for (int index = 0; index < 8; ++index)
        {
          if (this.gamePlayer_0[index] == player)
          {
            this.gamePlayer_0[index] = (GamePlayer) null;
            this.byte_0[index] = (byte) 0;
            this.int_0[index] = -1;
            --this.int_1;
            pos = index;
            break;
          }
        }
      }
      if (pos != -1)
      {
        this.UpdatePosUnsafe(pos, true);
        player.CurrentRoom = (BaseRoom) null;
        player.TempBag.ClearBag();
        this.SendToAll(player.Out.SendRoomPlayerRemove(player));
        if (isKick)
          player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Game.Server.SceneGames.KickRoom"));
        bool flag = false;
        if (this.gamePlayer_1 == player)
        {
          if (this.int_1 > 0)
          {
            for (int index = 0; index < 8; ++index)
            {
              if (this.gamePlayer_0[index] != null)
              {
                this.SetHost(this.gamePlayer_0[index]);
                flag = true;
                break;
              }
            }
          }
          else
            this.gamePlayer_1 = (GamePlayer) null;
        }
        if (this.IsPlaying)
        {
          if (this.abstractGame_0 != null)
          {
            if (flag && this.abstractGame_0 is PVEGame)
            {
              foreach (Player player1 in (this.abstractGame_0 as PVEGame).Players.Values)
              {
                if (player1.PlayerDetail == this.gamePlayer_1)
                  player1.Ready = false;
              }
            }
            this.abstractGame_0.RemovePlayer((IGamePlayer) player, isKick);
          }
          if (this.BattleServer != null)
          {
            if (this.abstractGame_0 != null)
            {
              if (!isKick && this.RoomType == eRoomType.Match)
                WorldMgr.UpdateExitGame(player.PlayerId);
              this.BattleServer.Server.SendPlayerDisconnet(this.Game.Id, player.GameId, this.RoomId);
              if (this.PlayerCount == 0)
                this.BattleServer.RemoveRoom(this);
            }
            else
            {
              this.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("Game.Server.SceneGames.PairUp.Failed"));
              RoomMgr.AddAction((IAction) new CancelPickupAction(this.BattleServer, this));
              this.BattleServer.RemoveRoom(this);
              this.IsPlaying = false;
            }
          }
        }
        else
        {
          this.UpdateGameStyle();
          if (flag)
          {
            this.HardLevel = this.RoomType != eRoomType.Exploration ? eHardLevel.Simple : eHardLevel.Normal;
            foreach (GamePlayer player1 in this.GetPlayers())
              player1.Out.SendGameRoomSetupChange(this);
          }
        }
      }
      return pos != -1;
    }

    public void RemovePlayerAtUnsafe(int pos)
    {
      if (pos < 0 || pos > 7 || this.gamePlayer_0[pos] == null)
        return;
      if (this.gamePlayer_0[pos].KickProtect)
      {
        string translation = LanguageMgr.GetTranslation("Game.Server.SceneGames.Protect", (object) this.gamePlayer_0[pos].PlayerCharacter.NickName);
        GSPacketIn pkg = new GSPacketIn((short) 3);
        pkg.WriteInt(0);
        pkg.WriteString(translation);
        this.SendToHost(pkg);
      }
      else
      {
        if (this.gamePlayer_0[pos] == null)
          return;
        this.RemovePlayerUnsafe(this.gamePlayer_0[pos], true);
      }
    }

    public bool SwitchTeamUnsafe(GamePlayer m_player)
    {
      if (this.RoomType == eRoomType.Match)
        return false;
      int num = -1;
      lock (this.gamePlayer_0)
      {
        for (int index = (m_player.CurrentRoomIndex + 1) % 2; index < 8; index += 2)
        {
          if (this.gamePlayer_0[index] == null && this.int_0[index] == -1)
          {
            num = index;
            this.gamePlayer_0[m_player.CurrentRoomIndex] = (GamePlayer) null;
            this.gamePlayer_0[index] = m_player;
            this.int_0[m_player.CurrentRoomIndex] = -1;
            this.int_0[index] = m_player.PlayerId;
            this.byte_0[index] = this.byte_0[m_player.CurrentRoomIndex];
            this.byte_0[m_player.CurrentRoomIndex] = (byte) 0;
            break;
          }
        }
      }
      if (num == -1)
        return false;
      m_player.CurrentRoomIndex = num;
      m_player.CurrentRoomTeam = num % 2 + 1;
      this.SendToAll(m_player.Out.SendRoomPlayerChangedTeam(m_player), m_player);
      this.SendPlaceState();
      return true;
    }

    public eLevelLimits GetLevelLimit(GamePlayer player)
    {
      if (player.PlayerCharacter.Grade <= 10)
        return eLevelLimits.ZeroToTen;
      return player.PlayerCharacter.Grade > 20 ? eLevelLimits.TwentyOneToThirty : eLevelLimits.ElevenToTwenty;
    }

    public AbstractGame Game
    {
      get
      {
        return this.abstractGame_0;
      }
    }

    public void StartGame(AbstractGame game)
    {
      if (this.abstractGame_0 != null)
      {
        foreach (IGamePlayer player in this.GetPlayers())
          this.abstractGame_0.RemovePlayer(player, false);
        this.method_2(this.abstractGame_0);
      }
      this.abstractGame_0 = game;
      this.IsPlaying = true;
      this.abstractGame_0.GameStopped += new GameEventHandle(this.method_2);
    }

    private void method_2(AbstractGame abstractGame_1)
    {
      if (abstractGame_1 == null)
        return;
      this.abstractGame_0.GameStopped -= new GameEventHandle(this.method_2);
      this.abstractGame_0 = (AbstractGame) null;
      this.IsPlaying = false;
      this.currentFloor = 0;
      RoomMgr.WaitingRoom.SendUpdateCurrentRoom(this);
    }

    public void ProcessData(GSPacketIn packet)
    {
      if (this.abstractGame_0 == null)
        return;
      this.abstractGame_0.ProcessData(packet);
    }

    public void RemoveAllPlayer()
    {
      for (int index = 0; index < 10; ++index)
      {
        if (this.gamePlayer_0[index] != null)
        {
          RoomMgr.AddAction((IAction) new ExitRoomAction(this, this.gamePlayer_0[index], true));
          RoomMgr.AddAction((IAction) new EnterWaitingRoomAction(this.gamePlayer_0[index]));
        }
      }
    }

    public override string ToString()
    {
      return string.Format("Id:{0},player:{1},game:{2},isPlaying:{3}", (object) this.RoomId, (object) this.PlayerCount, (object) this.Game, (object) this.IsPlaying);
    }
  }
}
