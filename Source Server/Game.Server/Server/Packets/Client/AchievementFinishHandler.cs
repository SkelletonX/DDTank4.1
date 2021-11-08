// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.AchievementFinishHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(230, "Send achievement finish")]
  public class AchievementFinishHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      AchievementInfo achievement = client.Player.AchievementInventory.FindAchievement(num);
      if (new PlayerBussiness().GetUserAchievementData(client.Player.PlayerCharacter.ID, num).Count == 0)
      {
        GSPacketIn pkg = new GSPacketIn((short) 230, client.Player.PlayerCharacter.ID);
        pkg.WriteInt(num);
        DateTime now = DateTime.Now;
        pkg.WriteInt(now.Year);
        pkg.WriteInt(now.Month);
        pkg.WriteInt(now.Day);
        client.Player.AchievementInventory.AddAchievementData(achievement);
        client.Player.AchievementInventory.SendReward(achievement);
        client.Player.OnAchievementQuest();
        client.Player.AchievementInventory.SaveToDatabase();
        client.SendTCP(pkg);
      }
      return 0;
    }
  }
}
