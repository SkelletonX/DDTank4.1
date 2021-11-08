// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.QuestOneKeyFinishHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
  [PacketHandler(86, "任务完成")]
  public class QuestOneKeyFinishHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      BaseRoom currentRoom = client.Player.CurrentRoom;
      client.Player.CurrentRoom.GetPlayers();
      if (currentRoom != null && currentRoom.Host == client.Player)
      {
        if (client.Player.MainWeapon == null)
        {
          client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.SceneGames.NoEquip"));
          return 0;
        }
        if (currentRoom.RoomType == eRoomType.Dungeon && !client.Player.IsPvePermission(currentRoom.MapId, currentRoom.HardLevel))
        {
          client.Player.SendMessage("A operação falhou!");
          return 0;
        }
        RoomMgr.StartGame(client.Player.CurrentRoom);
        client.Player.CurrentRoom.IsPlaying = true;
      }
      return 0;
    }
  }
}
