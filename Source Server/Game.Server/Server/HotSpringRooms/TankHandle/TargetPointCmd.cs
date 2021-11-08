// Decompiled with JetBrains decompiler
// Type: Game.Server.HotSpringRooms.TankHandle.TargetPointCmd
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.HotSpringRooms.TankHandle
{
  [HotSpringCommandAttbute(1)]
  public class TargetPointCmd : IHotSpringCommandHandler
  {
    public bool HandleCommand(
      TankHotSpringLogicProcessor process,
      GamePlayer player,
      GSPacketIn packet)
    {
      if (player.CurrentHotSpringRoom != null)
      {
        string str = packet.ReadString();
        int num1 = packet.ReadInt();
        int val1 = packet.ReadInt();
        int val2 = packet.ReadInt();
        packet.ReadInt();
        int num2 = packet.ReadInt();
        GamePlayer playerWithId = player.CurrentHotSpringRoom.GetPlayerWithID(num1);
        if (playerWithId != null)
        {
          playerWithId.Hot_X = val1;
          playerWithId.Hot_Y = val2;
          playerWithId.Hot_Direction = num2;
          GSPacketIn packet1 = new GSPacketIn((short) 191);
          packet1.WriteByte((byte) 1);
          packet1.WriteString(str);
          packet1.WriteInt(num1);
          packet1.WriteInt(val1);
          packet1.WriteInt(val2);
          player.CurrentHotSpringRoom.SendToPlayerExceptSelf(packet1, player);
        }
      }
      return false;
    }
  }
}
