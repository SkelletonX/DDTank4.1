// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaDutyDelete
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(9)]
  public class ConsortiaDutyDelete : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      int num = packet.ReadInt();
      bool val = false;
      string msg = "ConsortiaDutyDeleteHandler.Failed";
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        if (consortiaBussiness.DeleteConsortiaDuty(num, Player.PlayerCharacter.ID, Player.PlayerCharacter.ConsortiaID, ref msg))
        {
          msg = "ConsortiaDutyDeleteHandler.Success";
          val = true;
        }
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 9);
      packet1.WriteInt(num);
      packet1.WriteBoolean(val);
      packet1.WriteString(LanguageMgr.GetTranslation(msg));
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
