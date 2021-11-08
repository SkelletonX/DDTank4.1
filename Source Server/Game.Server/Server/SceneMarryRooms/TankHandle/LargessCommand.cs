// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.TankHandle.LargessCommand
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets;
using SqlDataProvider.Data;

namespace Game.Server.SceneMarryRooms.TankHandle
{
  [MarryCommandAttbute(5)]
  public class LargessCommand : IMarryCommandHandler
  {
    public bool HandleCommand(
      TankMarryLogicProcessor process,
      GamePlayer player,
      GSPacketIn packet)
    {
      if (player.CurrentMarryRoom == null)
        return false;
      int num1 = packet.ReadInt();
      int num2 = GameProperties.LimitLevel(3);
      if (player.PlayerCharacter.Grade < num2)
      {
        player.Out.SendMessage(eMessageType.GM_NOTICE, string.Format("Você precisa ser nível {0} para continuar.", (object) num2));
        return false;
      }
      if (num1 <= 0 || !player.MoneyDirect(num1, false))
        return false;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        string translation1 = LanguageMgr.GetTranslation("LargessCommand.Content", (object) player.PlayerCharacter.NickName, (object) (num1 / 2));
        string translation2 = LanguageMgr.GetTranslation("LargessCommand.Title", (object) player.PlayerCharacter.NickName);
        MailInfo mail1 = new MailInfo()
        {
          Annex1 = "",
          Content = translation1,
          Gold = 0,
          IsExist = true,
          Money = num1 / 2,
          Receiver = player.CurrentMarryRoom.Info.BrideName,
          ReceiverID = player.CurrentMarryRoom.Info.BrideID,
          Sender = LanguageMgr.GetTranslation("LargessCommand.Sender"),
          SenderID = 0,
          Title = translation2,
          Type = 14
        };
        playerBussiness.SendMail(mail1);
        player.Out.SendMailResponse(mail1.ReceiverID, eMailRespose.Receiver);
        MailInfo mail2 = new MailInfo()
        {
          Annex1 = "",
          Content = translation1,
          Gold = 0,
          IsExist = true,
          Money = num1 / 2,
          Receiver = player.CurrentMarryRoom.Info.GroomName,
          ReceiverID = player.CurrentMarryRoom.Info.GroomID,
          Sender = LanguageMgr.GetTranslation("LargessCommand.Sender"),
          SenderID = 0,
          Title = translation2,
          Type = 14
        };
        playerBussiness.SendMail(mail2);
        player.Out.SendMailResponse(mail2.ReceiverID, eMailRespose.Receiver);
      }
      player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("LargessCommand.Succeed"));
      GSPacketIn packet1 = player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("LargessCommand.Notice", (object) player.PlayerCharacter.NickName, (object) num1));
      player.CurrentMarryRoom.SendToPlayerExceptSelf(packet1, player);
      return true;
    }
  }
}
