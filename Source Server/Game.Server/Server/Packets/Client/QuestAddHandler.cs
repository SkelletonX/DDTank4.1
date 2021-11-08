// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.QuestAddHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(176, "添加任务")]
  public class QuestAddHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      for (int index = 0; index < num; ++index)
      {
        QuestInfo singleQuest = QuestMgr.GetSingleQuest(packet.ReadInt());
        string msg;
        client.Player.QuestInventory.AddQuest(singleQuest, out msg);
      }
      return 0;
    }
  }
}
