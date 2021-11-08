// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaEquipControl
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(24)]
  public class ConsortiaEquipControl : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      bool val1 = false;
      string msg = "ConsortiaEquipControlHandler.Fail";
      ConsortiaEquipControlInfo info = new ConsortiaEquipControlInfo();
      info.ConsortiaID = Player.PlayerCharacter.ConsortiaID;
      List<int> intList = new List<int>();
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        for (int index = 0; index < 5; ++index)
        {
          info.Riches = packet.ReadInt();
          info.Type = 1;
          info.Level = index + 1;
          consortiaBussiness.AddAndUpdateConsortiaEuqipControl(info, Player.PlayerCharacter.ID, ref msg);
          intList.Add(info.Riches);
        }
        info.Riches = packet.ReadInt();
        info.Type = 2;
        info.Level = 0;
        intList.Add(info.Riches);
        consortiaBussiness.AddAndUpdateConsortiaEuqipControl(info, Player.PlayerCharacter.ID, ref msg);
        info.Riches = packet.ReadInt();
        info.Type = 3;
        info.Level = 0;
        intList.Add(info.Riches);
        consortiaBussiness.AddAndUpdateConsortiaEuqipControl(info, Player.PlayerCharacter.ID, ref msg);
        val1 = true;
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 24);
      packet1.WriteBoolean(val1);
      foreach (int val2 in intList)
        packet1.WriteInt(val2);
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
