// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.HotSpringRoomQuickEnterHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.HotSpringRooms;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
  [PacketHandler(190, "礼堂数据")]
  public class HotSpringRoomQuickEnterHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentHotSpringRoom == null)
      {
        HotSpringRoom randomRoom = HotSpringMgr.GetRandomRoom();
        if (randomRoom != null)
        {
          if (randomRoom.AddPlayer(client.Player))
            client.Out.SendEnterHotSpringRoom(client.Player);
        }
        else
          client.Player.SendMessage(LanguageMgr.GetTranslation("SpaRoomLoginHandler.Failed4"));
      }
      else
        client.Player.SendMessage(LanguageMgr.GetTranslation("SpaRoomLoginHandler.Failed"));
      return 0;
    }
  }
}
