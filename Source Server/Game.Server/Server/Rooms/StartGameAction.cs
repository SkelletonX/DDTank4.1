// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.StartGameAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Logic;
using Game.Server.Battle;
using Game.Server.GameObjects;
using Game.Server.Games;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.RingStation;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Rooms
{
  public class StartGameAction : IAction
  {
    private BaseRoom baseRoom_0;

    public StartGameAction(BaseRoom room)
    {
      this.baseRoom_0 = room;
    }

    public void Execute()
    {
      if (!this.baseRoom_0.CanStart())
        return;
      List<GamePlayer> players1 = this.baseRoom_0.GetPlayers();
      if (this.baseRoom_0.RoomType == eRoomType.Freedom)
      {
        List<IGamePlayer> red = new List<IGamePlayer>();
        List<IGamePlayer> blue = new List<IGamePlayer>();
        foreach (GamePlayer gamePlayer in players1)
        {
          if (gamePlayer != null)
          {
            if (gamePlayer.CurrentRoomTeam == 1)
              red.Add((IGamePlayer) gamePlayer);
            else
              blue.Add((IGamePlayer) gamePlayer);
            gamePlayer.PetBag.ReduceHunger();
          }
        }
        this.method_0(GameMgr.StartPVPGame(this.baseRoom_0.RoomId, red, blue, this.baseRoom_0.MapId, this.baseRoom_0.RoomType, this.baseRoom_0.GameType, (int) this.baseRoom_0.TimeMode));
      }
      else if (this.baseRoom_0.IsPVE())
      {
        List<IGamePlayer> players2 = new List<IGamePlayer>();
        foreach (GamePlayer gamePlayer in players1)
        {
          if (gamePlayer != null)
            players2.Add((IGamePlayer) gamePlayer);
        }
        this.method_1();
        this.method_0(GameMgr.StartPVEGame(this.baseRoom_0.RoomId, players2, this.baseRoom_0.MapId, this.baseRoom_0.RoomType, this.baseRoom_0.GameType, (int) this.baseRoom_0.TimeMode, this.baseRoom_0.HardLevel, this.baseRoom_0.LevelLimits, this.baseRoom_0.currentFloor));
      }
      else if (this.baseRoom_0.RoomType == eRoomType.Match)
      {
        this.baseRoom_0.UpdateAvgLevel();
        this.method_1();
        foreach (GamePlayer player in this.baseRoom_0.GetPlayers())
        {
          DateTime dateTime = WorldMgr.CheckTimeEnterRoom(player.PlayerId);
          if (dateTime > DateTime.Now)
          {
            this.baseRoom_0.SendToAll(this.baseRoom_0.Host.Out.SendMessage(eMessageType.ChatERROR, string.Format("{0} está bloqueado de entrar na sala devido a mais de 3 tentativas. Depois de {1} poderá entrar novamente.", (object) player.PlayerCharacter.NickName, (object) dateTime.ToShortTimeString())), this.baseRoom_0.Host);
            this.baseRoom_0.SendCancelPickUp();
            return;
          }
        }
        if (this.baseRoom_0.GetPlayers().Count == 1 && this.baseRoom_0.Host != null && !this.baseRoom_0.isCrosszone)
        {
          if (this.baseRoom_0.Host.PlayerCharacter.Grade <= 5)
          {
            this.baseRoom_0.StartWithNpc = true;
            this.baseRoom_0.PickUpNpcId = RingStationMgr.GetAutoBot(this.baseRoom_0.Host, (int) this.baseRoom_0.RoomType, (int) this.baseRoom_0.GameType);
            Console.WriteLine("GetAutoBot {0}", (object) this.baseRoom_0.PickUpNpcId);
          }
          else
          {
            this.baseRoom_0.PickUpNpcId = RingStationConfiguration.NextPlayerID();
            Console.WriteLine("NextPlayerID {0}", (object) this.baseRoom_0.PickUpNpcId);
          }
        }
        BattleServer battleServer = BattleMgr.AddRoom(this.baseRoom_0);
        if (battleServer != null)
        {
          this.baseRoom_0.BattleServer = battleServer;
          this.baseRoom_0.IsPlaying = true;
          this.baseRoom_0.SendStartPickUp();
        }
        else
        {
          this.baseRoom_0.SendToAll(this.baseRoom_0.Host.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("GameServer.FightBattle.NotReady.Msg")), this.baseRoom_0.Host);
          this.baseRoom_0.SendCancelPickUp();
        }
      }
      else if (this.baseRoom_0.RoomType == eRoomType.EliteGameScore || this.baseRoom_0.RoomType == eRoomType.EliteGameChampion)
      {
        this.baseRoom_0.UpdateAvgLevel();
        if (this.baseRoom_0.GetPlayers().Count == 1)
        {
          if (ExerciseMgr.IsBlockWeapon(this.baseRoom_0.Host.MainWeapon.TemplateID))
          {
            this.baseRoom_0.Host.SendMessage("Bạn đang sử dụng vũ khí bị cấm khi thi đấu Vua Gà. Vui lòng sử dụng vũ khí khác.");
            this.baseRoom_0.SendCancelPickUp();
            return;
          }
          if (this.baseRoom_0.RoomType == eRoomType.EliteGameChampion)
          {
            EliteGameRoundInfo eliteRoundByUser = ExerciseMgr.FindEliteRoundByUser(this.baseRoom_0.Host.PlayerCharacter.ID);
            if (eliteRoundByUser != null)
            {
              this.baseRoom_0.Host.CurrentEnemyId = eliteRoundByUser.PlayerOne.UserID == this.baseRoom_0.Host.PlayerCharacter.ID ? eliteRoundByUser.PlayerTwo.UserID : eliteRoundByUser.PlayerOne.UserID;
            }
            else
            {
              Console.WriteLine("/// Host is not ALLOW HERE: " + (object) this.baseRoom_0.Host.PlayerCharacter.ID);
              this.baseRoom_0.Host.SendMessage("Você não é elegível para participar do Torneio de Elite");
              this.baseRoom_0.SendCancelPickUp();
              return;
            }
          }
          BattleServer battleServer = BattleMgr.AddRoom(this.baseRoom_0);
          if (battleServer != null)
          {
            this.baseRoom_0.BattleServer = battleServer;
            this.baseRoom_0.IsPlaying = true;
            this.baseRoom_0.SendStartPickUp();
            if (this.baseRoom_0.RoomType == eRoomType.EliteGameChampion)
              GameServer.Instance.LoginServer.SendEliteChampionBattleStatus(this.baseRoom_0.Host.PlayerCharacter.ID, true);
          }
          else
          {
            this.baseRoom_0.SendToAll(this.baseRoom_0.Host.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("GameServer.FightBattle.NotReady.Msg")), this.baseRoom_0.Host);
            this.baseRoom_0.SendCancelPickUp();
          }
        }
      }
      RoomMgr.WaitingRoom.SendUpdateCurrentRoom(this.baseRoom_0);
    }

    private void method_0(BaseGame baseGame_0)
    {
      if (baseGame_0 != null)
      {
        this.baseRoom_0.IsPlaying = true;
        this.baseRoom_0.StartGame((AbstractGame) baseGame_0);
      }
      else
      {
        this.baseRoom_0.IsPlaying = false;
        this.baseRoom_0.SendPlayerState();
      }
    }

    private void method_1()
    {
      if (this.baseRoom_0.IsPVE())
      {
        switch (this.baseRoom_0.HardLevel)
        {
          case eHardLevel.Normal:
            this.baseRoom_0.TimeMode = (byte) 5;
            break;
          case eHardLevel.Hard:
            this.baseRoom_0.TimeMode = (byte) 4;
            break;
          case eHardLevel.Terror:
            this.baseRoom_0.TimeMode = (byte) 3;
            break;
          case eHardLevel.Simple:
            this.baseRoom_0.TimeMode = (byte) 6;
            break;
        }
      }
      else
        this.baseRoom_0.TimeMode = (byte) 5;
    }
  }
}
