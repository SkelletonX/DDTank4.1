// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.LittleGameHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(166, "场景用户离开")]
  public class LittleGameHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      byte num1 = packet.ReadByte();
      GSPacketIn pkg = new GSPacketIn((short) 166, client.Player.PlayerCharacter.ID);
      switch (num1)
      {
        case 2:
          pkg.WriteByte((byte) 2);
          pkg.WriteInt(1);
          pkg.WriteInt(1);
          pkg.WriteString("bogu1,bogu2,bogu3,bogu4,bogu5,bogu6,bogu7,bogu8");
          pkg.WriteString("2001");
          client.Player.SendTCP(pkg);
          break;
        case 3:
          int val1 = 1;
          pkg.WriteByte((byte) 3);
          pkg.WriteInt(val1);
          for (int index = 0; index < val1; ++index)
          {
            pkg.WriteInt(1);
            pkg.WriteInt(2039);
            pkg.WriteInt(123);
            pkg.WriteInt(1);
            pkg.WriteInt(client.Player.PlayerCharacter.ID);
            pkg.WriteInt(client.Player.PlayerCharacter.Grade);
            pkg.WriteInt(client.Player.PlayerCharacter.Repute);
            pkg.WriteString(client.Player.PlayerCharacter.NickName);
            pkg.WriteByte(client.Player.PlayerCharacter.typeVIP);
            pkg.WriteInt(client.Player.PlayerCharacter.VIPLevel);
            pkg.WriteBoolean(client.Player.PlayerCharacter.Sex);
            pkg.WriteString(client.Player.PlayerCharacter.Style);
            pkg.WriteString(client.Player.PlayerCharacter.Colors);
            pkg.WriteString(client.Player.PlayerCharacter.Skin);
            pkg.WriteInt(client.Player.PlayerCharacter.Hide);
            pkg.WriteInt(client.Player.PlayerCharacter.FightPower);
            pkg.WriteInt(client.Player.PlayerCharacter.Win);
            pkg.WriteInt(client.Player.PlayerCharacter.Total);
            pkg.WriteBoolean(false);
            int val2 = 1;
            int num2 = 0;
            pkg.WriteInt(val2);
            for (; num2 < val2; ++num2)
            {
              pkg.WriteString("livingInhale");
              pkg.WriteInt(1);
              pkg.WriteString("stand");
              pkg.WriteString("1");
              pkg.WriteInt(1);
              pkg.WriteInt(2039);
              pkg.WriteInt(123);
            }
          }
          client.Player.SendTCP(pkg);
          break;
      }
      return 0;
    }
  }
}
