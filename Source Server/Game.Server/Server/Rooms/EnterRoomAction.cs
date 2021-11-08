// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.EnterRoomAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Logic;
using Game.Server.GameObjects;
using Game.Server.Packets;

namespace Game.Server.Rooms
{
  internal class EnterRoomAction : IAction
  {
    private GamePlayer m_player;
    private int m_roomId;
    private string m_pwd;
    private int m_type;
    private int m_hallType;
    private bool m_isInvite;

    public EnterRoomAction(
      GamePlayer player,
      int roomId,
      string pwd,
      int hallType,
      bool isInvite)
    {
      this.m_player = player;
      this.m_roomId = roomId;
      this.m_pwd = pwd;
      this.m_hallType = hallType;
      this.m_isInvite = isInvite;
    }

    public void Execute()
    {
      bool flag = true;
      if (!this.m_player.IsActive)
        return;
      if (this.m_player.CurrentRoom != null)
        this.m_player.CurrentRoom.RemovePlayerUnsafe(this.m_player);
      BaseRoom[] rooms = RoomMgr.Rooms;
      BaseRoom randomRoom;
      if (this.m_roomId == -1)
      {
        randomRoom = this.FindRandomRoom(rooms);
        if (randomRoom == null)
        {
          this.m_player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("EnterRoomAction.noroom"));
          this.m_player.Out.SendRoomLoginResult(false);
          return;
        }
      }
      else
      {
        if (this.m_roomId > rooms.Length || this.m_roomId <= 0)
        {
          this.m_player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("EnterRoomAction.noexist"));
          return;
        }
        randomRoom = rooms[this.m_roomId - 1];
      }
      if (!randomRoom.IsUsing)
        this.m_player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("EnterRoomAction.noexist"));
      else if (this.m_hallType == 1 && (randomRoom.RoomType == eRoomType.Academy || randomRoom.RoomType == eRoomType.Exploration || randomRoom.RoomType == eRoomType.Dungeon))
        this.m_player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("EnterRoomAction.NotInPve"));
      else if (this.m_hallType == 2 && (randomRoom.RoomType == eRoomType.Freedom || randomRoom.RoomType == eRoomType.Match))
      {
        this.m_player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("EnterRoomAction.NotInPvp"));
      }
      else
      {
        if (randomRoom.IsPlaying)
        {
          if (randomRoom.Game is PVEGame)
          {
            if ((randomRoom.Game as PVEGame).GameState != eGameState.SessionPrepared || !this.m_isInvite)
            {
              this.m_player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("EnterRoomAction.start"));
              flag = false;
            }
          }
          else
          {
            this.m_player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("EnterRoomAction.start"));
            flag = false;
          }
        }
        if (flag)
        {
          if (randomRoom.PlayerCount == randomRoom.PlacesCount)
          {
            flag = false;
            this.m_player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("EnterRoomAction.full"));
          }
          else if (!randomRoom.NeedPassword || randomRoom.Password == this.m_pwd)
          {
            if (randomRoom.Game == null || randomRoom.Game.CanAddPlayer())
            {
              if (randomRoom.RoomType == eRoomType.Dungeon && (eLevelLimits) randomRoom.LevelLimits > randomRoom.GetLevelLimit(this.m_player))
              {
                this.m_player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("EnterRoomAction.level"));
                return;
              }
              RoomMgr.WaitingRoom.RemovePlayer(this.m_player);
              this.m_player.Out.SendRoomLoginResult(true);
              this.m_player.Out.SendRoomCreate(randomRoom);
              if (randomRoom.AddPlayerUnsafe(this.m_player) && randomRoom.Game != null)
                randomRoom.Game.AddPlayer((IGamePlayer) this.m_player);
              RoomMgr.WaitingRoom.SendUpdateRoom(randomRoom);
              this.m_player.Out.SendGameRoomSetupChange(randomRoom);
            }
          }
          else
          {
            this.m_player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, !randomRoom.NeedPassword || !string.IsNullOrEmpty(this.m_pwd) ? LanguageMgr.GetTranslation("EnterRoomAction.passworderror") : LanguageMgr.GetTranslation("EnterRoomAction.EnterPassword"));
            this.m_player.Out.SendRoomLoginResult(false);
          }
        }
        if (flag)
          return;
        int roomId = this.m_roomId;
      }
    }

    private BaseRoom FindRandomRoom(BaseRoom[] rooms)
    {
      for (int index = 0; index < rooms.Length; ++index)
      {
        if (rooms[index].PlayerCount > 0 && rooms[index].CanAddPlayer() && (!rooms[index].NeedPassword && !rooms[index].IsPlaying) && rooms[index].RoomType != eRoomType.Freshman)
        {
          if (10 != this.m_type)
          {
            if (rooms[index].RoomType == (eRoomType) this.m_type)
              return rooms[index];
          }
          else if (rooms[index].RoomType == (eRoomType) this.m_type && (eLevelLimits) rooms[index].LevelLimits < rooms[index].GetLevelLimit(this.m_player))
            return rooms[index];
        }
      }
      return (BaseRoom) null;
    }
  }
}
