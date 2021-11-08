// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UseReworkNameHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(171, "场景用户离开")]
  public class UseReworkNameHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      byte num = packet.ReadByte();
      int slot = packet.ReadInt();
      string newNickName = packet.ReadString();
      string msg = "";
      PlayerInventory inventory = client.Player.GetInventory((eBageType) num);
      ItemInfo itemAt = inventory.GetItemAt(slot);
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        if (playerBussiness.RenameNick(client.Player.PlayerCharacter.UserName, client.Player.PlayerCharacter.NickName, newNickName))
          inventory.RemoveCountFromStack(itemAt, 1);
        else
          msg = "Não foi possível renomear o apelido.";
      }
      if (msg != "")
        client.Player.SendMessage(msg);
      return 0;
    }
  }
}
