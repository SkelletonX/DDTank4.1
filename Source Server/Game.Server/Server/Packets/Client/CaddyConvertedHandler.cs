// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.CaddyConvertedHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(215, "Envia mensagem para todos de sua associação")]
  public class CaddyConvertedHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      string str1 = packet.ReadString();
      string str2 = packet.ReadString();
      bool result = false;
      int riches = 1000;
      string translateId = "ConsortiaRichiUpdateHandler.Failed";
      ConsortiaInfo consortiaInfo = ConsortiaMgr.FindConsortiaInfo(client.Player.PlayerCharacter.ConsortiaID);
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        ConsortiaUserInfo[] memberByConsortia = playerBussiness.GetAllMemberByConsortia(client.Player.PlayerCharacter.ConsortiaID);
        MailInfo mail = new MailInfo();
        foreach (ConsortiaUserInfo consortiaUserInfo in memberByConsortia)
        {
          mail.SenderID = client.Player.PlayerCharacter.ID;
          mail.Sender = "Sua associação " + consortiaInfo.ConsortiaName;
          mail.ReceiverID = consortiaUserInfo.UserID;
          mail.Receiver = consortiaUserInfo.UserName;
          mail.Title = str1;
          mail.Content = str2;
          mail.Type = 59;
          if (consortiaUserInfo.UserID != client.Player.PlayerCharacter.ID && playerBussiness.SendMail(mail))
          {
            translateId = "ConsortiaRichiUpdateHandler.Success";
            result = true;
            if ((uint) consortiaUserInfo.State > 0U)
              client.Player.Out.SendMailResponse(consortiaUserInfo.UserID, eMailRespose.Receiver);
            client.Player.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Send);
          }
          if (!result)
          {
            translateId = "ConsortiaRichiUpdateHandler.Success";
            result = true;
            if ((uint) consortiaUserInfo.State > 0U)
              client.Player.Out.SendMailResponse(consortiaUserInfo.UserID, eMailRespose.Receiver);
            client.Player.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Send);
          }
        }
      }
      if (result)
      {
        using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
          consortiaBussiness.ConsortiaRichRemove(client.Player.PlayerCharacter.ConsortiaID, ref riches);
      }
      client.Out.SendConsortiaMail(result, client.Player.PlayerCharacter.ID);
      client.Player.SendMessage(LanguageMgr.GetTranslation(translateId));
      return 0;
    }
  }
}
