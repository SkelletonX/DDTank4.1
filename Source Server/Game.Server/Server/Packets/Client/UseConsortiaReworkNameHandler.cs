// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UseConsortiaReworkNameHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(188, "场景用户离开")]
  public class UseConsortiaReworkNameHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num1 = packet.ReadInt();
      byte num2 = packet.ReadByte();
      int slot = packet.ReadInt();
      string newNickName = packet.ReadString();
      string msg = "";
      if ((uint) client.Player.PlayerCharacter.ConsortiaID > 0U)
      {
        PlayerInventory inventory = client.Player.GetInventory((eBageType) num2);
        ItemInfo itemAt = inventory.GetItemAt(slot);
        if (itemAt.Count < 1)
        {
          client.Player.SendMessage("Não é possível renomear o nome da sociedade.");
          return 0;
        }
        using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
        {
          ConsortiaInfo consortiaSingle = consortiaBussiness.GetConsortiaSingle(num1);
          if (consortiaSingle == null)
          {
            client.Player.SendMessage("A sociedade não foi encontrada.");
            return 0;
          }
          if (client.Player.PlayerCharacter.ID != consortiaSingle.ChairmanID)
          {
            client.Player.SendMessage("Você não é o proprietário dessa sociedade.");
            return 0;
          }
          if (consortiaBussiness.RenameConsortia(num1, client.Player.PlayerCharacter.NickName, newNickName))
            inventory.RemoveCountFromStack(itemAt, 1);
          else
            msg = "Não foi possível renomear o nome da sociedade.";
        }
        if (msg != "")
          client.Player.SendMessage(msg);
      }
      return 0;
    }
  }
}
