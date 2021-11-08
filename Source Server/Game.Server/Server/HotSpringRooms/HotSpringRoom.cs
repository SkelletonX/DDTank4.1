// Decompiled with JetBrains decompiler
// Type: Game.Server.HotSpringRooms.HotSpringRoom
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

namespace Game.Server.HotSpringRooms
{
  public class HotSpringRoom
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static object object_0 = new object();
    private GInterface2 ginterface2_0;
    public HotSpringRoomInfo Info;
    private int int_0;
    private List<GamePlayer> list_0;

    public HotSpringRoom(HotSpringRoomInfo info, GInterface2 processor)
    {
      this.Info = info;
      this.ginterface2_0 = processor;
      this.list_0 = new List<GamePlayer>();
      this.int_0 = 0;
    }

    public bool AddPlayer(GamePlayer player)
    {
      lock (HotSpringRoom.object_0)
      {
        if (player.CurrentRoom != null || player.CurrentHotSpringRoom != null)
          return false;
        if (this.list_0.Count > this.Info.maxCount)
        {
          player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("SpaRoom.Msg1"));
          return false;
        }
        if (player.Extra.Info.MinHotSpring <= 0)
        {
          player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("SpaRoom.TimeOver"));
          return false;
        }
        ++this.int_0;
        this.list_0.Add(player);
        player.CurrentHotSpringRoom = this;
        player.Extra.Info.LastTimeHotSpring = DateTime.Now;
        this.SetDefaultPostion(player);
        if (player.CurrentRoom != null)
          player.CurrentRoom.RemovePlayerUnsafe(player);
        player.Extra.BeginHotSpringTimer();
        player.OnEnterHotSpring();
        HotSpringMgr.SendUpdateAllRoom((GamePlayer) null, new HotSpringRoom[1]
        {
          player.CurrentHotSpringRoom
        });
        GSPacketIn packet = new GSPacketIn((short) 198);
        packet.WriteInt(player.PlayerCharacter.ID);
        packet.WriteInt(player.PlayerCharacter.Grade);
        packet.WriteInt(player.PlayerCharacter.Hide);
        packet.WriteInt(player.PlayerCharacter.Repute);
        packet.WriteString(player.PlayerCharacter.NickName);
        packet.WriteByte(player.PlayerCharacter.typeVIP);
        packet.WriteInt(player.PlayerCharacter.VIPLevel);
        packet.WriteBoolean(player.PlayerCharacter.Sex);
        packet.WriteString(player.PlayerCharacter.Style);
        packet.WriteString(player.PlayerCharacter.Colors);
        packet.WriteString(player.PlayerCharacter.Skin);
        packet.WriteInt(player.Hot_X);
        packet.WriteInt(player.Hot_Y);
        packet.WriteInt(player.PlayerCharacter.FightPower);
        packet.WriteInt(player.PlayerCharacter.Win);
        packet.WriteInt(player.PlayerCharacter.Total);
        packet.WriteInt(player.Hot_Direction);
        player.OnEnterSpa();
        this.SendToPlayerExceptSelf(packet, player);
      }
      return true;
    }

    public bool CanEnter()
    {
      return this.int_0 < this.Info.maxCount;
    }

    public GamePlayer[] GetAllPlayers()
    {
      lock (HotSpringRoom.object_0)
        return this.list_0.ToArray();
    }

    public GamePlayer GetPlayerWithID(int playerId)
    {
      lock (HotSpringRoom.object_0)
      {
        foreach (GamePlayer gamePlayer in this.list_0)
        {
          if (gamePlayer.PlayerCharacter.ID == playerId)
            return gamePlayer;
        }
        return (GamePlayer) null;
      }
    }

    protected void OnTick(object obj)
    {
      this.ginterface2_0.OnTick(this);
    }

    public void ProcessData(GamePlayer player, GSPacketIn data)
    {
      lock (HotSpringRoom.object_0)
        this.ginterface2_0.OnGameData(this, player, data);
    }

    public void RemovePlayer(GamePlayer player)
    {
      lock (HotSpringRoom.object_0)
      {
        if (player.CurrentHotSpringRoom == null)
          return;
        --this.int_0;
        this.list_0.Remove(player);
        player.Extra.StopHotSpringTimer();
        GSPacketIn packet = new GSPacketIn((short) 199, player.PlayerCharacter.ID);
        packet.WriteInt(player.PlayerCharacter.ID);
        packet.WriteString("");
        player.CurrentHotSpringRoom.SendToAll(packet);
        this.SetDefaultPostion(player);
        HotSpringMgr.SendUpdateAllRoom((GamePlayer) null, new HotSpringRoom[1]
        {
          player.CurrentHotSpringRoom
        });
        player.CurrentHotSpringRoom = (HotSpringRoom) null;
      }
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

    public void SendToRoomPlayer(GSPacketIn packet)
    {
      GamePlayer[] allPlayers = this.GetAllPlayers();
      if (allPlayers == null)
        return;
      foreach (GamePlayer gamePlayer in allPlayers)
        gamePlayer.Out.SendTCP(packet);
    }

    public void SetDefaultPostion(GamePlayer p)
    {
      p.Hot_X = 480;
      p.Hot_Y = 560;
      p.Hot_Direction = 3;
    }

    public int Count
    {
      get
      {
        return this.int_0;
      }
    }
  }
}
