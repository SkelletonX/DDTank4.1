// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.QuestFinishHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Quests;

namespace Game.Server.Packets.Client
{
  [PacketHandler(179, "任务完成")]
  public class QuestFinishHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      int selectedItem = packet.ReadInt();
      BaseQuest quest = client.Player.QuestInventory.FindQuest(num);
      bool flag = false;
      if (quest != null)
        flag = client.Player.QuestInventory.Finish(quest, selectedItem);
      if (flag)
      {
        GSPacketIn packet1 = new GSPacketIn((short) 179, client.Player.PlayerCharacter.ID);
        packet1.WriteInt(num);
        client.Out.SendTCP(packet1);
      }
      return 1;
    }
  }
}
