// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.RequestBadLuckHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.Packets.Client
{
  [PacketHandler(45, "打开物品")]
  public class RequestBadLuckHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GSPacketIn pkg = new GSPacketIn((short) 45);
      pkg.WriteString(WorldMgr.LastTimeUpdateCaddyRank.ToString());
      List<UsersExtraInfo> list = WorldMgr.CaddyRank.Values.ToList<UsersExtraInfo>();
      pkg.WriteInt(list.Count);
      int val = 1;
      foreach (UsersExtraInfo usersExtraInfo in list)
      {
        pkg.WriteInt(val);
        pkg.WriteInt(usersExtraInfo.UserID);
        pkg.WriteInt(usersExtraInfo.TotalCaddyOpen);
        pkg.WriteInt(0);
        pkg.WriteString(usersExtraInfo.NickName);
        ++val;
      }
      client.Player.SendTCP(pkg);
      return 0;
    }
  }
}
