// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.AcademyMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Server.GameObjects;
using Game.Server.Packets;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Managers
{
  public class AcademyMgr
  {
    public static readonly int LEVEL_GAP = 5;
    public static readonly int TARGET_PLAYER_MIN_LEVEL = 6;
    public static readonly int ACADEMY_LEVEL_MIN = 20;
    public static readonly int ACADEMY_LEVEL_MAX = 16;
    public static readonly int RECOMMEND_MAX_NUM = 3;
    public static readonly int NONE_STATE = 0;
    public static readonly int APPRENTICE_STATE = 1;
    public static readonly int MASTER_STATE = 2;
    public static readonly int MASTER_FULL_STATE = 3;
    public static object m_object = new object();
    private static List<AcademyRequestInfo> Requests;

    public static bool Init()
    {
      AcademyMgr.Requests = new List<AcademyRequestInfo>();
      return true;
    }

    public static void AddRequest(AcademyRequestInfo request)
    {
      lock (AcademyMgr.Requests)
        AcademyMgr.Requests.Add(request);
    }

    public static AcademyRequestInfo GetRequest(int senderid, int receiveid)
    {
      AcademyRequestInfo academyRequestInfo = (AcademyRequestInfo) null;
      lock (AcademyMgr.Requests)
      {
        foreach (AcademyRequestInfo request in AcademyMgr.Requests)
        {
          if (request.SenderID == senderid && request.ReceiderID == receiveid)
          {
            academyRequestInfo = request;
            break;
          }
        }
      }
      return academyRequestInfo;
    }

    public static void RemoveRequest(AcademyRequestInfo request)
    {
      lock (AcademyMgr.Requests)
        AcademyMgr.Requests.Remove(request);
    }

    public static void RemoveOldRequest()
    {
      List<AcademyRequestInfo> academyRequestInfoList = new List<AcademyRequestInfo>();
      lock (AcademyMgr.Requests)
      {
        foreach (AcademyRequestInfo request in AcademyMgr.Requests)
        {
          if (request.CreateTime.AddHours(1.0) < DateTime.Now)
            academyRequestInfoList.Add(request);
        }
      }
      if (academyRequestInfoList.Count <= 0)
        return;
      foreach (AcademyRequestInfo request in academyRequestInfoList)
        AcademyMgr.RemoveRequest(request);
    }

    public static bool FireApprentice(GamePlayer player, int uid, bool isSilent)
    {
      bool flag = false;
      lock (AcademyMgr.m_object)
      {
        if (flag = player.PlayerCharacter.RemoveMasterOrApprentices(uid))
        {
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
          {
            GamePlayer playerById = WorldMgr.GetPlayerById(uid);
            PlayerInfo player1 = playerById == null ? playerBussiness.GetUserSingleByUserID(uid) : playerById.PlayerCharacter;
            if (player1 != null)
            {
              player1.RemoveMasterOrApprentices(player1.masterID);
              player1.masterID = 0;
              playerBussiness.UpdateAcademyPlayer(player1);
              if (playerById != null)
              {
                if (!isSilent)
                  playerById.Out.SendAcademySystemNotice(LanguageMgr.GetTranslation("Game.Server.AppSystem.BreakApprenticeShipMsg.Apprentice", (object) player.PlayerCharacter.NickName), true);
                playerById.Out.SendAcademyAppState(player1, uid);
              }
            }
          }
        }
      }
      return flag;
    }

    public static bool FireMaster(GamePlayer player, bool isComplete)
    {
      bool flag = false;
      lock (AcademyMgr.m_object)
      {
        if (flag = player.PlayerCharacter.RemoveMasterOrApprentices(player.PlayerCharacter.masterID))
        {
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
          {
            GamePlayer playerById = WorldMgr.GetPlayerById(player.PlayerCharacter.masterID);
            PlayerInfo player1 = playerById == null ? playerBussiness.GetUserSingleByUserID(player.PlayerCharacter.masterID) : playerById.PlayerCharacter;
            if (player1 != null)
            {
              if (isComplete)
              {
                ++player1.graduatesCount;
                playerById?.OnAcademyEvent(player, 2);
                player.OnAcademyEvent((GamePlayer) null, 3);
              }
              player1.RemoveMasterOrApprentices(player.PlayerId);
              playerBussiness.UpdateAcademyPlayer(player1);
              if (playerById != null)
              {
                if (!isComplete)
                  playerById.Out.SendAcademySystemNotice(LanguageMgr.GetTranslation("Game.Server.AppSystem.BreakApprenticeShipMsg.Master", (object) player.PlayerCharacter.NickName), true);
                playerById.Out.SendAcademyAppState(player1, player.PlayerCharacter.ID);
              }
              player.PlayerCharacter.masterID = 0;
            }
          }
        }
      }
      return flag;
    }

    public static bool AddApprentice(GamePlayer master, GamePlayer app)
    {
      bool flag = false;
      lock (AcademyMgr.m_object)
      {
        if (flag = master.PlayerCharacter.AddMasterOrApprentices(app.PlayerCharacter.ID, app.PlayerCharacter.NickName) && app.PlayerCharacter.masterID == 0)
        {
          app.PlayerCharacter.masterID = master.PlayerCharacter.ID;
          app.PlayerCharacter.AddMasterOrApprentices(master.PlayerCharacter.ID, master.PlayerCharacter.NickName);
          app.Out.SendAcademyAppState(app.PlayerCharacter, -1);
          master.Out.SendAcademyAppState(master.PlayerCharacter, -1);
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
          {
            playerBussiness.UpdateAcademyPlayer(app.PlayerCharacter);
            playerBussiness.UpdateAcademyPlayer(master.PlayerCharacter);
          }
          app.OnAcademyEvent(master, 0);
          master.OnAcademyEvent(app, 1);
        }
      }
      return flag;
    }

    public static void UpdateAwardApp(GamePlayer player, int oldLevel)
    {
      Dictionary<int, int> dictionary1 = GameProperties.AcademyApprenticeAwardArr();
      Dictionary<int, int> dictionary2 = GameProperties.AcademyMasterAwardArr();
      int masterId = player.PlayerCharacter.masterID;
      string translation1 = LanguageMgr.GetTranslation("Game.Server.Managers.AcademyMgr.TitleGraduated");
      GamePlayer playerById = WorldMgr.GetPlayerById(masterId);
      for (int key = oldLevel + 1; key <= player.PlayerCharacter.Grade; ++key)
      {
        if (dictionary1.ContainsKey(key))
        {
          SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(dictionary1[key]), 1, 103);
          fromTemplate.IsBinds = true;
          WorldEventMgr.SendItemToMail(fromTemplate, player.PlayerCharacter.ID, player.PlayerCharacter.NickName, player.ZoneId, (AreaConfigInfo) null, LanguageMgr.GetTranslation("Game.Server.AppSystem.GraduateBox.Success", (object) key));
        }
        if (dictionary2.ContainsKey(key))
        {
          SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(dictionary2[key]), 1, 103);
          fromTemplate.IsBinds = true;
          WorldEventMgr.SendItemToMail(fromTemplate, masterId, player.PlayerCharacter.MasterOrApprenticesArr[masterId], player.ZoneId, (AreaConfigInfo) null, LanguageMgr.GetTranslation("Game.Server.AppSystem.TakeAppBox.Success", (object) player.PlayerCharacter.NickName, (object) key));
        }
        using (PlayerBussiness pb = new PlayerBussiness())
        {
          if (playerById != null)
          {
            playerById.SendMailToUser(pb, LanguageMgr.GetTranslation("Game.Server.AppSystem.ApprenticeLevelUp.mailTitle", (object) player.PlayerCharacter.NickName, (object) key), LanguageMgr.GetTranslation("Game.Server.AppSystem.ApprenticeLevelUp.mailTitle", (object) player.PlayerCharacter.NickName, (object) key), eMailType.ItemOverdue);
          }
          else
          {
            MailInfo mail = new MailInfo()
            {
              Content = LanguageMgr.GetTranslation("Game.Server.AppSystem.ApprenticeLevelUp.mailTitle", (object) player.PlayerCharacter.NickName, (object) key),
              Title = LanguageMgr.GetTranslation("Game.Server.AppSystem.ApprenticeLevelUp.mailTitle", (object) player.PlayerCharacter.NickName, (object) key),
              Gold = 0,
              IsExist = true,
              Money = 0,
              GiftToken = 0,
              Receiver = player.PlayerCharacter.MasterOrApprenticesArr[masterId],
              ReceiverID = masterId,
              Sender = player.PlayerCharacter.MasterOrApprenticesArr[masterId],
              SenderID = masterId,
              Type = 9
            };
            mail.Annex1 = "";
            mail.Annex1Name = "";
            pb.SendMail(mail);
          }
        }
      }
      if (player.PlayerCharacter.Grade < AcademyMgr.ACADEMY_LEVEL_MIN)
        return;
      PlayerInfo playerInfo = (PlayerInfo) null;
      if (playerById != null)
      {
        playerById.Out.SendAcademyGradute(player, 1);
        playerInfo = playerById.PlayerCharacter;
      }
      else
      {
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
          playerInfo = playerBussiness.GetUserSingleByUserID(masterId);
      }
      string[] strArray1 = GameProperties.AcademyAppAwardComplete.Split(',')[player.PlayerCharacter.Sex ? 1 : 0].Split('|');
      List<SqlDataProvider.Data.ItemInfo> infos = new List<SqlDataProvider.Data.ItemInfo>();
      foreach (string s in strArray1)
      {
        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(int.Parse(s)), 1, 103);
        fromTemplate.ValidDate = 365;
        fromTemplate.IsBinds = true;
        infos.Add(fromTemplate);
      }
      WorldEventMgr.SendItemsToMails(infos, player.PlayerCharacter.ID, player.PlayerCharacter.NickName, player.ZoneId, (AreaConfigInfo) null, LanguageMgr.GetTranslation("Game.Server.AppSystem.GraduateBox.Success"));
      string[] strArray2 = GameProperties.AcademyMasAwardComplete.Split(',')[playerInfo.Sex ? 1 : 0].Split('|');
      infos.Clear();
      foreach (string s in strArray2)
      {
        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(int.Parse(s)), 1, 103);
        fromTemplate.ValidDate = 3;
        fromTemplate.IsBinds = true;
        infos.Add(fromTemplate);
      }
      WorldEventMgr.SendItemsToMail(infos, playerInfo.ID, playerInfo.NickName, LanguageMgr.GetTranslation("Game.Server.AppSystem.GraduateBoxForMaster.MailTitle", (object) player.PlayerCharacter.NickName), LanguageMgr.GetTranslation("Game.Server.AppSystem.GraduateBoxForMaster.MailContert"));
      AcademyMgr.FireMaster(player, true);
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
        playerBussiness.UpdateAcademyPlayer(player.PlayerCharacter);
      player.Out.SendAcademyAppState(player.PlayerCharacter, masterId);
      player.Out.SendAcademyGradute(player, 0);
      player.Rank.AddRank(translation1);
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        string translation2 = LanguageMgr.GetTranslation("Game.Server.AppSystem.MasterGainHonour.content", (object) playerInfo.NickName, (object) player.PlayerCharacter.NickName, (object) translation1);
        allPlayer.SendMessage(translation2);
      }
    }

    public static bool CheckCanApp(int levelApp)
    {
      if (levelApp >= AcademyMgr.TARGET_PLAYER_MIN_LEVEL)
        return levelApp <= AcademyMgr.ACADEMY_LEVEL_MAX;
      return false;
    }

    public static bool CheckCanMaster(int levelMaster)
    {
      return levelMaster >= AcademyMgr.ACADEMY_LEVEL_MIN;
    }
  }
}
