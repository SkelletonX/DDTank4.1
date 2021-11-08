// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.HotSpringRoomEnterHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.HotSpringRooms;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
  [PacketHandler(202, "礼堂数据")]
  public class HotSpringRoomEnterHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int id = packet.ReadInt();
      packet.ReadString();
      if (client.Player.CurrentHotSpringRoom == null)
      {
        HotSpringRoom hotSpringRoombyId = HotSpringMgr.GetHotSpringRoombyID(id);
        if (hotSpringRoombyId != null)
        {
          int num = 10000;
          if (client.Player.PlayerCharacter.Gold >= num)
          {
            if (hotSpringRoombyId.AddPlayer(client.Player) && client.Player.RemoveGold(num) > 0)
              client.Out.SendEnterHotSpringRoom(client.Player);
          }
          else
            client.Player.SendMessage(LanguageMgr.GetTranslation("SpaRoomLoginHandler.Failed1"));
        }
        else
          client.Player.SendMessage(LanguageMgr.GetTranslation("SpaRoomLoginHandler.Failed4"));
      }
      return 0;
    }
  }
}
