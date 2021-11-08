using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(141, "防沉迷系统开关")]
  public class AcademyHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      switch (packet.ReadByte())
      {
        case 4:
          int num1 = packet.ReadInt();
          string str1 = packet.ReadString();
          if (AcademyMgr.GetRequest(client.Player.PlayerId, num1) == null)
          {
            if (client.Player.PlayerCharacter.freezesDate <= DateTime.Now)
            {
              GamePlayer playerById = WorldMgr.GetPlayerById(num1);
              if (playerById != null && playerById.PlayerCharacter.apprenticeshipState < AcademyMgr.MASTER_FULL_STATE && AcademyMgr.CheckCanMaster(playerById.PlayerCharacter.Grade))
              {
                AcademyMgr.AddRequest(new AcademyRequestInfo()
                {
                  SenderID = client.Player.PlayerId,
                  ReceiderID = num1,
                  Type = 1,
                  CreateTime = DateTime.Now
                });
                GSPacketIn pkg = new GSPacketIn((short) 141);
                pkg.WriteByte((byte) 4);
                pkg.WriteInt(client.Player.PlayerId);
                pkg.WriteString(client.Player.PlayerCharacter.NickName);
                pkg.WriteString(str1);
                playerById.SendTCP(pkg);
                break;
              }
              client.Player.SendMessage(LanguageMgr.GetTranslation("LoginServerConnector.HandleSysMess.Msg2"));
              break;
            }
            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.TakeApprentice.Frozen", (object) this.checkDate(client.Player.PlayerCharacter.freezesDate)));
            break;
          }
          break;
        case 5:
          int num2 = packet.ReadInt();
          string str2 = packet.ReadString();
          if (AcademyMgr.GetRequest(client.Player.PlayerId, num2) == null)
          {
            if (client.Player.PlayerCharacter.freezesDate <= DateTime.Now)
            {
              GamePlayer playerById = WorldMgr.GetPlayerById(num2);
              if (playerById != null && playerById.PlayerCharacter.masterID == 0 && AcademyMgr.CheckCanApp(playerById.PlayerCharacter.Grade))
              {
                AcademyMgr.AddRequest(new AcademyRequestInfo()
                {
                  SenderID = client.Player.PlayerId,
                  ReceiderID = num2,
                  Type = 0,
                  CreateTime = DateTime.Now
                });
                GSPacketIn pkg = new GSPacketIn((short) 141);
                pkg.WriteByte((byte) 5);
                pkg.WriteInt(client.Player.PlayerId);
                pkg.WriteString(client.Player.PlayerCharacter.NickName);
                pkg.WriteString(str2);
                playerById.SendTCP(pkg);
                break;
              }
              client.Player.SendMessage(LanguageMgr.GetTranslation("LoginServerConnector.HandleSysMess.Msg2"));
              break;
            }
            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BeApprentice.Frozen", (object) this.checkDate(client.Player.PlayerCharacter.freezesDate)));
            break;
          }
          break;
        case 6:
          int num3 = packet.ReadInt();
          AcademyRequestInfo request1 = AcademyMgr.GetRequest(num3, client.Player.PlayerId);
          if (request1 != null && request1.Type == 1)
          {
            AcademyMgr.RemoveRequest(request1);
            if (client.Player.PlayerCharacter.freezesDate <= DateTime.Now)
            {
              if (client.Player.PlayerCharacter.apprenticeshipState < AcademyMgr.MASTER_FULL_STATE && AcademyMgr.CheckCanMaster(client.Player.PlayerCharacter.Grade))
              {
                GamePlayer playerById = WorldMgr.GetPlayerById(num3);
                if (playerById != null && AcademyMgr.CheckCanApp(playerById.PlayerCharacter.Grade))
                {
                  if (AcademyMgr.AddApprentice(client.Player, playerById))
                  {
                    playerById.Out.SendAcademySystemNotice(LanguageMgr.GetTranslation("Game.Server.AppSystem.MasterConfirm", (object) client.Player.PlayerCharacter.NickName), true);
                    client.Player.SendMailToUser(new PlayerBussiness(), LanguageMgr.GetTranslation("Game.Server.AppSystem.TakeApprenticeMail.Content"), LanguageMgr.GetTranslation("Game.Server.AppSystem.TakeApprenticeMail.Title"), eMailType.ItemOverdue);
                    client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.ApprenticeConfirm", (object) playerById.PlayerCharacter.NickName));
                    break;
                  }
                  client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.TakeApprentice.Failed.AlreadyHasMaster"));
                  break;
                }
                client.Player.SendMessage(LanguageMgr.GetTranslation("LoginServerConnector.HandleSysMess.Msg2"));
                break;
              }
              client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.TakeApprentice.Failed"));
              break;
            }
            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.TakeApprentice.Frozen", (object) this.checkDate(client.Player.PlayerCharacter.freezesDate)));
            break;
          }
          client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.AppClub.RemoveInfo.RecordNotFound"));
          break;
        case 7:
          int num4 = packet.ReadInt();
          AcademyRequestInfo request2 = AcademyMgr.GetRequest(num4, client.Player.PlayerId);
          if (request2 != null && request2.Type == 0)
          {
            AcademyMgr.RemoveRequest(request2);
            if (client.Player.PlayerCharacter.freezesDate <= DateTime.Now)
            {
              if (client.Player.PlayerCharacter.masterID == 0 && AcademyMgr.CheckCanApp(client.Player.PlayerCharacter.Grade))
              {
                GamePlayer playerById = WorldMgr.GetPlayerById(num4);
                if (playerById != null && playerById.PlayerCharacter.Grade >= client.Player.PlayerCharacter.Grade + AcademyMgr.LEVEL_GAP && AcademyMgr.CheckCanMaster(playerById.PlayerCharacter.Grade))
                {
                  if (AcademyMgr.AddApprentice(playerById, client.Player))
                  {
                    playerById.Out.SendAcademySystemNotice(LanguageMgr.GetTranslation("Game.Server.AppSystem.ApprenticeConfirm", (object) client.Player.PlayerCharacter.NickName), true);
                    playerById.SendMailToUser(new PlayerBussiness(), LanguageMgr.GetTranslation("Game.Server.AppSystem.TakeApprenticeMail.Content"), LanguageMgr.GetTranslation("Game.Server.AppSystem.TakeApprenticeMail.Title"), eMailType.ItemOverdue);
                    client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.MasterConfirm", (object) playerById.PlayerCharacter.NickName));
                    break;
                  }
                  client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.AlreadlyHasRelationship.Apprentice"));
                  break;
                }
                client.Player.SendMessage(LanguageMgr.GetTranslation("LoginServerConnector.HandleSysMess.Msg2"));
                break;
              }
              client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BeApprentice.Failed"));
              break;
            }
            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BeApprentice.Frozen", (object) this.checkDate(client.Player.PlayerCharacter.freezesDate)));
            break;
          }
          client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.AppClub.RemoveInfo.RecordNotFound"));
          break;
        case 8:
          int num5 = packet.ReadInt();
          AcademyRequestInfo request3 = AcademyMgr.GetRequest(num5, client.Player.PlayerId);
          if (request3 != null && request3.Type == 1)
          {
            AcademyMgr.RemoveRequest(request3);
            GamePlayer playerById = WorldMgr.GetPlayerById(num5);
            if (playerById != null)
            {
              playerById.Out.SendAcademySystemNotice(LanguageMgr.GetTranslation("Game.Server.AppSystem.MasterRefuse", (object) client.Player.PlayerCharacter.NickName), false);
              break;
            }
            break;
          }
          break;
        case 9:
          int num6 = packet.ReadInt();
          AcademyRequestInfo request4 = AcademyMgr.GetRequest(num6, client.Player.PlayerId);
          if (request4 != null && request4.Type == 0)
          {
            AcademyMgr.RemoveRequest(request4);
            GamePlayer playerById = WorldMgr.GetPlayerById(num6);
            if (playerById != null)
            {
              playerById.Out.SendAcademySystemNotice(LanguageMgr.GetTranslation("Game.Server.AppSystem.ApprenticeRefuse", (object) client.Player.PlayerCharacter.NickName), false);
              break;
            }
            break;
          }
          break;
        case 12:
          int removeUserId = packet.ReadInt();
          if (client.Player.RemoveGold(10000) > 0)
          {
            if (client.Player.PlayerCharacter.masterID == removeUserId && AcademyMgr.FireMaster(client.Player, false))
            {
              client.Player.PlayerCharacter.freezesDate = DateTime.Now.AddHours((double) GameProperties.AcademyApprenticeFreezeHours);
              using (PlayerBussiness playerBussiness = new PlayerBussiness())
                playerBussiness.UpdateAcademyPlayer(client.Player.PlayerCharacter);
              client.Player.Out.SendAcademyAppState(client.Player.PlayerCharacter, removeUserId);
              break;
            }
            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BreakApprentice.FireApprenticeCD"));
            break;
          }
          client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BreakApprentice.NotEnoughGold"));
          break;
        case 13:
          int num7 = packet.ReadInt();
          if (client.Player.RemoveGold(20000) > 0)
          {
            if (client.Player.PlayerCharacter.apprenticeshipState >= AcademyMgr.MASTER_STATE && AcademyMgr.FireApprentice(client.Player, num7, false))
            {
              client.Player.PlayerCharacter.freezesDate = DateTime.Now.AddHours((double) GameProperties.AcademyMasterFreezeHours);
              using (PlayerBussiness playerBussiness = new PlayerBussiness())
                playerBussiness.UpdateAcademyPlayer(client.Player.PlayerCharacter);
              client.Player.Out.SendAcademyAppState(client.Player.PlayerCharacter, num7);
              break;
            }
            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BreakApprentice.FireApprenticeCD"));
            break;
          }
          client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.AppSystem.BreakApprentice.NotEnoughGold"));
          break;
      }
      return 0;
    }

    private int checkDate(DateTime dateTime)
    {
      if (dateTime > DateTime.Now)
        return (int) Math.Ceiling((dateTime - DateTime.Now).TotalHours);
      return 0;
    }
  }
}
