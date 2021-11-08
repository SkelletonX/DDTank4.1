// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserDeleteMailHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(112, "删除邮件")]
  public class UserDeleteMailHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GSPacketIn packet1 = new GSPacketIn((short) 112, client.Player.PlayerCharacter.ID);
      if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
        return 0;
      }
      int num = packet.ReadInt();
      packet1.WriteInt(num);
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        int senderID;
        if (playerBussiness.DeleteMail(client.Player.PlayerCharacter.ID, num, out senderID))
        {
          client.Out.SendMailResponse(senderID, eMailRespose.Receiver);
          packet1.WriteBoolean(true);
        }
        else
          packet1.WriteBoolean(false);
      }
      client.Out.SendTCP(packet1);
      return 0;
    }
  }
}
