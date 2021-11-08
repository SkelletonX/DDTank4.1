// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.QuestRemoveHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Quests;

namespace Game.Server.Packets.Client
{
  [PacketHandler(177, "删除任务")]
  public class QuestRemoveHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int id = packet.ReadInt();
      BaseQuest quest = client.Player.QuestInventory.FindQuest(id);
      if (quest != null)
        client.Player.QuestInventory.RemoveQuest(quest);
      return 0;
    }
  }
}
