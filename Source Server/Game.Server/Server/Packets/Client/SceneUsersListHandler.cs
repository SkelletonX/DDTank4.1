// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.SceneUsersListHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
  [PacketHandler(69, "用户列表")]
  public class SceneUsersListHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GSPacketIn packet1 = packet.Clone();
      packet1.ClearContext();
      byte num1 = packet.ReadByte();
      byte num2 = packet.ReadByte();
      GamePlayer[] allPlayersNoGame = WorldMgr.GetAllPlayersNoGame();
      int length = allPlayersNoGame.Length;
      byte val = length > (int) num2 ? num2 : (byte) length;
      packet1.WriteByte(val);
      for (int index = (int) num1 * (int) num2; index < (int) num1 * (int) num2 + (int) val; ++index)
      {
        GamePlayer gamePlayer = allPlayersNoGame[index % length];
        packet1.WriteInt(gamePlayer.PlayerCharacter.ID);
        packet1.WriteString(gamePlayer.PlayerCharacter.NickName == null ? "" : gamePlayer.PlayerCharacter.NickName);
        packet1.WriteByte(gamePlayer.PlayerCharacter.typeVIP);
        packet1.WriteInt(gamePlayer.PlayerCharacter.VIPLevel);
        packet1.WriteBoolean(gamePlayer.PlayerCharacter.Sex);
        packet1.WriteInt(gamePlayer.PlayerCharacter.Grade);
        packet1.WriteInt(gamePlayer.PlayerCharacter.ConsortiaID);
        packet1.WriteString(gamePlayer.PlayerCharacter.ConsortiaName == null ? "" : gamePlayer.PlayerCharacter.ConsortiaName);
        packet1.WriteInt(gamePlayer.PlayerCharacter.Offer);
        packet1.WriteInt(gamePlayer.PlayerCharacter.Win);
        packet1.WriteInt(gamePlayer.PlayerCharacter.Total);
        packet1.WriteInt(gamePlayer.PlayerCharacter.Escape);
        packet1.WriteInt(gamePlayer.PlayerCharacter.Repute);
        packet1.WriteInt(gamePlayer.PlayerCharacter.FightPower);
      }
      client.Out.SendTCP(packet1);
      return 0;
    }
  }
}
