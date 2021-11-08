// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.LookupEffortHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(203, "场景用户离开")]
  public class LookupEffortHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        AchievementData[] userAchievement = playerBussiness.GetUserAchievement(num);
        PlayerInfo userSingleByUserId = playerBussiness.GetUserSingleByUserID(num);
        GSPacketIn pkg = new GSPacketIn((short) 203, num);
        pkg.WriteInt(userSingleByUserId.AchievementPoint);
        pkg.WriteInt(userAchievement.Length);
        for (int index = 0; index < userAchievement.Length; ++index)
        {
          AchievementData achievementData = userAchievement[index];
          pkg.WriteInt(achievementData.AchievementID);
        }
        client.SendTCP(pkg);
      }
      return 0;
    }
  }
}
