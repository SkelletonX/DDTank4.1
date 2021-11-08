// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.HotSpringEnterConfirmHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
  [PacketHandler(212, "礼堂数据")]
  public class HotSpringEnterConfirmHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      if (client.Player.CurrentHotSpringRoom == null)
      {
        if (HotSpringMgr.GetHotSpringRoombyID(num) != null)
        {
          GSPacketIn packet1 = new GSPacketIn((short) 212);
          packet1.WriteInt(num);
          client.Out.SendTCP(packet1);
        }
        else
          client.Player.SendMessage(LanguageMgr.GetTranslation("SpaRoomLoginHandler.Failed4"));
      }
      return 0;
    }
  }
}
