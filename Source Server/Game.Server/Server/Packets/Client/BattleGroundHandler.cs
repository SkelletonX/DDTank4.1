// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.BattleGroundHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(132, "场景用户离开")]
  public class BattleGroundHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      byte num = packet.ReadByte();
      int levelLimit = client.Player.BattleData.LevelLimit;
      GSPacketIn packet1 = new GSPacketIn((short) 132, client.Player.PlayerCharacter.ID);
      switch (num)
      {
        case 3:
          byte val = packet.ReadByte();
          packet1.WriteByte((byte) 3);
          packet1.WriteBoolean(true);
          packet1.WriteByte(val);
          switch (val)
          {
            case 1:
              if (client.Player.BattleData.MatchInfo == null)
              {
                packet1.WriteInt(0);
                packet1.WriteInt(0);
                packet1.WriteInt(client.Player.BattleData.fairBattleDayPrestige);
                break;
              }
              packet1.WriteInt(client.Player.BattleData.MatchInfo.addDayPrestge);
              packet1.WriteInt(client.Player.BattleData.MatchInfo.totalPrestige);
              packet1.WriteInt(client.Player.BattleData.fairBattleDayPrestige);
              break;
            case 2:
              packet1.WriteInt(client.Player.BattleData.GetRank());
              break;
          }
          client.Player.Out.SendTCP(packet1);
          break;
        case 5:
          packet1.WriteByte((byte) 5);
          packet1.WriteInt(client.Player.BattleData.Attack);
          packet1.WriteInt(client.Player.BattleData.Defend);
          packet1.WriteInt(client.Player.BattleData.Agility);
          packet1.WriteInt(client.Player.BattleData.Lucky);
          packet1.WriteInt(client.Player.BattleData.Damage);
          packet1.WriteInt(client.Player.BattleData.Guard);
          packet1.WriteInt(client.Player.BattleData.Blood);
          packet1.WriteInt(client.Player.BattleData.Energy);
          client.Player.Out.SendTCP(packet1);
          break;
      }
      return 0;
    }
  }
}
