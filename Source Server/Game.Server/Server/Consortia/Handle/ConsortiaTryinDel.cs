// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaTryinDel
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(5)]
  public class ConsortiaTryinDel : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      bool val = false;
      string msg = "ConsortiaApplyAllyDeleteHandler.Failed";
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        if (consortiaBussiness.DeleteConsortiaApplyUsers(num, Player.PlayerCharacter.ID, Player.PlayerCharacter.ConsortiaID, ref msg))
        {
          msg = Player.PlayerCharacter.ID == 0 ? "ConsortiaApplyAllyDeleteHandler.Success" : "ConsortiaApplyAllyDeleteHandler.Success2";
          val = true;
        }
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 5);
      packet1.WriteInt(num);
      packet1.WriteBoolean(val);
      packet1.WriteString(LanguageMgr.GetTranslation(msg));
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
