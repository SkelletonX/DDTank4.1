// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.MarryRoom
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.SceneMarryRooms
{
  public class MarryRoom
  {
    private static object _syncStop = new object();
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private int _count;
    private List<GamePlayer> _guestsList;
    private IMarryProcessor _processor;
    private eRoomState _roomState;
    private Timer _timer;
    private Timer _timerForHymeneal;
    private List<int> _userForbid;
    private List<int> _userRemoveList;
    public MarryRoomInfo Info;

    public MarryRoom(MarryRoomInfo info, IMarryProcessor processor)
    {
      this.Info = info;
      this._processor = processor;
      this._guestsList = new List<GamePlayer>();
      this._count = 0;
      this._roomState = eRoomState.FREE;
      this._userForbid = new List<int>();
      this._userRemoveList = new List<int>();
    }

    public bool AddPlayer(GamePlayer player)
    {
      lock (MarryRoom._syncStop)
      {
        if (player.CurrentRoom != null || player.IsInMarryRoom)
          return false;
        if (this._guestsList.Count > this.Info.MaxCount)
        {
          player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("MarryRoom.Msg1"));
          return false;
        }
        ++this._count;
        this._guestsList.Add(player);
        player.CurrentMarryRoom = this;
        player.MarryMap = 1;
        if (player.CurrentRoom != null)
          player.CurrentRoom.RemovePlayerUnsafe(player);
      }
      return true;
    }

    public void BeginTimer(int interval)
    {
      if (this._timer == null)
        this._timer = new Timer(new TimerCallback(this.OnTick), (object) null, interval, interval);
      else
        this._timer.Change(interval, interval);
    }

    public void BeginTimerForHymeneal(int interval)
    {
      if (this._timerForHymeneal == null)
        this._timerForHymeneal = new Timer(new TimerCallback(this.OnTickForHymeneal), (object) null, interval, interval);
      else
        this._timerForHymeneal.Change(interval, interval);
    }

    public bool CheckUserForbid(int userID)
    {
      lock (MarryRoom._syncStop)
        return this._userForbid.Contains(userID);
    }

    public GamePlayer[] GetAllPlayers()
    {
      lock (MarryRoom._syncStop)
        return this._guestsList.ToArray();
    }

    public GamePlayer GetPlayerByUserID(int userID)
    {
      lock (MarryRoom._syncStop)
      {
        foreach (GamePlayer guests in this._guestsList)
        {
          if (guests.PlayerCharacter.ID == userID)
            return guests;
        }
      }
      return (GamePlayer) null;
    }

    public void KickAllPlayer()
    {
      foreach (GamePlayer allPlayer in this.GetAllPlayers())
      {
        this.RemovePlayer(allPlayer);
        allPlayer.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("MarryRoom.TimeOver"));
      }
    }

    public bool KickPlayerByUserID(GamePlayer player, int userID)
    {
      GamePlayer playerByUserId = this.GetPlayerByUserID(userID);
      if (playerByUserId == null || playerByUserId.PlayerCharacter.ID == player.CurrentMarryRoom.Info.GroomID || playerByUserId.PlayerCharacter.ID == player.CurrentMarryRoom.Info.BrideID)
        return false;
      this.RemovePlayer(playerByUserId);
      playerByUserId.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("Game.Server.SceneGames.KickRoom"));
      GSPacketIn packet = player.Out.SendMessage(eMessageType.ChatERROR, playerByUserId.PlayerCharacter.NickName + "  " + LanguageMgr.GetTranslation("Game.Server.SceneGames.KickRoom2"));
      player.CurrentMarryRoom.SendToPlayerExceptSelf(packet, player);
      return true;
    }

    protected void OnTick(object obj)
    {
      this._processor.OnTick(this);
    }

    protected void OnTickForHymeneal(object obj)
    {
      try
      {
        this._roomState = eRoomState.FREE;
        GSPacketIn packet = new GSPacketIn((short) 249);
        packet.WriteByte((byte) 9);
        this.SendToAll(packet);
        this.StopTimerForHymeneal();
        this.SendUserRemoveLate();
        this.SendMarryRoomInfoUpdateToScenePlayers(this);
      }
      catch (Exception ex)
      {
        if (!MarryRoom.log.IsErrorEnabled)
          return;
        MarryRoom.log.Error((object) nameof (OnTickForHymeneal), ex);
      }
    }

    public void ProcessData(GamePlayer player, GSPacketIn data)
    {
      lock (MarryRoom._syncStop)
        this._processor.OnGameData(this, player, data);
    }

    public void RemovePlayer(GamePlayer player)
    {
      lock (MarryRoom._syncStop)
      {
        if (this.RoomState == eRoomState.FREE)
        {
          --this._count;
          this._guestsList.Remove(player);
          GSPacketIn packet = player.Out.SendPlayerLeaveMarryRoom(player);
          player.CurrentMarryRoom.SendToPlayerExceptSelfForScene(packet, player);
          player.CurrentMarryRoom = (MarryRoom) null;
          player.MarryMap = 0;
        }
        else if (this.RoomState == eRoomState.Hymeneal)
        {
          this._userRemoveList.Add(player.PlayerCharacter.ID);
          --this._count;
          this._guestsList.Remove(player);
          player.CurrentMarryRoom = (MarryRoom) null;
        }
        this.SendMarryRoomInfoUpdateToScenePlayers(this);
      }
    }

    public void ReturnPacket(GamePlayer player, GSPacketIn packet)
    {
      GSPacketIn packet1 = packet.Clone();
      packet1.ClientID = player.PlayerCharacter.ID;
      this.SendToPlayerExceptSelf(packet1, player);
    }

    public void ReturnPacketForScene(GamePlayer player, GSPacketIn packet)
    {
      GSPacketIn packet1 = packet.Clone();
      packet1.ClientID = player.PlayerCharacter.ID;
      this.SendToPlayerExceptSelfForScene(packet1, player);
    }

    public void RoomContinuation(int time)
    {
      int num = this.Info.AvailTime * 60 - (DateTime.Now - this.Info.BeginTime).Minutes + time * 60;
      this.Info.AvailTime += time;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
        playerBussiness.UpdateMarryRoomInfo(this.Info);
      this.BeginTimer(60000 * num);
    }

    public GSPacketIn SendMarryRoomInfoUpdateToScenePlayers(MarryRoom room)
    {
      GSPacketIn packet = new GSPacketIn((short) byte.MaxValue);
      bool val = room != null;
      packet.WriteBoolean(val);
      if (val)
      {
        packet.WriteInt(room.Info.ID);
        packet.WriteBoolean(room.Info.IsHymeneal);
        packet.WriteString(room.Info.Name);
        packet.WriteBoolean(!(room.Info.Pwd == ""));
        packet.WriteInt(room.Info.MapIndex);
        packet.WriteInt(room.Info.AvailTime);
        packet.WriteInt(room.Count);
        packet.WriteInt(room.Info.PlayerID);
        packet.WriteString(room.Info.PlayerName);
        packet.WriteInt(room.Info.GroomID);
        packet.WriteString(room.Info.GroomName);
        packet.WriteInt(room.Info.BrideID);
        packet.WriteString(room.Info.BrideName);
        packet.WriteDateTime(room.Info.BeginTime);
        packet.WriteByte((byte) room.RoomState);
        packet.WriteString(room.Info.RoomIntroduction);
      }
      this.SendToScenePlayer(packet);
      return packet;
    }

    public void SendToAll(GSPacketIn packet)
    {
      this.SendToAll(packet, (GamePlayer) null, false);
    }

    public void SendToAll(GSPacketIn packet, GamePlayer self, bool isChat)
    {
      GamePlayer[] allPlayers = this.GetAllPlayers();
      if (allPlayers == null)
        return;
      foreach (GamePlayer gamePlayer in allPlayers)
      {
        if (!isChat || !gamePlayer.IsBlackFriend(self.PlayerCharacter.ID))
          gamePlayer.Out.SendTCP(packet);
      }
    }

    public void SendToAllForScene(GSPacketIn packet, int sceneID)
    {
      GamePlayer[] allPlayers = this.GetAllPlayers();
      if (allPlayers == null)
        return;
      foreach (GamePlayer gamePlayer in allPlayers)
      {
        if (gamePlayer.MarryMap == sceneID)
          gamePlayer.Out.SendTCP(packet);
      }
    }

    public void SendToPlayerExceptSelf(GSPacketIn packet, GamePlayer self)
    {
      GamePlayer[] allPlayers = this.GetAllPlayers();
      if (allPlayers == null)
        return;
      foreach (GamePlayer gamePlayer in allPlayers)
      {
        if (gamePlayer != self)
          gamePlayer.Out.SendTCP(packet);
      }
    }

    public void SendToPlayerExceptSelfForScene(GSPacketIn packet, GamePlayer self)
    {
      GamePlayer[] allPlayers = this.GetAllPlayers();
      if (allPlayers == null)
        return;
      foreach (GamePlayer gamePlayer in allPlayers)
      {
        if (gamePlayer != self && gamePlayer.MarryMap == self.MarryMap)
          gamePlayer.Out.SendTCP(packet);
      }
    }

    public void SendToRoomPlayer(GSPacketIn packet)
    {
      GamePlayer[] allPlayers = this.GetAllPlayers();
      if (allPlayers == null)
        return;
      foreach (GamePlayer gamePlayer in allPlayers)
        gamePlayer.Out.SendTCP(packet);
    }

    public void SendToScenePlayer(GSPacketIn packet)
    {
      WorldMgr.MarryScene.SendToALL(packet);
    }

    public void SendUserRemoveLate()
    {
      lock (MarryRoom._syncStop)
      {
        foreach (int userRemove in this._userRemoveList)
          this.SendToAllForScene(new GSPacketIn((short) 244, userRemove), 1);
        this._userRemoveList.Clear();
      }
    }

    public void SetUserForbid(int userID)
    {
      lock (MarryRoom._syncStop)
        this._userForbid.Add(userID);
    }

    public void StopTimer()
    {
      if (this._timer == null)
        return;
      this._timer.Dispose();
      this._timer = (Timer) null;
    }

    public void StopTimerForHymeneal()
    {
      if (this._timerForHymeneal == null)
        return;
      this._timerForHymeneal.Dispose();
      this._timerForHymeneal = (Timer) null;
    }

    public int Count
    {
      get
      {
        return this._count;
      }
    }

    public eRoomState RoomState
    {
      get
      {
        return this._roomState;
      }
      set
      {
        if (this._roomState == value)
          return;
        this._roomState = value;
        this.SendMarryRoomInfoUpdateToScenePlayers(this);
      }
    }
  }
}
