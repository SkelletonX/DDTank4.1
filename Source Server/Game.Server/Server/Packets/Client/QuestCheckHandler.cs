// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.QuestCheckHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Quests;

namespace Game.Server.Packets.Client
{
  [PacketHandler(181, "客服端任务检查")]
  public class QuestCheckHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int id1 = packet.ReadInt();
      int id2 = packet.ReadInt();
      int num = packet.ReadInt();
      BaseQuest quest = client.Player.QuestInventory.FindQuest(id1);
      if (quest != null)
      {
        ClientModifyCondition conditionById = quest.GetConditionById(id2) as ClientModifyCondition;
        if (conditionById != null)
          conditionById.Value = num;
      }
      return 0;
    }
  }
}
