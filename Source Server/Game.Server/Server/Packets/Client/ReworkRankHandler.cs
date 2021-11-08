// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ReworkRankHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(189, "场景用户离开")]
  public class ReworkRankHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      string honor = packet.ReadString();
      if (!string.IsNullOrEmpty(honor))
      {
        client.Player.UpdateHonor(honor);
        new PlayerBussiness().AddDailyRecord(new DailyRecordInfo()
        {
          UserID = client.Player.PlayerCharacter.ID,
          Type = 7,
          Value = honor
        });
      }
      return 0;
    }
  }
}
